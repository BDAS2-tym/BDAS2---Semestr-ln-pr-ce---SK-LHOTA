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
    /// Interaction logic for DialogPridejKontrakt.xaml
    /// </summary>
    public partial class DialogPridejKontrakt : Window
    {
        private ObservableCollection<Kontrakt> kontraktyData;
        private List<Hrac> hraci = new List<Hrac>();
        private const int MinimalniMzda = 20800;
        private const int MinHraniceTelCisla = 9;
        private const int MaxHraniceTelCisla = 12;

        public DialogPridejKontrakt(ObservableCollection<Kontrakt> kontraktyData)
        {
            InitializeComponent();

            this.kontraktyData = kontraktyData;
            NaplnCbHrac();
        }

        /// <summary>
        /// Metoda vymaže textová pole a resetuje IntegerUpDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxPlat.Clear();
            tboxTelCisloAgenta.Clear();
            tboxVystupniKlauzule.Clear();
            cbHrac.SelectedItem = null;
            dpDatumKonceKontraktu.SelectedDate = DateTime.Now;
            dpDatumZacatkuKontraktu.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if(!int.TryParse(tboxPlat.Text, out int resultPlat))
            {
                throw new NonValidDataException("Plat není celé číslo!");
            }

            if(resultPlat < MinimalniMzda)
            {
                throw new NonValidDataException($"Plat musí být minimálně ve výši minimální mzdy {MinimalniMzda} !");
            }

            if(tboxTelCisloAgenta.Text.Length < MinHraniceTelCisla || tboxTelCisloAgenta.Text.Length > MaxHraniceTelCisla)
            {
                throw new NonValidDataException($"Telefonní číslo musí být v rozmezí {MinHraniceTelCisla} a {MaxHraniceTelCisla} !");
            }

            if(!tboxTelCisloAgenta.Text.All(char.IsDigit))
            {
                throw new NonValidDataException("Telefonní číslo se musí skládat pouze z číslic!");
            }

            if(!int.TryParse(tboxVystupniKlauzule.Text, out int resultKlauzule))
            {
                throw new NonValidDataException("Výstupní klauzule není celé číslo!");
            }

            if(resultKlauzule < 0)
            {
                throw new NonValidDataException("Výstupní klauzule nemůže být záporná !");
            }

            if(cbHrac.SelectedItem == null)
            {
                throw new NonValidDataException("Vybraný hráč nemůže být NULL!");
            }

            if(dpDatumZacatkuKontraktu.SelectedDate == null || dpDatumKonceKontraktu.SelectedDate == null)
            {
                throw new NonValidDataException("Vybrané datum nemůže být NULL!");
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých hráčů
        /// </summary>
        private void NaplnCbHrac()
        {
            try
            {
                var conn = DatabaseManager.GetConnection();

                using var cmd = new OracleCommand("SELECT * FROM HRACI_OPATRENI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                hraci.Clear();

                while (reader.Read())
                {
                    Hrac hrac = new Hrac();

                    // IDCLENKLUBU - NOT NULL
                    if (reader["IDCLENKLUBU"] != DBNull.Value)
                        hrac.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);

                    // RODNE_CISLO - NOT NULL
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        hrac.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        hrac.RodneCislo = "";


                    // JMENO - NOT NULL
                    if (reader["JMENO"] != DBNull.Value)
                        hrac.Jmeno = reader["JMENO"].ToString();
                    else
                        hrac.Jmeno = "";

                    // PRIJMENI - NOT NULL
                    if (reader["PRIJMENI"] != DBNull.Value)
                        hrac.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        hrac.Prijmeni = "";

                    hraci.Add(hrac);
                }

                cbHrac.ItemsSource = hraci;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k nastavení datamu do dpDatumKonce, pokaždé když se změní datum v dpDatumZacatek
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dpDatumZacatkuKontraktu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dpDatumZacatkuKontraktu.SelectedDate.HasValue)
                dpDatumKonceKontraktu.SelectedDate = dpDatumZacatkuKontraktu.SelectedDate.Value.AddYears(1);
        }

        /// <summary>
        /// Metoda slouží k přidání nového kontraktu do tabulky a zároveň také do databáze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                Kontrakt pridanyKontrakt = new Kontrakt();
                Hrac? vybranyHrac = cbHrac.SelectedItem as Hrac;
                if(vybranyHrac != null)
                {                  
                    pridanyKontrakt.KontraktHrace = vybranyHrac;
                    pridanyKontrakt.IdClena = vybranyHrac.IdClenKlubu;
                    pridanyKontrakt.DatumZacatku = DateOnly.FromDateTime(dpDatumZacatkuKontraktu.SelectedDate.Value);
                    pridanyKontrakt.DatumKonce = DateOnly.FromDateTime(dpDatumKonceKontraktu.SelectedDate.Value);
                    pridanyKontrakt.Plat = Convert.ToInt32(tboxPlat.Text);
                    pridanyKontrakt.TelCisloNaAgenta = tboxTelCisloAgenta.Text;
                    pridanyKontrakt.VystupniKlauzule = Convert.ToInt32(tboxVystupniKlauzule.Text);

                    var conn = DatabaseManager.GetConnection();
                    

                        // Nastavení přihlášeného uživatele pro logování
                        DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                        // Přidání hráče
                        DatabaseKontrakty.AddKontrakt(conn, pridanyKontrakt);

                        kontraktyData.Add(pridanyKontrakt);
                    

                    MessageBox.Show("Kontrakt byl úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
