/// <reference types="cypress" />

/**
 * Teste E2E Cypress — Exclusão em cascata
 *
 * Cenário: Ao excluir uma pessoa, todas as transações associadas
 * devem ser removidas automaticamente (exclusão em cascata).
 *
 * Pré-requisito: API (localhost:5000) e frontend (localhost:5173) rodando.
 */

const API_BASE = 'http://localhost:5000/api/v1';

describe('Exclusão em cascata — Pessoa e Transações', () => {

    let pessoaId;
    let categoriaId;

    before(() => {
        // Criar pessoa via API
        cy.request('POST', `${API_BASE}/Pessoas`, {
            nome: 'Cypress Cascata Test',
            dataNascimento: '1990-01-01'
        }).then((resp) => {
            expect(resp.status).to.eq(201);
            pessoaId = resp.body.id;
        });

        // Criar categoria de Despesa via API
        cy.request('POST', `${API_BASE}/Categorias`, {
            descricao: 'Cypress Cat Cascata',
            finalidade: 0 // Despesa
        }).then((resp) => {
            expect(resp.status).to.eq(201);
            categoriaId = resp.body.id;
        });
    });

    it('Deve criar transação, excluir pessoa e verificar que a pessoa não existe mais', () => {
        // Criar transação associada à pessoa
        cy.request('POST', `${API_BASE}/Transacoes`, {
            descricao: 'Despesa Cypress Cascata',
            valor: 200,
            tipo: 0, // Despesa
            pessoaId: pessoaId,
            categoriaId: categoriaId,
            data: '2024-06-01'
        }).then((resp) => {
            expect(resp.status).to.eq(201);
        });

        // Excluir pessoa via API
        cy.request('DELETE', `${API_BASE}/Pessoas/${pessoaId}`).then((resp) => {
            expect(resp.status).to.eq(204);
        });

        // Verificar que a pessoa não existe mais
        cy.request({
            method: 'GET',
            url: `${API_BASE}/Pessoas/${pessoaId}`,
            failOnStatusCode: false
        }).then((resp) => {
            expect(resp.status).to.eq(404);
        });

        // Verificar na interface que a pessoa não aparece mais
        cy.visit('/pessoas');
        cy.get('body').should('not.contain', 'Cypress Cascata Test');
    });

    it('Deve verificar na interface que a exclusão funcionou', () => {
        // Navegar para transações e verificar que não há transação órfã visível
        cy.visit('/transacoes');
        // A transação associada à pessoa excluída não deve aparecer
        cy.get('body').should('not.contain', 'Despesa Cypress Cascata');
    });
});
