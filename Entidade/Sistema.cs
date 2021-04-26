using EscudoNarrador.Entidade.Extensoes;
using System;
using System.Collections.Generic;

namespace EscudoNarrador.Entidade
{
    public class Sistema : EntidadeNomeHigienizado
    {
        public string Abreviacao { get; set; }
        public List<Versao> Versoes { get; set; }

        public Guid AdicionarVersao(Versao versao)
        {
            return Versoes.Adicionar(versao);
        }

        public bool AtualizarVersao(Versao versao)
        {
            return Versoes.Atualizar(versao);
        }

        public bool RemoverVersao(Versao versao)
        {
            return Versoes.Remover(versao);
        }

        public Versao ObterVersao(Guid id)
        {
            return Versoes.Obter(id);
        }
    }
}
