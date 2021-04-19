using EscudoNarrador.Dominio.Abstracoes;
using EscudoNarrador.Dominio.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tiamat.Abstracoes;

namespace EscudoNarrador.Dominio.Recursos
{
    public static class ServicoExtensao
    {
        public static IServiceCollection AddRecursos(this IServiceCollection servicos)
        {
            servicos.AddSingletonTodosPorInterface(typeof(IValidador<>), typeof(ServicoExtensao));
            servicos.AddSingleton<IDisplayNameExtrator>(sp => new DisplayNameExtratorPadrao());

            return servicos;
        }
        public static IServiceCollection AddServicos(this IServiceCollection servicos)
        {
            servicos.AddScoped<ITermoServico, TermoServico>();

            return servicos;
        }
    }
}
