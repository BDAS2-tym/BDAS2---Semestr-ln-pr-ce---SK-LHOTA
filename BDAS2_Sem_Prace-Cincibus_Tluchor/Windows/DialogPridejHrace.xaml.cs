using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

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

            cbPozice.ItemsSource = new List<Pozice>
            {
                new Pozice { Id = 1, Nazev = "Brankář" },
                new Pozice { Id = 2, Nazev = "Obránce" },
                new Pozice { Id = 3, Nazev = "Záložník" },
                new Pozice { Id = 4, Nazev = "Útočník" }
            };

            cbPozice.DisplayMemberPath = "Nazev";
            cbPozice.SelectedValuePath = "Id";
            cbPozice.SelectedIndex = 0;
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
                // Načtení hodnot z formuláře
                string rodneCislo = tboxRodneCislo.Text.Trim();
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();

                // Získání pozice hráče
                int idPozice = (int)cbPozice.SelectedValue;

                // Validace základních údajů
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);

                // Validace statistických hodnot
                Validator.ValidujCeleCislo(iudPocetGolu.Value.ToString(), "Počet gólů");
                Validator.ValidujCeleCislo(iudPocetZlutychKaret.Value.ToString(), "Počet žlutých karet");
                Validator.ValidujCeleCislo(iudPocetCervenychKaret.Value.ToString(), "Počet červených karet");

                // Zjištění, zda má hráč disciplinární opatření
                bool maOpatreni = chkMaOpatreni.IsChecked == true;

                // Validace disciplinárního opatření
                if (maOpatreni)
                {
                    Validator.ValidujDatum(dpDatumOpatreni.SelectedDate, "Datum disciplinárního opatření");
                    Validator.ValidujCeleCislo(iudDelkaTrestu.Value.ToString(), "Délka trestu");

                    
                }

                // Vytvoření objektu hráče
                Hrac novyHrac = new Hrac(
                    rodneCislo,
                    jmeno,
                    prijmeni,
                    telCislo,
                    (int)iudPocetGolu.Value,
                    (int)iudPocetZlutychKaret.Value,
                    (int)iudPocetCervenychKaret.Value,
                    idPozice
                );

                novyHrac.PoziceNaHristi = ((Pozice)cbPozice.SelectedItem).Nazev;

                // Uložení disciplinárního opatření do objektu
                if (maOpatreni)
                {
                    novyHrac.DatumOpatreni = dpDatumOpatreni.SelectedDate.Value;
                    novyHrac.DelkaTrestu = (int)iudDelkaTrestu.Value;
                    novyHrac.DuvodOpatreni = tboxDuvodOpatreni.Text.Trim();
                    novyHrac.DatumOpatreniText = novyHrac.DatumOpatreni.ToString("dd.MM.yyyy");
                }
                else
                {
                    novyHrac.DatumOpatreni = DateTime.MinValue;
                    novyHrac.DelkaTrestu = 0;
                    novyHrac.DuvodOpatreni = null;
                    novyHrac.DatumOpatreniText = "Bez opatření";
                }

                // Uložení hráče do databáze
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Kontrola UNIQUE rodného čísla
                    if (Validator.ExistujeRodneCislo(conn, rodneCislo))
                    {
                        MessageBox.Show("Hráč s tímto rodným číslem již existuje!",
                                        "Duplicitní rodné číslo",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    DatabaseHraci.AddHrac(conn, novyHrac);
                }

                // Přidání hráče do kolekce pro DataGrid
                HraciData.Add(novyHrac);

                MessageBox.Show("Hráč byl úspěšně přidán", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                // Zavření dialogu
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přidávání hráče:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
