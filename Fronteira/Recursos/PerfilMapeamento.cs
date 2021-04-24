using AutoMapper;
using EscudoNarrador.Entidade;
using EscudoNarrador.Fronteira.DTOs.API;

namespace EscudoNarrador.Fronteira.Recursos
{
    public class PerfilMapeamento : Profile
    {
        public PerfilMapeamento()
        {
            CreateMap<Termo, TermoDTO>().ReverseMap();
        }
    }
}
