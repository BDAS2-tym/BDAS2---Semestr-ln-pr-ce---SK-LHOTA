using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialog pro přidání nového tréninku
    /// </summary>
    public partial class DialogPridejTrenink : Window
    {
        private const int MaxPopis = 30;

        /// <summary>
        /// Inicializuje dialog a naplní combobox dostupnými trenéry
        /// </summary>
        /// <param name="treneri">Kolekce trenérů pro výběr trenéra tréninku</param
        public DialogPridejTrenink(ObservableCollection<Trener> treneri)
        {
            InitializeComponent();

            // Nastavení aktuálního data jako výchozí hodnoty
            dtpDatumTreninku.Value = DateTime.Now;

            // Naplnění comboboxu trenéry
            TreneriOkno.NactiTrenery();
            cbTrener.Items.Clear();

            foreach (var trener in TreneriOkno.TreneriData)
            {
                cbTrener.Items.Add(trener.Prijmeni + " (" + trener.RodneCislo + ")");
            }
        }

        /// <summary>
        /// Potvrdí přidání tréninku po provedení validace vstupů
        /// Vytvoří nový objekt tréninku a uloží jej do databáze i do kolekce pro DataGrid
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kontrola, zda je vybrán trenér
                if (cbTrener.SelectedItem == null)
                {
                    throw new Exception("Vyberte trenéra");
                }

                // Validace data
                Validator.ValidujDatum(dtpDatumTreninku.Value, "Datum tréninku");

                // Validace místa
                string misto = tboxMistoTreninku.Text.Trim();
                Validator.ValidujMistoTreninku(misto);

                // Získání textu popisu z RichTextBoxu
                string popis = new TextRange(
                    rtboxPopisTreninku.Document.ContentStart,
                    rtboxPopisTreninku.Document.ContentEnd
                ).Text.Trim();

                // Validace popisu tréninku
                Validator.ValidujPopisTreninku(popis);

                // Zpracování vybraného trenéra z comboboxu
                string vybrany = cbTrener.SelectedItem.ToString();
                string prijmeni = vybrany.Split('(')[0].Trim();
                string rcString = vybrany.Split('(', ')')[1].Trim();

                // Validace rodného čísla trenéra
                Validator.ValidujRodneCislo(rcString);

                // Vytvoření nového tréninku
                TreninkView novyTrenink = new TreninkView();
                novyTrenink.Prijmeni = prijmeni;
                novyTrenink.RodneCislo = rcString;
                novyTrenink.Datum = dtpDatumTreninku.Value.Value;
                novyTrenink.Misto = misto;
                novyTrenink.Popis = popis;

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseTreninky.AddTrenink(conn, novyTrenink);

                    TreninkyOkno.TreninkyData.Add(novyTrenink);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message, "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Vymaže všechna vstupní pole
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            rtboxPopisTreninku.Document.Blocks.Clear();
            dtpDatumTreninku.Value = DateTime.Now;
            tboxMistoTreninku.Clear();
            cbTrener.SelectedItem = null;
        }

        /// <summary>
        /// Omezí délku textu v popisu na maximální limit
        /// </summary>
        private void rtboxPopisTreninku_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(
                rtboxPopisTreninku.Document.ContentStart,
                rtboxPopisTreninku.Document.ContentEnd
            ).Text;

            // Kontrola délky textu MaxPopis znaků
            // plus 2 znaky, které RichTextBox přidává pro konec odstavce "\r\n")
            if (text.Length > MaxPopis + 2)
            {
                text = text.Substring(0, MaxPopis);
                rtboxPopisTreninku.Document.Blocks.Clear();
                rtboxPopisTreninku.Document.Blocks.Add(new Paragraph(new Run(text)));
                rtboxPopisTreninku.CaretPosition = rtboxPopisTreninku.Document.ContentEnd;
            }
        }
    }
}
