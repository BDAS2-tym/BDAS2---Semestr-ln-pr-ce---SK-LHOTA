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
        private HraciOkno hraciOkno;
     
        public DialogNajdiHrace(HraciOkno hraciOkno)
        {
            InitializeComponent();
            this.hraciOkno = hraciOkno;
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            bool nalezen = false;

            foreach (var hledanyHrac in hraciOkno.HraciData)
            {
                if (tboxRodneCislo.Text.Equals(hledanyHrac.RodneCislo.ToString()) && hledanyHrac != null)
                {
                    nalezen = true;
        
                    hraciOkno.dgHraci.SelectedItem = hledanyHrac;   

                    MessageBox.Show($"Nalezený hráč: {hledanyHrac.Jmeno} {hledanyHrac.Prijmeni} " +
                        $", Telefonní číslo: {hledanyHrac.TelefonniCislo}, Počet gólů: {hledanyHrac.PocetVstrelenychGolu}, " +
                        $"Pozice: {hledanyHrac.PoziceNaHristi}, Počet žlutých karet: {hledanyHrac.PocetZlutychKaret}, " +
                        $"Počet červených karet: {hledanyHrac.PocetCervenychKaret}",
                        "Dialog", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
            }
                 if(nalezen == false)
                {
                    MessageBox.Show("Hráč nebyl nalezen! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }

        }

        private void btnZrusit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
