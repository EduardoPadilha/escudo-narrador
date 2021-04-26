using EscudoNarrador.Entidade;
using FluentValidation;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Validacoes;

namespace EscudoNarrador.Dominio.Validadores
{
    public class VersaoValidador : ValidadorAbstrato<Versao>
    {
        public VersaoValidador(IDisplayNameExtrator displayNameExtrator) : base(displayNameExtrator)
        {
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.Nome)
                .NotEmpty()
                .Length(3, 150);

            RuleFor(p => p.Abreviacao)
                .NotEmpty();

            RuleForEach(p => p.Suplementos)
                .SetValidator(this);
        }
    }
}
