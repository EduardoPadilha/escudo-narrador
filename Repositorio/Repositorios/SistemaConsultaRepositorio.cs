using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Entidade;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class SistemaConsultaRepositorio : ConsultaRepositorio<Sistema>, ISistemaConsultaRepositorio
    {
        public SistemaConsultaRepositorio(IMongoContexto contexto) : base(contexto)
        {
        }
        public override IOrderedMongoQueryable<Sistema> OrdernarPadrao(IMongoQueryable<Sistema> query)
        {
            return query.OrderBy(c => c.Nome);
        }
    }
}
