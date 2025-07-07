using System.Net;
using System.Net.Http.Json;

namespace WMB.Api.IntegrationTests.Clients
{
    public class HttpResponseWrapper<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? ResponseBody { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class IntegrationHttpClient
    {
        private readonly HttpClient _httpClient;

        public IntegrationHttpClient(string baseUrl, TimeSpan timeout)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = timeout
            };
        }

        public async Task<HttpResponseWrapper<TResponse>> GetAsync<TResponse>(string endpoint)
        {
            var responseWrapper = new HttpResponseWrapper<TResponse>();

            try
            {
                var respone = await _httpClient.GetAsync(endpoint);
                responseWrapper.StatusCode = respone.StatusCode;

                if (respone.Content != null)
                {
                    try
                    {
                        responseWrapper.ResponseBody = await respone.Content.ReadFromJsonAsync<TResponse>();
                    }
                    catch (Exception ex)
                    {
                        responseWrapper.ErrorMessage = $"Deserialization Error: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                responseWrapper.ErrorMessage = ex.Message;
            }

            return responseWrapper;
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var responseWrapper = new HttpResponseWrapper<TResponse>();

            try
            {
                var respone = await _httpClient.PostAsJsonAsync(endpoint, data);
                responseWrapper.StatusCode = respone.StatusCode;

                if (respone.Content != null)
                {
                    try
                    {
                        responseWrapper.ResponseBody = await respone.Content.ReadFromJsonAsync<TResponse>();
                    }
                    catch (Exception ex)
                    {
                        responseWrapper.ErrorMessage = $"Deserialization Error: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                responseWrapper.ErrorMessage = ex.Message;
            }

            return responseWrapper;
        }

        public async Task<HttpResponseWrapper<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var responseWrapper = new HttpResponseWrapper<TResponse>();

            try
            {
                var respone = await _httpClient.PutAsJsonAsync(endpoint, data);
                responseWrapper.StatusCode = respone.StatusCode;

                if (respone.Content != null)
                {
                    try
                    {
                        responseWrapper.ResponseBody = await respone.Content.ReadFromJsonAsync<TResponse>();
                    }
                    catch (Exception ex)
                    {
                        responseWrapper.ErrorMessage = $"Deserialization Error: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                responseWrapper.ErrorMessage = ex.Message;
            }

            return responseWrapper;
        }
        
        public async Task<HttpResponseWrapper<object>> DeleteAsync(string endpoint)
        {
            var responseWrapper = new HttpResponseWrapper<object>();

            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                responseWrapper.StatusCode = response.StatusCode;

                if (response.Content != null)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                             responseWrapper.ErrorMessage = content;
                        }   
                    }
                    catch (Exception ex)
                    {
                        responseWrapper.ErrorMessage = $"Error reading response content: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                responseWrapper.ErrorMessage = ex.Message;
            }
            
            return responseWrapper;
        }
    }
}
