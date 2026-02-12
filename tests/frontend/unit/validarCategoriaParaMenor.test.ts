import { describe, it, expect } from 'vitest';
import { validarCategoriaParaMenor } from '@/lib/validacoes';

// Função de exemplo para validação (deve ser mockada ou implementada no teste)
// function validarCategoriaParaMenor(tipoCategoria: 'Receita' | 'Despesa' | 'Ambas', tipoTransacao: 'Receita' | 'Despesa', idade: number): boolean {
//   if (idade < 18 && tipoTransacao === 'Receita' && tipoCategoria !== 'Ambas') return false;
//   return true;
// }

describe('Validação de categoria para menor de idade', () => {
  it('não permite menor cadastrar receita em categoria Receita', () => {
    const resultado = validarCategoriaParaMenor('Receita', 'Receita', 10);
    expect(resultado).toBe(false);
  });

  it('permite menor cadastrar despesa em categoria Despesa', () => {
    const resultado = validarCategoriaParaMenor('Despesa', 'Despesa', 10);
    expect(resultado).toBe(true);
  });

  it('permite menor cadastrar despesa em categoria Ambas', () => {
    const resultado = validarCategoriaParaMenor('Ambas', 'Despesa', 10);
    expect(resultado).toBe(true);
  });

  it('não permite menor cadastrar receita em categoria Ambas', () => {
    const resultado = validarCategoriaParaMenor('Ambas', 'Receita', 10);
    expect(resultado).toBe(false);
  });
});
