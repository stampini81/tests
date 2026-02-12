import { test, expect } from '@playwright/test';

// Este teste cobre a regra de negócio: Menor de idade não pode cadastrar receita
test('Menor de idade não pode cadastrar receita', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa menor de idade
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Menor E2E');
  await page.fill('input[name="dataNascimento"]', '2016-01-01');
  await page.click('text=Salvar');
  // Aguarda confirmação de pessoa salva
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Navega para Transações
  await page.click('text=Transações');

  // Tenta cadastrar uma receita para o menor
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Teste');
  await page.fill('input[name="valor"]', '1000');
  await page.fill('input[name="data"]', '2026-02-10');
  // Seleciona pessoa menor
  await expect(page.locator('#pessoa-select')).toBeVisible();
  await expect(page.locator('#pessoa-select')).toBeEnabled();
  await page.fill('#pessoa-select', '');
  await page.fill('#pessoa-select', 'Menor E2E');
  await page.click('#pessoa-select');
  // Seleciona a pessoa criada
  let found = await page.locator('div[role="option"]:has-text("Menor E2E")').isVisible();
  while (!found) {
    await page.click('button[aria-label="Carregar mais"]');
    found = await page.locator('div[role="option"]:has-text("Menor E2E")').first().isVisible();
  }
  await page.locator('div[role="option"]:has-text("Menor E2E")').first().click({ force: true });
  await expect(page.locator('#categoria-select')).toBeVisible();
  await expect(page.locator('#categoria-select')).toBeEnabled();
  await page.click('#categoria-select');
  await page.click('text=Freelance'); // categoria do tipo Receita
  await page.click('text=Salvar');
  // Valida que o sistema bloqueia a ação e exibe mensagem de erro
  await expect(page.locator('text=Menores só podem registrar despesas')).toBeVisible();
});
