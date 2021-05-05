using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Dealership.SQL.Extensions
{
    internal static class DataReaderExtensions
    {
        internal static T ReadAs<T>(this IDataReader reader, string column)
        {
            var obj = reader[column];

            if (obj is DBNull)
                return default;

            if (obj is T value)
                return value;

            throw new InvalidCastException().FromMethod($"Не удалось преобразовать значение из {obj.GetType()} в {typeof(T)}",MethodBase.GetCurrentMethod(),"IDataReader",column);
        }
    }
}
