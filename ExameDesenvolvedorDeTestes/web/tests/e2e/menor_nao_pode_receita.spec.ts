import { test, expect } from '@playwright/test';

test('Menor de idade não pode cadastrar receita', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa menor
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Menor E2E');
  await page.fill('input[name="dataNascimento"]', '2016-01-01');
  await page.click('text=Salvar');
  // Aguarda confirmação de pessoa salva
  await expect(page.locator('text=Pessoa salva com sucesso!')).toBeVisible();

  // Volta para dashboard ou navega para Transações
  await page.click('text=Transações');

  // Agora cadastra a transação
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Teste');
  await page.fill('input[name="valor"]', '1000');
  await page.fill('input[name="data"]', '2026-02-10');
  // Se houver campo de tipo, selecione "Receita" (ajuste se necessário)
  // await page.click('input[name="tipo"]');
  // await page.click('text=Receita');
  // Agora seleciona a pessoa
  await expect(page.locator('#pessoa-select')).toBeVisible();
  await expect(page.locator('#pessoa-select')).toBeEnabled();
  await page.fill('#pessoa-select', ''); // Limpa
  await page.fill('#pessoa-select', 'Menor E2E');
  await page.click('#pessoa-select');
  // Tenta encontrar o item, se não aparecer, clica em carregar mais
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
  // Deve exibir mensagem de erro ou bloquear ação
  await expect(page.locator('text=Menores só podem registrar despesas')).toBeVisible();
});
