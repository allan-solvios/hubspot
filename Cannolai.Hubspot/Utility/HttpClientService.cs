using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Cannolai.Hubspot.Utility
{
    public class HttpClientService
    {
        public async Task<(ResponseModel, HttpStatusCode?)> GetAsync(string url, string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = httpClient.GetAsync(url).Result;

                return await ProcessResponse(response, accessToken);
            }
        }

        public async Task<(ResponseModel, HttpStatusCode?)> PostAsync<T>(string url, string accessToken, T content)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var jsonContent = JsonSerializer.Serialize(content);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);

                return await ProcessResponse(response, accessToken);
            }
        }

        private async Task<(ResponseModel, HttpStatusCode?)> ProcessResponse(HttpResponseMessage response, string accessToken = null)
        {
            var responseModel = new ResponseModel();
            HttpStatusCode? statusCode = HttpStatusCode.OK;

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
                statusCode = ex.StatusCode;
                responseModel.IsSuccess = false;
                responseModel.Message = $"HTTP request error: {ex.Message}";
                responseModel.Result = $"Not added record with access token : {accessToken}";
            }

            return (responseModel, statusCode);
        }

    }
}
