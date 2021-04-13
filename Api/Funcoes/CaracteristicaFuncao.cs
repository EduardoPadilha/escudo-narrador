using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Repositorio.Excecoes;
using EscudoNarrador.Shared.Abstracoes.Repositorios;
using EscudoNarrador.Shared.Dtos.Entrada;
using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EscudoNarrador.Api.Funcoes
{
    public class CaracteristicaFuncao
    {
        private readonly ICaracteristicaRepositorio repositorio;
        public CaracteristicaFuncao(ICaracteristicaRepositorio caracteristicaRepositorio)
        {
            this.repositorio = caracteristicaRepositorio;
        }

        [FunctionName(nameof(ObterCaracteristas))]
        public IActionResult ObterCaracteristas(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "caracteristica")] HttpRequest req,
            ILogger log)
        {
            var nome = req.Query.Obter<string>("nome");
            var tagsParam = req.Query.Obter<string>("tags");
            tagsParam = string.IsNullOrWhiteSpace(tagsParam) ? null : tagsParam;
            var separador = tagsParam?.IndexOf(";") > 0 ? ";" : ",";
            var tags = tagsParam?.Split(separador).Select(c => c.Trim()).ToList() ?? new List<string>();
            try
            {
                var resultado = repositorio.ObterTodos(nome, TipoSistema.Storyteller, tags.ToArray());
                return new OkObjectResult(resultado);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
        }

        [FunctionName(nameof(ObterCaracteristaPorId))]
        public async Task<IActionResult> ObterCaracteristaPorId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "caracteristica/{nome}")] HttpRequest req, string nome,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                var argumentMsg = $"Erro em {nameof(ObterCaracteristaPorId)}, id da característica deve vir no path da requisição.";
                log.LogError(argumentMsg);
                return new BadRequestObjectResult(new { error = argumentMsg });
            }
            Caracteristica result;
            try
            {
                result = await repositorio.ObterAsync(nome, TipoSistema.Storyteller); ;
            }
            catch (RecursoNaoEncontradoException)
            {
                return new NotFoundObjectResult("Característica não encontrada");
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
            return new OkObjectResult(result);
        }

        [FunctionName(nameof(AdicionarCaracteristica))]
        public async Task<IActionResult> AdicionarCaracteristica(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "caracteristica")] HttpRequest req, ILogger log)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            var dto = JsonConvert.DeserializeObject<AddCaracteristicaDto>(content);
            try
            {
                var entidade = dto.ParaEntidade();
                Caracteristica resultado;
                if (HttpMethods.IsPost(req.Method))
                    resultado = await repositorio.AdicionarAsync(entidade);
                else resultado = await repositorio.AtualizarAsync(entidade);
                return new OkObjectResult(resultado);
            }
            catch (Exception e)
            {
                var mensagem = e.GetBaseException().Message;
                log.LogError(mensagem);
                return new BadRequestObjectResult(mensagem);
            }
        }

        [FunctionName(nameof(DeletarCaracterista))]
        public async Task<IActionResult> DeletarCaracterista(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "caracteristica/{nome}")] HttpRequest req, string nome,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                var argumentMsg = $"Erro em {nameof(ObterCaracteristaPorId)}, id da característica deve vir no path da requisição.";
                log.LogError(argumentMsg);
                return new BadRequestObjectResult(new { error = argumentMsg });
            }
            try
            {
                await repositorio.DeletarAsync(nome, TipoSistema.Storyteller); ;
            }
            catch (RecursoNaoEncontradoException)
            {
                return new NotFoundObjectResult("Característica não encontrada");
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
