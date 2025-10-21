using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseTreneri
    {

        public static int GetPocetTreneru()
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("SELECT COUNT(*) FROM TRENERI", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static void AddTrener(Trener trener)
        {

            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_ADD_TRENERI", conn))
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
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_TRENERI: {ex.Message}", ex);
                }
            }

        }

        public static void UpdateTrener(Trener trener)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_UPDATE_TRENERI", conn))
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
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_TRENERI: {ex.Message}", ex);
                }
            }
        }

        public static void OdeberTrenera(Trener trener)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_ODEBER_TRENERI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = trener.RodneCislo;
                cmd.ExecuteNonQuery();
            }
        }



    }
}
