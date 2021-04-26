using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Excecoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Excecoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EscudoNarrador.Api.Funcoes
{
    public abstract class FuncaoCrud<TEntidade, TDto>
        where TEntidade : Nebularium.Tiamat.Entidades.Entidade, new()
        where TDto : class, new()
    {
        protected readonly IServicoCrud<TEntidade> servico;

        public FuncaoCrud(IServicoCrud<TEntidade> servico)
        {
            this.servico = servico;
        }

        public virtual Task<IActionResult> EntradaCrud(HttpRequest req, ILogger log, string id)
        {
            var verbo = Enum.Parse<VerboHTTP>(req.Method);
            switch (verbo)
            {
                case VerboHTTP.GET:
                    var todos = id.limpoNuloBrancoOuZero();
                    return todos ? ObterTodos(req, log) : ObterPorId(log, id);
                case VerboHTTP.POST:
                    return Adicionar(req, log);
                case VerboHTTP.PUT:
                    return Atualizar(req, log, id);
                case VerboHTTP.DELETE:
                    return Deletar(log, id);
                default:
                    return Task.FromResult(RetornaFalha(log, "O recurso que você procura não existe"));
            }
        }

        protected abstract IFiltro<TEntidade> CriarFiltro(HttpRequest req);

        public virtual async Task<IActionResult> ObterTodos(HttpRequest req, ILogger log)
        {
            try
            {
                var filtro = CriarFiltro(req);
                var resultado = await servico.ObterTodosAsync(filtro);
                return RetornarSucesso<IEnumerable<TDto>>(resultado);
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

        public virtual async Task<IActionResult> ObterPorId(ILogger log, string id)
        {
            try
            {
                var resultado = await servico.ObterAsync(id); ;
                return RetornarSucesso<TDto>(resultado);
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

        public async Task<IActionResult> Adicionar(HttpRequest req, ILogger log)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, true);
                return RetornarSucesso<TDto>(resultado);
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

        public async Task<IActionResult> Atualizar(HttpRequest req, ILogger log, string id)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, false, id);
                return RetornarSucesso<TDto>(resultado);
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

        public virtual async Task<IActionResult> Deletar(ILogger log, string id)
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

        protected virtual async Task<TEntidade> SalvarAsync(string body, bool novo, string id = null)
        {
            var dto = JsonConvert.DeserializeObject<TDto>(body);
            var entidade = dto.Como<TEntidade>();
            if (novo)
                return await servico.AdicionarAsync(entidade);

            entidade.Id = id;
            return await servico.AtualizarAsync(entidade);
        }

        protected virtual IActionResult RetornaFalha(ILogger log, string mensagem)
        {
            log.LogError(mensagem);
            return new BadRequestObjectResult(mensagem);
        }

        protected virtual IActionResult RetornarSucesso<T>(object resultado)
        {
            return new OkObjectResult(resultado.Como<T>());
        }
    }
}
