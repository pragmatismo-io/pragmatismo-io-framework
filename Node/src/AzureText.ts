/*****************************************************************************\
|                                               ( )_  _                       |
|    _ _    _ __   _ _    __    ___ ___     _ _ | ,_)(_)  ___   ___     _     |
|   ( '_`\ ( '__)/'_` ) /'_ `\/' _ ` _ `\ /'_` )| |  | |/',__)/' _ `\ /'_`\   |
|   | (_) )| |  ( (_| |( (_) || ( ) ( ) |( (_| || |_ | |\__, \| ( ) |( (_) )  |
|   | ,__/'(_)  `\__,_)`\__  |(_) (_) (_)`\__,_)`\__)(_)(____/(_) (_)`\___/'  |
|   | |                ( )_) |                                                |
|   (_)                 \___/'                                                |
|                                                                             |
| Copyright (c) Pragmatismo.io. All rights reserved.                          |
| Licensed under the MIT license                                              |
|                                                                             |
| MIT License:                                                                |
| Permission is hereby granted, free of charge, to any person obtaining       |
| a copy of this software and associated documentation files (the             |
| "Software"), to deal in the Software without restriction, including         |
| without limitation the rights to use, copy, modify, merge, publish,         |
| distribute, sublicense, and/or sell copies of the Software, and to          |
| permit persons to whom the Software is furnished to do so, subject to       |
| the following conditions:                                                   |
|                                                                             |
| The above copyright notice and this permission notice shall be              |
| included in all copies or substantial portions of the Software.             |
|                                                                             |
| THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,           |
| EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF          |
| MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND                       |
| NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE      |
| LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION      |
| OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION       |
| WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.             |
|                                                                             |
\*****************************************************************************/

"use strict";

const textAnalytics = require("cogserv-text-analytics")

export class AzureText {

  static async getSpelledText(
    key: string,
    text: string
  ): Promise<string> {
    return new Promise<string>(
      (resolve, reject) => {

        let https = require("https");

        let host = "api.cognitive.microsoft.com";
        let path = "/bing/v7.0/spellcheck";

        let mkt = "pt-PT";
        let mode = "spell";
        let query_string = "?mkt=" + mkt + "&mode=" + mode;

        let request_params = {
          method: "POST",
          hostname: host,
          path: path + query_string,
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            "Content-Length": text.length + 5,
            "Ocp-Apim-Subscription-Key": key
          }
        };

        let getCorrectedText = (sourceText: string, response: any) => {

          let getWordIndexByWordOffsetInText = function (text, wordOffsetInText) {
            let index = 0;
            let currentWordIndex = 0;
            text = text.split(" ");
            text.forEach(element => {
              if (index >= wordOffsetInText) {
                return false;
              }
              index += element.length + 1;
              currentWordIndex++;
            });
            return currentWordIndex;
          };
          let replaceWordAtIndex = function (text, index, replacement) {
            text = text.split(" ");
            text[index] = replacement;
            text = text.join(" ");
            return text;
          };

          let index = 0;
          if (response.flaggedTokens) {
            response.flaggedTokens.forEach(element => {
              index = getWordIndexByWordOffsetInText(sourceText, element.offset);
              sourceText = replaceWordAtIndex(sourceText, index, element.suggestions[0].suggestion);
              index++;
            });
          }
          return sourceText;
        };

        let response_handler = function (response) {
          let body = "";
          response.on("data", function (d) {
            body += d;
          });
          response.on("end", function () {
            resolve(getCorrectedText(text, JSON.parse(body)));
          });
          response.on("error", function (e) {
            reject(e.message);
          });
        };
        let req = https.request(request_params, response_handler);
        req.write("text=" + text);
        req.end();
      });
  }

  static isIntentYes(utterance) {
    return utterance.match(
      /^(sim|s|positivo|afirmativo|claro|evidente|sem dúvida)/i
    );
  }

  static isIntentNo(utterance) {
    return utterance.match(/^(não|nao|n|esse não|nenhum.*)/i);
  }

  static isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
  }

  static async getSentiment(key: string, language: string, text: string):
    Promise<number> {
    const documents = [
      {
        id: "1",
        language: language,
        text: text
      }
    ];
    const { sentiment } = textAnalytics({ key: key });

    try {
      let response = await sentiment(documents);
      var result = JSON.parse(response);
      return Promise.resolve(result.documents[0].score);
    } catch (reason) {
      return Promise.reject(reason);
    }
  }

  getKeywords(key: string, language: string, phrase: string) {
    return new Promise(
      (resolve, reject) => {

        const documents = [
          {
            id: "1",
            language: language,
            text: phrase
          }
        ];

        const { keyPhrases } = textAnalytics({ key: key });

        keyPhrases(documents)
          .then(res => JSON.parse(res))
          .then(res => {
            console.log(res.documents);
            resolve(res);
          })

          .catch(err => {
            console.log(`Sentiment error: ${err}`);
            reject(err);
          });

        keyPhrases(documents)
          .then(res => JSON.parse(res))
          .then(res => {
            console.log(res.documents);
            resolve(res);
          })

          .catch(err => {
            console.log(`Sentiment error: ${err}`);
            reject(err);
          });
      });
  }
}
