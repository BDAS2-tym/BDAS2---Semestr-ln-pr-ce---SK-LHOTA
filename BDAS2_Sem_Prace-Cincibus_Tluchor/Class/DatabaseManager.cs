using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseManager
    {
        private static readonly string user = "st72870";
        private static readonly string password = "databaze";
        private static readonly string connectionString =
            $"User Id={user};Password={password};" +
            "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)))";

        public static OracleConnection GetConnection() => new OracleConnection(connectionString);

    }
}

