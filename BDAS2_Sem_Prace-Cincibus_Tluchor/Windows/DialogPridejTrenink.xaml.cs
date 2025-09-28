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
            tboxId.Clear();
            dtpDatumTreninku.Value = DateTime.Now;
            tboxMistoTreninku.Clear();
            cbTrener.SelectedItem = null;
        }
    }
}
