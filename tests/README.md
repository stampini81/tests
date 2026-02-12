# Testes Automatizados — Minhas Finanças


## Estrutura da Pirâmide de Testes

O repositório está organizado para cobrir as principais regras de negócio do sistema, sem alterar o código da aplicação original. Os testes estão divididos em três níveis:

- **Testes Unitários**
  - Backend: xUnit (.NET)
  - Frontend: Vitest (React/TypeScript)
- **Testes de Integração**
  - Backend: xUnit
  - Frontend: Vitest
- **Testes End-to-End (E2E)**
  - Playwright (Frontend + Backend)

```
tests/
  backend/
    unit/
    integration/
  frontend/
    unit/
    integration/
    e2e/
  docs/
    bugs.md
    justificativas.md
    evidencias/
```

### Regras de negócio validadas

- **Menor de idade não pode ter receitas:**
  - Testes unitários e de integração garantem que pessoas menores de idade não conseguem cadastrar transações do tipo receita.
  - Testes frontend (unitário, integração e E2E) validam o bloqueio do campo e a exibição de mensagens adequadas.

- **Categoria só pode ser usada conforme sua finalidade (receita/despesa/ambas):**
  - Testes garantem que só é possível selecionar categorias compatíveis com o tipo de transação e perfil do usuário (ex: menor só pode usar categoria Despesa ou Ambas).

- **Exclusão em cascata de transações ao excluir pessoa:**
  - Testes de integração backend validam que ao excluir uma pessoa, as transações associadas são removidas (ou, se for o caso, sugerem soft delete para manter histórico).

Essas regras são o foco principal dos testes automatizados e de todas as validações documentadas.



## Como rodar os testes

### Pré-requisitos
- .NET 9 SDK instalado
- Bun instalado globalmente (`npm install -g bun` ou veja https://bun.sh/docs/installation)
- Dependências do frontend instaladas (veja abaixo)

### Backend (.NET/xUnit)

#### 1. Criar projetos de teste (executar uma vez)
No terminal, na raiz do projeto:
```bash
dotnet new xunit -n Backend.Unit -o tests/backend/unit
dotnet new xunit -n Backend.Integration -o tests/backend/integration
```

#### 2. Rodar testes unitários
```bash
dotnet test tests/backend/unit
```

#### 3. Rodar testes de integração
```bash
dotnet test tests/backend/integration
```

### Frontend (Vitest/Playwright)

No terminal, navegue até a pasta do frontend:
```bash
cd ExameDesenvolvedorDeTestes/web
bun install
```

#### 1. Rodar testes unitários e integração (Vitest)
```bash
bun run test
```

#### 2. Rodar testes E2E (Playwright)
```bash
bun run test:e2e
```

> **Obs:** Certifique-se de que a aplicação está rodando (via Docker ou local) antes de executar os testes de integração e E2E.

#### 3. Evidências dos testes
- Salve prints/relatórios das execuções em `tests/evidencias/`.

---


## Bugs encontrados

Todos os bugs e falhas de regras de negócio identificados durante os testes estão documentados em `tests/docs/bugs.md`, com evidências visuais e referência ao local do código.


## Justificativa das escolhas de testes

As decisões sobre o que testar, ferramentas e abordagem estão detalhadas em `tests/docs/justificativas.md`.
Os testes priorizam regras de negócio críticas, cenários reais de uso e validação de mensagens e fluxos, garantindo aderência ao enunciado e boas práticas.


## Como a pirâmide foi estruturada

- **Unitários:** Validam regras de negócio isoladas (ex: menor de idade não pode ter receita, categoria só pode ser usada conforme finalidade).
- **Integração:** Validam fluxos completos entre camadas (ex: exclusão em cascata, persistência de dados, respostas da API).
- **E2E:** Validam o sistema como um todo, simulando o uso real pelo usuário, cobrindo fluxos críticos e mensagens.


## Observações

- Não altere o código da aplicação.
- Não suba o código da aplicação para o repositório de testes.
- O foco está nas regras de negócio principais.
- Remova qualquer referência à Maxiprod antes de publicar.
- Testes manuais via Swagger/Postman foram utilizados apenas para apoio e validação exploratória, mas toda a automação está no repositório.

---

Dúvidas: rh@maxiprod.com.br
