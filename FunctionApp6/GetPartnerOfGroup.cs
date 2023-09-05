using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp6.DAL;

namespace FunctionApp6
{
    public static class GetPartnerOfGroup
    {
        [FunctionName("getPartnerOfGroup")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing Partner data Azure Function");
            string groupId = req.Query["groupId"];

            Microsoft.Extensions.Primitives.StringValues origin;
            Microsoft.Extensions.Primitives.StringValues referrer;

            req.Headers.TryGetValue("Origin", out origin);
            req.Headers.TryGetValue("Referer", out referrer);

            log.LogInformation("Request Origin :" + origin);
            log.LogInformation("Request Referrer :" + referrer);

            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-NAME", out origin);

            log.LogInformation("Requestor Name : " + origin);

            GetPartnerData getPartnerData = new GetPartnerData();
            var dataOutput = getPartnerData.getPartnerOfAPartner(groupId, log);
            log.LogInformation("Requestor Name : " + dataOutput);

            string responseMessage = JsonConvert.SerializeObject(dataOutput);

            return new OkObjectResult(responseMessage);
        }
    }

}
