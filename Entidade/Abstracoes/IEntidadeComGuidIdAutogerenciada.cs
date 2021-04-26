using System;

namespace EscudoNarrador.Entidade.Abstracoes
{
    public interface IEntidadeComGuidIdAutogerenciada
    {
        Guid Id { get; set; }
    }
}
