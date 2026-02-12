import { test, expect } from '@playwright/test';

test('Consulta de totais por pessoa após cadastro de transações', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Pessoa Totais E2E');
  await page.fill('input[name="dataNascimento"]', '1990-01-01');
  await page.click('text=Salvar');
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Vai para transações e cadastra duas transações
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Totais');
  await page.fill('input[name="valor"]', '100');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.selectOption('select#tipo', 'receita');
  await page.fill('#pessoa-select', 'Pessoa Totais E2E');
  await page.click('#pessoa-select');
  await page.locator('div[role="option"]:has-text("Pessoa Totais E2E")').first().click({ force: true });
  await page.click('#categoria-select');
  await page.locator('div[role="option"]:has-text("Freelance")').first().click({ force: true });
  await page.click('text=Salvar');
  await expect(page.locator('text=/sucesso/i')).toBeVisible({ timeout: 10000 });

  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Despesa Totais');
  await page.fill('input[name="valor"]', '50');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.selectOption('select#tipo', 'despesa');
  await page.fill('#pessoa-select', 'Pessoa Totais E2E');
  await page.click('#pessoa-select');
  await page.locator('div[role="option"]:has-text("Pessoa Totais E2E")').first().click({ force: true });
  await page.click('#categoria-select');
  await page.locator('div[role="option"]:has-text("Alimentação")').first().click({ force: true });
  await page.click('text=Salvar');
  await expect(page.locator('text=/sucesso/i')).toBeVisible({ timeout: 10000 });

  // Consulta totais por pessoa
  await page.click('text=Totais');
  await page.click('text=Pessoa Totais E2E');
  // Valida que os totais estão corretos
  await expect(page.locator('text=Receita Totais')).toBeVisible();
  await expect(page.locator('text=Despesa Totais')).toBeVisible();
  await expect(page.locator('text=100')).toBeVisible(); // Receita
  await expect(page.locator('text=50')).toBeVisible(); // Despesa
});
