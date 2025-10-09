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
using Xceed.Wpf.Toolkit;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogPridejOpatreni.xaml
    /// </summary>
    public partial class DialogPridejOpatreni : Window
    {
        private const int MaxLimitZnaku = 20;

        public DialogPridejOpatreni()
        {
            InitializeComponent();
            dpDatumOpatreni.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Metoda vymaže textová pole a resetuje IntegerUpDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxId.Clear();
            dpDatumOpatreni.SelectedDate = DateTime.Now;
            iudDelkaTrestu.Value = 0;
            rtboxDuvodTrestu.Document.Blocks.Clear();
            cbHrac.SelectedItem = null;
        }

        private void rtboxDuvodTrestu_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(rtboxDuvodTrestu.Document.ContentStart, rtboxDuvodTrestu.Document.ContentEnd).Text;

            // RichTextBox dává na konec 2 speciální znaky '\r \n', proto + 2
            if (text.Length > MaxLimitZnaku + 2)
            {
                // Kontrola rozmezí
                text = text.Substring(0, MaxLimitZnaku);
                rtboxDuvodTrestu.Document.Blocks.Clear();
                rtboxDuvodTrestu.Document.Blocks.Add(new Paragraph(new Run(text)));

                // Přesunutí pointeru na konec věty
                rtboxDuvodTrestu.CaretPosition = rtboxDuvodTrestu.Document.ContentEnd;
            }
        }
    }
}
