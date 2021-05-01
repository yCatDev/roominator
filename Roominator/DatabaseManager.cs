using System;
using System.Runtime.InteropServices;




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
                "Password=a59a3aaea385ae55734f1dcf1442c731df76ea594bb4e7e838ad369899d8e3ae;");
        }

        public DataTable ExecQuery(string sqlCommand)
        {
            Connection.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, Connection);
            cmd.Prepare();

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

            Connection.Close();
            return dataTable;
        }
        public void UpdateData(string tableName, string querySetData, int id)
        {
            Connection.Open();
            var cmd = $"UPDATE {tableName} SET {querySetData} WHERE id = {id};";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                command.Prepare();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            Connection.Close();
        }

        public void InsertData(string tableName, string fields, string values)
        {
            Connection.Open();
            var cmd = $"INSERT INTO public.{tableName} ({fields}) VALUES ({values});";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                command.Prepare();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            Connection.Close();
        }

        public void DeleteData(string tableName, int id)
        {
            Connection.Open();
            var cmd = $"DELETE FROM public.{tableName} WHERE id = {id};";
            NpgsqlCommand command = new NpgsqlCommand(cmd, Connection);
            try
            {
                command.Prepare();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(cmd);
                Console.WriteLine(e);
                throw;
            }

            Connection.Close();
        }
    }
}
