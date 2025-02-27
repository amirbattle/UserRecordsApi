using System.ComponentModel.DataAnnotations;

    // This custom Attribute is unused but is useful and can possibly be used in the future.
    // So keeping it here for now.
    public class EmailUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var _context = (UserDbContext)validationContext.GetService(typeof(UserDbContext));
            var entity = _context.Users.SingleOrDefault(e => e.Email == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use.";
        }
}