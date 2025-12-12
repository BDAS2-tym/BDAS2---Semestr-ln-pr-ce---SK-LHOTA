using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s trenéri v databázi
    /// Obsahuje metody pro načtení počtu trenérů, přidání, úpravu a odstranění trenéra
    /// </summary>
    internal static class DatabaseTreneri
    {
        /// <summary>
        /// Vrátí počet trenérů v databázi
        /// </summary>
        /// <returns>Počet trenérů jako celé číslo</returns>
        public static int GetPocetTreneru()
        {
            OracleConnection conn = DatabaseManager.GetConnection();

            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM TRENERI_VIEW", conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Přidá nového trenéra pomocí uložené procedury <b>SP_ADD_TRENERI</b> z balíčku <b>PKG_TRENERI</b>.
        /// </summary>
        /// <param name="trener">Objekt trenéra s vyplněnými údaji (rodné číslo, jméno, licence, atd.)</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddTrener(OracleConnection conn, Trener trener)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_ADD_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trener.RodneCislo;
                cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = trener.Jmeno;
                cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = trener.Prijmeni;
                cmd.Parameters.Add("v_telefonni_cislo", OracleDbType.Varchar2).Value = trener.TelefonniCislo;
                cmd.Parameters.Add("v_trenerska_licence", OracleDbType.Varchar2).Value = trener.TrenerskaLicence;
                cmd.Parameters.Add("v_specializace", OracleDbType.Varchar2).Value = trener.Specializace;
                cmd.Parameters.Add("v_pocet_let_praxe", OracleDbType.Int32).Value = trener.PocetLetPraxe;

                try
                {
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
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateTrener(OracleConnection conn, Trener trener, string puvodniRodneCislo)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_UPDATE_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("v_rodne_cislo_puvodni", OracleDbType.Varchar2).Value = puvodniRodneCislo;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trener.RodneCislo;
                cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = trener.Jmeno;
                cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = trener.Prijmeni;
                cmd.Parameters.Add("v_telefonni_cislo", OracleDbType.Varchar2).Value = trener.TelefonniCislo;
                cmd.Parameters.Add("v_trenerska_licence", OracleDbType.Varchar2).Value = trener.TrenerskaLicence;
                cmd.Parameters.Add("v_specializace", OracleDbType.Varchar2).Value = trener.Specializace;
                cmd.Parameters.Add("v_pocet_let_praxe", OracleDbType.Int32).Value = trener.PocetLetPraxe;

                try
                {
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
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberTrenera(OracleConnection conn, Trener trener)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_TRENERI.SP_ODEBER_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trener.RodneCislo;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Zavolá PL/SQL funkci F_TOP3_TRENERI_BLOB a uloží výsledek do souboru.
        /// </summary>
        /// <param name="cestaSouboru">Cesta k výstupnímu souboru</param>
        public static void ExportTop3TreneriDoSouboru(string cestaSouboru, Uzivatel uzivatel)
        {
            OracleConnection conn = DatabaseManager.GetConnection();

            // Nastavení přihlášeného uživatele (logování)
            DatabaseAppUser.SetAppUser(conn, uzivatel);

            using (var cmd = new OracleCommand("PKG_TRENERI.F_TOP3_TRENERI_BLOB", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("return_value",
                    OracleDbType.Blob,
                    System.Data.ParameterDirection.ReturnValue);

                cmd.ExecuteNonQuery();

                OracleBlob blob = cmd.Parameters["return_value"].Value as OracleBlob;

                if (blob == null || blob.Length == 0)
                {
                    throw new Exception("Funkce F_TOP3_TRENERI_BLOB nevrátila žádná data!");
                }

                using (var fs = new FileStream(cestaSouboru, FileMode.Create, FileAccess.Write))
                {
                    blob.CopyTo(fs);
                }
            }
        }
    }
}
