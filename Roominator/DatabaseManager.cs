using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Roominator {

    using Npgsql;
    using System.Data;

    public class DatabaseManager {

        private NpgsqlConnection _connection;
        public NpgsqlConnection Connection => _connection;

        public DatabaseManager()
        {
            _connection = new NpgsqlConnection("Host=ec2-34-203-255-149.compute-1.amazonaws.com;Port=5432;Database=de66qfduidsjeq;" +
                "Username=myemokdpaypchi;" +
                "Password=a59a3aaea385ae55734f1dcf1442c731df76ea594bb4e7e838ad369899d8e3ae;" +
                "sslmode = Require;Trust Server Certificate = true;");
        }
        
        public async Task<DataTable> ExecQuery(string sqlCommand/*, params string[] parameters*/)
        {
            /*foreach (var parameter in parameters)
            {
                sqlCommand = sqlCommand.ReplaceFirst("'$'", parameter);
            }*/
           // await Connection.WaitAsync();
            await Connection.OpenAsync();
            await using var cmd = new NpgsqlCommand(sqlCommand, Connection);
            await cmd.PrepareAsync();
				
            var da = new NpgsqlDataAdapter(cmd);
				
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            da.Fill(dataSet);
				
            try
            {
                dataTable = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
                
            await Connection.CloseAsync();
            return dataTable;
        }

        public async Task UpdateData(string tableName, string querySetData, int id)
        {
           // await Connection.WaitAsync();
            await Connection.OpenAsync();
            var cmd = $"UPDATE {tableName} SET {querySetData} WHERE id = {id};";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            await Connection.CloseAsync();
        }

        public async Task InsertData(string tableName, string fields, string values)
        {
            //await Connection.WaitAsync();
            await Connection.OpenAsync();
            var cmd = $"INSERT INTO public.{tableName} ({fields}) VALUES ({values});";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            await Connection.CloseAsync();
        }

        public async Task DeleteData(string tableName, int id)
        {
           // await Connection.WaitAsync();
            await Connection.OpenAsync();
            var cmd = $"DELETE FROM public.{tableName} WHERE id = {id};";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            await Connection.CloseAsync();
        }
    }
}
