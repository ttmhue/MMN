using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunctionApp6.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;

namespace FunctionApp6
{
    public static class GetListPartnerOfAllGroup
    {
        [FunctionName("getListPartnerOfAllGroup")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing Partner data Azure Function");

            GetPartnerData getPartnerData = new GetPartnerData();
            string json = getPartnerData.GetGroupedPartnersJson(log);
            if (json == string.Empty)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(json);
            }
        }
    }
}