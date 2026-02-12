
# Testes Automatizados — Frontend (web)

Este diretório contém os testes automatizados do frontend da aplicação Minhas Finanças.

## Estrutura dos testes

- `tests/e2e/` — Testes End-to-End (E2E) com Playwright
- `tests/unit/` — Testes unitários (Vitest)
- `tests/integration/` — Testes de integração (Vitest)

## Pré-requisitos

- [Bun](https://bun.sh/) instalado globalmente
- Dependências instaladas: `bun install`
- Variáveis de ambiente configuradas em `.env` (se necessário)

## Como rodar os testes

### Unitários e Integração

```sh
bun run test
```

### E2E (End-to-End)

```sh
bun run test:e2e
```

> Os testes E2E devem ser criados em `web/tests/e2e` para que o Playwright reconheça e execute corretamente.

## Ferramentas utilizadas

- [Playwright](https://playwright.dev/) — Testes E2E
- [Vitest](https://vitest.dev/) — Testes unitários e integração
- [Bun](https://bun.sh/) — Gerenciador de pacotes e execução de scripts

## Observações

- Certifique-se de que a aplicação backend está rodando antes de executar testes de integração/E2E.
- O arquivo `.env` pode ser necessário para configurar endpoints de API ou variáveis de ambiente dos testes.
- Resultados de execuções E2E são salvos em `test-results/`.

---

Para mais detalhes sobre a pirâmide de testes, regras de negócio e bugs conhecidos, consulte o README principal em `/tests/README.md`.
