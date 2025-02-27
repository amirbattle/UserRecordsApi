using System.ComponentModel.DataAnnotations;

public class MinAgeAttribute(int Limit) : ValidationAttribute
    {
        private int _Limit = Limit;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
        {
                DateTime bday = DateTime.Parse(value.ToString());
                DateTime today = DateTime.Today;
                int age = today.Year - bday.Year;
                if (bday > today.AddYears(-age))
                {
                   age--; 
                }
                if (age < _Limit)
                {
                    var result = new ValidationResult($"You must be {_Limit} years or older");
                    return result; 
                } 
            
            return ValidationResult.Success;
        }
    }