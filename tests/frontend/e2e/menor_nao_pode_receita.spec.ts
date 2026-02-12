import { test, expect } from '@playwright/test';

test('Menor de idade não pode cadastrar receita', async ({ page }) => {
  await page.goto('http://localhost:5173');
  // Cria pessoa menor
  await page.click('text=Pessoas');
  await page.click('text=Adicionar Pessoa');
  await page.fill('input[name="nome"]', 'Menor E2E');
  await page.fill('input[name="dataNascimento"]', '2016-01-01');
  await page.click('text=Salvar');
  // Tenta cadastrar receita para menor
  await page.click('text=Transações');
  await page.click('text=Adicionar Transação');
  await page.fill('input[name="descricao"]', 'Receita Teste');
  await page.fill('input[name="valor"]', '1000');
  await page.fill('input[name="data"]', '2026-02-10');
  await page.click('input[name="pessoa"]');
  await page.click('text=Menor E2E');
  await page.click('input[name="categoria"]');
  await page.click('text=Freelance'); // categoria do tipo Receita
  await page.click('text=Salvar');
  // Deve exibir mensagem de erro ou bloquear ação
  await expect(page.locator('text=Menores só podem registrar despesas')).toBeVisible();
});
