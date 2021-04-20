using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Entidade;
using EscudoNarrador.Fronteira.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Excecoes;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EscudoNarrador.Api.Funcoes
{
    public class TermoApi
    {
        private readonly ITermoServico servico;
        public TermoApi(ITermoServico servico)
        {
            this.servico = servico;
        }

        [FunctionName(nameof(ObterTermos))]
        public IActionResult ObterTermos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{sistema}/termo")]
        HttpRequest req, ILogger log, string sistema)
        {
            var nome = req.Query.Obter<string>("nome");
            var tags = req.Query.Obter<string>("tags");
            try
            {
                var resultado = servico.ObterTodosAsync(sistema, nome, tags);
                return new OkObjectResult(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                var mensagem = JsonConvert.SerializeObject(e.Erros);
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
        }

        [FunctionName(nameof(ObterTermoPorChaves))]
        public async Task<IActionResult> ObterTermoPorChaves(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{sistema}/termo/{nome}")]
        HttpRequest req, ILogger log, string sistema, string nome)
        {
            Termo result;
            try
            {
                result = await servico.ObterAsync(sistema, nome); ;
            }
            catch (ValidacaoExcecao e)
            {
                var mensagem = JsonConvert.SerializeObject(e.Erros);
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            return new OkObjectResult(result);
        }

        [FunctionName(nameof(SalvarTermo))]
        public async Task<IActionResult> SalvarTermo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "{sistema}/termo")]
        HttpRequest req, ILogger log, string sistema)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            var dto = JsonConvert.DeserializeObject<TermoDTO>(content);
            try
            {
                var entidade = dto.Como<Termo>();
                entidade.Sistema = sistema;
                Termo resultado;
                if (HttpMethods.IsPost(req.Method))
                    resultado = await servico.AdicionarAsync(entidade);
                else resultado = await servico.AtualizarAsync(entidade);
                return new OkObjectResult(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                var mensagem = JsonConvert.SerializeObject(e.Erros);
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
        }

        [FunctionName(nameof(DeletarTermo))]
        public async Task<IActionResult> DeletarTermo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "{sistema}/termo/{nome}")]
        HttpRequest req, ILogger log, string sistema, string nome)
        {
            try
            {
                await servico.DeletarAsync(sistema, nome);
            }
            catch (ValidacaoExcecao e)
            {
                var mensagem = JsonConvert.SerializeObject(e.Erros);
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            return new OkResult();
        }
    }
}
