using Expenses.Domain.Exceptions;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Entities
{
    // TODO: Do we need an interface for domain logic here?
    // TODO: Should we relay on System exceptions for basic validaition e.g. ArgumentNullException or InvalidArgumentException
    public class Expense
    {
        private Credit _credit;
        private IReadOnlyList<Debit> _debits;

        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; }
        public DateTime Date { get; set; }
        public User CreatorId { get; }

        public Credit Credit => _credit;
        public IReadOnlyList<Debit> Debits => _debits;

        private Expense() { } // EF

        public Expense(User creatorId, string title, string description, DateTime date, string currency)
        {
            // domain validation
            if (creatorId == null) throw new ExpenseValidationException("CreatorInvalid", "Invalid creator.");
            if (string.IsNullOrWhiteSpace(title)) throw new ExpenseValidationException("TitleInvalid", "Invalid title.");
            if (string.IsNullOrWhiteSpace(description)) throw new ExpenseValidationException("DescriptionInvalid", "description.");  // TODO: allow null?
            if (date == default) throw new ExpenseValidationException("DateInvalid", "Invalid date.");
            
            // TODO: Use value object for currency
            if (string.IsNullOrWhiteSpace(currency)) throw new ExpenseValidationException("CurrencyInvalid", "Invalid currency set.");
            if (currency.Length != 3) throw new ExpenseValidationException("CurrencyInvalid", "No valid currency string.");

            CreatorId = creatorId;
            Title = title;
            Description = description;
            Date = date;
            Currency = currency;
        }

        public void Split(Credit credit, List<Debit> debits)
        {
            if (credit == null) throw new ExpenseValidationException("CreditInvalid", "Credit must not be null.");
            if (debits == null || debits.Count == 0) throw new ExpenseValidationException("DebitsInvalid", "Debits must not be null or empty.");

            if (credit.Amount != debits.Sum(d => d.Amount)) throw new ExpenseValidationException("CreditNotDebitsAmount", "Invalid amount betwenn credit and debits.");
            
            // TODO: does this matter?
            if (debits.Count() != debits.Select(d => d.DebitorId).Distinct().Count()) throw new ExpenseValidationException("DebitorSame", "Same debitor is not allowed");

            _credit = credit;
            _debits = debits.AsReadOnly();
        }
    }
}
