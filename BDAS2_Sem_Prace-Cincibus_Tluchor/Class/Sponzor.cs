using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Sponzor
    {
        /// <summary>
        /// Jedinečné ID sponzora
        /// </summary>
        public int IdSponzor { get; set; }

        /// <summary>
        /// Název nebo jméno sponzora
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Částka, kterou sponzor přispívá
        /// </summary>
        public decimal SponzorovanaCastka { get; set; }

        /// <summary>
        /// Konstruktor prázdný
        /// </summary>
        public Sponzor() { }

        /// <summary>
        /// Konstruktor pro vytvoření sponzora
        /// </summary>
        public Sponzor(int idSponzor, string jmeno, decimal sponzorovanaCastka)
        {
            IdSponzor = idSponzor;
            Jmeno = jmeno;
            SponzorovanaCastka = sponzorovanaCastka;
        }
    }
}
