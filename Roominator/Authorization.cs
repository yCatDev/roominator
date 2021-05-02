using System.Runtime.InteropServices;
using System;
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
            if (!validationResult.validate() || await validationResult.UserExists())
                return false;
            Program.databaseManager.InsertData("user", "user_email, user_password", $"'{email}', '{password}'");
            return true;
        }
    }
}
