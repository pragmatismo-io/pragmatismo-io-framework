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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Calling;
using Microsoft.Bot.Builder.Calling.Events;
using Microsoft.Bot.Builder.Calling.ObjectModel.Contracts;
using Microsoft.Bot.Builder.Calling.ObjectModel.Misc;
using Microsoft.Bot.Builder.Dialogs;

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    public class CallingBot : ICallingBot
    {
        private readonly List<string> _response = new List<string>();
        private readonly IDialog<object> _rootDialog;

        private int _silenceTimes;

        private bool _sttFailed;

        public CallingBot(ICallingBotService callingBotService, IDialog<object> rootDialog)
        {
            _rootDialog = rootDialog;
            CallingBotService = callingBotService ?? throw new ArgumentNullException(nameof(callingBotService));

            CallingBotService.OnIncomingCallReceived += OnIncomingCallReceived;
            CallingBotService.OnPlayPromptCompleted += OnPlayPromptCompleted;
            CallingBotService.OnRecordCompleted += OnRecordCompleted;
            CallingBotService.OnHangupCompleted += OnHangupCompleted;
        }

        public ICallingBotService CallingBotService { get; }

        private Task OnIncomingCallReceived(IncomingCallEvent incomingCallEvent)
        {
            var id = Guid.NewGuid().ToString();
            incomingCallEvent.ResultingWorkflow.Actions = new List<ActionBase>
            {
                new Answer {OperationId = id},
                GetRecordForText("Oi, como posso ajudar?")
            };

            return Task.FromResult(true);
        }

        private ActionBase GetRecordForText(string promptText)
        {
            var prompt = string.IsNullOrEmpty(promptText) ? null : GetPromptForText(promptText);
            var id = Guid.NewGuid().ToString();
            return new Record
            {
                OperationId = id,
                PlayPrompt = prompt,
                MaxDurationInSeconds = 10,
                InitialSilenceTimeoutInSeconds = 2,
                MaxSilenceTimeoutInSeconds = 1,
                PlayBeep = false,
                RecordingFormat = RecordingFormat.Wav,
                StopTones = new List<char> {'#'}
            };
        }

        private Task OnPlayPromptCompleted(PlayPromptOutcomeEvent playPromptOutcomeEvent)
        {
            if (_response.Count > 0)
            {
                _silenceTimes = 0;
                var actionList = new List<ActionBase>();
                foreach (var res in _response)
                    Debug.WriteLine($"Resposta -- {res}");
                actionList.Add(GetPromptForText(_response));
                actionList.Add(GetRecordForText(string.Empty));
                playPromptOutcomeEvent.ResultingWorkflow.Actions = actionList;
                _response.Clear();
            }
            else
            {
                if (_sttFailed)
                {
                    playPromptOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                    {
                        GetRecordForText("Não entendi, poderia repetir?")
                    };
                    _sttFailed = false;
                    _silenceTimes = 0;
                }
                else if (_silenceTimes > 2)
                {
                    playPromptOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                    {
                        GetPromptForText("Ocorreu um problema no sistema. Por favor, ligue novamente."),
                        new Hangup {OperationId = Guid.NewGuid().ToString()}
                    };
                    playPromptOutcomeEvent.ResultingWorkflow.Links = null;
                    _silenceTimes = 0;
                }
                else
                {
                    _silenceTimes++;
                    playPromptOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                    {
                        GetSilencePrompt(2000)
                    };
                }
            }
            return Task.CompletedTask;
        }

        private async Task OnRecordCompleted(RecordOutcomeEvent recordOutcomeEvent)
        {
            if (recordOutcomeEvent.RecordOutcome.Outcome == Outcome.Success)
            {
                var record = await recordOutcomeEvent.RecordedContent;
                var bs = new BotBingSpeech(recordOutcomeEvent.ConversationResult, t => _response.Add(t),
                    s => _sttFailed = s, () => _rootDialog);
                bs.CreateDataRecoClient();
                bs.SendAudioHelper(record);

                recordOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                {
                    GetSilencePrompt()
                };
            }
            else
            {
                if (_silenceTimes > 1)
                {
                    recordOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                    {
                        GetPromptForText("Obrigado por sua ligação."),
                        new Hangup {OperationId = Guid.NewGuid().ToString()}
                    };
                    recordOutcomeEvent.ResultingWorkflow.Links = null;
                    _silenceTimes = 0;
                }
                else
                {
                    _silenceTimes++;
                    recordOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                    {
                        GetRecordForText("Não entendi, poderia repetir?")
                    };
                }
            }
        }

        private Task OnHangupCompleted(HangupOutcomeEvent hangupOutcomeEvent)
        {
            hangupOutcomeEvent.ResultingWorkflow = null;
            return Task.FromResult(true);
        }

        private static PlayPrompt GetPromptForText(string text)
        {
            var prompt = new Prompt {Value = text, Voice = VoiceGender.Female};
            return new PlayPrompt {OperationId = Guid.NewGuid().ToString(), Prompts = new List<Prompt> {prompt}};
        }

        private static PlayPrompt GetPromptForText(List<string> text)
        {
            var prompts = new List<Prompt>();
            foreach (var txt in text)
                if (!string.IsNullOrEmpty(txt))
                    prompts.Add(new Prompt {Value = txt, Voice = VoiceGender.Female});
            if (prompts.Count == 0)
                return GetSilencePrompt(1000);
            return new PlayPrompt {OperationId = Guid.NewGuid().ToString(), Prompts = prompts};
        }

        private static PlayPrompt GetSilencePrompt(uint silenceLengthInMilliseconds = 3000)
        {
            var prompt = new Prompt
            {
                Value = string.Empty,
                Voice = VoiceGender.Female,
                SilenceLengthInMilliseconds = silenceLengthInMilliseconds
            };
            return new PlayPrompt {OperationId = Guid.NewGuid().ToString(), Prompts = new List<Prompt> {prompt}};
        }
    }
}