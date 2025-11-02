using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseSouteze
    {
        /// <summary>
        /// Metoda slouží k získání Oracle Connection do databáze
        /// </summary>
        /// <returns>Připojení do Oracle databáze</returns>
        private static OracleConnection GetConnection()
        {
            return DatabaseManager.GetConnection(); // využijeme metodu z DatabaseManager
        }

        /// <summary>
        /// Metoda slouží k přidání soutěže do databáze
        /// </summary>
        /// <param name="soutez">soutěž, kterou chceme přidat do databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddSoutez(Soutez soutez)
        {

        }

        /// <summary>
        /// Metoda slouží k odebrání soutěže z databáze
        /// </summary>
        /// <param name="soutez">Soutěž, kterou chceme odebrat z databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberSoutez(Soutez soutez)
        {

        }

        /// <summary>
        /// Metoda slouží k editaci souteže v databázi
        /// </summary>
        /// <param name="soutez">Soutěž, kterou chceme editovat v databázi</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateSoutez(Soutez soutez)
        {

        }

        /// <summary>
        /// Metoda slouží k získání nového ID z databáze
        /// </summary>
        /// <returns>Nové ID</returns>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static int? GetCurrentId()
        {
            using var conn = GetConnection();
            conn.Open();
            int? currentId = null;

            using (var cmd = new OracleCommand("SELECT sekv_soutez.currval FROM dual", conn))
            {
                cmd.CommandType = CommandType.Text;

                try
                {
                    currentId = Convert.ToInt32(cmd.ExecuteScalar());
                    return currentId;
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_SOUTEZ: {ex.Message}", ex);
                }
            }
        }

    }
}
