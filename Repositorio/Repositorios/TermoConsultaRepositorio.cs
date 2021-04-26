using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Entidade;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class TermoConsultaRepositorio : ConsultaRepositorio<Termo>, ITermoConsultaRepositorio
    {
        public TermoConsultaRepositorio(IMongoContexto contexto) : base(contexto)
        {
        }
        public override IOrderedMongoQueryable<Termo> OrdernarPadrao(IMongoQueryable<Termo> query)
        {
            return query.OrderBy(c => c.Nome);
        }
    }
}
