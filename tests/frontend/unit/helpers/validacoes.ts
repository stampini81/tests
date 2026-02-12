/**
 * Valida se uma categoria pode ser usada por um menor de idade
 * conforme a regra de negócio: Menor de idade não pode ter receita.
 *
 * @param tipoCategoria - Finalidade da categoria (Receita, Despesa ou Ambas)
 * @param tipoTransacao - Tipo da transação (Receita ou Despesa)
 * @param idade - Idade da pessoa
 * @returns true se a combinação é permitida, false caso contrário
 */
export function validarCategoriaParaMenor(
    tipoCategoria: 'Receita' | 'Despesa' | 'Ambas',
    tipoTransacao: 'Receita' | 'Despesa',
    idade: number
): boolean {
    // Menores de idade não podem registrar receitas
    if (idade < 18 && tipoTransacao === 'Receita') {
        return false;
    }
    return true;
}
