using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models;

namespace Api
{
    public static class FetchTokenList
    {
        [FunctionName("FetchTokenList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var rootDirectory = context.FunctionAppDirectory;
            var filePath = Path.Combine(rootDirectory, "coinlist.json");
            var content = File.ReadAllText(filePath);

            var coinList = JsonConvert.DeserializeObject<List<CryptoCurrency>>(content);

            return new OkObjectResult(coinList);
        }
    }
}

