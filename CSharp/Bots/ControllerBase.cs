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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    public class ControllerBase : ApiController
    {
        protected readonly IDialog<object> _rootDialog;
        protected Activity CurrentActivity;
        protected bool SkipCancelingProcessing;
        protected bool SendRootOnStart = true;

        public ControllerBase(IDialog<object> rootDialog)
        {
            _rootDialog = rootDialog;
        }

        public async Task<TT> GetProperty<TT>(string name)
        {
            var stateClient = CurrentActivity.GetStateClient();
            var userData = await stateClient.BotState.GetUserDataAsync(CurrentActivity.ChannelId, CurrentActivity.From.Id);

            return userData.GetProperty<TT>(name);
        }

        public async void SetProperty<TT>(string name, TT data)
        {
            var stateClient = CurrentActivity.GetStateClient();
            var userData = await stateClient.BotState.GetUserDataAsync(CurrentActivity.ChannelId, CurrentActivity.From.Id);
            userData.SetProperty<TT>(name, data);
            await stateClient.BotState.SetUserDataAsync(CurrentActivity.ChannelId, CurrentActivity.From.Id, userData);
        }

        [Serializable]
        protected class ConversationStarter
        {
            public string ToId { get; set; }
            public string ToName { get; set; }
            public string FromId { get; set; }
            public string ConversationId { get; set; }
            public string ServiceUrl { get; set; }
            public string FromName { get; set; }
            public string ChannelId { get; set; }
        }

        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> PostAsync([FromBody] Activity activity)
        {

            try
            {

                CurrentActivity = activity;

                // TODO: Parametrizar var service = new BingSpellCheckService();

                if (activity.Type == ActivityTypes.Message)
                {

                    using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                    {
                        var stateClient = activity.GetStateClient();
                        var userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                        var convRef = scope.Resolve<ConversationReference>();
                        userData.SetProperty("conversationReference", convRef);
                    }

                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    var isTypingReply = activity.CreateReply();
                    isTypingReply.Type = ActivityTypes.Typing;
                    await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                    OnActivityMessage(activity);
                    await Conversation.SendAsync(activity, () => _rootDialog);
                }
                else
                {
                    await HandleSystemMessage(activity);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message + e.StackTrace);

                var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
                telemetry.TrackException(e);
                OnException(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected virtual void OnException(Exception e)
        {

        }

        protected virtual Task OnCancelation(Activity activity, IConnectorClient original)
        {
            throw new System.NotImplementedException();
        }

        protected virtual async void OnActivityEvent(Activity activity, string eventName)
        {
        }

        protected virtual async void OnActivityMessage(Activity activity)
        {

        }


        private async Task HandleSystemMessage(Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.Event:
                    OnActivityEvent(activity, activity.AsEventActivity().Name);
                    break;
                case ActivityTypes.DeleteUserData:
                    // Implement user deletion here
                    // If we handle user deletion, return a real activity
                    break;
                case ActivityTypes.ConversationUpdate:
                    //if (sendRootOnStart)
                    //{
                    //    if (activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id))
                    //    {
                    //        await Conversation.SendAsync(activity, () => _rootDialog);
                    //    }
                    //}
                    break;
                case ActivityTypes.ContactRelationUpdate:
                    if (activity.Type == ActivityTypes.ContactRelationUpdate)
                    {
                        if (activity.AsContactRelationUpdateActivity().Action == ContactRelationUpdateActionTypes.Add)
                        {
                            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            var response = activity.CreateReply();
                            response.Text = "Obrigado por me adicionar na sua lista de contatos.";
                            await connector.Conversations.ReplyToActivityAsync(response);
                        }
                    }
                    break;
                case ActivityTypes.Typing:
                    // Handle knowing tha the user is typing
                    break;
                case ActivityTypes.Ping:
                    break;
            }
        }
    }
}