using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Trener : IClenKlubu
    {
        // Implementace vlastností z rozhraní IClenKlubu

        /// <summary>
        /// Jedinečné ID člena klubu (trenéra)
        /// </summary>
        public int IdClenKlubu { get; set; }

        /// <summary>
        /// Jméno trenéra
        /// </summary>
        public string Jmeno { get; set; }

        /// <summary>
        /// Příjmení trenéra
        /// </summary>
        public string Prijmeni { get; set; }

        /// <summary>
        /// Typ člena klubu (trenér)
        /// </summary>
        public TypClena TypClena { get; set; }

        /// <summary>
        /// Telefonní číslo trenéra
        /// </summary>
        public string TelefonniCislo { get; set; }


        // Specifické vlastnosti pro Trenera

        /// <summary>
        /// Trenérská licence trenéra
        /// </summary>
        public string TrenerskaLicence { get; set; }

        /// <summary>
        /// Specializace trenéra (například obrana, útok atd)
        /// </summary>
        public string Specializace { get; set; }

        /// <summary>
        /// Počet let praxe trenéra 
        /// </summary>
        public int PocetLetPraxe { get; set; }

        /// <summary>
        /// Konstruktor třídy Trener
        /// </summary>
        /// <param name="idClenKlubu">Jedinečné ID člena klubu (trenéra)</param>
        /// <param name="jmeno">Jméno trenéra</param>
        /// <param name="prijmeni">Příjmení trenéra</param>
        /// <param name="typClena">Typ člena klubu (trenér)</param>
        /// <param name="telefonniCislo">Telefonní číslo trenéra</param>
        /// <param name="trenerskaLicence">Trenérská licence trenéra</param>
        /// <param name="specializace">Specializace trenéra</param>
        /// <param name="pocetLetPraxe">Počet let praxe trenéra</param>
        public Trener(int idClenKlubu, string jmeno, string prijmeni, TypClena typClena, string telefonniCislo, string trenerskaLicence, string specializace, int pocetLetPraxe)
        {
            IdClenKlubu = idClenKlubu;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            TypClena = typClena;
            TelefonniCislo = telefonniCislo;
            TrenerskaLicence = trenerskaLicence;
            Specializace = specializace;
            PocetLetPraxe = pocetLetPraxe;
        }

    }
}
