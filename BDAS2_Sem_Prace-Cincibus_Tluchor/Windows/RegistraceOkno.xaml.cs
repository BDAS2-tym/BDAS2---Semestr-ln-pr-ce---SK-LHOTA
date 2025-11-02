using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro přidání nového uživatele do systému ADMINEM!
    /// Zajišťuje validaci vstupů, hashování hesla a zápis do databáze Oracle
    /// </summary>
    public partial class RegistraceOkno : Window
    {
        /// <summary>
        /// Inicializuje nové okno pro registraci uživatele
        /// </summary>
        public RegistraceOkno()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Povolen pouze zadání číslic 0–9 v poli pro rodné číslo
        /// </summary>
        private void TxtRodneCislo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                MessageBox.Show("Rodné číslo může obsahovat pouze číslice!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Upozorní, pokud uživatel zadá více než 10 číslic do pole pro rodné číslo
        /// </summary>
        private void TxtRodneCislo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtRodneCislo.Text.Length > 10)
            {
                MessageBox.Show("Rodné číslo může mít maximálně 10 číslic!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Ověří, zda zadaný e-mail odpovídá platnému formátu (např. jmeno@domena.cz)
        /// </summary>
        /// <param name="email">E-mailová adresa zadaná uživatelem.</param>
        /// <returns>True, pokud je e-mail ve správném formátu, jinak false</returns>
        private bool JeEmailValidni(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// Reakce na změnu výběru role v ComboBoxu – zobrazí/skrývá pole pro rodné číslo.
        /// </summary>
        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRole.SelectedItem is ComboBoxItem selectedItem)
            {
                string role = selectedItem.Content.ToString().ToLowerInvariant();

                // rodné číslo se zobrazuje pouze u hráče a trenéra
                if (role.Contains("hráč") || role.Contains("trenér"))
                    panelRodneCislo.Visibility = Visibility.Visible;
                else
                    panelRodneCislo.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Zpracuje registraci nového uživatele po kliknutí na tlačítko "Registrovat"
        /// </summary>
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string uzivatelskeJmeno = txtUser.Text.Trim();
            string email = txtEmail.Text.Trim();
            string heslo = txtPass.Password.Trim();
            string heslo2 = txtPass2.Password.Trim();

            ComboBoxItem selectedItem = (ComboBoxItem)cmbRole.SelectedItem;

            // Kontrola výběru role
            if (selectedItem == null)
            {
                MessageBox.Show("Vyberte roli!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string role = selectedItem.Content.ToString();
            string rodneCislo = txtRodneCislo.Text.Trim();

            // --- Validace vstupů ---
            if (string.IsNullOrEmpty(uzivatelskeJmeno) || string.IsNullOrEmpty(heslo) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vyplňte všechna povinná pole!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (uzivatelskeJmeno.Length < 3 || uzivatelskeJmeno.Length > 30)
            {
                MessageBox.Show("Uživatelské jméno musí mít v rozsahu 3–30 znaků", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo.Length < 8 || heslo.Length > 15)
            {
                MessageBox.Show("Heslo musí mít 8–15 znaků!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.Equals(heslo, heslo2, StringComparison.Ordinal))
            {
                MessageBox.Show("Hesla se neshodují!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (email.Length > 100)
            {
                MessageBox.Show("E-mail je příliš dlouhý (max. 100 znaků)", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!JeEmailValidni(email))
            {
                MessageBox.Show("E-mail není ve správném formátu!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // U hráče nebo trenéra ověř existenci podle rodného čísla, jestli je v databázi Oracle

                // Převádí roli na malá písmena 'Hráč' -> 'hrac', 'Trenér' -> 'trener'
                role = role.ToLowerInvariant().Replace("á", "a").Replace("é", "e").Replace("č", "c");

                if (role == "hrac" || role == "trener")
                {
                    if (string.IsNullOrEmpty(rodneCislo))
                    {
                        MessageBox.Show("Zadejte rodné číslo!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool existuje = DatabaseRegistrace.OverClenaPodleRodnehoCisla(rodneCislo, role);
                    if (!existuje)
                    {
                        MessageBox.Show($"Tento {role} s rodným číslem {rodneCislo} nebyl nalezen v databázi!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Hash + Salt hesla
                string salt = PasswordHasher.GenerateSalt();
                string hash = PasswordHasher.HashPassword(heslo, salt);

                // Vytvoření nového objektu uživatele
                Uzivatel novyUzivatel = new Uzivatel();
                novyUzivatel.UzivatelskeJmeno = uzivatelskeJmeno;
                novyUzivatel.Heslo = hash;
                novyUzivatel.Salt = salt;
                novyUzivatel.Role = role;
                novyUzivatel.Email = email;
                novyUzivatel.PosledniPrihlaseni = DateTime.Now;

                // Ulož rodné číslo jen pokud má význam
                if (role == "hrac" || role == "trener")
                {
                    novyUzivatel.RodneCislo = rodneCislo;
                }
                else
                {
                    novyUzivatel.RodneCislo = null;
                }

                // Uložení uživatele do databáze
                DatabaseRegistrace.AddUzivatel(novyUzivatel);

                MessageBox.Show($"Uživatel '{uzivatelskeJmeno}' byl úspěšně registrován jako '{role}'.", "Registrace úspěšná", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (OracleException ex)
            {
                // Ošetření duplicitního uživatelského jména
                if (ex.Number == 20002)
                    MessageBox.Show("Toto uživatelské jméno již existuje!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show("Chyba při registraci: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zavře registrační okno po kliknutí na tlačítko zavřít
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Minimalizuje registrační okno
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Umožní tažení okna podržením levého tlačítka myši
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
