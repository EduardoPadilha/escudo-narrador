using EscudoNarrador.Repositorio.Abstracoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public abstract class RepositorioBase<TEntidade>
        where TEntidade : class, ITableEntity, new()
    {
        protected readonly ICosmosTableContexto contexto;
        protected readonly CloudTable tabela;
        protected readonly ILogger<TEntidade> log;

        protected RepositorioBase(ICosmosTableContexto contexto, ILogger<TEntidade> log)
        {
            this.contexto = contexto;
            this.log = log;
            this.tabela = NomeTabela.limpoNuloBrancoOuZero() ?
                contexto.ObterTabela<TEntidade>() :
                contexto.ObterTabela(NomeTabela);
        }

        protected virtual string NomeTabela => typeof(TEntidade).Name.SnakeCase();

        protected async Task<TResultado> ExecutarAsync<TResultado>(TableOperation operacao) where TResultado : class
        {
            var resultado = await ExecutarAsync(operacao);
            var entidade = resultado;
            var retorno = entidade.Como<TResultado>();
            return retorno;
        }

        protected async Task<TEntidade> ExecutarAsync(TableOperation operacao)
        {
            try
            {
                var resultado = await tabela.ExecuteAsync(operacao);
                return resultado.Result as TEntidade;
            }
            catch (StorageException e)
            {
                log.LogError(e, $"Erro ao gerenciar a tabela {NomeTabela}");
                throw;
            }
            catch (Exception e)
            {
                log.LogError(e, "Erro desconhecido");
                throw;
            }
        }

        protected TEntidade CriarInstancia(string chaveParticao, string chaveLinha)
        {
            var entidade = (TEntidade)Activator.CreateInstance(typeof(TEntidade));
            entidade.PartitionKey = chaveParticao;
            entidade.RowKey = chaveLinha;
            return entidade;
        }

        #region Suporte LINQ Querys

        protected virtual IQueryable<TEntidade> ObterTodos()
        {
            var todos = tabela.CreateQuery<TEntidade>().Where(x => true == true);
            return todos;
        }

        //protected virtual IQueryable<TEntidade> OrdernarPadrao(IQueryable<TEntidade> query)
        //{
        //    return query.OrderBy(z => z.Timestamp);
        //}

        #endregion
    }
}
