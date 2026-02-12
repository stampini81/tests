using Xunit;
using System;
using MinhasFinancas.Domain.Entities;

namespace Backend.Unit
{
    public class PessoaTransacaoTests
    {
        [Fact]
        public void MenorDeIdade_NaoPodeCadastrarReceita()
        {
            // Arrange
            var pessoa = new Pessoa("João Menor", new DateTime(DateTime.Now.Year - 10, 1, 1)); // 10 anos
            var transacao = new Transacao("Salário", 1000, DateTime.Now, TipoTransacao.Receita, pessoa.Id, "Salário");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pessoa.AdicionarTransacao(transacao));
        }
    }
}
