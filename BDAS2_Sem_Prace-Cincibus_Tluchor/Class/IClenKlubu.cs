using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public interface IClenKlubu
    {
        /// <summary>
        /// ID člena klubu
        /// </summary>
        //int IdClenKlubu { get; set; }

        /// <summary>
        /// Rodné číslo člena klubu
        /// </summary>
        long RodneCislo { get; set; }

        /// <summary>
        /// Jméno člena klubu
        /// </summary>
        string Jmeno { get; set; }

        /// <summary>
        /// Příjmení člena klubu
        /// </summary>
        string Prijmeni { get; set; }

        /// <summary>
        /// Typ člena klubu (hráč nebo trenér)
        /// </summary>
        string TypClena { get; set; }

        /// <summary>
        /// Telefonní číslo člena klubu
        /// </summary>
        string TelefonniCislo { get; set; }
    }

  
}
