using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Data.SqlClient;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class TreneriOkno : Window
    {
        private HlavniOkno hlavniOkno;

   

        // Kolekce trenérů pro DataGrid
        public ObservableCollection<Trener> Treneri { get; set; } = new ObservableCollection<Trener>();

        public TreneriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
    

        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

       
   

    }
}
