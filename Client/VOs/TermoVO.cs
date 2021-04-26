using EscudoNarrador.Entidade.Enums;
using EscudoNarrador.Fronteira.DTOs.API;
using Nebularium.Tarrasque.Extensoes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Markdig;
using Ganss.XSS;
using Microsoft.AspNetCore.Components;

namespace Client.VOs
{
    public class TermoVO
    {
        public TermoVO() { }
        public TermoVO(TermoDTO entidade)
        {
            Id = entidade.Id;
            Nome = entidade.Nome;
            TagsCampo = string.Join(";", entidade.Tags);
            Tipos = entidade.Tipo.ObterValores().ToHashSet();
            Pontos = entidade.Pontos;
            Descricao = entidade.Descricao;
            Sistema = entidade.Sistema;
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Nome não pode ser vazio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Tag não pode ser vazio.")]
        public string TagsCampo { get; set; } = "";
        public string[] Tags => TagsCampo.Contains(";") ? TagsCampo.Split(';') : TagsCampo.Split(',');
        public HashSet<TipoTermo> Tipos { get; set; } = new HashSet<TipoTermo>();
        public int Pontos { get; set; }
        [Required(ErrorMessage = "Decrição não pode ser vazio.")]
        public string Descricao { get; set; } = string.Empty;
        public MarkupString Preview => ConverteStringParaMarkdown(Descricao);
        private MarkupString ConverteStringParaMarkdown(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new MarkupString();
            var html = Markdown.ToHtml(value, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
            var sanitizedHtml = new HtmlSanitizer().Sanitize(html);
            return new MarkupString(sanitizedHtml);
        }
        public string Sistema { get; set; }

        private TipoTermo TiposSelecionados()
        {
            var tipo = (TipoTermo)default;
            foreach (var t in Tipos)
                tipo |= t;
            return tipo;
        }

        public TermoDTO ParaDTO()
        {
            return new TermoDTO
            {
                Nome = Nome,
                Tags = Tags,
                Tipo = TiposSelecionados(),
                Pontos = Pontos,
                Descricao = Descricao,
                Sistema = Sistema
            };
        }
    }
}
