using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions
{
    /// <summary>
    /// Třída obsahující různé validační metody pro kontrolu uživatelských vstupů
    /// Slouží k ověření textových polí, čísel, telefonních čísel, rodného čísla,
    /// textů, dat a dalších údajů před uložením do databáze
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Povolující pouze česká písmena a diakritiku.
        /// Používá se pro kontrolu jména a příjmení.
        /// </summary>
        private static readonly Regex RegexPismena = new Regex(@"^[A-Za-zÁÉĚÍÓÚŮÝŽŠČŘĎŤŇáéěíóúůýžščřďťň]+$");

        /// <summary>
        /// Validuje jméno – nesmí být prázdné a musí obsahovat pouze písmena
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
        /// Validuje příjmení – nesmí být prázdné a musí obsahovat pouze písmena
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
        /// Validuje rodné číslo – musí mít 10 číslic
        /// </summary>
        public static void ValidujRodneCislo(string rodneCislo)
        {
            if (string.IsNullOrWhiteSpace(rodneCislo))
            {
                throw new Exception("Rodné číslo nesmí být prázdné!");
            }

            if (rodneCislo.Length != 10)
            {
                throw new Exception("Rodné číslo musí mít přesně 10 číslic!");
            }

            if (!rodneCislo.All(char.IsDigit))
            {
                throw new Exception("Rodné číslo může obsahovat pouze číslice!");
            }
                
        }

        /// <summary>
        /// Validuje telefonní číslo – délka musí být v daném rozsahu a obsahovat jen číslice
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
        /// Validace libovolného celého čísla v textu
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
        /// Kontrola, zda je datum vyplněno (není null)
        /// </summary>
        public static void ValidujDatum(DateTime? datum, string nazev)
        {
            if (datum == null)
            {
                throw new Exception($"{nazev} nesmí být prázdné!");
            }
               
        }

        /// <summary>
        /// Validace trenérské licence – povinná, max 15 znaků (kvůli VARCHAR2 limitu)
        /// </summary>
        public static void ValidujTrenerskouLicenci(string licence)
        {
            if (string.IsNullOrWhiteSpace(licence)) 
            {
                throw new Exception("Trenérská licence nesmí být prázdná");
            }

            // Bezpečný limit pro VARCHAR2(20)
            if (licence.Length > 15) {
                throw new Exception("Trenérská licence nesmí být delší než 15 znaků");
            }
        }

        /// <summary>
        /// Validace praxe – musí být celé číslo a alespoň 1 neboli 1 rok praxe
        /// </summary>
        public static void ValidujPocetLetPraxeTrenera(string pocet)
        {
            if (!int.TryParse(pocet, out int cislo))
            {
                throw new Exception("Počet let praxe musí být celé číslo!");
            }

            if (cislo < 1)
            {
                throw new Exception("Počet let praxe musí být alespoň 1!");
            }
               
        }

        /// <summary>
        /// Validace specializace trenéra – nepovinná, ale má limit délky
        /// </summary>
        public static void ValidujSpecializaciTrenera(string? specializace)
        {
            if (string.IsNullOrWhiteSpace(specializace))
            {
                return;
            }

            // Bezpečný limit pro VARCHAR2(30)
            if (specializace.Length > 22)
            {
                throw new Exception("Specializace nesmí být delší než 22 znaků");
            }
              
        }

        /// <summary>
        /// Validace místa tréninku – povinné, max 40 znaků, omezené znaky
        /// </summary>
        public static void ValidujMistoTreninku(string misto)
        {
            if (string.IsNullOrWhiteSpace(misto))
            {
                throw new Exception("Místo tréninku nesmí být prázdné!");
            }

            if (misto.Length > 40)
            {
                throw new Exception("Místo tréninku nesmí být delší než 40 znaků!");
            }

            if (!Regex.IsMatch(misto, @"^[A-Za-z0-9ÁÉĚÍÓÚŮÝŽŠČŘĎŤŇáéěíóúůýžščřďťň ]+$"))
            {
                throw new Exception("Místo tréninku může obsahovat pouze písmena, číslice a mezery!");
            }
                
        }

        /// <summary>
        /// Validace popisu tréninku – kontroluje i byte velikost kvůli VARCHAR2 limitu
        /// </summary>
        public static void ValidujPopisTreninku(string popis)
        {
            if (string.IsNullOrWhiteSpace(popis))
            {
                return; // popis je volitelný
            }

            // Odstranime neviditelné znaky z RichTextBoxu 
            popis = popis.Trim();

            if (popis.Length > 30)
            {
                throw new Exception("Popis tréninku nesmí být delší než 30 znaků!");
            }
                 
            int byteCount = System.Text.Encoding.UTF8.GetByteCount(popis);

            if (byteCount > 30)
            {
                throw new Exception("Popis/ Náplň obsahuje znaky s diakritikou – převyšuje limit 30 bajtů (Oracle limit)!");
            }
    
        }


        /// <summary>
        /// Ověří, zda již existuje člen s daným rodným číslem v databázi
        /// Používá jednoduchý SELECT COUNT(*)
        /// </summary>
        public static bool ExistujeRodneCislo(OracleConnection conn, string rodneCislo)
        {
            string sql = "SELECT COUNT(*) FROM PREHLED_UZIVATELSKE_UCTY WHERE RODNE_CISLO = :rc";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add("rc", OracleDbType.Varchar2).Value = rodneCislo;
                int pocet = Convert.ToInt32(cmd.ExecuteScalar());
                return pocet > 0;
            }
        }
    }
}
