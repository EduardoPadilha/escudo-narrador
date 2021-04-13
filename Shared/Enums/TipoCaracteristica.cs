using System;
using System.ComponentModel;

namespace EscudoNarrador.Shared.Enums
{
    [Flags]
    public enum TipoCaracteristica
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
