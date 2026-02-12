Feature: Bloqueio de receita para menor de idade

  Scenario: Menor de idade não pode registrar receita
    Given existe uma pessoa menor chamada "Testepessoa" com data de nascimento "11/02/2016"
    And existe uma categoria cadastrada com finalidade "receita"
    When tento cadastrar uma receita para a pessoa menor "Testepessoa"
    Then o sistema deve exibir a mensagem de bloqueio de receita para menor
