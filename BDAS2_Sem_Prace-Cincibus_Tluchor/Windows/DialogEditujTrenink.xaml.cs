using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialog pro editaci existujícího tréninku
    /// Umožňuje změnit trenéra, datum, místo a popis
    /// Po potvrzení provádí validaci a ukládá změny do databáze
    /// </summary>
    public partial class DialogEditujTrenink : Window
    {
        private TreninkView editovanyTrenink;
        private TreninkyOkno treninkyOkno;
        private const int MaxPopis = 30;

        /// <summary>
        /// Inicializuje dialog a naplní vstupní pole hodnotami upravovaného tréninku
        /// </summary>
        public DialogEditujTrenink(TreninkView trenink, TreninkyOkno treninkyOkno)
        {
            InitializeComponent();

            this.editovanyTrenink = trenink;
            this.treninkyOkno = treninkyOkno;

            // Datum a místo
            dtpDatumTreninku.Value = editovanyTrenink.Datum;
            tboxMistoTreninku.Text = editovanyTrenink.Misto;

            // Popis (může být null)
            rtboxPopisTreninku.Document.Blocks.Clear();
            rtboxPopisTreninku.Document.Blocks.Add(new Paragraph(new Run(editovanyTrenink.Popis)));

            TreneriOkno.NactiTrenery();
            cbTrener.Items.Clear();

            foreach (var trener in TreneriOkno.TreneriData)
            {
                string zaznam = trener.Prijmeni + " (" + trener.RodneCislo + ")";
                cbTrener.Items.Add(zaznam);

                // Pokud je trenér shodný s tréninkem, vybere se
                if (trener.RodneCislo == editovanyTrenink.RodneCislo)
                {
                    cbTrener.SelectedItem = zaznam;
                }
            }
        }

        /// <summary>
        /// Zavře dialog bez uložení
        /// </summary>
        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Uloží upravený trénink po validaci vstupních dat
        /// Aktualizuje objekt i databázi a obnoví DataGrid v hlavním okně
        /// </summary>
        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validace trenéra
                if (cbTrener.SelectedItem == null)
                {
                    throw new Exception("Vyberte trenéra");
                }

                // Validace data
                Validator.ValidujDatum(dtpDatumTreninku.Value, "Datum tréninku");

                // Validace místa
                string misto = tboxMistoTreninku.Text.Trim();
                Validator.ValidujMistoTreninku(misto);

                // Načtení popisu z RichTextBoxu
                string popis = new TextRange(
                    rtboxPopisTreninku.Document.ContentStart,
                    rtboxPopisTreninku.Document.ContentEnd
                ).Text.Trim();

                Validator.ValidujPopisTreninku(popis);

                string vybrany = cbTrener.SelectedItem.ToString();
                string prijmeni = vybrany.Split('(')[0].Trim();
                string rcStr = vybrany.Split('(', ')')[1].Trim();

                // Validace rodného čísla trenéra přiřazeného k tréninku
                Validator.ValidujRodneCislo(rcStr);

                // Uložení změn do objektu editovaného tréninku
                editovanyTrenink.Prijmeni = prijmeni;
                editovanyTrenink.RodneCislo = rcStr;
                editovanyTrenink.Datum = dtpDatumTreninku.Value.Value;
                editovanyTrenink.Misto = misto;
                editovanyTrenink.Popis = popis;

                    // Uložení do databáze
                    var conn = DatabaseManager.GetConnection();
                
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseTreninky.UpdateTrenink(conn, editovanyTrenink);
                

                treninkyOkno.dgTreninky.Items.Refresh();

                MessageBox.Show("Trénink byl úspěšně upraven", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání tréninku:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Omezí text v poli popisu na maximální délku
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
