/// <reference types="cypress" />

context('Login page', () => {

    #TODO: check whether user exists or create always a user for the test
    it('Login as Testimann', () => {
    cy.visit('http://localhost:3000/login')
    cy.get('[data-test=username-field]').type('testimann')
    cy.get('[data-test=password-field]').type('TestPasswort1234&')
    cy.get('[data-test=signIn-button]').click()
    cy.url().should('include', '/dashboard')
    cy.get('[data-test=logout-button]').contains('Logout')
})
})
