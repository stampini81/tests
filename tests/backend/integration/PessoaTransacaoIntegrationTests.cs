using Xunit;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Integration
{
    public class PessoaTransacaoIntegrationTests
    {
        [Fact]
        public void Deve_Excluir_Transacoes_Ao_Excluir_Pessoa()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1, Nome = "Maria" };
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, PessoaId = pessoa.Id, Valor = 100 },
                new Transacao { Id = 2, PessoaId = pessoa.Id, Valor = 200 }
            };
            var pessoaRepo = new FakePessoaRepository(new List<Pessoa> { pessoa });
            var transacaoRepo = new FakeTransacaoRepository(transacoes);
            var service = new PessoaService(pessoaRepo, null, transacaoRepo);

            // Act
            service.ExcluirPessoa(pessoa.Id);

            // Assert
            Assert.Empty(transacaoRepo.ObterPorPessoaId(pessoa.Id));
        }
    }

    // Fakes para simular repositórios em memória
    public class FakePessoaRepository : IPessoaRepository
    {
        private List<Pessoa> _pessoas;
        public FakePessoaRepository(List<Pessoa> pessoas) => _pessoas = pessoas;
        public void Excluir(int id) => _pessoas.RemoveAll(p => p.Id == id);
        // ...outros métodos necessários...
    }
    public class FakeTransacaoRepository : ITransacaoRepository
    {
        private List<Transacao> _transacoes;
        public FakeTransacaoRepository(List<Transacao> transacoes) => _transacoes = transacoes;
        public IEnumerable<Transacao> ObterPorPessoaId(int pessoaId) => _transacoes.Where(t => t.PessoaId == pessoaId);
        public void ExcluirPorPessoaId(int pessoaId) => _transacoes.RemoveAll(t => t.PessoaId == pessoaId);
        // ...outros métodos necessários...
    }
}
