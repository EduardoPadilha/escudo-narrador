using EscudoNarrador.Dominio.Recursos;
using EscudoNarrador.Repositorio.Recursos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Recursos;

[assembly: FunctionsStartup(typeof(EscudoNarrador.Api.Startup))]
namespace EscudoNarrador.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOpcoes()
                .AddAutoMapper()
                .AddRepositorios()
                .AddRecursos()
                .AddServicos()
                .AddGestorDependenciaAspnetPadrao();
        }
    }
}
