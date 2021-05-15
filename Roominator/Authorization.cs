using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Roominator {
    class Authorization {

        string email;
        string password;
        ValidationResult validationResult;

        public Authorization(string password, string password_copy, string email) {
            this.email = email.ToLower();
            this.password = password;
            validationResult = new ValidationResult(password, password_copy, email);
        }

        public async Task<bool> Register() {
            if (!await Program.databaseManager.InsertData("user", "user_email, user_password", $"'{email}', '{password}'"))
                return false;
            Console.WriteLine($"Registring user with mail {email}");
            return true;
        }

        public string GetEmail()
        {
            return email;
        }
    }
}
