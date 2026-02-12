using Xunit;
using MinhasFinancas.Domain.Entities;

namespace Backend.Unit;

/// <summary>
/// Testes unitários para a entidade Pessoa — regras de idade.
/// </summary>
public class PessoaTests
{
    [Fact(DisplayName = "Pessoa com 15 anos deve ser menor de idade")]
    public void MenorDeIdade_DeveRetornarFalsoParaEhMaiorDeIdade()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "João Menor",
            DataNascimento = DateTime.Today.AddYears(-15)
        };

        // Act & Assert
        Assert.False(pessoa.EhMaiorDeIdade());
        Assert.Equal(15, pessoa.Idade);
    }

    [Fact(DisplayName = "Pessoa com 18 anos deve ser maior de idade")]
    public void MaiorDeIdade_DeveRetornarVerdadeiroParaEhMaiorDeIdade()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Maria Maior",
            DataNascimento = DateTime.Today.AddYears(-18)
        };

        // Act & Assert
        Assert.True(pessoa.EhMaiorDeIdade());
        Assert.Equal(18, pessoa.Idade);
    }

    [Fact(DisplayName = "Pessoa com 30 anos deve ser maior de idade")]
    public void Adulto_DeveRetornarVerdadeiroParaEhMaiorDeIdade()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Carlos Adulto",
            DataNascimento = DateTime.Today.AddYears(-30)
        };

        // Act & Assert
        Assert.True(pessoa.EhMaiorDeIdade());
        Assert.Equal(30, pessoa.Idade);
    }

    [Fact(DisplayName = "Pessoa com 17 anos deve ser menor de idade")]
    public void QuaseMaior_DeveRetornarFalsoParaEhMaiorDeIdade()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Ana Quase Maior",
            DataNascimento = DateTime.Today.AddYears(-17).AddDays(-364)
        };

        // Act & Assert
        Assert.False(pessoa.EhMaiorDeIdade());
    }

    [Fact(DisplayName = "Pessoa recém-nascida deve ter idade 0")]
    public void RecemNascida_DeveRetornarIdadeZero()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Bebê",
            DataNascimento = DateTime.Today
        };

        // Act & Assert
        Assert.Equal(0, pessoa.Idade);
        Assert.False(pessoa.EhMaiorDeIdade());
    }
}
