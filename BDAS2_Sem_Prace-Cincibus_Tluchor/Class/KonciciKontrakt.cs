using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class KonciciKontrakt
    {
        /// <summary>
        /// Jedinečné ID kontraktu
        /// </summary>
        public int IdHrace { get; set; }

        /// <summary>
        /// Jméno hráče
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Příjmení hráče
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Datum ukončení platnosti kontraktu
        /// </summary>
        public string DatumUkonceni { get; set; }

        /// <summary>
        /// Celkový počet dnů, kolik chybí do ukončení kontraktu
        /// </summary>
        public int DnuDoKonce { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public KonciciKontrakt KontraktHrace => this;

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public KonciciKontrakt() { }
    }
}
