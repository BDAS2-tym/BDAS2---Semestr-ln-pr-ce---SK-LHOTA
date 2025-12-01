using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída poskytující databázové operace nad entitou Hráč
    /// Obsahuje CRUD metody pro vložení, úpravu, smazání a pomocné operace
    /// </summary>
    internal static class DatabaseHraci
    {
        /// <summary>
        /// Vrací celkový počet hráčů v tabulce HRACI
        /// </summary>
        /// <returns>Počet hráčů uložených v databázi</returns>
        public static int GetPocetHracu()
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                using (var cmd = new OracleCommand("SELECT COUNT(*) FROM HRACI", conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Přidá nového hráče pomocí uložené procedury <c>PKG_HRACI.SP_ADD_HRAC</c>
        /// Zároveň nastaví přihlášeného uživatele (pro logování triggerem)
        /// </summary>
        /// <param name="conn">Otevřené Oracle připojení</param>
        /// <param name="hrac">Objekt hráče, který se má vložit</param>
        /// <exception cref="Exception">Vyvoláno, pokud Oracle vyhodí chybu</exception>
        public static void AddHrac(OracleConnection conn, Hrac hrac)
        {
            try
            {
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                using (var cmd = new OracleCommand("PKG_HRACI.SP_ADD_HRAC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = hrac.RodneCislo;
                    cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = hrac.Jmeno;
                    cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = hrac.Prijmeni;
                    cmd.Parameters.Add("v_telefonni_cislo", OracleDbType.Varchar2).Value = hrac.TelefonniCislo;
                    cmd.Parameters.Add("v_id_pozice", OracleDbType.Int32).Value = hrac.IdPozice;
                    cmd.Parameters.Add("v_pocet_golu", OracleDbType.Int32).Value = hrac.PocetVstrelenychGolu;
                    cmd.Parameters.Add("v_pocet_zlute", OracleDbType.Int32).Value = hrac.PocetZlutychKaret;
                    cmd.Parameters.Add("v_pocet_cervene", OracleDbType.Int32).Value = hrac.PocetCervenychKaret;

                    // Pokud hráč nemá Disciplinární opatření
                    if (hrac.DatumOpatreni == DateTime.MinValue)
                    {
                        cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = hrac.DatumOpatreni;
                    }

                    if (hrac.DelkaTrestu == 0)
                    {
                        cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = hrac.DelkaTrestu;
                    }

                    if (string.IsNullOrEmpty(hrac.DuvodOpatreni))
                    {
                        cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = hrac.DuvodOpatreni;
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Chyba při volání procedury SP_ADD_HRAC: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Aktualizuje existujícího hráče pomocí procedury <c>PKG_HRACI.SP_UPDATE_HRAC</c>
        /// </summary>
        /// <param name="conn">Otevřené Oracle připojení</param>
        /// <param name="hrac">Objekt hráče s upravenými hodnotami</param>
        /// <exception cref="Exception">Vyvoláno, pokud Oracle hlásí chybu</exception>
        public static void UpdateHrac(OracleConnection conn, Hrac hrac, string puvodniRodneCislo)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_HRACI.SP_UPDATE_HRAC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("v_rodne_cislo_puvodni", OracleDbType.Varchar2).Value = puvodniRodneCislo;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = hrac.RodneCislo;

                cmd.Parameters.Add("v_jmeno", OracleDbType.Varchar2).Value = hrac.Jmeno;
                cmd.Parameters.Add("v_prijmeni", OracleDbType.Varchar2).Value = hrac.Prijmeni;
                cmd.Parameters.Add("v_telefonni_cislo", OracleDbType.Varchar2).Value = hrac.TelefonniCislo;
                cmd.Parameters.Add("v_id_pozice", OracleDbType.Int32).Value = hrac.IdPozice;
                cmd.Parameters.Add("v_pocet_golu", OracleDbType.Int32).Value = hrac.PocetVstrelenychGolu;
                cmd.Parameters.Add("v_pocet_zlute", OracleDbType.Int32).Value = hrac.PocetZlutychKaret;
                cmd.Parameters.Add("v_pocet_cervene", OracleDbType.Int32).Value = hrac.PocetCervenychKaret;

                // Disciplinární opatření
                if (hrac.DatumOpatreni == DateTime.MinValue)
                {
                    cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("v_datum_opatreni", OracleDbType.Date).Value = hrac.DatumOpatreni;
                }

                if (hrac.DelkaTrestu == 0)
                {
                    cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("v_delka_trestu", OracleDbType.Int32).Value = hrac.DelkaTrestu;
                }

                if (string.IsNullOrEmpty(hrac.DuvodOpatreni))
                {
                    cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("v_duvod", OracleDbType.Varchar2).Value = hrac.DuvodOpatreni;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw new Exception("Chyba při volání procedury SP_UPDATE_HRAC: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Odebere hráče z databáze pomocí procedury <c>PKG_HRACI.SP_ODEBER_HRACE</c>
        /// </summary>
        /// <param name="conn">Otevřené Oracle připojení</param>
        /// <param name="hrac">Objekt hráče, který má být odstraněn (dle rodného čísla)</param>
        public static void OdeberHrace(OracleConnection conn, Hrac hrac)
        {
            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

            using (var cmd = new OracleCommand("PKG_HRACI.SP_ODEBER_HRACE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_rodne_cislo", OracleDbType.Varchar2).Value = hrac.RodneCislo;

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Načte TOP 3 střelce z databázového pohledu TOP_3_NEJLEPSI_STRELCI_VIEW
        /// </summary>
        /// <returns>Seznam nejlepších střelců.</returns>
        public static List<TopStrelec> GetTopStrelci()
        {
            var seznamStrelcu = new List<TopStrelec>();

            using (var conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string sql = "SELECT JMENO, PRIJMENI, POCETVSTRELENYCHGOLU, NAZEV_POZICE, PORADI FROM TOP_3_NEJLEPSI_STRELCI_VIEW";

                using (var cmd = new OracleCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seznamStrelcu.Add(new TopStrelec(
                            reader["JMENO"].ToString(),
                            reader["PRIJMENI"].ToString(),
                            Convert.ToInt32(reader["POCETVSTRELENYCHGOLU"]),
                            reader["NAZEV_POZICE"].ToString(),
                            Convert.ToInt32(reader["PORADI"])
                        ));
                    }
                }
            }

            return seznamStrelcu;
        }
    }
}

