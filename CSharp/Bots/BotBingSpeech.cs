/*****************************************************************************\
*
* Copyright (c) Pragmatismo.io. All rights reserved.
* Licensed under the MIT license.
*
* Pragmatismo.io: http://pragmatismo.io
*
* MIT License:
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
* 
\*****************************************************************************/

#region

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Calling.ObjectModel.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Microsoft.CognitiveServices.SpeechRecognition;
using Conversation = Microsoft.Bot.Builder.Dialogs.Conversation;

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    public class BotBingSpeech
    {
        private readonly Action<string> _callback;
        private readonly ConversationResult _conversationResult;
        private readonly Func<IDialog<object>> _dialog;
        private readonly Action<bool> _failedCallback;
        private DataRecognitionClient _dataClient;

        public BotBingSpeech(ConversationResult conversationResult, Action<string> callback, Action<bool> failedCallback,
            Func<IDialog<object>> dialog)
        {
            _conversationResult = conversationResult;
            _callback = callback;
            _failedCallback = failedCallback;
            _dialog = dialog;
        }

        public void CreateDataRecoClient()
        {
            _dataClient = SpeechRecognitionServiceFactory.CreateDataClient(
                SpeechRecognitionMode.ShortPhrase,
                DefaultLocale,
                SubscriptionKey);

            _dataClient.OnResponseReceived += OnDataShortPhraseResponseReceivedHandler;
        }

        public string SubscriptionKey { get; set; }

        public string DefaultLocale { get; set; }

        public void SendAudioHelper(Stream recordedStream)
        {
            // Note for wave files, we can just send data from the file right to the server.
            // In the case you are not an audio file in wave format, and instead you have just
            // raw data (for example audio coming over bluetooth), then before sending up any 
            // audio data, you must first send up an SpeechAudioFormat descriptor to describe 
            // the layout and format of your raw audio data via DataRecognitionClient's sendAudioFormat() method.
            var buffer = new byte[1024];
            try
            {
                int bytesRead;
                do
                {
                    // Get more Audio data to send into byte buffer.
                    bytesRead = recordedStream.Read(buffer, 0, buffer.Length);

                    // Send of audio data to service. 
                    _dataClient.SendAudio(buffer, bytesRead);
                } while (bytesRead > 0);
            }
            catch (Exception ex)
            {
                WriteLine("Exception ------------ " + ex.Message);
            }
            finally
            {
                // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                _dataClient.EndAudio();
            }
        }

        private async void OnDataShortPhraseResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            WriteLine("--- OnDataShortPhraseResponseReceivedHandler ---");
            WriteResponseResult(e);

            // we got the final result, so it we can end the mic reco.  No need to do this
            // for dataReco, since we already called endAudio() on it as soon as we were done
            // sending all the data.

            // Send to bot
            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.RecognitionSuccess)
                await SendToBot(e.PhraseResponse.Results
                    .OrderBy(k => k.Confidence)
                    .FirstOrDefault(), _dialog);
            else
                _failedCallback(true);
        }

        private async Task SendToBot(RecognizedPhrase recognizedPhrase, Func<IDialog<object>> dialog)
        {
            var activity = new Activity
            {
                From = new ChannelAccount {Id = _conversationResult.Id},
                Conversation = new ConversationAccount {Id = _conversationResult.Id},
                Recipient = new ChannelAccount {Id = "Bot"},
                ServiceUrl = "https://skype.botframework.com",
                ChannelId = "skype",
                Locale = "pt-Br",
                Text = recognizedPhrase.DisplayText
            };

            using (var scope = Conversation
                .Container.BeginLifetimeScope(DialogModule.LifetimeScopeTag, Configure))
            {
                scope.Resolve<IMessageActivity>(TypedParameter.From((IMessageActivity) activity));
                DialogModule_MakeRoot.Register(scope, dialog);
                var postToBot = scope.Resolve<IPostToBot>();
                await postToBot.PostAsync(activity, CancellationToken.None);
            }
        }

        private void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new BotToUserSpeech(c.Resolve<IMessageActivity>(), _callback))
                .As<IBotToUser>()
                .InstancePerLifetimeScope();
        }

        private void WriteResponseResult(SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length == 0)
            {
                WriteLine("No phrase response is available.");
            }
            else
            {
                WriteLine("********* Final n-BEST Results *********");
                for (var i = 0; i < e.PhraseResponse.Results.Length; i++)
                    WriteLine(
                        "[{0}] Confidence={1}, Text=\"{2}\"",
                        i,
                        e.PhraseResponse.Results[i].Confidence,
                        e.PhraseResponse.Results[i].DisplayText);

                WriteLine(string.Empty);
            }
        }

        private void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);
            System.Diagnostics.Trace.WriteLine(formattedStr);
        }
    }
}