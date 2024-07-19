using KeyServiceAPI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace KeyServiceAPI
{
    //TODO: Maybe Move it to Controllers Component and use namespace Clients..
    public class KeyServiceClient : IKeyServiceClient //TODO: Change name of interface to API or service???
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<KeyServiceClient> _logger;

        private const string KEY_ENDPOINT = "key";

        public KeyServiceClient(
            ILogger<KeyServiceClient> logger,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<(ResultStatus Status, EncryptionKeyModel KeyData)> GetEncryptionKeyAsync(Guid fileId)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //TODO: Authorization on KeyService

            try
            {
                var response = await _httpClient.GetAsync($"{KEY_ENDPOINT}/{fileId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var keyResponse = JsonConvert.DeserializeObject<ContentEncryptionKeyModel>(responseContent); //TODO: Response model???
                    //TODO: BaseString???
                    if (keyResponse?.EncryptionKey == null)
                    {
                        _logger.LogError("Deserialized uri object is empty."); 
                        return (ResultStatus.Failed, null); //TODO or empty byte list?? null generates exceptions when used so it is a better option
                    }

                    return (ResultStatus.Success, keyResponse.EncryptionKey);
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound: 
                        return (ResultStatus.NotFound, null);
                    case HttpStatusCode.Unauthorized:
                        _logger.LogWarning("Request should be blocked in APIGateway middleware!"); //TODO:
                        return (ResultStatus.AccessDenied, null);
                    case HttpStatusCode.Forbidden:
                        _logger.LogWarning("Request should be blocked in APIGateway middleware!");
                        return (ResultStatus.AccessDenied, null);
                }

                _logger.LogError($"Unexpected error in response while trying to get key. Message: {response.ReasonPhrase}");
                return (ResultStatus.Failed, null);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error occurred while trying to get key.");
                return (ResultStatus.Failed, null);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error occurred while trying to get key.");
                return (ResultStatus.Failed, null);
            }
        }

        //TODO: Test!!!!!!!
        public async Task<ResultStatus> CreateEncryptionKeyAsync(Guid fileId)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //TODO: Authorization on KeyService
            var requestContent = new StringContent(
                            JsonConvert.SerializeObject(new { fileId }),
                            Encoding.UTF8,
                            "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"{KEY_ENDPOINT}", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return ResultStatus.Success;
                    }

                    switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        return ResultStatus.Conflict;
                    case HttpStatusCode.Unauthorized:
                        _logger.LogWarning("Request should be blocked in APIGateway middleware!"); //TODO:
                        return ResultStatus.AccessDenied;
                    case HttpStatusCode.Forbidden:
                        _logger.LogWarning("Request should be blocked in APIGateway middleware!");
                        return ResultStatus.AccessDenied;
                }

                _logger.LogError($"Unexpected error in response while trying to get key. Message: {response.ReasonPhrase}");
                return ResultStatus.Failed;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error occurred while trying to get key.");
                return ResultStatus.Failed;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error occurred while trying to get key.");
                return ResultStatus.Failed;
            }
        }
    }
}
