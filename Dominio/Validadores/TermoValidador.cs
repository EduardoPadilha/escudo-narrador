using EscudoNarrador.Entidade;
using FluentValidation;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Validacoes;

namespace EscudoNarrador.Dominio.Validadores
{
    public class TermoValidador : ValidadorAbstrato<Termo>
    {
        public const string AO_ADICIONAR = nameof(AO_ADICIONAR);
        public const string AO_ATUALIZAR = nameof(AO_ATUALIZAR);
        public TermoValidador(IDisplayNameExtrator displayNameExtrator) : base(displayNameExtrator)
        {
            RuleSet(AO_ADICIONAR, () =>
            {
                RuleFor(p => p.Sistema)
                .NotEmpty();

                AddRegrasGerais();
            });

            RuleSet(AO_ATUALIZAR, () =>
            {
                AddRegrasGerais();
            });
        }

        private void AddRegrasGerais()
        {

            RuleFor(p => p.Nome)
                .NotEmpty()
                .Length(3, 150);

            RuleFor(p => p.Tags)
                .NotEmpty();

            RuleFor(p => p.Pontos)
                .GreaterThan(0);

            RuleFor(p => p.Descricao)
                .NotEmpty();
        }
    }
}
