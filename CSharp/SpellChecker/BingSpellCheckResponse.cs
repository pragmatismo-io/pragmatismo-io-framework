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

namespace Pragmatismo.Io.Framework.SpellChecker
{
    public abstract class BingSpellCheckResponse
    {
        public string Type { get; set; }

        public Flaggedtoken[] FlaggedTokens { get; set; }

        public Error Error { get; set; }
    }

    public abstract class Flaggedtoken
    {
        public int Offset { get; set; }
        public string Token { get; set; }
        public string Type { get; set; }
        public Suggestion[] Suggestions { get; set; }
    }

    public class Suggestion
    {
        public string SuggestionValue { get; set; }
        public double Score { get; set; }
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}