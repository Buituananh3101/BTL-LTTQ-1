using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace Quanlilaptop
{
    internal class Database
    {
        private static MySqlConnection connection = new MySqlConnection("Data Source=localhost\\SQLEXPRESS; Database=quanlimaytinh2; Integrated Security=true=");
        public static void Execute(string sql, Dictionary<string, object> parameters = null)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(sql, connection);
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    command.Parameters.Add(new MySqlParameter(key, parameters[key])); 
                }
            }
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public static DataTable Query(string sql, Dictionary<string, object> parameters = null)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(sql, connection);
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    command.Parameters.Add(new MySqlParameter(key, parameters[key]));
                }
            }
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            connection.Close();
            return table;
        }
    }
}
