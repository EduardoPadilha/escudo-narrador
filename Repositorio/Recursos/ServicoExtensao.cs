using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Repositorio.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Behemoth.Mongo.Abstracoes;

namespace EscudoNarrador.Repositorio.Recursos
{
    public static class ServicoExtensao
    {
        public static IServiceCollection AddRepositorios(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IMongoContexto, Contexto>();
            servicos.AddSingleton<ITermoComandoRepositorio, TermoComandoRepositorio>();
            servicos.AddSingleton<ITermoConsultaRepositorio, TermoConcusltaRepositorio>();

            return servicos;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection servicos)
        {
            RepositorioGestorAutoMapper.Inicializar();
            servicos.AddSingleton(sp => RepositorioGestorAutoMapper.Instancia.Mapper);

            return servicos;
        }
    }
}
