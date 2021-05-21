using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealership.SQL.Common
{
    internal static class SqliteConnectionString
    {
        public static string ConnectionString { get; } = @"Data Source=C:\DB\DealerDB.db; Version=3;";
    }
}
