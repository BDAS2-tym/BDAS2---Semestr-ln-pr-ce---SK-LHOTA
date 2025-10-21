using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DialogPridejTrenera.xaml
    /// </summary>
    public partial class DialogPridejTrenera : Window
    {

        private ObservableCollection<Trener> TreneriData;

        public DialogPridejTrenera(ObservableCollection<Trener> TreneriData)
        {
            InitializeComponent();
            this.TreneriData = TreneriData;
        }

        /// <summary>
        /// Metoda vymaže textová pole a resetuje IntegerUpDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxRodneCislo.Clear();
            tboxJmeno.Clear();
            tboxPrijmeni.Clear();
            tboxTelCislo.Clear();
            tboxLicence.Clear();
            tboxSpecializace.Clear();
            iudPraxe.Value = 0;
        }

        /// <summary>
        /// Ověří vstupní data a pokusí se vytvořit a přidat nového trenéra.
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // --- VALIDACE VSTUPŮ ---
                if (!long.TryParse(tboxRodneCislo.Text, out long rodneCislo))
                {
                    MessageBox.Show("Rodné číslo může obsahovat pouze číslice ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error
                    );
                    return;
                }
                string jmeno = tboxJmeno.Text;
                string prijmeni = tboxPrijmeni.Text;
                string telCislo = tboxTelCislo.Text;
                string licence = tboxLicence.Text;
                string specializace = tboxSpecializace.Text; // Specializace může být prázdná
                int praxe = (int)iudPraxe.Value; // Hodnota může být null

                if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(prijmeni) || string.IsNullOrWhiteSpace(telCislo) || 
                    string.IsNullOrWhiteSpace(licence))
                {
                    MessageBox.Show("Prosím vyplňte všechna povinná pole správně ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!telCislo.All(char.IsDigit))
                {
                    MessageBox.Show("Telefonní číslo může obsahovat pouze číslice! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kontrola záporných hodnot počet let praxe
                if (praxe < 0)
                {
                    MessageBox.Show("Počet let praxe je povinný údaj a nesmí být záporný! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error
                    );
                    return;
                }

                // Délka rodného čísla (10 číslic)
                if (rodneCislo.ToString().Length != 10)
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic bez lomítka! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vytvoření nového trenéra
                Trener novyTrener = new Trener(jmeno, prijmeni, rodneCislo, "Trener", telCislo, licence, specializace, praxe);

                DatabaseTreneri.AddTrener(novyTrener); // Přidání do databáze

                // Přidání trenéra do kolekce
                TreneriData.Add(novyTrener);

                MessageBox.Show("Trenér byl úspěšně přidán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při přidávání trenéra: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
