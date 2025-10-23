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
    /// Interakční logika pro DialogNajdiTrenera.xaml
    /// </summary>
    public partial class DialogNajdiTrenera : Window
    {
        private TreneriOkno treneriOkno;

        public DialogNajdiTrenera(TreneriOkno treneriOkno)
        {
            InitializeComponent();
            this.treneriOkno = treneriOkno;
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            bool nalezen = false;

            foreach (var hledanyTrener in TreneriOkno.TreneriData)
            {
                if (tboxRodneCislo.Text.Equals(hledanyTrener.RodneCislo.ToString()) && hledanyTrener != null)
                {
                    nalezen = true;

                    treneriOkno.dgTreneri.SelectedItem = hledanyTrener;

                    MessageBox.Show($"Nalezený trenér: {hledanyTrener.Jmeno} {hledanyTrener.Prijmeni} " +
                        $", Telefonní číslo: {hledanyTrener.TelefonniCislo}, Specializace: {hledanyTrener.Specializace}, " +
                        $"Počet let praxe: {hledanyTrener.PocetLetPraxe}", "Dialog", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
            }
            if (nalezen == false)
            {
                MessageBox.Show("Trenér nebyl nalezen! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }

        }

        private void BtnZrusit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
