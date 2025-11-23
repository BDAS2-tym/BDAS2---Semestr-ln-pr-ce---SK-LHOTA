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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Interaction logic for DialogNajdiKontrakt.xaml
    /// </summary>
    public partial class DialogNajdiKontrakt : Window
    {
        private ObservableCollection<Kontrakt> kontraktyData;
        private List<Hrac> hraci = new List<Hrac>();

        public IEnumerable<Kontrakt> VyfiltrovaneKontrakty { get; private set; }

        public DialogNajdiKontrakt(ObservableCollection<Kontrakt> kontraktyData)
        {
            InitializeComponent();

            this.kontraktyData = kontraktyData;
            NaplnCbHrac();
        }

        /// <summary>
        /// Metoda vyresetuje textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbHrac.SelectedItem = null;
            dpDatumZacatkuKontraktu.SelectedDate = dpDatumKonceKontraktu.SelectedDate = null;
            slPlat.LowerValue = slPlat.Minimum;
            slPlat.HigherValue = slPlat.Maximum;
            slVystupniKlauzule.LowerValue = slVystupniKlauzule.Minimum;
            slVystupniKlauzule.HigherValue = slVystupniKlauzule.Maximum;
        }

        /// <summary>
        /// Metoda slouží k vyfiltrování kontraktů a nastevení DialogResult na true
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaneKontrakty = FiltrujKontrakty();
                DialogResult = true;
                this.Close();
            }

            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při filtrování :\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých hráčů
        /// </summary>
        private void NaplnCbHrac()
        {
            try
            {
                hraci.Clear();

                kontraktyData.ToList().ForEach(kontrakt => hraci.Add(kontrakt.KontraktHrace));

                cbHrac.ItemsSource = hraci;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k filtrování všech hráčů podle zadaných kritérií
        /// </summary>
        /// <returns>Vrací kolekci IEnumerable vyfiltrovaných hráčů</returns>
        private IEnumerable<Kontrakt> FiltrujKontrakty()
        {
            if (dpDatumZacatkuKontraktu.SelectedDate != null && dpDatumKonceKontraktu.SelectedDate != null)
            {
                if (dpDatumZacatkuKontraktu.SelectedDate.Value > dpDatumKonceKontraktu.SelectedDate.Value)
                {
                    throw new NonValidDataException("Datum od nemůže být později než datum do!");
                }
            }

            if(!string.IsNullOrWhiteSpace(tboxCisloNaAgenta.Text) && !tboxCisloNaAgenta.Text.All(char.IsDigit))
            {
                throw new NonValidDataException("Telefonní číslo na agenta nemůže být NULL a musí se skládat jenom z číslic");
            }

            var vysledkyFiltrovani = kontraktyData.AsEnumerable();

            Hrac? vybranyHrac = cbHrac.SelectedItem as Hrac;

            // Hráč
            if (vybranyHrac != null)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.KontraktHrace != null &&
                    z.KontraktHrace.RodneCislo == vybranyHrac.RodneCislo);
            }

            // Plat
            vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                z.Plat >= (int)slPlat.LowerValue &&
                z.Plat <= (int)slPlat.HigherValue);

            // Výstupní klauzule
            vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                z.VystupniKlauzule >= (int)slVystupniKlauzule.LowerValue &&
                z.VystupniKlauzule <= (int)slVystupniKlauzule.HigherValue);

            // Tel. číslo agenta
            if (!string.IsNullOrWhiteSpace(tboxCisloNaAgenta.Text) && tboxCisloNaAgenta.Text.All(char.IsDigit))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.TelCisloNaAgenta.Contains(tboxCisloNaAgenta.Text, StringComparison.OrdinalIgnoreCase));
            }

            // Datum od
            if (dpDatumZacatkuKontraktu.SelectedDate.HasValue)
            {
                DateOnly datumOd = DateOnly.FromDateTime(dpDatumZacatkuKontraktu.SelectedDate.Value);

                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.DatumZacatku >= datumOd);
            }

            // Datum do
            if (dpDatumKonceKontraktu.SelectedDate.HasValue)
            {
                DateOnly datumDo = DateOnly.FromDateTime(dpDatumKonceKontraktu.SelectedDate.Value);

                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.DatumKonce <= datumDo);
            }

            return vysledkyFiltrovani;
        }
    }
}
