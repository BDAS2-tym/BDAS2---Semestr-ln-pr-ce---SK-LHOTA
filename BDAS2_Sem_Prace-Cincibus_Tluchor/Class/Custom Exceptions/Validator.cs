using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions
{
    /// <summary>
    /// Třída obsahující centralizované validační metody pro vstupní data.
    /// Používá se napříč celou aplikací.
    /// </summary>
    public static class Validator
    {
        // Regex pouze pro česká jména (písmena + diakritika)
        private static readonly Regex RegexPismena =
            new Regex(@"^[A-Za-zÁÉĚÍÓÚŮÝŽŠČŘĎŤŇáéěíóúůýžščřďťň]+$");

        /// <summary>
        /// Ověří, že jméno není prázdné a obsahuje pouze písmena.
        /// </summary>
        public static void ValidujJmeno(string jmeno)
        {
            if (string.IsNullOrWhiteSpace(jmeno))
            {
                throw new Exception("Jméno nesmí být prázdné!");
            }

            if (!RegexPismena.IsMatch(jmeno))
            {
                throw new Exception("Jméno může obsahovat pouze písmena!");
            }
        }

        /// <summary>
        /// Ověří, že příjmení není prázdné a obsahuje pouze písmena.
        /// </summary>
        public static void ValidujPrijmeni(string prijmeni)
        {
            if (string.IsNullOrWhiteSpace(prijmeni))
            {
                throw new Exception("Příjmení nesmí být prázdné!");
            }

            if (!RegexPismena.IsMatch(prijmeni))
            {
                throw new Exception("Příjmení může obsahovat pouze písmena!");
            }
        }

        /// <summary>
        /// Ověří rodné číslo – musí mít přesně 10 číslic.
        /// </summary>
        public static void ValidujRodneCislo(string rc)
        {
            if (string.IsNullOrWhiteSpace(rc))
            {
                throw new Exception("Rodné číslo nesmí být prázdné!");
            }

            if (rc.Length != 10)
            {
                throw new Exception("Rodné číslo musí mít přesně 10 číslic!");
            }

            if (!rc.All(char.IsDigit))
            {
                throw new Exception("Rodné číslo může obsahovat pouze číslice!");
            }
        }

        /// <summary>
        /// Validace telefonního čísla.
        /// </summary>
        public static void ValidujTelefon(string telefon, int min = 9, int max = 12)
        {
            if (string.IsNullOrWhiteSpace(telefon))
            {
                throw new Exception("Telefonní číslo nesmí být prázdné!");
            }

            if (telefon.Length < min || telefon.Length > max)
            {
                throw new Exception($"Telefonní číslo musí být v délce {min}-{max} číslic!");
            }

            if (!telefon.All(char.IsDigit))
            {
                throw new Exception("Telefonní číslo může obsahovat pouze číslice!");
            }
        }

        /// <summary>
        /// Validace obecného celého nezáporného čísla.
        /// </summary>
        public static void ValidujCeleCislo(string text, string nazev)
        {
            if (!int.TryParse(text, out int cislo))
            {
                throw new Exception($"{nazev} musí být celé číslo!");
            }

            if (cislo < 0)
            {
                throw new Exception($"{nazev} nemůže být záporné!");
            }
        }

        /// <summary>
        /// Validace data – musí existovat.
        /// </summary>
        public static void ValidujDatum(DateTime? datum, string nazev)
        {
            if (datum == null)
            {
                throw new Exception($"{nazev} nesmí být prázdné!");
            }
        }
    }
}
