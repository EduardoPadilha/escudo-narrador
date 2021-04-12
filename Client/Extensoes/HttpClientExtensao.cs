using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Client.Extensoes
{
    public static class HttpClientExtensao
    {
        #region GET

        public async static Task<T> GetAsync<T>(this HttpClient cliente, string path, Dictionary<string, string> parametros = null, Action<bool> atualizaLoading = null)
        {
            string corpoResposta = await GetAsync(cliente, path, parametros);

            return JsonConvert.DeserializeObject<T>(corpoResposta);
        }
        public async static Task<string> GetAsync(this HttpClient cliente, string path, Dictionary<string, string> parametros = null, Action<bool> atualizaLoading = null)
        {
            atualizaLoading.TrataAtualizacaoLoading(true);
            var url = MontaUrl(cliente.BaseAddress, path, parametros);

            Console.WriteLine($"[Disparando GET] URL = {url}");
            var resposta = await cliente.GetAsync(url);
            atualizaLoading.TrataAtualizacaoLoading(false);
            return await MontaResposta(resposta);
        }

        private static void TrataAtualizacaoLoading(this Action<bool> atualizaLoading, bool valor)
        {
            if (atualizaLoading != null)
                atualizaLoading(valor);
        }

        #endregion

        #region Suportes as chamadas http

        //private static StringContent MontaRequisicaoCoporJson(object corpo)
        //{
        //    var jsonRequest = JsonConvert.SerializeObject(corpo);
        //    return new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        //}
        private async static Task<string> MontaResposta(HttpResponseMessage resposta)
        {
            Console.WriteLine($"[Status do {resposta.RequestMessage.Method}] {resposta.StatusCode} - {resposta.ReasonPhrase}");
            string corpoResposta = await resposta.Content.ReadAsStringAsync();
            Console.WriteLine($"[Retorno do {resposta.RequestMessage.Method}] {corpoResposta}");
            resposta.EnsureSuccessStatusCode();

            return corpoResposta;
        }
        private static string MontaUrl(Uri urlBase, string path, Dictionary<string, string> parametros = null)
        {
            var builder = new UriBuilder(urlBase) { Path = path };

            if (parametros != null)
                builder.Query = MontarQuery(parametros);

            return builder.ToString();
        }
        private static string MontarQuery(Dictionary<string, string> parametros)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in parametros)
                query[item.Key] = item.Value;

            return query.ToString();
        }

        #endregion
    }
}
