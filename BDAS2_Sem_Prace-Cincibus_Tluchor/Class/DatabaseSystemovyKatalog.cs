using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída slouží k rychlému načítání objektů systémového katalogu Oracle
    /// Obsahuje optimalizace – minimální počet SQL dotazů.
    /// </summary>
    internal static class DatabaseSystemovyKatalog
    {

        /// <summary>
        /// Načte kompletní systémový katalog uživatele (tabulky, procedury, package, indexy…)
        /// </summary>
        public static List<SystemovyObjekt> GetSystemoveObjekty()
        {
            // Kolekce, do které se budou postupně ukládat všechny načtené objekty
            List<SystemovyObjekt> vysledek = new List<SystemovyObjekt>();

            using (OracleConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                // Dotazem načteme CREATED všech objektů
                var mapaCreated = LoadCreatedMap(conn);

                // Základní objekty z USER_OBJECTS
                vysledek.AddRange(LoadZakladniObjekty(conn, mapaCreated));

                // Procedury a funkce uvnitř PACKAGE
                vysledek.AddRange(LoadObjektyZPackage(conn, mapaCreated));

                // Indexy
                vysledek.AddRange(LoadIndexy(conn, mapaCreated));

                // Constrainty 
                vysledek.AddRange(LoadConstrainty(conn));

                // Sekvence
                vysledek.AddRange(LoadSekvence(conn, mapaCreated));
            }

            return vysledek;
        }

        //  CREATED MAP – Načtení CREATED pro všechny objekty uživatele
        /// <summary>
        /// Načte název objektu, typ objektu a datum vytvoření uloží je do Dictionary
        /// </summary>
        private static Dictionary<(string, string), string> LoadCreatedMap(OracleConnection conn)
        {
            Dictionary<(string, string), string> mapa = new Dictionary<(string, string), string>();

            string sql = @"
                SELECT OBJECT_NAME, OBJECT_TYPE,
                TO_CHAR(CREATED,'DD.MM.YYYY HH24:MI') AS CREATED
                FROM USER_OBJECTS
            ";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string nazev = r["OBJECT_NAME"].ToString();
                    string typ = r["OBJECT_TYPE"].ToString();
                    string created = r["CREATED"].ToString();

                    mapa[(nazev, typ)] = created;
                }
            }

            return mapa;
        }

        // Tabulky / View / Triggery / Package
        /// <summary>
        /// Vrátí všechny základní objekty z USER_OBJECTS (bez procedur/funkcí uvnitř balíčku)
        /// </summary>
        private static List<SystemovyObjekt> LoadZakladniObjekty(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            // SQL dotaz načítá název objektu, jeho typ a stav z katalogu USER_OBJECTS
            string sql = @"
                SELECT OBJECT_NAME, OBJECT_TYPE, STATUS
                FROM USER_OBJECTS
                ORDER BY OBJECT_TYPE, OBJECT_NAME
            ";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    SystemovyObjekt o = new SystemovyObjekt();

                    // Naplnění názvu, typu a stavu podle dat z databáze
                    o.NazevObjektu = r["OBJECT_NAME"].ToString();
                    o.TypObjektu = r["OBJECT_TYPE"].ToString();
                    o.Stav = r["STATUS"].ToString(); // VALID / INVALID

                    // Tento objekt není uvnitř balíčku
                    o.ParentPackage = "";         
                    o.PocetRadku = -1;
                    o.PocetRadkuKodu = -1;
                    o.ZdrojovyKod = null;

                    // Najdeme datum vytvoření
                    if (!createdMap.ContainsKey((o.NazevObjektu, o.TypObjektu)))
                        o.DatumVytvoreni = "";
                    else
                        o.DatumVytvoreni = createdMap[(o.NazevObjektu, o.TypObjektu)];

                    list.Add(o);
                }
            }

            return list;
        }

        // Procedury + Funkce uvnitř balíčku (PACKAGE)

        /// <summary>
        /// Najde v definici všech balíčků procedury a funkce a vrátí je jako samostatné objekty
        /// </summary>
        private static List<SystemovyObjekt> LoadObjektyZPackage(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
          
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            // Dotaz na USER_SOURCE vrací všech PACKAGE 
            // ORDER BY NAME, LINE:
            //   - seřadí nejdřív balíčky podle jména (NAME)
            //   - uvnitř každého balíčku seřadí řádky podle LINE = správné pořadí kódu
            string sql = @"
            SELECT NAME, TEXT
            FROM USER_SOURCE
            WHERE TYPE = 'PACKAGE'
            ORDER BY NAME, LINE
            ";

            Dictionary<string, StringBuilder> textyBalicku = new Dictionary<string, StringBuilder>();

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string pkg = r["NAME"].ToString();   // název balíčku
                    string text = r["TEXT"].ToString();  // řádek textu z USER_SOURCE

                    if (!textyBalicku.ContainsKey(pkg))
                    {
                        textyBalicku[pkg] = new StringBuilder();
                    }

                    // Přidáme řádek textu do celkové definice balíčku
                    textyBalicku[pkg].Append(text);
                }
            }

            foreach (var p in textyBalicku)
            {
                string balicek = p.Key;            // název balíčku
                string obsah = p.Value.ToString(); // kompletní text balíčku

                // Datum vytvoření balíčku 
                string created = "";
                if (createdMap.ContainsKey((balicek, "PACKAGE")))
                {
                    created = createdMap[(balicek, "PACKAGE")];
                }

                // Rozdělení textu na jednotlivé řádky
                string[] radky = obsah.Split('\n');

                foreach (string r in radky)
                {
                    string radek = r.Trim().ToUpper();

                    //  Nalezení Procedur v balíčku
                    if (radek.StartsWith("PROCEDURE "))
                    {
                        // Název procedury = druhé slovo na řádku
                        string nazev = r.Trim().Split(' ')[1].Replace(";", "");

                        list.Add(new SystemovyObjekt
                        {
                            NazevObjektu = nazev,
                            TypObjektu = "PROCEDURE",
                            ParentPackage = balicek,   
                            DatumVytvoreni = created,
                            Stav = "VALID"
                        });
                    }

                    //  Hledání funkcí v balíčku
                    if (radek.StartsWith("FUNCTION "))
                    {
                        // Název funkce = druhé slovo na řádku
                        string nazev = r.Trim().Split(' ')[1].Replace(";", "");

                        list.Add(new SystemovyObjekt
                        {
                            NazevObjektu = nazev,
                            TypObjektu = "FUNCTION",
                            ParentPackage = balicek,     
                            DatumVytvoreni = created,
                            Stav = "VALID"
                        });
                    }
                }
            }

            // Vrátíme všechny nalezené procedury a funkce uvnitř balíčků
            return list;
        }

        /// <summary>
        /// Načte všechny indexy uživatele z USER_INDEXES a vrátí je jako SystemovyObjekt
        /// Oracle vede index jako databázový objekt
        /// </summary>
        private static List<SystemovyObjekt> LoadIndexy(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            string sql = @"
                SELECT INDEX_NAME, TABLE_NAME, UNIQUENESS
                FROM USER_INDEXES
            ";

            // Provedení SQL dotazu
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string nazev = r["INDEX_NAME"].ToString();

                    string created = "";
                    if (createdMap.ContainsKey((nazev, "INDEX")))
                        created = createdMap[(nazev, "INDEX")];

                    SystemovyObjekt o = new SystemovyObjekt();
                    o.NazevObjektu = nazev;
                    o.TypObjektu = "INDEX";
                    o.Stav = r["UNIQUENESS"].ToString();
                    o.DatumVytvoreni = created;
                    o.ZdrojovyKod = ""; // Index nemá přímo SQL zdrojový kód

                    list.Add(o);
                }
            }

            return list;
        }

        /// <summary>
        /// Načte všechny databázové constrainty uživatele z USER_CONSTRAINTS
        /// Constrainty jsou omezení tabulek jako PRIMARY KEY, FOREIGN KEY, CHECK, UNIQUE
        /// </summary>
        private static List<SystemovyObjekt> LoadConstrainty(OracleConnection conn)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            string sql = @"
                SELECT CONSTRAINT_NAME, CONSTRAINT_TYPE, TABLE_NAME
                FROM USER_CONSTRAINTS
            ";

            // Provedení SQL dotazu
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    SystemovyObjekt o = new SystemovyObjekt();
                    o.NazevObjektu = r["CONSTRAINT_NAME"].ToString();
                    o.TypObjektu = "CONSTRAINT " + r["CONSTRAINT_TYPE"].ToString();
                    o.Stav = "VALID";
                    o.DatumVytvoreni = "";
                    o.ZdrojovyKod = "";

                    list.Add(o);
                }
            }

            return list;
        }

        // SEQUENCE
        /// <summary>
        /// Vrátí všechny sekvence uživatele 
        /// </summary>
        private static List<SystemovyObjekt> LoadSekvence(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            // USER_SEQUENCES obsahuje seznam všech sekvencí vytvořených uživatelem
            string sql = "SELECT SEQUENCE_NAME FROM USER_SEQUENCES";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string name = r["SEQUENCE_NAME"].ToString();

                    string created = "";
                    if (createdMap.ContainsKey((name, "SEQUENCE")))
                        created = createdMap[(name, "SEQUENCE")];

                    SystemovyObjekt o = new SystemovyObjekt();
                    o.NazevObjektu = name;
                    o.TypObjektu = "SEQUENCE";
                    o.DatumVytvoreni = created;
                    o.Stav = "VALID";
                    o.ZdrojovyKod = "-- Detail sekvence se načte v detailu --";

                    list.Add(o);
                }
            }

            return list;
        }

        // NAČÍTÁNÍ ZDROJOVÉHO KÓDU
        /// <summary>
        /// Načte SQL zdrojový kód procedury, funkce, triggeru, package nebo balíčku
        /// </summary>
        public static void LoadSqlSource(OracleConnection conn, SystemovyObjekt obj)
        {
            // Objekt je uvnitř balíčku - musíme hledat v PACKAGE BODY
            if (obj.ParentPackage != "")
            {
                string sql = @"
                    SELECT TEXT
                    FROM USER_SOURCE
                    WHERE NAME = :pkg AND TYPE='PACKAGE BODY'
                    ORDER BY LINE
                ";

                // Složíme všechny řádky textu balíčku
                StringBuilder sb = new StringBuilder();

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("pkg", obj.ParentPackage);

                    using (OracleDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            sb.Append(r["TEXT"].ToString());
                        }
                    }
                }

                // Kompletní text balíčku
                string text = sb.ToString();
                string upper = text.ToUpper();

                string hledat = obj.TypObjektu + " " + obj.NazevObjektu.ToUpper();

                // Najdeme začátek definice procedury/funkce
                int start = upper.IndexOf(hledat);

                // Pokud se nenašlo - vracíme chybu
                if (start == -1)
                {
                    obj.ZdrojovyKod = "-- Kód nebyl nalezen --";
                    obj.PocetRadkuKodu = 0;
                    return;
                }

                int end = upper.IndexOf("END " + obj.NazevObjektu.ToUpper(), start);

                if (end == -1)
                {
                    // Zkusíme najít obecné "END;"
                    int temp = upper.IndexOf("END;", start);
                    if (temp != -1)
                        end = temp + 4;
                    else
                        end = text.Length;
                }

                // Samotný kód procedury/funkce
                string kod = text.Substring(start, end - start);

                obj.ZdrojovyKod = kod;
                obj.PocetRadkuKodu = kod.Split('\n').Length;
                return;
            }

            // Načítání zdrojového kódu procedury/funkce/trigeru, které nejsou v balíčku
            string sqlNormal = @"
                SELECT TEXT
                FROM USER_SOURCE
                WHERE NAME = :name
                ORDER BY LINE
            ";

            using (OracleCommand cmd = new OracleCommand(sqlNormal, conn))
            {
                cmd.Parameters.Add("name", obj.NazevObjektu);

                StringBuilder sb = new StringBuilder();
                int counter = 0;

                using (OracleDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        sb.Append(r["TEXT"].ToString());
                        counter++;
                    }
                }

                string kod = sb.ToString().Trim();

                // Pokud objekt nemá žádný zdrojový PL/SQL kód (VIEW, INDEX, TABLE, ...)
                if (string.IsNullOrWhiteSpace(kod))
                {
                    obj.ZdrojovyKod = "";
                    obj.PocetRadkuKodu = 0;
                }
                else
                {
                    obj.ZdrojovyKod = kod;
                    obj.PocetRadkuKodu = kod.Split('\n').Length;
                }

            }
        }

        // TABULKY A VIEW – INFORMACE O SLOUPCÍCH
        /// <summary>
        /// Vrátí počet řádků tabulky (NUM_ROWS)
        /// </summary>
        public static int GetPocetRadkuTabulky(OracleConnection conn, string tableName)
        {
           
            // USER_TABLES.NUM_ROWS obsahuje počet řádků tabulky 
            // NVL(NUM_ROWS, 0) - pokud je NUM_ROWS NULL vrátí 0
            string sql = "SELECT NVL(NUM_ROWS, 0) FROM USER_TABLES WHERE TABLE_NAME = :t";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add("t", tableName);

                object val = cmd.ExecuteScalar();
                if (val == DBNull.Value) {
                    return 0;
                } 

                return Convert.ToInt32(val);
            }
        }

        /// <summary>
        /// Vrátí seznam sloupců zadané tabulky (název, datový typ, délka a zda může být NULL)
        /// Informace  USER_TAB_COLUMNS
        /// </summary>
        public static List<SloupecTabulky> GetSloupceTabulky(OracleConnection conn, string tableName)
        {
            List<SloupecTabulky> list = new List<SloupecTabulky>();

            string sql = @"
                SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE
                FROM USER_TAB_COLUMNS
                WHERE TABLE_NAME = :t
                ORDER BY COLUMN_ID
            ";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add("t", tableName);

                using (OracleDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        SloupecTabulky s = new SloupecTabulky();
                        s.Nazev = r["COLUMN_NAME"].ToString();
                        s.Typ = r["DATA_TYPE"].ToString();
                        s.Delka = Convert.ToInt32(r["DATA_LENGTH"]);
                        s.Nullable = r["NULLABLE"].ToString();

                        list.Add(s);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Vrátí SQL definici VIEW.
        /// </summary>
        public static string GetViewSql(OracleConnection conn, string viewName)
        {
            string sql = "SELECT TEXT FROM USER_VIEWS WHERE VIEW_NAME = :v";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add("v", viewName);

                object res = cmd.ExecuteScalar();
                if (res == null) return "-- View nemá SQL definici --";

                return res.ToString();
            }
        }
    }
}
