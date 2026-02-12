const { defineConfig } = require('cypress')

module.exports = defineConfig({
  e2e: {
    baseUrl: 'http://localhost:5173',
    supportFile: 'tests_extras/cypress/support/e2e.js',
    specPattern: 'tests_extras/**/*.cy.js',
  },
})
