import { test, expect } from '@playwright/test';

test('Exclusão em cascata de transações ao excluir pessoa', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Pessoa Cascata E2E');
  await page.fill('input[name="dataNascimento"]', '1990-01-01');
  await page.click('text=Salvar');
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Vai para transações e cadastra uma transação
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Transação Cascata');
  await page.fill('input[name="valor"]', '200');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.selectOption('select#tipo', 'despesa');
  await page.fill('#pessoa-select', 'Pessoa Cascata E2E');
  await page.click('#pessoa-select');
  await page.locator('div[role="option"]:has-text("Pessoa Cascata E2E")').first().click({ force: true });
  await page.click('#categoria-select');
  await page.locator('div[role="option"]:has-text("Alimentação")').first().click({ force: true });
  await page.click('text=Salvar');
  await expect(page.locator('text=/sucesso/i')).toBeVisible({ timeout: 10000 });

  // Exclui a pessoa
  await page.click('text=Pessoas');
  // Navega até encontrar a pessoa
  let pessoaEncontrada = await page.locator('text=Pessoa Cascata E2E').isVisible();
  let tentativas = 0;
  while (!pessoaEncontrada && tentativas < 15) {
    await page.click('button:has-text("Próximo")');
    pessoaEncontrada = await page.locator('text=Pessoa Cascata E2E').isVisible();
    tentativas++;
  }
  // Clica no botão deletar da pessoa
  await page.locator('text=Pessoa Cascata E2E').first().click();
  await page.click('button.delete');
  await page.click('button:has-text("Confirmar")');
  await expect(page.locator('text=Pessoa excluída com sucesso!')).toBeVisible();

  // Vai para transações e verifica que a transação foi excluída
  await page.click('text=Transações');
  await expect(page.locator('text=Transação Cascata')).not.toBeVisible();
});
