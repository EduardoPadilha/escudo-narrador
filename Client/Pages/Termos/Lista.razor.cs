using Client.Extensoes;
using Client.Shared;
using EscudoNarrador.Fronteira.DTOs.API;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Pages.Termos
{
    public partial class Lista
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public NavigationManager Url { get; set; }

        [CascadingParameter]
        public ErroComponente Error { get; set; }
        [Parameter] public string Sistema { get; set; }

        public TableSkeleton TabelaEsqueleto { get; set; }

        private IEnumerable<TermoDTO> dadosPagina;
        private MudTable<TermoDTO> table;

        private int totalItems;
        private string termosBusca = "";

        private async Task<TableData<TermoDTO>> AtualizacaoServidor(TableState state)
        {
            try
            {
                var dataJson = await Http.GetAsync($"/api/termo", new Dictionary<string, string>
                    {
                        {"sistema", Sistema },
                        {"query", termosBusca },
                    }, TabelaEsqueleto.AtualizarLoading);
                var termos = JsonConvert.DeserializeObject<List<TermoDTO>>(dataJson);

                totalItems = termos.Count();

                dadosPagina = termos.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();

                return new TableData<TermoDTO>() { TotalItems = totalItems, Items = dadosPagina };
            }
            catch (Exception e)
            {
                TabelaEsqueleto.AtualizarLoading(false);
                Error.ProcessarErro(e);
                return new TableData<TermoDTO>() { TotalItems = 0, Items = null };
            }
        }

        private void AoProcurar(string text)
        {
            termosBusca = text;
            table.ReloadServerData();
        }
    }
}
