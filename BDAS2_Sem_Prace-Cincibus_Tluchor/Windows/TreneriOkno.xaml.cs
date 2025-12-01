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
    /// <summary>
    /// Okno pro správu trenérů
    /// Umožňuje přidávání, mazání, úpravu, filtrování a export trenérů
    /// </summary>
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

        /// <summary>
        /// FUNKCE, Umožní export TOP 3 trenérů do textového souboru a uložit na plochu PC
        /// </summary>
        private void BtnExportTopTreneri_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Title = "Uložit TOP 3 trenéry";
                dialog.Filter = "Textový soubor (*.txt)|*.txt";

                if (dialog.ShowDialog() == true)
                {
                    Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

                    DatabaseTreneri.ExportTop3TreneriDoSouboru(dialog.FileName, uzivatel);

                    MessageBox.Show("TOP 3 trenéři byli úspěšně exportováni!",
                                    "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při exportu:\n" + ex.Message,
                                "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Skryje rodné číslo a telefon trenérům a hráčům
        /// Uživatelům bez oprávnění zakáže přidávání, mazání a vyhledávání
        /// </summary>
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
                //btnNajdi.IsEnabled = false;
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
                //btnNajdi.Opacity = 0.2;
            }
        }

        /// <summary>
        /// Zavře okno trenérů a vrátí uživatele do hlavního menu
        /// Aktualizuje počet trenérů v hlavním okně
        /// </summary>
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

        /// <summary>
        /// Otevře dialog pro přidání nového trenéra
        /// </summary>
        private void BtnPridejDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejTrenera dialogPridejTrenera = new DialogPridejTrenera(TreneriData);
            dialogPridejTrenera.ShowDialog();
        }

        /// <summary>
        /// Otevře dialog pro editaci trenéra při dvojkliku na záznam
        /// </summary>
        private void DgTreneri_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Trener vybranyTrener = dgTreneri.SelectedItem as Trener;

            if (vybranyTrener == null)
            {
                MessageBox.Show("Prosím vyberte trenéra, kterého chcete editovat! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = uzivatel.Role.ToLower();

            if (role == "hrac" || role == "uzivatel" || role == "host" || role == "trener")
            {
                MessageBox.Show("Nemáte oprávnění upravovat kontrakty",
                                "Omezení přístupu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            DialogEditujTrenera dialogEditujTrenera = new DialogEditujTrenera(vybranyTrener, this);
            dialogEditujTrenera.ShowDialog();

        }

        /// <summary>
        /// Odebere vybraného trenéra po potvrzení uživatelem.
        /// Nejprve zjistí, zda má trenér vlastní uživatelský účet.
        /// Pokud ano → smaže nejdříve účet a pak teprve trenéra.
        /// Poté odebere trenéra i z kolekce v DataGridu.
        /// </summary>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Trener vybranyTrener = dgTreneri.SelectedItem as Trener;

            // Ověření, že byl trenér vybrán
            if (vybranyTrener == null)
            {
                MessageBox.Show("Prosím, vyberte trenéra, kterého chcete odebrat!",
                                "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení mazání
            var potvrzeni = MessageBox.Show(
                $"Opravdu chcete odebrat trenéra {vybranyTrener.Jmeno} {vybranyTrener.Prijmeni}?",
                "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastaví přihlášeného uživatele pro logování triggerem
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Hledání účtu podle rodného čísla přes pohled
                    string sqlFindAcc = @"
                                        SELECT UZIVATELSKEJMENO
                                        FROM PREHLED_UZIVATELSKE_UCTY
                                        WHERE RODNE_CISLO = :rc";

                    // Sem se uloží nalezené uživatelské jméno
                    string uzivatelskeJmeno = null;

                    using (var cmdFind = new OracleCommand(sqlFindAcc, conn))
                    {
                        // předáme rodné číslo trenéra jako parametr
                        cmdFind.Parameters.Add(":rc", OracleDbType.Varchar2).Value = vybranyTrener.RodneCislo;

                        // ExecuteScalar vrátí první hodnotu - uživatelské jméno
                        object result = cmdFind.ExecuteScalar();

                        // pokud trenér nějaký účet má, uložíme ho
                        if (result != null)
                            uzivatelskeJmeno = result.ToString();
                    }

                    // Pokud má trenér uživatelský účet, smažeme ho jako první
                    if (!string.IsNullOrEmpty(uzivatelskeJmeno))
                    {
                        Uzivatel uzivatelTrener = new Uzivatel();
                        uzivatelTrener.UzivatelskeJmeno = uzivatelskeJmeno;

                        DatabaseRegistrace.DeleteUzivatel(conn, uzivatelTrener);
                    }

                    // Poté smažeme samotného trenéra
                    DatabaseTreneri.OdeberTrenera(conn, vybranyTrener);

                    // Odebereme hráče z kolekce
                    TreneriData.Remove(vybranyTrener);
                }

                MessageBox.Show(
                    $"Trenér {vybranyTrener.Jmeno} {vybranyTrener.Prijmeni} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání trenéra:\n{ex.Message}",
                                "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání trenéra:\n{ex.Message}",
                                "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Načte trenéry z databáze pomocí view TRENERI_VIEW
        /// Naplní kolekci TreneriData pro DataGrid
        /// </summary>
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

                    // Rodné číslo (NOT NULL)
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        trener.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        trener.RodneCislo = "";

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
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu
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