using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Core
{
    public static class TypeExtensions
    {
        public static string GetRealTypeName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            StringBuilder sb = new();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetRealTypeName(arg));
                appendComma = true;
            }
            sb.Append('>');
            return sb.ToString();
        }

        public static string GetIntegrationEventName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;
            return t.GetGenericArguments().First().Name;
        }
    }
}
