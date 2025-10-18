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
        public int IdClenKlubu { get; set; }


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

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public ClenKlubu() { }

        /// <summary>
        /// Paremetrický konstruktor pro vytvoření objektu ClenKlubu
        /// </summary>
        /// <param name="idClenKlubu">ID člena</param>
        /// <param name="rodneCislo">Rodné číslo člena</param>
        /// <param name="jmeno">Jméno člena</param>
        /// <param name="prijmeni">Příjmení člena</param>
        /// <param name="typClena">Typ člena</param>
        /// <param name="telefonniCislo">Tel. číslo člena</param>
        public ClenKlubu(int idClenKlubu, long rodneCislo, string jmeno, string prijmeni, string typClena, string telefonniCislo)
        {
            IdClenKlubu = idClenKlubu;
            RodneCislo = rodneCislo;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            TypClena = typClena;
            TelefonniCislo = telefonniCislo;
        }
    }
}
