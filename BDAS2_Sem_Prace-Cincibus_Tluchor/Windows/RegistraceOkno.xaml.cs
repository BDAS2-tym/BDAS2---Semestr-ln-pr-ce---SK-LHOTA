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
            string rodneCislo = txtRodneCislo.Text.Trim();

            ComboBoxItem selectedItem = (ComboBoxItem)cmbRole.SelectedItem;

            // Kontrola role
            if (selectedItem == null)
            {
                MessageBox.Show("Vyberte roli!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string role = selectedItem.Content.ToString();

            // Validace
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

            // Uživatel vybere roli v ComboBoxu, např. "Hráč" nebo "Trenér"
            // Role převede na malá písmena + odstraní se diakritika v databázi je bez diakritiky
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

            try
            {
                // Validace rodného čísla, pokud je hrac nebo trener
                if (roleVyzadujeRodneCislo)
                {
                    if (string.IsNullOrWhiteSpace(rodneCislo))
                    {
                        MessageBox.Show("Pro roli Hráč/Trenér musíte zadat rodné číslo", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                    // Ověření typu člena
                    using (var conn = DatabaseManager.GetConnection())
                    {
                        conn.Open();

                        // Pokud COUNT(*) vrátí 1 - člen existuje
                        // Pokud COUNT(*) vrátí 0 - neexistuje člen se stejným rodným číslem a rolí
                        string sql =
                            "SELECT COUNT(*) FROM CLENOVE_KLUBU " +
                            "WHERE RODNE_CISLO = :rc AND LOWER(TYPCLENA) = :typ";

                        using (var cmd = new OracleCommand(sql, conn))
                        {
                            cmd.Parameters.Add(":rc", OracleDbType.Varchar2).Value = rodneCislo;
                            cmd.Parameters.Add(":typ", OracleDbType.Varchar2).Value = normalizovanaRole;

                            int pocet = Convert.ToInt32(cmd.ExecuteScalar());

                            if (pocet == 0)
                            {
                                MessageBox.Show(
                                    "Rodné číslo nepatří členu typu '" + normalizovanaRole + "'.",
                                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }

                // HASH + SALT
                string salt = PasswordHasher.GenerateSalt();
                string hash = PasswordHasher.HashPassword(heslo, salt);

                // Vytvoření objektu uživatele
                Uzivatel novyUzivatel = new Uzivatel();
                novyUzivatel.UzivatelskeJmeno = uzivatelskeJmeno;
                novyUzivatel.Email = email;
                novyUzivatel.Heslo = hash;
                novyUzivatel.Salt = salt;
                novyUzivatel.Role = normalizovanaRole;
                novyUzivatel.PosledniPrihlaseni = DateTime.Now;

                if (roleVyzadujeRodneCislo)
                {
                    novyUzivatel.RodneCislo = rodneCislo;
                }
                else
                {
                    novyUzivatel.RodneCislo = null;
                }

                // Uložení do DB
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseRegistrace.AddUzivatel(conn, novyUzivatel);
                }

                MessageBox.Show(
                    "Uživatel '" + uzivatelskeJmeno + "' byl úspěšně registrován", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20002)
                {
                    MessageBox.Show("Uživatelské jméno již existuje!", "Chyba",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Chyba při registraci: " + ex.Message, "Chyba",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message);
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
