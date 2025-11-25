using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje jednoho hráče z pohledu TOP 3 nejlepších střelců
    /// Obsahuje základní informace potřebné k zobrazení v příslušném dialogu
    /// </summary>
    public class TopStrelec
    {
        /// <summary>
        /// Jméno hráče
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Příjmení hráče
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Celkový počet vstřelených gólů
        /// </summary>
        public int PocetGolu { get; set; }

        /// <summary>
        /// Název herní pozice, na které hráč nastupuje (např. Útočník, Obránce)
        /// </summary>
        public string Pozice { get; set; }

        /// <summary>
        /// Pořadí hráče v žebříčku střelců 
        /// </summary>
        public int Poradi { get; set; }

        /// <summary>
        /// Konstruktor třídy TopStrelec
        /// </summary>
        /// <param name="jmeno">Jméno hráče</param>
        /// <param name="prijmeni">Příjmení hráče</param>
        /// <param name="pocetGolu">Počet vstřelených gólů</param>
        /// <param name="pozice">Název pozice hráče</param>
        /// <param name="poradi">Pořadí hráče v TOP 3</param>
        public TopStrelec(string jmeno, string prijmeni, int pocetGolu, string pozice, int poradi)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            PocetGolu = pocetGolu;
            Pozice = pozice;
            Poradi = poradi;
        }

        /// <summary>
        /// Bezparametrický konstruktor se využívá při načítání dat z databáze 
        /// </summary>
        public TopStrelec()
        {
        }
    }
}
