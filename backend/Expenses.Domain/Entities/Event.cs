using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            _participants = new List<UserId>();
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

            _participants.Remove(p);
        }

        public void AddExpense(Expense expense)
        {

        }

        public void RemoveExpense(Expense expense)
        {

        }

        // add expense
        // remove expense
    }
}
