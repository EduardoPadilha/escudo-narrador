using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Entidade;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;

namespace EscudoNarrador.Repositorio.Repositorios
{
    public class TermoConcusltaRepositorio : ConsultaRepositorio<Termo>, ITermoConsultaRepositorio
    {
        public TermoConcusltaRepositorio(IMongoContexto contexto) : base(contexto)
        {
        }
    }
}
