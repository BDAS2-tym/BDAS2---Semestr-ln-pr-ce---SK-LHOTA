using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal class DatabaseTreninky
    {
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