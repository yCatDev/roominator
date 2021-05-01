using System.Runtime.InteropServices;
using System;

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


        public bool register() {
            if (validationResult.validate())
            {
                if (validationResult.userExists())
                    return false;
                Program.databaseManager.InsertData("User", "user_email, user_password", $"\"{email}\", \"{password}\"");
            }
            return true;
        }
    }
}
