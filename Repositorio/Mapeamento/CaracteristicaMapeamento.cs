using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace EscudoNarrador.Repositorio.Mapeamento
{
    public class CaracteristicaMapeamento : TableEntity
    {
        public CaracteristicaMapeamento() { }
        public CaracteristicaMapeamento(Caracteristica caracteristica)
        {
            Tags = caracteristica.Tags;
            Tipo = caracteristica.Tipo;
            Pontos = caracteristica.Pontos;

            PartitionKey = caracteristica.Sistema.ToString();
            RowKey = caracteristica.Nome;
        }
        public string[] Tags { get; set; }
        public TipoCaracteristica Tipo { get; set; }
        public int Pontos { get; set; }

        public Caracteristica ParaEntidade()
        {
            return new Caracteristica
            {
                Nome = RowKey,
                Tags = Tags,
                Tipo = Tipo,
                Pontos = Pontos,
                Sistema = Enum.Parse<TipoSistema>(PartitionKey)
            };
        }
    }
}
