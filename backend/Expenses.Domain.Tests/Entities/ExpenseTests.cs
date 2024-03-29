﻿using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace Expenses.Domain.Tests.Entities
{
    public class ExpenseTests
    {

        // https://medium.com/swlh/introduction-to-test-driven-development-tdd-and-aaa-testing-using-xunit-4164683a8275
        // arrange act assert
        [Fact]
        public void CreateExpense()
        {
            var user = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime date = DateTime.UtcNow;
            const string currency = "EUR";

            var expense = new Expense(user, title, description, date, currency);

            Assert.Equal(user.Id, expense.Creator.Id);
            Assert.Equal(title, expense.Title);
            Assert.Equal(description, expense.Description);
            Assert.Equal(date, expense.Date);
            Assert.Equal(currency, expense.Currency);
        }

        [Fact]
        public void CreateExpense_WithDefaultOrEmptyParameter()
        {
            // arrange
            var user = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime date = DateTime.UtcNow;
            const string currency = "EUR";

            // act
            var ex_userId = Record.Exception(() => new Expense(default, title, description, date, currency));
            var ex_title_default = Record.Exception(() => new Expense(user, default, description, date, currency));
            var ex_title_empty = Record.Exception(() => new Expense(user, string.Empty, description, date, currency));
            var ex_description_default = Record.Exception(() => new Expense(user, title, default, date, currency));
            var ex_description_empty = Record.Exception(() => new Expense(user, title, string.Empty, date, currency));
            var ex_date = Record.Exception(() => new Expense(user, title, description, default, currency));
            var ex_currency_default = Record.Exception(() => new Expense(user, title, description, date, default));
            var ex_currency_empty = Record.Exception(() => new Expense(user, title, description, date, string.Empty));

            // assert
            Assert.IsType<ExpenseValidationException>(ex_userId);
            Assert.IsType<ExpenseValidationException>(ex_title_default);
            Assert.IsType<ExpenseValidationException>(ex_title_empty);
            Assert.IsType<ExpenseValidationException>(ex_description_default);
            Assert.IsType<ExpenseValidationException>(ex_description_empty);
            Assert.IsType<ExpenseValidationException>(ex_date);
            Assert.IsType<ExpenseValidationException>(ex_currency_default);
            Assert.IsType<ExpenseValidationException>(ex_currency_empty);
        }

        [Fact]
        public void CreateExpense_ChangeTitle()
        {
            // arrange
            var expense = GetExpenseWithDefaultValues();
            var title = expense.Title + Guid.NewGuid().ToString("n");

            // act
            expense.Title = title;

            // assert
            Assert.Equal(title, expense.Title);
        }

        [Fact]
        public void CreateExpense_ChangeDescription()
        {
            // arrange
            var expense = GetExpenseWithDefaultValues();
            var description = expense.Description + Guid.NewGuid().ToString("n");

            // act
            expense.Description = description;

            // assert
            Assert.Equal(description, expense.Description);
        }

        [Fact]
        public void CreateExpense_WithInvalidCurrency()
        {
            // act and arrange
            var ex = Record.Exception(() => GetExpenseWithDefaultValues(currency: "ABCDE"));

            // assert
            Assert.IsType<ExpenseValidationException>(ex);
        }

        [Fact]
        public void Split()
        {
            // arrange
            var expense = GetExpenseWithDefaultValues();
            var creditor = expense.Creator;
            var debitor1 = new User(Guid.NewGuid().ToString());
            var debitor2 = new User(Guid.NewGuid().ToString());

            var credit = new Credit(creditor, 10);
            var debits = new List<Debit>
            {
                new Debit(creditor, 2),
                new Debit(debitor1, 2),
                new Debit(debitor2, 6)
            };

            // act
            expense.Split(credit, debits);

            // assert
            Assert.Equal(credit.Creditor.Id, expense.Credit.Creditor.Id);
            Assert.Equal(credit.Amount, expense.Credit.Amount);

            Assert.Equal(3, expense.Debits.Count);
            Assert.Equal(debits[1].Debitor.Id, expense.Debits[1].Debitor.Id);
            Assert.Equal(debits[1].Amount, expense.Debits[1].Amount);
        }

        [Fact]
        public void Split_WithRest()
        {
            // arrange
            var expense = GetExpenseWithDefaultValues();
            var creditor = expense.Creator;
            var debitor1 = new User(Guid.NewGuid().ToString());
            var debitor2 = new User(Guid.NewGuid().ToString());

            var credit = new Credit(creditor, 10);
            var debits = new List<Debit>
            {
                new Debit(creditor, 2),
                new Debit(debitor1, 10),
                new Debit(debitor2, 6)
            };

            // act
            var ex = Record.Exception(() => expense.Split(credit, debits));

            // assert
            Assert.IsType<ExpenseValidationException>(ex);
        }

        [Fact]
        public void Split_WithSameDebitors()
        {
            // arrange
            var expense = GetExpenseWithDefaultValues();
            var creditor = expense.Creator;
            var debitor1 = new User(Guid.NewGuid().ToString());

            var credit = new Credit(creditor, 10);
            var debits = new List<Debit>
            {
                new Debit(creditor, 2),
                new Debit(debitor1, 2),
                new Debit(debitor1, 6)
            };

            // act
            var ex = Record.Exception(() => expense.Split(credit, debits));

            // assert
            Assert.IsType<ExpenseValidationException>(ex);
        }

        private static Expense GetExpenseWithDefaultValues(
            string title = "title",
            string description = "description",
            string currency = "EUR"
            )
        {
            var user = new User(Guid.NewGuid().ToString());

            return new Expense(user, title, description, DateTime.UtcNow, currency);
        }
    }
}
