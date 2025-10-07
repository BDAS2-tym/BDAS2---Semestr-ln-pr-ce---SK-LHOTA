using System;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseManager
    {
        // Každý uživatel si jen změní své údaje ↓
        private static readonly string user = "stXXXXX";     // ← ZMĚŇ na své
        private static readonly string password = "XXXXXXXX";  // ← ZMĚŇ na své

        private static readonly string connectionString =
            $"User Id={user};Password={password};" +
            "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)))";

        public static OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }
    }
}
