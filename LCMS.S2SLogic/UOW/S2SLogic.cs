using Common.HttpClient;
using Galaxy.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace LCMS.S2SLogic
{
    public class S2SLogic(IHttpServiceClient httpServiceClient, BaseRequestProfile baseRequestProfile, IConfiguration configuration) : IS2SLogic
    {
        public AdminS2SLogic Admin => new AdminS2SLogic(httpServiceClient, baseRequestProfile, configuration);
    }
}
