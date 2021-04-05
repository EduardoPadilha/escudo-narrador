using EscudoNarrador.Repositorio.Recursos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EscudoNarrador.Api.Startup))]
namespace EscudoNarrador.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOpcoes()
                .AddRepositorios();
        }
    }
}
