using EscudoNarrador.Shared.Entidades;
using EscudoNarrador.Shared.Enums;

namespace EscudoNarrador.Shared.Dtos.Entrada
{
    public class AddCaracteristicaDto
    {
        public AddCaracteristicaDto() { }
        public AddCaracteristicaDto(Caracteristica caracteristica)
        {
            Nome = caracteristica.Nome;
            Tags = caracteristica.Tags;
            Tipo = caracteristica.Tipo;
            Pontos = caracteristica.Pontos;
            Sistema = caracteristica.Sistema;
        }
        public string Nome { get; set; }
        public string[] Tags { get; set; }
        public TipoCaracteristica Tipo { get; set; }
        public int Pontos { get; set; }
        public TipoSistema Sistema { get; set; }

        public Caracteristica ParaEntidade()
        {
            return new Caracteristica
            {
                Nome = Nome,
                Tags = Tags,
                Tipo = Tipo,
                Pontos = Pontos,
                Sistema = Sistema
            };
        }

        public static AddCaracteristicaDto Construir(Caracteristica caracteristica)
        {
            return new AddCaracteristicaDto(caracteristica);
        }
    }

}
