using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Expenses.Domain.Tests.Entities
{
    public class EventTests
    {
        [Fact]
        public void CreateEvent()
        {
            // arrange
            UserId creatorId = new UserId(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(10);
            const string currency = "EUR";

            // act
            var @event = new Event(creatorId, title, description, startDate, endDate, currency);

            // assert
            Assert.Equal(creatorId.Id, @event.CreatorId.Id);
            Assert.Equal(title, @event.Title);
            Assert.Equal(description, @event.Description);
            Assert.Equal(startDate, @event.StartDate);
            Assert.Equal(endDate, @event.EndDate);
            Assert.Equal(currency, @event.Currency);

            Assert.True(@event.Participants.First().Id == creatorId.Id);
        }

        [Fact]
        public void CreateEvent_WithDefaultOrEmptyParameter()
        {
            // arrange
            UserId creatorId = new UserId(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(10);
            const string currency = "EUR";

            // act
            var ex_userId = Record.Exception(() => new Event(default, title, description, startDate, endDate, currency));
            var ex_title_default = Record.Exception(() => new Event(creatorId, default, description, startDate, endDate, currency));
            var ex_title_empty = Record.Exception(() => new Event(creatorId, string.Empty, description, startDate, endDate, currency));
            var ex_description_default = Record.Exception(() => new Event(creatorId, title, default, startDate, endDate, currency));
            var ex_description_empty = Record.Exception(() => new Event(creatorId, title, string.Empty, startDate, endDate, currency));
            var ex_startDate = Record.Exception(() => new Event(creatorId, title, description, default, endDate, currency));
            var ex_endDate = Record.Exception(() => new Event(creatorId, title, description, startDate, default, currency));
            var ex_currency_default = Record.Exception(() => new Event(creatorId, title, description, startDate, endDate, default));
            var ex_currency_empty = Record.Exception(() => new Event(creatorId, title, description, startDate, endDate, string.Empty));

            // assert
            Assert.IsType<Exception>(ex_userId);
            Assert.IsType<Exception>(ex_title_default);
            Assert.IsType<Exception>(ex_title_empty);
            Assert.IsType<Exception>(ex_description_default);
            Assert.IsType<Exception>(ex_description_empty);
            Assert.IsType<Exception>(ex_startDate);
            Assert.IsType<Exception>(ex_endDate);
            Assert.IsType<Exception>(ex_currency_default);
            Assert.IsType<Exception>(ex_currency_empty);
        }

        [Fact]
        public void CreateEvent_WithInvalidStartAndEndDate()
        {
            // arrange
            UserId creatorId = new UserId(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(-10);
            const string currency = "EUR";

            // act
            var ex = Record.Exception(() => new Event(default, title, description, startDate, endDate, currency));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void ChangeTitle()
        {
            // arrange
            var @event = GetValidEvent();
            var title = @event.Title + Guid.NewGuid().ToString("n");

            // act
            @event.Title = title;

            // assert
            Assert.Equal(title, @event.Title);
        }

        [Fact]
        public void ChangeDescription()
        {
            // arrange
            var @event = GetValidEvent();
            var description = @event.Description + Guid.NewGuid().ToString("n");

            // act
            @event.Description = description;

            // assert
            Assert.Equal(description, @event.Description);
        }

        [Fact]
        // https://stackoverflow.com/questions/46653557/whats-the-idiomatic-way-to-verify-collection-size-in-xunit
        public void AddParticipant_Single()
        {
            // arrange
            var @event = GetValidEvent();
            var participant = new UserId(Guid.NewGuid().ToString());

            // act
            @event.AddParticipant(participant);

            // assert
            Assert.Single(@event.Participants);
            Assert.Equal(participant.Id, @event.Participants.First().Id);
        }

        [Fact]
        public void AddParticipant_Multiple()
        {
            // arrange
            var @event = GetValidEvent();
            var participant1 = new UserId(Guid.NewGuid().ToString());
            var participant2 = new UserId(Guid.NewGuid().ToString());

            // act
            @event.AddParticipant(participant1);
            @event.AddParticipant(participant2);

            // assert
            Assert.Collection(@event.Participants,
                i => Assert.Equal(participant1.Id, i.Id),
                i => Assert.Equal(participant2.Id, i.Id));
        }

        [Fact]
        public void AddParticipant_WhoAlreadyExists()
        {
            // arrange
            var @event = GetValidEvent();
            var participantId = Guid.NewGuid().ToString();
            @event.AddParticipant(new UserId(participantId));

            // act
            var ex = Record.Exception(() => @event.AddParticipant(new UserId(participantId)));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void RemoveParticipant()
        {
            // arrange
            var @event = GetValidEvent();

            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var participantId3 = Guid.NewGuid().ToString();

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));
            @event.AddParticipant(new UserId(participantId3));

            // act
            @event.RemoveParticipant(new UserId(participantId3));

            // assert
            Assert.True(@event.Participants.Count() == 2);

            Assert.Collection(@event.Participants,
                i => Assert.Equal(participantId1, i.Id),
                i => Assert.Equal(participantId2, i.Id));
        }

        [Fact]
        public void RemoveParticipant_WhoParticipatesInEvent()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });
            @event.AddExpense(expense);

            // act
            var ex = Record.Exception(() => @event.RemoveParticipant(new UserId(participantId1)));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void RemoveParticipant_WhoNotExist()
        {
            // arrange
            var @event = GetValidEvent();

            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var participantId3 = Guid.NewGuid().ToString();

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            // act
            var ex = Record.Exception(() => @event.RemoveParticipant(new UserId(participantId3)));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void AddSingleExpense()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            // act
            @event.AddExpense(expense);

            // assert
            Assert.Single(@event.Expenses);

            Assert.Equal(expense.CreatorId.Id, @event.Expenses.First().CreatorId.Id);
        }

        [Fact]
        public void AddSingleExpense_WithNoSplit()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void AddExpense_WithUnknownCreator()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var unknownCreatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(unknownCreatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void AddExpense_WithUnknownParticipant()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var unknownParticipantId = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(unknownParticipantId), Amount = 5 }
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }


        [Fact]
        public void AddExpense_DateNotBetweenEventDate()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(100), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void AddExpense_CurrencyNotLikeEventCurrency()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "USD");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public void RemoveExpense()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            @event.AddExpense(expense);

            // act
            @event.RemoveExpense(expense);

            // assert
            Assert.True(@event.Expenses.Count() == 0);
        }

        [Fact]
        public void RemoveExpense_ThatNotExists()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new UserId(creatorId));

            @event.AddParticipant(new UserId(participantId1));
            @event.AddParticipant(new UserId(participantId2));

            var expense = new Expense(new UserId(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit { CreditorId = new UserId(creatorId), Amount = 10 },
                new List<Debit>
                {
                    new Debit { DebitorId = new UserId(creatorId), Amount = 5 },
                    new Debit { DebitorId = new UserId(participantId1), Amount = 5 }
                });

            // act
            var ex = Record.Exception(() => @event.RemoveExpense(expense));

            // assert
            Assert.IsType<Exception>(ex);
        }


        private Event GetValidEvent(
            string title = "title",
            string description = "description",
            string currency = "EUR")
        {
            var creatorId = new UserId(Guid.NewGuid().ToString());

            return GetValidEvent(creatorId, title, description, currency);
        }

        private Event GetValidEvent(
            UserId creatorId,
            string title = "title",
            string description = "description",
            string currency = "EUR")
        {
            return new Event(creatorId, title, description, DateTime.UtcNow, DateTime.UtcNow.AddDays(10), currency);
        }
    }
}
