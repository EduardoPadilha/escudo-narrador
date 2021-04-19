using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Gestores;

namespace EscudoNarrador.Fronteira.Recursos
{
    public class FronteiraGestorAutoMapper<T> : GestorMapeamento<T> where T : IGestorMapeamento, new()
    {
        public FronteiraGestorAutoMapper()
        {
            AdicionarPerfil<PerfilMapeamento>();
        }
    }
}
