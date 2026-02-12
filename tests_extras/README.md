# Testes Extras

Este diretório contém testes implementados com diferentes tecnologias e padrões para validar o sistema Minhas Finanças:

1. **Cucumber + Selenium WebDriver (Ruby)**: Automação BDD com padrão Page Object.
2. **Cypress (JavaScript)**: Testes E2E modernos para cenários complexos (exclusão em cascata).

## Estrutura

```
tests_extras/
  features/                    # Cucumber BDD
    specs/                     # Arquivos .feature (Gherkin)
    step_definitions/          # Implementação dos steps (Ruby)
    pages/                     # Page Objects (abstração da UI)
    support/                   # Configuração e hooks
    config/                    # Arquivos YAML de configuração
  cypress/                     # Cypress E2E
    exclusao_cascata.cy.js     # Cenário de exclusão em cascata
  Gemfile                      # Dependências Ruby
```

## Configuração

### Ruby (Cucumber)

1. Instale o Ruby 3.x+
2. Instale as dependências:
   ```bash
   bundle install
   ```
3. Execute os testes:
   ```bash
   bundle exec cucumber
   ```

### Execução Otimizada (Tags)

Para evitar rodar todos os testes de uma vez e ganhar tempo, utilize **tags** para filtrar os cenários. Esta é uma boa prática recomendada.

**Comandos comuns:**

*   **Rodar um grupo específico:**
    ```bash
    bundle exec cucumber -t @menor
    ```
*   **Rodar combinando tags (E):**
    ```bash
    bundle exec cucumber -t @categoria_despesa -t @menor1
    ```
*   **Rodar ignorando uma tag (NÃO):**
    ```bash
    bundle exec cucumber -t "not @wip"
    ```

**Tags Disponíveis neste Projeto:**
*   `@menor` / `@menor1`: Cenários envolvendo pessoas menores de idade.
*   `@maior`: Cenários envolvendo pessoas maiores de idade.
*   `@categoria_despesa`: Testes específicos de Categoria do tipo Despesa.
*   `@categoria_receita`: Testes específicos de Categoria do tipo Receita.
*   `@categoria_ambas`: Testes específicos de Categoria do tipo Ambas.

   
   > **Nota**: Os steps foram otimizados para serem **idempotentes**. Se o registro (Pessoa/Categoria) já existir na interface, ele não tentará criar novamente, agilizando a execução.

### Cypress

1. Instale o Node.js e dependências (na raiz ou global)
2. Para rodar o teste específico:
   ```bash
   # Necessário ter o Cypress instalado
   npx cypress run --spec "tests_extras/cypress/exclusao_cascata.cy.js" --project .
   # Ou abrir a interface:
   npx cypress open --project .
   ```
   > Observação: O arquivo `cypress.config.js` está em `tests_extras/cypress/`, então pode ser necessário passar o caminho da config se não detectar automaticamente.

## Cenários Cobertos

- **Menor de idade não pode receita**: Validado via Cucumber (`menor_receita.feature`) e Cypress.
- **Categoria e Finalidade**: Validação cruzada de Receita/Despesa/Ambas para Maior/Menor (`categoria_finalidade.feature`).
- **Exclusão em Cascata**: Implementado em Cypress para garantir que remover uma pessoa remove suas transações.

## Observações

- **Idempotência**: O Background dos testes Cucumber verifica se os dados já existem antes de criar, evitando duplicação e erros.
- **Ambientes**: O arquivo `features/config/test_config.yaml` permite configurar URLs para Local, Homologação e Produção.
