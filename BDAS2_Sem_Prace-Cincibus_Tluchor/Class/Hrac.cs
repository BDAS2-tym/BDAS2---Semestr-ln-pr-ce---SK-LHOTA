using System;
using System.Collections.ObjectModel;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{

    /// <summary>
    /// Reprezentuje jednu pozici hráče z číselníku (např. Brankář, Obránce, Záložnik, Útočník)
    /// Tato třída se používá do ComboBoxu při výběru pozice
    /// </summary>
    public class Pozice
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
    }

    /// <summary>
    /// Reprezentuje hráče klubu
    /// </summary>
    public class Hrac : ClenKlubu
    {
        /// <summary>
        /// Cizí klíč – ID pozice z číselníku POZICE_HRACE
        /// </summary>
        public int IdPozice { get; set; }

        /// <summary>
        /// Rodné číslo hráče
        /// </summary>
        public string RodneCislo { get; set; }

        /// <summary>
        /// Jméno hráče
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Příjmení hráče
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Typ člena klubu (např. "Hrac", "Trener")
        /// </summary>
        public string TypClena { get; set; }

        /// <summary>
        /// Telefonní číslo hráče
        /// </summary>
        public string TelefonniCislo { get; set; }

        /// <summary>
        /// Počet vstřelených gólů hráče
        /// </summary>
        public int PocetVstrelenychGolu { get; set; }

        /// <summary>
        /// Text názvu pozice pro DataGrid (z view – Brankář/Obránce/Záložník/Útočník)
        /// </summary>
        public string PoziceNaHristi { get; set; }

        /// <summary>
        /// Počet žlutých karet
        /// </summary>
        public int PocetZlutychKaret { get; set; }

        /// <summary>
        /// Počet červených karet
        /// </summary>
        public int PocetCervenychKaret { get; set; }

        /// <summary>
        /// Datum, kdy bylo opatření uděleno
        /// </summary>
        public DateTime DatumOpatreni { get; set; }

        /// <summary>
        /// Délka disciplinárního trestu 
        /// </summary>
        public int DelkaTrestu { get; set; }

        /// <summary>
        /// Důvod disciplinárního opatření (může být null)
        /// </summary>
        public string? DuvodOpatreni { get; set; }

        /// <summary>
        /// Textová reprezentace data disciplinárního opatření pro DataGrid,
        /// pokud hráč žádné opatření nemá
        /// </summary>
        public string DatumOpatreniText { get; set; } = "–";

        /// <summary>
        /// Výchozí konstruktor
        /// </summary>
        public Hrac() { }

        /// <summary>
        /// Konstruktor pro inicializaci hráče
        /// </summary>
        /// <param name="rodneCislo">Rodné číslo hráče</param>
        /// <param name="jmeno">Jméno hráče</param>
        /// <param name="prijmeni">Příjmení hráče</param>
        /// <param name="telefonniCislo">Telefonní číslo hráče</param>
        /// <param name="pocetVstrelenychGolu">Počet vstřelených gólů</param>
        /// <param name="pocetZlutychKaret">Počet žlutých karet</param>
        /// <param name="pocetCervenychKaret">Počet červených karet</param>
        /// <param name="poziceNaHristi">Pozice hráče na hřišti přes combobox v dialogu přidej</param>
        public Hrac(string rodneCislo, string jmeno, string prijmeni, string telefonniCislo, int pocetVstrelenychGolu, 
            int pocetZlutychKaret, int pocetCervenychKaret, int idPozice)
        {
  
            this.RodneCislo = rodneCislo;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.TelefonniCislo = telefonniCislo;
            this.PocetVstrelenychGolu = pocetVstrelenychGolu;
            
            this.PocetZlutychKaret = pocetZlutychKaret;
            this.PocetCervenychKaret = pocetCervenychKaret;
            this.IdPozice = idPozice;     
            this.TypClena = "Hrac"; // Defaultně "Hrac"
        }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni}   RČ: {RodneCislo}";
        }

    }
}
