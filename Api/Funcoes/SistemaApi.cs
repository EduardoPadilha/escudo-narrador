using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Dominio.Abstracoes.Servicos;
using EscudoNarrador.Dominio.Filtros;
using EscudoNarrador.Entidade;
using EscudoNarrador.Fronteira.DTOs.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using System.Threading.Tasks;

namespace EscudoNarrador.Api.Funcoes
{
    public class SistemaApi : FuncaoCrud<Sistema, SistemaDTO>
    {
        public SistemaApi(IServicoCrud<Sistema> servico) : base(servico)
        {
        }

        [FunctionName(nameof(SistemaFunction))]
        public async Task<IActionResult> SistemaFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, new string[]{ "get", "post", "put", "delete"}, Route = "sistema/{id?}")]
        HttpRequest req, ILogger log, string id)
        {
            return await EntradaCrud(req, log, id);
        }

        protected override IFiltro<Sistema> CriarFiltro(HttpRequest req)
        {
            var query = req.Query.Obter<string>("query");
            return new SistemaFiltro(query);
        }
    }
}
