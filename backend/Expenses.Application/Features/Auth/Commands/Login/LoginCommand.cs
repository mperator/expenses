using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<(Result, TokenModel, RefreshToken)>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, (Result, TokenModel, RefreshToken)>
    {   
        // https://www.codewithmukesh.com/blog/refresh-tokens-in-aspnet-core/
        private readonly IIdentityService _identityService;


        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<(Result, TokenModel, RefreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Username, null, request.Password);
        }
    }
}
