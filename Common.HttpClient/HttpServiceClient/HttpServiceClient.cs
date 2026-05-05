using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Utility;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Common.HttpClient
{
    public class HttpServiceClient : IHttpServiceClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly BaseRequestProfile _profile;

        public HttpServiceClient(System.Net.Http.HttpClient httpClient, BaseRequestProfile userProfile, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _profile = userProfile;
            _configuration = configuration;
        }

        private string GetBaseUrl(string integrationEndPoint) => _configuration.GetConfigurationFromAppSettings(integrationEndPoint) ?? throw new InvalidOperationException($"Base URL is not configured. Please configure '{integrationEndPoint}' in AppSettings.");

        private void ConfigureHttpClient(HttpServiceRequest request)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (request.IsAuthorized)
            {
                if (string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    throw new HttpRequestException(ErrorMessages.AccessTokenRequired, null, HttpStatusCode.Unauthorized);
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {request.AccessToken}");
            }
            if (!string.IsNullOrWhiteSpace(request.TenantCode))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Tenant-Code", request.TenantCode);
            }
            else if (request.IsMutiTenentSolution)
            {
                throw new HttpRequestException(ErrorMessages.TenantCodeRequired, null, HttpStatusCode.BadRequest);
            }
            if (!string.IsNullOrWhiteSpace(request.CorrelationId))
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", request.CorrelationId);
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        private async Task<T?> SendRequestAsync<T>(HttpMethod method, HttpServiceRequest requestDto)
        {
            string? fullUrl = null;
            string baseUrl = GetBaseUrl(requestDto.ServiceEndpoint);
            ConfigureHttpClient(requestDto);

            string route = requestDto.RoutePath.TrimStart('/');
            string? query = requestDto.QueryString?.TrimStart('?');

            fullUrl = $"{baseUrl.TrimEnd('/')}/{route}";

            if (method == HttpMethod.Get && !string.IsNullOrWhiteSpace(query))
                fullUrl += $"?{query}";

            using var request = new HttpRequestMessage(method, fullUrl);

            if (requestDto.BodyPayload is not null && method != HttpMethod.Get)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestDto.BodyPayload),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            using var response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            return HandleResponse<T>(response, content);
        }

        private T? HandleResponse<T>(HttpResponseMessage response, string content)
        {
            if (response.IsSuccessStatusCode)
            {
                return !string.IsNullOrWhiteSpace(content) ? JsonSerializer.Deserialize<T>(content, _jsonOptions) : default;
            }
            var (ExceptionError, ErrorMessage) = TryExtractErrorDetail(content, response.StatusCode);
            string exceptionMessage = ExceptionError ?? ErrorMessages.UnhandledError;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    if (ErrorMessage != null)
                    {
                        throw new BusinessException(ErrorMessage.ErrorMessage.Select(x => x.Msg).ToList(), response.StatusCode);
                    }
                    throw new BusinessException(exceptionMessage, response.StatusCode);
                default:
                    throw new BusinessException(exceptionMessage, response.StatusCode);
            }
        }

        private void SetErrorMessage<T>(T? objectInstance, AppMessage? errorMessage)
        {
            var msgProp = typeof(T).GetProperty("Msg", BindingFlags.Public | BindingFlags.Instance);
            if (msgProp != null && msgProp.CanWrite && msgProp.PropertyType == typeof(AppMessage))
            {
                msgProp.SetValue(objectInstance, errorMessage);
            }
        }
        public static class ErrorMessages
        {
            public const string UnexpectedErrorOccurred = "An unexpected error occurred while processing your request. Please try again or contact support if the issue persists.";
            public const string BadRequestError = "The request could not be processed. Please verify your input and try again.";
            public const string UnhandledError = "An error occurred while processing your request. Please contact support for further assistance.";
            public const string AccessTokenRequired = "Authorization failed. Access token is missing. Please ensure you are properly authenticated or contact support.";
            public const string TenantCodeRequired = "Tenant identification is required for this request. Please provide a valid Tenant Code or contact support.";
        }

        private static (string?, AppMessage?) TryExtractErrorDetail(string content, HttpStatusCode responseStatusCode)
        {
            try
            {
                if (responseStatusCode == HttpStatusCode.BadRequest)
                {
                    var ErrorMessage = !string.IsNullOrWhiteSpace(content)
                        ? JsonSerializer.Deserialize<AppMessage>(content, _jsonOptions)
                        : default;
                    return (null, ErrorMessage);
                }
                else
                {
                    using var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.TryGetProperty("Detail", out var detailProp))
                    {
                        return (detailProp.GetString(), null);
                    }
                }
            }
            catch
            {
                return (ExtractFromRegex(content), null);
            }
            return (null, null);
        }

        private static string? ExtractFromRegex(string content)
        {
            const string pattern = @"Detail\s*=\s*""([^""]+)""";
            var match = Regex.Match(content, pattern);
            return match.Success ? match.Groups[1].Value : null;
        }

        public Task<T?> GetAsync<T>(HttpServiceRequest request)
        {
            return SendRequestAsync<T>(HttpMethod.Get, request);
        }

        public Task<T?> PostAsync<T>(HttpServiceRequest request)
        {
            return SendRequestAsync<T>(HttpMethod.Post, request);
        }

        public Task<T?> TenantPostAsync<T>(HttpServiceRequest request)
        {
            return SendRequestAsync<T>(HttpMethod.Post, request);
        }

        public Task<T?> PutAsync<T>(HttpServiceRequest request)
        {
            return SendRequestAsync<T>(HttpMethod.Put, request);
        }

        public Task<T?> DeleteAsync<T>(HttpServiceRequest request)
        {
            return SendRequestAsync<T>(HttpMethod.Delete, request);
        }

    }
}

