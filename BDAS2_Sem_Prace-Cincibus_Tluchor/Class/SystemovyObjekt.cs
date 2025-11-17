using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje jeden databázový objekt z Oracle systémového katalogu
    /// Objekt může být například tabulka, pohled, funkce, trigger, sekvence atd.
    /// </summary>
    public class SystemovyObjekt
    {
        /// <summary>
        /// Název objektu (OBJECT_NAME).
        /// Příklad: HRACI, PKG_HRACI, TRIG_LOG_HRACI
        /// </summary>
        public string NazevObjektu { get; set; }

        /// <summary>
        /// Typ objektu (OBJECT_TYPE).
        /// Příklad: TABLE, VIEW, FUNCTION, TRIGGER, SEQUENCE, PACKAGE
        /// </summary>
        public string TypObjektu { get; set; }

        /// <summary>
        /// Datum vytvoření objektu v databázi (CREATED).
        /// </summary>
        public string DatumVytvoreni { get; set; }

        /// <summary>
        /// Stav objektu (VALID / INVALID)
        /// </summary>
        public string Stav { get; set; }

        /// <summary>
        /// Vytvoří nový záznam databázového objektu
        /// </summary>
        /// <param name="nazev">Název objektu z katalogu</param>
        /// <param name="typ">Typ objektu</param>
        /// <param name="datum">Datum vytvoření objektu</param>
        /// <param name="stav">Stav objektu</param>
        public SystemovyObjekt(string nazev, string typ, string datum, string stav)
        {
            this.NazevObjektu = nazev;
            this.TypObjektu = typ;
            this.DatumVytvoreni = datum;
            this.Stav = stav;
        }

        /// <summary>
        /// Prázdný konstruktor nutný pro funkci některých DataGrid bindingů
        /// </summary>
        public SystemovyObjekt()
        {
        }
    }
}
