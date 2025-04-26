using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generate_Code.Global_Classes
{
    public static class clsGenerate
    {
        public static string BusinessLayer(string Table , List<(string Name, string DataType, bool IsNullable)> values)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Threading.Tasks;");
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
            sb.AppendLine("        public async Task <bool> Save()");
            sb.AppendLine("        {");
            sb.AppendLine("            switch (_Mode)");
            sb.AppendLine("            {");
            sb.AppendLine("                case enMode.AddNew:");
            sb.AppendLine("                    {");
            sb.AppendLine("                        if (await _AddNew())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            _Mode = enMode.AddNew;");
            sb.AppendLine("                            return true;");
            sb.AppendLine("                        }");
            sb.AppendLine("                        else");
            sb.AppendLine("                            return false;");
            sb.AppendLine("                    }");
            sb.AppendLine("                case enMode.Update:");
            sb.AppendLine("                    return await _Update();");
            sb.AppendLine("            }");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }\n");
            #endregion

            #region AddNew Method
            sb.AppendLine("        private async Task<bool> _AddNew()");
            sb.AppendLine("        {");
            sb.AppendLine($"            this.{values[0].Name} = await cls{Table}Data.AddNew({string.Join(", ", values.Skip(1).Select(c => $"this.{c.Name}"))});");
            sb.AppendLine($"            return {values[0].Name} != -1;");
            sb.AppendLine("        }\n");
            #endregion

            #region Update Method
            sb.AppendLine("        private async Task <bool> _Update()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return await cls{Table}Data.Update({string.Join(", ", values.Select(c => $"this.{c.Name}"))});");
            sb.AppendLine("        }\n");
            #endregion

            #region Delete Method
            sb.AppendLine($"        public static async Task <bool> Delete({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name})");
            sb.AppendLine("        {");
            sb.AppendLine($"            return await cls{Table}Data.Delete({values[0].Name});");
            sb.AppendLine("        }\n");
            #endregion

            #region GeAll Method
            sb.AppendLine($"        public static async Task<DataTable> GetAll{Table}()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return await cls{Table}Data.GetAll{Table}();");
            sb.AppendLine("        }\n");
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

            sb.AppendLine("    }");

            sb.AppendLine("}");


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
            sb.AppendLine("            using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("            {");
            sb.AppendLine($"                string Query = \"SELECT * FROM {Table} WHERE {values[0].Name} = @ID;\";\n");
            sb.AppendLine("                using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    Command.Parameters.AddWithValue(\"@ID\", {values[0].Name});\n");
            sb.AppendLine("                    try");
            sb.AppendLine("                    {");
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
            sb.AppendLine("                    }");
            sb.AppendLine("                    catch (Exception ex)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        clsSettings.LogError(ex);");
            sb.AppendLine("                        return false;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }\n");
            #endregion

            #region Delete Method
            sb.AppendLine($"        public static async Task<bool> Delete({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name})");
            sb.AppendLine("        {");
            sb.AppendLine("            int result = 0;\n");
            sb.AppendLine("            using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("            {");
            sb.AppendLine($"                string Query = \"DELETE FROM {Table} WHERE {values[0].Name} = @ID;\";\n");
            sb.AppendLine("                using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    Command.Parameters.AddWithValue(\"@ID\", {values[0].Name});\n");
            sb.AppendLine("                    try");
            sb.AppendLine("                    {");
            sb.AppendLine("                        await Connection.OpenAsync();\n");                    
            sb.AppendLine("                        result = await Command.ExecuteNonQueryAsync();");
            sb.AppendLine("                    }");
            sb.AppendLine("                    catch (Exception ex)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        clsSettings.LogError(ex);");
            sb.AppendLine("                        return false;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return result > 0;");
            sb.AppendLine("        }\n");
            #endregion

            #region GetAll Method
            sb.AppendLine($"        public static async Task<DataTable> GetAll{Table}()");
            sb.AppendLine("        {");
            sb.AppendLine("            DataTable Records = new DataTable();\n");
            sb.AppendLine("            using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("            {");
            sb.AppendLine($"                string Query = \"SELECT * FROM {Table};\";\n");
            sb.AppendLine("                using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                {");
            sb.AppendLine("                    try");
            sb.AppendLine("                    {");
            sb.AppendLine("                        await Connection.OpenAsync();\n");
            sb.AppendLine("                        using (SqlDataReader Reader = await Command.ExecuteReaderAsync())");
            sb.AppendLine("                        {");
            sb.AppendLine("                            if (Reader.HasRows)");
            sb.AppendLine("                                Records.Load(Reader);");
            sb.AppendLine("                        }");
            sb.AppendLine("                    }");
            sb.AppendLine("                    catch (Exception ex)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        clsSettings.LogError(ex);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return Records;");
            sb.AppendLine("        }\n");
            #endregion

            #region AddNew Method
            sb.AppendLine($"        public static async Task<int> AddNew({string.Join(", ", values.Skip(1).Select(c => $"{clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {"); 
            sb.AppendLine("            int ID = 0;\n");
            sb.AppendLine("            using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("            {");
            sb.AppendLine($"                string Query = \"INSERT INTO {Table} ({string.Join(", ", values.Skip(1).Select(c => $"{c.Name}"))}) VALUES ({string.Join(", ", values.Skip(1).Select(c => $"@{c.Name}"))});Select SCOPE_IDENTITY();\";\n");
            sb.AppendLine("                using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                {");
            for (int i = 1 ; i < values.Count; i++)
                sb.AppendLine($"                    Command.Parameters.AddWithValue(\"@{values[i].Name}\", {values[i].Name});\n");
            sb.AppendLine("                    try");
            sb.AppendLine("                    {");
            sb.AppendLine("                        await Connection.OpenAsync();\n");
            sb.AppendLine("                        object Result = await Command.ExecuteScalarAsync();\n");
            sb.AppendLine("                        if (Result != null && int.TryParse(Result.ToString(), out int NewID))");
            sb.AppendLine("                            ID = NewID;");
            sb.AppendLine("                    }");
            sb.AppendLine("                    catch (Exception ex)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        clsSettings.LogError(ex);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return ID;");
            sb.AppendLine("        }\n");
            #endregion

            #region Update Method
            sb.AppendLine($"        public static async Task<bool> Update({clsSystem.ConvertSqlToCSharp(values[0].DataType, values[0].IsNullable)} {values[0].Name}, {string.Join(", ", values.Skip(1).Select(c => $"{clsSystem.ConvertSqlToCSharp(c.DataType, c.IsNullable)} {c.Name}"))})");
            sb.AppendLine("        {");
            sb.AppendLine("            int result = 0;\n");
            sb.AppendLine("            using (SqlConnection Connection = new SqlConnection(clsSettings.GetConnectionString()))");
            sb.AppendLine("            {");
            sb.AppendLine($"                string Query = \"UPDATE {Table} set ({string.Join(", ", values.Skip(1).Select(c => $"{c.Name} = @{c.Name}"))} where {values[0].Name} = @ID ;\";\n");
            sb.AppendLine("                using (SqlCommand Command = new SqlCommand(Query, Connection))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    Command.Parameters.AddWithValue(\"@ID\", {values[0].Name});\n");
            for (int i = 1; i < values.Count; i++)
                sb.AppendLine($"                    Command.Parameters.AddWithValue(\"@{values[i].Name}\", {values[i].Name});\n");
            sb.AppendLine("                    try");
            sb.AppendLine("                    {");
            sb.AppendLine("                        await Connection.OpenAsync();\n");
            sb.AppendLine("                        result = await Command.ExecuteNonQueryAsync();");
            sb.AppendLine("                    }");
            sb.AppendLine("                    catch (Exception ex)");
            sb.AppendLine("                    {");
            sb.AppendLine("                        clsSettings.LogError(ex);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return result > 0;");
            sb.AppendLine("        }\n");
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
    }
}
