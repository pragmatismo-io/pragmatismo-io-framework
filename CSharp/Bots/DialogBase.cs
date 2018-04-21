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
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Pragmatismo.Io.Framework.Services;

#endregion

// TODO: Localization
namespace Pragmatismo.Io.Framework.Bots
{
    [Serializable]
    public class DialogBase<T> : LuisDialog<T>
    {
        [NonSerialized] private readonly IServiceBase _service;

        protected DialogBase(IServiceBase service)
        {
            _service = service;
        }


        [LuisIntent("pragmatismo-io-framework-Hello")]
        public async Task OnHello(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá. O que deseja fazer?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("pragmatismo-io-framework-GoodBye")]
        public async Task OnGoodBye(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await context.PostAsync("Obrigado por ligar, volte sempre!");
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe-me, não entendi o que disse. Poderia dizer novamente?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("pragmatismo-io-framework-OnAddSampleData")]
        public async Task OnAddSampleData(IDialogContext context, LuisResult result)
        {
            _service.OnAddSampleData();
            await context.PostAsync("Dados de exemplo adicionados.");
            context.Wait(MessageReceived);
        }
    }
}