using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno zajišťující první registraci uživatele do systému
    /// Uživatel je vždy zaregistrován s rolí "HOST"
    /// Provádí validaci vstupních dat, hashování hesla a zápis do databáze Oracle
    /// </summary>
    public partial class PrvniRegistracniOkno : Window
    {
        public PrvniRegistracniOkno()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ověří, zda má e-mail správný formát
        /// </summary>
        /// <param name="email">Zadaná e-mailová adresa</param>
        /// <returns>Vrací true, pokud má e-mail platný formát, jinak false</returns>
        private bool JeEmailValidni(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Událost pro tlačítko "Registrovat"
        /// Provádí validaci vstupů, hashování hesla a registraci uživatele do databáze jako HOST
        /// </summary>
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string uzivatelskeJmeno = txtUser.Text.Trim();
            string email = txtEmail.Text.Trim();
            string heslo = txtPass.Password.Trim();
            string heslo2 = txtPass2.Password.Trim();

            // Validace vstupů 
            if (string.IsNullOrEmpty(uzivatelskeJmeno) ||
                string.IsNullOrEmpty(heslo) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vyplňte všechna pole!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (uzivatelskeJmeno.Length < 3 || uzivatelskeJmeno.Length > 30)
            {
                MessageBox.Show("Uživatelské jméno musí mít 3–30 znaků", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo.Length < 8 || heslo.Length > 15)
            {
                MessageBox.Show("Heslo musí mít 8–15 znaků", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo != heslo2)
            {
                MessageBox.Show("Hesla se neshodují!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!JeEmailValidni(email))
            {
                MessageBox.Show("E-mail není ve správném formátu!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (uzivatelskeJmeno.Contains(" "))
            {
                MessageBox.Show("Uživatelské jméno nesmí obsahovat mezery!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Hash + Salt
                string salt = PasswordHasher.GenerateSalt();
                string hash = PasswordHasher.HashPassword(heslo, salt);

                // Nový účet jako HOST
                Uzivatel novyUzivatel = new Uzivatel();
                novyUzivatel.UzivatelskeJmeno = uzivatelskeJmeno;
                novyUzivatel.Email = email;
                novyUzivatel.Heslo = hash;
                novyUzivatel.Salt = salt;
                novyUzivatel.Role = "Host";
                novyUzivatel.PosledniPrihlaseni = DateTime.Now;

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Přidání uživatele
                    DatabaseRegistrace.AddUzivatel(conn, novyUzivatel);
                }

                MessageBox.Show($"Uživatel '{uzivatelskeJmeno}' byl úspěšně registrován jako 'HOST' ", "Registrace úspěšná", MessageBoxButton.OK, MessageBoxImage.Information);

                // Nastavení přihlášeného uživatele a otevření hlavního okna
                HlavniOkno.NastavPrihlaseneho(novyUzivatel);
                new HlavniOkno().Show();
                this.Close();
            }
            catch (OracleException ex)
            {
                // Ošetření duplicity a chyb Oracle
                if (ex.Number == 20002)
                {
                    MessageBox.Show("Toto uživatelské jméno už existuje!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                else
                {
                    MessageBox.Show("Chyba při registraci: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zavře registrační okno a otevře přihlašovací okno
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            PrihlaseniOkno prihlaseni = new PrihlaseniOkno(null);
            prihlaseni.Show();
            this.Close();
        }

        /// <summary>
        /// Minimalizuje aktuální okno registrace
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Umožní přesouvání okna podržením levého tlačítka myši
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
