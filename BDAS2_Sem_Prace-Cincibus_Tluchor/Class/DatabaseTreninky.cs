using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s tréninky v databázi
    /// Obsahuje metody pro načtení počtu přidání, úpravu a odstranění tréninku
    /// </summary>
    internal class DatabaseTreninky
    {
        /// <summary>
        /// Přidá nový trénink do databáze pomocí uložené procedury PKG_TRENINKY.SP_ADD_TRENINK
        /// </summary>
        /// <param name="trenink">Objekt tréninku s daty k vložení</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddTrenink(OracleConnection conn, TreninkView trenink)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_ADD_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = trenink.Prijmeni;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trenink.RodneCislo;
                cmd.Parameters.Add("v_datum", OracleDbType.Date).Value = trenink.Datum;
                cmd.Parameters.Add("v_misto", OracleDbType.Varchar2).Value = trenink.Misto;
                cmd.Parameters.Add("v_popis", OracleDbType.Varchar2).Value = trenink.Popis;
                    
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání PKG_TRENINKY.SP_ADD_TRENINK: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Aktualizuje existující trénink trenéra v databázi pomocí uložené procedury PKG_TRENINKY.SP_UPDATE_TRENINK
        /// </summary>
        /// <param name="trenink">Objekt tréninku s upravenými daty</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateTrenink(OracleConnection conn, TreninkView trenink)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_UPDATE_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = trenink.Prijmeni;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trenink.RodneCislo;
                cmd.Parameters.Add("v_datum", OracleDbType.Date).Value = trenink.Datum;
                cmd.Parameters.Add("v_misto", OracleDbType.Varchar2).Value = trenink.Misto;
                cmd.Parameters.Add("v_popis", OracleDbType.Varchar2).Value = trenink.Popis;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání PKG_TRENINKY.SP_UPDATE_TRENINK: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Odstraní trénink daného trenéra z databáze pomocí uložené procedury PKG_TRENINKY.SP_DELETE_TRENINK
        /// </summary>
        /// <param name="trenink">Objekt tréninku s rodným číslem trenéra, podle kterého se trénink maže</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void DeleteTrenink(OracleConnection conn, TreninkView trenink)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_DELETE_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = trenink.RodneCislo;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání PKG_TRENINKY.SP_DELETE_TRENINK: {ex.Message}", ex);
                }
            }
        }

    }
}