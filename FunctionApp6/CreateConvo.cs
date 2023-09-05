using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FunctionApp6.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using FunctionApp6.Model;

namespace FunctionApp6
{
    public static class CreateNewConvo
    {
        [FunctionName("createNewConvo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Microsoft.Extensions.Primitives.StringValues requester;
            req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-NAME", out requester);

            log.LogInformation(requester.ToString());

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ConvoRequest deserialize = JsonConvert.DeserializeObject<ConvoRequest>(requestBody);
            GetConvoData getConvoData = new GetConvoData();


            bool newConvo = getConvoData.CreateConvo(deserialize, log);

            string responseMessage = newConvo == true ? "ok": "error";

            return new OkObjectResult(responseMessage);
        }
    }
}