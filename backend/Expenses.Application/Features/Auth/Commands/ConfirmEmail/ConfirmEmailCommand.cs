using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
