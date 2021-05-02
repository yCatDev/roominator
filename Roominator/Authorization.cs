using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Roominator {
    class Authorization {

        string email;
        string password;
        string password_copy;
        ValidationResult validationResult;

        public Authorization(string password, string password_copy, string email) {
            this.email = email;
            this.password = password;
            validationResult = new ValidationResult(password, password_copy, email);
        }

        public async Task<bool> Register() {
            await Program.databaseManager.InsertData("user", "user_email, user_password", $"'{email}', '{password}'");
                Console.WriteLine($"Registring user with mail {email}");
            return true;
        }
    }
}
