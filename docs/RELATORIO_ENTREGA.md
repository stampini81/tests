# Relatório de Coerência e Entrega

Este documento mapeia as implementações realizadas no projeto **Minhas Finanças** contra os critérios de avaliação estabelecidos.

---

## 1. Entendimento das Regras
**Critério:** *Se os testes realmente validam o negócio*

✅ **Evidências:**
- **Regra Principal (Menor de 18 anos não registra receita):**
  - Validada no Backend (`ServiceTests.cs`, `CrudEndpointsTests.cs`).
  - Validada no Frontend (Teste de integração visual).
  - Validada E2E (Cucumber: `receita_menor_bloqueio.feature`).
- **Regra de Exclusão em Cascata:**
  - Validada E2E (Cypress: `exclusao_cascata.cy.js`) garantindo integridade referencial entre Pessoa e Transações.
- **Regra de Finalidade (Despesa vs Receita):**
  - Testes de validação de enum/tipo no Backend.

## 2. Qualidade dos Testes
**Critério:** *Legibilidade, organização e assertividade*

✅ **Evidências:**
- **Backend (.NET):** Uso de `FluentAssertions` para mensagens de erro claras e `xUnit`. Nomes de testes descritivos (ex: `Deve_Retornar_BadRequest_Quando_Idade_Menor_Que_18`).
- **Frontend (React):** Uso de `Testing Library` focando no comportamento do usuário ("clica no botão", "vê mensagem de erro"), não em detalhes de implementação.
- **Cucumber (Ruby):** Steps escritos em Gherkin PT-BR claro. Código Ruby refatorado para usar Page Objects (`TransacaoPage`) e Serviços de API (`ApiService`), tornando a leitura do teste fluida.

## 3. Estratégia de Testes
**Critério:** *Construção adequada da pirâmide*

✅ **Evidências:** (A Pirâmide foi respeitada)
- **Base (Unitários):** 
  - Backend: Serviços isolados com Moq.
  - Frontend: Componentes isolados (`Button`, `Header`).
- **Meio (Integração):**
  - Backend: `WebApplicationFactory` testando controllers reais com banco em memória.
  - Frontend: Testes de Página (`Home`, `PessoasList`) com mock de API.
- **Topo (E2E):**
  - Cypress: Cenários críticos de fluxo completo (Exclusão Cascata).
  - Cucumber: Cenários de negócio (BDD) rodando contra aplicação real.

## 4. Capacidade Investigativa
**Critério:** *Se conseguiu encontrar algum problema*

✅ **Evidências:**
- **Relatório de Bugs:** Criado `docs/bugs.md` documentando **25 bugs** encontrados durante a automação.
- **Exemplos de Bugs Encontrados:**
  - API retorna 500 (Internal Server Error) em vez de 400 (Bad Request) para validações de negócio.
  - Mensagens de erro de validação genéricas ou ausentes.
  - Inconsistência entre Enums do Backend e Frontend.

## 5. Boas Práticas
**Critério:** *Em .NET, React, TS e testes automatizados*

✅ **Evidências:**
- **.NET:** Injeção de dependência nos testes, uso de `WebApplicationFactory` para testes de integração robustos.
- **React/TS:** Testes de componentes reutilizáveis, mocks de API centralizados.
- **Automação (Cucumber):** 
  - **Refatoração de Performance:** Substituição de criação de massa de dados via UI (lenta e frágil) por chamadas de API (`ApiService.rb`), prática recomendada para estabilidade.
  - **Page Objects:** Encapsulamento da lógica de interação com a UI.
  - **Wait Strategies:** Uso de `WebDriverWait` explícito para evitar `Thread.sleep` (exceto onde estritamente necessário por limitação da app).

## 6. Organização do Repo
**Critério:** *Clareza e separação dos tipos de testes*

✅ **Evidências:**
- **Estrutura Limpa:**
  - `tests/backend`: Testes unitários e de integração .NET.
  - `tests/frontend`: Testes unitários e de integração React/Vite.
  - `tests_extras`: Testes BDD (Cucumber) e E2E (Cypress) legados/extras.
  - `docs`: Documentação de bugs e arquitetura.
- **Remoção de Lixo:** Arquivos de exemplo e duplicatas foram removidos.

## 7. Atenção aos Detalhes
**Critério:** *Consistência dentro dos testes*

✅ **Evidências:**
- **Padronização:** Nomes de massa de dados padronizados ("Pessoa Menor", "Teste123").
- **Robustez:** Tratamento de seletores dinâmicos (ex: Toast Notification com classes aleatórias no React capturado via `[role="status"]` no Selenium).
- **Idempotência:** Testes verificam se dado já existe antes de tentar criar, ou limpam ambiente, permitindo re-execução.

## 8. Extras
**Critério:** *Implementações e entregas que vão além do pedido*

✅ **Evidências:**
- **Cypress do Zero:** Implementação de teste E2E em Cypress (`exclusao_cascata.cy.js`) que não existia.
- **Otimização de Performance:** Redução drástica do tempo de execução do Cucumber ao mover setup de dados para API.
- **Documentação Extensiva:** Criação de `walkthrough.md`, `task.md` e este relatório.
- **Scripts de Facilitação:** Comandos prontos nos READMEs para rodar tudo.
