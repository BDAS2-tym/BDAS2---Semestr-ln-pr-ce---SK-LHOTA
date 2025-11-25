using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s binárním obsahem (PKG_BINARNI_OBSAH)
    /// </summary>
    internal static class DatabaseBinarniObsah
    {
        /// <summary>
        /// Přidá nový binární obsah pomocí uložené procedury PKG_BINARNI_OBSAH.SP_ADD_OBSAH
        /// </summary>
        public static void AddBinarniObsah(
            string nazevSouboru,
            string typSouboru,
            string priponaSouboru,
            byte[] obsah,
            string operace,
            int idUzivatelskyUcet)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // Nastavení přihlášeného uživatele pro logování do LOG_TABLE
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            if (prihlaseny != null)
            {
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            using var cmd = new OracleCommand("PKG_BINARNI_OBSAH.SP_ADD_OBSAH", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_nazev_souboru", OracleDbType.Varchar2).Value = nazevSouboru;
            cmd.Parameters.Add("v_typ_souboru", OracleDbType.Varchar2).Value = typSouboru;
            cmd.Parameters.Add("v_pripona_souboru", OracleDbType.Varchar2).Value = priponaSouboru;
            cmd.Parameters.Add("v_obsah", OracleDbType.Blob).Value = obsah;
            cmd.Parameters.Add("v_operace", OracleDbType.Varchar2).Value = operace;
            cmd.Parameters.Add("v_id_uzivatel", OracleDbType.Int32).Value = idUzivatelskyUcet;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_ADD_OBSAH: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Aktualizuje existující binární obsah podle ID pomocí procedury PKG_BINARNI_OBSAH.SP_UPDATE_OBSAH
        /// </summary>
        public static void UpdateBinarniObsah(int idObsah, byte[] obsah, string operace, int idUzivatelRole)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            //  Nastavení přihlášeného uživatele
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            if (prihlaseny != null)
            {
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            using var cmd = new OracleCommand("PKG_BINARNI_OBSAH.SP_UPDATE_OBSAH", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id_obsah", OracleDbType.Int32).Value = idObsah;
            cmd.Parameters.Add("v_obsah", OracleDbType.Blob).Value = obsah;
            cmd.Parameters.Add("v_operace", OracleDbType.Varchar2).Value = operace;
            cmd.Parameters.Add("v_id_uzivatel", OracleDbType.Int32).Value = idUzivatelRole;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_UPDATE_OBSAH: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Smaže binární obsah podle ID pomocí procedury PKG_BINARNI_OBSAH.SP_DELETE_OBSAH
        /// </summary>
        public static void DeleteBinarniObsah(int idObsah)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // Nastavení přihlášeného uživatele
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            if (prihlaseny != null)
            {
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            using var cmd = new OracleCommand("PKG_BINARNI_OBSAH.SP_DELETE_OBSAH", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("v_id_obsah", OracleDbType.Int32).Value = idObsah;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání procedury SP_DELETE_OBSAH: {ex.Message}", ex);
            }
        }


    }
}
