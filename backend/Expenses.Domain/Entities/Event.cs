using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Entities
{
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
            if (creatorId == null) throw new Exception("No creator id set.");
            if (string.IsNullOrWhiteSpace(title)) throw new Exception("Invalid title.");
            if (string.IsNullOrWhiteSpace(description)) throw new Exception("Invalid description.");
            if (startDate == default) throw new Exception("Invalid start date.");
            if (endDate == default) throw new Exception("Invalid end date.");
            if (string.IsNullOrWhiteSpace(currency)) throw new Exception("Invalid currency");

            if (startDate > endDate) throw new Exception("Start date must be smaller or same like end date.");

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

        public void AddParticipant(UserId participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p != null)
                throw new Exception("Participant already in exception.");

            _participants.Add(participant);
        }

        public void RemoveParticipant(UserId participant)
        {
            var p = _participants.Find(p => p.Id == participant.Id);
            if (p == null)
                throw new Exception("Participant not found.");

            // Check if participant takes part in any expense.

            var _1 = _expenses.Select(e => e.CreatorId).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _2 = _expenses.Select(e => e.Credit.CreditorId).FirstOrDefault(a => a.Id == participant.Id) != null;
            var _3 = _expenses.SelectMany(e => e.Debits.ToList())?.Select(a => a.DebitorId).FirstOrDefault(a => a.Id == participant.Id) != null;

            if (_1 || _2 || _3)
                throw new Exception("Participant takes part in expense and cannot be deleted.");

            _participants.Remove(p);
        }

        public void AddExpense(Expense expense)
        {
            // Check if split is set.
            if (expense.Credit == null || expense.Debits == null)
                throw new Exception("Invalid expense no split set.");

            // Check if creator, creditor and debitor take part in event.
            var users = new List<UserId> { expense.CreatorId, expense.Credit.CreditorId };
            users.AddRange(expense.Debits.Select(a => a.DebitorId));

            foreach (var user in users)
                if (_participants.Find(p => p.Id == user.Id) == null) throw new Exception("User unknown for event.");

            // Check if event date is between event date
            if (expense.Date.Date < StartDate.Date || expense.Date.Date > EndDate.Date)
                throw new Exception("Date not between event date.");

            // Check if currency matches.
            if (expense.Currency != Currency)
                throw new Exception("Invalid Currency");

            _expenses.Add(expense);
        }

        public void RemoveExpense(Expense expense)
        {
            // usually identified by id but if not saved yet how to identify
            if (!_expenses.Contains(expense)) throw new Exception("Expnese does not exists");

            _expenses.Remove(expense);
        }

        // add expense
        // remove expense
    }
}
