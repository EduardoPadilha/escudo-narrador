using System;
using System.Collections.Generic;

namespace EscudoNarrador.Fronteira.DTOs.API
{
    public class VersaoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
        public List<VersaoDTO> Suplementos { get; set; }
    }
}
