const { defineConfig } = require('cypress')

module.exports = defineConfig({
  e2e: {
    baseUrl: 'http://localhost:5173',
    supportFile: './support/e2e.js',
    specPattern: '**/*.cy.js',
  },
})
