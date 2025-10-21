using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseHraci
    {
        private static OracleConnection GetConnection()
        {
            return DatabaseManager.GetConnection(); // využijeme metodu z DatabaseManager

        }

        public static int GetPocetHracu()
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("SELECT COUNT(*) FROM HRACI", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static void AddHrac(Hrac hrac)
        {

            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_ADD_HRAC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = hrac.RodneCislo;
                cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = hrac.Jmeno;
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = hrac.Prijmeni;
                cmd.Parameters.Add("p_telefonni_cislo", OracleDbType.Varchar2).Value = hrac.TelefonniCislo;
                cmd.Parameters.Add("p_pozice_na_hristi", OracleDbType.Varchar2).Value = hrac.PoziceNaHristi;
                cmd.Parameters.Add("p_pocet_golu", OracleDbType.Int32).Value = hrac.PocetVstrelenychGolu;
                cmd.Parameters.Add("p_pocet_zlute", OracleDbType.Int32).Value = hrac.PocetZlutychKaret;
                cmd.Parameters.Add("p_pocet_cervene", OracleDbType.Int32).Value = hrac.PocetCervenychKaret;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_HRAC: {ex.Message}", ex);
                }
            }

        }

        public static void UpdateHrac(Hrac hrac)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_UPDATE_HRAC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = hrac.RodneCislo;
                cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = hrac.Jmeno;
                cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = hrac.Prijmeni;
                cmd.Parameters.Add("p_telefonni_cislo", OracleDbType.Varchar2).Value = hrac.TelefonniCislo;
                cmd.Parameters.Add("p_pozice_na_hristi", OracleDbType.Varchar2).Value = hrac.PoziceNaHristi;
                cmd.Parameters.Add("p_pocet_golu", OracleDbType.Int32).Value = hrac.PocetVstrelenychGolu;
                cmd.Parameters.Add("p_pocet_zlute", OracleDbType.Int32).Value = hrac.PocetZlutychKaret;
                cmd.Parameters.Add("p_pocet_cervene", OracleDbType.Int32).Value = hrac.PocetCervenychKaret;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_HRAC: {ex.Message}", ex);
                }
            }
        }

        public static void OdeberHrace(Hrac hrac)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_ODEBER_HRACE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = hrac.RodneCislo;
                cmd.ExecuteNonQuery();
            }
        }

    }

}
