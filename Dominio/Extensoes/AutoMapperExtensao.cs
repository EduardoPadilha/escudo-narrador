using AutoMapper;
using System;
using System.Linq.Expressions;

namespace EscudoNarrador.Dominio.Extensoes
{
    public static class AutoMapperExtensao
    {
        public static IMappingExpression<TSource, TDestination> ForMemberMapFrom
            <TDestination, TDestinationMember, TSource, TSourceMember>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, TDestinationMember>> destinationMember,
            Expression<Func<TSource, TSourceMember>> mapExpression)
        {
            return map.ForMember(destinationMember, origem => origem.MapFrom(mapExpression));
        }
    }
}
