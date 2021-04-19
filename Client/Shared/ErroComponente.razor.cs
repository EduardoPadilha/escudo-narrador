using Client.Excecoes;
using Client.VOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;

namespace Client.Shared
{
    public partial class ErroComponente
    {
        [Inject] public ISnackbar Snackbar { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        //public string Text { get; set; }

        public void ProcessarErro(Exception ex)
        {
            if (ex is ValidacaoExcecao excecao)
                ProcessarErro(excecao.Erros.FormataErros());
            else
                ProcessarErro(ex.GetBaseException().Message);
        }

        public void ProcessarErro(string msg)
        {
            Snackbar.Add(msg, Severity.Error);
            Console.WriteLine(msg);
        }
    }
}
