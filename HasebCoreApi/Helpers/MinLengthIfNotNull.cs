using HasebCoreApi.Localize;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi
{
    public class MinLengthIfNotNull : ValidationAttribute
    {
        public int Length { get; }
        public MinLengthIfNotNull(int value)
        {
            Length = value;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                var _value = ((string)value).Trim();
                if (_value.Length >= Length) return ValidationResult.Success;
            }

            var _localizationService = (IStringLocalizer<Resource>)validationContext.GetService(typeof(IStringLocalizer<Resource>));
            var localizedError = _localizationService[ErrorMessage];

            return new ValidationResult(localizedError);
        }
    }

    public class LengthIfNotNull : ValidationAttribute
    {
        public int Length { get; }
        public LengthIfNotNull(int value)
        {
            Length = value;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                var _value = ((string)value).Trim();
                if (_value.Length == Length) return ValidationResult.Success;
            }

            var _localizationService = (IStringLocalizer<Resource>)validationContext.GetService(typeof(IStringLocalizer<Resource>));
            var localizedError = _localizationService[ErrorMessage];

            return new ValidationResult(localizedError);
        }
    }
}
