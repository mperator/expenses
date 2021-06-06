using Expenses.Domain.Exceptions;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Entities
{
    // TODO: Do we need an interface for domain logic here?
    // TODO: Should we relay on System exceptions for basic validaition e.g. ArgumentNullException or InvalidArgumentException
    public class Event
    {
        private List<User> _participants;
        private List<Expense> _expenses;

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //public string CreatorId { get; }
        public User Creator { get; private set; }   // Hide?

        public string Currency { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public IReadOnlyList<User> Participants => _participants.AsReadOnly();
        public IReadOnlyList<Expense> Expenses => _expenses.AsReadOnly();

        private Event() { } // EF

        public Event(User creator, string title, string description, DateTime startDate, DateTime endDate, string currency)
        {
            if (creator == null) throw new EventValidationException(Localization.Language.EventNoCreator);
            if (string.IsNullOrWhiteSpace(title)) throw new EventValidationException(Localization.Language.EventNoTitle);
            if (string.IsNullOrWhiteSpace(description)) throw new EventValidationException(Localization.Language.EventNoDescription);
            if (startDate == default) throw new EventValidationException(Localization.Language.EventNoStartDate);
            if (endDate == default) throw new EventValidationException(Localization.Language.EventNoEndDate);
            
            // TODO: Validate in value object
            if (string.IsNullOrWhiteSpace(currency)) throw new EventValidationException(Localization.Language.EventNoCurrency);

            if (startDate > endDate) throw new EventValidationException(Localization.Language.EventInvalidDateRange);

            Creator = creator;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Currency = currency;

            // Add creator as first participants.
            _participants = new List<User> { creator };
            _expenses = new List<Expense>();
        }

        // TODO: Do we need an other exception type here.
        // TODO: Do we need an exception at all. Just ignore if an participant exists.
        public void AddParticipant(User participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p != null)
                throw new EventValidationException(Localization.Language.EventAddParticipantExists);

            _participants.Add(new User(participant.Id));
        }

        // TODO: Do we need an other exception type here.
        // TODO: Does it matter if the user does not exists
        public void RemoveParticipant(User participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p == null)
                throw new EventValidationException(Localization.Language.EventRemoveParticipantNotFound);

            // Check if participant takes part in any expense.
            var _1 = _expenses.Select(e => e.Creator).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _2 = _expenses.Select(e => e.Credit.Creditor).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _3 = _expenses.SelectMany(e => e.Debits.ToList())?.Select(a => a.Debitor).FirstOrDefault(a => a.Id == participant.Id) != null;

            if (_1 || _2 || _3)
                throw new EventValidationException(Localization.Language.EventRemoveParticipantHasExpenses);

            _participants.Remove(p);
        }

        // TODO: Do we need an other exception type here.
        public void AddExpense(Expense expense)
        {
            // Check if split is set. An expense can be created without split.
            if (expense.Credit == null)
                throw new EventValidationException(Localization.Language.EventAddExpenseNoCredit);
            if (expense.Debits == null || expense.Debits.Count == 0)
                throw new EventValidationException(Localization.Language.EventAddExpenseNoDebits);

            // Check if creator, creditor and debitor take part in event.
            var userIds = new List<string> { expense.Creator.Id, expense.Credit.Creditor.Id };
            userIds.AddRange(expense.Debits.Select(a => a.Debitor.Id));

            foreach (var id in userIds)
                if (_participants.Find(p => p.Id == id) == null) throw new EventValidationException(Localization.Language.EventAddExpenseUserNotInEvent);

            // Check if event date is between event date
            if (expense.Date.Date < StartDate.Date || expense.Date.Date > EndDate.Date)
                throw new EventValidationException(Localization.Language.EventAddExpenseDateNotInRange);

            // Check if currency matches.
            if (expense.Currency != Currency)
                throw new EventValidationException(Localization.Language.EventAddExpenseInvalidCurrency);

            _expenses.Add(expense);
        }

        // TODO: Do we need an other exception type here.
        // TODO: Do we need to throw an excpetion if the event does not exists or can we simple ignore it.
        public void RemoveExpense(Expense expense)
        {
            // usually identified by id but if not saved yet how to identify
            if (!_expenses.Contains(expense)) throw new EventValidationException(Localization.Language.EventRemoveExpenseNotFound);

            _expenses.Remove(expense);
        }
    }
}
