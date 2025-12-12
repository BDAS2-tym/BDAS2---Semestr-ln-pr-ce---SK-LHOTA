using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

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
        /// Reakce na změnu výběru role v ComboBoxu – zobrazí/skrývá pole pro rodné číslo
        /// </summary>
        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRole.SelectedItem is ComboBoxItem selectedItem)
            {
                string role = selectedItem.Content.ToString().ToLowerInvariant();

                if (role.Contains("hráč") || role.Contains("trenér"))
                {
                    panelRodneCislo.Visibility = Visibility.Visible;
                }
                else
                {
                    panelRodneCislo.Visibility = Visibility.Collapsed;
                }
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
            string rodneCislo = txtRodneCislo.Text.Trim();

            ComboBoxItem selectedItem = (ComboBoxItem)cmbRole.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Vyberte roli!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string role = selectedItem.Content.ToString();

            if (string.IsNullOrEmpty(uzivatelskeJmeno) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(heslo))
            {
                MessageBox.Show("Vyplňte všechna povinná pole!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (uzivatelskeJmeno.Length < 3 || uzivatelskeJmeno.Length > 30)
            {
                MessageBox.Show("Uživatelské jméno musí mít 3–30 znaků.", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo.Length < 8 || heslo.Length > 15)
            {
                MessageBox.Show("Heslo musí mít 8–15 znaků.", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo != heslo2)
            {
                MessageBox.Show("Hesla se neshodují!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (email.Length > 100)
            {
                MessageBox.Show("E-mail je příliš dlouhý (max 100 znaků)", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("E-mail není ve správném formátu!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string normalizovanaRole =
                role.ToLowerInvariant()
                    .Replace("á", "a")
                    .Replace("é", "e")
                    .Replace("í", "i")
                    .Replace("ó", "o")
                    .Replace("ú", "u")
                    .Replace("ů", "u")
                    .Replace("č", "c")
                    .Replace("ř", "r")
                    .Replace("š", "s")
                    .Replace("ť", "t")
                    .Replace("ň", "n")
                    .Replace("ý", "y")
                    .Replace("ž", "z");

            bool roleVyzadujeRodneCislo = normalizovanaRole == "hrac" || normalizovanaRole == "trener";

            if (roleVyzadujeRodneCislo)
            {
                if (string.IsNullOrWhiteSpace(rodneCislo))
                {
                    MessageBox.Show("Pro roli Hráč/Trenér musíte zadat rodné číslo",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    Validator.ValidujRodneCislo(rodneCislo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                rodneCislo = null;
            }

            try
            {
                string salt = PasswordHasher.GenerateSalt();
                string hash = PasswordHasher.HashPassword(heslo, salt);

                Uzivatel novyUzivatel = new Uzivatel();
                novyUzivatel.UzivatelskeJmeno = uzivatelskeJmeno;
                novyUzivatel.Email = email;
                novyUzivatel.Heslo = hash;
                novyUzivatel.Salt = salt;
                novyUzivatel.Role = normalizovanaRole;
                novyUzivatel.PosledniPrihlaseni = DateTime.Now;
                novyUzivatel.RodneCislo = rodneCislo;

                OracleConnection conn = DatabaseManager.GetConnection();

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseRegistrace.AddUzivatel(conn, novyUzivatel);

                MessageBox.Show(
                    $"Uživatel '{uzivatelskeJmeno}' byl úspěšně registrován.",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20003)
                {
                    MessageBox.Show("Uživatelské jméno nebo email již existuje!", "Chyba",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20100)
                {
                    MessageBox.Show("Rodné číslo je povinné pro hráče/trenéra",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20101)
                {
                    MessageBox.Show("Člen s daným rodným číslem neexistuje v databázi",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20103)
                {
                    MessageBox.Show("Hráč/trenér již má vytvořený účet!",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Chyba při registraci: " + ex.Message, "Chyba",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message, "Chyba");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}