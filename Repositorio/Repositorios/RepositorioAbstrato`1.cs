using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Repositorio.Abstracoes;
using EscudoNarrador.Repositorio.Excecoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public abstract class RepositorioAbstrato<TEntidade, TMapeamento> :
        RepositorioBase<TMapeamento>, IRepositorio<TEntidade>
        where TMapeamento : class, ITableEntity, new()
        where TEntidade : class, new()
    {
        protected RepositorioAbstrato(ICosmosTableContexto contexto, ILogger<TMapeamento> log) :
            base(contexto, log)
        {
        }

        public virtual async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var mapeamento = entidade.Como<TMapeamento>();
            try
            {
                var operacaoAdd = TableOperation.Insert(mapeamento);
                return await ExecutarAsync<TEntidade>(operacaoAdd);
            }
            catch (StorageException e)
            {
                if (e.Message.Contains("The specified entity already exists"))
                    throw new UnicidadeException(mapeamento.RowKey);
                throw;
            }
        }

        public virtual async Task<TEntidade> AtualizarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var mapeamento = entidade.Como<TMapeamento>();
            mapeamento.ETag = "*";
            var operacaoMerge = TableOperation.Merge(mapeamento);
            return await ExecutarAsync<TEntidade>(operacaoMerge);
        }

        public virtual async Task DeletarAsync(string chaveParticao, string chaveLinha)
        {
            var mapeamento = CriarInstancia(chaveParticao, chaveLinha);
            mapeamento.ETag = "*";
            var operacoDelete = TableOperation.Delete(mapeamento);
            await ExecutarAsync<TEntidade>(operacoDelete);
        }

        public virtual async Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha)
        {
            var operacoBusca = TableOperation.Retrieve<TMapeamento>(chaveParticao, chaveLinha);
            var entidade = await ExecutarAsync<TEntidade>(operacoBusca);
            if (entidade == null)
                throw new RecursoNaoEncontradoException();

            log.LogInformation(JsonConvert.SerializeObject(entidade));

            return entidade;
        }

        public virtual IEnumerable<TEntidade> ObterTodos<T>(IFiltro<T> filtro)
        {
            return ObterTodos(filtro.ObterPredicados());
        }

        public virtual IEnumerable<TEntidade> ObterTodos<T>(Expression<Func<T, bool>> predicado)
        {
            var todos = ObterTodos();

            if (todos == null)
                throw new RecursoNaoEncontradoException();

            var predicadoConvertido = predicado.ConvertePredicado<T, TMapeamento>();
            var consulta = todos.Where(predicadoConvertido).ToList();

            return ConverteSeguro(consulta);
        }

        public IEnumerable<TEntidade> ObterTodos<T>()
        {
            var todos = ObterTodos().ToList();
            return ConverteSeguro(todos);
        }

        private IEnumerable<TEntidade> ConverteSeguro(IEnumerable<TMapeamento> lista)
        {
            if (lista.AnySafe())
                return lista.Como<IEnumerable<TEntidade>>();

            return Enumerable.Empty<TEntidade>();
        }
    }
}
