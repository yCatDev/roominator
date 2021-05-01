using System.Runtime.InteropServices;
using System;
using System.Data;

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

        public bool validate() {
            return true;
        }

        public bool isEverythingCorrect() {
            return correctEmail && correctPassword;
        }

        public bool userExists() {
            DataTable res  = Program.databaseManager.ExecQuery($"SELECT User.email FROM User WHERE User.email = '{email}'");
            if (res.Rows.Count == 0)
                return true;
            return false;
        }
    }
}
