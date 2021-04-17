using AutoMapper;
using EscudoNarrador.Dominio.Extensoes;
using EscudoNarrador.Entidade;
using EscudoNarrador.Entidade.Enums;
using EscudoNarrador.Repositorio.Mapeamento;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscudoNarrador.Repositorio.Recursos
{
    public class PerfilMapeamento : Profile
    {
        public PerfilMapeamento()
        {
            CreateMap<Termo, TermoMapeamento>()
                .ForMemberMapFrom(destino => destino.PartitionKey, origem => origem.Sistema.ToString())
                .ForMemberMapFrom(destino => destino.RowKey, origem => origem.Nome.HigienizaString())
                .ForMemberMapFrom(destino => destino.TagsApresentacao, origem => ConverteArray(origem.Tags, ';', false))
                .ForMemberMapFrom(destino => destino.TagsHigienizadas, origem => ConverteArray(origem.Tags, ';', true))
                .ForMemberMapFrom(destino => destino.Tipo, origem => (int)origem.Tipo)
                .ReverseMap()
                .ForMemberMapFrom(destino => destino.Sistema, origem => origem.PartitionKey)
                .ForMemberMapFrom(destino => destino.Tags, origem => ConverteString(origem.TagsApresentacao, ';'))
                .ForMemberMapFrom(destino => destino.Tipo, origem => ConveteEnum<TipoTermo>(origem.Tipo));
        }


        #region Suporte as conversões

        private string ConverteArray(IEnumerable<string> lista, char separador = ';', bool higienizar = true)
        {
            if (!lista.AnySafe()) return string.Empty;

            if (higienizar)
                lista = lista.Select(c => c.HigienizaString());
            return string.Join(separador, lista);
        }

        private IEnumerable<string> ConverteString(string texto, char separador = ';')
        {
            if (texto.LimpoNuloBranco()) return Enumerable.Empty<string>();

            return texto.Split(separador);
        }

        private TEnum ConveteEnum<TEnum>(int valor) where TEnum : struct
        {
            return Enum.Parse<TEnum>(valor.ToString());
        }

        #endregion
    }
}
