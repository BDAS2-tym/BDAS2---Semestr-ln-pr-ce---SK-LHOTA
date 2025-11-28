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
    /// Třída pro práci s kontrakty v databázi
    /// Obsahuje metody pro přidání, úpravu a odstranění kontraktu
    /// </summary>
    internal static class DatabaseKontrakty
    {
        /// <summary>
        /// Metoda slouží k přidání kontraktu do databáze
        /// </summary>
        /// <param name="kontrakt">Kontrakt, který chceme přidat do databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddKontrakt(OracleConnection conn, Kontrakt kontrakt)
        {
            using (var cmd = new OracleCommand("PKG_KONTRAKTY.SP_ADD_KONTRAKT", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = kontrakt.KontraktHrace.IdClenKlubu;
                cmd.Parameters.Add("v_datum_zacatku", OracleDbType.Date).Value = kontrakt.DatumZacatku.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.Add("v_datum_konce", OracleDbType.Date).Value = kontrakt.DatumKonce.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.Add("v_plat", OracleDbType.Int32).Value = kontrakt.Plat;
                cmd.Parameters.Add("v_cislo_na_agenta", OracleDbType.Int64).Value = Convert.ToInt64(kontrakt.TelCisloNaAgenta);

                if (kontrakt.VystupniKlauzule == 0)
                {
                    cmd.Parameters.Add("v_vystupni_klauzule", OracleDbType.Int32).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("v_vystupni_klauzule", OracleDbType.Int32).Value = kontrakt.VystupniKlauzule;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_KONTRAKT: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání kontraktu z databáze
        /// </summary>
        /// <param name="kontrakt">Kontrakt, který chceme odebrat z databáze</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberKontrakt(OracleConnection conn, Kontrakt kontrakt)
        {
            using (var cmd = new OracleCommand("PKG_KONTRAKTY.SP_ODEBER_KONTRAKT", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = kontrakt.KontraktHrace.IdClenKlubu;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch(OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_KONTRAKT: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k editaci kontraktu v databázi
        /// </summary>
        /// <param name="kontrakt">Kontrakt, který chceme editovat v databázi</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="Exception">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void UpdateKontrakt(OracleConnection conn, Kontrakt kontrakt)
        {
            using (var cmd = new OracleCommand("PKG_KONTRAKTY.SP_UPDATE_KONTRAKT", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = kontrakt.KontraktHrace.IdClenKlubu;
                cmd.Parameters.Add("v_datum_zacatku", OracleDbType.Date).Value = kontrakt.DatumZacatku.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.Add("v_datum_konce", OracleDbType.Date).Value = kontrakt.DatumKonce.ToDateTime(TimeOnly.MinValue);
                cmd.Parameters.Add("v_plat", OracleDbType.Int32).Value = kontrakt.Plat;
                cmd.Parameters.Add("v_cislo_na_agenta", OracleDbType.Int64).Value = Convert.ToInt64(kontrakt.TelCisloNaAgenta);

                if (kontrakt.VystupniKlauzule == 0)
                {
                    cmd.Parameters.Add("v_vystupni_klauzule", OracleDbType.Int32).Value = DBNull.Value;
                }

                else
                {
                    cmd.Parameters.Add("v_vystupni_klauzule", OracleDbType.Int32).Value = kontrakt.VystupniKlauzule;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_UPDATE_KONTRAKT: {ex.Message}", ex);
                }

            }
        }
    }
}
