using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    
    public partial class PridejTrenera : Window
    {
        public PridejTrenera()
        {
            InitializeComponent();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxId.Clear();
            tboxJmeno.Clear();
            tboxPrijmeni.Clear();
            tboxTelCislo.Clear();
            tboxLicence.Clear();
            tboxSpecializace.Clear();
            iudPraxe.Value = 0;
        }
    }
}
