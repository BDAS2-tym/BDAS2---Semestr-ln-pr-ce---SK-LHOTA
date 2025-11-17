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
    /// Interakční logika pro SystemovyKatalogOkno.xaml
    /// </summary>
    public partial class SystemovyKatalogOkno : Window
    {
        public List<SystemovyObjekt> KatalogData { get; set; }
        private HlavniOkno hlavniOkno;

        public SystemovyKatalogOkno()
        {
            InitializeComponent();

            KatalogData = DatabaseSystemovyKatalog.GetSystemoveObjekty();
            DataContext = this;
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
            this.Close();
        }
    }
}