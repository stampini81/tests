
# Testes Extras (BDD, Selenium WebDriver, Page Object, Configuração YAML)


## Estrutura de Pastas e Arquivos



```
features/
   specs/                     # Cenários Gherkin (.feature)
      menor_receita.feature
      categoria_finalidade.feature
   step_definitions/
      selenium_steps.rb         # Steps Cucumber usando Page Object
   support/
      env.rb                   # Configuração Selenium, carregamento do YAML
   pages/
      pessoa_page.rb           # Page Object para tela de pessoas
      categoria_page.rb        # Page Object para tela de categorias
      transacao_page.rb        # Page Object para tela de transações
   config/
      test_config.yaml         # Configuração de ambientes (local, homolog, produção)
cypress/
   exclusao_cascata.cy.js     # Teste E2E Cypress para exclusão em cascata
Gemfile                      # Dependências Ruby
```

### Linguagens e Ferramentas
- Ruby (Cucumber, Selenium WebDriver)
- Page Object Pattern para organização dos testes
- YAML para configuração de ambientes
- Cypress (para alguns testes E2E)


## Como rodar os testes

### Cucumber + Selenium WebDriver (Ruby)
1. Instale Ruby ([https://rubyinstaller.org/](https://rubyinstaller.org/))
2. Instale as dependências:
   ```sh
   bundle install
   ```
3. Execute os testes (ambiente local):
   ```sh
   bundle exec cucumber
   ```
   Para rodar em homologação ou produção:
   ```sh
   # No Windows PowerShell
   $env:TEST_ENV="homolog"; bundle exec cucumber
   # Ou para produção
   $env:TEST_ENV="producao"; bundle exec cucumber
   ```

> Os testes usam o padrão Page Object, centralizando seletores e interações nas classes em `features/pages/`.
> A configuração de ambiente (URLs, timeout) está em `features/config/test_config.yaml`.


### Cypress
1. Instale Cypress:
   ```sh
   npm install cypress --save-dev
   ```
2. Execute Cypress:
   ```sh
   npx cypress open
   ```


## Observações
- Os testes seguem os cenários propostos no enunciado.
- Page Object facilita manutenção e reuso dos steps.
- Configuração de ambiente via YAML permite rodar em local, homolog ou produção facilmente.
- Exclusão em cascata está implementada como E2E (Cypress).
- Não altere o código da aplicação.
- Documente bugs encontrados em `docs/bugs.md`.
