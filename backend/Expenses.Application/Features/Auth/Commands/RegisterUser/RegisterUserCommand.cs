using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result>
    {
        public string ConfirmationLink { get; set; }
        public RegisterUserRequestModel Model { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RegisterAsync(
                request.Model.FirstName, 
                request.Model.LastName, 
                request.Model.Username, 
                request.Model.Email, 
                request.Model.Password,
                request.ConfirmationLink);
        }
    }
}
