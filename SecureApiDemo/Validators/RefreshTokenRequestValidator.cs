using FluentValidation;
using SecureApiDemo.DTOs;

namespace SecureApiDemo.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token boş olamaz.")
                .MinimumLength(20).WithMessage("Refresh token çok kısa.");
        }
    }
}
