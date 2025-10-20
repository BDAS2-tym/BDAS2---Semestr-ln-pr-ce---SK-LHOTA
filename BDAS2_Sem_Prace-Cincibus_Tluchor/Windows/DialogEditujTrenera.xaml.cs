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
    /// Interakční logika pro DialogEditujTrenera.xaml
    /// </summary>
    public partial class DialogEditujTrenera : Window
    {
        private TreneriOkno treneriOkno;
        private Trener editovanyTrener;

        public DialogEditujTrenera(Trener editovanyTrener, TreneriOkno treneriOkno)
        {
            InitializeComponent();
            this.editovanyTrener = editovanyTrener;
            this.treneriOkno = treneriOkno;

            tboxRodneCislo.Text = editovanyTrener.RodneCislo.ToString();
            tboxJmeno.Text = editovanyTrener.Jmeno;
            tboxPrijmeni.Text = editovanyTrener.Prijmeni;
            tboxTelCislo.Text = editovanyTrener.TelefonniCislo;
            tboxLicence.Text = editovanyTrener.TrenerskaLicence;
            tboxSpecializace.Text = editovanyTrener.Specializace;
            iudPraxe.Value = editovanyTrener.PocetLetPraxe;

        }

        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // --- VALIDACE RODNÉHO ČÍSLA ---
                if (!long.TryParse(tboxRodneCislo.Text.Trim(), out long rodneCislo))
                {
                    MessageBox.Show("Rodné číslo může obsahovat pouze číslice! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Délka rodného čísla (10 číslic)
                if (rodneCislo.ToString().Length != 10)
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic bez lomítka! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- VALIDACE TEXTOVÝCH POLÍ ---
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string licence = tblockLicence.Text.Trim();
                string specializace = tboxSpecializace.Text.Trim();


                if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(prijmeni) ||
                    string.IsNullOrWhiteSpace(telCislo) || string.IsNullOrWhiteSpace(licence))
                {
                    MessageBox.Show("Prosím vyplňte všechna povinná pole! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- VALIDACE ČÍSELNÝCH HODNOT ---
                int pocetLetPraxe = (int)iudPraxe.Value;


                if (pocetLetPraxe < 0)
                {
                    MessageBox.Show("Počet roků nesmí být záporná hodnota !", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- NASTAVENÍ HODNOT DO EDITOVANEHO TRENERA ---
                editovanyTrener.RodneCislo = rodneCislo;
                editovanyTrener.Jmeno = jmeno;
                editovanyTrener.Prijmeni = prijmeni;
                editovanyTrener.TelefonniCislo = telCislo;
                editovanyTrener.TrenerskaLicence = licence; 
                editovanyTrener.Specializace = specializace;    
                editovanyTrener.PocetLetPraxe = pocetLetPraxe;  

                // --- UPDATE V DATABÁZI ---
                DatabaseTreneri.UpdateTrener(editovanyTrener);

                // --- REFRESH DATAGRIDU ---
                treneriOkno.dgTreneri.Items.Refresh();

                MessageBox.Show("Trenér byl úspěšně editován! ", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání trenéra:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
