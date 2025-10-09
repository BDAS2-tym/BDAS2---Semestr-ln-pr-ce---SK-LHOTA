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
    /// Interaction logic for DialogPridejZapas.xaml
    /// </summary>
    public partial class DialogPridejZapas : Window
    {
        public DialogPridejZapas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda vyresetuje Combobox a DateTimePicker
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = cbStavZapasu.SelectedItem = null;
            dtpDatumZapasu.Value = DateTime.Now;
        }
    }
}
