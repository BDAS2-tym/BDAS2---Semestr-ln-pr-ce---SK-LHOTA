using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogPridejHrace : Window
    {
        private ObservableCollection<Hrac> HraciData;

        public DialogPridejHrace(ObservableCollection<Hrac> HraciData)
        {
            InitializeComponent();
            this.HraciData = HraciData;

            // Naplnění ComboBoxu pro pozice hráčů
            cbPozice.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };
            cbPozice.SelectedIndex = 0; // Výchozí hodnota "Brankář"
        }

        // Button pro Reset všech polí
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxRodneCislo.Clear();
            tboxJmeno.Clear();
            tboxPrijmeni.Clear();
            tboxTelCislo.Clear();
            cbPozice.SelectedIndex = 0;
            iudPocetCervenychKaret.Value =
            iudPocetGolu.Value =
            iudPocetZlutychKaret.Value = 0;
        }

        // Button přidání hráče přes dialog do datagridu a databáze
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // --- VALIDACE VSTUPŮ ---
                if (!long.TryParse(tboxRodneCislo.Text, out long rodneCislo))
                {
                    MessageBox.Show(
                        "Rodné číslo může obsahovat pouze číslice.",
                        "Chyba",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                string jmeno = tboxJmeno.Text;
                string prijmeni = tboxPrijmeni.Text;
                string telCislo = tboxTelCislo.Text;
                string pozice = cbPozice.SelectedItem.ToString();
                int pocetGolu = (int)iudPocetGolu.Value;
                int pocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                int pocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                if (string.IsNullOrWhiteSpace(jmeno) ||string.IsNullOrWhiteSpace(prijmeni) ||
                    string.IsNullOrWhiteSpace(telCislo) || string.IsNullOrWhiteSpace(pozice))
                {
                    MessageBox.Show("Prosím vyplňte všechna pole správně.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Délka rodného čísla (10 číslic)
                if (rodneCislo.ToString().Length != 10)
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic bez lomítka! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kontrola záporných hodnot počtů gólů a karet
                if (pocetGolu < 0 || pocetZlutychKaret < 0 || pocetCervenychKaret < 0)
                {
                    MessageBox.Show(
                        "Počet gólů a karet nesmí být záporné hodnoty!",
                        "Chyba",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                // Vytvoření nového hráče
                Hrac novyHrac = new Hrac(
                    rodneCislo, jmeno, prijmeni, telCislo,
                    pocetGolu, pocetZlutychKaret, pocetCervenychKaret, pozice
                );

                DatabaseHraci.AddHrac(novyHrac); // Přidání do databáze
                HraciData.Add(novyHrac);        // Přidání do ObservableCollection pro datagrid

                MessageBox.Show("Hráč byl úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Chybný formát vstupu!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala chyba:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
