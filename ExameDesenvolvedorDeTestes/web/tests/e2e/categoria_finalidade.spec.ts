import { test, expect } from '@playwright/test';

test('Categoria só pode ser usada conforme sua finalidade', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria categoria de Receita
  await page.click('text=Categorias');
  await page.click('text=Adicionar Categoria');
  await page.fill('input[name="descricao"]', 'Categoria Receita E2E');
  await page.selectOption('select#finalidade', 'receita');
  await page.click('text=Salvar');
  await expect(page.locator('text=Categoria salva com sucesso!')).toBeVisible();

  // Cria pessoa maior
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Pessoa Receita E2E');
  await page.fill('input[name="dataNascimento"]', '1990-01-01');
  await page.click('text=Salvar');
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Vai para transações e cadastra receita com categoria correta
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Categoria');
  await page.fill('input[name="valor"]', '100');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.selectOption('select#tipo', 'receita');
  await page.fill('#pessoa-select', 'Pessoa Receita E2E');
  await page.click('#pessoa-select');
  await page.locator('div[role="option"]:has-text("Pessoa Receita E2E")').first().click({ force: true });
  await page.fill('#categoria-select', 'Categoria Receita E2E');
  await page.click('#categoria-select');
  await page.locator('div[role="option"]:has-text("Categoria Receita E2E")').first().click({ force: true });
  await page.click('text=Salvar');
  await expect(page.locator('text=/sucesso/i')).toBeVisible({ timeout: 10000 });

  // Tenta cadastrar despesa com categoria de receita (deve bloquear)
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Despesa Categoria');
  await page.fill('input[name="valor"]', '50');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.selectOption('select#tipo', 'despesa');
  await page.fill('#pessoa-select', 'Pessoa Receita E2E');
  await page.click('#pessoa-select');
  await page.locator('div[role="option"]:has-text("Pessoa Receita E2E")').first().click({ force: true });
  await page.fill('#categoria-select', 'Categoria Receita E2E');
  await page.click('#categoria-select');
  await page.locator('div[role="option"]:has-text("Categoria Receita E2E")').first().click({ force: true });
  await page.click('text=Salvar');
  // Captura erro da API
  const [response] = await Promise.all([
    page.waitForResponse(resp => resp.url().includes('/transacoes') && resp.status() >= 400),
    page.click('text=Salvar')
  ]);
  // Salva evidência do erro da API
  const errorBody = await response.text();
  await page.screenshot({ path: 'test-results/categoria_finalidade_api_error.png' });
  // Documenta o erro
  console.log('Erro da API ao tentar usar categoria de receita em despesa:', errorBody);
});
