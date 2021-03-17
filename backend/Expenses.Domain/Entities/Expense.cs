using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Entities
{
    public class Expense
    {
        private Credit _credit;
        private IReadOnlyList<Debit> _debits;

        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; }
        public DateTime Date { get; set; }
        public UserId CreatorId { get; }

        public Credit Credit => _credit;
        public IReadOnlyList<Debit> Debits => _debits;

        private Expense() { } // EF

        public Expense(UserId creatorId, string title, string description, DateTime date, string currency)
        {
            // domain validation
            if (creatorId == null) throw new Exception("Invalid creator.");
            if (string.IsNullOrWhiteSpace(title)) throw new Exception("Invalid title.");
            if (string.IsNullOrWhiteSpace(description)) throw new Exception("Invalid description.");  // TODO: allow null?
            if (date == default) throw new Exception("Invalid date.");
            if (string.IsNullOrWhiteSpace(currency)) throw new Exception("Invalid currency set.");
            if(currency.Length != 3) throw new Exception("No valid currency string.");

            CreatorId = creatorId;
            Title = title;
            Description = description;
            Date = date;
            Currency = currency;
        }

        public void Split(Credit credit, List<Debit> debits)
        {
            // != null

            // TODO validate credit - debits = 0;

            _credit = credit;
            _debits = debits.AsReadOnly();
        }
    }
}
