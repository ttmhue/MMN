using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunctionApp6.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;

namespace FunctionApp6
{
    public static class CreateConvo
    {
        [FunctionName("getListConvoOfPartnerGroup")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing Partner data Azure Function");

            if (!int.TryParse(req.Query["PartnersGroup"], out int partnersGroup))
            {
                return new BadRequestObjectResult("Invalid or missing 'PartnersGroup' parameter.");
            }

            GetConvoData getConvoData = new GetConvoData();

            string json = getConvoData.GetGroupedConvoJson(log, partnersGroup);
            if (json == string.Empty)
            {
                return new NotFoundObjectResult("No data found for the specified partner group.");
            }
            return new OkObjectResult(json);
        }
    }
}