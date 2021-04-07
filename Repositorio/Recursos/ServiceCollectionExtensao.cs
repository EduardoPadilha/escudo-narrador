using EscudoNarrador.Repositorio.Configuracoes;
using EscudoNarrador.Repositorio.Repositorios;
using EscudoNarrador.Shared.Abstracoes.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EscudoNarrador.Repositorio.Recursos
{
    public static class ServiceCollectionExtensao
    {
        public static IServiceCollection AddOpcoes(this IServiceCollection servicos)
        {
            servicos.AddOptions<ConexaoOpcoes>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(ConexaoOpcoes)).Bind(settings);
                });

            return servicos;
        }

        public static IServiceCollection AddRepositorios(this IServiceCollection servicos)
        {
            servicos.AddSingleton<CosmosContexto>();
            servicos.AddSingleton<ICaracteristicaRepositorio, CaracteristicaRepositorio>();

            return servicos;
        }
    }
}
