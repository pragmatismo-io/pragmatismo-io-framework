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
using System.Globalization;
using System.Security.Authentication;
using Microsoft.Bot.Connector;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace Pragmatismo.Io.Framework.Diagnostics
{
    internal class NoSqlLogger
    {
        private static string _connectionString;
        private static string _databaseName;

        public NoSqlLogger(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public static void InsertLog(IMessageActivity activity, string original)
        {
            var msg = new BsonDocument
            {
                {"messageOriginal", original},
                {"messageCorrected", activity.Text},
                {"datetime", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}
            };

            var settings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
            settings.SslSettings =
                new SslSettings {EnabledSslProtocols = SslProtocols.Tls12};
            var clientm = new MongoClient(settings);
            var database = clientm.GetDatabase(_databaseName);
            var todoTaskCollection = database.GetCollection<BsonDocument>("messages");
            todoTaskCollection.InsertOneAsync(msg);
        }
    }
}