using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogEditujSoutez.xaml
    /// </summary>
    public partial class DialogEditujSoutez : Window
    {
        private List<string> typySoutezi = new List<string>();
        private Soutez editovanaSoutez;
        private SoutezeOkno soutezeOkno;

        public DialogEditujSoutez(Soutez editovanaSoutez, SoutezeOkno soutezeOkno)
        {
            InitializeComponent();

            // Nastavení DataContextu
            DataContext = this;
            NaplnCbSoutez();

            this.editovanaSoutez = editovanaSoutez;
            this.soutezeOkno = soutezeOkno;

            cbSoutez.SelectedItem = typySoutezi.FirstOrDefault(typ => typ.Equals(editovanaSoutez.TypSouteze, StringComparison.InvariantCultureIgnoreCase));
            dpDatumStartuSouteze.SelectedDate = editovanaSoutez.StartDatum.ToDateTime(TimeOnly.MinValue);
            dpDatumKonceSouteze.SelectedDate = editovanaSoutez.KonecDatum.ToDateTime(TimeOnly.MinValue);
        }

        /// <summary>
        /// Metoda slouží k zavření dialogového okna
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k editaci vybrané soutěže z tabulky a zároveň také v databázi
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                string? vybranyTypSouteze = cbSoutez.SelectedItem as String;
                if (vybranyTypSouteze != null && !String.IsNullOrEmpty(vybranyTypSouteze) &&
                    dpDatumStartuSouteze.SelectedDate.HasValue && dpDatumKonceSouteze.SelectedDate.HasValue)
                {
                    editovanaSoutez.TypSouteze = vybranyTypSouteze;
                    editovanaSoutez.StartDatum = DateOnly.FromDateTime(dpDatumStartuSouteze.SelectedDate.Value);
                    editovanaSoutez.KonecDatum = DateOnly.FromDateTime(dpDatumKonceSouteze.SelectedDate.Value);

                    using (var conn = DatabaseManager.GetConnection())
                    {
                        conn.Open();

                        // Nastavení přihlášeného uživatele pro logování
                        DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                        // Editování soutěže
                        DatabaseSouteze.UpdateSoutez(conn, editovanaSoutez);

                        soutezeOkno.dgSouteze.Items.Refresh();

                        MessageBox.Show("Soutěž byla úspěšně editována!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                this.Close();
            }

            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if (cbSoutez.SelectedItem == null)
            {
                throw new NonValidDataException("Vybraný typ soutěže nemůže být NULL!");
            }

            if (dpDatumKonceSouteze.SelectedDate == null || dpDatumStartuSouteze.SelectedDate == null)
            {
                throw new NonValidDataException("Vybrané datum začátku ani konce nemůže být NULL!");
            }

            if (dpDatumStartuSouteze.SelectedDate.Value.Date > dpDatumKonceSouteze.SelectedDate.Value.Date)
            {
                throw new NonValidDataException("Datum začátku nemůže být později než datum konce!");
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých soutěží
        /// </summary>
        private void NaplnCbSoutez()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM TYP_SOUTEZ_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                typySoutezi.Clear();

                while (reader.Read())
                {
                    // NAZEVSOUTEZE - NOT NULL
                    if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                        typySoutezi.Add(reader["NAZEVSOUTEZE"].ToString());
                    else
                        typySoutezi.Add("Nenalezeno!");
                }

                cbSoutez.ItemsSource = typySoutezi;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání typu soutěží:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
