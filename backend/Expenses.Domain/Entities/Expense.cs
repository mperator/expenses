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
        private List<Debit> _debits;

        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; }
        public DateTime Date { get; }
        public User Creator { get; }

        public Credit Credit => _credit;
        public IReadOnlyList<Debit> Debits => _debits;

        private Expense() { } // EF

        public Expense(User creator, string title, string description, DateTime date, string currency)
        {
            // domain validation
            if (creator == null) throw new ExpenseValidationException(Localization.Language.ExpenseNoCreator);
            if (string.IsNullOrWhiteSpace(title)) throw new ExpenseValidationException(Localization.Language.ExpenseNoTitle);
            if (string.IsNullOrWhiteSpace(description)) throw new ExpenseValidationException(Localization.Language.ExpenseNoDescription);  // TODO: allow null?
            if (date == default) throw new ExpenseValidationException(Localization.Language.ExpenseNoDate);
            
            // TODO: Use value object for currency
            if (string.IsNullOrWhiteSpace(currency)) throw new ExpenseValidationException(Localization.Language.ExpenseNoCurrency);
            if (currency.Length != 3) throw new ExpenseValidationException(Localization.Language.ExpenseInvalidCurrencyLength);

            Creator = creator;
            Title = title;
            Description = description;
            Date = date;
            Currency = currency;
        }

        public void Split(Credit credit, List<Debit> debits)
        {
            if (credit == null) throw new ExpenseValidationException(Localization.Language.ExpenseSplitNoCredit);
            if (debits == null || debits.Count == 0) throw new ExpenseValidationException(Localization.Language.ExpenseSplitNoDebits);

            if (credit.Amount != debits.Sum(d => d.Amount)) throw new ExpenseValidationException(Localization.Language.ExpenseSplitInvalidBalance);
            
            // TODO: does this matter?
            if (debits.Count() != debits.Select(d => d.Debitor).Distinct().Count()) throw new ExpenseValidationException(Localization.Language.ExpenseSplitDebitorDuplication);

            _credit = credit;
            _debits = debits;
        }
    }
}
