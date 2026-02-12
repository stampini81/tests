import { test, expect } from '@playwright/test';

test('Menor de idade pode cadastrar despesa com sucesso', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa menor
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Menor Despesa E2E');
  await page.fill('input[name="dataNascimento"]', '2010-01-01');
  await page.click('text=Salvar');
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Vai para transações e cadastra despesa
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Despesa Menor');
  await page.fill('input[name="valor"]', '50');
  await page.fill('input[name="data"]', '2026-02-10');
  await expect(page.locator('#pessoa-select')).toBeVisible();
  await expect(page.locator('#pessoa-select')).toBeEnabled();
  await page.fill('#pessoa-select', ''); // Limpa
  await page.fill('#pessoa-select', 'Menor Despesa E2E');
  await page.click('#pessoa-select');
  // Tenta encontrar o item, se não aparecer, clica em carregar mais
  let found = await page.locator('div[role="option"]:has-text("Menor Despesa E2E")').first().isVisible();
  let carregarMaisVisivel = await page.locator('button[aria-label="Carregar mais"]').isVisible();
  while (!found && carregarMaisVisivel) {
    await page.click('button[aria-label="Carregar mais"]');
    found = await page.locator('div[role="option"]:has-text("Menor Despesa E2E")').first().isVisible();
    carregarMaisVisivel = await page.locator('button[aria-label="Carregar mais"]').isVisible();
  }
  await page.locator('div[role="option"]:has-text("Menor Despesa E2E")').first().click({ force: true });
  await expect(page.locator('#categoria-select')).toBeVisible();
  await expect(page.locator('#categoria-select')).toBeEnabled();
  await page.click('#categoria-select');
  // Usa o papel ARIA e força o clique para evitar overlay
  await page.click('div[role="option"]:has-text("Alimentação")', { force: true });
  await page.click('text=Salvar');
  // Deve exibir mensagem de sucesso
  await expect(page.locator('text=Transação salva com sucesso!')).toBeVisible();
});
