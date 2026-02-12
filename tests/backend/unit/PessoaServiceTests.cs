using Xunit;
using Moq;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using System;

namespace Backend.Unit
{
    public class PessoaServiceTests
    {
        [Fact]
        public void Nao_Deve_Permitir_Receita_Para_Menor_De_Idade()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1, Nome = "Joao", DataNascimento = DateTime.Today.AddYears(-15) };
            var categoria = new Categoria { Id = 1, Nome = "Salário", Tipo = "Receita" };
            var transacao = new Transacao { Id = 1, PessoaId = pessoa.Id, CategoriaId = categoria.Id, Valor = 100, Tipo = "Receita" };

            var pessoaRepo = new Mock<IPessoaRepository>();
            var categoriaRepo = new Mock<ICategoriaRepository>();
            var transacaoRepo = new Mock<ITransacaoRepository>();

            var service = new PessoaService(pessoaRepo.Object, categoriaRepo.Object, transacaoRepo.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.AdicionarReceitaParaPessoa(pessoa, transacao));
        }
    }
}
