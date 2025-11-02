using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro správu uživatelů v databázi.
    /// Umožňuje přidávání, úpravu, mazání a ověřování uživatelů
    /// </summary>
    internal static class DatabaseRegistrace
    {
        /// <summary>
        /// Odstraní uživatele z databáze podle uživatelského jména
        /// </summary>
        /// <param name="uzivatel">Objekt uživatele, který má být odstraněn.</param>
        public static void DeleteUzivatel(Uzivatel uzivatel)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // SQL příkaz pro odstranění uživatele podle jména
            using var cmd = new OracleCommand("DELETE FROM UZIVATELSKE_UCTY WHERE UZIVATELSKEJMENO = :v_uzivatelske_jmeno", conn);
            cmd.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;

            try
            {
                cmd.ExecuteNonQuery(); // provede smazání
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při mazání uživatele: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Vloží nového uživatele do databáze pomocí uložené procedury v balíčku PKG_REGISTRACE.SP_ADD_UZIVATEL.
        /// </summary>
        /// <param name="uzivatel">Objekt s daty nového uživatele</param>
        public static void AddUzivatel(Uzivatel uzivatel)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // Nejprve zjistíme ID role uživatele podle názvu role
            int idRole;
            using (var roleCmd = new OracleCommand("SELECT IDROLE FROM ROLE WHERE LOWER(REPLACE(NAZEVROLE, 'Á', 'A')) = :nazev", conn))
            {
                // Normalizace názvu role (odstranění diakritiky)
                string normalizedRole = uzivatel.Role.ToLowerInvariant()
                    .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                    .Replace("ó", "o").Replace("ú", "u").Replace("ý", "y")
                    .Replace("č", "c").Replace("ř", "r").Replace("š", "s").Replace("ž", "z");

                roleCmd.Parameters.Add("nazev", OracleDbType.Varchar2).Value = normalizedRole;

                object result = roleCmd.ExecuteScalar(); // zjištění ID role

                if (result == null)
                    throw new Exception($"Role '{uzivatel.Role}' neexistuje v tabulce ROLE.");

                idRole = Convert.ToInt32(result);
            }

            // Volání uložené procedury pro vložení uživatele
            using var cmd = new OracleCommand("PKG_REGISTRACE.SP_ADD_UZIVATEL", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Předání všech potřebných parametrů proceduře
            cmd.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2, 30).Value = uzivatel.UzivatelskeJmeno;
            cmd.Parameters.Add("v_heslo", OracleDbType.Varchar2, 100).Value = uzivatel.Heslo;
            cmd.Parameters.Add("v_posledni_prihlaseni", OracleDbType.TimeStamp).Value = uzivatel.PosledniPrihlaseni;
            cmd.Parameters.Add("v_idrole", OracleDbType.Int32).Value = idRole;
            cmd.Parameters.Add("v_salt", OracleDbType.Varchar2, 64).Value = uzivatel.Salt;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2, 100).Value = uzivatel.Email;

            try
            {
                cmd.ExecuteNonQuery(); // vloží uživatele

                // Pokud má uživatel rodné číslo (je člen klubu), najdeme jeho ID a vytvoříme vazbu
                if (!string.IsNullOrEmpty(uzivatel.RodneCislo))
                {
                    using var cmdId = new OracleCommand("SELECT IDCLENKLUBU FROM CLENOVE_KLUBU WHERE RODNE_CISLO = :rodnecislo", conn);
                    cmdId.Parameters.Add(":rodnecislo", OracleDbType.Varchar2).Value = uzivatel.RodneCislo;

                    object idClena = cmdId.ExecuteScalar();

                    // Pokud člen existuje, aktualizujeme vazbu mezi uživatelem a členem klubu
                    if (idClena != null)
                    {
                        using var cmdUpdate = new OracleCommand(@"
                            UPDATE UZIVATELSKE_UCTY 
                            SET CLEN_KLUBU_IDCLENKLUBU = :id 
                            WHERE UZIVATELSKEJMENO = :jmeno", conn);

                        cmdUpdate.Parameters.Add(":id", OracleDbType.Int32).Value = Convert.ToInt32(idClena);
                        cmdUpdate.Parameters.Add(":jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                // Ošetření specifické chyby – duplicitní jméno nebo email
                if (ex.Number == 20003)
                    throw new Exception("Duplicitní uživatelské jméno nebo e-mail!");
                else
                    throw new Exception($"Chyba při volání procedury PKG_REGISTRACE.SP_ADD_UZIVATEL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Aktualizuje údaje uživatele (včetně možnosti změnit jméno a heslo)
        /// </summary>
        /// <param name="uzivatel">Nová data uživatele</param>
        /// <param name="stareJmeno">Původní uživatelské jméno pro identifikaci záznamu</param>
        public static void UpdateUzivatel(Uzivatel uzivatel, string stareJmeno)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            // Pokud nebylo zadáno nové heslo, načteme staré
            if (string.IsNullOrEmpty(uzivatel.Heslo))
            {
                using var cmdOld = new OracleCommand(
                    "SELECT HESLO, SALT FROM UZIVATELSKE_UCTY WHERE UZIVATELSKEJMENO = :jmeno", conn);
                cmdOld.Parameters.Add(":jmeno", OracleDbType.Varchar2).Value = stareJmeno;

                using var reader = cmdOld.ExecuteReader();
                if (reader.Read())
                {
                    uzivatel.Heslo = reader.GetString(0);
                    uzivatel.Salt = reader.GetString(1);
                }
            }

            // Aktualizační dotaz – změní údaje uživatele podle původního jména
            using var cmd = new OracleCommand(@"
                UPDATE UZIVATELSKE_UCTY
                SET UZIVATELSKEJMENO = :noveJmeno,
                    EMAIL = :email,
                    HESLO = :heslo,
                    SALT = :salt
                WHERE UZIVATELSKEJMENO = :stareJmeno", conn);

            cmd.Parameters.Add(":noveJmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
            cmd.Parameters.Add(":email", OracleDbType.Varchar2).Value = uzivatel.Email;
            cmd.Parameters.Add(":heslo", OracleDbType.Varchar2).Value = uzivatel.Heslo;
            cmd.Parameters.Add(":salt", OracleDbType.Varchar2).Value = uzivatel.Salt;
            cmd.Parameters.Add(":stareJmeno", OracleDbType.Varchar2).Value = stareJmeno;

            int rows = cmd.ExecuteNonQuery();

            // Pokud se neaktualizoval žádný záznam, uživatel nebyl nalezen
            if (rows == 0)
                throw new Exception("Uživatel nebyl nalezen – žádný záznam se neaktualizoval.");
        }

        /// <summary>
        /// Ověří, zda v databázi existuje člen klubu s daným rodným číslem a typem (hráč/trenér)
        /// </summary>
        /// <param name="rodneCislo">Rodné číslo člena klubu</param>
        /// <param name="typClena">Typ člena ("hrac" nebo "trener")</param>
        /// <returns>True, pokud člen existuje, jinak False</returns>
        public static bool OverClenaPodleRodnehoCisla(string rodneCislo, string typClena)
        {
            using var conn = DatabaseManager.GetConnection();
            conn.Open();

            const string sql = @"
                SELECT COUNT(*) 
                FROM CLENOVE_KLUBU 
                WHERE RODNE_CISLO = :v_rodne_cislo 
                  AND LOWER(TYPCLENA) = :v_typ_clena";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = rodneCislo;
            cmd.Parameters.Add("v_typ_clena", OracleDbType.Varchar2).Value = typClena.ToLowerInvariant();

            int existuje = Convert.ToInt32(cmd.ExecuteScalar());
            return existuje > 0; // Vrátí true, pokud člen existuje
        }
    }
}
