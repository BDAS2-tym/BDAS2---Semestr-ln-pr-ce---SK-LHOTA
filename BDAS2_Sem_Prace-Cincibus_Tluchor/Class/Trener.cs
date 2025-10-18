using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje trenéra klubu
    /// </summary>
    public class Trener : ClenKlubu
    {
        /// <summary>
        /// Jedinečné ID člena klubu (trenéra)
        /// </summary>
        //public int IdClenKlubu { get; set; }

        /// <summary>
        /// Rodné číslo trenéra
        /// </summary>
        public long RodneCislo { get; set; }

        /// <summary>
        /// Jméno trenéra
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Příjmení trenéra
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Typ člena klubu (např. "Trener", "Hrac")
        /// </summary>
        public string TypClena { get; set; }

        /// <summary>
        /// Telefonní číslo trenéra
        /// </summary>
        public string TelefonniCislo { get; set; }

        /// <summary>
        /// Trenérská licence trenéra
        /// </summary>
        public string TrenerskaLicence { get; set; }

        /// <summary>
        /// Specializace trenéra
        /// </summary>
        public string Specializace { get; set; }

        /// <summary>
        /// Počet let praxe trenéra
        /// </summary>
        public int PocetLetPraxe { get; set; }

        /// <summary>
        /// Výchozí konstruktor
        /// </summary>
        public Trener() { }

        /// <summary>
        /// Konstruktor pro inicializaci trenéra se všemi parametry
        /// </summary>
        /// <param name="jmeno">Jméno trenéra</param>
        /// <param name="prijmeni">Příjmení trenéra</param>
        /// <param name="rodneCislo">Rodné číslo trenéra</param>
        /// <param name="typClena">Typ člena klubu (trenér)</param>
        /// <param name="telefonniCislo">Telefonní číslo trenéra</param>
        /// <param name="trenerskaLicence">Trenérská licence</param>
        /// <param name="specializace">Specializace trenéra</param>
        /// <param name="pocetLetPraxe">Počet let praxe trenéra</param>
        public Trener(string jmeno, string prijmeni, long rodneCislo, string typClena,
            string telefonniCislo, string trenerskaLicence, string specializace, int pocetLetPraxe)
        {
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.RodneCislo = rodneCislo;
            this.TypClena = typClena;
            this.TelefonniCislo = telefonniCislo;
            this.TrenerskaLicence = trenerskaLicence;
            this.Specializace = specializace;
            this.PocetLetPraxe = pocetLetPraxe;
            this.TypClena = "Trener"; // Defaultně "Trener"
        }
    }
}
