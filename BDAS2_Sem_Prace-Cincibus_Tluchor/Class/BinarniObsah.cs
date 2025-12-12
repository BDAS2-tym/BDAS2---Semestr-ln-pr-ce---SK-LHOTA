using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje binární obsah uložený v databázi
    /// </summary>
    public class BinarniObsah
    {
        /// <summary>
        /// Jedinečný identifikátor záznamu v tabulce BINARNI_OBSAH
        /// </summary>
        public int IdObsah { get; set; }

        /// <summary>
        /// Název souboru bez přípony 
        /// </summary>
        public string NazevSouboru { get; set; }

        /// <summary>
        /// Typ souboru určený podle MIME typu (např. "image/png", "application/pdf")
        /// </summary>
        public string TypSouboru { get; set; }

        /// <summary>
        /// Přípona souboru bez tečky (např. "png", "pdf")
        /// </summary>
        public string PriponaSouboru { get; set; }

        /// <summary>
        /// Binární obsah samotného souboru
        /// </summary>
        public byte[] Obsah { get; set; }

        /// <summary>
        /// Datum a čas, kdy byl soubor nahrán do databáze
        /// </summary>
        public DateTime DatumNahrani { get; set; }

        /// <summary>
        /// Datum a čas poslední modifikace souboru
        /// </summary>
        public DateTime DatumModifikace { get; set; }

        /// <summary>
        /// Typ operace, která byla s obsahem provedena (např. "INSERT", "UPDATE", "DELETE")
        /// </summary>
        public string Operace { get; set; }

        /// <summary>
        /// Identifikátor uživatelského účtu, který soubor nahrál nebo upravil
        /// </summary>
        public int IdUzivatelskyUcet { get; set; }

        /// <summary>
        /// Název role nebo jméno uživatele, který soubor nahrál či upravil
        /// </summary>
        public string Uzivatel { get; set; }
    }
}
