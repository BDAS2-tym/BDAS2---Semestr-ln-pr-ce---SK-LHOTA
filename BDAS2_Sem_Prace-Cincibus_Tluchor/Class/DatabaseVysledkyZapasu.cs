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
        /// Metoda slouží k odebrání výsledku zápasu z databáze
        /// </summary>
        /// <param name="vysledekZapasu">Výsledek, který chceme odebrat z databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberVysledekZapasu(OracleConnection conn, VysledekZapasu vysledekZapasu)
        {
            using (var cmd = new OracleCommand("PKG_VYSLEDKY_ZAPASU.SP_ODEBER_VYSLEDEK_ZAPASU", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_zapas", OracleDbType.Int32).Value = vysledekZapasu.IdZapasu;             

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_VYSLEDEK_ZAPASU: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k editaci výsledku zápasu v databázi
        /// </summary>
        /// <param name="vysledekZapasu">Výsledek, který chceme editovat v databázi</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateVysledekZapasu(OracleConnection conn, VysledekZapasu vysledekZapasu)
        {
            using (var cmd = new OracleCommand("PKG_VYSLEDKY_ZAPASU.SP_UPDATE_VYSLEDEK_ZAPASU", conn))
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
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_VYSLEDEK_ZAPASU: {ex.Message}", ex);
                }
            }
        }
    }
}
