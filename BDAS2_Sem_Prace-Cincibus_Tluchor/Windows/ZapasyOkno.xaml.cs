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
    /// Interakční logika pro ZapasyOkno.xaml
    /// </summary>
    public partial class ZapasyOkno : Window
    {
        private HlavniOkno hlavniOkno;
        public ZapasyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            hlavniOkno.Show();
            this.Close();
        }
    }
}
