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
        public bool correctEmail;
        public bool correctPassword;

        public ValidationResult(string password, string password_copy, string email) {
            this.password = password;
            this.password_copy = password_copy;
            this.email = email;
            this.correctEmail = false;
            this.correctPassword = false;
        }

        public bool validate() {
            if (password.Length >= 3 && password.Length <= 50)
            {
                return true;
            }
            return false;
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

        public async Task<bool> UserExists() {
            DataTable res  = await Program.databaseManager.ExecQuery($"SELECT * FROM public.user WHERE public.user.user_email = '{email}'");
            if (res.Rows.Count == 0)
                return false;
            return true;
        }
    }
}
