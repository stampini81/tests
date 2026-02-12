Feature: Validação de finalidade da categoria em transações

  # Menor de idade



  Background:
    Given existe uma pessoa menor chamada "Teste123m" com data de nascimento "10/02/2016"
    And existe uma pessoa maior chamada "Teste123M" com data de nascimento "10/02/1990"
    And existe uma categoria chamada "CategoriaDespesa" com finalidade "despesa"
    And existe uma categoria chamada "CategoriaReceita" com finalidade "receita"
    And existe uma categoria chamada "CategoriaAmbas" com finalidade "ambas"

  @categoria_despesa @menor
  Scenario: Menor de idade pode usar categoria de despesa em despesa
    When tento cadastrar uma despesa para a pessoa menor "Teste123m" usando a categoria "CategoriaDespesa"
    Then o sistema deve permitir o cadastro

  @categoria_despesa @menor1
  Scenario: Menor de idade não pode usar categoria de despesa em receita
    When tento cadastrar uma receita para a pessoa menor "Teste123m" usando a categoria "CategoriaDespesa"
    Then o sistema deve exibir a mensagem de erro de finalidade


  @categoria_receita @menor
  Scenario: Menor de idade não pode usar categoria de receita em receita
    When tento cadastrar uma receita para a pessoa menor "Teste123m" usando a categoria "CategoriaReceita"
    Then o sistema deve exibir a mensagem de erro de finalidade

  @categoria_receita @menor1
  Scenario: Menor de idade não pode usar categoria de receita em despesa
    When tento cadastrar uma despesa para a pessoa menor "Teste123m" usando a categoria "CategoriaReceita"
    Then o sistema deve exibir a mensagem de erro de finalidade


  @categoria_ambas @menor
  Scenario: Menor de idade pode usar categoria ambas em despesa, mas não em receita
    When tento cadastrar uma receita para a pessoa menor "Teste123m" usando a categoria "CategoriaAmbas"
    Then o sistema deve exibir a mensagem de erro de finalidade
    When tento cadastrar uma despesa para a pessoa menor "Teste123m" usando a categoria "CategoriaAmbas"
    Then o sistema deve permitir o cadastro

  # Maior de idade



  @categoria_despesa @maior
  Scenario: Maior de idade pode usar categoria de despesa em despesa
    When tento cadastrar uma despesa para a pessoa maior "Teste123M" usando a categoria "CategoriaDespesa"
    Then o sistema deve permitir o cadastro

  @categoria_despesa @maior1
  Scenario: Maior de idade não pode usar categoria de despesa em receita
    When tento cadastrar uma receita para a pessoa maior "Teste123M" usando a categoria "CategoriaDespesa"
    Then o sistema deve exibir a mensagem de erro de finalidade


  @categoria_receita @maior
  Scenario: Maior de idade pode usar categoria de receita em receita
    When tento cadastrar uma receita para a pessoa maior "Teste123M" usando a categoria "CategoriaReceita"
    Then o sistema deve permitir o cadastro

  @categoria_receita @maior1
  Scenario: Maior de idade não pode usar categoria de receita em despesa
    When tento cadastrar uma despesa para a pessoa maior "Teste123M" usando a categoria "CategoriaReceita"
    Then o sistema deve exibir a mensagem de erro de finalidade


  @categoria_ambas @maior
  Scenario: Maior de idade pode usar categoria ambas em receita e despesa
    When tento cadastrar uma receita para a pessoa maior "Teste123M" usando a categoria "CategoriaAmbas"
    Then o sistema deve permitir o cadastro
    When tento cadastrar uma despesa para a pessoa maior "Teste123M" usando a categoria "CategoriaAmbas"
    Then o sistema deve permitir o cadastro
