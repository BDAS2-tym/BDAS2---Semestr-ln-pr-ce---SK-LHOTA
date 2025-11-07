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
        /// Metoda slouží k přidání sponzora do databáze
        /// </summary>
        /// <param name="soutez">Soutěž, kterou chceme přidat do databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vytypSoutezeí, pokud nastane chyba při volání procedury</exception>
        public static void AddSoutez(OracleConnection conn, Soutez soutez)
        {
            using (var cmd = new OracleCommand("PKG_SOUTEZE.SP_ADD_SOUTEZ", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                TypSouteze typSouteze = new TypSouteze();
                int indexStavu = typSouteze.TypySoutezi.FirstOrDefault(st => st.Value == soutez.TypSouteze).Key;

                if (indexStavu != 0)
                {
                    cmd.Parameters.Add("v_id_typ_souteze", OracleDbType.Int32).Value = indexStavu;
                }

                cmd.Parameters.Add("v_datum_zacatek", OracleDbType.Date).Value = soutez.StartDatum.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.Add("v_datum_konec", OracleDbType.Date).Value = soutez.KonecDatum.ToDateTime(TimeOnly.MinValue);

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_SOUTEZ: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání soutěže z databáze
        /// </summary>
        /// <param name="soutez">Soutěž, kterou chceme odebrat z databáze</param>
        /// <exception cref="Exception">Výjimka se vytypSoutezeí, pokud nastane chyba při volání procedury</exception>
        public static void OdeberSoutez(Soutez soutez)
        {

        }

        /// <summary>
        /// Metoda slouží k editaci souteže v databázi
        /// </summary>
        /// <param name="soutez">Soutěž, kterou chceme editovat v databázi</param>
        /// <exception cref="Exception">Výjimka se vytypSoutezeí, pokud nastane chyba při volání procedury</exception>
        public static void UpdateSoutez(Soutez soutez)
        {

        }

        /// <summary>
        /// Metoda slouží k získání nového ID z databáze
        /// </summary>
        /// <returns>Nové ID</returns>
        /// <exception cref="Exception">Výjimka se vytypSoutezeí, pokud nastane chyba při volání procedury</exception>
        public static int? GetCurrentId(OracleConnection conn)
        {
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
