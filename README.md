# Testes Automatizados - Controle de Gastos Residenciais

## Como rodar os testes

### Backend (.NET)
- **Unitários:**
  ```sh
  dotnet test tests/backend/unit
  ```
- **Integração:**
  ```sh
  dotnet test tests/backend/integration
  ```


### Frontend (React/TypeScript)
- **Unitários e Integração:**
  ```sh
  cd ExameDesenvolvedorDeTestes/web
  bun run test
  ```
- **E2E:**
  ```sh
  cd ExameDesenvolvedorDeTestes/web
  bun run test:e2e
  ```

> **Nota:** Os testes E2E devem ser criados em `ExameDesenvolvedorDeTestes/web/tests/e2e` para que o Playwright reconheça e execute corretamente, pois as dependências estão instaladas na pasta `web`.

## Pirâmide de Testes

- **Unitários:** Validam regras isoladas (ex: menor de idade não pode ter receita)
- **Integração:** Validam interações entre componentes/módulos
- **E2E:** Validam fluxos completos no sistema

## Bugs encontrados

Veja `docs/bugs.md`.

## Justificativa das escolhas

- Priorizamos regras de negócio críticas.
- Testes E2E cobrem fluxos principais do usuário.
- Testes de integração garantem comunicação entre módulos.
