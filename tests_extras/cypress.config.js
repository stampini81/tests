module.exports = {
  allowCypressEnv: false,

  e2e: {
    specPattern: 'e2e/**/*.cy.js',
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
};
