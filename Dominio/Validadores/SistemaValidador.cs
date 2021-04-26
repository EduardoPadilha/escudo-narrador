using EscudoNarrador.Entidade;
using FluentValidation;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Validacoes;

namespace EscudoNarrador.Dominio.Validadores
{
    public class SistemaValidador : ValidadorAbstrato<Sistema>
    {
        public SistemaValidador(IDisplayNameExtrator displayNameExtrator, IValidador<Versao> validadorVersao) : base(displayNameExtrator)
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .Length(3, 150);

            RuleFor(p => p.Abreviacao)
                .NotEmpty();

            RuleForEach(p => p.Versoes)
               .SetValidator((VersaoValidador)validadorVersao);
        }
    }
}
