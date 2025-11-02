using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Kontrakt
    {
        /// <summary>
        /// Jedinečné ID kontraktu
        /// </summary>
        public int IdClena { get; set; }

        /// <summary>
        /// Datum začátku platnosti kontraktu
        /// </summary>
        public DateOnly DatumZacatku { get; set; }

        /// <summary>
        /// Datum ukončení platnosti kontraktu
        /// </summary>
        public DateOnly DatumKonce { get; set; }

        /// <summary>
        /// Měsíční plat hráče
        /// </summary>
        public int Plat { get; set; }

        /// <summary>
        /// Telefonní číslo na agenta hráče
        /// </summary>
        public string TelCisloNaAgenta { get; set; }

        /// <summary>
        /// Výstupní klauzule kontraktu
        /// </summary>
        public int VystupniKlauzule { get; set; }

        /// <summary>
        /// Hráč, ke kterému kontrakt patří
        /// </summary>
        public Hrac KontraktHrace { get; set; }


        /// <summary>
        /// Prázdný kontruktor pro vytvoření objektu Kontrakt
        /// </summary>
        public Kontrakt() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu Kontrakt
        /// </summary>
        /// <param name="datumZacatku">Začátek platnosti kontraktu</param>
        /// <param name="datumKonce">Konec platnosti kontraktu</param>
        /// <param name="plat">Měsíční plat hráče</param>
        /// <param name="telCisloNaAgenta">Telefonní číslo na agenta</param>
        /// <param name="vystupniKlauzule">Výstupní klauzule kontraktu</param>
        /// <param name="hrac">Hráč, ke kterému kontrakt patří</param>
        public Kontrakt(DateOnly datumZacatku, DateOnly datumKonce, int plat, string telCisloNaAgenta, int vystupniKlauzule, Hrac hrac)
        {
            IdClena = hrac.IdClenKlubu;
            DatumZacatku = datumZacatku;
            DatumKonce = datumKonce;
            Plat = plat;
            TelCisloNaAgenta = telCisloNaAgenta;
            VystupniKlauzule = vystupniKlauzule;
            KontraktHrace = hrac;
        }
    }
}
