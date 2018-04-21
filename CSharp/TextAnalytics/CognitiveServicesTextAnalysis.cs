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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Pragmatismo.Io.Framework.TextAnalytics
{
    /// <summary>
    ///     Implementation of https://www.microsoft.com/cognitive-services/en-us/text-analytics/documentation
    ///     by https://raw.githubusercontent.com/ealsur/CognitiveServicesText
    /// </summary>
    public class CognitiveServicesTextAnalysis
    {
        /// <summary>
        ///     Cognitive Text service endpoint
        /// </summary>
        private const string ServiceEndpoint = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";

        private readonly HttpClient _httpClient;

        public CognitiveServicesTextAnalysis(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
        }

        /// <summary>
        ///     Key phrase analysis
        /// </summary>
        /// <param name="language"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<List<string>> KeyPhrases(string language, string text)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
                throw new ArgumentNullException();
            var request = new TextRequest();
            request.Documents.Add(new TextDocument(text, language));
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{ServiceEndpoint}keyPhrases", content);
            var response = JObject.Parse(await result.Content.ReadAsStringAsync());
            CatchAndThrow(response);
            return response["documents"].Children().First().Value<JArray>("keyPhrases").ToObject<List<string>>();
        }

        /// <summary>
        ///     Sentiment analysis
        /// </summary>
        /// <param name="language"></param>
        /// <param name="text"></param>
        /// <returns>From 0 to 1 (1 being totally positive sentiment)</returns>
        public async Task<double> Sentiment(string language, string text)
        {
            if (string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
                throw new ArgumentNullException();
            var request = new TextRequest();
            request.Documents.Add(new TextDocument(text, language));
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{ServiceEndpoint}sentiment", content);
            var response = JObject.Parse(await result.Content.ReadAsStringAsync());
            CatchAndThrow(response);
            return response["documents"].Children().First().Value<double>("score");
        }

        /// <summary>
        ///     Generic catch and throw that detects errors on the response body
        /// </summary>
        /// <param name="response"></param>
        private void CatchAndThrow(JObject response)
        {
            if (response["errors"] != null && response["errors"].Children().Any())
                throw new Exception(response["errors"].Children().First().Value<string>("message"));
            if (response["code"] != null && response["message"] != null)
                throw new Exception(response["message"].Value<string>());
        }

        #region Requests

        private class TextRequest
        {
            public TextRequest()
            {
                Documents = new List<TextDocument>();
            }

            [JsonProperty("documents")]
            public List<TextDocument> Documents { get; }
        }

        private class TextDocument
        {
            public TextDocument(string text, string language)
            {
                Id = Guid.NewGuid().ToString();
                Language = language;
                Text = text;
            }

            [JsonProperty("language")]
            public string Language { get; private set; }

            [JsonProperty("id")]
            public string Id { get; private set; }

            [JsonProperty("text")]
            public string Text { get; private set; }
        }

        #endregion
    }
}