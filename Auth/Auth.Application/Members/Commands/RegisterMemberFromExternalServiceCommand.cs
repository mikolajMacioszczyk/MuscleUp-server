using Auth.Application.Common.Interfaces;
using Auth.Application.Dtos;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using Common.Consts;
using Common.Exceptions;
using MediatR;

namespace Auth.Application.Members.Commands
{
    public record RegisterMemberFromExternalServiceCommand(FacebookLoginViewModel Model) : IRequest<Member>
    {}

    public class RegisterMemberFromExternalServiceCommandHandler : IRequestHandler<RegisterMemberFromExternalServiceCommand, Member>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;

        public RegisterMemberFromExternalServiceCommandHandler(ISpecificUserRepository<Member, RegisterMemberDto> repository)
        {
            _repository = repository;
        }

        public async Task<Member> Handle(RegisterMemberFromExternalServiceCommand request, CancellationToken cancellationToken)
        {
            var avatarUrl = string.IsNullOrEmpty(request.Model.AvatarUrl) ? 
                SeedConsts.DefaultUserAvatarUrl : request.Model.AvatarUrl;

            var names = request.Model.Name?.Split(" ") ?? Array.Empty<string>();
            names = names.Where(n => !string.IsNullOrEmpty(n)).ToArray();

            var firstName = names.Any() ? new string(names[0].Take(30).ToArray()) : "First Name";
            var lastName = names.Skip(1).Any() ? new string(names[1].Take(30).ToArray()) : "Last Name";

            var registerForm = new RegisterMemberDto()
            {
                Email = request.Model.Email,
                Password = SeedConsts.DefaultPassword,
                ConfirmPassword = SeedConsts.DefaultPassword,
                AvatarUrl = avatarUrl,
                BirthDate = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Gender = Domain.Enums.GenderType.Male,
                HeightInCm = 180,
                WeightInKg = 70
            };

            var memberResult = await _repository.Register(registerForm, userId: request.Model.UserId, preventPasswordLogin: true);

            if (memberResult.IsSuccess)
            {
                return memberResult.Value;
            }

            throw new BadRequestException(memberResult.ErrorCombined);
        }
    }
}
