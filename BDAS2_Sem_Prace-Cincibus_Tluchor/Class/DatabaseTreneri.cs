using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s trenéry v databázi
    /// Volá uložené procedury v balíčku PKG_TRENERI (přidání, editace, odstranění trenéra)
    /// a poskytuje pomocné funkce jako zjištění počtu trenérů
    /// </summary>
    internal static class DatabaseTreneri
    {

        /// <summary>
        /// Vrátí počet trenérů v databázi
        /// </summary>
        /// <returns>Počet trenérů jako celé číslo</returns>
        public static int GetPocetTreneru()
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("SELECT COUNT(*) FROM TRENERI", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Přidá nového trenéra pomocí uložené procedury <b>SP_ADD_TRENERI</b> z balíčku <b>PKG_TRENERI</b>.
        /// </summary>
        /// <param name="trener">Objekt trenéra s vyplněnými údaji (rodné číslo, jméno, licence, atd.)</param>
        public static void AddTrener(Trener trener)
        {

            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_ADD_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trener.RodneCislo;
                cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = trener.Jmeno;
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = trener.Prijmeni;
                cmd.Parameters.Add("p_telefonni_cislo", OracleDbType.Varchar2).Value = trener.TelefonniCislo;
                cmd.Parameters.Add("p_trenerska_licence", OracleDbType.Varchar2).Value = trener.TrenerskaLicence;
                cmd.Parameters.Add("p_specializace", OracleDbType.Varchar2).Value = trener.Specializace;
                cmd.Parameters.Add("p_pocet_let_praxe", OracleDbType.Int32).Value = trener.PocetLetPraxe;

                try
                {
                    // Provede volání uložené procedury v databázi
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_TRENERI v balíčku PKG_TRENERI: {ex.Message}", ex);
                }
            }

        }

        /// <summary>
        /// Aktualizuje existujícího trenéra pomocí uložené procedury <b>SP_UPDATE_TRENERI</b>
        /// </summary>
        /// <param name="trener">Objekt trenéra s novými údaji (např. změna jména nebo praxe)</param>
        public static void UpdateTrener(Trener trener)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_UPDATE_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trener.RodneCislo;
                cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = trener.Jmeno;
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = trener.Prijmeni;
                cmd.Parameters.Add("p_telefonni_cislo", OracleDbType.Varchar2).Value = trener.TelefonniCislo;
                cmd.Parameters.Add("p_trenerska_licence", OracleDbType.Varchar2).Value = trener.TrenerskaLicence;
                cmd.Parameters.Add("p_specializace", OracleDbType.Varchar2).Value = trener.Specializace;
                cmd.Parameters.Add("p_pocet_let_praxe", OracleDbType.Int32).Value = trener.PocetLetPraxe;

                try
                {
                    // Provede volání uložené procedury v databázi
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_TRENERI v balíčku PKG_TRENERI: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Odstraní trenéra z databáze pomocí uložené procedury <b>SP_ODEBER_TRENERI</b>
        /// </summary>
        /// <param name="trener">Objekt trenéra, který má být odstraněn</param>
        public static void OdeberTrenera(Trener trener)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_ODEBER_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Decimal).Value = trener.RodneCislo;
                cmd.ExecuteNonQuery();
            }
        }

    }
}
