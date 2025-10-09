using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje trenéra klubu
    /// </summary>
    public class Trener : IClenKlubu
    {
        /// <summary>
        /// Jedinečné ID člena klubu (trenéra)
        /// </summary>
        public int IdClenKlubu { get; set; }

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
        /// Typ člena klubu (trenér, hráč)
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
        /// Prázdný konstruktor 
        /// </summary>
        public Trener() { }

        /// <summary>
        /// Konstruktor s parametry pro inicializaci všech vlastností trenéra
        /// </summary>
        /// <param name="idClenKlubu">Jedinečné ID člena klubu (trenéra).</param>
        /// <param name="jmeno">Jméno trenéra.</param>
        /// <param name="prijmeni">Příjmení trenéra.</param>
        /// <param name="rodneCislo">Rodné číslo trenéra.</param>
        /// <param name="typClena">Typ člena klubu (trenér).</param>
        /// <param name="telefonniCislo">Telefonní číslo trenéra.</param>
        /// <param name="trenerskaLicence">Trenérská licence trenéra.</param>
        /// <param name="specializace">Specializace trenéra.</param>
        /// <param name="pocetLetPraxe">Počet let praxe trenéra.</param>
        public Trener(int idClenKlubu, string jmeno, string prijmeni, long rodneCislo,
                      string typClena, string telefonniCislo, string trenerskaLicence,
                      string specializace, int pocetLetPraxe)
        {
            IdClenKlubu = idClenKlubu;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            RodneCislo = rodneCislo;
            TypClena = typClena;
            TelefonniCislo = telefonniCislo;
            TrenerskaLicence = trenerskaLicence;
            Specializace = specializace;
            PocetLetPraxe = pocetLetPraxe;
        }
    }
}
