using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Repositorio.Abstracoes;
using EscudoNarrador.Repositorio.Configuracoes;
using EscudoNarrador.Repositorio.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EscudoNarrador.Repositorio.Recursos
{
    public static class ServicoExtensao
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
            servicos.AddSingleton<ICosmosTableContexto, CosmosTableContextoBase>();
            servicos.AddSingleton<ITermoRepositorio, TermoRepositorio>();

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
