using System;

namespace EscudoNarrador.Shared.Enums
{
    [Flags]
    public enum TipoCaracteristica
    {
        Ponto = 1,
        Expansivel = 2,
        Fixo = 4,
        Consumivel = 16,
        Informativa = 32
    }
}
