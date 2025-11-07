using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class VysledekZapasu
    {
        /// <summary>
        /// Jedinečné ID výsledku zápasu
        /// </summary>
        public int IdZapasu { get; set; }

        /// <summary>
        /// Výsledek zápasu (Domácí : Hosté)
        /// </summary>
        public string Vysledek { get; set; }

        /// <summary>
        /// Celkový počet žlutých karet za zápas
        /// </summary>
        public int PocetZlutychKaret {  get; set; }

        /// <summary>
        /// Celkový počet červených karet za zápas
        /// </summary>
        public int PocetCervenychKaret { get; set; }

        /// <summary>
        /// Počet gólů domacích za zápas
        /// </summary>
        public int PocetGolyDomaci {  get; set; }

        /// <summary>
        /// Počet gólů hostů za zápas
        /// </summary>
        public int PocetGolyHoste { get; set; }

        /// <summary>
        /// Zápas, ke kterému výsledek patří
        /// </summary>
        public Zapas Zapas { get; set; }

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public VysledekZapasu() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu VysledekZapasu
        /// </summary>
        /// <param name="vysledek">Výsledek zápasu</param>
        /// <param name="pocetZlutychKaret">Celkový počet žlutých karet</param>
        /// <param name="pocetCervenychKaret">Celkový počet červených karet</param>
        /// <param name="zapas">Zápas, ke kterému patří výsledek</param>
        public VysledekZapasu(string vysledek, int pocetZlutychKaret, int pocetCervenychKaret, Zapas zapas)
        {
            IdZapasu = zapas.IdZapas;
            Vysledek = vysledek;
            PocetZlutychKaret = pocetZlutychKaret;
            PocetCervenychKaret = pocetCervenychKaret;

            if (vysledek.Contains(':'))
            {
                string[] parts = vysledek.Split(':');

                if (parts.Length == 2 && int.TryParse(parts[0], out int golyDomaci) && int.TryParse(parts[1], out int golyHoste))
                {
                    PocetGolyDomaci = golyDomaci;
                    PocetGolyHoste = golyHoste;
                }
            }

            else
            {
                PocetGolyDomaci = PocetGolyHoste = 0;
            }
        }
    }
}
