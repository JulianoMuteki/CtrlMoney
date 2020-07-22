using CtrlMoney.Domain.Entities;
using FluentValidation;

namespace CtrlMoney.Domain.Validations
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.CEP).NotNull().Length(0, 15);
            RuleFor(x => x.Street).NotNull().Length(0, 250);
            RuleFor(x => x.Number).NotNull().Length(0, 15);
            RuleFor(x => x.City).NotNull().Length(0, 50);
            RuleFor(x => x.District).NotNull().Length(0, 250);
            RuleFor(x => x.Estate).NotNull().Length(0, 50);
            RuleFor(x => x.Reference).NotNull().Length(0, 50);
        }
    }
}
