using Expenses.Application.Interfaces;
using Expenses.Application.Services;
using Expenses.Domain.Entities;
using Moq;
using System;
using Xunit;

namespace Expenses.Application.Tests.Services
{
    // https://stackoverflow.com/questions/19578551/using-moq-to-test-a-repository
    // https://sodocumentation.net/moq/topic/6774/mocking-properties
    // https://softwareengineering.stackexchange.com/questions/304635/stubbing-properties-with-private-setters-for-tests
    public class EventServiceTests
    {
        [Fact]
        public void DeleteEvent_VeryfiyExpectedMethodsInvokedOnlyOnce()
        {
            // Arrange
            var @event = new Event(new User(Guid.NewGuid().ToString()), "title", "description", DateTime.Now, DateTime.Now.AddDays(10), "EUR");
            // There is no way to mock id without changing domain class. Using custom constructors, virtual properties, or an object builder. Is not an option as well because it provides code manipulation from outside.
            @event.GetType().GetProperty(nameof(Event.Id)).SetValue(@event, 10, null);

            var repo = new Mock<IEventRepository>();
            IEventService eventService = new EventService(repo.Object);

            // Act
            eventService.DeleteEvent(@event.Id);

            // Assert
            repo.Verify(x => x.Delete(@event.Id), Times.Once);
        }
    }
}
