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
    /// Interaction logic for DialogPridejSoutez.xaml
    /// </summary>
    public partial class DialogPridejSoutez : Window
    {
        private ObservableCollection<Soutez> soutezeData;
        private List<string> typySoutezi = new List<string>();

        public DialogPridejSoutez(ObservableCollection<Soutez> soutezeData)
        {
            InitializeComponent();

            this.soutezeData = soutezeData;

            NaplnCbSoutez();
        }

        /// <summary>
        /// Metoda vyresetuje Combobox a DatePicker
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = null;
            dpDatumStartuSouteze.SelectedDate = dpDatumKonceSouteze.SelectedDate = DateTime.Now;
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

            if(dpDatumStartuSouteze.SelectedDate.Value.Date > dpDatumKonceSouteze.SelectedDate.Value.Date)
            {
                throw new NonValidDataException("Datum začátku nemůže být později než datum konce!");
            }
        }

        /// <summary>
        /// Metoda slouží k přidání nové soutěže do tabulky a zároveň také do databáze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                Soutez pridanaSoutez = new Soutez();
                string? vybranyTypSouteze = cbSoutez.SelectedItem as String;
                if (vybranyTypSouteze != null && !String.IsNullOrEmpty(vybranyTypSouteze) && 
                    dpDatumStartuSouteze.SelectedDate.HasValue && dpDatumKonceSouteze.SelectedDate.HasValue)
                {
                    pridanaSoutez.TypSouteze = vybranyTypSouteze;
                    pridanaSoutez.StartDatum = DateOnly.FromDateTime(dpDatumStartuSouteze.SelectedDate.Value);
                    pridanaSoutez.KonecDatum = DateOnly.FromDateTime(dpDatumKonceSouteze.SelectedDate.Value);

                    var conn = DatabaseManager.GetConnection();
                    

                        // Nastavení přihlášeného uživatele pro logování
                        DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                        // Přidání soutěže
                        DatabaseSouteze.AddSoutez(conn, pridanaSoutez);
                        
                        // Získání ID ze stejné session
                        int? idSoutez = DatabaseSouteze.GetCurrentId(conn);
                        if (idSoutez == null)
                        {
                            throw new NullReferenceException("ID soutěže nemůže být NULL! Nastala chyba u spojení s databází...");
                        }

                        pridanaSoutez.IdSoutez = (int)idSoutez;

                        soutezeData.Add(pridanaSoutez);

                        MessageBox.Show("Soutěž byla úspěšně přidána!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    
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
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých soutěží
        /// </summary>
        private void NaplnCbSoutez()
        {
            try
            {
                var conn = DatabaseManager.GetConnection();

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
