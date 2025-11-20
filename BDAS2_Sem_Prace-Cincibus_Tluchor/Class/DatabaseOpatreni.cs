using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s disciplinárními opatřeními (PKG_OPATRENI)
    /// </summary>
    internal static class DatabaseOpatreni
    {
        /// <summary>
        /// Přidá nové disciplinární opatření do databáze
        /// </summary>
        public static void AddOpatreni(DateTime datumOpatreni, int delkaTrestu, string? duvod)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("PKG_OPATRENI.SP_ADD_OPATRENI", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = datumOpatreni;
            cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = delkaTrestu;

            if (string.IsNullOrWhiteSpace(duvod))
                cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = DBNull.Value;
            else
                cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = duvod;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_ADD_OPATRENI: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Aktualizuje disciplinární opatření podle ID
        /// </summary>
        public static void UpdateOpatreni(int idOpatreni, DateTime datumOpatreni, int delkaTrestu, string? duvod)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("PKG_OPATRENI.SP_UPDATE_OPATRENI", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id_opatreni", OracleDbType.Int32).Value = idOpatreni;
            cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = datumOpatreni;
            cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = delkaTrestu;
            cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = duvod ?? (object)DBNull.Value;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_UPDATE_OPATRENI: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Smaže disciplinární opatření podle ID
        /// </summary>
        public static void DeleteOpatreni(int idOpatreni)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            using var cmd = new OracleCommand("PKG_OPATRENI.SP_DELETE_OPATRENI", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id_opatreni", OracleDbType.Int32).Value = idOpatreni;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_DELETE_OPATRENI: {ex.Message}", ex);
            }
        }
    }
}
