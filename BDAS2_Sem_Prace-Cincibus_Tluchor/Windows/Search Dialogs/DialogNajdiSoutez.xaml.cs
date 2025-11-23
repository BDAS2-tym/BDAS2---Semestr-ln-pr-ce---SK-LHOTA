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
    /// Interaction logic for DialogNajdiSoutez.xaml
    /// </summary>
    public partial class DialogNajdiSoutez : Window
    {
        private ObservableCollection<Soutez> soutezeData;

        public IEnumerable<Soutez> VyfiltrovaneSouteze { get; private set; }

        public DialogNajdiSoutez(ObservableCollection<Soutez> soutezeData)
        {
            InitializeComponent();

            NaplnCbSoutez();
            this.soutezeData = soutezeData;
        }

        /// <summary>
        /// Metoda vyresetuje textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = null;
            dpDatumKonceSouteze.SelectedDate = dpDatumZacatkuSouteze.SelectedDate = null;
        }

        /// <summary>
        /// Metoda slouží k vyfiltrování soutěží a nastevení DialogResult na true
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaneSouteze = FiltrujSouteze();
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
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých soutěží
        /// </summary>
        private void NaplnCbSoutez()
        {
            try
            {
                TypSouteze typSouteze = new TypSouteze();
                cbSoutez.ItemsSource = typSouteze.TypySoutezi;
                cbSoutez.DisplayMemberPath = "Value";
                cbSoutez.SelectedValuePath = "Key";
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soutěží:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k filtrování všech záznamů podle zadaných kritérií
        /// </summary>
        /// <returns>Vrací kolekci IEnumerable vyfiltrovaných záznamů</returns>
        private IEnumerable<Soutez> FiltrujSouteze()
        {
            if (dpDatumZacatkuSouteze.SelectedDate != null && dpDatumKonceSouteze.SelectedDate != null)
            {
                if (dpDatumZacatkuSouteze.SelectedDate > dpDatumKonceSouteze.SelectedDate)
                {
                    throw new NonValidDataException("Datum od nemůže být později než datum do!");
                }
            }

            var vysledkyFiltrovani = soutezeData.AsEnumerable();

            var dict = cbSoutez.ItemsSource as Dictionary<int, string>;
            int? vybraneId = cbSoutez.SelectedValue == null ? (int?)null : Convert.ToInt32(cbSoutez.SelectedValue);

            if (vybraneId != null && dict != null && dict.TryGetValue(vybraneId.Value, out string vybranyNazev))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.TypSouteze != null &&
                    z.TypSouteze.Contains(vybranyNazev, StringComparison.OrdinalIgnoreCase));
            }

            // Datum začátku
            if (dpDatumZacatkuSouteze.SelectedDate != null)
            {
                DateOnly datumZacatku = DateOnly.FromDateTime(dpDatumZacatkuSouteze.SelectedDate.Value);

                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.StartDatum >= datumZacatku);
            }

            // Datum konce
            if (dpDatumKonceSouteze.SelectedDate != null)
            {
                DateOnly datumKonce = DateOnly.FromDateTime(dpDatumKonceSouteze.SelectedDate.Value);

                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.KonecDatum <= datumKonce);
            }

            return vysledkyFiltrovani;
        }
    }
}
