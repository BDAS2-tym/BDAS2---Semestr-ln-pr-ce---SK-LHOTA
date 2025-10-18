using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje člena klubu (hráče nebo trenéra).
    /// </summary>
    public class ClenKlubu
    {
        /// <summary>
        /// Unikátní identifikátor člena klubu.
        /// </summary>

      //  public int IdClenKlubu { get; set; }
        int IdClenKlubu { get; set; }

        /// <summary>
        /// Rodné číslo člena klubu.
        /// </summary>
        public long RodneCislo { get; set; }

        /// <summary>
        /// Jméno člena klubu.
        /// </summary>
        public string Jmeno { get; set; } 

        /// <summary>
        /// Příjmení člena klubu.
        /// </summary>
        public string Prijmeni { get; set; } 

        /// <summary>
        /// Typ člena klubu (např. "Hráč" nebo "Trenér").
        /// </summary>
        public string TypClena { get; set; } 

        /// <summary>
        /// Telefonní číslo člena klubu.
        /// </summary>
        public string TelefonniCislo { get; set; } 


    }
}
