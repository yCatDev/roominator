using System.Runtime.InteropServices;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Roominator
{
    class ValidationResult
    {
        string password;
        string password_copy;
        string email;
        bool correctEmail;
        bool correctPassword;

        public ValidationResult(string password, string password_copy, string email) {
            this.password = password;
            this.password_copy = password_copy;
            this.email = email;
            this.correctEmail = false;
            this.correctPassword = false;
        }

        public async Task<RegistrationErrors> Validate() {
            RegistrationErrors registrationError = RegistrationErrors.Correct;
            if (email.Length == 0)
                registrationError = RegistrationErrors.EmailIsEmpty;
            else if (!email.Contains('@'))
                registrationError = RegistrationErrors.EmailFormat;
            else if (password.Length == 0)
                registrationError = RegistrationErrors.PasswordIsEmpty;
            else if (password.Length < 8 || password.Length > 50)
                registrationError = RegistrationErrors.PasswordLength;
            else if (password.Equals(email))
                registrationError = RegistrationErrors.PasswordEqualsEmail;
            else if (password_copy.Length == 0)
                registrationError = RegistrationErrors.PasswordCopyIsEmpty;
            else if (!password_copy.Equals(password))
                registrationError = RegistrationErrors.PasswordCopyNotEqualsPassword;
            else if (await UserExists())
            {
                Task.WaitAll();
                registrationError = RegistrationErrors.EmailAlreadyExists;
            }
            return registrationError;
        }

        public async Task<bool> IsEverythingCorrect() {
            DataTable res = await Program.databaseManager.ExecQuery($"SELECT * FROM public.user WHERE public.user.user_email = '{email}'");
            if (res.Rows.Count == 1)
            {
                correctEmail = true;
                res = await Program.databaseManager.ExecQuery($"SELECT user_password FROM public.user WHERE public.user.user_email = '{email}'");
                if (res.Rows[0][0].ToString() == password)
                {
                    correctPassword = true;
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
    }

    public enum RegistrationErrors{ 
        EmailIsEmpty,
        EmailAlreadyExists,
        EmailFormat,
        PasswordIsEmpty,
        PasswordLength,
        PasswordEqualsEmail,
        PasswordCopyNotEqualsPassword,
        PasswordCopyIsEmpty,
        Correct
    }
}

