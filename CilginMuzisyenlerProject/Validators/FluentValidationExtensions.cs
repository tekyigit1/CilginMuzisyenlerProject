using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CrazyMusicians.Api.Validators
{
    public static class FluentValidationExtensions
    {
        // Burada TİPİ TAM ADIYLA yazdık ki çakışma olmasın
        public static void AddToModelState(
            this FluentValidation.Results.ValidationResult result,
            ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
