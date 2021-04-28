using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscudoNarrador.Api.Funcoes
{
    public static class SegurancaApi
    {
        [FunctionName("SegurancaApi")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "seguranca")] HttpRequest req,
            ILogger log, ClaimsPrincipal principal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var autenticado = false;
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                log.LogInformation("Claims: Not authenticated");
            }
            else
            {
                log.LogInformation("Claims: Authenticated as " + principal.Identity.Name);
                autenticado = true;
            }


            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name ?? "Fulano";
            var termo = autenticado ? "já" : "não";
            string responseMessage = $"Fale, {name}. Você {termo} está autenticado como [{principal.Identity.Name}].";

            return new OkObjectResult(responseMessage);
        }
    }
}

