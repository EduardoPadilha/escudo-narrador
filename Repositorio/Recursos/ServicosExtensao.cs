using EscudoNarrador.Dominio.Abstracoes.Repositorios;
using EscudoNarrador.Repositorio.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;

namespace EscudoNarrador.Repositorio.Recursos
{
    public static class ServicosExtensao
    {
        public static IServiceCollection AddRepositorios(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IDbConfiguracao, DBConfiguracaoPadrao>();
            servicos.AddSingleton<IMongoContexto, Contexto>();

            servicos.AddScopedTodosPorInterface(typeof(IComandoRepositorioBase<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IComandoRepositorio<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IConsultaRepositorio<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IConsultaRepositorioBase<>), typeof(ServicosExtensao));

            servicos.AddSingleton<ITermoComandoRepositorio, TermoComandoRepositorio>();
            servicos.AddSingleton<ITermoConsultaRepositorio, TermoConsultaRepositorio>();

            servicos.AddSingleton<ISistemaComandoRepositorio, SistemaComandoRepositorio>();
            servicos.AddSingleton<ISistemaConsultaRepositorio, SistemaConsultaRepositorio>();

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
