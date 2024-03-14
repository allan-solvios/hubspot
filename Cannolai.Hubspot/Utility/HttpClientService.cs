using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cannolai.Hubspot.Utility
{
    public class HttpClientService
    {
        public async Task<ResponseModel> GetAsync(string url, string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = httpClient.GetAsync(url).Result;

                return await ProcessResponse(response, accessToken);
            }
        }

        public async Task<ResponseModel> PostAsync<T>(string url, string accessToken, T content)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var jsonContent = JsonConvert.SerializeObject(content);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                
                return await ProcessResponse(response, accessToken);
            }
        }

        private async Task<ResponseModel> ProcessResponse(HttpResponseMessage response, string accessToken = null)
        {
            var responseModel = new ResponseModel();

            try
            {
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();

                responseModel.IsSuccess = true;
                responseModel.Message = $"Record added success with token : {accessToken}";
                responseModel.Result = responseData;
            }
            catch (HttpRequestException ex)
            {
                responseModel.IsSuccess = false;
                responseModel.Message = $"HTTP request error: {ex.Message}";
                responseModel.Result = $"Not added record with access token : {accessToken}";
            }

            return responseModel;
        }

    }
}
