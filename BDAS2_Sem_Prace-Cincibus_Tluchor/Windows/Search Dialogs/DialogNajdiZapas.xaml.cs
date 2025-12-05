using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DialogNajdiZapas.xaml
    /// </summary>
    public partial class DialogNajdiZapas : Window
    {
        private ObservableCollection<Zapas> zapasyData;
        private List<string> hoste = new List<string>();
        private List<string> domaci = new List<string>();
        private const string HintText = "např. 3:2";

        public IEnumerable<Zapas> VyfiltrovaneZapasy { get; private set; }

        public DialogNajdiZapas(ObservableCollection<Zapas> zapasyData)
        {
            InitializeComponent();

            this.zapasyData = zapasyData;
            NaplnCbSoutez();
            NaplnCbStav();
            NaplnCbDomaciTym();
            NaplnCbHosteTym();
            
            tboxVysledek.Text = HintText;
            tboxVysledek.Foreground = Brushes.Gray;

            /* Vlastní čárkovaná čára pod textem */
            var underline = new TextDecoration
            {
                Location = TextDecorationLocation.Underline,
                Pen = new Pen(Brushes.DimGray, 1)
                {
                    DashStyle = DashStyles.Dash
                },
                PenThicknessUnit = TextDecorationUnit.FontRecommended
            };

            tblockVysledek.TextDecorations = new TextDecorationCollection { underline };
        }

        /// <summary>
        /// Metoda slouží k odebrání hint textu a zapnutí normálního textu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void TboxVysledek_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tboxVysledek.Text == HintText)
            {
                tboxVysledek.Text = "";
                tboxVysledek.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Metoda slouží k přidání hint textu při ztracení focusu na textovém poli
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void TboxVysledek_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxVysledek.Text))
            {
                tboxVysledek.Text = HintText;
                tboxVysledek.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Metoda vyresetuje textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = cbDomaciTym.SelectedItem = cbHosteTym.SelectedItem = cbStav.SelectedItem = null;
            tboxVysledek.Clear();
            tboxVysledek.Text = HintText;
            tboxVysledek.Foreground = Brushes.Gray;
            dpDatumOd.SelectedDate = dpDatumDo.SelectedDate = null;
        }

        /// <summary>
        /// Metoda slouží k vyfiltrování zápasů a nastevení DialogResult na true
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaneZapasy = FiltrujZapasy();
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
                using var conn = DatabaseManager.GetConnection();
                conn.Open();
                TypSouteze typSouteze = new TypSouteze(conn);
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
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých stavů zápasů
        /// </summary>
        private void NaplnCbStav()
        {           
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();
                StavZapasu stav = new StavZapasu(conn);
                cbStav.ItemsSource = stav.StavyZapasu;
                cbStav.DisplayMemberPath = "Value";
                cbStav.SelectedValuePath = "Key";
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání stavů zápasů:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých týmů hostů
        /// </summary>
        private void NaplnCbHosteTym()
        {
            try
            {
                hoste = zapasyData
                    .Where(s => !string.IsNullOrEmpty(s.HosteTym))
                    .Select(s => s.HosteTym!)
                    .Distinct()
                    .ToList();

                cbHosteTym.ItemsSource = hoste;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hosté týmů:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých týmů domácích
        /// </summary>
        private void NaplnCbDomaciTym()
        {
            try
            {
                domaci = zapasyData
                    .Where(s => !string.IsNullOrEmpty(s.DomaciTym))
                    .Select(s => s.DomaciTym!)
                    .Distinct()         
                    .ToList();

                cbDomaciTym.ItemsSource = domaci;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání domácích týmů:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k filtrování všech zápasů podle zadaných kritérií
        /// </summary>
        /// <returns>Vrací kolekci IEnumerable vyfiltrovaných zápasů</returns>
        private IEnumerable<Zapas> FiltrujZapasy()
        {
            if (!string.IsNullOrEmpty(tboxVysledek.Text) && (string.Compare(tboxVysledek.Text, HintText, StringComparison.OrdinalIgnoreCase)) != 0)
            {
                if (!Regex.IsMatch(tboxVysledek.Text, @"^[0-9]{1,2}:[0-9]{1,2}$"))
                {
                    throw new NonValidDataException("Výsledek není ve validním formátu!");
                }
            }

            var vysledkyFiltrovani = zapasyData.AsEnumerable();

            // Soutěž
            if (cbSoutez.SelectedValue is int vybraneIdSouteze)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Soutez != null &&
                    z.Soutez.IdSoutez == vybraneIdSouteze);
            }

            // Stav zápasu
            if (cbStav.SelectedValue is int vybraneIdStavu)
            {
                // Získáme text podle ID
                var stavDict = cbStav.ItemsSource as Dictionary<int, string>;
                string vybranyStav = stavDict[vybraneIdStavu];

                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    !string.IsNullOrWhiteSpace(z.StavZapasu) &&
                    z.StavZapasu.Equals(vybranyStav, StringComparison.OrdinalIgnoreCase));
            }

            // Domácí tým
            if (cbDomaciTym.SelectedItem is string vybranyDomaciTym)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    !string.IsNullOrWhiteSpace(z.DomaciTym) &&
                    z.DomaciTym.Equals(vybranyDomaciTym, StringComparison.OrdinalIgnoreCase));
            }

            // Hosté tým
            if (cbHosteTym.SelectedItem is string vybranyHosteTym)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    !string.IsNullOrWhiteSpace(z.HosteTym) &&
                    z.HosteTym.Equals(vybranyHosteTym, StringComparison.OrdinalIgnoreCase));
            }

            // Datum od
            if (dpDatumOd.SelectedDate is DateTime datumOd)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Datum.Date >= datumOd.Date);
            }

            // Datum do
            if (dpDatumDo.SelectedDate is DateTime datumDo)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Datum.Date <= datumDo.Date);
            }

            // Výsledek
            if (!string.IsNullOrWhiteSpace(tboxVysledek.Text) &&
                !tboxVysledek.Text.Equals(HintText, StringComparison.OrdinalIgnoreCase))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    !string.IsNullOrWhiteSpace(z.Vysledek) &&
                    z.Vysledek.Contains(tboxVysledek.Text, StringComparison.OrdinalIgnoreCase));
            }

            return vysledkyFiltrovani;
        }
    }
}
