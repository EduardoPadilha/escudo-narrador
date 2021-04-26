using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tiamat.Abstracoes;

namespace EscudoNarrador.Dominio.Recursos
{
    public static class ServicosExtensao
    {
        public static IServiceCollection AddRecursos(this IServiceCollection servicos)
        {
            servicos.AddSingletonTodosPorInterface(typeof(IValidador<>), typeof(ServicosExtensao));
            servicos.AddSingleton<IDisplayNameExtrator>(sp => new DisplayNameExtratorPadrao());

            return servicos;
        }
        public static IServiceCollection AddServicos(this IServiceCollection servicos)
        {
            servicos.AddScopedTodosPorInterface(typeof(IServicoCrud<>), typeof(ServicosExtensao));
            servicos.AddScoped<ITermoServico, TermoServico>();
            servicos.AddScoped<ISistemaServico, SistemaServico>();

            return servicos;
        }
    }
}
