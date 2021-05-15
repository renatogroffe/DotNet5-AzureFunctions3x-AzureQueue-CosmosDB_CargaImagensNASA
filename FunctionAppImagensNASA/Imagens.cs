using System.Linq;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppImagensNASA.Data;

namespace FunctionAppImagensNASA
{
    public static class Imagens
    {
        [Function("Imagens")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Imagens");
            var dados = ImagensNASARepository.GetAll();
            logger.LogInformation($"Número de cargas realizadas: {dados.Count()}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(dados).AsTask().Wait();
            return response;
        }
    }
}