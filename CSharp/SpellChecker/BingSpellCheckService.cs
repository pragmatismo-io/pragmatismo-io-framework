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

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace Pragmatismo.Io.Framework.SpellChecker
{
    public class BingSpellCheckService
    {
        private const string SpellCheckApiUrl = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?form=BCSSCK";

        /// <summary>
        ///     Microsoft Bing Spell Check Api Key.
        /// </summary>
        private static readonly string ApiKey = WebConfigurationManager.AppSettings["BingSpellCheckApiKey"];


        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            errorArgs.ErrorContext.Handled = true;
        }

        /// <summary>
        ///     Gets the correct spelling for the given text
        /// </summary>
        /// <param name="text">The text to be corrected</param>
        /// <returns>string with corrected text</returns>
        public async Task<string> GetCorrectedTextAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);

                var values = new Dictionary<string, string>
                {
                    {"text", text}
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(SpellCheckApiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var spellCheckResponse = JsonConvert.DeserializeObject<BingSpellCheckResponse>(responseString,
                    new JsonSerializerSettings
                    {
                        Error = HandleDeserializationError
                    });

                var sb = new StringBuilder();
                var previousOffset = 0;

                foreach (var flaggedToken in spellCheckResponse.FlaggedTokens)
                {
                    // Append the text from the previous offset to the current misspelled word offset
                    sb.Append(text.Substring(previousOffset, flaggedToken.Offset - previousOffset));

                    // Append the corrected word instead of the misspelled word
                    sb.Append(flaggedToken.Suggestions.First().SuggestionValue);

                    // Increment the offset by the length of the misspelled word
                    previousOffset = flaggedToken.Offset + flaggedToken.Token.Length;
                }

                // Append the text after the last misspelled word.
                if (previousOffset < text.Length)
                    sb.Append(text.Substring(previousOffset));

                return sb.ToString();
            }
        }
    }
}