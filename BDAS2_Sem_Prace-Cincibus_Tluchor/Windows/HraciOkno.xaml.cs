using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    /// <summary>
    /// Interakční logika pro HraciOkno.xaml
    /// </summary>
    public partial class HraciOkno : Window
    {
        private HlavniOkno hlavniOkno;

        //Kolekce hráčů pro DataGrid
        public ObservableCollection<Hrac> HraciData { get; set; } = new ObservableCollection<Hrac>();

        public HraciOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem

            NactiHrace();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        private void BtnPridejDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejHrace dialogPridejHrace = new DialogPridejHrace(HraciData);
            dialogPridejHrace.ShowDialog();
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e) 
        {
            DialogNajdiHrace dialogNajdiHrace = new DialogNajdiHrace(this);
            dialogNajdiHrace.ShowDialog();
        }

        private void DgHraci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;

            if (vybranyHrac == null)
            {
                MessageBox.Show("Prosím vyberte hráče, kterého chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogEditujHrace dialogEditujHrace = new DialogEditujHrace(vybranyHrac, this);
            dialogEditujHrace.ShowDialog();

        }

        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            // Získání vybraného hráče z DataGridu
            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;
        if (vybranyHrac == null)
            {
                MessageBox.Show(
                    "Prosím vyberte hráče, kterého chcete odebrat.",
                    "Chyba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            try
            {
                DatabaseHraci.OdeberHrace(vybranyHrac);

                // 5️⃣ Odebrání z ObservableCollection (aktualizuje DataGrid)
                HraciData.Remove(vybranyHrac);

                MessageBox.Show(
                    "Hráč byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chyba při odebírání hráče:\n{ex.Message}",
                    "Chyba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void NactiHrace()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM HRACI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                HraciData.Clear();

                while (reader.Read())
                {
                    Hrac hrac = new Hrac();

                    // RODNE_CISLO - NOT NULL
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        hrac.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        hrac.RodneCislo = 0L;

                    // JMENO - NOT NULL
                    if (reader["JMENO"] != DBNull.Value)
                        hrac.Jmeno = reader["JMENO"].ToString();
                    else
                        hrac.Jmeno = "";

                    // PRIJMENI - NOT NULL
                    if (reader["PRIJMENI"] != DBNull.Value)
                        hrac.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        hrac.Prijmeni = "";

                    // TELEFONNICISLO - NOT NULL
                    if (reader["TELEFONNICISLO"] != DBNull.Value)
                        hrac.TelefonniCislo = reader["TELEFONNICISLO"].ToString();
                    else
                        hrac.TelefonniCislo = "000000000";

                    // POCETVSTRELENYCHGOLU - NOT NULL
                    if (reader["POCETVSTRELENYCHGOLU"] != DBNull.Value)
                        hrac.PocetVstrelenychGolu = Convert.ToInt32(reader["POCETVSTRELENYCHGOLU"]);
                    else
                        hrac.PocetVstrelenychGolu = 0;

                    // POCETZLUTYCHKARET - NOT NULL
                    if (reader["POCET_ZLUTYCH_KARET"] != DBNull.Value)
                        hrac.PocetZlutychKaret = Convert.ToInt32(reader["POCET_ZLUTYCH_KARET"]);
                    else
                        hrac.PocetZlutychKaret = 0;

                    // POCETCERVENYCHKARET - NOT NULL
                    if (reader["POCET_CERVENYCH_KARET"] != DBNull.Value)
                        hrac.PocetCervenychKaret = Convert.ToInt32(reader["POCET_CERVENYCH_KARET"]);
                    else
                        hrac.PocetCervenychKaret = 0;

                    // NAZEV_POZICE - číselník, NOT NULL
                    if (reader["POZICENAHRISTI"] != DBNull.Value)
                        hrac.PoziceNaHristi = reader["POZICENAHRISTI"].ToString();
                    else
                        hrac.PoziceNaHristi = "Neznámá"; // default, pokud by bylo NULL

                    HraciData.Add(hrac);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Klávesou DELETE nelze smazat hráče z datagridu 
        private void DgHraci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání hráče klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}