using Nebularium.Tarrasque.Extensoes;
using System.Collections.Generic;
using System.Linq;

namespace Client.VOs
{
    public class ErroValidacao
    {
        public string NomePropriedade { get; set; }
        public string Mensagem { get; set; }

        public override string ToString()
        {
            return $"'{NomePropriedade}' - {Mensagem}";
        }
    }

    public static class ErroValidacaoExtensao
    {
        public static string FormataErros(this IEnumerable<ErroValidacao> erros)
        {
            if (!erros.AnySafe()) return string.Empty;
            return string.Concat(erros.Select(erro => $"<p>{erro}</p>"));
        }
    }
}
