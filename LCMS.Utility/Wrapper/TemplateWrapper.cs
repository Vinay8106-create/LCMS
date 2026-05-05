using Newtonsoft.Json;
using System.Text;

namespace LCMS.Utility
{
    public static class TemplateWrapper
    {
        public class TemplateServiceResponse<T>
        {
            public T Content { get; init; }
            public string Error { get; init; }
            public bool IsSuccess => Error == null;
        }
        public static async Task<TemplateServiceResponse<byte[]>> PostJsonForBinaryAsync(HttpClient httpClient, string templateName, object requestBody, JsonSerializerSettings jsonSettings, CancellationToken cancellationToken = default)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestBody, jsonSettings);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, templateName) { Content = content };
                using var response = await httpClient.SendAsync(request, cancellationToken);

                var bytes = await response.Content.ReadAsByteArrayAsync();

                return response.IsSuccessStatusCode
                     ? new TemplateServiceResponse<byte[]> { Content = bytes }
                     : new TemplateServiceResponse<byte[]> { Error = $"HTTP {(int)response.StatusCode}: {Encoding.UTF8.GetString(bytes)}" };
            }
            catch (Exception ex)
            {
                return new TemplateServiceResponse<byte[]> { Error = ex.Message };
            }
        }
    }
}
