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
        private List<UserId> _participants;
        private List<Expense> _expenses;

        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserId CreatorId { get; }
        public string Currency { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public IReadOnlyList<UserId> Participants => _participants.AsReadOnly();
        public IReadOnlyList<Expense> Expenses => _expenses.AsReadOnly();

        private Event() { } // EF

        public Event(UserId creatorId, string title, string description, DateTime startDate, DateTime endDate, string currency)
        {
            if (creatorId == null) throw new EventValidationException("CreatorIdInvalid", "Creator id is invalid.");
            if (string.IsNullOrWhiteSpace(title)) throw new EventValidationException("TitleInvalid", "No title set.");
            if (string.IsNullOrWhiteSpace(description)) throw new EventValidationException("DescriptionInvalid", "No description set.");
            if (startDate == default) throw new EventValidationException("StartDateInvalid", "Start date not set.");
            if (endDate == default) throw new EventValidationException("EndDateInvalid", "End Date not set.");
            
            // TODO: Validate in value object
            if (string.IsNullOrWhiteSpace(currency)) throw new EventValidationException("CurrencyInvalid", "Currency not set.");

            if (startDate > endDate) throw new EventValidationException("StartDateBehindEndDate", "Start date must be smaller or same like end date.");

            CreatorId = creatorId;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Currency = currency;

            // Add creator as first participants.
            _participants = new List<UserId> { creatorId };
            _expenses = new List<Expense>();
        }

        // TODO: Do we need an other exception type here.
        // TODO: Do we need an exception at all. Just ignore if an participant exists.
        public void AddParticipant(UserId participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p != null)
                throw new EventValidationException("AddParticipantAlreadyExists", "Participant already in event.");

            _participants.Add(participant);
        }

        // TODO: Do we need an other exception type here.
        // TODO: Does it matter if the user does not exists
        public void RemoveParticipant(UserId participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p == null)
                throw new EventValidationException("RemoveParticipantNotExists", "Participant was not found.");

            // Check if participant takes part in any expense.
            var _1 = _expenses.Select(e => e.CreatorId).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _2 = _expenses.Select(e => e.Credit.CreditorId).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _3 = _expenses.SelectMany(e => e.Debits.ToList())?.Select(a => a.DebitorId).FirstOrDefault(a => a.Id == participant.Id) != null;

            if (_1 || _2 || _3)
                throw new EventValidationException("RemoveParticipantHasExpenses", "Participant takes part in expense and cannot be deleted.");

            _participants.Remove(p);
        }

        // TODO: Do we need an other exception type here.
        public void AddExpense(Expense expense)
        {
            // Check if split is set. An expense can be created without split.
            if (expense.Credit == null)
                throw new EventValidationException("AddExpenseInvalidCredit", "Credit is not set.");
            if (expense.Debits == null || expense.Debits.Count == 0)
                throw new EventValidationException("AddExpenseInvalidDebits", "Debits is not set or does not contain elements.");

            // Check if creator, creditor and debitor take part in event.
            var users = new List<UserId> { expense.CreatorId, expense.Credit.CreditorId };
            users.AddRange(expense.Debits.Select(a => a.DebitorId));

            foreach (var user in users)
                if (_participants.Find(p => p.Id == user.Id) == null) throw new EventValidationException("AddExpenseUnknownParticipant", "User does not participate in event.");

            // Check if event date is between event date
            if (expense.Date.Date < StartDate.Date || expense.Date.Date > EndDate.Date)
                throw new EventValidationException("AddExpenseDateNotInRange", "Date not between event start and end date.");

            // Check if currency matches.
            if (expense.Currency != Currency)
                throw new EventValidationException("AddExpenseInvalidCurrency", "The currency does not match the currency of the event.");

            _expenses.Add(expense);
        }

        // TODO: Do we need an other exception type here.
        // TODO: Do we need to throw an excpetion if the event does not exists or can we simple ignore it.
        public void RemoveExpense(Expense expense)
        {
            // usually identified by id but if not saved yet how to identify
            if (!_expenses.Contains(expense)) throw new EventValidationException("Remove expense not found.", "Expense not found.");

            _expenses.Remove(expense);
        }
    }
}
