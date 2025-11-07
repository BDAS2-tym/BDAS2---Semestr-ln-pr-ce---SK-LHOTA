using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public static class DatabaseVysledkyZapasu
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
        /// Metoda slouží k přidání výsledku zápasu do databáze
        /// </summary>
        /// <param name="vysledekZapasu">Výsledek, který chceme přidat do databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddVysledekZapasu(OracleConnection conn, VysledekZapasu vysledekZapasu)
        {
            using (var cmd = new OracleCommand("PKG_VYSLEDKY_ZAPASU.SP_ADD_VYSLEDEK_ZAPASU", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_zapas", OracleDbType.Int32).Value = vysledekZapasu.IdZapasu;
                cmd.Parameters.Add("v_vysledek", OracleDbType.Varchar2).Value = vysledekZapasu.Vysledek;
                cmd.Parameters.Add("v_pocet_zlutych_karet", OracleDbType.Int32).Value = vysledekZapasu.PocetZlutychKaret;
                cmd.Parameters.Add("v_pocet_cervenych_karet", OracleDbType.Int32).Value = vysledekZapasu.PocetCervenychKaret;
                cmd.Parameters.Add("v_pocet_goly_domaci", OracleDbType.Int32).Value = vysledekZapasu.PocetGolyDomaci;
                cmd.Parameters.Add("v_pocet_goly_hoste", OracleDbType.Int32).Value = vysledekZapasu.PocetGolyHoste;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_VYSLEDEK_ZAPASU: {ex.Message}", ex);
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
            //using var conn = GetConnection();
            //conn.Open();

            //// Nastavení App user pro zprovoznění logování změn
            //SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            //using (var cmd = new OracleCommand("PKG_SPONZORI.SP_ODEBER_SPONZOR", conn))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    // Naplníme všechny parametry procedury
            //    cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;

            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }

            //    catch (OracleException ex)
            //    {
            //        throw new Exception($"Chyba při volání procedury SP_ODEBER_SPONZOR: {ex.Message}", ex);
            //    }
            //}
        }

        /// <summary>
        /// Metoda slouží k editaci sponzora v databázi
        /// </summary>
        /// <param name="sponzor">Sponzor, kterého chceme editovat v databázi</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateSponzor(Sponzor sponzor)
        {
            //using var conn = GetConnection();
            //conn.Open();

            //// Nastavení App user pro zprovoznění logování změn
            //SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            //using (var cmd = new OracleCommand("PKG_SPONZORI.SP_UPDATE_SPONZOR", conn))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    // Naplníme všechny parametry procedury
            //    cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;
            //    cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = sponzor.Jmeno;
            //    cmd.Parameters.Add("v_sponzorovana_castka", OracleDbType.Varchar2).Value = sponzor.SponzorovanaCastka;

            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }

            //    catch (OracleException ex)
            //    {
            //        throw new Exception($"Chyba při volání procedury SP_UPDATE_SPONZOR: {ex.Message}", ex);
            //    }
            //}
        }
    }
}
