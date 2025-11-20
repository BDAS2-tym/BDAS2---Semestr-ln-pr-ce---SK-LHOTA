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

            // Nastaví combobox na aktuální roli
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
        private bool OverRodneCislo(string rodneCislo)
        {
            if (string.IsNullOrEmpty(rodneCislo))
                return true;

            if (rodneCislo.Length != 10)
                return false;

            return rodneCislo.All(char.IsDigit);
        }

        /// <summary>
        /// Tlačítko "Uložit změny"
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Připravíme data z formuláře
                string jmeno = txtUser.Text.Trim();
                string email = txtEmail.Text.Trim();
                string rodneCislo = txtRodneCislo.Text.Trim();
                string heslo = txtHeslo.Text.Trim();

                if (string.IsNullOrEmpty(jmeno))
                {
                    MessageBox.Show("Zadej uživatelské jméno!");
                    return;
                }

                if (string.IsNullOrEmpty(email))
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

                // 🔥 Vytvoříme JEDNO připojení pro celou operaci
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                // Nastavíme přihlášeného uživatele pro triggery
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                // Pokud uživatel změnil roli – uložíme ji hned
                if (cmbRole.SelectedItem is ComboBoxItem selectedRole)
                {
                    uzivatel.Role = selectedRole.Content.ToString();

                    int idRole = Convert.ToInt32(selectedRole.Tag);

                    // ✔ Správné předání připojení
                    AktualizujRoliUzivatele(conn, uzivatel.UzivatelskeJmeno, idRole);
                }

                // Pokud zadal nové heslo – uděláme hash + salt
                if (!string.IsNullOrEmpty(heslo))
                {
                    string salt = PasswordHasher.GenerateSalt();
                    string hash = PasswordHasher.HashPassword(heslo, salt);
                    uzivatel.Heslo = hash;
                    uzivatel.Salt = salt;
                }

                // Aktualizace databáze přes uloženou proceduru
                DatabaseRegistrace.UpdateUzivatel(conn, uzivatel, stareJmeno);

                MessageBox.Show("Změny byly uloženy.");
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        /// <summary>
        /// Aktualizuje ID role uživatele — používá EXISTUJÍCÍ spojení
        /// </summary>
        private void AktualizujRoliUzivatele(OracleConnection conn, string uzivatelskeJmeno, int idRole)
        {
            try
            {
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

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
