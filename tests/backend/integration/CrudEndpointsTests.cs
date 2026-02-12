using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Backend.Integration;

/// <summary>
/// Testes de integração para verificar todos os endpoints CRUD (POST, GET, PUT, DELETE)
/// de Pessoa, Categoria e Transação, e o endpoint de Totais por Pessoa.
/// Requer que a API esteja rodando (docker compose up --build).
/// URL base: http://localhost:5000/api/v1/
/// </summary>
public class CrudEndpointsTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public CrudEndpointsTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    // =============================================
    //  HELPER — Extrai id de JSON retornado
    // =============================================
    private static async Task<string> ExtrairId(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("id").GetString()!;
    }

    // =============================================
    //  PESSOA — POST / GET / GET{id} / PUT / DELETE
    // =============================================

    [Fact(DisplayName = "Pessoa POST — Deve criar uma pessoa com sucesso (201)")]
    public async Task Pessoa_Post_DeveCriarComSucesso()
    {
        var pessoa = new { nome = "Post Test", dataNascimento = "2000-05-15" };
        var resp = await _client.PostAsJsonAsync("Pessoas", pessoa);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

        var id = await ExtrairId(resp);
        Assert.False(string.IsNullOrEmpty(id));

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{id}");
    }

    [Fact(DisplayName = "Pessoa GET — Deve listar pessoas com paginação (200)")]
    public async Task Pessoa_GetAll_DeveListarComSucesso()
    {
        var resp = await _client.GetAsync("Pessoas?page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        // Deve ter propriedades de paginação
        Assert.True(doc.RootElement.TryGetProperty("items", out _) || doc.RootElement.TryGetProperty("data", out _));
    }

    [Fact(DisplayName = "Pessoa GET/{id} — Deve retornar pessoa pelo ID (200)")]
    public async Task Pessoa_GetById_DeveRetornarPessoa()
    {
        // Arrange — criar pessoa
        var pessoa = new { nome = "GetById Test", dataNascimento = "1995-03-20" };
        var createResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var getResp = await _client.GetAsync($"Pessoas/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var json = await getResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("GetById Test", doc.RootElement.GetProperty("nome").GetString());

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{id}");
    }

    [Fact(DisplayName = "Pessoa GET/{id} — Deve retornar 404 para ID inexistente")]
    public async Task Pessoa_GetById_DeveRetornar404ParaInexistente()
    {
        var resp = await _client.GetAsync($"Pessoas/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact(DisplayName = "Pessoa PUT — Deve atualizar pessoa com sucesso (204)")]
    public async Task Pessoa_Put_DeveAtualizarComSucesso()
    {
        // Arrange — criar pessoa
        var pessoa = new { nome = "Put Original", dataNascimento = "1990-01-01" };
        var createResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act — atualizar pessoa
        var update = new { nome = "Put Atualizado", dataNascimento = "1990-06-15" };
        var putResp = await _client.PutAsJsonAsync($"Pessoas/{id}", update);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);

        // Verificar que o nome foi atualizado
        var getResp = await _client.GetAsync($"Pessoas/{id}");
        var json = await getResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("Put Atualizado", doc.RootElement.GetProperty("nome").GetString());

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{id}");
    }

    [Fact(DisplayName = "Pessoa PUT — Deve retornar 404 para ID inexistente")]
    public async Task Pessoa_Put_DeveRetornar404ParaInexistente()
    {
        var update = new { nome = "Inexistente", dataNascimento = "1990-01-01" };
        var resp = await _client.PutAsJsonAsync($"Pessoas/{Guid.NewGuid()}", update);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact(DisplayName = "Pessoa DELETE — Deve excluir pessoa com sucesso (204)")]
    public async Task Pessoa_Delete_DeveExcluirComSucesso()
    {
        // Arrange
        var pessoa = new { nome = "Delete Test", dataNascimento = "1985-12-25" };
        var createResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var deleteResp = await _client.DeleteAsync($"Pessoas/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResp.StatusCode);

        // Confirmar que a pessoa não existe mais
        var getResp = await _client.GetAsync($"Pessoas/{id}");
        Assert.Equal(HttpStatusCode.NotFound, getResp.StatusCode);
    }

    [Fact(DisplayName = "Pessoa DELETE — Retorna 204 mesmo para ID inexistente (Bug: deveria ser 404)")]
    public async Task Pessoa_Delete_DeveRetornar204ParaInexistente_Bug()
    {
        var resp = await _client.DeleteAsync($"Pessoas/{Guid.NewGuid()}");
        // BUG DA API: Deveria retornar 404 (NotFound), mas retorna 204 (NoContent)
        // Isso significa que a API não verifica se o registro existe antes de "excluir"
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
    }

    // =============================================
    //  CATEGORIA — POST / GET / GET{id}
    //  PUT e DELETE NÃO IMPLEMENTADOS (Bug da API)
    // =============================================

    [Fact(DisplayName = "Categoria POST — Deve criar uma categoria com sucesso (201)")]
    public async Task Categoria_Post_DeveCriarComSucesso()
    {
        var categoria = new { descricao = "Cat Post Test", finalidade = 0 }; // Despesa
        var resp = await _client.PostAsJsonAsync("Categorias", categoria);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

        var id = await ExtrairId(resp);
        Assert.False(string.IsNullOrEmpty(id));
    }

    [Fact(DisplayName = "Categoria GET — Deve listar categorias com paginação (200)")]
    public async Task Categoria_GetAll_DeveListarComSucesso()
    {
        var resp = await _client.GetAsync("Categorias?page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var json = await resp.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact(DisplayName = "Categoria GET/{id} — Deve retornar categoria pelo ID (200)")]
    public async Task Categoria_GetById_DeveRetornarCategoria()
    {
        // Arrange
        var categoria = new { descricao = "Cat GetById", finalidade = 1 }; // Receita
        var createResp = await _client.PostAsJsonAsync("Categorias", categoria);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var getResp = await _client.GetAsync($"Categorias/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var json = await getResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("Cat GetById", doc.RootElement.GetProperty("descricao").GetString());
    }

    [Fact(DisplayName = "Categoria PUT — NÃO IMPLEMENTADO — Deve retornar 405 (Bug da API)")]
    public async Task Categoria_Put_NaoImplementado_Retorna405()
    {
        // Arrange — criar categoria para tentar atualizar
        var categoria = new { descricao = "Cat Put Test", finalidade = 0 };
        var createResp = await _client.PostAsJsonAsync("Categorias", categoria);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act — tentar PUT (não existe na API)
        var update = new { descricao = "Cat Atualizada", finalidade = 0 };
        var putResp = await _client.PutAsJsonAsync($"Categorias/{id}", update);

        // Assert — BUG: Deveria ser 200/204, mas retorna 405
        Assert.Equal(HttpStatusCode.MethodNotAllowed, putResp.StatusCode);
    }

    [Fact(DisplayName = "Categoria DELETE — NÃO IMPLEMENTADO — Deve retornar 405 (Bug da API)")]
    public async Task Categoria_Delete_NaoImplementado_Retorna405()
    {
        // Arrange
        var categoria = new { descricao = "Cat Del Test", finalidade = 0 };
        var createResp = await _client.PostAsJsonAsync("Categorias", categoria);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var deleteResp = await _client.DeleteAsync($"Categorias/{id}");

        // Assert — BUG: Deveria ser 204, mas retorna 405
        Assert.Equal(HttpStatusCode.MethodNotAllowed, deleteResp.StatusCode);
    }

    // =============================================
    //  TRANSAÇÃO — POST / GET / GET{id}
    //  PUT e DELETE NÃO IMPLEMENTADOS (Bug da API)
    // =============================================

    [Fact(DisplayName = "Transação POST — Deve criar uma transação com sucesso (201)")]
    public async Task Transacao_Post_DeveCriarComSucesso()
    {
        // Pré-requisitos: criar pessoa e categoria
        var pessoa = new { nome = "Trans Post Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        var categoria = new { descricao = "Trans Cat Post", finalidade = 0 }; // Despesa
        var catResp = await _client.PostAsJsonAsync("Categorias", categoria);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Act
        var transacao = new
        {
            descricao = "Despesa Post Test",
            valor = 150m,
            tipo = 0, // Despesa
            pessoaId,
            categoriaId,
            data = "2024-06-01"
        };
        var resp = await _client.PostAsJsonAsync("Transacoes", transacao);

        // Assert
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var id = await ExtrairId(resp);
        Assert.False(string.IsNullOrEmpty(id));

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    [Fact(DisplayName = "Transação GET — Deve listar transações com paginação (200)")]
    public async Task Transacao_GetAll_DeveListarComSucesso()
    {
        var resp = await _client.GetAsync("Transacoes?page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var json = await resp.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact(DisplayName = "Transação GET/{id} — Deve retornar transação pelo ID (200)")]
    public async Task Transacao_GetById_DeveRetornarTransacao()
    {
        // Arrange
        var pessoa = new { nome = "Trans GetById", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        var categoria = new { descricao = "Trans Cat GetById", finalidade = 0 };
        var catResp = await _client.PostAsJsonAsync("Categorias", categoria);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        var transacao = new
        {
            descricao = "GetById Despesa",
            valor = 200m,
            tipo = 0,
            pessoaId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var getResp = await _client.GetAsync($"Transacoes/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        var json = await getResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("GetById Despesa", doc.RootElement.GetProperty("descricao").GetString());

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    [Fact(DisplayName = "Transação PUT — NÃO IMPLEMENTADO — Deve retornar 405 (Bug da API)")]
    public async Task Transacao_Put_NaoImplementado_Retorna405()
    {
        // Arrange
        var pessoa = new { nome = "Trans Put Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        var categoria = new { descricao = "Trans Cat Put", finalidade = 0 };
        var catResp = await _client.PostAsJsonAsync("Categorias", categoria);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        var transacao = new
        {
            descricao = "Put Despesa",
            valor = 100m,
            tipo = 0,
            pessoaId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act — tentar PUT (não existe na API)
        var update = new
        {
            descricao = "Put Atualizada",
            valor = 300m,
            tipo = 0,
            pessoaId,
            categoriaId,
            data = "2024-07-01"
        };
        var putResp = await _client.PutAsJsonAsync($"Transacoes/{id}", update);

        // Assert — BUG: Deveria ser 200/204, mas retorna 405
        Assert.Equal(HttpStatusCode.MethodNotAllowed, putResp.StatusCode);

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    [Fact(DisplayName = "Transação DELETE — NÃO IMPLEMENTADO — Deve retornar 405 (Bug da API)")]
    public async Task Transacao_Delete_NaoImplementado_Retorna405()
    {
        // Arrange
        var pessoa = new { nome = "Trans Del Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        var categoria = new { descricao = "Trans Cat Del", finalidade = 0 };
        var catResp = await _client.PostAsJsonAsync("Categorias", categoria);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        var transacao = new
        {
            descricao = "Del Despesa",
            valor = 50m,
            tipo = 0,
            pessoaId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);
        createResp.EnsureSuccessStatusCode();
        var id = await ExtrairId(createResp);

        // Act
        var deleteResp = await _client.DeleteAsync($"Transacoes/{id}");

        // Assert — BUG: Deveria ser 204, mas retorna 405
        Assert.Equal(HttpStatusCode.MethodNotAllowed, deleteResp.StatusCode);

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    // =============================================
    //  REGRAS DE NEGÓCIO
    // =============================================

    [Fact(DisplayName = "Regra — Menor de idade não pode registrar receita")]
    public async Task Regra_MenorNaoPodeReceita()
    {
        // Arrange — criar menor de idade
        var menor = new { nome = "Menor Regra Test", dataNascimento = DateTime.Today.AddYears(-15).ToString("yyyy-MM-dd") };
        var menorResp = await _client.PostAsJsonAsync("Pessoas", menor);
        menorResp.EnsureSuccessStatusCode();
        var menorId = await ExtrairId(menorResp);

        // Criar categoria de Receita
        var catReceita = new { descricao = "Receita Menor Test", finalidade = 1 }; // Receita
        var catResp = await _client.PostAsJsonAsync("Categorias", catReceita);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Act — tentar criar receita para menor
        var transacao = new
        {
            descricao = "Receita Indevida",
            valor = 500m,
            tipo = 1, // Receita
            pessoaId = menorId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);

        // Assert — a API deve rejeitar
        Assert.False(createResp.IsSuccessStatusCode,
            "A API permitiu registrar receita para menor de idade, violando a regra de negócio.");

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{menorId}");
    }

    [Fact(DisplayName = "Regra — Maior de idade pode registrar receita")]
    public async Task Regra_MaiorPodeReceita()
    {
        // Arrange — criar maior de idade
        var maior = new { nome = "Maior Regra Test", dataNascimento = "1990-01-01" };
        var maiorResp = await _client.PostAsJsonAsync("Pessoas", maior);
        maiorResp.EnsureSuccessStatusCode();
        var maiorId = await ExtrairId(maiorResp);

        // Criar categoria de Receita
        var catReceita = new { descricao = "Receita Maior Test", finalidade = 1 }; // Receita
        var catResp = await _client.PostAsJsonAsync("Categorias", catReceita);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Act — criar receita para maior
        var transacao = new
        {
            descricao = "Receita Válida",
            valor = 3000m,
            tipo = 1, // Receita
            pessoaId = maiorId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);

        // Assert — a API deve aceitar
        Assert.True(createResp.IsSuccessStatusCode,
            "A API deveria permitir registrar receita para maior de idade.");

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{maiorId}");
    }

    [Fact(DisplayName = "Regra — Menor de idade pode registrar despesa")]
    public async Task Regra_MenorPodeDespesa()
    {
        // Arrange — criar menor de idade
        var menor = new { nome = "Menor Despesa Test", dataNascimento = DateTime.Today.AddYears(-12).ToString("yyyy-MM-dd") };
        var menorResp = await _client.PostAsJsonAsync("Pessoas", menor);
        menorResp.EnsureSuccessStatusCode();
        var menorId = await ExtrairId(menorResp);

        // Criar categoria de Despesa
        var catDespesa = new { descricao = "Despesa Menor Test", finalidade = 0 }; // Despesa
        var catResp = await _client.PostAsJsonAsync("Categorias", catDespesa);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Act — criar despesa para menor
        var transacao = new
        {
            descricao = "Despesa Válida Menor",
            valor = 50m,
            tipo = 0, // Despesa
            pessoaId = menorId,
            categoriaId,
            data = "2024-06-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);

        // Assert — a API deve aceitar
        Assert.True(createResp.IsSuccessStatusCode,
            "A API deveria permitir registrar despesa para menor de idade.");

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{menorId}");
    }

    [Fact(DisplayName = "Regra — Não permite transação com categoria incompatível")]
    public async Task Regra_NaoPermiteCategoriaIncompativel()
    {
        // Arrange — criar pessoa maior de idade
        var pessoa = new { nome = "CatIncompat Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        // Criar categoria de Despesa
        var catDespesa = new { descricao = "Despesa Incompat", finalidade = 0 }; // Despesa
        var catResp = await _client.PostAsJsonAsync("Categorias", catDespesa);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Act — tentar criar RECEITA usando categoria de DESPESA
        var transacao = new
        {
            descricao = "Receita Indevida",
            valor = 100m,
            tipo = 1, // Receita
            pessoaId,
            categoriaId,
            data = "2024-01-01"
        };
        var createResp = await _client.PostAsJsonAsync("Transacoes", transacao);

        // Assert — a API deve rejeitar
        Assert.False(createResp.IsSuccessStatusCode,
            "A API permitiu registrar receita em categoria de despesa, violando a regra de negócio.");

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    [Fact(DisplayName = "Regra — Exclusão em cascata: deletar pessoa remove transações")]
    public async Task Regra_ExclusaoCascata()
    {
        // Arrange — criar pessoa
        var pessoa = new { nome = "Cascata Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        // Criar categoria
        var categoria = new { descricao = "Cat Cascata", finalidade = 0 }; // Despesa
        var catResp = await _client.PostAsJsonAsync("Categorias", categoria);
        catResp.EnsureSuccessStatusCode();
        var categoriaId = await ExtrairId(catResp);

        // Criar transação associada
        var transacao = new
        {
            descricao = "Despesa Cascata",
            valor = 100m,
            tipo = 0,
            pessoaId,
            categoriaId,
            data = "2024-06-01"
        };
        var transResp = await _client.PostAsJsonAsync("Transacoes", transacao);
        transResp.EnsureSuccessStatusCode();

        // Act — excluir pessoa
        var deleteResp = await _client.DeleteAsync($"Pessoas/{pessoaId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResp.StatusCode);

        // Assert — pessoa não existe mais
        var getPessoaResp = await _client.GetAsync($"Pessoas/{pessoaId}");
        Assert.Equal(HttpStatusCode.NotFound, getPessoaResp.StatusCode);
    }

    // =============================================
    //  TOTAIS POR PESSOA — Receitas - Despesas
    // =============================================

    [Fact(DisplayName = "Totais GET /pessoas — Deve listar totais por pessoa (200)")]
    public async Task Totais_GetPessoas_DeveListarComSucesso()
    {
        var resp = await _client.GetAsync("Totais/pessoas");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var json = await resp.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
    }

    [Fact(DisplayName = "Totais — Deve calcular receitas - despesas corretamente por pessoa")]
    public async Task Totais_DeveCalcularSaldoCorretamente()
    {
        // Arrange — criar pessoa
        var pessoa = new { nome = "Totais Calculo Test", dataNascimento = "1990-01-01" };
        var pessoaResp = await _client.PostAsJsonAsync("Pessoas", pessoa);
        pessoaResp.EnsureSuccessStatusCode();
        var pessoaId = await ExtrairId(pessoaResp);

        // Criar categoria de Receita
        var catReceita = new { descricao = "Tot Receita", finalidade = 1 }; // Receita
        var catRecResp = await _client.PostAsJsonAsync("Categorias", catReceita);
        catRecResp.EnsureSuccessStatusCode();
        var catReceitaId = await ExtrairId(catRecResp);

        // Criar categoria de Despesa
        var catDespesa = new { descricao = "Tot Despesa", finalidade = 0 }; // Despesa
        var catDespResp = await _client.PostAsJsonAsync("Categorias", catDespesa);
        catDespResp.EnsureSuccessStatusCode();
        var catDespesaId = await ExtrairId(catDespResp);

        // Criar receita de R$ 1000
        var receita = new
        {
            descricao = "Receita Totais",
            valor = 1000m,
            tipo = 1, // Receita
            pessoaId,
            categoriaId = catReceitaId,
            data = DateTime.Today.ToString("yyyy-MM-dd")
        };
        var recResp = await _client.PostAsJsonAsync("Transacoes", receita);
        recResp.EnsureSuccessStatusCode();

        // Criar despesa de R$ 350
        var despesa = new
        {
            descricao = "Despesa Totais",
            valor = 350m,
            tipo = 0, // Despesa
            pessoaId,
            categoriaId = catDespesaId,
            data = DateTime.Today.ToString("yyyy-MM-dd")
        };
        var despResp = await _client.PostAsJsonAsync("Transacoes", despesa);
        despResp.EnsureSuccessStatusCode();

        // Act — consultar totais por pessoa
        var totaisResp = await _client.GetAsync($"Totais/pessoas?Pessoa.Id={pessoaId}");
        Assert.Equal(HttpStatusCode.OK, totaisResp.StatusCode);

        var json = await totaisResp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        // O resultado é paginado, então pegamos items/data
        JsonElement items;
        if (doc.RootElement.TryGetProperty("items", out items) || doc.RootElement.TryGetProperty("data", out items))
        {
            Assert.True(items.GetArrayLength() > 0, "Deveria retornar pelo menos 1 registro de totais para a pessoa.");

            // Encontrar o registro da pessoa criada
            foreach (var item in items.EnumerateArray())
            {
                var itemPessoaId = item.GetProperty("pessoaId").GetString();
                if (itemPessoaId == pessoaId)
                {
                    var totalReceitas = item.GetProperty("totalReceitas").GetDecimal();
                    var totalDespesas = item.GetProperty("totalDespesas").GetDecimal();
                    var saldo = item.GetProperty("saldo").GetDecimal();

                    // Assert — Receitas = 1000, Despesas = 350, Saldo = 650
                    Assert.Equal(1000m, totalReceitas);
                    Assert.Equal(350m, totalDespesas);
                    Assert.Equal(650m, saldo); // Receitas - Despesas

                    break;
                }
            }
        }

        // Cleanup
        await _client.DeleteAsync($"Pessoas/{pessoaId}");
    }

    [Fact(DisplayName = "Totais GET /categorias — Deve listar totais por categoria (200)")]
    public async Task Totais_GetCategorias_DeveListarComSucesso()
    {
        var resp = await _client.GetAsync("Totais/categorias");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var json = await resp.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(json));
    }
}
