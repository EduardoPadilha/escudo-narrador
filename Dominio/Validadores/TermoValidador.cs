using EscudoNarrador.Entidade;
using FluentValidation;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Validacoes;

namespace EscudoNarrador.Dominio.Validadores
{
    public class TermoValidador : ValidadorAbstrato<Termo>
    {
        public TermoValidador(IDisplayNameExtrator displayNameExtrator) : base(displayNameExtrator)
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .Length(3, 150);

            RuleFor(p => p.Sistema)
                .NotEmpty();

            RuleFor(p => p.Tags)
                .NotEmpty();

            RuleFor(p => p.Pontos)
                .GreaterThan(0);
        }
    }
}
