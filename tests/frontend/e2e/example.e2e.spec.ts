import { test, expect } from '@playwright/test';

test('Exemplo de teste E2E', async ({ page }) => {
  // Exemplo fictício: acessar uma página
  await page.goto('https://example.com');
  await expect(page).toHaveTitle(/Example/);
});
