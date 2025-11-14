using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
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
    /// Interaction logic for DialogPridejTrenink.xaml
    /// </summary>
    public partial class DialogPridejTrenink : Window
    {
        private const int MaxLimitZnaku = 30;

        public DialogPridejTrenink(ObservableCollection<Trener> treneri)
        {
            InitializeComponent();
            dtpDatumTreninku.Value = DateTime.Now;

            TreneriOkno.NactiTrenery();

            foreach (var trener in TreneriOkno.TreneriData)
            {
                cbTrener.Items.Add($" {trener.Prijmeni} ({trener.RodneCislo.ToString()})");
            }
        }

        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            //  Validace vstupů
            if (cbTrener.SelectedItem == null)
            {
                MessageBox.Show("Vyberte trenéra!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dtpDatumTreninku.Value == null)
            {
                MessageBox.Show("Zadejte datum tréninku!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(tboxMistoTreninku.Text))
            {
                MessageBox.Show("Zadejte místo tréninku!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string popisTreninku = new TextRange(rtboxPopisTreninku.Document.ContentStart, rtboxPopisTreninku.Document.ContentEnd).Text.Trim();

            if (popisTreninku.Length > MaxLimitZnaku)
            {
                MessageBox.Show($"Popis tréninku nesmí přesáhnout {MaxLimitZnaku} znaků ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string vybranyTrener = cbTrener.SelectedItem.ToString();
            string prijmeniTrenera = vybranyTrener.Split('(')[0].Trim();
            string rodneCisloString = vybranyTrener.Split('(', ')')[1].Trim();

            if (!long.TryParse(rodneCisloString, out long rodneCisloTrenera))
            {
                MessageBox.Show("Neplatný formát rodného čísla trenéra", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vytvoření nového objektu tréninku
            TreninkView novyTrenink = new TreninkView
            {
                Prijmeni = prijmeniTrenera,
                RodneCislo = rodneCisloTrenera,
                Datum = dtpDatumTreninku.Value.Value,
                Misto = tboxMistoTreninku.Text.Trim(),
                Popis = popisTreninku
            };

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Přidání tréninku
                    DatabaseTreninky.AddTrenink(conn, novyTrenink);

                    TreninkyOkno.TreninkyData.Add(novyTrenink);

                    this.DialogResult = true;
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při přidávání tréninku: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda vymaže textová pole a ComboBoxy
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            rtboxPopisTreninku.Document.Blocks.Clear();
            dtpDatumTreninku.Value = DateTime.Now;
            tboxMistoTreninku.Clear();
            cbTrener.SelectedItem = null;
        }

        private void rtboxPopisTreninku_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(rtboxPopisTreninku.Document.ContentStart, rtboxPopisTreninku.Document.ContentEnd).Text;

            // RichTextBox dává na konec 2 speciální znaky '\r \n', proto + 2
            if (text.Length > MaxLimitZnaku + 2)
            {
                // Kontrola rozmezí
                text = text.Substring(0, MaxLimitZnaku);
                rtboxPopisTreninku.Document.Blocks.Clear();
                rtboxPopisTreninku.Document.Blocks.Add(new Paragraph(new Run(text)));

                // Přesunutí pointeru na konec věty
                rtboxPopisTreninku.CaretPosition = rtboxPopisTreninku.Document.ContentEnd;
            }
        }
    }
}
