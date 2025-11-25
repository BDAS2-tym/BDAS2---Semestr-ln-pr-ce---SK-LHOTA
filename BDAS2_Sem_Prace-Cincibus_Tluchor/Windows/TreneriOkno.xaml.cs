using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class TreneriOkno : Window
    {
        private HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        // Kolekce trenérů pro DataGrid
        public static ObservableCollection<Trener> TreneriData { get; set; } = new ObservableCollection<Trener>();

        public TreneriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem

            NactiTrenery();
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
            hlavniOkno.txtPocetTreneru.Text = DatabaseTreneri.GetPocetTreneru().ToString();
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované záznamy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiTrenera dialogNajdiTrenera = new DialogNajdiTrenera(TreneriData);
            bool? vysledekDiaOkna = dialogNajdiTrenera.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiTrenera.VyfiltrovaniTreneri.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgTreneri.ItemsSource = new ObservableCollection<Trener>(dialogNajdiTrenera.VyfiltrovaniTreneri);
                jeVyhledavaniAktivni = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zrušení vyhledávacího módu, pokud se zmáčkne klávesa CTRL + X
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Zrušení vyhledávacího módu při zmáčknutí klávesy CTRL + X
            if (jeVyhledavaniAktivni && (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X))
            {
                jeVyhledavaniAktivni = false;
                dgTreneri.ItemsSource = TreneriData;
                e.Handled = true;
            }
        }

        private void BtnPridejDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejTrenera dialogPridejTrenera = new DialogPridejTrenera(TreneriData);
            dialogPridejTrenera.ShowDialog();
        }

        private void DgTreneri_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Trener vybranyTrener = dgTreneri.SelectedItem as Trener;

            if (vybranyTrener == null)
            {
                MessageBox.Show("Prosím vyberte trenéra, kterého chcete editovat! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogEditujTrenera dialogEditujTrenera = new DialogEditujTrenera(vybranyTrener, this);
            dialogEditujTrenera.ShowDialog();

        }

        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {

            Trener vybranyTrener = dgTreneri.SelectedItem as Trener;

            if (vybranyTrener == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte trenéra, kterého chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            var potvrzeni = MessageBox.Show($"Opravdu chcete odebrat trenéra {vybranyTrener.Jmeno} {vybranyTrener.Prijmeni}?", "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

            // Smazání z databáze
            try
            {

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Odebrání trenéra
                    DatabaseTreneri.OdeberTrenera(conn, vybranyTrener);

                    // Aktualizace DataGridu (odebrání z kolekce)
                    TreneriData.Remove(vybranyTrener);
                }

                // Úspěch
                MessageBox.Show(
                    $"Trenér {vybranyTrener.Jmeno} {vybranyTrener.Prijmeni} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání trenéra:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání trenéra:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void NactiTrenery()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM TRENERI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                // Vyprázdníme kolekci, aby se nepřidávaly duplicitní záznamy
                TreneriData.Clear();

                while (reader.Read())
                {
                    Trener trener = new Trener();

                    //// ID člena klubu (NOT NULL) - pokud je NULL, nastavíme 0
                    //if (reader["IDCLENKLUBU"] != DBNull.Value)
                    //    trener.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);
                    //else
                    //    trener.IdClenKlubu = 0;

                    // Rodné číslo (NOT NULL) 
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        trener.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        trener.RodneCislo = 0L;

                    // Jméno (NOT NULL)
                    if (reader["JMENO"] != DBNull.Value)
                        trener.Jmeno = reader["JMENO"].ToString();
                    else
                        trener.Jmeno = "";

                    // Příjmení (NOT NULL) 
                    if (reader["PRIJMENI"] != DBNull.Value)
                        trener.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        trener.Prijmeni = "";

                    // Telefonní číslo (NOT NULL) 
                    if (reader["TELEFONNICISLO"] != DBNull.Value)
                        trener.TelefonniCislo = reader["TELEFONNICISLO"].ToString();
                    else
                        trener.TelefonniCislo = "000000000";

                    // Trenérská licence (NOT NULL) 
                    if (reader["TRENERSKALICENCE"] != DBNull.Value)
                        trener.TrenerskaLicence = reader["TRENERSKALICENCE"].ToString();
                    else
                        trener.TrenerskaLicence = "";

                    // Specializace (volitelný sloupec) 
                    if (reader["SPECIALIZACE"] != DBNull.Value)
                        trener.Specializace = reader["SPECIALIZACE"].ToString();
                    else
                        trener.Specializace = "Volitelné nezadáno !";

                    // Počet let praxe (NOT NULL) 
                    if (reader["POCETLETPRAXE"] != DBNull.Value)
                        trener.PocetLetPraxe = Convert.ToInt32(reader["POCETLETPRAXE"]);
                    else
                        trener.PocetLetPraxe = 0;

                    // Přidáme trenéra do kolekce pro DataGrid
                    TreneriData.Add(trener);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání trenérů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgTreneri_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání trenéra klávesou Delete není povoleno.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgTreneri.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgTreneri.Focusable = false;
                Keyboard.ClearFocus();
                dgTreneri.Focusable = true;
            }
        }
    }
}