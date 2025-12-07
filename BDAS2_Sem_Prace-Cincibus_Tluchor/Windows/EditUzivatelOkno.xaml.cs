using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno umožňující úpravu uživatelského účtu
    /// Zahrnuje validaci údajů, změnu role, práci s rodným číslem a uložení dat do databáze
    /// </summary>
    public partial class EditUzivatelOkno : Window
    {
        /// <summary>
        /// Objekt aktuálně editovaného uživatele
        /// </summary>
        private Uzivatel uzivatel;

        /// <summary>
        /// Původní uživatelské jméno nutné pro update v databázi
        /// </summary>
        private string stareJmeno;

        /// <summary>
        /// Inicializace okna a naplnění údajů
        /// </summary>
        public EditUzivatelOkno(Uzivatel editovanyUzivatel)
        {
            InitializeComponent();
            uzivatel = editovanyUzivatel;
            stareJmeno = editovanyUzivatel.UzivatelskeJmeno;

            NactiRoleZDB();
            NaplnFormular();
            AktualizujZamceniRodnehoCisla();
        }

        /// <summary>
        /// Načte role z databáze a naplní ComboBox
        /// </summary>
        private void NactiRoleZDB()
        {
            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT IDROLE, NAZEVROLE FROM ROLE_VIEW ORDER BY IDROLE", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = reader["NAZEVROLE"].ToString();
                        item.Tag = Convert.ToInt32(reader["IDROLE"]);
                        cmbRole.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání rolí\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Naplní textová pole daty o uživateli
        /// </summary>
        private void NaplnFormular()
        {
            txtUser.Text = uzivatel.UzivatelskeJmeno;
            txtEmail.Text = uzivatel.Email;
            txtRodneCislo.Text = uzivatel.RodneCislo;

            foreach (ComboBoxItem item in cmbRole.Items)
            {
                if (item.Content.ToString().ToLower() == uzivatel.Role.ToLower())
                {
                    cmbRole.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Uzamyká nebo odemyká rodné číslo textbox podle role
        /// </summary>
        private void AktualizujZamceniRodnehoCisla()
        {
            ComboBoxItem item = cmbRole.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                return;
            }

            string role = item.Content.ToString().ToLower();

            if (role == "hrac" || role == "trener")
            {
                txtRodneCislo.IsEnabled = true;
                txtRodneCislo.Opacity = 1;
            }
            else
            {
                txtRodneCislo.IsEnabled = false;
                txtRodneCislo.Opacity = 0.4;
                txtRodneCislo.Text = "";
            }
        }

        /// <summary>
        /// Při změně role aktualizuje nastavení textboxu rodného čísla
        /// </summary>
        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AktualizujZamceniRodnehoCisla();
        }

        /// <summary>
        /// Uloží změny upravovaného uživatele
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jmeno = txtUser.Text.Trim();
                string email = txtEmail.Text.Trim();
                string rodneCislo = txtRodneCislo.Text.Trim();
                string heslo = txtHeslo.Text.Trim();

                ComboBoxItem item = cmbRole.SelectedItem as ComboBoxItem;
                if (item == null)
                {
                    MessageBox.Show("Musíte vybrat roli uživatele", "Varování", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string novaRole = item.Content.ToString().ToLower();

                if (jmeno == "" || jmeno.Contains(" ") || jmeno.Length < 3)
                {
                    MessageBox.Show("Neplatné uživatelské jméno", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool emailOK = System.Text.RegularExpressions.Regex.IsMatch(
                    email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");

                if (!emailOK)
                {
                    MessageBox.Show("Zadejte platný e-mail", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool roleMusiZadatRodneCislo = novaRole == "hrac" || novaRole == "trener";

                if (roleMusiZadatRodneCislo)
                {
                    if (rodneCislo == "")
                    {
                        MessageBox.Show("Pro roli hráč nebo trenér musíte zadat rodné číslo", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Validator.ValidujRodneCislo(rodneCislo);
                }
                else
                {
                    rodneCislo = null;
                }

                uzivatel.UzivatelskeJmeno = jmeno;
                uzivatel.Email = email;
                uzivatel.RodneCislo = rodneCislo;
                uzivatel.Role = novaRole;

                if (heslo != "")
                {
                    string salt = PasswordHasher.GenerateSalt();
                    string hash = PasswordHasher.HashPassword(heslo, salt);

                    uzivatel.Heslo = hash;
                    uzivatel.Salt = salt;
                }

                OracleConnection conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                int idRole = Convert.ToInt32(item.Tag);

                DatabaseRegistrace.UpdateUzivatel(conn, uzivatel, stareJmeno);

                MessageBox.Show("Změny byly úspěšně uloženy", "Uloženo", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20100)
                {
                    MessageBox.Show("Pro hráče/trenéra je rodné číslo povinné",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20101)
                {
                    MessageBox.Show("Člen s tímto rodným číslem neexistuje",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20102)
                {
                    MessageBox.Show("Typ člena nesedí (musí být hráč / trenér)",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20103)
                {
                    MessageBox.Show("Hráč/trenér už má vytvořený účet",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Databázová chyba\n" + ex.Message,
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání dat\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zavře okno
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Minimalizuje okno
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Umožní přesouvat okno myší
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
