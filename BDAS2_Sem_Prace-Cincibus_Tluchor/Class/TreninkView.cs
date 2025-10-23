using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída reprezentující trénink v datagridu
    /// </summary>
    public class TreninkView : Trener
    {

        /// <summary>
        /// Rodné číslo trenéra
        /// </summary>
        public long RodneCislo { get; set; }

        /// <summary>
        /// Příjmení trenéra
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Datum a čas konání tréninku
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        /// Místo konání tréninku 
        /// </summary>
        public string Misto { get; set; }

        /// <summary>
        /// Volitelný popis tréninku 
        /// </summary>
        public string? Popis { get; set; }

        /// <summary>
        /// Výchozí konstruktor třídy Trenink.
        /// </summary>
        public TreninkView() { }

        /// <summary>
        /// Přetížený konstruktor pro snadné vytvoření objektu s parametry.
        /// </summary>
        /// <param name="datum">Datum a čas tréninku.</param>
        /// <param name="misto">Místo konání tréninku.</param>
        /// <param name="popis">Volitelný popis tréninku.</param>
        public TreninkView(long rodneCislo, string prijmeni, DateTime datum, string misto, string? popis = null)
        {   
            this.RodneCislo = rodneCislo;
            this.Prijmeni = prijmeni;
            this.Datum = datum;
            this.Misto = misto;
            this.Popis = popis;
        }

    }
}
