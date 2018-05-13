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


export class AzureSearch {
  client: any;
  searchKey: string;
  searchIndex: string;
  searchIndexer: string;

  constructor(searchKey: string, searchHost: string, searchIndex: string, searchIndexer: string) {
    this.searchKey = searchKey;
    this.searchIndex = searchIndex;
    this.searchIndexer = searchIndexer;
    this.client = require("azure-search")({
      url: "https://" + searchHost,
      key: searchKey
    });
  }

  search(queryText: string, callback: any, top = 1000) {
    this.client.search(
      this.searchIndex,
      {
        search: queryText,
        queryType: "simple",
        searchMode: "any"
      },
      callback
    );
  }

  deleteIndex(cb: any) {
    this.client.deleteIndex(this.searchIndex, (err) => {
      this.client.deleteIndexer(this.searchIndexer, (err) => {
        if (err) { console.log(err); }
        cb(null, err);
      });
    });
  }

  /** Creates an index in Azure search. */
  createIndex(schema, dataSourceName, cb: any) {
    let _this_ = this;
    this.client.createIndex(schema, (err, schemaReturned)=> {
      let schemaIndexer = {
        name: _this_.searchIndexer,
        dataSourceName: dataSourceName,
        targetIndexName: this.searchIndex,
        parameters: {
          'maxFailedItems': 10,
          'maxFailedItemsPerBatch': 5,
          'base64EncodeKeys': false,
          'batchSize': 500
        }
      };

      this.client.createIndexer(schemaIndexer, function (err, schemaIndexerReturned) {
        cb(schemaIndexerReturned, err);
      });
    });
  }
}

