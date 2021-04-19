using System;
using System.ComponentModel;

namespace EscudoNarrador.Entidade.Enums
{
    [Flags]
    public enum TipoTermo
    {
        Ponto = 1,
        [Description("Expansível")]
        Expansivel = 2,
        Fixo = 4,
        [Description("Consumível")]
        Consumivel = 16,
        Informativa = 32
    }
}
