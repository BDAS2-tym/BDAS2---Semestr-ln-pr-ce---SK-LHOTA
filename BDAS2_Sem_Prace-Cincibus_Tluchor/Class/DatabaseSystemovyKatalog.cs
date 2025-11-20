using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída zajišťuje načítání systémového katalogu Oracle databáze
    /// Umožňuje získat seznam všech databázových objektů (tabulky, pohledy,
    /// funkce, triggery, sekvence, balíčky) vytvořených v rámci aplikace
    /// </summary>
    internal static class DatabaseSystemovyKatalog
    {
        /// <summary>
        /// Načte všechny objekty z Oracle systémového katalogu (ALL_OBJECTS),
        /// které patří aktuálnímu přihlášenému uživateli.
        /// </summary>
        public static List<SystemovyObjekt> GetSystemoveObjekty()
        {
            var seznamObjektu = new List<SystemovyObjekt>();

            string sql = @"
                SELECT 
                    OBJECT_NAME, 
                    OBJECT_TYPE, 
                    TO_CHAR(CREATED, 'DD.MM.YYYY HH24:MI') AS CREATED,
                    STATUS
                FROM ALL_OBJECTS
                WHERE OWNER = USER
                ORDER BY OBJECT_TYPE, OBJECT_NAME";

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    using (var cmd = new OracleCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var objekt = new SystemovyObjekt
                            {
                                NazevObjektu = reader["OBJECT_NAME"].ToString(),
                                TypObjektu = reader["OBJECT_TYPE"].ToString(),
                                DatumVytvoreni = reader["CREATED"].ToString(),
                                Stav = reader["STATUS"].ToString()
                            };

                            seznamObjektu.Add(objekt);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception($"Chyba při načítání systémového katalogu: {ex.Message}", ex);
            }

            return seznamObjektu;
        }
    }
}
