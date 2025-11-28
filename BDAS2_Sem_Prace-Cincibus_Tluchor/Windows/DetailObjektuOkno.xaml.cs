using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno, které zobrazí detailní informace o systémovém objektu
    /// tabulka, view, procedura, funkce, package, trigger, sekvence
    /// </summary>
    public partial class DetailObjektuOkno : Window
    {
        /// <summary>
        /// Objekt vybraný ze systémového katalogu
        /// </summary>
        private SystemovyObjekt systemovyObjekt;

        public DetailObjektuOkno(SystemovyObjekt systemovyObjekt)
        {
            InitializeComponent();

            this.systemovyObjekt = systemovyObjekt;

            NactiDetaily();
        }

        /// <summary>
        /// Načte detaily o objektu podle jeho typu
        /// </summary>
        private void NactiDetaily()
        {
            using (OracleConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                // Tabulka
                if (systemovyObjekt.TypObjektu == "TABLE")
                {
                    // Načtení sloupců tabulky
                    systemovyObjekt.Sloupce = DatabaseSystemovyKatalog.GetSloupceTabulky(conn, systemovyObjekt.NazevObjektu);

                    // Spočítání počet řádků v tabulce
                    systemovyObjekt.PocetRadku = DatabaseSystemovyKatalog.GetPocetRadkuTabulky(conn, systemovyObjekt.NazevObjektu);

                    systemovyObjekt.PocetRadkuKodu = 0;
                }

                // View
                else if (systemovyObjekt.TypObjektu == "VIEW")
                {
                    // Načtení sloupců view 
                    systemovyObjekt.Sloupce = DatabaseSystemovyKatalog.GetSloupceTabulky(conn, systemovyObjekt.NazevObjektu);

                    // Získání SQL SELECT pohledu
                    systemovyObjekt.ZdrojovyKod = DatabaseSystemovyKatalog.GetViewSql(conn, systemovyObjekt.NazevObjektu);

                    // VIEW NEMÁ PL/SQL KÓD - řádky kódu = 0
                    systemovyObjekt.PocetRadkuKodu = 0;

                    // Počet záznamů ve view
                    systemovyObjekt.PocetRadku = GetCountFromView(conn, systemovyObjekt.NazevObjektu);
                }


                // Indexy
                else if (systemovyObjekt.TypObjektu == "INDEX")
                {
                    systemovyObjekt.Sloupce = null;
                    systemovyObjekt.PocetRadku = 0;
                    systemovyObjekt.PocetRadkuKodu = 0;

                    systemovyObjekt.ZdrojovyKod = "Název indexu: " + systemovyObjekt.NazevObjektu + "\n";
                }

                // Constrainty
                else if (systemovyObjekt.TypObjektu.StartsWith("CONSTRAINT"))
                {
                    systemovyObjekt.Sloupce = null;
                    systemovyObjekt.PocetRadku = 0;
                    systemovyObjekt.PocetRadkuKodu = 0;

                    systemovyObjekt.ZdrojovyKod =
                        "Název: " + systemovyObjekt.NazevObjektu + "\n" +
                        "Typ: " + systemovyObjekt.Stav + "\n";
                }

                // Sekvence
                else if (systemovyObjekt.TypObjektu == "SEQUENCE")
                {
                    string sql = @"
                    SELECT MIN_VALUE, MAX_VALUE, INCREMENT_BY, LAST_NUMBER
                    FROM USER_SEQUENCES
                    WHERE SEQUENCE_NAME = :s
                    ";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("s", systemovyObjekt.NazevObjektu);

                        using (OracleDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                systemovyObjekt.ZdrojovyKod =
                                    "CREATE SEQUENCE " + systemovyObjekt.NazevObjektu + "\n" +
                                    "   MINVALUE " + r["MIN_VALUE"] + "\n" +
                                    "   MAXVALUE " + r["MAX_VALUE"] + "\n" +
                                    "   INCREMENT BY " + r["INCREMENT_BY"] + "\n" +
                                    "   START WITH " + r["LAST_NUMBER"] + ";\n";
                            }
                            else
                            {
                                systemovyObjekt.ZdrojovyKod = "-- Sekvence nebyla nalezena --";
                            }
                        }
                    }

                    systemovyObjekt.Sloupce = null;
                    systemovyObjekt.PocetRadku = 0;
                    systemovyObjekt.PocetRadkuKodu = SpoctiRadkyKodu(systemovyObjekt.ZdrojovyKod);
                }

                // Procedury, Funkce, Triggery, Package
                else
                {
                    // Načtení SQL dané procedury/funkce
                    DatabaseSystemovyKatalog.LoadSqlSource(conn, systemovyObjekt);

                    systemovyObjekt.Sloupce = null;

                    systemovyObjekt.PocetRadku = 0;

                    // Počet řádků kódu VIEW
                    if (string.IsNullOrWhiteSpace(systemovyObjekt.ZdrojovyKod))
                    {
                        systemovyObjekt.PocetRadkuKodu = 0;   // VIEW nemá žádný PL/SQL kód
                    }
                    else
                    {
                        systemovyObjekt.PocetRadkuKodu = systemovyObjekt.ZdrojovyKod.Split('\n').Length;
                    }

                }
            }

            txtTitle.Text =
                systemovyObjekt.TypObjektu + " – " + systemovyObjekt.NazevObjektu +
                "   (Řádky kódu: " + systemovyObjekt.PocetRadkuKodu +
                ", Záznamy: " + systemovyObjekt.PocetRadku + ")";

            txtCode.Text = systemovyObjekt.ZdrojovyKod;

            dgSloupce.ItemsSource = systemovyObjekt.Sloupce;
        }



        /// <summary>
        /// Vrátí počet záznamů ve View pomocí SELECT COUNT(*)
        /// Pokud dojde k chybě (např. view neexistuje), vrátí 0
        /// </summary>
        private int GetCountFromView(OracleConnection conn, string viewName)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM " + viewName;

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    object val = cmd.ExecuteScalar();

                    if (val == DBNull.Value || val == null)
                    {
                        return 0;
                    }

                    return Convert.ToInt32(val);
                }
            }
            catch
            {

                return 0;
            }
        }

        /// <summary>
        /// Spočítá  počet řádků zdrojového kódu
        /// </summary>
        private int SpoctiRadkyKodu(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            string cleaned = text.TrimEnd('\n', '\r');

            if (string.IsNullOrWhiteSpace(cleaned))
            {
                return 0;
            }

            return cleaned.Split('\n').Length;
        }


    }
}
