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
    /// Interaction logic for DialogPridejKontrakt.xaml
    /// </summary>
    public partial class DialogPridejKontrakt : Window
    {
        public DialogPridejKontrakt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda vymaže textová pole a resetuje IntegerUpDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxId.Clear();
            iudPocetLetKontraktu.Value = 0;
            tboxPlat.Clear();
            tboxTelCisloAgenta.Clear();
            tboxVystupniKlauzule.Clear();
            cbHrac.SelectedItem = null;
        }
    }
}
