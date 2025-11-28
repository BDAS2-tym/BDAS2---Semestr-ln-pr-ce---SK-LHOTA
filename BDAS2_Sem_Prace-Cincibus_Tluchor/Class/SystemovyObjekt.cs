using System.Collections.Generic;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje jeden sloupec tabulky 
    /// </summary>
    public class SloupecTabulky
    {
        /// <summary>
        /// Název sloupce v tabulce
        /// </summary>
        public string Nazev { get; set; }

        /// <summary>
        /// Datový typ sloupce (VARCHAR2, NUMBER, DATE, ...)
        /// </summary>
        public string Typ { get; set; }

        /// <summary>
        /// Maximální délka sloupce 
        /// </summary>
        public int Delka { get; set; }

        /// <summary>
        /// Informace, zda může být sloupec NULL (Y/N)
        /// </summary>
        public string Nullable { get; set; }
    }

    /// <summary>
    /// Reprezentuje jeden objekt systémového katalogu Oracle
    /// Může jít o tabulku, view, proceduru, funkci, trigger, package, index, sekvenci nebo constraint
    /// </summary>
    public class SystemovyObjekt
    {
        /// <summary>
        /// Název objektu v databázi
        /// </summary>
        public string NazevObjektu { get; set; }

        /// <summary>
        /// Typ objektu (TABLE, VIEW, PROCEDURE, FUNCTION, TRIGGER, PACKAGE, INDEX, SEQUENCE, ...)
        /// </summary>
        public string TypObjektu { get; set; }

        /// <summary>
        /// Datum vytvoření objektu (z USER_OBJECTS.CREATED)
        /// </summary>
        public string DatumVytvoreni { get; set; }

        /// <summary>
        /// Stav objektu (VALID / INVALID)
        /// </summary>
        public string Stav { get; set; }

        /// <summary>
        /// Počet záznamů v tabulce nebo view
        /// </summary>
        public int PocetRadku { get; set; }

        /// <summary>
        /// SQL zdrojový kód objektu (procedura, funkce, trigger, package body)
        /// U tabulek, indexů a constraintů může být prázdný
        /// </summary>
        public string ZdrojovyKod { get; set; }

        /// <summary>
        /// Počet řádků zdrojového kódu objektu (pro PL/SQL objekty)
        /// </summary>
        public int PocetRadkuKodu { get; set; }

        /// <summary>
        /// Pokud je procedura/funkce uvnitř balíčku, obsahuje jméno parent balíčku
        /// Pokud není součástí balíčku, je prázdný string
        /// </summary>
        public string ParentPackage { get; set; }

        /// <summary>
        /// Seznam sloupců. Pouze pro TABLE a VIEW. U jiných objektů je prázdný
        /// </summary>
        public List<SloupecTabulky> Sloupce { get; set; }

        /// <summary>
        /// Konstruktor – vytvoří objekt se základně inicializovaným seznamem sloupců
        /// </summary>
        public SystemovyObjekt()
        {
            Sloupce = new List<SloupecTabulky>();
        }
    }
}
