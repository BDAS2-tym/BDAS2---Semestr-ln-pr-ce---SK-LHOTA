using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Zapas
    {
        /// <summary>
        /// Jedinečné ID zápasu
        /// </summary>
        public int IdZapas { get; set; }

        /// <summary>
        /// Datum a čas zápasu
        /// </summary>
        public DateTime Datum {  get; set; }

        /// <summary>
        /// Výsledek zápasu (Domácí : Hosté)
        /// </summary>
        public string Vysledek { get; set; }

        /// <summary>
        /// Konkrétní stav zápasu
        /// </summary>
        public string StavZapasu {  get; set; }

        /// <summary>
        /// Tým domácích
        /// </summary>
        public string DomaciTym { get; set;}

        /// <summary>
        /// Tým hostů
        /// </summary>
        public string HosteTym { get; set;}

        /// <summary>
        /// Soutěž, do které zápas patří
        /// </summary>
        public Soutez Soutez { get; set; }

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public Zapas() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu Zapasu
        /// </summary>
        /// <param name="idZapas">ID zápasu</param>
        /// <param name="datum">Datum a čas zápasu</param>
        /// <param name="stavZapasu">Konkrétní stav zápasu</param>
        /// <param name="domaciTym">Tým domácích</param>
        /// <param name="hosteTym">Tým hostů</param>
        /// <param name="soutez">Soutěž, do které zápas patří</param>
        public Zapas(int idZapas, DateTime datum, string vysledek, string stavZapasu, string domaciTym, string hosteTym, Soutez soutez)
        {
            if (vysledek.Contains(':') && vysledek.Length == 5)
            {
                Vysledek = vysledek;
            }

            else
            {
                throw new NonValidDataException("Špatný formát zápasu!");
            }

            IdZapas = idZapas;
            Datum = datum;
            StavZapasu = stavZapasu;
            DomaciTym = domaciTym;
            HosteTym = hosteTym;
            Soutez = soutez;
        }
    }
}
