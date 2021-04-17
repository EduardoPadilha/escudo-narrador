using EscudoNarrador.Dominio.Recursos;
using EscudoNarrador.Repositorio.Recursos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Gestores;
using System;

namespace Teste.Unidade.Recursos
{
    public class ContainerDI : GestorDependencia<ContainerDI>
    {
        internal protected IServiceProvider Container { get; }
        private readonly ServiceCollection Servicos;

        public ContainerDI()
        {
            if (Servicos == null)
                Servicos = new ServiceCollection();

            IConfiguration configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Servicos.AddSingleton(sp => configuracao);

            Servicos
                .AddOpcoes()
                .AddAutoMapper()
                .AddRepositorios()
                .AddServicos();

            Container = Servicos.BuildServiceProvider();//(new ServiceProviderOptions { ValidateOnBuild = true });
        }


        public void ConfiguracaoesAdicionais(Action<IServiceProvider> configuracoesAdicionais)
        {
            configuracoesAdicionais(Container);
        }

        public override object ObterInstancia(Type tipo)
        {
            return Container.GetService(tipo);
        }

        public override TInstancia ObterInstancia<TInstancia>()
        {
            return Container.GetService<TInstancia>();
        }
    }
}
