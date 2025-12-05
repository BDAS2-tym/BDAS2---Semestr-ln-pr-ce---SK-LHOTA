using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Media;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro správu uživatelů v databázi
    /// Komunikuje s uloženými procedurami v balíčku PKG_REGISTRACE
    /// Umožňuje přidávat, upravovat a mazat uživatele
    /// </summary>
    internal static class DatabaseRegistrace
    {
        /// <summary>
        /// Odstraní uživatele z databáze pomocí procedury PKG_REGISTRACE.SP_DELETE_UZIVATEL
        /// </summary>
        /// <param name="uzivatel">Objekt uživatele, který se má smazat</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void DeleteUzivatel(OracleConnection conn, Uzivatel uzivatel)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            // Voláme proceduru z balíčku PKG_REGISTRACE
            using var cmd = new OracleCommand("PKG_REGISTRACE.SP_DELETE_UZIVATEL", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Parametr – uživatelské jméno
            cmd.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při volání PKG_REGISTRACE.SP_DELETE_UZIVATEL: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Přidá nového uživatele do databáze pomocí uložené procedury PKG_REGISTRACE.SP_ADD_UZIVATEL
        /// </summary>
        /// <param name="uzivatel">Objekt s daty nového uživatele</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddUzivatel(OracleConnection conn, Uzivatel uzivatel)
        {
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();

            if (prihlaseny != null)
            {
                // Nastaví přihlášeného uživatele pro logování
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            int idRole;

            // Zjištění ID ROLE
            using (var cmdRole = new OracleCommand(
                "SELECT IDROLE FROM ROLE_VIEW WHERE UPPER(NAZEVROLE) = UPPER(:nazev)", conn))
            {
                cmdRole.Parameters.Add(":nazev", OracleDbType.Varchar2).Value = uzivatel.Role;

                object result = cmdRole.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception("Zvolená role neexistuje");
                }

                idRole = Convert.ToInt32(result);
            }

            // ID člena klubu (pouze pro hráče / trenéra)
            int? idClena = null;

            // Pokud má uživatel rodné číslo - jedná se o hráče/trenéra
            if (!string.IsNullOrEmpty(uzivatel.RodneCislo))
            {
                // Ověření, že existuje člen klubu s tímto rodným číslem a správným typem
                idClena = OverClena(conn, uzivatel.RodneCislo, uzivatel.Role);

                // Ověření, že tento člen ještě nemá vytvořený účet
                OverJestliNemaJinyUcet(conn, idClena.Value, "");
            }

            // Vložíme účet do tabulky přes uloženou proceduru
            using (var cmd = new OracleCommand("PKG_REGISTRACE.SP_ADD_UZIVATEL", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2, 30).Value = uzivatel.UzivatelskeJmeno;

                cmd.Parameters.Add("v_heslo", OracleDbType.Varchar2, 100).Value = uzivatel.Heslo;

                cmd.Parameters.Add("v_posledni_prihlaseni", OracleDbType.TimeStamp).Value = uzivatel.PosledniPrihlaseni;

                cmd.Parameters.Add("v_idrole", OracleDbType.Int32).Value = idRole;

                cmd.Parameters.Add("v_salt", OracleDbType.Varchar2, 64).Value = uzivatel.Salt;

                cmd.Parameters.Add("v_email", OracleDbType.Varchar2, 100).Value = uzivatel.Email;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 20003)
                    {
                        throw new Exception("Duplicitní uživatelské jméno nebo e-mail");
                    }

                    throw new Exception("Chyba při provádění SP_ADD_UZIVATEL: " + ex.Message);
                }
            }

            // Pokud je to hráč nebo trenér - doplníme vazbu
            if (idClena.HasValue)
            {
                using (var cmdUpdate = new OracleCommand("PKG_REGISTRACE.SP_NASTAV_CLENA", conn))
                {
                    cmdUpdate.CommandType = CommandType.StoredProcedure;

                    cmdUpdate.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
                    cmdUpdate.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = idClena.Value;

                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }

        /// <summary>
        /// Aktualizuje údaje uživatele v databázi
        /// Pokud není zadáno nové heslo, načte se původní
        /// Kontroluje platnost rodného čísla u hráče/trenéra
        /// a ověřuje, zda daný člen klubu nemá již jiný účet
        /// Po kontrole uloží změny přes uloženou proceduru
        /// a aktualizuje vazbu na člena klubu
        /// </summary>
        public static void UpdateUzivatel(OracleConnection conn, Uzivatel uzivatel, string stareJmeno)
        {
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();

            if (prihlaseny != null)
            {
                // Nastaví přihlášeného uživatele pro logování
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            // pokud nebylo zadáno nové heslo, načteme původní heslo
            if (string.IsNullOrEmpty(uzivatel.Heslo))
            {
                using var cmdOld = new OracleCommand(
                    "SELECT HESLO, SALT FROM PREHLED_UZIVATELSKE_UCTY WHERE UZIVATELSKEJMENO = :jmeno", conn);

                cmdOld.Parameters.Add(":jmeno", OracleDbType.Varchar2).Value = stareJmeno;

                using var reader = cmdOld.ExecuteReader();
                if (reader.Read())
                {
                    uzivatel.Heslo = reader.GetString(0);
                    uzivatel.Salt = reader.GetString(1);
                }
            }

            // zjistíme ID role podle názvu
            int idRole;
            using (var cmdRole = new OracleCommand(
                "SELECT IDROLE FROM ROLE_VIEW WHERE UPPER(NAZEVROLE) = UPPER(:nazev)", conn))
            {
                cmdRole.Parameters.Add(":nazev", OracleDbType.Varchar2).Value = uzivatel.Role;
                idRole = Convert.ToInt32(cmdRole.ExecuteScalar());
            }

            // zpracování rodného čísla jestli hráč nebo trenér jinak null pro další role
            int? idClena = null;

            if (!string.IsNullOrEmpty(uzivatel.RodneCislo))
            {
                // ověří typ hráč nebo trenér a existence člena v DB
                idClena = OverClena(conn, uzivatel.RodneCislo, uzivatel.Role);

                // ověří, že tento člen už nemá jiný účet
                OverJestliNemaJinyUcet(conn, idClena.Value, stareJmeno);
            }

            // volání procedury z balíčku
            using var cmd = new OracleCommand("PKG_REGISTRACE.SP_UPDATE_UZIVATEL", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("v_stare_jmeno", OracleDbType.Varchar2).Value = stareJmeno;
            cmd.Parameters.Add("v_nove_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = uzivatel.Email;
            cmd.Parameters.Add("v_heslo", OracleDbType.Varchar2).Value = uzivatel.Heslo;
            cmd.Parameters.Add("v_salt", OracleDbType.Varchar2).Value = uzivatel.Salt;
            cmd.Parameters.Add("v_idrole", OracleDbType.Int32).Value = idRole;

            cmd.ExecuteNonQuery();

            // Pokud je idClena vyplněné (hráč / trenér byl nalezen),
            // přiřadí se jeho ID do uživatelského účtu.
            // Pokud idClena není (např. role změněna na „admin“),
            // vazba se vymaže = účet už nebude propojen s žádným členem.
            if (idClena.HasValue)
            {
  
                using var cmdUpdate = new OracleCommand("PKG_REGISTRACE.SP_NASTAV_CLENA", conn);
                cmdUpdate.CommandType = CommandType.StoredProcedure;

                cmdUpdate.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
                cmdUpdate.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = idClena.Value;

                cmdUpdate.ExecuteNonQuery();

            }
            else
            {
                using var cmdClear = new OracleCommand("PKG_REGISTRACE.SP_NASTAV_CLENA", conn);
                cmdClear.CommandType = CommandType.StoredProcedure;

                cmdClear.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatel.UzivatelskeJmeno;
                cmdClear.Parameters.Add("v_id_clen", OracleDbType.Int32).Value = DBNull.Value;

                cmdClear.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Aktualizuje čas posledního přihlášení uživatele pomocí
        /// procedury PKG_REGISTRACE.SP_UPDATE_POSLEDNI_PRIHLASENI
        /// </summary>
        public static void UpdatePosledniPrihlaseni(OracleConnection conn, string uzivatelskeJmeno)
        {
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();

            if (prihlaseny != null)
            {
                // Nastaví přihlášeného uživatele pro logování
                DatabaseAppUser.SetAppUser(conn, prihlaseny);
            }

            using (var cmd = new OracleCommand("PKG_REGISTRACE.SP_UPDATE_POSLEDNI_PRIHLASENI", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_uzivatelske_jmeno", OracleDbType.Varchar2).Value = uzivatelskeJmeno;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception("Chyba při volání SP_UPDATE_POSLEDNI_PRIHLASENI: " + ex.Message, ex);
                }
            }
        }


        /// <summary>
        /// Najde člena klubu podle rodného čísla a typu (hráč / trenér)
        /// Metoda ověřuje, že rodné číslo existuje a že patří členovi
        /// správného typu. Pokud nic nenajde, vyhodí výjimku
        /// Vrací ID člena pro následné použití
        /// </summary>
        private static int OverClena(OracleConnection conn, string rodneCislo, string role)
        {
            string sql = @"
                        SELECT IDCLENKLUBU
                        FROM CLENOVE_KLUBU_VIEW
                        WHERE RODNE_CISLO = :rc
                        AND LOWER(TYPCLENA) = :typ";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":rc", OracleDbType.Varchar2).Value = rodneCislo;
            cmd.Parameters.Add(":typ", OracleDbType.Varchar2).Value = role.ToLower();

            object result = cmd.ExecuteScalar();

            if (result == null)
            {
                throw new Exception("Rodné číslo neodpovídá správnému typu člena nebo žádnému členovi klubu tohoto typu");
            }

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Ověří, zda daný člen klubu hráč/trenér už nemá jiný
        /// existující uživatelský účet. Používá se hlavně při editaci.
        /// Parametr stareJmeno slouží k tomu, aby se při kontrole
        /// ignoroval účet, který právě upravujeme. Pokud existuje jiný
        /// účet s tímto ID člena, metoda vyhodí výjimku.
        /// </summary>
        private static void OverJestliNemaJinyUcet(OracleConnection conn, int idClena, string stareJmeno)
        {
            string sql = @"
                        SELECT COUNT(*) 
                        FROM PREHLED_UZIVATELSKE_UCTY
                        WHERE IDCLENKLUBU = :id
                        AND UZIVATELSKEJMENO <> :starejmeno";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = idClena;
            cmd.Parameters.Add(":starejmeno", OracleDbType.Varchar2).Value = stareJmeno;

            int pocet = Convert.ToInt32(cmd.ExecuteScalar());

            if (pocet > 0)
            {
                throw new Exception("Tento hráč/trenér již má vytvořený jiný uživatelský účet");
            }
        }


        /// <summary>
        /// Ověří, zda v databázi existuje člen klubu podle rodného čísla a typu (hráč/trenér)
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
                FROM CLENOVE_KLUBU_VIEW
                WHERE RODNE_CISLO = :v_rodne_cislo 
                  AND LOWER(TYPCLENA) = :v_typ_clena";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = rodneCislo;
            cmd.Parameters.Add("v_typ_clena", OracleDbType.Varchar2).Value = typClena.ToLowerInvariant();

            int existuje = Convert.ToInt32(cmd.ExecuteScalar());
            return existuje > 0;
        }
    }
}
