using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseSystemovyKatalog
    {
        public static List<SystemovyObjekt> GetSystemoveObjekty()
        {
            List<SystemovyObjekt> vysledek = new List<SystemovyObjekt>();

            OracleConnection conn = DatabaseManager.GetConnection();

            var mapaCreated = LoadCreatedMap(conn);

            vysledek.AddRange(LoadZakladniObjekty(conn, mapaCreated));
            vysledek.AddRange(LoadObjektyZPackage(conn, mapaCreated));
            vysledek.AddRange(LoadIndexy(conn, mapaCreated));
            vysledek.AddRange(LoadConstrainty(conn));
            vysledek.AddRange(LoadSekvence(conn, mapaCreated));

            return vysledek;
        }

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

        private static List<SystemovyObjekt> LoadZakladniObjekty(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

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
                    o.NazevObjektu = r["OBJECT_NAME"].ToString();
                    o.TypObjektu = r["OBJECT_TYPE"].ToString();
                    o.Stav = r["STATUS"].ToString();
                    o.ParentPackage = "";
                    o.PocetRadku = -1;
                    o.PocetRadkuKodu = -1;
                    o.ZdrojovyKod = null;
                    o.DatumVytvoreni = createdMap.ContainsKey((o.NazevObjektu, o.TypObjektu))
                        ? createdMap[(o.NazevObjektu, o.TypObjektu)]
                        : "";

                    list.Add(o);
                }
            }

            return list;
        }

        private static List<SystemovyObjekt> LoadObjektyZPackage(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

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
                    string pkg = r["NAME"].ToString();
                    string text = r["TEXT"].ToString();

                    if (!textyBalicku.ContainsKey(pkg))
                        textyBalicku[pkg] = new StringBuilder();

                    textyBalicku[pkg].Append(text);
                }
            }

            foreach (var p in textyBalicku)
            {
                string balicek = p.Key;
                string obsah = p.Value.ToString();
                string created = createdMap.ContainsKey((balicek, "PACKAGE")) ? createdMap[(balicek, "PACKAGE")] : "";

                string[] radky = obsah.Split('\n');

                foreach (string r in radky)
                {
                    string radek = r.Trim().ToUpper();

                    if (radek.StartsWith("PROCEDURE "))
                    {
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

                    if (radek.StartsWith("FUNCTION "))
                    {
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

            return list;
        }

        private static List<SystemovyObjekt> LoadIndexy(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            string sql = @"
                SELECT INDEX_NAME, TABLE_NAME, UNIQUENESS
                FROM USER_INDEXES
            ";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string nazev = r["INDEX_NAME"].ToString();
                    string created = createdMap.ContainsKey((nazev, "INDEX")) ? createdMap[(nazev, "INDEX")] : "";

                    SystemovyObjekt o = new SystemovyObjekt
                    {
                        NazevObjektu = nazev,
                        TypObjektu = "INDEX",
                        Stav = r["UNIQUENESS"].ToString(),
                        DatumVytvoreni = created,
                        ZdrojovyKod = ""
                    };

                    list.Add(o);
                }
            }

            return list;
        }

        private static List<SystemovyObjekt> LoadConstrainty(OracleConnection conn)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            string sql = @"
                SELECT CONSTRAINT_NAME, CONSTRAINT_TYPE, TABLE_NAME
                FROM USER_CONSTRAINTS
            ";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    SystemovyObjekt o = new SystemovyObjekt
                    {
                        NazevObjektu = r["CONSTRAINT_NAME"].ToString(),
                        TypObjektu = "CONSTRAINT " + r["CONSTRAINT_TYPE"].ToString(),
                        Stav = "VALID",
                        DatumVytvoreni = "",
                        ZdrojovyKod = ""
                    };

                    list.Add(o);
                }
            }

            return list;
        }

        private static List<SystemovyObjekt> LoadSekvence(
            OracleConnection conn,
            Dictionary<(string, string), string> createdMap)
        {
            List<SystemovyObjekt> list = new List<SystemovyObjekt>();

            string sql = "SELECT SEQUENCE_NAME FROM USER_SEQUENCES";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            using (OracleDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string name = r["SEQUENCE_NAME"].ToString();
                    string created = createdMap.ContainsKey((name, "SEQUENCE")) ? createdMap[(name, "SEQUENCE")] : "";

                    SystemovyObjekt o = new SystemovyObjekt
                    {
                        NazevObjektu = name,
                        TypObjektu = "SEQUENCE",
                        DatumVytvoreni = created,
                        Stav = "VALID",
                        ZdrojovyKod = "-- Detail sekvence se načte v detailu --"
                    };

                    list.Add(o);
                }
            }

            return list;
        }

        public static void LoadSqlSource(OracleConnection conn, SystemovyObjekt obj)
        {
            if (obj.ParentPackage != "")
            {
                string sql = @"
                    SELECT TEXT
                    FROM USER_SOURCE
                    WHERE NAME = :pkg AND TYPE='PACKAGE BODY'
                    ORDER BY LINE
                ";

                StringBuilder sb = new StringBuilder();

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("pkg", obj.ParentPackage);

                    using (OracleDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            sb.Append(r["TEXT"].ToString());
                    }
                }

                string text = sb.ToString();
                string upper = text.ToUpper();
                string hledat = obj.TypObjektu + " " + obj.NazevObjektu.ToUpper();
                int start = upper.IndexOf(hledat);

                if (start == -1)
                {
                    obj.ZdrojovyKod = "-- Kód nebyl nalezen --";
                    obj.PocetRadkuKodu = 0;
                    return;
                }

                int end = upper.IndexOf("END " + obj.NazevObjektu.ToUpper(), start);
                if (end == -1)
                {
                    int temp = upper.IndexOf("END;", start);
                    end = temp != -1 ? temp + 4 : text.Length;
                }

                string kod = text.Substring(start, end - start);
                obj.ZdrojovyKod = kod;
                obj.PocetRadkuKodu = kod.Split('\n').Length;
                return;
            }

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

        public static int GetPocetRadkuTabulky(OracleConnection conn, string tableName)
        {
            string sql = "SELECT NVL(NUM_ROWS, 0) FROM USER_TABLES WHERE TABLE_NAME = :t";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add("t", tableName);

                object val = cmd.ExecuteScalar();
                if (val == DBNull.Value)
                    return 0;

                return Convert.ToInt32(val);
            }
        }

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
                        SloupecTabulky s = new SloupecTabulky
                        {
                            Nazev = r["COLUMN_NAME"].ToString(),
                            Typ = r["DATA_TYPE"].ToString(),
                            Delka = Convert.ToInt32(r["DATA_LENGTH"]),
                            Nullable = r["NULLABLE"].ToString()
                        };

                        list.Add(s);
                    }
                }
            }

            return list;
        }

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
