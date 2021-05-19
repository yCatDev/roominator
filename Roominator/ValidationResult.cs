using System.Runtime.InteropServices;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

namespace Roominator
{
    class ValidationResult
    {
        string password;
        string password_copy;
        string email;
        bool correctEmail;
        bool correctPassword;

        public ValidationResult()
        {
            this.password = "";
            this.password_copy = "";
            this.email = "";
            correctEmail = false;
            correctPassword = false;
        }

        public ValidationResult(string password, string password_copy, string email) {
            this.password = password;
            this.password_copy = password_copy;
            this.email = email.ToLower();
            correctEmail = false;
            correctPassword = false;
        }

        private bool EmailValidate(string email)
        {
            string pattern = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
            if (Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        public async Task<RegistrationErrors> Validate() {
            RegistrationErrors registrationError = RegistrationErrors.Correct;
            if (email.Length == 0)
                registrationError = RegistrationErrors.EmailIsEmpty;
            else if (!EmailValidate(email))
                registrationError = RegistrationErrors.EmailFormat;
            else if (await UserExists())
                registrationError = RegistrationErrors.EmailAlreadyExists;
            else if (password.Length == 0)
                registrationError = RegistrationErrors.PasswordIsEmpty;
            else if (password.Length < 8)
                registrationError = RegistrationErrors.PasswordLengthTooShort;
            else if (password.Length > 50)
                registrationError = RegistrationErrors.PasswordLengthTooLong;
            else if (password.Equals(email))
                registrationError = RegistrationErrors.PasswordEqualsEmail;
            else if (password_copy.Length == 0)
                registrationError = RegistrationErrors.PasswordCopyIsEmpty;
            else if (!password_copy.Equals(password))
                registrationError = RegistrationErrors.PasswordCopyNotEqualsPassword; 
            return registrationError;
        }

        public async Task<bool> IsEverythingCorrect() {
            DataTable res = await Program.databaseManager.ExecQuery($"SELECT * FROM public.user WHERE public.user.user_email = '{email}'");
            if (res == null || res.Rows.Count == 0 || email.Length == 0)
                return false;
            if (res.Rows.Count == 1)
            {
                correctEmail = true;
                res = await Program.databaseManager.ExecQuery($"SELECT user_password FROM public.user WHERE public.user.user_email = '{email}'");
                if (res.Rows[0][0].ToString() == password)
                {
                    correctPassword = true;
                    Program.currentEmail = email;
                    return true;
                }
            }
            return false;
        }

        public bool GetCorrectEmail() {
            return correctEmail;
        }

        public bool GetCorrectPassword() {
            return correctPassword;
        }

        public async Task<bool> UserExists() {
            DataTable res  = await Program.databaseManager.ExecQuery($"SELECT * FROM public.user WHERE public.user.user_email = '{email}'");
            if (res == null || res.Rows.Count == 0)
                return false;
            return true;
        }

        public async Task<bool> CheckPremium()
        {
            // Проверка наличия премиума в базе
            return true;
        }
    }

    public enum RegistrationErrors{ 
        EmailIsEmpty,
        EmailAlreadyExists,
        EmailFormat,
        PasswordIsEmpty,
        PasswordLengthTooShort,
        PasswordLengthTooLong,
        PasswordEqualsEmail,
        PasswordCopyNotEqualsPassword,
        PasswordCopyIsEmpty,
        Correct
    }
}

