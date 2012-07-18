using System;
using System.Data;

namespace Common.Tools
{
    public class Misc
    {
        // Methods
        public static string ConvertShortNameToSystemDataType(string shortDataType)
        {
            string str = shortDataType;
            string str2 = shortDataType;
            if (str2 == null)
            {
                return str;
            }
            if (str2 != "int")
            {
                if (str2 != "string")
                {
                    if (str2 == "datetime")
                    {
                        return "System.DateTime";
                    }
                    if (str2 == "boolean")
                    {
                        return "System.Boolean";
                    }
                    if (str2 != "decimal")
                    {
                        return str;
                    }
                    return "System.Decimal";
                }
            }
            else
            {
                return "System.Int32";
            }
            return "System.String";
        }

        public static string ConvertToSystemDataType(SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                    return "System.Int64";

                case SqlDbType.Bit:
                    return "System.Boolean";

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return "System.DateTime";

                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Money:
                case SqlDbType.Real:
                case SqlDbType.SmallMoney:
                    return "System.Decimal";

                case SqlDbType.Int:
                    return "System.Int32";

                case SqlDbType.UniqueIdentifier:
                    return "System.Guid";

                case SqlDbType.SmallInt:
                    return "System.Int16";

                case SqlDbType.TinyInt:
                    return "System.Byte";
            }
            return "System.String";
        }

        public static string ConvertToSystemDataType(string sqlDataType)
        {
            SqlDbType @int;
            if (string.Compare(sqlDataType, "numeric", true) == 0)
            {
                @int = SqlDbType.Decimal;
            }
            else
            {
                @int = (SqlDbType)Enum.Parse(typeof(SqlDbType), sqlDataType, true);
            }
            return ConvertToSystemDataType(@int);
        }

        public static bool IsInteger(string number)
        {
            bool flag = true;
            try
            {
                int.Parse(number);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool IsNumeric(string number)
        {
            bool flag = true;
            try
            {
                double.Parse(number);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool IsDecimal(string number)
        {
            bool flag = true;
            try
            {
                decimal.Parse(number);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }


        public static bool IsSystemDataTypeNumeric(string systemDataType)
        {
            systemDataType = systemDataType.ToLower();
            switch (systemDataType)
            {
                case "system.int64":
                case "system.int32":
                case "system.int16":
                case "system.byte":
                case "int":
                case "long":
                case "byte":
                    return true;
            }
            return false;
        }
    }
}
