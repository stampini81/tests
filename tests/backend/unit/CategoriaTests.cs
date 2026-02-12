using Xunit;
using MinhasFinancas.Domain.Entities;

namespace Backend.Unit;

/// <summary>
/// Testes unitários para a entidade Categoria — regra de finalidade.
/// Regra: Categoria só pode ser usada conforme sua finalidade (receita/despesa/ambas).
/// </summary>
public class CategoriaTests
{
    [Fact(DisplayName = "Categoria de Despesa deve permitir apenas despesa")]
    public void CategoriaDespesa_DevePermitirApenasDespesa()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Aluguel",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Act & Assert
        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Despesa));
        Assert.False(categoria.PermiteTipo(Transacao.ETipo.Receita));
    }

    [Fact(DisplayName = "Categoria de Receita deve permitir apenas receita")]
    public void CategoriaReceita_DevePermitirApenasReceita()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Act & Assert
        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Receita));
        Assert.False(categoria.PermiteTipo(Transacao.ETipo.Despesa));
    }

    [Fact(DisplayName = "Categoria Ambas deve permitir receita e despesa")]
    public void CategoriaAmbas_DevePermitirReceitaEDespesa()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Transferência",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        // Act & Assert
        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Receita));
        Assert.True(categoria.PermiteTipo(Transacao.ETipo.Despesa));
    }

    [Fact(DisplayName = "Categoria de Despesa não deve permitir receita")]
    public void CategoriaDespesa_NaoDevePermitirReceita()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Act & Assert
        Assert.False(categoria.PermiteTipo(Transacao.ETipo.Receita));
    }

    [Fact(DisplayName = "Categoria de Receita não deve permitir despesa")]
    public void CategoriaReceita_NaoDevePermitirDespesa()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Freelance",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Act & Assert
        Assert.False(categoria.PermiteTipo(Transacao.ETipo.Despesa));
    }
}
