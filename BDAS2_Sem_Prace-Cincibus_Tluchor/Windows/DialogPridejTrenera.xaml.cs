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
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialog pro přidání nového trenéra
    /// Umožňuje vyplnit osobní údaje, licenci a praxi a vložit trenéra do databáze
    /// </summary>
    public partial class DialogPridejTrenera : Window
    {
      
        private ObservableCollection<Trener> TreneriData;

        public DialogPridejTrenera(ObservableCollection<Trener> TreneriData)
        {
            InitializeComponent();
            this.TreneriData = TreneriData;

            // Výchozí hodnota počet let praxe
            iudPraxe.Value = 1;
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
            iudPraxe.Value = 1;
        }

        /// <summary>
        /// Ověří vstupní data a pokusí se vytvořit a přidat nového trenéra do databáze a datagridu
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vstupy
                string rodneCislo = tboxRodneCislo.Text.Trim();
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string licence = tboxLicence.Text.Trim();
                string specializace = tboxSpecializace.Text.Trim(); // nepovinné
                string praxeText = iudPraxe.Value.ToString();

                // Validace
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);
                Validator.ValidujTrenerskouLicenci(licence);
                Validator.ValidujPocetLetPraxeTrenera(praxeText);
                Validator.ValidujSpecializaciTrenera(specializace);

                int praxe = (int)iudPraxe.Value;

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Kontrola duplicity rodného čísla
                    if (Validator.ExistujeRodneCislo(conn, rodneCislo))
                    {
                        MessageBox.Show("Trenér s tímto rodným číslem již existuje!", "Duplicitní rodné číslo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Vytvoření trenéra
                    Trener novyTrener = new Trener(
                        jmeno,
                        prijmeni,
                        rodneCislo,        
                        telCislo,
                        licence,
                        specializace,
                        praxe
                    );

                    // Vložení do databáze
                    DatabaseTreneri.AddTrener(conn, novyTrener);

                    // Vložení do datagridu
                    TreneriData.Add(novyTrener);
                }

                MessageBox.Show("Trenér byl úspěšně přidán", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při přidávání trenéra:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
