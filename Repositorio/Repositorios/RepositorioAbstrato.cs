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
    public abstract class RepositorioAbstrato<TEntidade> :
        RepositorioBase<TEntidade>, IRepositorio<TEntidade>
        where TEntidade : class, ITableEntity, new()
    {
        protected RepositorioAbstrato(ICosmosTableContexto contexto, ILogger<TEntidade> log) :
            base(contexto, log)
        {
        }

        public virtual async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            try
            {
                var operacaoAdd = TableOperation.Insert(entidade);
                return await ExecutarAsync(operacaoAdd);
            }
            catch (StorageException e)
            {
                if (e.Message.Contains("The specified entity already exists"))
                    throw new UnicidadeException(entidade.RowKey);
                throw;
            }
        }

        public virtual async Task<TEntidade> AtualizarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            entidade.ETag = "*";
            var operacaoMerge = TableOperation.Merge(entidade);
            return await ExecutarAsync(operacaoMerge);
        }

        public virtual async Task DeletarAsync(string chaveParticao, string chaveLinha)
        {
            var entidade = CriarInstancia(chaveParticao, chaveLinha);
            entidade.ETag = "*";
            var operacoDelete = TableOperation.Delete(entidade);
            await ExecutarAsync(operacoDelete);
        }

        public virtual async Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha)
        {
            var operacoBusca = TableOperation.Retrieve<TEntidade>(chaveParticao, chaveLinha);
            var entidade = await ExecutarAsync(operacoBusca);
            if (entidade == null)
                throw new RecursoNaoEncontradoException();

            log.LogInformation(JsonConvert.SerializeObject(entidade));

            return entidade;
        }

        public virtual IEnumerable<TEntidade> ObterTodos<T>(string chaveParticao, IFiltro<T> filtro)
        {
            return ObterTodos(chaveParticao, filtro.ObterPredicados());
        }

        public virtual IEnumerable<TEntidade> ObterTodos<T>(string chaveParticao, Expression<Func<T, bool>> predicado)
        {
            var todos = ObterTodos(chaveParticao);

            if (todos.AnySafe())
                return Enumerable.Empty<TEntidade>();

            var predicadoConvertido = predicado.ConvertePredicado<T, TEntidade>();
            var consulta = todos.Where(predicadoConvertido).ToList();
            var resultado = consulta.Como<IEnumerable<TEntidade>>();

            return resultado;
        }
    }
}
