using Galaxy.Utility;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;


namespace LCMS.Utility
{
    public static class CommonUtility
    {
        public static IRestResponse ExecuteTemplateService(this RestRequest request, RestClient _client, out string Err)
        {
            Err = null;
            request.Timeout = 320000;
            var serializersettings = JsonUtility.GetSerializerSettings();
            serializersettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
            object p = _client.UseNewtonsoftJson(serializersettings);

            IRestResponse response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Err = response.Content;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(response.Content))
                {
                    Err = response.ErrorException + " " + response.ErrorMessage;
                }
                else
                {
                    Err = response.Content;
                }
            }
            return default;
        }
    }
}
