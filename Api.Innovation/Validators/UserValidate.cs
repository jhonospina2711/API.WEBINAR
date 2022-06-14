using Entities;
using FluentValidation;
using System.Globalization;

namespace API.Covid19.Validations
{
    public class UserValidate : AbstractValidator<User>
    {
        public UserValidate()
        {
            //string NickName
            //RuleFor(x => x.NickName)
            //    .NotEmpty();
            //RuleFor(x => x.NickName)
            //    .MaximumLength(50);

            //string Name
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Name)
                .MaximumLength(50);

            //string LastName
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .MaximumLength(50);

            //string Email
            RuleFor(x => x.Email)
                .NotEmpty();
            RuleFor(x => x.Email)
                .MaximumLength(50);

            //string Password
            RuleFor(x => x.Password)
                .NotEmpty();
            RuleFor(x => x.Password)
                .MaximumLength(150);

            //Int InstitutionId

            //RuleFor(c => c.InstitutionId)
            //    .NotEmpty();

            ////Int SpecialtyId

            //RuleFor(c => c.SpecialtyId)
            //    .NotEmpty();

            //Int ProfileId

            //RuleFor(c => c.ProfileId)
            //    .NotEmpty();
        }
    }
}