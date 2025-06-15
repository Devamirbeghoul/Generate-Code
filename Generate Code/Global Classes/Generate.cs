using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Generate_Code.Global_Classes
{
    public static class clsGenerate
    {
        public static string BusinessLayer(string Table , List<(string Name, string DataType, bool IsNullable)> values)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using DataLayer;");
            sb.AppendLine("namespace BusinessLayer");
            sb.AppendLine("{");
            sb.AppendLine($"    public class cls{Table}");
            sb.AppendLine("    {");

            #region Variables
            sb.AppendLine("        public enum enMode { AddNew = 0, Update = 1 };");
            sb.AppendLine("        public enMode _Mode = enMode.AddNew;\n");
            
            foreach (var value in values)
                sb.AppendLine($"        public {clsSystem.ConvertSqlToCSharp(value.DataType , value.IsNullable)} {value.Name} {{ get; set; }}");
            #endregion

            #region Constructors
            sb.AppendLine($"\n        public cls{Table}()");
            sb.AppendLine("        {");
            sb.AppendLine("            _Mode = enMode.AddNew;");
            foreach (var value in values)
                sb.AppendLine($"            {value.Name} = {clsSystem.DefaultValue(value.DataType, value.IsNullable)};");

            sb.AppendLine("        }\n");
            sb.AppendLine($"        private cls{Table}({string.Join(", " , values.Select(c => $"{clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {");
            sb.AppendLine("            _Mode = enMode.Update;");
            foreach (var value in values)
                sb.AppendLine($"            this.{value.Name} = {value.Name};");
            sb.AppendLine("        }\n");
            #endregion

            #region Save Method
            sb.AppendLine("        public bool Save()");
            sb.AppendLine("        {");
            sb.AppendLine("            switch (_Mode)");
            sb.AppendLine("            {");
            sb.AppendLine("                case enMode.AddNew:");
            sb.AppendLine("                    {");
            sb.AppendLine("                        if (_AddNew())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            _Mode = enMode.Update;");
            sb.AppendLine("                            return true;");
            sb.AppendLine("                        }");
            sb.AppendLine("                        else");
            sb.AppendLine("                            return false;");
            sb.AppendLine("                    }");
            sb.AppendLine("                case enMode.Update:");
            sb.AppendLine("                    return _Update();");
            sb.AppendLine("            }");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }\n");
            #endregion

            #region AddNew Method
            sb.AppendLine("        private bool _AddNew()");
            sb.AppendLine("        {");
            sb.AppendLine($"            this.{values[0].Name} = cls{Table}Data.AddNew({string.Join(", ", values.Skip(1).Select(c => $"this.{c.Name}"))});");
            sb.AppendLine($"            return {values[0].Name} != -1;");
            sb.AppendLine("        }\n");
            #endregion

            #region Update Method
            sb.AppendLine("        private bool _Update()");
            sb.AppendLine($"            => cls{Table}Data.Update({string.Join(", ", values.Select(c => $"this.{c.Name}"))});");
            sb.AppendLine("\n");
            #endregion

            #region Delete Method
            sb.AppendLine($"        public static bool Delete({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name})");
            sb.AppendLine($"            => cls{Table}Data.Delete({values[0].Name});");
            sb.AppendLine("\n");
            #endregion

            #region GeAll Method
            sb.AppendLine($"        public static DataTable GetAll{Table}()");
            sb.AppendLine($"            => cls{Table}Data.GetAll{Table}();");
            sb.AppendLine("\n");
            #endregion

            #region Find Method
            sb.AppendLine($"        public static cls{Table} Find({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name})");
            sb.AppendLine("        {");
            for (int i = 1; i < values.Count; i++)
                sb.AppendLine($"            {clsSystem.ConvertSqlToCSharp(values[i].DataType, values[i].IsNullable)} {values[i].Name} = {clsSystem.DefaultValue(values[i].DataType, values[i].IsNullable)};");
            sb.AppendLine($"\n            if (cls{Table}Data.GetByID({values[0].Name}, {string.Join(", " , values.Skip(1).Select(c => $"ref {c.Name}"))}))");
            sb.AppendLine($"                return new cls{Table}({string.Join(", ", values.Select(c => $"{c.Name}"))});");
            sb.AppendLine("\n            return null;");
            sb.AppendLine("        }\n");
            #endregion

            #region GetAllPerPages Method
            sb.AppendLine($"        public static DataTable GetAll{Table}PerPages(int NumberPage, int SizePage)");
            sb.AppendLine($"            => cls{Table}Data.GetAll{Table}PerPages(NumberPage, SizePage);");
            #endregion

            #region GetAllPerPagesBy Method
            sb.AppendLine($"        public static DataTable Get{Table}PerPagesBy<T>(int PageNumber, int PageSize, string Parameter, T Value)");
            sb.AppendLine($"            => cls{Table}Data.Get{Table}PerPagesBy(PageNumber, PageSize, Parameter, Value)");
            #endregion

            #region GetTotalPages Method
            sb.AppendLine("        public static int? GetTotalPages(int PageSize)");
            sb.AppendLine($"            => cls{Table}Data.GetTotalPages(PageSize)");
            #endregion

            #region GetTotalPagesBy Method
            sb.AppendLine("        public static int? GetTotalPagesBy<T>(int PageSize, string Parameter, T Value)");
            sb.AppendLine($"            => cls{Table}Data.GetTotalPagesBy(PageSize, Parameter, Value);");
            #endregion

            sb.AppendLine("    }");

            sb.AppendLine("}");


            return sb.ToString();
        }

        public static string Settings()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Diagnostics;");
            sb.AppendLine("using System.Configuration;");

            sb.AppendLine("\n\n     /*\n\n     ***  YOU HAVE TO ADD ADD A FRERANCE TO DATA ACCESS LAYER TO USE - Configuration Manager -  ***\n\n                    Go To Reeferences and Add \"System.Configuration\"\n\n     */");

            sb.AppendLine("namespace DataLayer");
            sb.AppendLine("{");
            sb.AppendLine("    public static class  clsSettings");
            sb.AppendLine("    {");
            sb.AppendLine("        // Put here your connection string name");
            sb.AppendLine("        private static string ConnectionStringName = string.Empty;\n");
            sb.AppendLine("        public static void LogError(Exception ex)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                if (!EventLog.SourceExists(\"MyApp\"))");
            sb.AppendLine("                    EventLog.CreateEventSource(\"MyApp\", \"Application\");\n");
            sb.AppendLine("                EventLog.WriteEntry(\"MyApp\", $\"Error: {ex.Message}\\nStackTrace: {ex.StackTrace}\", EventLogEntryType.Error);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception)");
            sb.AppendLine("            {");
            sb.AppendLine("            }");
            sb.AppendLine("        }\n");
            sb.AppendLine("         public static string GetConnectionString()");
            sb.AppendLine("         {");
            sb.AppendLine("             return ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString ?? string.Empty;");
            sb.AppendLine("         }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public static string Generics()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("namespace DataLayer");
            sb.AppendLine("{\n");
            sb.AppendLine("public class clsGenerics");
            sb.AppendLine("{");

            #region GetAll Method
            sb.AppendLine("        public static DataTable GetAll(string StoredProcedure)");
            sb.AppendLine("        {");
            sb.AppendLine("            DataTable Records = new DataTable();\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine("                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;\n");
            sb.AppendLine("                        Connection.Open();\n");
            sb.AppendLine("                        using (SqlDataReader Reader = Command.ExecuteReader())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            if (Reader.Read())");
            sb.AppendLine("                                Records.Load(Reader);");
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return Records;");
            sb.AppendLine("        }\n");
            #endregion

            #region Delete Method
            sb.AppendLine("        public static bool Delete<T>(string StoredProcedure , string ParameterName , T Value)");
            sb.AppendLine("        {");
            sb.AppendLine("            int Result = 0;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine("                   using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                   {");
            sb.AppendLine("                       Command.Parameters.AddWithValue($\"@{ParameterName}\", (object)Value ?? DBNull.Value);\n");
            sb.AppendLine("                       Connection.Open();\n");
            sb.AppendLine("                       result = Command.ExecuteNonQuery();");
            sb.AppendLine("                   }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("                return false;");
            sb.AppendLine("            }");
            sb.AppendLine("            return result > 0;");
            sb.AppendLine("        }\n");
            #endregion

            #region Exists V1 Method
            sb.AppendLine("        public static bool Exists<T>(string StoredProcedure, string ParameterName, T Value)");
            sb.AppendLine("        {");
            sb.AppendLine("            bool IsFound = false;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                     {");
            sb.AppendLine("                          Command.CommandType = CommandType.StoredProcedure;\n");
            sb.AppendLine("                          Command.Parameters.AddWithValue($\"@{ParameterName}\", (object)Value ?? DBNull.Value);\n");
            sb.AppendLine("                          SqlParameter existsParam = new SqlParameter(\"@Exists\", SqlDbType.Bit)");
            sb.AppendLine("                          {");
            sb.AppendLine("                               Direction = ParameterDirection.Output");
            sb.AppendLine("                          };\n");
            sb.AppendLine("                          Command.Parameters.Add(existsParam);");
            sb.AppendLine("                          Connection.Open();");
            sb.AppendLine("                          Command.ExecuteNonQuery()");
            sb.AppendLine("                          IsFound = (bool)existsParam.Value;");
            sb.AppendLine("                     }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return IsFound;");
            sb.AppendLine("        }\n");
            #endregion

            #region Exists V2 Method
            sb.AppendLine("        public static bool Exists<T1 , T2>(string StoredProcedure, string ParameterName1, T1 Value1, string ParameterName2, T2 Value2)");
            sb.AppendLine("        {");
            sb.AppendLine("            bool IsFound = false;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                     {");
            sb.AppendLine("                          Command.CommandType = CommandType.StoredProcedure;\n");
            sb.AppendLine("                          Command.Parameters.AddWithValue($\"@{ParameterName1}\", (object)Value1 ?? DBNull.Value);\n");
            sb.AppendLine("                          Command.Parameters.AddWithValue($\"@{ParameterName2}\", (object)Value2 ?? DBNull.Value);\n");
            sb.AppendLine("                          SqlParameter existsParam = new SqlParameter(\"@Exists\", SqlDbType.Bit)");
            sb.AppendLine("                          {");
            sb.AppendLine("                               Direction = ParameterDirection.Output");
            sb.AppendLine("                          };\n");
            sb.AppendLine("                          Command.Parameters.Add(existsParam);");
            sb.AppendLine("                          Connection.Open();");
            sb.AppendLine("                          Command.ExecuteNonQuery()");
            sb.AppendLine("                          IsFound = (bool)existsParam.Value;");
            sb.AppendLine("                     }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return IsFound;");
            sb.AppendLine("        }\n");
            #endregion

            #region GetAllPerPages Method
            sb.AppendLine("        public static DataTable GetAllPerPages(string StoredProcedure, int PageNumber, int PageSize)");
            sb.AppendLine("        {");
            sb.AppendLine("            DataTable Records = new DataTable();\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine("                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine("                        Command.Parameters.AddWithValue($\"@PageNumber\", PageNumber);");
            sb.AppendLine("                        Command.Parameters.AddWithValue($\"@PageSize\", @PageSize);\n");
            sb.AppendLine("                        Connection.Open();");
            sb.AppendLine("                        using (SqlDataReader Reader = Command.ExecuteReader())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            if (Reader.HasRows)");
            sb.AppendLine("                                Records.Load(Reader);");
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return Records;");
            sb.AppendLine("        }\n");

            #endregion

            #region GetAllPerPagesBy Method
            sb.AppendLine("        public static DataTable GetAllPerPages<T>(string StoredProcedure, int PageNumber, int PageSize , T Value)");
            sb.AppendLine("        {");
            sb.AppendLine("            DataTable Records = new DataTable();\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine("                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine("                        Command.Parameters.AddWithValue($\"@PageNumber\", PageNumber);");
            sb.AppendLine("                        Command.Parameters.AddWithValue($\"@PageSize\", @PageSize);");
            sb.AppendLine("                        Command.Parameters.AddWithValue(\"@Value\", (object)Value ?? DBNull.Value);\n");
            sb.AppendLine("                        Connection.Open();");
            sb.AppendLine("                        using (SqlDataReader Reader = Command.ExecuteReader())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            if (Reader.HasRows)");
            sb.AppendLine("                                Records.Load(Reader);");
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return Records;");
            sb.AppendLine("        }");

            #endregion

            #region GetTotalPages Method
            sb.AppendLine("        public static int? GetTotalPages(string StoredProcedure, int PageSize)");
            sb.AppendLine("        {");
            sb.AppendLine("            int? TotalPages = null;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine("                        Command.Parameters.AddWithValue(\"@PageSize\", PageSize);\n");

            sb.AppendLine("                        SqlParameter totalPagesParam = new SqlParameter(\"@TotalPages\", SqlDbType.Int)");
            sb.AppendLine("                        {");
            sb.AppendLine("                            Direction = ParameterDirection.Output");
            sb.AppendLine("                        };\n");
            sb.AppendLine("                        Command.Parameters.Add(totalPagesParam);");
            sb.AppendLine("                        Connection.Open();");
            sb.AppendLine("                        Command.ExecuteNonQuery()");
            sb.AppendLine("                        TotalPages = (int?)totalPagesParam.Value;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return TotalPages;");
            sb.AppendLine("        }\n");
            #endregion

            #region GetTotalPagesBy Method
            sb.AppendLine("        public static int? GetTotalPages<T>(string StoredProcedure , int PageSize, T Value)");
            sb.AppendLine("        {");
            sb.AppendLine("            int? TotalPages = null;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(StoredProcedure, Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine("                        Command.Parameters.AddWithValue(\"@PageSize\", PageSize);");
            sb.AppendLine("                        Command.Parameters.AddWithValue(\"@Value\", (object)Value ?? DBNull.Value)\n");

            sb.AppendLine("                        SqlParameter totalPagesParam = new SqlParameter(\"@TotalPages\", SqlDbType.Int)");
            sb.AppendLine("                        {");
            sb.AppendLine("                            Direction = ParameterDirection.Output");
            sb.AppendLine("                        };\n");
            sb.AppendLine("                        Command.Parameters.Add(totalPagesParam);");
            sb.AppendLine("                        Connection.Open();");
            sb.AppendLine("                        Command.ExecuteNonQuery()");
            sb.AppendLine("                        TotalPages = (int?)totalPagesParam.Value;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return TotalPages;");
            sb.AppendLine("        }\n");
            #endregion

            return sb.ToString();
        }

        public static string DataLayer(string Table, List<(string Name, string DataType, bool IsNullable)> values)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Configuration;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Threading.Tasks;");

            sb.AppendLine("\n\n     /*\n     ***  YOU HAVE TO ADD YOUR CONNECTION STRING IN - App.config - FILE  ***\r\n       < connectionStrings>\r\n              < add name=\"MyDatabaseConnection\" \n             connectionString=\"Connection String Here\" \n             providerName=\"System.Data.SqlClient\" /> \r\n     </connectionStrings>\r\n     */\n\n");

            sb.AppendLine("namespace DataLayer");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class  cls{Table}Data");
            sb.AppendLine("    {");

            #region GetByID Method
            sb.AppendLine($"        public static bool GetByID({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name}, {string.Join(", ", values.Skip(1).Select(c => $"ref {clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                   using (SqlCommand Command = new SqlCommand(\"sp_{Table}_GetByID\", Connection))");
            sb.AppendLine("                   {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine($"                        Command.Parameters.AddWithValue(\"@ID\", (object){values[0].Name} ?? DBNull.Value);\n");
            sb.AppendLine("                        Connection.Open();\n");
            sb.AppendLine("                        using (SqlDataReader Reader = Command.ExecuteReader())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            if (Reader.Read())");
            sb.AppendLine("                            {");
                
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i].IsNullable)
                {
                    sb.AppendLine($"                                if (Reader[\"{values[i].Name}\"] == DBNull.Value)");
                    sb.AppendLine($"                                    {values[i].Name} = {clsSystem.DefaultValue(values[i].DataType, false)};");
                    sb.Append($"                                else\n    ");
                }
                
                sb.AppendLine($"                                {values[i].Name} = {clsSystem.ConvertValue($"Reader[\"{values[i].Name}\"]", values[i].DataType)};\n");
            }
                
            sb.AppendLine("                                return true;");
            sb.AppendLine("                            }");
            sb.AppendLine("                        }");
            sb.AppendLine("                   }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("                return false;");
            sb.AppendLine("            }");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }\n");
            #endregion

            #region Delete Method
            sb.AppendLine($"        public static bool Delete({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name})");
            sb.AppendLine($"            => clsGenerics.Delete<int?>(\"sp_{Table}_DeleteByID\", \"{values[0].Name}\", {values[0].Name});\n");
            #endregion

            #region GetAll Method
            sb.AppendLine($"        public static DataTable GetAll{Table}()");
            sb.AppendLine($"            => clsGenerics.GetAll(\"sp_{Table}_GetAll\");\n");
            #endregion

            #region AddNew Method
            sb.AppendLine($"        public int? AddNew({string.Join(", ", values.Skip(1).Select(c => $"{clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {"); 
            sb.AppendLine("            int? ID = null;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(\"sp_{Table}_AddNew\", Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        Command.CommandType = CommandType.StoredProcedure;\n");

            for (int i = 1 ; i < values.Count; i++)
                sb.AppendLine($"                        Command.Parameters.AddWithValue(\"@{values[i].Name}\" , " + ((values[i].IsNullable) ? $"(object){values[0].Name} ?? DBNull.Value" : $"{values[i].Name}") + ");\n");

            sb.AppendLine("                        SqlParameter outputIdParam = new SqlParameter(\"@ID\", SqlDbType.Int)");
            sb.AppendLine("                        {");
            sb.AppendLine("                            Direction = ParameterDirection.Output");
            sb.AppendLine("                        };\n");
            sb.AppendLine("                        Command.Parameters.Add(outputIdParam);");
            sb.AppendLine("                        Connection.Open();");
            sb.AppendLine("                        Command.ExecuteNonQuery()");
            sb.AppendLine("                        ID = (int?)outputIdParam.Value;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return ID;");
            sb.AppendLine("        }\n");
            #endregion

            #region Update Method
            sb.AppendLine($"        public static bool Update({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name}, {string.Join(", ", values.Skip(1).Select(c => $"{clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {");
            sb.AppendLine("            int result = 0;\n");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    using (SqlCommand Command = new SqlCommand(\"sp_{Table}_UpdateByID\", Connection))");
            sb.AppendLine("                    {");
            sb.AppendLine($"                        Command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine($"                        Command.Parameters.AddWithValue(\"@ID\", {values[0].Name});\n");
            
            for (int i = 1; i < values.Count; i++)
                sb.AppendLine($"                        Command.Parameters.AddWithValue(\"@{values[i].Name}\" , " + ((values[i].IsNullable) ? $"(object){values[0].Name} ?? DBNull.Value" : $"{values[i].Name}") + ");\n");

            sb.AppendLine("                        Connection.Open();\n");
            sb.AppendLine("                        result = Command.ExecuteNonQuery();");
            sb.AppendLine("                    }");
                
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                clsSettings.LogError(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("            return result > 0;");
            sb.AppendLine("        }\n");
            #endregion

            #region GetAllPerPages Method
            sb.AppendLine($"        public static DataTable GetAll{Table}PerPages(int PageNumber, int PageSize)");
            sb.AppendLine($"            => clsGenerics.GetAllPerPages(\"sp_{Table}_GetAllPerPages\", PageNumber, PageSize);");
            #endregion

            #region GetAllPerPagesBy Method
            sb.AppendLine($"        public static DataTable Get{Table}PerPagesBy<T>(int PageNumber, int PageSize, string Patameter, T Value)");
            sb.AppendLine($"            => clsGenerics.GetAllPerPages($\"sp_{Table}_GetPerPagesBy{{Patameter}}\", PageNumber, PageSize, Value);");
            #endregion

            #region GetTotalPages Method
            sb.AppendLine("        public static int? GetTotalPages(int PageSize)");
            sb.AppendLine($"            => clsGenerics.GetTotalPages(\"sp_{Table}_GetTotalPages\", PageSize);");
            #endregion

            #region GetTotalPagesBy Method
            sb.AppendLine("        public static int? GetTotalPagesBy<T>(int PageSize, string Patameter, T Value)");
            sb.AppendLine($"            => clsGenerics.GetTotalPages<T>($\"sp_{Table}_GetTotalPagesBy{{Patameter}}\", PageSize, Value);");
            #endregion

            sb.AppendLine("    }");

            sb.AppendLine("}");

            return sb.ToString();
        }

        static string Where(string DataType , string By)
        {
            string type = DataType.ToLower();

            switch (type)
            {
                case "bigint":
                case "binary":
                case "varbinary":
                case "image":
                case "bit":
                case "decimal":
                case "money":
                case "smallmoney":
                case "numeric":
                case "smallint":
                case "float":
                case "int":
                case "real":
                case "tinyint":
                    return $"{By} = @{By}";
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    return $"{By} like @{By} + \'%\'";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "datetimeoffset":
                case "time":
                    return $"FORMAT({By}, 'dd/MM/yyyy') = @{By}";
                default:
                    return $"{By} = @{By}";
            }
        }

        static string GetAllBy(string Table , string By , string DataType , string OrderBy)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Create Procedure sp_{Table}_GetAllPerPageBy{By}");
            sb.AppendLine("@PageNumber int ,");
            sb.AppendLine("@PageSize int ,");
            sb.AppendLine($"@{By} {DataType}");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine("DECLARE @Offset INT");
            sb.AppendLine("SET @Offset = (@PageNumber - 1) * @PageSize;");
            sb.AppendLine($"Select * from {Table} where {Where(DataType , By)}");
            sb.AppendLine($"ORDER BY {OrderBy}");
            sb.AppendLine($"OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;");

            sb.AppendLine("end");


            return sb.ToString();
        }

        static string GetTotalPagesBy(string Table, string By, string DataType, string OrderBy)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Create Procedure sp_{Table}_GetTotalPagesBy{By}");
            sb.AppendLine("@PageSize INT,");
            sb.AppendLine($"@{By} {DataType},");
            sb.AppendLine("@TotalPages INT OUTPUT ");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"Declare @TotalRows INT = (Select Count(*) from {Table} where {Where(DataType , By)}) ;");
            sb.AppendLine("SET @TotalPages = CEILING(CAST(@TotalRows AS FLOAT) / @PageSize);");

            sb.AppendLine("end");


            return sb.ToString();
        }

        public static string StoredProcedures(string Table, List<(string Name, string DataType, bool IsNullable)> values)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Use {clsConnection.DBName}\n");

            #region AddNew

            sb.AppendLine($"Create Procedure sp_{Table}_AddNew");
            
            sb.AppendLine(string.Join(",\n" , values.Skip(1).Select(x => "@" + x.Name + " " + x.DataType)) + ",");

            sb.AppendLine($"@{values[0].Name} {values[0].DataType} Output");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"INSERT INTO {Table} ({string.Join(", ", values.Skip(1).Select(c => $"{c.Name}"))}) VALUES ({string.Join(", ", values.Skip(1).Select(c => $"@{c.Name}"))});\n");

            sb.AppendLine($"set @{values[0].Name} = SCOPE_IDENTITY();");

            sb.AppendLine("end");

            #endregion

            #region Update

            sb.AppendLine($"Create Procedure sp_{Table}_UpdateByID");

            sb.AppendLine(string.Join(",\n", values.Select(x => "@" + x.Name + " " + x.DataType)));

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"UPDATE {Table} set \n{string.Join(", \n", values.Skip(1).Select(c => $"{c.Name} = @{c.Name}"))} \nwhere {values[0].Name} = @{values[0].Name};\n");

            sb.AppendLine("end");

            #endregion

            #region GetAll

            sb.AppendLine($"Create Procedure sp_{Table}_GetAll");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"Select * from {Table};\n");

            sb.AppendLine("end");

            #endregion

            #region Find ByID

            sb.AppendLine($"Create Procedure sp_{Table}_GetByID");
            sb.AppendLine($"@{values[0].Name} {values[0].DataType}");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"Select * from {Table}");
            sb.AppendLine($"Where {values[0].Name} = @{values[0].Name}");

            sb.AppendLine("end");

            #endregion

            #region Delete ByID

            sb.AppendLine($"Create Procedure sp_{Table}_DeleteByID");
            sb.AppendLine($"@{values[0].Name} {values[0].DataType}");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"Delete from {Table}");
            sb.AppendLine($"Where {values[0].Name} = @{values[0].Name}");

            sb.AppendLine("end");

            #endregion

            #region GetAllPerPage

            sb.AppendLine($"Create Procedure sp_{Table}_GetAllPerPage");
            sb.AppendLine("@PageNumber int ,");
            sb.AppendLine("@PageSize int ");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine("DECLARE @Offset INT");
            sb.AppendLine("SET @Offset = (@PageNumber - 1) * @PageSize;");
            sb.AppendLine($"Select * from {Table}");
            sb.AppendLine($"ORDER BY {values[0].Name}");
            sb.AppendLine($"OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;");

            sb.AppendLine("end");

            #endregion

            #region GetAllPerPage By

            for (int i = 0; i < values.Count; i++)
                sb.AppendLine(GetAllBy(Table, values[i].Name, values[i].DataType, values[0].Name));

            #endregion

            #region GetTotalPages

            sb.AppendLine($"Create Procedure sp_{Table}_GetTotalPages");
            sb.AppendLine("@PageSize INT,");
            sb.AppendLine("@TotalPages INT OUTPUT ");

            sb.AppendLine("as");
            sb.AppendLine("Begin");

            sb.AppendLine($"Declare @TotalRows INT = (Select Count(*) from {Table}) ;");
            sb.AppendLine("SET @TotalPages = CEILING(CAST(@TotalRows AS FLOAT) / @PageSize);");

            sb.AppendLine("end");

            #endregion

            #region GetTotalPages By

            for (int i = 0; i < values.Count; i++)
                sb.AppendLine(GetTotalPagesBy(Table, values[i].Name, values[i].DataType, values[0].Name));

            #endregion

            return sb.ToString();
        }
    }
}
