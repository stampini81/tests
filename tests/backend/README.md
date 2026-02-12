# Testes Backend — Minhas Finanças

## Pirâmide de Testes

- **Unitários (12 testes)**: Validam regras isoladas do domínio (Pessoa, Categoria)
- **Integração (26 testes)**: Validam CRUD, regras de negócio e totais via API HTTP real

## Pré-requisitos

- .NET 9 SDK
- Docker rodando (`docker compose up --build`) para testes de integração
- NuGet restaurado com `--configfile nuget.config`

## Como rodar os testes

### Restaurar dependências (executar uma vez)

```bash
dotnet restore tests/backend/unit/Backend.Unit.csproj --configfile nuget.config
dotnet restore tests/backend/integration/Backend.Integration.csproj --configfile nuget.config
```

### Testes unitários (não requer API)

```bash
dotnet test tests/backend/unit/Backend.Unit.csproj
```

**Resultado esperado:**
```
Aprovado! — Failed: 0, Passed: 12, Total: 12
```

### Testes de integração (requer `docker compose up --build`)

```bash
dotnet test tests/backend/integration/Backend.Integration.csproj
```

**Resultado esperado:**
```
Aprovado! — Failed: 0, Passed: 26, Total: 26
```

---

## Cobertura de Testes

### Testes Unitários (12 testes)

| Arquivo | Testes | Regra coberta |
|---------|--------|---------------|
| `UnitTest1.cs` (PessoaTests) | 5 | Cálculo de idade, `EhMaiorDeIdade` |
| `CategoriaTests.cs` | 5 | Finalidade (Despesa/Receita/Ambas), `PermiteTipo` |
| `PessoaTransacaoTests.cs` | 2 | Documentação da limitação do setter `internal` |

### Testes de Integração (26 testes — `CrudEndpointsTests.cs`)

#### CRUD por Entidade

| Entidade | POST | GET (list) | GET/{id} | PUT | DELETE |
|----------|------|------------|----------|-----|--------|
| **Pessoa** | ✅ 201 | ✅ 200 | ✅ 200 / 404 | ✅ 204 / 404 | ✅ 204 |
| **Categoria** | ✅ 201 | ✅ 200 | ✅ 200 | ❌ 405 (bug) | ❌ 405 (bug) |
| **Transação** | ✅ 201 | ✅ 200 | ✅ 200 | ❌ 405 (bug) | ❌ 405 (bug) |
| **Totais** | — | ✅ /pessoas | ✅ /categorias | — | — |

#### Regras de Negócio

| Cenário | Resultado |
|---------|-----------|
| Menor de idade não pode registrar receita | ✅ |
| Maior de idade pode registrar receita | ✅ |
| Menor de idade pode registrar despesa | ✅ |
| Categoria incompatível com tipo de transação | ✅ |
| Exclusão em cascata (pessoa → transações) | ✅ |
| Cálculo de totais (R$1000 - R$350 = R$650) | ✅ |
| DELETE Pessoa retorna 204 para ID inexistente (bug) | ✅ |

---

## Limitações

- Os setters `Transacao.Pessoa` e `Transacao.Categoria` são `internal`, impedindo testes unitários diretos fora do assembly do domínio.
- A API não implementa PUT e DELETE para Categorias e Transações (retorna 405).
- DELETE Pessoa retorna 204 para ID inexistente (deveria ser 404).
- Todos os problemas estão documentados em `docs/bugs.md`.
- **Não foi alterado nenhum código da aplicação original.**
