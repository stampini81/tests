import { test, expect } from '@playwright/test';

test('Maior de idade pode cadastrar receita com sucesso', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa maior
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Maior E2E');
  await page.fill('input[name="dataNascimento"]', '1990-01-01');
  await page.click('text=Salvar');
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Vai para transações e cadastra receita
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Maior');
  await page.fill('input[name="valor"]', '500');
  await page.fill('input[name="data"]', '2026-02-10');
  // Seleciona o tipo Receita
  await page.selectOption('select#tipo', 'receita');
  await expect(page.locator('#pessoa-select')).toBeVisible();
  await expect(page.locator('#pessoa-select')).toBeEnabled();
  await page.click('#pessoa-select');
  await page.click('text=Maior E2E');
  await expect(page.locator('#categoria-select')).toBeVisible();
  await expect(page.locator('#categoria-select')).toBeEnabled();
  await page.click('#categoria-select');
  await page.click('text=Freelance'); // categoria do tipo Receita
  await page.click('text=Salvar');
  // Aguarda mensagem de sucesso ou a transação aparecer na lista
  await expect(page.locator('text=/sucesso/i')).toBeVisible({ timeout: 10000 });
  // Alternativamente, aguarde a transação aparecer na tabela
  await expect(page.locator('text=Receita Maior')).toBeVisible({ timeout: 10000 });
});
