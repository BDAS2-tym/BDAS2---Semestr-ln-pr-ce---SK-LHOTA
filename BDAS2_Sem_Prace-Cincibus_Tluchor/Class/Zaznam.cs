using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Zaznam
    {
        /// <summary>
        /// Jedinečné ID záznamu
        /// </summary>
        public int IdZaznam { get; set; }

        /// <summary>
        /// Uživatel, který provedl změnu
        /// </summary>
        public Uzivatel Uzivatel { get; set; }

        /// <summary>
        /// Název tabulky, která byla změněna
        /// </summary>
        public string Tabulka { get; set; }

        /// <summary>
        /// Datum a čas změny
        /// </summary>
        public DateTime Cas { get; set; }
        
        /// <summary>
        /// Operace, která byla provedena (INSERT, DELETE, UPDATE)
        /// </summary>
        public string Operace {  get; set; }

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public Zaznam() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu Zaznamu
        /// </summary>
        /// <param name="idZaznam">ID záznamu</param>
        /// <param name="uzivatel">Uživatel, který změnu provedl</param>
        /// <param name="tabulka">Tabulka, které byla změněna</param>
        /// <param name="cas">Datum a čas změny</param>
        /// <param name="operace">Operace, která byla provedena</param>
        public Zaznam(int idZaznam, Uzivatel uzivatel, string tabulka, DateTime cas, string operace)
        {
            IdZaznam = idZaznam;
            Uzivatel = uzivatel;
            Tabulka = tabulka;
            Cas = cas;
            Operace = operace;
        }
    }
}
