using AutoMapper.Extensions.ExpressionMapping;
using EscudoNarrador.Dominio.Recursos;

namespace EscudoNarrador.Repositorio.Recursos
{
    public class RepositorioGestorAutoMapper : DominioGestorAutoMapper<RepositorioGestorAutoMapper>
    {
        public RepositorioGestorAutoMapper()
        {
            AdicionarConfig(cfg => cfg.AddExpressionMapping());
            AdicionarPerfil<PerfilMapeamento>();
        }
    }
}
