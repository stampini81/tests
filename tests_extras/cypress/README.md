# Cypress Tests (Testes Extras)

Este diretório contém testes E2E implementados com Cypress para validar cenários específicos que se beneficiam da velocidade e estabilidade da ferramenta.

## Cenário Principal: Exclusão em Cascata (exclusão_cascata.cy.js)

O arquivo `exclusao_cascata.cy.js` valida o fluxo crítico de exclusão de dados relacionados:
1. Cria Pessoa, Categoria e Transação via API.
2. Exclui a Pessoa via API.
3. Verifica que a API retorna 404 para a Pessoa excluída.
4. Verifica que a Transação não aparece mais na listagem da UI (`/transacoes`).

**Status Recente**: ✅ Teste passando (2/2 assercões - API e UI).

## Como Executar

Estando na raiz do projeto `tests_extras/cypress`:

```bash
# Executar este teste específico no terminal (headless)
npx cypress run --spec "e2e/exclusao_cascata.cy.js" --project .

# Abrir interface gráfica do Cypress
npx cypress open --project .
```

## Configuração

O arquivo `cypress.config.js` define a `baseUrl` padrão como `http://localhost:5173` (Frontend) e espera que o backend esteja rodando em `http://localhost:5000`.

> **Importante**: Garanta que o ambiente Docker esteja rodando (`docker compose up`) antes de iniciar os testes.
