using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s hráči v databázi
    /// Obsahuje metody pro načtení počtu hráčů, přidání, úpravu a odstranění hráče
    /// </summary>
    internal static class DatabaseHraci
    {
        /// <summary>
        /// Vrátí počet hráčů v databázi
        /// </summary>
        /// <returns>Počet hráčů jako celé číslo</returns>
        public static int GetPocetHracu()
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("SELECT COUNT(*) FROM HRACI", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Přidá nového hráče do databáze pomocí uložené procedury SP_ADD_HRAC
        /// </summary>
        /// <param name="hrac">Objekt hráče s vyplněnými údaji</param>
        public static void AddHrac(Hrac hrac)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_ADD_HRAC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplnění všech parametrů uložené procedury
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

        /// <summary>
        /// Aktualizuje údaje hráče v databázi pomocí uložené procedury SP_UPDATE_HRAC
        /// </summary>
        /// <param name="hrac">Objekt hráče s novými údaji.</param>
        public static void UpdateHrac(Hrac hrac)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using (var cmd = new OracleCommand("SP_UPDATE_HRAC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplnění všech parametrů uložené procedury
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

        /// <summary>
        /// Odstraní hráče z databáze podle rodného čísla pomocí procedury SP_ODEBER_HRACE
        /// </summary>
        /// <param name="hrac">Objekt hráče, který se má odstranit.</param>
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
