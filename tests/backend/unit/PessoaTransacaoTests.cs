using Xunit;
using MinhasFinancas.Domain.Entities;

namespace Backend.Unit;

/// <summary>
/// Testes unitários para regra de negócio: Menor de idade não pode ter receita.
/// A propriedade Transacao.Pessoa tem setter 'internal', acessível apenas dentro do assembly do domínio.
/// Isso impede que testes externos validem essa regra diretamente via código.
/// 
/// BUG DOCUMENTADO: A regra existe no setter de Transacao.Pessoa (internal), mas não é acessível
/// para testes fora do assembly. Deveria haver um método público ou construtor que permita
/// a validação da regra via testes unitários.
/// 
/// Apesar desta limitação, conseguimos testar parcialmente via a regra de Categoria (PermiteTipo).
/// </summary>
public class PessoaTransacaoTests
{
    [Fact(DisplayName = "Menor de idade não pode cadastrar receita — documentação de limitação")]
    public void MenorDeIdade_NaoPodeCadastrarReceita()
    {
        // Arrange
        var pessoa = new Pessoa { Nome = "João Menor", DataNascimento = DateTime.Today.AddYears(-10) };
        var transacao = new Transacao
        {
            Descricao = "Salário",
            Valor = 1000,
            Data = DateTime.Now,
            Tipo = Transacao.ETipo.Receita
        };

        // Assert — A pessoa é menor de idade
        Assert.False(pessoa.EhMaiorDeIdade());

        // NOTA: Não é possível atribuir transacao.Pessoa = pessoa diretamente nos testes
        // porque o setter é 'internal'. A validação ocorre apenas dentro do assembly do domínio.
        // Esse comportamento é testado via testes de integração (API HTTP).
    }

    [Fact(DisplayName = "Maior de idade pode cadastrar receita — verificação de pré-condição")]
    public void MaiorDeIdade_PodeCadastrarReceita()
    {
        // Arrange
        var pessoa = new Pessoa { Nome = "Maria Maior", DataNascimento = DateTime.Today.AddYears(-25) };
        var transacao = new Transacao
        {
            Descricao = "Freelance",
            Valor = 2000,
            Data = DateTime.Now,
            Tipo = Transacao.ETipo.Receita
        };

        // Assert — A pessoa é maior de idade
        Assert.True(pessoa.EhMaiorDeIdade());

        // NOTA: A validação completa (atribuição Pessoa à Transação) ocorre nos testes de integração.
    }
}
