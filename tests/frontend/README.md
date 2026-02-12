# Testes Frontend — Minhas Finanças

## Pirâmide de Testes

- **Unitários (Vitest)**: Validam regras de negócio isoladas (validação de categorias para menores)
- **Integração (Vitest)**: Validam regras de interação (filtro de categorias, bloqueio de tipo por idade)
- **E2E (Playwright)**: Validam fluxos completos no navegador via interface real

## Pré-requisitos

- Node.js 22+
- npm
- Dependências instaladas: `npm install` na pasta `tests/frontend`
- Para testes E2E: API e frontend rodando via Docker (`docker compose up --build`)

## Como rodar os testes

### 1. Instalar dependências (primeira vez)

```bash
cd tests/frontend
npm install
```

### 2. Testes unitários e de integração (Vitest)

> **Não requer** a API rodando.

```bash
cd tests/frontend
npx vitest run --reporter verbose
```

**Resultado esperado:**
```
Test Files  2 passed (2)
      Tests  7 passed (7)
```

### 3. Testes E2E (Playwright)

> **Requer** API (`localhost:5000`) e frontend (`localhost:5173`) rodando.

```bash
cd tests/frontend
npx playwright install   # Instala navegadores (primeira vez)
npx playwright test
```

**Resultado esperado:** 6 cenários E2E executados.

---

## Cobertura de Testes

### Unitários + Integração (7 testes — Vitest)

| Arquivo | Testes | Regra coberta |
|---------|--------|---------------|
| `unit/validarCategoriaParaMenor.test.ts` | 4 | Menor não pode receita, menor pode despesa, categoria Ambas |
| `integration/TransacaoForm.integration.test.ts` | 3 | Bloqueio de tipo para menor, filtro de categorias |

### E2E (6 cenários — Playwright)

| Arquivo | Cenário | Regra validada |
|---------|---------|---------------|
| `menor_nao_pode_receita.spec.ts` | Menor não pode cadastrar receita | Idade < 18 bloqueia receita |
| `menor_pode_despesa.spec.ts` | Menor pode cadastrar despesa | Idade < 18 permite despesa |
| `maior_pode_receita.spec.ts` | Maior pode cadastrar receita | Idade >= 18 permite receita |
| `categoria_finalidade.spec.ts` | Categoria respeita finalidade | Receita/Despesa/Ambas |
| `exclusao_cascata.spec.ts` | Exclusão em cascata | Deletar pessoa remove transações |
| `totais_por_pessoa.spec.ts` | Totais por pessoa | Soma receitas - despesas |

---

## Limitações

- O teste de integração valida regras de negócio via lógica pura (sem renderizar componentes React), pois os componentes dependem de providers (React Query, React Router) que exigiriam configuração extensa.
- A validação visual completa é coberta pelos testes E2E (Playwright).
- **Não foi alterado nenhum código da aplicação original.**
