using Teste.Unidade.Recursos;
using Xunit.Abstractions;

namespace Teste.Unidade
{
    public class TesteBase
    {
        protected readonly ITestOutputHelper Console;
        public TesteBase(ITestOutputHelper saida)
        {
            this.Console = saida;
            ContainerDI.Inicializar();
            //ConfiguracaoRecursos.AdicionarRecurso(typeof(Properties.Resources));
            //Configuracao.DisplayNameExtrator = () => new DisplayNameExtratorPadrao();
        }
    }
}
