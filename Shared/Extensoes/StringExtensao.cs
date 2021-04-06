using Nebularium.Tarrasque.Extensoes;
using System.Text.RegularExpressions;

namespace EscudoNarrador.Shared.Extensoes
{
    public static class StringExtensao
    {
        public static string HigienizaString(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;
            texto = Regex.Replace(texto, @"(?:\'|\""|\\|\0|\a|\f|\n|\r|\t|\v|\\b)", string.Empty).Trim(' ');
            return texto.RemoveAcentos().ToLower();
        }
    }
}
