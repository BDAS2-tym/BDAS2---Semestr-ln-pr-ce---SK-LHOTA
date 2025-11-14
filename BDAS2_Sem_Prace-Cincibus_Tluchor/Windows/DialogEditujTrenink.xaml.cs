using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interakční logika pro DialogEditujTrenink.xaml
    /// </summary>
    public partial class DialogEditujTrenink : Window
    {
        private TreninkView editovanyTrenink;
        private TreninkyOkno treninkyOkno;
        private const int MaxLimitZnaku = 30;

        public DialogEditujTrenink(TreninkView trenink, TreninkyOkno treninkyOkno)
        {
            InitializeComponent();
            this.editovanyTrenink = trenink;
            this.treninkyOkno = treninkyOkno;

            // Načtení hodnot do formuláře
            dtpDatumTreninku.Value = editovanyTrenink.Datum;
            tboxMistoTreninku.Text = editovanyTrenink.Misto;

            // Nastavení textu do RichTextBoxu 
            rtboxPopisTreninku.Document.Blocks.Clear();
            rtboxPopisTreninku.Document.Blocks.Add(new Paragraph(new Run(editovanyTrenink.Popis)));

            // Naplnění trenérů
            TreneriOkno.NactiTrenery();
            cbTrener.Items.Clear();

            foreach (var trener in TreneriOkno.TreneriData)
            {
                string zaznam = $"{trener.Prijmeni} ({trener.RodneCislo})";
                cbTrener.Items.Add(zaznam);

                if (trener.RodneCislo == editovanyTrenink.RodneCislo)
                    cbTrener.SelectedItem = zaznam;
            }
        }

        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // VALIDACE
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
                if (popisTreninku.Length > 100)
                {
                    MessageBox.Show("Popis tréninku nesmí přesáhnout 100 znaků", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string vybranyTrener = cbTrener.SelectedItem.ToString();
                string prijmeniTrenera = vybranyTrener.Split('(')[0].Trim();
                string rcStr = vybranyTrener.Split('(', ')')[1].Trim();

                if (!long.TryParse(rcStr, out long rodneCisloTrenera))
                {
                    MessageBox.Show("Neplatný formát rodného čísla trenéra.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // AKTUALIZACE OBJEKTU
                editovanyTrenink.Prijmeni = prijmeniTrenera;
                editovanyTrenink.RodneCislo = rodneCisloTrenera;
                editovanyTrenink.Datum = dtpDatumTreninku.Value.Value;
                editovanyTrenink.Misto = tboxMistoTreninku.Text.Trim();
                editovanyTrenink.Popis = popisTreninku;


                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Editování soutěže
                    DatabaseTreninky.UpdateTrenink(conn, editovanyTrenink);

                    treninkyOkno.dgTreninky.Items.Refresh();

                    this.DialogResult = true;
                    MessageBox.Show("Trénink byl úspěšně upraven!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
               
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání tréninku:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
