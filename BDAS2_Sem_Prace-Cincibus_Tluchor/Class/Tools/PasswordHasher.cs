using System;
using System.Security.Cryptography;
using System.Text;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools
{
    /// <summary>
    /// Pomocná třída pro práci s hesly – generuje salt, vytváří hash a ověřuje hesla
    /// 
    /// Salt = náhodný řetězec, který se přidává k heslu, aby nebylo možné snadno uhodnout hash
    /// Hash = otisk hesla, který nelze zpětně převést na původní text (heslo)
    /// SHA256 = algoritmus, který z hesla a saltu udělá jedinečný otisk
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Vygeneruje náhodný salt
        /// </summary>
        /// <param name="velikost">Velikost saltu v bajtech (výchozí 32)</param>
        /// <returns>Salt jako text</returns>
        public static string GenerateSalt(int velikost = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[velikost];
                randomNumberGenerator.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        /// <summary>
        /// Vytvoří hash z hesla a saltu pomocí SHA256
        /// </summary>
        /// <param name="heslo">Zadané heslo</param>
        /// <param name="salt">Salt uživatele</param>
        /// <returns>Vrací hash hesla</returns>
        public static string HashPassword(string heslo, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var kombinace = Encoding.UTF8.GetBytes(heslo + salt);
                var hash = sha256.ComputeHash(kombinace);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Ověří, jestli zadané heslo odpovídá uloženému hashi
        /// </summary>
        /// <param name="zadaneHeslo">Heslo zadané uživatelem</param>
        /// <param name="ulozenyHash">Hash uložený v databázi</param>
        /// <param name="ulozenySalt">Salt uložený v databázi</param>
        /// <returns>True, pokud heslo sedí, jinak false</returns>
        public static bool VerifyPassword(string zadaneHeslo, string ulozenyHash, string ulozenySalt)
        {
            string hashZadaneho = HashPassword(zadaneHeslo, ulozenySalt);
            return hashZadaneho == ulozenyHash;
        }
    }
}
