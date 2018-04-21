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
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    // TODO: Localization.
    public static class DialogUtil
    {
        public static bool IsPeopleCanceling(string text)
        {
            var values = new[]
            {
                "Sai ir", "Deixa", "Deixa pra lá", "Vou desligar", "Desliga", "Cancelar", "Abortar", "Deixa", "Chega",
                "Sair", "Fechar", "Nada"
            };
            return values.Any(t => String.Equals(t, text, StringComparison.CurrentCultureIgnoreCase));
        }

        public static DateTime ParseDate(string date, DateTime relativeTo)
        {
            var mesMinus1 = new[]
                {"Mas passado", "Mas passar", "Mês passado", "Último mês", "Nesse passado", "Eles passaram"};
            if (mesMinus1.Any(t => String.Equals(t, date, StringComparison.CurrentCultureIgnoreCase)))
                return relativeTo.AddMonths(-1);

            var mesMinus2 = new[] {"Me atrasado", "Mês retrasado", "Antes do mês passado"};
            if (mesMinus2.Any(t => String.Equals(t, date, StringComparison.CurrentCultureIgnoreCase)))
                return relativeTo.AddMonths(-2);

            // 11 do 5 as 14 horas
            const string pattern = @"(\d+)";
            var cadeia = Regex.Split(date, pattern);

            var compData = String.Empty;
            var compTempo = String.Empty;
            var contador = 0;
            foreach (var item in cadeia)
            {
                Console.WriteLine(item);
                switch (item)
                {
                    case "1":
                        if (contador > 2)
                            compTempo += "1:";
                        else compData += "1/";

                        contador++;
                        break;
                    case "2":
                        if (contador > 2)
                            compTempo += "2:";
                        else
                            compData += "2/";
                        contador++;
                        break;
                    case "3":
                        if (contador > 2)
                            compTempo += "3:";
                        else
                            compData += "3/";
                        contador++;
                        break;
                }
            }
            if (compData.EndsWith("/"))
                compData = compData.Substring(1, compData.Length - 1);
            compData += "/2017";

            // TODO: if (compTempo.EndsWith(":"))
            //    compTempo = compTempo.Substring(1, compTempo.Length - 1);

            try
            {
                relativeTo = DateTime.Parse(compData);
                Console.Write(compTempo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return relativeTo;
        }

        public static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage,
            CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage> { cardImage },
                Buttons = new List<CardAction> { cardAction }
            };

            return heroCard.ToAttachment();
        }
    }
}