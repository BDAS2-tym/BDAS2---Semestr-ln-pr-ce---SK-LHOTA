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
    /// Interakční logika pro NastaveniOkno.xaml
    /// </summary>
    public partial class NastaveniOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        public NastaveniOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            this.hlavniOkno = hlavniOkno;
        }
        
        private void BtnBinarniObsah_Click(object sender, RoutedEventArgs e)
        {
            BinarniObsahOkno binarniObsahOkno = new BinarniObsahOkno();
            binarniObsahOkno.Show();
            this.Close();
        }

        private void BtnUzivatele_Click(object sender, RoutedEventArgs e)
        {
            NastaveniUzivateleOkno uzivateleOkno = new NastaveniUzivateleOkno();
            uzivateleOkno.Show();
            this.Close();
        }

        private void BtnSystemovyKatalog_Click(object sender, RoutedEventArgs e)
        {
            SystemovyKatalogOkno systemovyKatalogOkno = new SystemovyKatalogOkno();
            systemovyKatalogOkno.Show();
            this.Close();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            HlavniOkno hlavniOkno = new HlavniOkno();
            hlavniOkno.ShowDialog();
            this.Close();
        }

        private void btnZmeny_Click(object sender, RoutedEventArgs e)
        {
            LogTableOkno logTableOkno = new LogTableOkno(hlavniOkno);
            logTableOkno.Show();
            this.Close();
        }
    }
}
