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
                    MessageBox.Show("Vyberte roli uživatele.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string novaRole = item.Content.ToString();

                if (string.IsNullOrWhiteSpace(jmeno))
                {
                    MessageBox.Show("Zadejte uživatelské jméno.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Zadejte e-mail.");
                    return;
                }

                bool roleVyžadujeRodneCislo = false;

                if (novaRole.Equals("Hrac", StringComparison.OrdinalIgnoreCase))
                {
                    roleVyžadujeRodneCislo = true;
                }

                if (novaRole.Equals("Trener", StringComparison.OrdinalIgnoreCase))
                {
                    roleVyžadujeRodneCislo = true;
                }

                // -------- VALIDACE PRO ROLE HRÁČ A TRENÉR --------

                if (roleVyžadujeRodneCislo)
                {
                    if (string.IsNullOrWhiteSpace(rodneCislo))
                    {
                        MessageBox.Show("Pro roli Hráč nebo Trenér musíte zadat rodné číslo.",
                            "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    using (var conn = DatabaseManager.GetConnection())
                    {
                        conn.Open();

                        string sql = @"
                            SELECT COUNT(*) 
                            FROM CLENOVE_KLUBU 
                            WHERE RODNE_CISLO = :rc
                            AND LOWER(TYPCLENA) = :typ";

                        using (var cmd = new OracleCommand(sql, conn))
                        {
                            string typClena;

                            if (novaRole.Equals("Hrac", StringComparison.OrdinalIgnoreCase))
                                typClena = "hrac";
                            else
                                typClena = "trener";


                            cmd.Parameters.Add(":rc", OracleDbType.Varchar2).Value = rodneCislo;
                            cmd.Parameters.Add(":typ", OracleDbType.Varchar2).Value = typClena;

                            int pocet = Convert.ToInt32(cmd.ExecuteScalar());

                            if (pocet == 0)
                            {
                                MessageBox.Show(
                                    $"Rodné číslo {rodneCislo} patří jinému typu člena.\n" +
                                    $"Pro roli {novaRole} musí být v databázi člen typu '{typClena}'.",
                                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);

                                return;
                            }
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(rodneCislo))
                    {
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
                }

                // -------- ULOŽENÍ UŽIVATELE --------

                uzivatel.UzivatelskeJmeno = jmeno;
                uzivatel.Email = email;
                uzivatel.RodneCislo = rodneCislo;
                uzivatel.Role = novaRole;

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    int idRole = Convert.ToInt32(item.Tag);
                    AktualizujRoliUzivatele(conn, stareJmeno, idRole);

                    if (!string.IsNullOrWhiteSpace(heslo))
                    {
                        string salt = PasswordHasher.GenerateSalt();
                        string hash = PasswordHasher.HashPassword(heslo, salt);
                        uzivatel.Heslo = hash;
                        uzivatel.Salt = salt;
                    }

                    DatabaseRegistrace.UpdateUzivatel(conn, uzivatel, stareJmeno);
                }

                MessageBox.Show("Změny byly úspěšně uloženy.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
