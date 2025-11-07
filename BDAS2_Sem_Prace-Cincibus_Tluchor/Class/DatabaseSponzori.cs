using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseSponzori
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
        /// Metoda slouží k přidání sponzora do databáze
        /// </summary>
        /// <param name="sponzor">Sponzor, kterého chceme přidat do databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddSponzor(OracleConnection conn, Sponzor sponzor)
        {
            using (var cmd = new OracleCommand("PKG_SPONZORI.SP_ADD_SPONZOR", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = sponzor.Jmeno;
                if(sponzor.SponzorovanaCastka == 0)
                {
                    cmd.Parameters.Add("v_sponzorovana_castka", OracleDbType.Decimal).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("v_sponzorovana_castka", OracleDbType.Decimal).Value = sponzor.SponzorovanaCastka;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_SPONZOR: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání sponzora z databáze
        /// </summary>
        /// <param name="sponzor">Sponzor, kterého chceme přidat do databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberSponzor(Sponzor sponzor)
        {
            using var conn = GetConnection();
            conn.Open();

            // Nastavení App user pro zprovoznění logování změn
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_SPONZORI.SP_ODEBER_SPONZOR", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_SPONZOR: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k editaci sponzora v databázi
        /// </summary>
        /// <param name="sponzor">Sponzor, kterého chceme editovat v databázi</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateSponzor(Sponzor sponzor)
        {
            using var conn = GetConnection();
            conn.Open();

            // Nastavení App user pro zprovoznění logování změn
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_SPONZORI.SP_UPDATE_SPONZOR", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;
                cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = sponzor.Jmeno;
                cmd.Parameters.Add("v_sponzorovana_castka", OracleDbType.Varchar2).Value = sponzor.SponzorovanaCastka;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_SPONZOR: {ex.Message}", ex);
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

            using (var cmd = new OracleCommand("SELECT sekv_sponzor.currval FROM dual", conn))
            {
                cmd.CommandType = CommandType.Text;

                try
                {
                    currentId = Convert.ToInt32(cmd.ExecuteScalar());
                    return currentId;
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_SPONZOR: {ex.Message}", ex);
                }
            }
        }
    }
}
