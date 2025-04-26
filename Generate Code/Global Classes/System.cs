using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Generate_Code.Global_Classes
{
    public static class clsSystem
    {
        public static void ErrorLog(Exception ex)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Error_Log.txt");
            string Message = $"[{DateTime.Now}] {ex.Message}\n{ex.StackTrace}\n\n";

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(Message);
            }
        }

        public static bool SaveLoginInformationInRegistry(string Server, string UserId, string Password)
        {
            string Path = "HKEY_CURRENT_USER\\Software\\Generate_Code";

            try
            {
                Parallel.Invoke(
                    () => Registry.SetValue(Path, "Server", Server),
                    () => Registry.SetValue(Path, "UserId", UserId),
                    () => Registry.SetValue(Path, "Password", Password)
                );

                return true;
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                return false;
            }
        }

        public static bool RestoreLoginInformationFromRegistry(ref string Server, ref string UserId, ref string Password)
        {
            string Path = "HKEY_CURRENT_USER\\Software\\Generate_Code";

            try
            {
                Server = Registry.GetValue(Path, "Server", null) as string;
                UserId = Registry.GetValue(Path, "UserId", null) as string;
                Password = Registry.GetValue(Path, "Password", null) as string;

                return Server != null && UserId != null && Password != null;
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                return false;

            }
        }

        public static string ConvertSqlToCSharp(string DataType , bool IsNullable)
        {
            string type = DataType.ToLower();

            switch (type)
            {
                case "bigint":
                    type = "long";
                    break;
                case "binary":
                case "varbinary":
                case "image":
                    type = "byte[]";
                    break;
                case "bit":
                    type = "bool";
                    break;
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    type = "string";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    type = "DateTime";
                    break;
                case "datetimeoffset":
                    type = "DateTimeOffset";
                    break;
                case "decimal":
                case "money":
                case "smallmoney":
                case "numeric":
                    type = "decimal";
                    break;
                case "float":
                    type = "double";
                    break;
                case "int":
                    type = "int";
                    break;
                case "real":
                    type = "float";
                    break;
                case "smallint":
                    type = "short";
                    break;
                case "time":
                    type = "TimeSpan";
                    break;
                case "timestamp":
                case "rowversion":
                    type = "byte[]";
                    break;
                case "tinyint":
                    type = "byte";
                    break;
                case "uniqueidentifier":
                    type = "Guid";
                    break;
                case "xml":
                    type = "string";
                    break;
                case "sql_variant":
                    type = "object";
                    break;
                default:
                    type = "object";
                    break;
            }


            return type + ((IsNullable && type != "string" && type != "byte[]" && type != "object") ? "?" : "");
        }

        public static string DefaultValue(string DataType , bool IsNullable)
        {
            if (IsNullable)
                return "null";

            switch (DataType)
            {
                case "int":
                case "bigint":
                case "smallint":
                case "tinyint":
                    return "0";
                case "bit":
                    return "false";
                case "decimal":
                case "numeric":
                case "float":
                case "real":
                    return "0";
                case "datetime":
                case "smalldatetime":
                case "date":
                case "datetime2":
                case "time":
                    return "DateTime.Now";
                case "char":
                case "varchar":
                case "text":
                case "nvarchar":
                case "ntext":
                case "uniqueidentifier":
                    return "\"\"";
                default:
                    return "null";
            }
        }

        public static void SaveToFile(string Path , string Word)
        {
            using (FileStream fs = new FileStream(Path, FileMode.CreateNew, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(Word);
            }
        }

        public static string ConvertValue(string dbValue, string sqlDataType)
        {
            string csharpType = ConvertSqlToCSharp(sqlDataType, false);

            switch (csharpType)
            {
                case "string": return $"Convert.ToString({dbValue})";
                case "int": return $"Convert.ToInt32({dbValue})";
                case "long": return $"Convert.ToInt64({dbValue})";
                case "decimal": return $"Convert.ToDecimal({dbValue})";
                case "bool": return $"Convert.ToBoolean({dbValue})";
                case "DateTime": return $"Convert.ToDateTime({dbValue})";
                case "double": return $"Convert.ToDouble({dbValue})";
                case "float": return $"Convert.ToSingle({dbValue})";
                case "byte[]": return $"{dbValue} as byte[]" ?? "(byte[]){dbValue}";
                case "TimeSpan": return $"TimeSpan.Parse({dbValue}.ToString())";
                case "short": return $"Convert.ToInt16({dbValue})";
                case "byte": return $"Convert.ToByte({dbValue})";
                case "DateTimeOffset": return $"DateTimeOffset.Parse({dbValue}.ToString())";
                default: return string.Empty;
            }

        }

    }
}
