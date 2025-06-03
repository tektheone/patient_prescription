using System;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EndDateAfterPrescriptionDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prescription = (Prescription)validationContext.ObjectInstance;
            
            if (prescription.EndDate < prescription.PrescriptionDate)
            {
                return new ValidationResult(
                    "End date must be after or equal to the prescription date.",
                    new[] { nameof(Prescription.EndDate) });
            }
            
            return ValidationResult.Success;
        }
    }
}
