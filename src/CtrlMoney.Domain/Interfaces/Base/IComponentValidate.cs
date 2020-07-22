using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Interfaces.Base
{
    public interface IComponentValidate
    {
        bool IsValid { get; set; }
        ValidationResult ValidationResult { get; set; }

        bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator);
        List<KeyValuePair<string, string>> GetNotifications();
    }
}
