using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Roominator {
    public class EmailDomainValidator : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string email = value.ToString().ToLower();
            if (!EmailValidate(email))
                return new ValidationResult("Некоректний формат електрнної пошти", new[] { validationContext.MemberName});
            return null;
        }

        private bool EmailValidate(string email)
        {
            string pattern = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
            if (Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }
    }
}
