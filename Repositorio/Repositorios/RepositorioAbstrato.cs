using EscudoNarrador.Repositorio.Abstracoes;
using EscudoNarrador.Repositorio.Excecoes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public abstract class RepositorioAbstrato<T, TMapeamento> 
        where TMapeamento : MapeamentoAbstrato<T>, new() where T : class
    {
        private readonly CosmosContexto contexto;
        private readonly CloudTable tabela;
        private readonly ILogger<CaracteristicaRepositorio> log;

        protected RepositorioAbstrato(CosmosContexto contexto, ILogger<CaracteristicaRepositorio> log)
        {
            this.contexto = contexto;
            this.log = log;
            this.tabela = contexto.ObterTabela(NomeTabela);
        }

        public abstract string NomeTabela { get; }

        public async Task<T> AdicionarAsync(T entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var entidadeTable = ParaMapeamento(entidade);
            try
            {
                var operacaoAdd = TableOperation.Insert(entidadeTable);
                return await ExecutarAsync<T>(operacaoAdd);
            }
            catch (StorageException e)
            {
                if (e.Message.Contains("The specified entity already exists"))
                    throw new UnicidadeException(entidadeTable.RowKey);
                throw;
            }
        }

        public async Task<T> AtualizarAsync(T entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var entidadeTable = ParaMapeamento(entidade);
            entidadeTable.ETag = "*";
            var operacaoMerge = TableOperation.Merge(entidadeTable);
            return await ExecutarAsync<T>(operacaoMerge);
        }

        public async Task DeletarAsync(string chaveParticao, string chaveLinha)
        {
            var entidadeTable = CriarMapeamento(chaveParticao, chaveLinha);
            entidadeTable.ETag = "*";
            var operacoDelete = TableOperation.Delete(entidadeTable);
            await ExecutarAsync<T>(operacoDelete);
        }

        public async Task<T> ObterAsync(string chaveParticao, string chaveLinha)
        {
            var operacoBusca = TableOperation.Retrieve<TMapeamento>(chaveParticao, chaveLinha);
            var entidadeTable = await ExecutarAsync<TMapeamento>(operacoBusca);
            if (entidadeTable == null)
                throw new RecursoNaoEncontradoException();

            log.LogInformation(JsonConvert.SerializeObject(entidadeTable));

            return entidadeTable.ParaEntidade();
        }

        public List<T> ObterTodos(string chaveParticao, Func<TMapeamento, bool> filtros)
        {
            var query = tabela.CreateQuery<TMapeamento>()
                .Where(c => c.PartitionKey == chaveParticao).ToList();

            if (query == null)
                throw new RecursoNaoEncontradoException();

            var consulta = query.Where(filtros);
            var caracteristicas = consulta.ToList().ConvertAll(c => c.ParaEntidade());

            return caracteristicas;
        }

        #region Suportes

        private async Task<TResult> ExecutarAsync<TResult>(TableOperation operacao) where TResult : class
        {
            var resultado = await ExecutarAsync(operacao);
            var entidadeTabela = resultado as TResult;

            return entidadeTabela;
        }

        private async Task<object> ExecutarAsync(TableOperation operacao)
        {
            try
            {
                var resultado = await tabela.ExecuteAsync(operacao);
                return resultado.Result;
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

        private TMapeamento ParaMapeamento(T entidade)
        {
            return (TMapeamento)Activator.CreateInstance(typeof(TMapeamento), entidade);
        }

        private TMapeamento CriarMapeamento(string chaveParticao, string chaveLinha)
        {
            return (TMapeamento)Activator.CreateInstance(typeof(TMapeamento), chaveParticao, chaveLinha);
        }

        #endregion
    }
}
