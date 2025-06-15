using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generate_Code.Global_Classes
{
    public static class clsConnection
    {
        private static string LoginConnectionString;
        private static string ConnectionString;

        private static string Server , UserId , Password ;

        public static string DBName;

        public static async Task<bool> Connect(string server , string userId , string password)
        {
            string loginConnectionString = $"Server={server};User Id={userId};Password={password}";

            try
            {
                using (SqlConnection con = new SqlConnection(loginConnectionString))
                {
                    await con.OpenAsync();
                    LoginConnectionString = loginConnectionString;
                    Server = server;
                    UserId = userId;
                    Password = password;
                    return true;
                }
            }
            catch (Exception ex)
            {
                clsSystem.ErrorLog(ex);
                return false;
            }
        }

        public static async Task <List<string>> GetAllDB()
        {
            List<string> list = new List<string>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(LoginConnectionString))
                {
                    string Query = @"SELECT name 
                        FROM sys.databases
                        WHERE database_id > 4
                        ORDER BY name;";

                    await Connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(Query, Connection))
                    {
                        using (SqlDataReader Reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await Reader.ReadAsync())
                                list.Add(Reader["name"].ToString());
                        }

                    }

                } 
            }

            catch (Exception ex)
            {
                clsSystem.ErrorLog(ex);
            }

            return list;
        }

        public static async Task<List<string>> GetAllTables(string DB)
        {
            List<string> list = new List<string>();

            ConnectionString = $"Server={Server};Database={DB};User Id={UserId};Password={Password}";

            try
            {
                using (SqlConnection Connection = new SqlConnection(ConnectionString))
                {
                    string Query = @"SELECT name
                        FROM sys.tables
                        ORDER BY name;";

                    await Connection.OpenAsync();

                    DBName = DB;

                    using (SqlCommand cmd = new SqlCommand(Query, Connection))
                    {
                        using (SqlDataReader Reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await Reader.ReadAsync())
                                list.Add(Reader["name"].ToString());
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsSystem.ErrorLog(ex);
            }

            return list;
        }

        public static async Task<List<(string Name , string DataType , bool IsNullable)>> GetColumnsInfoOfTable(string Table)
        {
            List<(string Name, string DataType, bool IsNullable)> list = new List<(string Name, string DataType, bool IsNullable)>() ;

            try
            {
                using (SqlConnection Connection = new SqlConnection(ConnectionString))
                {
                    string Query = @"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName;";

                    await Connection.OpenAsync();

                    using (SqlCommand Command = new SqlCommand(Query, Connection))
                    {
                        Command.Parameters.AddWithValue("@TableName", Table);

                        using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                        {
                            while (await Reader.ReadAsync())
                            {
                                list.Add((
                                    Reader["COLUMN_NAME"].ToString(),
                                    Reader["DATA_TYPE"].ToString(),
                                    Reader["IS_NULLABLE"].ToString() == "YES"
                                    ));
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                clsSystem.ErrorLog(e);
            }

            return list;
        }
    }
}
