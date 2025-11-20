using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialogové okno pro přidání nového hráče
    /// Umožňuje vyplnit osobní údaje, statistiky, pozici a případné disciplinární opatření
    /// Po potvrzení vloží hráče do databáze a přidá ho do kolekce zobrazené v DataGridu
    /// </summary>
    public partial class DialogPridejHrace : Window
    {
        private ObservableCollection<Hrac> HraciData;

        public DialogPridejHrace(ObservableCollection<Hrac> HraciData)
        {
            InitializeComponent();
            this.HraciData = HraciData;

            // Naplnění ComboBoxu pro pozice hráčů
            cbPozice.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };

            // Výchozí hodnoty statistických čísel
            cbPozice.SelectedIndex = 0;
            iudPocetCervenychKaret.Value = 0;
            iudPocetGolu.Value = 0;
            iudPocetZlutychKaret.Value = 0;
        }

        /// <summary>
        /// Po zaškrtnutí zobrazí panel s disciplinárním opatřením
        /// </summary>
        private void chkMaOpatreni_Checked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Po odškrtnutí skryje panel opatření a smaže vyplněné hodnoty
        /// </summary>
        private void chkMaOpatreni_Unchecked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Collapsed;
            dpDatumOpatreni.SelectedDate = null;
            iudDelkaTrestu.Value = 1;
            tboxDuvodOpatreni.Clear();
        }

        /// <summary>
        /// Resetuje celý formulář na výchozí stav
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxRodneCislo.Clear();
            tboxJmeno.Clear();
            tboxPrijmeni.Clear();
            tboxTelCislo.Clear();
            cbPozice.SelectedIndex = 0;
            iudPocetCervenychKaret.Value = 0;
            iudPocetGolu.Value = 0;
            iudPocetZlutychKaret.Value = 0;
            chkMaOpatreni.IsChecked = false;
        }

        /// <summary>
        /// Zpracuje přidání nového hráče – provádí validaci vstupů, vytvoření objektu hráče,
        /// doplnění disciplinárního opatření, uložení do DB a následné přidání do kolekce
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // VALIDACE
                if (!long.TryParse(tboxRodneCislo.Text, out long rodneCislo))
                {
                    MessageBox.Show("Rodné číslo může obsahovat pouze číslice", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string pozice = cbPozice.SelectedItem?.ToString() ?? "";

                if (string.IsNullOrEmpty(jmeno) || string.IsNullOrEmpty(prijmeni) || string.IsNullOrEmpty(telCislo) || string.IsNullOrEmpty(pozice))
                {
                    MessageBox.Show("Vyplňte všechna povinná pole.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (telCislo.Any(c => !char.IsDigit(c)))
                {
                    MessageBox.Show("Telefonní číslo může obsahovat pouze číslice", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (rodneCislo.ToString().Length != 10)
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic bez lomítka", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int pocetGolu = (int)iudPocetGolu.Value;
                int pocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                int pocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                if (pocetGolu < 0 || pocetZlutychKaret < 0 || pocetCervenychKaret < 0)
                {
                    MessageBox.Show("Počet gólů a karet nesmí být záporný", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Vytvoření nového hráče
                Hrac novyHrac = new Hrac(rodneCislo, jmeno, prijmeni, telCislo, pocetGolu, pocetZlutychKaret, pocetCervenychKaret, pozice
                );

                // Pokud má opatření, nastav ho do objektu ještě před uložením 
                if (chkMaOpatreni.IsChecked == true)
                {
                    if (dpDatumOpatreni.SelectedDate == null)
                    {
                        MessageBox.Show("Datum disciplinárního opatření je povinné.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    novyHrac.DatumOpatreni = dpDatumOpatreni.SelectedDate.Value;
                    novyHrac.DelkaTrestu = (int)iudDelkaTrestu.Value;
                    novyHrac.DuvodOpatreni = tboxDuvodOpatreni.Text.Trim();
                    novyHrac.DatumOpatreniText = novyHrac.DatumOpatreni.ToString("dd.MM.yyyy");
                }
                else
                {
                    // Pokud nemá opatření, nastav defaulty
                    novyHrac.DatumOpatreni = DateTime.MinValue;
                    novyHrac.DelkaTrestu = 0;
                    novyHrac.DuvodOpatreni = null;
                    novyHrac.DatumOpatreniText = "Bez opatření";
                }

                // Uložení hráče do databáze
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    DatabaseHraci.AddHrac(conn, novyHrac);
                }

                // Přidej hráče do kolekce a aktualizuj DataGrid 
                HraciData.Add(novyHrac);

                MessageBox.Show("Hráč byl úspěšně přidán", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přidávání hráče:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
