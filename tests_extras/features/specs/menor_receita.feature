Feature: Cadastro de receitas para pessoas
  Scenario: Menor de idade não pode cadastrar receita
    Given existe uma pessoa cadastrada com idade menor que 18 anos
    When tento cadastrar uma receita para essa pessoa
    Then o sistema deve impedir o cadastro e exibir uma mensagem de erro
