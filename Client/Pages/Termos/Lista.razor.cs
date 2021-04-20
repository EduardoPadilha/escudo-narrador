using Client.Extensoes;
using Client.Shared;
using EscudoNarrador.Fronteira.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nebularium.Tarrasque.Extensoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Pages.Termos
{
    public partial class Lista
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public NavigationManager Url { get; set; }

        [CascadingParameter]
        public ErroComponente Error { get; set; }
        [Parameter] public Guid Sistema { get; set; }

        public TableSkeleton TabelaEsqueleto { get; set; }

        private IEnumerable<TermoDTO> dadosPagina;
        private MudTable<TermoDTO> table;

        private int totalItems;
        private string termosBusca = "";

        private async Task<TableData<TermoDTO>> ServerReload(TableState state)
        {
            try
            {
                var tags = ObtemTagsDaBusca(termosBusca);
                var termoBusca = RemoveHashTags(termosBusca);

                var termos = await Http.GetAsync<List<TermoDTO>>($"/api/{Sistema}/termo", new Dictionary<string, string>
                {
                    {"nome", termoBusca },
                    {"tags", string.Join(';', tags) }
                }, TabelaEsqueleto.AtualizarLoading);

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

        private string RemoveHashTags(string texto)
        {
            var regex = new Regex(@"#[\d\w]*");
            texto = regex.Replace(texto, string.Empty);
            return texto.Trim();
        }

        private string[] ObtemTagsDaBusca(string texto)
        {
            var correspondencias = Regex.Matches(texto, @"#[\d\w]*");
            return correspondencias.Select(match => match.Value.RemoverCaracteresEspeciais()).ToArray();
        }

        private void OnSearch(string text)
        {
            termosBusca = text;
            table.ReloadServerData();
        }
    }
}
