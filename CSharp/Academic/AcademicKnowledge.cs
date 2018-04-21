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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Pragmatismo.Io.Framework.Academic
{
    public class AcademicKnowledgeResultRecommendation
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Url { get; set; }
    }

    public class AcademicKnowledgeResult : IEnumerable
    {
        public List<AcademicKnowledgeResultRecommendation> recommendations;

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class AcademicKnowledge
    {
        public AcademicKnowledgeResult GetArticle(string query, string key)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["attributes"] = "Ti,D,W,E";
            queryString["count"] = "3";
            queryString["expr"] = $"Composite(AA.AuN=='{query}')";

            var requestUrl = "https://westus.api.cognitive.microsoft.com/academic/v1.0/evaluate?" + queryString;

            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);

            webRequest.Headers["Ocp-Apim-Subscription-Key"] = key;

            var responseStr = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
            var jResponse = (JObject) JsonConvert.DeserializeObject(responseStr);
            var entities = jResponse["entities"];
            var item = new AcademicKnowledgeResult
            {
                recommendations = new List<AcademicKnowledgeResultRecommendation>()
            };

            foreach (var entity in entities)
            {
                var e = entity["E"].Value<string>();
                var detail = (JObject) JsonConvert.DeserializeObject(e);
                var title = detail["DN"].Value<string>();
                var url = detail["S"][0]["U"].Value<string>();

                item.recommendations.Add(new AcademicKnowledgeResultRecommendation
                {
                    Abstract = string.Empty,
                    Title = title,
                    Url = url
                });
            }

            return item;
        }

        //TODO: public static string ReconstructInvertedAbstract(int indexLength, Dictionary<string, int[]> invertedIndex)
        //var index = detail["IA"];
        //var indexLength = detail["IndexLength"].ToObject<int>();
        //var invertedIndex =
        //    index["InvertedIndex"].ToObject<Dictionary<string, int[]>>();

        //{
        //    string[] abstractStr = new string[indexLength];

        //    foreach (var pair in invertedIndex)
        //    {
        //        string word = pair.Key;
        //        foreach (var index in pair.Value)
        //        {
        //            abstractStr[index] = word;
        //            //GetCardsAttachments();
        //        }
        //    }

        //    return String.Join(" ", abstractStr); // TODO: Estruturar de acordo com o Carrousel.
        //}
    }
}