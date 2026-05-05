using Galaxy.Domain.Exceptions;
using Galaxy.Utility;
using LCMS.Utility;
using RestSharp;

namespace LCMS.Utility
{
    public class TemplateUtility
    {
        private readonly RestClient _client;
        public TemplateUtility()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["LCMSPdfTemplate"]);
        }
        public byte[] PrintReport(string ReportName, string Json)
        {
            string Err;
            RestRequest request = new RestRequest(ReportName, Method.POST);
            request.AddParameter("application/json", Json, ParameterType.RequestBody);
            var response = request.ExecuteTemplateService(_client, out Err);
            if (!string.IsNullOrEmpty(Err))
            {
                throw new BusinessException(Err);
            }
            return response.RawBytes;
        }


    }
}