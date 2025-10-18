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
        public long? SponzorovanaCastka { get; set; }

        /// <summary>
        /// Seznam členů klubu, kteří jsou sponzorováni daným sponzorem
        /// </summary>
        public List<IClenKlubu> SponzorovaniClenove { get; set; }

        /// <summary>
        /// Seznam soutěží, které jsou sponzorovány daným sponzorem
        /// </summary>
        public List<Soutez> SponzorovaneSouteze {get; set;}

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public Sponzor() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu Sponzor
        /// </summary>
        /// <param name="idSponzor">ID sponzora</param>
        /// <param name="jmeno">Jméno sponzora</param>
        /// <param name="sponzorovanaCastka">Částka, kterou přispěl sponzor</param>
        /// <param name="sponzorovaniClenove">Clenové, kteří jsou sponzorováni daným sponzorem</param>
        /// <param name="sponzorovaneSouteze">Soutěže, které jsou sponzorovány daným sponzorem</param>
        public Sponzor(int idSponzor, string jmeno, long? sponzorovanaCastka, List<IClenKlubu> sponzorovaniClenove, List<Soutez> sponzorovaneSouteze)
        {
            IdSponzor = idSponzor;
            Jmeno = jmeno;
            SponzorovanaCastka = sponzorovanaCastka;
            SponzorovaniClenove = sponzorovaniClenove;
            SponzorovaneSouteze = sponzorovaneSouteze;
        }
    }
}
