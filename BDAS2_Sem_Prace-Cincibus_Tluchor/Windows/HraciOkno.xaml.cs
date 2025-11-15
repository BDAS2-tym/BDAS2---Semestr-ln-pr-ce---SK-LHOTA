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
        public static ObservableCollection<Hrac> HraciData { get; set; } = new ObservableCollection<Hrac>();

        public HraciOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem

            NactiHrace();
            NastavViditelnostSloupcuProUzivatele();
        }

        private void NastavViditelnostSloupcuProUzivatele()
        {
            // Zjistíme, kdo je přihlášený
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = uzivatel.Role.ToLower();

            // Nejdřív zobrazíme oba sloupce
            RodneCisloSloupec.Visibility = Visibility.Visible;
            TelefonniCisloSloupec.Visibility = Visibility.Visible;

            // Pokud je to hráč nebo trenér tyto sloupce a funkce tlačítek schováme
            if (role == "hrac" || role == "trener")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;
                TelefonniCisloSloupec.Visibility = Visibility.Collapsed;

                btnPridej.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnNajdi.IsEnabled = false; 
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
                btnNajdi.Opacity = 0.2;
            }
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
            hlavniOkno.txtPocetHracu.Text = DatabaseHraci.GetPocetHracu().ToString();
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

            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;

            if (vybranyHrac == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte hráče, kterého chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            var potvrzeni = MessageBox.Show($"Opravdu chcete odebrat hráče {vybranyHrac.Jmeno} {vybranyHrac.Prijmeni}?", "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

            // smazání z databáze
            try
            {
                DatabaseHraci.OdeberHrace(vybranyHrac);

                // Aktualizace DataGridu (odebrání z kolekce)
                HraciData.Remove(vybranyHrac);

                // Úspěch
                MessageBox.Show(
                    $"Hráč {vybranyHrac.Jmeno} {vybranyHrac.Prijmeni} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání hráče:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání hráče:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda načte hráče z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiHrace()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM HRACI_OPATRENI_VIEW", conn);
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

                    // DATUMOPATRENI - NOT NULL
                    if (reader["DATUMOPATRENI"] != DBNull.Value)
                        hrac.DatumOpatreni = Convert.ToDateTime(reader["DATUMOPATRENI"]);
                    else
                        hrac.DatumOpatreni = DateTime.MinValue;

                    // DELKATRESTU - NOT NULL
                    if (reader["DELKATRESTU"] != DBNull.Value)
                        hrac.DelkaTrestu = Convert.ToInt32(reader["DELKATRESTU"]);
                    else
                        hrac.DelkaTrestu = 0;

                    // DUVODOPATRENI - může být NULL
                    if (reader["DUVOD"] != DBNull.Value)
                        hrac.DuvodOpatreni = reader["DUVOD"].ToString();
                    else
                        hrac.DuvodOpatreni = null;

                    HraciData.Add(hrac);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgHraci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání hráče klávesou Delete není povoleno.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgHraci.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgHraci.Focusable = false;
                Keyboard.ClearFocus();
                dgHraci.Focusable = true;
            }
        }

    }
}