using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje hráče klubu
    /// </summary>
    public class Hrac : IClenKlubu
    {
        /// <summary>
        /// Jedinečné ID člena klubu (hráče)
        /// </summary>
        //public int IdClenKlubu { get; set; }

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
        /// Typ člena klubu (trenér, hráč)
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
        /// Počet vstřelených gólů hráče
        /// </summary>
        public string PoziceNaHristi { get; set; }

        public int PocetZlutychKaret { get; set; }

        public int PocetCervenychKaret { get; set; }


        

        public Hrac() { }

        public Hrac( long rodneCislo, string jmeno, string prijmeni,
              string telefonniCislo, int pocetVstrelenychGolu, int pocetZlutychKaret, int pocetCervenychKaret,
             string poziceNaHristi)
        {
            //this.IdClenKlubu = idClenKlubu;
            this.RodneCislo = rodneCislo;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;

            this.TelefonniCislo = telefonniCislo;
            this.PocetVstrelenychGolu = pocetVstrelenychGolu;
            this.PoziceNaHristi = poziceNaHristi;
            this.PocetZlutychKaret = pocetZlutychKaret;
            this.PocetCervenychKaret = pocetCervenychKaret;
        }


    }
}