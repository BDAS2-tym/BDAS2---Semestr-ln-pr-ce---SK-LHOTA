using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro úpravu uživatele – umožňuje editovat údaje 
    /// </summary>
    public partial class EditUzivatelOkno : Window
    {
        private Uzivatel uzivatel;
        private string stareJmeno;

        /// <summary>
        /// Konstruktor – přijme uživatele, jehož data se budou upravovat
        /// </summary>
        /// <param name="u">Objekt uživatele pro úpravu</param>
        public EditUzivatelOkno(Uzivatel editovanyUzivatel)
        {
            InitializeComponent();
            this.uzivatel = editovanyUzivatel;
            this.stareJmeno = editovanyUzivatel.UzivatelskeJmeno;
            NactiRoleZDB();
            NaplnFormular();
        }

        /// <summary>
        /// Načte dostupné role z tabulky ROLE a naplní combobox
        /// </summary>
        private void NactiRoleZDB()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT IDROLE, NAZEVROLE FROM ROLE ORDER BY IDROLE", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = reader["NAZEVROLE"].ToString(),
                        Tag = Convert.ToInt32(reader["IDROLE"])
                    };
                    cmbRole.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání rolí: " + ex.Message);
            }
        }

        /// <summary>
        /// Naplní formulář aktuálními údaji o uživateli
        /// </summary>
        private void NaplnFormular()
        {
            txtUser.Text = uzivatel.UzivatelskeJmeno;
            txtEmail.Text = uzivatel.Email;
            txtRodneCislo.Text = uzivatel.RodneCislo;

            // Nastavení aktuální role podle názvu
            foreach (ComboBoxItem item in cmbRole.Items)
            {
                if (item.Content.ToString().Equals(uzivatel.Role, StringComparison.OrdinalIgnoreCase))
                {
                    cmbRole.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Ověří formát rodného čísla – musí mít 10 číslic nebo může být prázdné
        /// </summary>
        /// <param name="rodneCislo">Rodné číslo zadané uživatelem</param>
        /// <returns>True, pokud je rodné číslo platné, jinak false</returns>
        private bool OverRodneCislo(string rodneCislo)
        {
            if (string.IsNullOrEmpty(rodneCislo))
                return true;

            if (rodneCislo.Length != 10)
                return false;

            // Každý znak musí být číslo 0–9
            foreach (char c in rodneCislo)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Zpracuje kliknutí na tlačítko "Uložit změny".
        /// Ověří vstupy, volitelně vytvoří nové heslo (hash + salt),
        /// a uloží změny do databáze.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jmeno = txtUser.Text.Trim();
                string email = txtEmail.Text.Trim();
                string rodneCislo = txtRodneCislo.Text.Trim();
                string heslo = txtHeslo.Text.Trim();

                if (jmeno == "")
                {
                    MessageBox.Show("Zadej uživatelské jméno!");
                    return;
                }

                if (email == "")
                {
                    MessageBox.Show("Zadej e-mail!");
                    return;
                }

                if (!OverRodneCislo(rodneCislo))
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic!");
                    return;
                }

                uzivatel.UzivatelskeJmeno = jmeno;
                uzivatel.Email = email;
                uzivatel.RodneCislo = rodneCislo;

                // Uložení zvolené role – ID i text
                if (cmbRole.SelectedItem is ComboBoxItem selectedRole)
                {
                    uzivatel.Role = selectedRole.Content.ToString();

                    // aktualizace role v databázi
                    int idRole = Convert.ToInt32(selectedRole.Tag);
                    AktualizujRoliUzivatele(uzivatel.UzivatelskeJmeno, idRole);
                }

                if (heslo != "")
                {
                    string salt = PasswordHasher.GenerateSalt();
                    string hash = PasswordHasher.HashPassword(heslo, salt);
                    uzivatel.Heslo = hash;
                    uzivatel.Salt = salt;
                }


                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                // NASTAVÍ PŘIHLÁŠENÉHO UŽIVATELE PRO TRIGGERY
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                // Provede update
                DatabaseRegistrace.UpdateUzivatel(conn, uzivatel, stareJmeno);

                MessageBox.Show("Změny byly uloženy");
                DialogResult = true;
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        /// <summary>
        /// Aktualizuje ID role daného uživatele v databázi
        /// </summary>
        private void AktualizujRoliUzivatele(string uzivatelskeJmeno, int idRole)
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                string sql = "UPDATE UZIVATELSKE_UCTY SET IDROLE = :idrole WHERE UZIVATELSKEJMENO = :jmeno";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":idrole", OracleDbType.Int32).Value = idRole;
                cmd.Parameters.Add(":jmeno", OracleDbType.Varchar2).Value = uzivatelskeJmeno;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při změně role: " + ex.Message);
            }
        }

        /// <summary>
        /// Zavře okno po kliknutí na tlačítko "Zavřít"
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Minimalizuje okno po kliknutí na tlačítko "-"
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Umožní okno tažením myší po pozadí
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
