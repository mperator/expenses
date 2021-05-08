using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Commands.RefreshCurrentToken
{
    public class RefreshTokenCommand : IRequest<(Result, TokenModel, RefreshToken)> 
    {
        public string Token { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (Result, TokenModel, RefreshToken)>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<(Result, TokenModel, RefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.HandleRefreshTokenAsync(request.Token);
        }
    }
}
