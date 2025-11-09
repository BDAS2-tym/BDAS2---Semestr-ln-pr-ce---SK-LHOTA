using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseZapasy
    {
        /// <summary>
        /// Metoda slouží k získání Oracle Connection do databáze
        /// </summary>
        /// <returns>Připojení do Oracle databáze</returns>
        private static OracleConnection GetConnection()
        {
            return DatabaseManager.GetConnection(); // využijeme metodu z DatabaseManage
        }

        /// <summary>
        /// Metoda slouží k přidání zápasu do databáze
        /// </summary>
        /// <param name="zapas">Zápas, který chceme přidat do databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddZapas(OracleConnection conn, Zapas zapas)
        {
            using (var cmd = new OracleCommand("PKG_ZAPASY.SP_ADD_ZAPAS", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_datum", OracleDbType.Date).Value = zapas.Datum;
                cmd.Parameters.Add("v_id_soutez", OracleDbType.Int32).Value = zapas.Soutez.IdSoutez;

                StavZapasu stav = new StavZapasu();
                int indexStavu = stav.StavyZapasu.FirstOrDefault(st => st.Value == zapas.StavZapasu).Key;

                cmd.Parameters.Add("v_id_stav", OracleDbType.Int32).Value = indexStavu;
                cmd.Parameters.Add("v_domaci_tym", OracleDbType.Varchar2).Value = zapas.DomaciTym;
                cmd.Parameters.Add("v_hoste_tym", OracleDbType.Varchar2).Value = zapas.HosteTym;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_ZAPAS: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání zápasu z databáze
        /// </summary>
        /// <param name="zapas">Zápas, který chceme odebrat z databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberZapas(OracleConnection conn, Zapas zapas)
        {
            using (var cmd = new OracleCommand("PKG_ZAPASY.SP_ODEBER_ZAPAS", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_zapas", OracleDbType.Int32).Value = zapas.IdZapas;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_ZAPAS: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k editaci zápasu v databázi
        /// </summary>
        /// <param name="zapas">Zápas, který chceme editovat v databázi</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateZapas(OracleConnection conn, Zapas zapas)
        {
            using (var cmd = new OracleCommand("PKG_ZAPASY.SP_UPDATE_ZAPAS", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_zapas", OracleDbType.Int32).Value = zapas.IdZapas;
                cmd.Parameters.Add("v_datum", OracleDbType.Date).Value = zapas.Datum;
                cmd.Parameters.Add("v_id_soutez", OracleDbType.Int32).Value = zapas.Soutez.IdSoutez;

                StavZapasu stav = new StavZapasu();
                int indexStavu = stav.StavyZapasu.FirstOrDefault(st => st.Value == zapas.StavZapasu).Key;

                cmd.Parameters.Add("v_id_stav", OracleDbType.Int32).Value = indexStavu;
                cmd.Parameters.Add("v_domaci_tym", OracleDbType.Varchar2).Value = zapas.DomaciTym;
                cmd.Parameters.Add("v_hoste_tym", OracleDbType.Varchar2).Value = zapas.HosteTym;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_ZAPAS: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k získání nového ID z databáze
        /// </summary>
        /// <returns>Nové ID</returns>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static int? GetCurrentId(OracleConnection conn)
        {
            int? currentId = null;

            using (var cmd = new OracleCommand("SELECT sekv_zapas.currval FROM dual", conn))
            {
                cmd.CommandType = CommandType.Text;

                try
                {
                    currentId = Convert.ToInt32(cmd.ExecuteScalar());
                    return currentId;
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_ZAPAS: {ex.Message}", ex);
                }
            }
        }
    }
}
