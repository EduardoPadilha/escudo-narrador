using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Dominio.Excecoes;
using EscudoNarrador.Entidade;
using EscudoNarrador.Fronteira.DTOs.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Excecoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        [OpenApiOperation(operationId: nameof(ObterTermos), tags: new[] { "Termos" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "query", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A **Query** da consulta")]
        [OpenApiParameter(name: "sistema", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "O **Sistema** da busca")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<TermoDTO>), Summary = "operação bem sucedida", Description = "operação bem sucedida")]
        public async Task<IActionResult> ObterTermos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "termo")]
        HttpRequest req, ILogger log)
        {
            var query = req.Query.Obter<string>("query");
            var sistema = req.Query.Obter<string>("sistema");
            try
            {
                var resultado = await servico.ObterTodosAsync(sistema, query);
                return RetornarSucesso<IEnumerable<TermoDTO>>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        [FunctionName(nameof(ObterTermoPorChaves))]
        [OpenApiOperation(operationId: nameof(ObterTermoPorChaves), tags: new[] { "Termos" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "o **nome** termo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TermoDTO), Summary = "operação bem sucedida", Description = "operação bem sucedida")]
        public async Task<IActionResult> ObterTermoPorChaves(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "termo/{id}")]
        HttpRequest req, ILogger log, string id)
        {
            try
            {
                var resultado = await servico.ObterAsync(id); ;
                return RetornarSucesso<TermoDTO>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (RecursoNaoEncontradoExcecao ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        [FunctionName(nameof(SalvarTermo))]
        public async Task<IActionResult> SalvarTermo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "termo/{id?}")]
        HttpRequest req, ILogger log, string id)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var post = HttpMethods.IsPost(req.Method);
                var resultado = await SalvarAsync(content, post, id);

                return RetornarSucesso<TermoDTO>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        private async Task<Termo> SalvarAsync(string body, bool post, string id)
        {
            var dto = JsonConvert.DeserializeObject<TermoDTO>(body);
            var entidade = dto.Como<Termo>();
            if (post)
                return await servico.AdicionarAsync(entidade);

            entidade.Id = id;
            return await servico.AtualizarAsync(entidade);
        }

        [FunctionName(nameof(DeletarTermo))]
        public async Task<IActionResult> DeletarTermo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "termo/{id}")]
        HttpRequest req, ILogger log, string id)
        {
            try
            {
                await servico.DeletarAsync(id);
                return new OkResult();
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        private IActionResult RetornaFalha(ILogger log, string mensagem)
        {
            log.LogError(mensagem);
            return new BadRequestObjectResult(mensagem);
        }

        private IActionResult RetornarSucesso<T>(object resultado)
        {
            return new OkObjectResult(resultado.Como<T>());
        }
    }
}
