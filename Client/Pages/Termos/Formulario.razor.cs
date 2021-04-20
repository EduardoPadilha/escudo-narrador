using Client.Shared;
using Client.Extensoes;
using EscudoNarrador.Entidade;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Net.Http;
using Nebularium.Tarrasque.Extensoes;
using Microsoft.AspNetCore.Components.Forms;
using EscudoNarrador.Fronteira.DTOs;
using Client.VOs;

namespace Client.Pages.Termos
{
    public partial class Formulario
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        [Parameter] public string Id { get; set; }
        [Parameter] public Guid Sistema { get; set; }

        [CascadingParameter]
        public ErroComponente Error { get; set; }

        [CascadingParameter]
        public GerenciadorLoading Loading { get; set; }

        private bool operacaoAtaulizacao = false;
        private TermoVO modelo = new TermoVO();

        private void LimparFormulario()
        {
            SetModelo(new TermoVO());
        }

        private async void AbrirExclusaoPopUp()
        {
            var parameters = new DialogParameters();
            parameters.Add("TextoCorpo", $"Tem certeza que deseja deletar {modelo.Nome}? Esse procedirmento é irreversível.");
            parameters.Add("TextoBotao", "Deletar");
            parameters.Add("CorBotao", Color.Error);
            var dialogo = DialogService.Show<DialogoConfirmacao>("Cuidado =[", parameters);
            var resultado = await dialogo.Result;
            if (resultado.Cancelled) return;

            try
            {
                var deleteResultado = await Http.DeleteAsync<Termo>($"/api/{Sistema}/termo/{modelo.Nome}", Loading.AtualizarLoading);
                LimparFormulario();
                Snackbar.Add("Registro deletado!", Severity.Success);
            }
            catch (Exception e)
            {
                Loading.AtualizarLoading(false);
                Error.ProcessarErro(e);
            }
        }

        private async void OnValidSubmit(EditContext context)
        {
            HttpResponseMessage resposta;
            try
            {
                var path = $"/api/{Sistema}/termo";
                if (operacaoAtaulizacao)
                    resposta = await Http.PutAsJsonAsync(path, modelo.ParaDTO(), Loading.AtualizarLoading);
                else
                    resposta = await Http.PostAsJsonAsync(path, modelo.ParaDTO(), Loading.AtualizarLoading);

                LimparFormulario();

                Snackbar.Add("Termo salvo!", Severity.Success);
            }
            catch (Exception e)
            {
                Loading.AtualizarLoading(false);
                Error.ProcessarErro(e);
            }

        }

        protected async override void OnInitialized()
        {
            operacaoAtaulizacao = !Id.LimpoNuloBranco();
            if (!operacaoAtaulizacao) return;
            try
            {
                var termo = await Http.GetAsync<TermoDTO>($"/api/{Sistema}/termo/{Id}", atualizaLoading: Loading.AtualizarLoading);
                if (termo == null)
                {
                    Error.ProcessarErro($"Não foi possível recuperar o registro {Id} para edição.");
                    return;
                }
                SetModelo(new TermoVO(termo));
            }
            catch (Exception e)
            {
                Loading.AtualizarLoading(false);
                Error.ProcessarErro(e);
            }
        }

        private void SetModelo(TermoVO termo)
        {
            modelo = termo;
            StateHasChanged();
        }
    }
}
