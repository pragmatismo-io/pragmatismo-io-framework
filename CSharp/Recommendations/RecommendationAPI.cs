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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;

#endregion

namespace Pragmatismo.Io.Framework.Recommendations
{
    public sealed class RecommendationsApi
    {
        private const string BaseUri = "https://westus.api.cognitive.microsoft.com/recommendations/v4.0";

        private static readonly RecommendationsApi Recommender = null;

        // TODO: string modelName = "MyNewModel";
        // TODO: string modelId = "<INSERIR>";
        private readonly string _accountKey = string.Empty;

        private readonly HttpClient _httpClient;

        // TODO: public void TreinarVideos()
        //{
        //    string modelId = null;
        //    long buildId = -1;

        //    try
        //    {
        //        var recommender = new RecommendationsApi(AccountKey, BaseUri);

        //        // Create a model if not already provided.
        //        if (String.IsNullOrEmpty(modelId))
        //        {
        //            modelId = CreateModel(modelName);
        //        }

        //        // If build is not provided, trigger a build with new data.
        //        if (buildId == -1)
        //        {
        //            // Upload Catalog and Usage data and then train the model (create a build)
        //            buildId = UploadDataAndTrainModel(modelId, BuildType.Recommendation);
        //        }

        //        // Uncomment to score a batch of recommendations.
        //        // GetRecommendationsBatch(recommender, modelId, buildId);

        //        Console.WriteLine("Press any key to end");
        //        Console.ReadKey();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error encountered:");
        //        Console.WriteLine(e.Message);
        //        Console.WriteLine("Press any key to continue.");
        //        Console.ReadKey();
        //    }
        //    finally
        //    {
        //        // Uncomment the line below if you wish to delete the model.
        //        // Note that you can have up to 10 models at any time. 
        //        // You may have up to 20 builds per model.
        //        //recommender.DeleteModel(modelId); 
        //    }

        //}


        /// <summary>
        ///     Constructor that initializes the Http Client.
        /// </summary>
        public RecommendationsApi()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUri),
                Timeout = TimeSpan.FromMinutes(15),
                DefaultRequestHeaders =
                {
                    {"Ocp-Apim-Subscription-Key", _accountKey}
                }
            };
        }

        /// <summary>
        ///     Creates a model, upload catalog and usage file and trigger a build.
        ///     Returns the Build ID of the trained build.
        /// </summary>
        public static string CreateModel(string modelName)
        {
            Console.WriteLine("Creating a new model {0}...", modelName);
            var modelInfo = Recommender.CreateModel(modelName, "MSStore");
            var modelId = modelInfo.Id;
            Console.WriteLine("Model '{0}' created with ID: {1}", modelName, modelId);
            return modelId;
        }

        /// <summary>
        ///     Creates a model, upload catalog and usage files and trigger a build.
        ///     Returns the Build ID of the trained build.
        /// </summary>
        public static long UploadDataAndTrainModel(string modelId, BuildType buildType = BuildType.Recommendation)
        {
            long buildId;

            // Import data to the model.            
            var resourcesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Resources");
            Console.WriteLine("Importing catalog files...");

            var catalogFilesCount = 0;
            foreach (var catalog in Directory.GetFiles(resourcesDir, "catalog*.csv"))
            {
                var catalogFile = new FileInfo(catalog);
                Recommender.UploadCatalog(modelId, catalogFile.FullName, catalogFile.Name);
                catalogFilesCount++;
            }

            Console.WriteLine("Imported {0} catalog files.", catalogFilesCount);


            Console.WriteLine("Importing usage files...");
            var usageFilesCount = 0;
            foreach (var usage in Directory.GetFiles(resourcesDir, "usage*.csv"))
            {
                var usageFile = new FileInfo(usage);
                Recommender.UploadUsage(modelId, usageFile.FullName, usageFile.Name);
                usageFilesCount++;
            }

            Console.WriteLine("Imported {0} usage files.", usageFilesCount);

            #region training

            // Trigger a recommendation build.
            string operationLocationHeader;
            Console.WriteLine("Triggering build for model '{0}'. \nThis will take a few minutes...", modelId);
            if (buildType == BuildType.Recommendation)
                buildId = Recommender.CreateRecommendationsBuild(modelId,
                    "Recommendation Build " + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                    false,
                    out operationLocationHeader);
            else
                buildId = Recommender.CreateFbtBuild(modelId,
                    "Frequenty-Bought-Together Build " + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                    false,
                    out operationLocationHeader);

            // Monitor the build and wait for completion.
            Console.WriteLine("Monitoring build {0}", buildId);
            var buildInfo = Recommender.WaitForOperationCompletion<BuildInfo>(GetOperationId(operationLocationHeader));
            Console.WriteLine("Build {0} ended with status {1}.\n", buildId, buildInfo.Status);

            if (string.Compare(buildInfo.Status, "Succeeded", StringComparison.OrdinalIgnoreCase) != 0)
            {
                Console.WriteLine("Build {0} did not end successfully, the sample app will stop here.", buildId);
                Console.WriteLine("Press any key to end");
                Console.ReadKey();
                return -1;
            }

            // Waiting  in order to propagate the model updates from the build...
            Console.WriteLine("Waiting for 40 sec for propagation of the built model...");
            Thread.Sleep(TimeSpan.FromSeconds(40));

            // The below api is more meaningful when you want to give a certain build id to be an active build.
            // Currently this app has a single build which is already active.
            Console.WriteLine("Setting build {0} as active build.", buildId);
            Recommender.SetActiveBuild(modelId, buildId);

            #endregion

            return buildId;
        }


        /// <summary>
        ///     Creates a new model.
        /// </summary>
        /// <param name="modelName">Name for the model</param>
        /// <param name="description">Description for the model</param>
        /// <returns>Model Information.</returns>
        public ModelInfo CreateModel(string modelName, string description)
        {
            var uri = BaseUri + "/models/";
            var modelRequestInfo = new ModelRequestInfo {ModelName = modelName, Description = description};
            var response = _httpClient.PostAsJsonAsync(uri, modelRequestInfo).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to create model {modelName}, \n reason {ExtractErrorInfo(response)}");

            var jsonString = ExtractReponse(response);
            var modelInfo = JsonConvert.DeserializeObject<ModelInfo>(jsonString);
            return modelInfo;
        }

        /// <summary>
        ///     Upload catalog items to a model.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="catalogFilePath">Catalog file path</param>
        /// <param name="catalogDisplayName">Name for the catalog</param>
        /// <returns>Statistics about the catalog import operation.</returns>
        public void UploadCatalog(string modelId, string catalogFilePath, string catalogDisplayName)
        {
            Console.WriteLine("Uploading " + catalogDisplayName + " ...");
            var uri = BaseUri + "/models/" + modelId + "/catalog?catalogDisplayName=" + catalogDisplayName;
            using (var filestream = new FileStream(catalogFilePath, FileMode.Open, FileAccess.Read))
            {
                var response = _httpClient.PostAsync(uri, new StreamContent(filestream)).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception(
                        $"Error {response.StatusCode}: Failed to import catalog items {catalogFilePath}, for model {modelId} \n reason {ExtractErrorInfo(response)}");

                var jsonString = ExtractReponse(response);
                var catalogImportStats = JsonConvert.DeserializeObject<CatalogImportStats>(jsonString);
            }
        }

        /// <summary>
        ///     Upload usage data to a model.
        ///     Usage files must be less than 200 MB.
        ///     If you need to upload more than 200 MB, you may call this function multiple times.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="usageFilePath">Usage file path</param>
        /// <param name="usageDisplayName">Name for the usage data being uploaded</param>
        /// <returns>Statistics about the usage upload operation.</returns>
        public UsageImportStats UploadUsage(string modelId, string usageFilePath, string usageDisplayName)
        {
            Console.WriteLine("Uploading " + usageDisplayName + " ...");

            var uri = BaseUri + "/models/" + modelId + "/usage?usageDisplayName=" + usageDisplayName;

            using (var filestream = new FileStream(usageFilePath, FileMode.Open, FileAccess.Read))
            {
                var response = _httpClient.PostAsync(uri, new StreamContent(filestream)).Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception(
                        $"Error {response.StatusCode}: Failed to import usage data {usageFilePath}, for model {modelId} \n reason {ExtractErrorInfo(response)}");

                var jsonString = ExtractReponse(response);
                var usageImportStats = JsonConvert.DeserializeObject<UsageImportStats>(jsonString);
                return usageImportStats;
            }
        }


        /// <summary>
        ///     Submit a model build, with passed build parameters.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="buildRequestInfo">Build parameters</param>
        /// <param name="operationLocationHeader">Build operation location</param>
        /// <returns>The build id.</returns>
        public long BuildModel(string modelId, BuildRequestInfo buildRequestInfo, out string operationLocationHeader)
        {
            var uri = BaseUri + "/models/" + modelId + "/builds";
            var response = _httpClient.PostAsJsonAsync(uri, buildRequestInfo).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to start build for model {modelId}, \n reason {ExtractErrorInfo(response)}");

            operationLocationHeader = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            var buildModelResponse = JsonConvert.DeserializeObject<BuildModelResponse>(jsonString);
            return buildModelResponse.BuildId;
        }


        /// <summary>
        ///     Delete a certain build of a model.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        /// <param name="buildId">Unique identifier of the build</param>
        public void DeleteBuild(string modelId, long buildId)
        {
            var uri = BaseUri + "/models/" + modelId + "/builds/" + buildId;
            var response = _httpClient.DeleteAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to delete buildId {buildId} for modelId {modelId}, \n reason {ExtractErrorInfo(response)}");
        }

        /// <summary>
        ///     Delete a model, also the associated catalog/usage data and any builds.
        /// </summary>
        /// <param name="modelId">Unique identifier of the model</param>
        public void DeleteModel(string modelId)
        {
            var uri = BaseUri + "/models/" + modelId;
            var response = _httpClient.DeleteAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to delete modelId {modelId}, \n reason {ExtractErrorInfo(response)}");
        }


        /// <summary>
        ///     Trigger a recommendation build for the given model.
        ///     Note: unless configured otherwise the u2i (user to item/user based) recommendations are enabled too.
        /// </summary>
        /// <param name="modelId">the model id</param>
        /// <param name="buildDescription">a description for the build</param>
        /// <param name="enableModelInsights">
        ///     true to enable modeling insights, selects "LastEventSplitter" as the splitting
        ///     strategy by default.
        /// </param>
        /// <param name="operationLocationHeader">
        ///     operation location header, can be used to cancel the build operation and to get
        ///     status.
        /// </param>
        /// <returns>Unique indentifier of the build initiated.</returns>
        public long CreateRecommendationsBuild(string modelId,
            string buildDescription,
            bool enableModelInsights,
            out string operationLocationHeader)
        {
            // only used if splitter strategy is set to RandomSplitter
            var randomSplitterParameters = new RandomSplitterParameters
            {
                RandomSeed = 0,
                TestPercent = 10
            };

            var parameters = new RecommendationBuildParameters
            {
                NumberOfModelIterations = 10,
                NumberOfModelDimensions = 20,
                ItemCutOffLowerBound = 1,
                EnableModelingInsights = enableModelInsights,
                SplitterStrategy = SplitterStrategy.LastEventSplitter,
                RandomSplitterParameters = randomSplitterParameters,
                EnableU2I = true,
                UseFeaturesInModel = false,
                AllowColdItemPlacement = false
            };

            var requestInfo = new BuildRequestInfo
            {
                BuildType = BuildType.Recommendation,
                BuildParameters = new BuildParameters
                {
                    Recommendation = parameters
                },
                Description = buildDescription
            };

            return BuildModel(modelId, requestInfo, out operationLocationHeader);
        }

        /// <summary>
        ///     Trigger a recommendation build for the given model.
        ///     Note: unless configured otherwise the u2i (user to item/user based) recommendations are enabled too.
        /// </summary>
        public long CreateFbtBuild(string modelId, string buildDescription, bool enableModelInsights,
            out string operationLocationHeader)
        {
            // only used if splitter strategy is set to RandomSplitter
            var randomSplitterParameters = new RandomSplitterParameters
            {
                RandomSeed = 0,
                TestPercent = 10
            };

            var parameters = new FbtBuildParameters
            {
                MinimalScore = 0,
                SimilarityFunction = FbtSimilarityFunction.Lift,
                SupportThreshold = 3,
                MaxItemSetSize = 2,
                EnableModelingInsights = enableModelInsights,
                SplitterStrategy = SplitterStrategy.LastEventSplitter,
                RandomSplitterParameters = randomSplitterParameters
            };

            var requestInfo = new BuildRequestInfo
            {
                BuildType = BuildType.Fbt,
                BuildParameters = new BuildParameters
                {
                    Fbt = parameters
                },
                Description = buildDescription
            };

            return BuildModel(modelId, requestInfo, out operationLocationHeader);
        }

        /// <summary>
        ///     Monitor operation status and wait for completion.
        /// </summary>
        /// <param name="operationId">The operation id</param>
        /// <returns>Build status</returns>
        public OperationInfo<T> WaitForOperationCompletion<T>(string operationId)
        {
            OperationInfo<T> operationInfo;

            var uri = BaseUri + "/operations";

            while (true)
            {
                var response = _httpClient.GetAsync(uri + "/" + operationId).Result;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                operationInfo = JsonConvert.DeserializeObject<OperationInfo<T>>(jsonString);

                // Operation status {NotStarted, Running, Cancelling, Cancelled, Succeded, Failed}
                Console.WriteLine(" Operation Status: {0}. \t Will check again in 10 seconds.", operationInfo.Status);

                if (OperationStatus.Succeeded.ToString().Equals(operationInfo.Status) ||
                    OperationStatus.Failed.ToString().Equals(operationInfo.Status) ||
                    OperationStatus.Cancelled.ToString().Equals(operationInfo.Status))
                    break;

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            return operationInfo;
        }

        /// <summary>
        ///     Extract the operation id from the operation header
        /// </summary>
        /// <param name="operationLocationHeader"></param>
        /// <returns></returns>
        public static string GetOperationId(string operationLocationHeader)
        {
            var index = operationLocationHeader.LastIndexOf('/');
            var operationId = operationLocationHeader.Substring(index + 1);
            return operationId;
        }

        /// <summary>
        ///     Set an active build for the model.
        /// </summary>
        /// <param name="modelId">Unique idenfier of the model</param>
        /// <param name="updateActiveBuildInfo"></param>
        public void SetActiveBuild(string modelId, UpdateActiveBuildInfo updateActiveBuildInfo)
        {
            var uri = BaseUri + "/models/" + modelId;
            var content = new ObjectContent<UpdateActiveBuildInfo>(updateActiveBuildInfo, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri) {Content = content};
            var response = _httpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception("Error HTTP Status Code");
        }

        /// <summary>
        ///     Get Item to Item (I2I) Recommendations or Frequently-Bought-Together (FBT) recommendations
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null if you want to use active build</param>
        /// <param name="itemIds"></param>
        /// <param name="numberOfResults"></param>
        /// <returns>
        ///     The recommendation sets. Note that I2I builds will only return one item per set.
        ///     FBT builds will return more than one item per set.
        /// </returns>
        public RecommendedItemSetInfoList GetRecommendations(string modelId, long? buildId, string itemIds,
            int numberOfResults)
        {
            var uri = BaseUri + "/models/" + modelId + "/recommend/item?itemIds=" + itemIds +
                      "&numberOfResults=" + numberOfResults + "&minimalScore=0";

            //Set active build if passed.
            if (buildId != null)
                uri = uri + "&buildId=" + buildId;

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to get recommendations for modelId {modelId}, buildId {buildId}, Reason: {ExtractErrorInfo(response)}");

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);
            return recommendedItemSetInfoList;
        }

        /// <summary>
        ///     Use historical transaction data to provide personalized recommendations for a user.
        ///     The user history is extracted from the usage files used to train the model.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="buildId">The build identifier. Set to null to use active build.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="numberOfResults">Desired number of recommendation results.</param>
        /// <returns>The recommendations for the user.</returns>
        public RecommendedItemSetInfoList GetUserRecommendations(string modelId, long? buildId, string userId,
            int numberOfResults)
        {
            var uri = BaseUri + "/models/" + modelId + "/recommend/user?userId=" + userId + "&numberOfResults=" +
                      numberOfResults;

            //Set active build if passed.
            if (buildId != null)
                uri = uri + "&buildId=" + buildId;

            var response = _httpClient.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to get user recommendations for modelId {modelId}, buildId {buildId}, Reason: {ExtractErrorInfo(response)}");

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var recommendedItemSetInfoList = JsonConvert.DeserializeObject<RecommendedItemSetInfoList>(jsonString);
            return recommendedItemSetInfoList;
        }

        /// <summary>
        ///     Update model information
        /// </summary>
        /// <param name="modelId">the id of the model</param>
        /// <param name="activeBuildId">the id of the build to be active (optional)</param>
        public void SetActiveBuild(string modelId, long activeBuildId)
        {
            var info = new UpdateActiveBuildInfo
            {
                ActiveBuildId = activeBuildId
            };

            SetActiveBuild(modelId, info);
        }


        /// <summary>
        ///     Extract error message from the httpResponse, (reason phrase + body)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ExtractErrorInfo(HttpResponseMessage response)
        {
            string detailedReason = null;
            if (response.Content != null)
                detailedReason = response.Content.ReadAsStringAsync().Result;
            var errorMsg = detailedReason == null
                ? response.ReasonPhrase
                : response.ReasonPhrase + "->" + detailedReason;
            return errorMsg;
        }

        /// <summary>
        ///     Extract error information from HTTP response message.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ExtractReponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            string detailedReason = null;
            if (response.Content != null)
                detailedReason = response.Content.ReadAsStringAsync().Result;
            var errorMsg = detailedReason == null
                ? response.ReasonPhrase
                : response.ReasonPhrase + "->" + detailedReason;

            var error = $"Status code: {(int) response.StatusCode}\nDetail information: {errorMsg}";
            throw new Exception("Response: " + error);
        }

/*
        TODO: 

        /// <summary>
        ///     Request the batch job
        /// </summary>
        /// <param name="batchJobsRequestInfo">The batch job request information</param>
        /// <param name="operationLocationHeader">Batch operation location</param>
        /// <returns></returns>
        public string StartBatchJob(BatchJobsRequestInfo batchJobsRequestInfo, out string operationLocationHeader)
        {
            var uri = BaseUri + "/batchjobs";
            var response = _httpClient.PostAsJsonAsync(uri, batchJobsRequestInfo).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Error {response.StatusCode}: Failed to submit the batch job for model {batchJobsRequestInfo.Job.ModelId}, Reason: {ExtractErrorInfo(response)}");

            operationLocationHeader = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            var batchJobResponse = JsonConvert.DeserializeObject<BatchJobsResponse>(jsonString);
            return batchJobResponse.BatchId;
        }
*/
    }
}