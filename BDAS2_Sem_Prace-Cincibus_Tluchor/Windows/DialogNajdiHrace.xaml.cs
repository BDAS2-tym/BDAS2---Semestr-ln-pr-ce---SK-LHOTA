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
    /// Interakční logika pro DialogNajdiHrace.xaml
    /// </summary>
    public partial class DialogNajdiHrace : Window
    {
        public DialogNajdiHrace()
        {
            InitializeComponent();
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
