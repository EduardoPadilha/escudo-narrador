using EscudoNarrador.Entidade;
using System.Collections.Generic;

namespace EscudoNarrador.Fronteira.DTOs.API
{
    public class SistemaDTO
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
        public List<Versao> Versoes { get; set; }
    }
}
