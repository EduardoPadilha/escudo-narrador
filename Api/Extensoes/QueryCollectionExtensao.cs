using Microsoft.AspNetCore.Http;
using System.Web;

namespace EscudoNarrador.Api.Extensoes
{
    public static class QueryCollectionExtensao
    {
        public static string Obter(this IQueryCollection query, string param)
        {
            var objString = query[param];
            string obj = HttpUtility.UrlDecode(objString);
            return obj;
        }
        //public static T Obter<T>(this IQueryCollection query, string param)
        //{
        //    var objString = query[param];
        //    string obj = HttpUtility.UrlDecode(objString);
        //    return obj as T;
        //}
    }
}
