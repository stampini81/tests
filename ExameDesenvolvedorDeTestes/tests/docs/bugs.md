# Bugs e Evidências

## Cadastro de Pessoas, Categorias e Transações com Mesma Descrição

### Descrição do Problema
Atualmente, o sistema permite o cadastro de múltiplas pessoas, categorias e transações com a mesma descrição/nome. Isso dificulta a diferenciação dos registros em relatórios e consultas, pois não há informações adicionais para distinguir cada item.

#### Exemplos de Impacto
- **Pessoas**: Não é possível diferenciar pessoas com o mesmo nome, pois faltam campos como sexo, CPF, RG, etc.
- **Categorias**: Categorias com a mesma descrição dificultam a análise financeira.
- **Transações**: Transações com a mesma descrição, valor e data não possuem um identificador único visível.

### Evidências Visuais
As imagens a seguir demonstram o problema:

- ![Pessoas Iguais](../../evidencias/pessoas_iguais.png)
- ![Categorias Iguais](../../evidencias/categorias_iguais.png)
- ![Transações Iguais](../../evidencias/transacao_iguais.png)

### Sugestão de Melhoria
Adicionar campos obrigatórios e/ou únicos para:
- Pessoas: sexo, CPF, RG, data de nascimento, etc.
- Categorias: descrição única ou código identificador.
- Transações: algum identificador único ou detalhamento adicional.

Assim, será possível distinguir registros de forma confiável em relatórios e consultas.
