using FluentValidation;
using SecureApiDemo.DTOs;

namespace SecureApiDemo.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
            .MinimumLength(3).WithMessage("En az 3 karakter olmalı.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gerekli.")
            .MinimumLength(4).WithMessage("En az 4 karakterli olmalı.");

        RuleFor(x => x.Role)
            .Must(r => r == "User" || r == "Admin")
            .WithMessage("Rol sadece 'User' ya da 'Admin' olabilir.");
    }
}
