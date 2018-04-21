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

using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    public class ResumeSkype
    {
#pragma warning disable 618
        public static async Task Resume(ResumptionCookie resumptionCookie)
#pragma warning restore 618
        {
            var message = resumptionCookie.GetMessage();
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
            {
                var sc = scope.Resolve<IStateClient>();
                var userData = sc.BotState.GetUserData(message.ChannelId, message.From.Id);

                //Tell Skype to continue the conversation we registered before
#pragma warning disable CS0618 // Type or member is obsolete
                await Conversation.ResumeAsync(resumptionCookie, message);
#pragma warning restore CS0618 // Type or member is obsolete

                var waitingForSkype = true;

                while (waitingForSkype)
                {
                    //Keep checking if Skype is done with the questions on that channel
                    userData = sc.BotState.GetUserData(message.ChannelId, message.From.Id);
                    waitingForSkype = userData.GetProperty<bool>("waitingForSkype");
                }
            }
        }
    }
}