using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseManager
    {
        private static readonly string user = "st72870";
        private static readonly string password = "databaze";

        private static readonly string connectionString =
            $"User Id={user};Password={password};" +
            "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)))";

        private static OracleConnection? sharedConnection = null;
        private static readonly object connectionLock = new object();

        /// <summary>
        /// Metoda slouží k získaní společného připojení do databáze
        /// </summary>
        /// <returns>Vrací sdílené Oracle databázové připojení</returns>
        public static OracleConnection GetConnection()
        {
            lock (connectionLock)
            {
                if (sharedConnection == null)
                    sharedConnection = new OracleConnection(connectionString);

                if (sharedConnection.State != ConnectionState.Open)
                    sharedConnection.Open();

                return sharedConnection;
            }
        }

        /// <summary>
        /// Metoda slouží k zavření sdíleného připojení do databáze
        /// </summary>
        public static void CloseConnection()
        {
            lock (connectionLock)
            {
                if (sharedConnection != null)
                {
                    try
                    {
                        if (sharedConnection.State != ConnectionState.Closed)
                            sharedConnection.Close();

                        sharedConnection.Dispose();
                        sharedConnection = null;
                    }
                    catch { }
                }
            }
        }
    }
}
