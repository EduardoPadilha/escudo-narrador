using EscudoNarrador.Entidade.Enums;
using EscudoNarrador.Fronteira.DTOs;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Client.VOs
{
    public class TermoVO
    {
        public TermoVO() { }
        public TermoVO(TermoDTO entidade)
        {
            Nome = entidade.Nome;
            TagsCampo = string.Join(";", entidade.Tags);
            Tipos = entidade.Tipo.ObterValores().ToHashSet();
            Pontos = entidade.Pontos;
        }

        [Required(ErrorMessage = "Nome não pode ser vazio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Tag não pode ser vazio.")]
        public string TagsCampo { get; set; } = "";
        public string[] Tags => TagsCampo.Contains(";") ? TagsCampo.Split(';') : TagsCampo.Split(',');
        public HashSet<TipoTermo> Tipos { get; set; } = new HashSet<TipoTermo>();
        public int Pontos { get; set; }

        public TermoDTO ParaDTO()
        {
            var tipo = (TipoTermo)default;
            foreach (var t in Tipos)
                tipo |= t;

            return new TermoDTO
            {
                Nome = Nome,
                Tags = Tags,
                Tipo = tipo,
                Pontos = Pontos,
            };
        }
    }
}
