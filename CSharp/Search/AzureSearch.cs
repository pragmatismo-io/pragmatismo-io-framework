using System;
using System.Collections.Generic;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Pragmatismo.Io.Framework.Search
{
    public class AzureSearch
    {                  
        private static SearchIndexClient _indexClient;
        private readonly SearchParameters _defaultParameters;

        public AzureSearch(string searchServiceName, string queryApiKey, string indexName)
        {
            _indexClient = new SearchIndexClient(searchServiceName, indexName, new SearchCredentials(queryApiKey));
            _defaultParameters = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = 10,
                Skip = 0,
                IncludeTotalResultCount = true,
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>",
            };
        }

        public DocumentSearchResult GetSingleDocument(string query, string filter, IList<string> fields)
        {
            _defaultParameters.Select = fields;
            _defaultParameters.Filter = filter;
            return _indexClient.Documents.Search(query, _defaultParameters);
        }

    }
}