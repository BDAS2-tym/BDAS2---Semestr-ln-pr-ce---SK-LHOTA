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
    /// Interaction logic for DialogPridejTrenink.xaml
    /// </summary>
    public partial class DialogPridejTrenink : Window
    {
        private const int MaxLimitZnaku = 30;

        public DialogPridejTrenink()
        {
            InitializeComponent();
            dtpDatumTreninku.Value = DateTime.Now;
        }

        /// <summary>
        /// Metoda vymaže textová pole a ComboBoxy
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
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
