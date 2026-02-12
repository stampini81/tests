using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Backend.Integration
{
    public class PessoaExclusaoCascataTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public PessoaExclusaoCascataTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ExcluirPessoa_DeveRemoverTransacoesAssociadas()
        {
            // Arrange: cria pessoa e transação via API
            var pessoa = new { nome = "Teste", dataNascimento = "2000-01-01" };
            var pessoaResp = await _client.PostAsJsonAsync("/api/v1/pessoas", pessoa);
            var pessoaObj = await pessoaResp.Content.ReadFromJsonAsync<dynamic>();
            string pessoaId = pessoaObj.id;

            var transacao = new { descricao = "Despesa Teste", valor = 100, data = "2026-02-10", tipo = "Despesa", pessoaId, categoriaId = "1" };
            await _client.PostAsJsonAsync("/api/v1/transacoes", transacao);

            // Act: exclui pessoa
            var deleteResp = await _client.DeleteAsync($"/api/v1/pessoas/{pessoaId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResp.StatusCode);

            // Assert: transações da pessoa não existem mais
            var transacoesResp = await _client.GetAsync($"/api/v1/transacoes?pessoaId={pessoaId}");
            var transacoes = await transacoesResp.Content.ReadFromJsonAsync<dynamic[]>();
            Assert.Empty(transacoes);
        }
    }
}
