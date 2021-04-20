using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Excecoes;
using EscudoNarrador.Entidade;
using EscudoNarrador.Fronteira.DTOs;
using EscudoNarrador.Repositorio.Excecoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EscudoNarrador.Api.Funcoes
{
    public class TermoApi
    {
        private const string MSG_NAO_ENCONTRADO = "Erro em {0}, a chave do termo deve vir no path da requisição.";
        private const string MSG_SISTEMA_NAO_ENCONTRADO = "O id do sistema deve ser informado.";
        private readonly ITermoServico servico;
        public TermoApi(ITermoServico servico)
        {
            this.servico = servico;
        }

        [FunctionName(nameof(ObterTermos))]
        public IActionResult ObterTermos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{sistema}/termo")]
        HttpRequest req, ILogger log, Guid sistema)
        {
            var nome = req.Query.Obter<string>("nome");
            var tags = req.Query.Obter<string>("tags");
            try
            {
                var resultado = servico.ObterTodos(sistema, nome, tags);
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
        HttpRequest req, ILogger log, Guid sistema, string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                var argumentoMsg = string.Format(MSG_NAO_ENCONTRADO, nameof(ObterTermoPorChaves));
                log.LogError(argumentoMsg);
                return new BadRequestObjectResult(new { erro = argumentoMsg });
            }
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
            catch (RecursoNaoEncontradoException)
            {
                return new NotFoundObjectResult("Termo não encontrado");
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
        HttpRequest req, ILogger log, Guid sistema)
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
        HttpRequest req, ILogger log, Guid sistema, string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                var argumentMsg = string.Format(MSG_NAO_ENCONTRADO, nameof(DeletarTermo)); ;
                log.LogError(argumentMsg);
                return new BadRequestObjectResult(new { erro = argumentMsg });
            }
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
