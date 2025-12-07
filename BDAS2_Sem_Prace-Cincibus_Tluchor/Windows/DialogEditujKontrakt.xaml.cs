using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
using System.Windows.Xps;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogEditujKontrakt.xaml
    /// </summary>
    public partial class DialogEditujKontrakt : Window
    {
        private Kontrakt editovanyKontrakt;
        private KontraktyOkno kontraktyOkno;
        private List<Hrac> hraci = new List<Hrac>();
        private const int MinimalniMzda = 20800;
        private const int MinHraniceTelCisla = 9;
        private const int MaxHraniceTelCisla = 12;

        public DialogEditujKontrakt(Kontrakt editovanyKontrakt, KontraktyOkno kontraktyOkno)
        {
            InitializeComponent();

            // Nastavení DataContextu
            DataContext = this;
            NaplnCbHrac();

            this.editovanyKontrakt = editovanyKontrakt;
            this.kontraktyOkno = kontraktyOkno;

            tboxPlat.Text = editovanyKontrakt.Plat.ToString();
            tboxTelCisloAgenta.Text = editovanyKontrakt.TelCisloNaAgenta;
            tboxVystupniKlauzule.Text = editovanyKontrakt.VystupniKlauzule.ToString();
            cbHrac.SelectedItem = hraci.FirstOrDefault(hrac => hrac.IdClenKlubu == editovanyKontrakt.KontraktHrace.IdClenKlubu);
            dpDatumZacatkuKontraktu.SelectedDate = editovanyKontrakt.DatumZacatku.ToDateTime(TimeOnly.MinValue);
            dpDatumKonceKontraktu.SelectedDate = editovanyKontrakt.DatumKonce.ToDateTime(TimeOnly.MinValue);
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
        /// Metoda slouží k nastavení datamu do dpDatumKonce, pokaždé když se změní datum v dpDatumZacatek
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dpDatumZacatkuKontraktu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDatumZacatkuKontraktu.SelectedDate.HasValue)
                dpDatumKonceKontraktu.SelectedDate = dpDatumZacatkuKontraktu.SelectedDate.Value.AddYears(1);
        }

        /// <summary>
        /// Metoda slouží k editaci vybraného kontrakt z tabulky a zároveň také v databázi
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                Hrac? vybranyHrac = cbHrac.SelectedItem as Hrac;
                if(vybranyHrac != null)
                {
                    editovanyKontrakt.Plat = Convert.ToInt32(tboxPlat.Text);
                    editovanyKontrakt.DatumZacatku = DateOnly.FromDateTime(dpDatumZacatkuKontraktu.SelectedDate.Value);
                    editovanyKontrakt.DatumKonce = DateOnly.FromDateTime(dpDatumKonceKontraktu.SelectedDate.Value);
                    editovanyKontrakt.TelCisloNaAgenta = tboxTelCisloAgenta.Text;
                    editovanyKontrakt.VystupniKlauzule = Convert.ToInt32(tboxVystupniKlauzule.Text);
                    editovanyKontrakt.KontraktHrace = vybranyHrac;

                    var conn = DatabaseManager.GetConnection();

                        // Nastavení přihlášeného uživatele pro logování
                        DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                        // Editování soutěže
                        DatabaseKontrakty.UpdateKontrakt(conn, editovanyKontrakt);

                        kontraktyOkno.dgKontrakty.Items.Refresh();

                        MessageBox.Show("Kontrakt byl úspěšně editován!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    
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
            if (!int.TryParse(tboxPlat.Text, out int resultPlat))
            {
                throw new NonValidDataException("Plat není celé číslo!");
            }

            if (resultPlat < MinimalniMzda)
            {
                throw new NonValidDataException($"Plat musí být minimálně ve výši minimální mzdy {MinimalniMzda} !");
            }

            if (tboxTelCisloAgenta.Text.Length < MinHraniceTelCisla || tboxTelCisloAgenta.Text.Length > MaxHraniceTelCisla)
            {
                throw new NonValidDataException($"Telefonní číslo musí být v rozmezí {MinHraniceTelCisla} a {MaxHraniceTelCisla} !");
            }

            if (!tboxTelCisloAgenta.Text.All(char.IsDigit))
            {
                throw new NonValidDataException("Telefonní číslo se musí skládat pouze z číslic!");
            }

            if (!int.TryParse(tboxVystupniKlauzule.Text, out int resultKlauzule))
            {
                throw new NonValidDataException("Výstupní klauzule není celé číslo!");
            }

            if (resultKlauzule < 0)
            {
                throw new NonValidDataException("Výstupní klauzule nemůže být záporná !");
            }

            if (cbHrac.SelectedItem == null)
            {
                throw new NonValidDataException("Vybraný hráč nemůže být NULL!");
            }

            if (dpDatumZacatkuKontraktu.SelectedDate == null || dpDatumKonceKontraktu.SelectedDate == null)
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
    }
}
