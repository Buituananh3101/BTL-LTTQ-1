using Microsoft.Data.SqlClient; // Sử dụng thư viện SQL Server
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Quanlilaptop
{
    internal class Database
    {
        // Chuỗi kết nối SQL Server (đã sửa lỗi cú pháp ở Integrated Security và thêm TrustServerCertificate)
        private static Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection("Server=localhost\\SQLEXPRESS;Database=quanlimaytinh2;Integrated Security=true;TrustServerCertificate=True;");

        public static void Execute(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (string key in parameters.Keys)
                    {
                        // Thay MySqlParameter bằng SqlParameter
                        command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter(key, parameters[key] ?? DBNull.Value));
                    }
                }

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static DataTable Query(string sql, Dictionary<string, object> parameters = null)
        {
            DataTable table = new DataTable();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection);
                if (parameters != null)
                {
                    foreach (string key in parameters.Keys)
                    {
                        // Thay MySqlParameter bằng SqlParameter
                        command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter(key, parameters[key] ?? DBNull.Value));
                    }
                }

                // Thay MySqlDataAdapter bằng SqlDataAdapter
                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return table;
        }
    }
}