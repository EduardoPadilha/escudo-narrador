using EscudoNarrador.Fronteira.Recursos;
using Nebularium.Tarrasque.Abstracoes;

namespace EscudoNarrador.Dominio.Recursos
{
    public class DominioGestorAutoMapper<T> : FronteiraGestorAutoMapper<T> where T : IGestorMapeamento, new()
    {
        public DominioGestorAutoMapper()
        {
            AdicionarPerfil<PerfilMapeamento>();
        }
    }
}
