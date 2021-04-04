using EscudoNarrador.Api.Extensoes;
using EscudoNarrador.Shared.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscudoNarrador.Api.Funcoes
{
    public class CaracteristicaFuncao
    {
        private Caracteristica[] Caracteristicas = new Caracteristica[]
        {
            new Caracteristica
            {
                Id = Guid.Parse("3B25DF74-C9C7-4562-92D5-A4E8463D45BA"),
                Nome = "Nome",
                Tags = new string[] { "Base" },
                Tipo = TipoCaracteristica.Informativa
            },
            new Caracteristica
            {
                Id = Guid.Parse("05C85DA0-B1D3-4BE5-8177-CB5DBDA9A75B"),
                Nome = "Virtude",
                Tags = new string[] { "Base" },
                Tipo = TipoCaracteristica.Informativa
            },
            new Caracteristica
            {
                Id = Guid.Parse("65B80388-0A81-4774-9E8D-D2B28B01757A"),
                Nome = "Força",
                Tags = new string[] { "Atributo", "Poder", "Físico" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Expansivel
            },
            new Caracteristica
            {
                Id = Guid.Parse("A1DDF4D2-6A77-4034-BB99-ED0A892F28AA"),
                Nome = "Inteligência",
                Tags = new string[] { "Atributo", "Poder", "Mental" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Expansivel
            },
            new Caracteristica
            {
                Id = Guid.Parse("8D53F9E0-87D8-4406-8B97-589A6491272D"),
                Nome = "Manipulação",
                Tags = new string[] { "Atributo", "Refinamento", "Social" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Expansivel
            },
            new Caracteristica
            {
                Id = Guid.Parse("738CD674-AF3B-4C7A-BE82-A6E2DC7896C9"),
                Nome = "Ofícios",
                Tags = new string[] { "Habilidade", "Mental" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Expansivel
            },
            new Caracteristica
            {
                Id = Guid.Parse("3D6F15F6-DFAA-4C10-8A35-7BC0002D2493"),
                Nome = "Assistente Investigativo",
                Tags = new string[] { "Vantagem", "Mental" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Fixo,
                Pontos = 1
            },
            new Caracteristica
            {
                Id = Guid.Parse("DE178069-BE8F-466C-A438-0DC9583F677C"),
                Nome = "Ambidestro",
                Tags = new string[] { "Vantagem", "Física" },
                Tipo = TipoCaracteristica.Ponto | TipoCaracteristica.Fixo,
                Pontos = 3
            },
        };


        [FunctionName(nameof(ObterCaracteristas))]
        public IActionResult ObterCaracteristas(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "caracteristica/")] HttpRequest req,
            ILogger log)
        {
            var nome = req.Query.Obter<string>("nome")?.ToLower();
            var tagsParam = req.Query.Obter<string>("tags")?.ToLower();
            var tags = tagsParam?.Split(",").Select(c => c.Trim()).ToList();
            IEnumerable<Caracteristica> query = Caracteristicas;
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(c => c.Nome.ToLower().Contains(nome));
            if (tags?.Any() ?? false)
                query = query.Where(c => tags.All(t => c.Tags.Any(tt => tt.ToLower() == t)));

            return new OkObjectResult(query);
        }

        [FunctionName(nameof(ObterCaracteristaPorId))]
        public IActionResult ObterCaracteristaPorId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "caracteristica/{id}")] HttpRequest req, Guid id,
            ILogger log)
        {
            if (id == Guid.Empty)
            {
                var argumentMsg = $"Erro em {nameof(ObterCaracteristaPorId)}, id da característica deve vir no path da requisição.";
                log.LogError(argumentMsg);
                return new BadRequestObjectResult(new { error = argumentMsg });
            }

            var result = Caracteristicas.FirstOrDefault(c => c.Id == id);

            return new OkObjectResult(result);
        }
    }
}
