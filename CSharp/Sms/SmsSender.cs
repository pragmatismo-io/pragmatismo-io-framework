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
using System.Net;
using System.Text;
using System.Web;

#endregion

namespace Pragmatismo.Io.Framework.Sms
{
    public class SmsSender
    {
        public void Send(string recipient, string message, string password, string email, string accountId)
        {
            var client = new WebClient();

            var requestUrl = "https://redoxygen.net/sms.dll?Action=SendSMS";

            var requestData = "AccountId=" + accountId
                              + "&Email=" + HttpUtility.UrlEncode(email)
                              + "&Password=" + HttpUtility.UrlEncode(password)
                              + "&Recipient=" + HttpUtility.UrlEncode(recipient)
                              + "&Message=" + HttpUtility.UrlEncode(message);

            var postData = Encoding.ASCII.GetBytes(requestData);
            var response = client.UploadData(requestUrl, postData);

            var result = Encoding.ASCII.GetString(response);
            var resultCode = Convert.ToInt32(result.Substring(0, 4));
        }
    }
}