using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Expenses.Domain.Tests.Entities
{
    internal static class EventDataGenerator
    {
        public static IEnumerable<object[]> GenerateEvent_WithDefaultOrEmptyParameter()
        {
            var creatorId = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(10);
            const string currency = "EUR";

            yield return new object[] { default, title, description, startDate, endDate, currency };
            yield return new object[] { creatorId, default, description, startDate, endDate, currency };
            yield return new object[] { creatorId, string.Empty, description, startDate, endDate, currency };
            yield return new object[] { creatorId, title, default, startDate, endDate, currency };
            yield return new object[] { creatorId, title, string.Empty, startDate, endDate, currency };
            yield return new object[] { creatorId, title, description, default, endDate, currency };
            yield return new object[] { creatorId, title, description, startDate, default, currency };
            yield return new object[] { creatorId, title, description, startDate, endDate, default };
            yield return new object[] { creatorId, title, description, startDate, endDate, string.Empty };
        }
    }

    public class EventTests
    {
        public EventTests()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
        }


        [Fact]
        public void CreateEvent()
        {
            // arrange
            var creator = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(10);
            const string currency = "EUR";

            // act
            var @event = new Event(creator, title, description, startDate, endDate, currency);

            // assert
            Assert.Equal(creator.Id, @event.Creator.Id);
            Assert.Equal(title, @event.Title);
            Assert.Equal(description, @event.Description);
            Assert.Equal(startDate, @event.StartDate);
            Assert.Equal(endDate, @event.EndDate);
            Assert.Equal(currency, @event.Currency);

            Assert.True(@event.Participants.First().Id == creator.Id);
        }

        [Fact]
        public void CreateEvent_WithDefaultOrEmptyParameter()
        {
            // arrange
            var creator = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(10);
            const string currency = "EUR";

            // act
            var ex_userId = Record.Exception(() => new Event(default, title, description, startDate, endDate, currency));
            var ex_title_default = Record.Exception(() => new Event(creator, default, description, startDate, endDate, currency));
            var ex_title_empty = Record.Exception(() => new Event(creator, string.Empty, description, startDate, endDate, currency));
            var ex_description_default = Record.Exception(() => new Event(creator, title, default, startDate, endDate, currency));
            var ex_description_empty = Record.Exception(() => new Event(creator, title, string.Empty, startDate, endDate, currency));
            var ex_startDate = Record.Exception(() => new Event(creator, title, description, default, endDate, currency));
            var ex_endDate = Record.Exception(() => new Event(creator, title, description, startDate, default, currency));
            var ex_currency_default = Record.Exception(() => new Event(creator, title, description, startDate, endDate, default));
            var ex_currency_empty = Record.Exception(() => new Event(creator, title, description, startDate, endDate, string.Empty));

            // assert
            Assert.IsType<EventValidationException>(ex_userId);
            Assert.IsType<EventValidationException>(ex_title_default);
            Assert.IsType<EventValidationException>(ex_title_empty);
            Assert.IsType<EventValidationException>(ex_description_default);
            Assert.IsType<EventValidationException>(ex_description_empty);
            Assert.IsType<EventValidationException>(ex_startDate);
            Assert.IsType<EventValidationException>(ex_endDate);
            Assert.IsType<EventValidationException>(ex_currency_default);
            Assert.IsType<EventValidationException>(ex_currency_empty);
        }

        [Theory]
        [MemberData(nameof(EventDataGenerator.GenerateEvent_WithDefaultOrEmptyParameter), MemberType = typeof(EventDataGenerator))]
        public void CreateEvent_WithDefaultOrEmptyParameter1(User creator, string title, string description, DateTime startDate, DateTime endDate, string currency)
        {
            // act and assert
            var exception = Assert.Throws<EventValidationException>(() => new Event(creator, title, description, startDate, endDate, currency));
        }

        [Fact]
        public void CreateEvent_WithInvalidStartAndEndDate()
        {
            // arrange
            var creator = new User(Guid.NewGuid().ToString());
            const string title = "title";
            const string description = "description";
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(-10);
            const string currency = "EUR";

            // act
            var ex = Record.Exception(() => new Event(default, title, description, startDate, endDate, currency));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void ChangeTitle()
        {
            // arrange
            var @event = GetValidEventWithRandomCreator();
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
            var @event = GetValidEventWithRandomCreator();
            var description = @event.Description + Guid.NewGuid().ToString("n");

            // act
            @event.Description = description;

            // assert
            Assert.Equal(description, @event.Description);
        }

        // https://stackoverflow.com/questions/46653557/whats-the-idiomatic-way-to-verify-collection-size-in-xunit
        [Fact]
        public void AddParticipant_Multiple()
        {
            // arrange
            var @event = GetValidEventWithRandomCreator();
            var participant1 = new User(Guid.NewGuid().ToString());
            var participant2 = new User(Guid.NewGuid().ToString());

            // act
            @event.AddParticipant(participant1);
            @event.AddParticipant(participant2);

            // assert
            Assert.Collection(@event.Participants,
                i => Assert.Equal(@event.Creator.Id, i.Id),
                i => Assert.Equal(participant1.Id, i.Id),
                i => Assert.Equal(participant2.Id, i.Id));
        }

        [Fact]
        public void AddParticipant_WhoAlreadyExists()
        {
            // arrange
            var @event = GetValidEventWithRandomCreator();
            var participantId = Guid.NewGuid().ToString();
            @event.AddParticipant(new User(participantId));

            // act
            var ex = Record.Exception(() => @event.AddParticipant(new User(participantId)));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void RemoveParticipant()
        {
            // arrange
            var @event = GetValidEventWithRandomCreator();

            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var participantId3 = Guid.NewGuid().ToString();

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));
            @event.AddParticipant(new User(participantId3));

            // act
            @event.RemoveParticipant(new User(participantId3));

            // assert
            Assert.True(@event.Participants.Count() == 3);

            Assert.Collection(@event.Participants,
                i => Assert.Equal(@event.Creator.Id, i.Id),
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

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });
            @event.AddExpense(expense);

            // act
            var ex = Record.Exception(() => @event.RemoveParticipant(new User(participantId1)));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void RemoveParticipant_WhoNotExist()
        {
            // arrange
            var @event = GetValidEventWithRandomCreator();

            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var participantId3 = Guid.NewGuid().ToString();

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            // act
            var ex = Record.Exception(() => @event.RemoveParticipant(new User(participantId3)));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void AddSingleExpense()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });

            // act
            @event.AddExpense(expense);

            // assert
            Assert.Single(@event.Expenses);

            Assert.Equal(expense.Creator.Id, @event.Expenses.First().Creator.Id);
        }

        [Fact]
        public void AddSingleExpense_WithNoSplit()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void AddExpense_WithUnknownCreator()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var unknownCreatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(unknownCreatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void AddExpense_WithUnknownParticipant()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();
            var unknownParticipantId = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(unknownParticipantId), 5)
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }


        [Fact]
        public void AddExpense_DateNotBetweenEventDate()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(100), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void AddExpense_CurrencyNotLikeEventCurrency()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "USD");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });

            // act
            var ex = Record.Exception(() => @event.AddExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }

        [Fact]
        public void RemoveExpense()
        {
            // arrange
            var creatorId = Guid.NewGuid().ToString();
            var participantId1 = Guid.NewGuid().ToString();
            var participantId2 = Guid.NewGuid().ToString();

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
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

            var @event = GetValidEvent(new User(creatorId));

            @event.AddParticipant(new User(participantId1));
            @event.AddParticipant(new User(participantId2));

            var expense = new Expense(new User(creatorId), "title", "description", DateTime.Now.AddDays(1), "EUR");
            expense.Split(
                new Credit(new User(creatorId), 10),
                new List<Debit>
                {
                    new Debit(new User(creatorId), 5),
                    new Debit(new User(participantId1), 5)
                });

            // act
            var ex = Record.Exception(() => @event.RemoveExpense(expense));

            // assert
            Assert.IsType<EventValidationException>(ex);
        }


        private Event GetValidEventWithRandomCreator(
            string title = "title",
            string description = "description",
            string currency = "EUR")
        {
            var creator = new User(Guid.NewGuid().ToString());

            return GetValidEvent(creator, title, description, currency);
        }

        private Event GetValidEvent(
            User creator,
            string title = "title",
            string description = "description",
            string currency = "EUR")
        {
            return new Event(creator, title, description, DateTime.UtcNow, DateTime.UtcNow.AddDays(10), currency);
        }
    }
}
