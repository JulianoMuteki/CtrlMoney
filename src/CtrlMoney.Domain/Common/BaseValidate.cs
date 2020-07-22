using CtrlMoney.Domain.Interfaces.Base;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace CtrlMoney.Domain.Common
{
    public class BaseValidate : IComponentValidate
    {
        public bool IsValid { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            this.IsValid = ValidationResult.IsValid;
            return this.IsValid;
        }

        public List<KeyValuePair<string,string>> GetNotifications()
        {
            List<KeyValuePair<string, string>> listError = new List<KeyValuePair<string, string>>();
            foreach (var error in ValidationResult.Errors)
            {
                listError.Add(new KeyValuePair<string, string>(error.ErrorCode, error.ErrorMessage));
            }

            return listError;
        }



    }
}
