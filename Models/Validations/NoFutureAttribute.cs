using System;
using System.ComponentModel.DataAnnotations;
namespace SitioWebDePeliculas.Models.Validations
{
    public class NoFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "La fecha ingresada  no puede ser mayor a la fecha actual");
                }
            }
            return ValidationResult.Success;
        }
    }
}
