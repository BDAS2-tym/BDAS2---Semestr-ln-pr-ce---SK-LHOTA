using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
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
    /// Interaction logic for DialogNajdiSponzora.xaml
    /// </summary>
    public partial class DialogNajdiSponzora : Window
    {
        private ObservableCollection<Sponzor> sponzoriData;
        private List<ClenKlubu> clenove = new List<ClenKlubu>();

        public IEnumerable<Sponzor> VyfiltrovaniSponzori { get; private set; }


        public DialogNajdiSponzora(ObservableCollection<Sponzor> sponzoriData)
        {
            InitializeComponent();

            this.sponzoriData = sponzoriData;
            NaplnCbSoutez();
            NaplnCbClen();
        }

        /// <summary>
        /// Metoda vyresetuje textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = cbClen.SelectedItem = null;
            tboxJmenoSponzora.Clear();
            slCastka.LowerValue = slCastka.Minimum;
            slCastka.HigherValue = slCastka.Maximum;
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
                VyfiltrovaniSponzori = FiltrujSponzori();
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
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých členů
        /// </summary>
        private void NaplnCbClen()
        {
            try
            {
                clenove = sponzoriData
                        .Where(s => s.SponzorovaniClenove != null)
                        .SelectMany(s => s.SponzorovaniClenove!)
                        .DistinctBy(c => c.IdClenKlubu)
                        .ToList();

                cbClen.ItemsSource = clenove;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání členů:\n{ex.Message}",
                    "Chyba načítání", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k filtrování všech sponzorů podle zadaných kritérií
        /// </summary>
        /// <returns>Vrací kolekci IEnumerable vyfiltrovaných sponzorů</returns>
        private IEnumerable<Sponzor> FiltrujSponzori()
        {
            var vysledkyFiltrovani = sponzoriData.AsEnumerable();

            ClenKlubu? vybranyClen = cbClen.SelectedItem as ClenKlubu;
            var dict = cbSoutez.ItemsSource as Dictionary<int, string>;
            int? vybraneId = cbSoutez.SelectedValue == null ? (int?)null : Convert.ToInt32(cbSoutez.SelectedValue);

            // Soutěž
            if (vybraneId != null && dict != null && dict.TryGetValue(vybraneId.Value, out string vybranyNazev))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.SponzorovaneSouteze != null &&
                    z.SponzorovaneSouteze.Any(soutez =>
                    soutez.TypSouteze != null &&
                    soutez.TypSouteze.Contains(vybranyNazev, StringComparison.OrdinalIgnoreCase)
                    )
                 );
            }

            // Člen
            if (vybranyClen != null)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.SponzorovaniClenove != null &&
                    z.SponzorovaniClenove.Any(clen =>
                    clen.IdClenKlubu == vybranyClen.IdClenKlubu
                    )
                 );
            }

            // Jméno sponzora
            if (!string.IsNullOrEmpty(tboxJmenoSponzora.Text))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Jmeno.Contains(tboxJmenoSponzora.Text, StringComparison.OrdinalIgnoreCase));
            }

            // Sponzorovaná částka
            vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                z.SponzorovanaCastka >= (int)slCastka.LowerValue &&
                z.SponzorovanaCastka <= (int)slCastka.HigherValue);

            return vysledkyFiltrovani;
        }
    }
}
