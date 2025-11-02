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
    /// Třída pro práci s tréninky v databázi
    /// Obsahuje metody pro přidání, úpravu a smazání tréninku
    /// Vše probíhá přes uložené procedury z balíčku PKG_TRENINKY v Oracle databázi
    /// </summary>
    internal class DatabaseTreninky
    {

        /// <summary>
        /// Přidá nový trénink do databáze pomocí uložené procedury PKG_TRENINKY.SP_ADD_TRENINK
        /// </summary>
        /// <param name="trenink">Objekt tréninku s daty k vložení</param>
        public static void AddTrenink(TreninkView trenink)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_ADD_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = trenink.Prijmeni;
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trenink.RodneCislo;
                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = trenink.Datum;
                cmd.Parameters.Add("p_misto", OracleDbType.Varchar2).Value = trenink.Misto;
                cmd.Parameters.Add("p_popis", OracleDbType.Varchar2).Value = trenink.Popis;
                    
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
        public static void UpdateTrenink(TreninkView trenink)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_UPDATE_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = trenink.Prijmeni;
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trenink.RodneCislo;
                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = trenink.Datum;
                cmd.Parameters.Add("p_misto", OracleDbType.Varchar2).Value = trenink.Misto;
                cmd.Parameters.Add("p_popis", OracleDbType.Varchar2).Value = trenink.Popis;

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
        public static void DeleteTrenink(TreninkView trenink)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // voláme proceduru z balíčku
            using (var cmd = new OracleCommand("PKG_TRENINKY.SP_DELETE_TRENINK", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametry procedury
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trenink.RodneCislo;

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