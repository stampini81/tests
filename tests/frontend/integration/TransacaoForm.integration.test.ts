import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import TransacaoForm from '@/components/molecules/TransacaoForm';

// Mock de props mínimas
const pessoaMenor = { id: '1', nome: 'Menor', idade: 10 };
const categorias = [
  { id: 'c1', descricao: 'Freelance', tipo: 'Receita' },
  { id: 'c2', descricao: 'Alimentação', tipo: 'Despesa' },
  { id: 'c3', descricao: 'Ambas', tipo: 'Ambas' },
];

describe('TransacaoForm integração', () => {
  it('bloqueia campo tipo e só permite categorias válidas para menor', () => {
    render(<TransacaoForm pessoa={pessoaMenor} categorias={categorias} />);
    const tipoInput = screen.getByLabelText(/tipo/i);
    expect(tipoInput).toBeDisabled();
    // Simula abrir o combobox de categoria
    fireEvent.click(screen.getByLabelText(/categoria/i));
    // Deve listar apenas categorias do tipo Despesa ou Ambas
    expect(screen.queryByText('Freelance')).toBeNull();
    expect(screen.getByText('Alimentação')).toBeDefined();
    expect(screen.getByText('Ambas')).toBeDefined();
  });
});
