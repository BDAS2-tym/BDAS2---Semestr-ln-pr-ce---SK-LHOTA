using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Reprezentuje uživatele systému
    /// Obsahuje všechny základní údaje potřebné pro přihlášení, roli v systému 
    /// </summary>
    public class Uzivatel
    {
        /// <summary>
        /// Unikátní uživatelské jméno, které slouží k přihlášení do systému
        /// </summary>
        public string UzivatelskeJmeno { get; set; }

        /// <summary>
        /// Zahashovaná podoba hesla uživatele
        /// Heslo se nikdy neukládá v čitelné (plain text) podobě
        /// </summary>
        public string Heslo { get; set; }

        /// <summary>
        /// Náhodně generovaný řetězec (salt) používaný při hashování hesla
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Role uživatele v systému
        /// Možné hodnoty: "Admin", "Trener", "Hrac", "Uzivatel", "Host"
        /// Určuje úroveň oprávnění v aplikaci
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Unikátní E-mailová adresa uživatele
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Datum a čas posledního přihlášení uživatele do systému
        /// Při nové registraci se nastaví na aktuální čas
        /// </summary>
        public DateTime PosledniPrihlaseni { get; set; }

        /// <summary>
        /// Rodné číslo uživatele
        /// Vyplňuje se pouze u rolí "hráč" a "trenér"
        /// </summary>
        public string RodneCislo { get; set; }

        /// <summary>
        /// Pro Notifikace/zprávy potřebné, jestli je vybrán uživatel v checkboxu nebo ne
        /// </summary>
        public bool JeVybran { get; set; }

    }
}
