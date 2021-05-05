using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Dealership.SQL.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception FromMethod(this Exception innerException, string message, MethodBase method,
            params object[] values)
        {
            var parameterInfos = method.GetParameters();
            var nameValues = new object[2 * parameterInfos.Length];
            var msg =
                $"{message}{Environment.NewLine}\t{method.ReflectedType?.Name}.{method.Name}{Environment.NewLine}";

            for (int i = 0, j = 0; i < parameterInfos.Length; i++, j += 2)
            {
                msg += $"\t\t{{{j}}}: {{{j + 1}}} {Environment.NewLine}";
                nameValues[j] = parameterInfos[i].Name;

                if (i < values.Length)
                {
                    if (!(values[i] is string) && (values[i] is IEnumerable enumerable))
                        nameValues[j + 1] = string.Join(",", enumerable.OfType<object>());
                    else
                        nameValues[j + 1] = values[i].ToString();
                }
            }

            return new Exception(string.Format(msg, nameValues), innerException);
        }
    }
}
