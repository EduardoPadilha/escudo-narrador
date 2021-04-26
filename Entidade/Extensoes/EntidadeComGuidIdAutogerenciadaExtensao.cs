using EscudoNarrador.Entidade.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscudoNarrador.Entidade.Extensoes
{
    public static class EntidadeComGuidIdAutogerenciadaExtensao
    {
        public static Guid Adicionar<T>(this List<T> lista, T versao) where T : IEntidadeComGuidIdAutogerenciada
        {
            versao.Id = Guid.NewGuid();
            if (lista == null)
                lista = new List<T>();
            lista.Add(versao);
            return versao.Id;
        }

        public static bool Atualizar<T>(this List<T> lista, T versao) where T : IEntidadeComGuidIdAutogerenciada
        {
            if (!lista.Remover(versao)) return false;
            lista.Add(versao);
            return true;
        }

        public static bool Remover<T>(this List<T> lista, T versao) where T : IEntidadeComGuidIdAutogerenciada
        {
            if (!lista.AnySafe()) return false;
            return lista.Remove(versao);
        }

        public static T Obter<T>(this List<T> lista, Guid id) where T : IEntidadeComGuidIdAutogerenciada
        {
            return lista.FirstOrDefault(c => c.Id == id);
        }
    }
}
