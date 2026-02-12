# Testes Automatizados — Minhas Finanças
[![Test Validation](https://github.com/stampini81/tests/actions/workflows/tests.yml/badge.svg)](https://github.com/stampini81/tests/actions/workflows/tests.yml)

## Estrutura da Pirâmide de Testes

O repositório cobre as principais regras de negócio do sistema, **sem alterar o código da aplicação original**. Os testes estão divididos em três níveis:

```
tests/
  backend/
    unit/          → 12 testes (xUnit/.NET)
    integration/   → 26 testes (xUnit/.NET, todos passando)
  frontend/
    unit/          → 4 testes (Vitest)
    integration/   → 3 testes (Vitest)
    e2e/           → 6 cenários (Playwright)
docs/
  bugs.md          → 25 bugs documentados
evidencias/        → Capturas de tela dos bugs
```

### Regras de negócio validadas

| Regra | Unit | Integration | E2E |
|-------|------|-------------|-----|
| Menor de idade não pode ter receita | ✅ | ✅ | ✅ |
| Categoria conforme finalidade (receita/despesa/ambas) | ✅ | ✅ | ✅ |
| Exclusão em cascata de transações ao excluir pessoa | — | ✅ | ✅ |
| Maior de idade pode ter receita | ✅ | ✅ | ✅ |
| Menor de idade pode ter despesa | — | ✅ | ✅ |
| Totais por pessoa (receitas - despesas = saldo) | — | ✅ | ✅ |
| CRUD Pessoa (POST/GET/PUT/DELETE) | — | ✅ | — |
| CRUD Categoria (POST/GET — PUT/DELETE = bug) | — | ✅ | — |
| CRUD Transação (POST/GET — PUT/DELETE = bug) | — | ✅ | — |

## Pré-requisitos

| Ferramenta | Versão | Finalidade |
|------------|--------|-----------|
| .NET SDK | 9.0+ | Compilar e rodar testes backend (xUnit) |
| Node.js | 22+ | Rodar testes frontend (Vitest, Playwright) |
| Docker + Docker Compose | — | Subir API e banco de dados |
| npm | — | Instalar dependências do frontend |

## Como rodar os testes

### 1. Subir a aplicação (obrigatório para testes de integração e E2E)

```bash
cd ExameDesenvolvedorDeTestes
docker compose up --build
# Aguarde a API estar disponível em http://localhost:5000
# Frontend em http://localhost:5173
```

### 2. Backend — Testes Unitários

> **Não requer** a API rodando.

```bash
# Na raiz do projeto:
dotnet restore tests/backend/unit/Backend.Unit.csproj --configfile nuget.config
dotnet test tests/backend/unit/Backend.Unit.csproj

# Resultado esperado:
# Aprovado! — Failed: 0, Passed: 12, Total: 12
```

### 3. Backend — Testes de Integração

> **Requer** a API rodando (`docker compose up --build`).

```bash
dotnet restore tests/backend/integration/Backend.Integration.csproj --configfile nuget.config
dotnet test tests/backend/integration/Backend.Integration.csproj

# Resultado esperado:
# Aprovado! — Failed: 0, Passed: 26, Total: 26
```

### 4. Frontend — Testes Unitários e Integração (Vitest)

> **Não requer** a API rodando.

```bash
cd tests/frontend
npm install
npx vitest run --reporter verbose

# Resultado esperado:
# Test Files 2 passed (2)
# Tests 7 passed (7)
```

### 5. Frontend — Testes E2E (Playwright)

> **Requer** API (`localhost:5000`) e frontend (`localhost:5173`) rodando.

```bash
cd tests/frontend
npx playwright install   # Instala navegadores (primeira vez)
npx playwright test

# Resultado esperado: 6 cenários E2E
```

---

## Bugs encontrados

Todos os **25 bugs** identificados estão documentados em [`docs/bugs.md`](docs/bugs.md), com:
- Descrição do problema e tipo (API, Negócio, Usabilidade, Visual)
- Severidade (Alta, Média, Baixa)
- Passos de reprodução
- Comportamento esperado vs. observado
- Evidências visuais (capturas de tela na pasta `evidencias/`)
- Localização no código-fonte

## Observações

- **Não foi alterado nenhum código da aplicação original.**
- Os testes de integração que verificam PUT/DELETE em Categoria e Transação documentam os bugs retornando 405.
- Os setters `internal` de `Transacao.Pessoa` e `Transacao.Categoria` impedem testes unitários diretos de regras de negócio fora do assembly do domínio.
- Testes priorizam regras de negócio críticas e cenários reais de uso.
