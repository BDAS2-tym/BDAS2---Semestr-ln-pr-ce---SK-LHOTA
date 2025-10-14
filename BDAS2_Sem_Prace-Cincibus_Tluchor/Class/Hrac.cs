using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje hráče klubu
    /// </summary>
    public class Hrac : IClenKlubu
    {
        /// <summary>
        /// Jedinečné ID člena klubu (hráče).
        /// V aplikaci s ním přímo nepracujeme, ale v databázi Oracle se používá.
        /// </summary>
        // public int IdClenKlubu { get; set; }

        /// <summary>
        /// Rodné číslo hráče
        /// </summary>
        public long RodneCislo { get; set; }

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
        /// Pozice hráče na hřišti
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
        public Hrac(long rodneCislo, string jmeno, string prijmeni, string telefonniCislo, int pocetVstrelenychGolu, 
            int pocetZlutychKaret, int pocetCervenychKaret, string poziceNaHristi)
        {
            this.RodneCislo = rodneCislo;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.TelefonniCislo = telefonniCislo;
            this.PocetVstrelenychGolu = pocetVstrelenychGolu;
            this.PoziceNaHristi = poziceNaHristi;
            this.PocetZlutychKaret = pocetZlutychKaret;
            this.PocetCervenychKaret = pocetCervenychKaret;
            this.TypClena = "Hrac"; // Defaultně "Hrac"
        }
    }
}
