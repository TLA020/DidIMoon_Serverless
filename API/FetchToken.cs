using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api
{
    public static class FetchToken
    {
        [FunctionName("FetchToken")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var coinId = req.Query["coinId"];
            var date = req.Query["date"];
            var currency = req.Query["currency"];

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.cryptocurrencychart.com");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));
            httpClient.DefaultRequestHeaders.Add("Secret", Environment.GetEnvironmentVariable("API_SECRET"));

            var jsonRes = await httpClient.GetAsync($"/api/coin/view/{coinId}/{date}/{currency}");

            if (!jsonRes.IsSuccessStatusCode)
            {
                log.LogError("Something went wrong {r}", jsonRes.ReasonPhrase);
                return new StatusCodeResult(500);
            }

            var response = JsonConvert.DeserializeObject<CoinInfoResponse>(jsonRes.Content.ReadAsStringAsync().Result);
            return new OkObjectResult(response.Coin);
        }
    }
}

