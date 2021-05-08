using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<bool> { }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public LogoutCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LogoutAsync(_currentUserService.UserId);
        }
    }
}
