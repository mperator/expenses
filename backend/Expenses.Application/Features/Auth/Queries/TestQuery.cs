using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Queries
{
    public class TestQuery : IRequest<string> { }

    public class TestQueryHandler : IRequestHandler<TestQuery, string>
    {
        public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Secret {Guid.NewGuid()}");
        }
    }
}
