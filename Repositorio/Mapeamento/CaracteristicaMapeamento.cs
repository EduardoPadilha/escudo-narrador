using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;
using EscudoNarrador.Shared.Extensoes;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscudoNarrador.Repositorio.Mapeamento
{
    public class CaracteristicaMapeamento : TableEntity
    {
        public CaracteristicaMapeamento() { }
        public CaracteristicaMapeamento(Caracteristica caracteristica)
        {
            var tags = caracteristica.Tags?.Select(c => c.HigienizaString()) ?? new List<string>();
            TagsHigienizadas = string.Join(';', tags);
            Tags = string.Join(';', caracteristica.Tags);
            Tipo = (int)caracteristica.Tipo;
            Pontos = caracteristica.Pontos;

            PartitionKey = caracteristica.Sistema.ToString();
            RowKey = caracteristica.Nome.HigienizaString();
            Nome = caracteristica.Nome;
        }
        public string Nome { get; set; }
        public string TagsHigienizadas { get; set; }
        public string Tags { get; set; }
        public int Tipo { get; set; }
        public int Pontos { get; set; }

        public Caracteristica ParaEntidade()
        {
            return new Caracteristica
            {
                Nome = Nome,
                Tags = Tags.Split(';'),
                Tipo = (TipoCaracteristica)Tipo,
                Pontos = Pontos,
                Sistema = Enum.Parse<TipoSistema>(PartitionKey)
            };
        }
    }
}
