using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
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
        private bool jeVyhledavaniAktivni = false;

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

        /// <summary>
        /// Otevře dialogové okno s TOP 3 střelci
        /// </summary>
        private void BtnTopStrelci_Click(object sender, RoutedEventArgs e)
        {
            DialogTopStrelci dialogTopStrelci = new DialogTopStrelci();
            dialogTopStrelci.ShowDialog();
        }

        /// <summary>
        /// Skryje citlivé údaje (rodné číslo, telefon) podle role přihlášeného uživatele
        /// a zakáže funkce přidávání a mazání, pokud přihlášený není administrátor
        /// </summary>
        private void NastavViditelnostSloupcuProUzivatele()
        {
            // Zjistíme, kdo je přihlášený
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = uzivatel.Role.ToLower();

            // Nejdřív zobrazíme oba sloupce
            RodneCisloSloupec.Visibility = Visibility.Visible;
            TelefonniCisloSloupec.Visibility = Visibility.Visible;

            // Pro dané role funkce tlačítek schováme
            if (role == "hrac" || role == "trener" || role == "host")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;
                TelefonniCisloSloupec.Visibility = Visibility.Collapsed;

                btnPridej.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
            }
        }

        /// <summary>
        /// Zavře toto okno a vrátí uživatele zpět do hlavního menu
        /// Aktualizuje counter hráčů v hlavním okně
        /// </summary>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
            hlavniOkno.txtPocetHracu.Text = DatabaseHraci.GetPocetHracu().ToString();
        }

        /// <summary>
        /// Otevře dialog pro přidání nového hráče
        /// Po uzavření dialogu se DataGrid automaticky aktualizuje
        /// </summary>
        private void BtnPridejDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejHrace dialogPridejHrace = new DialogPridejHrace(HraciData);
            dialogPridejHrace.ShowDialog();
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované záznamy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiHrace dialogNajdiHrace = new DialogNajdiHrace(HraciData);
            bool? vysledekDiaOkna = dialogNajdiHrace.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiHrace.VyfiltrovaniHraci.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat ani odebírat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgHraci.ItemsSource = new ObservableCollection<Hrac>(dialogNajdiHrace.VyfiltrovaniHraci);
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
                dgHraci.ItemsSource = HraciData;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Otevře dialog pro editaci hráče při dvojkliku na jeho řádek v DataGridu
        /// Kontroluje, zda byl hráč skutečně vybrán
        /// </summary>
        private void DgHraci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;

            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = uzivatel.Role.ToLower();

            if (role == "hrac" || role == "trener" || role == "host")
            {
                MessageBox.Show("Nemáte oprávnění upravovat kontrakty",
                                "Omezení přístupu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (vybranyHrac == null)
            {
                MessageBox.Show("Prosím vyberte hráče, kterého chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            DialogEditujHrace dialogEditujHrace = new DialogEditujHrace(vybranyHrac, this);
            dialogEditujHrace.ShowDialog();
        }

        /// <summary>
        /// Odebere označeného hráče po potvrzení uživatelem
        /// Odstraní hráče z databáze i z kolekce zobrazené v DataGridu
        /// Pokud má hráč uživatelský účet, smaže se nejprve účet a poté hráč
        /// </summary>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;

            if (vybranyHrac == null)
            {
                MessageBox.Show("Prosím, vyberte hráče, kterého chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            var potvrzeni = MessageBox.Show(
                $"Opravdu chcete odebrat hráče {vybranyHrac.Jmeno} {vybranyHrac.Prijmeni}?",
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

                    // Nastavení přihlášeného uživatele pro logování triggerem
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
                        cmdFind.Parameters.Add(":rc", OracleDbType.Varchar2).Value = vybranyHrac.RodneCislo;

                        object result = cmdFind.ExecuteScalar();

                        // Pokud dotaz něco našel, tak výsledek převedeme na string
                        if (result != null)
                        {
                            uzivatelskeJmeno = result.ToString();
                        }
                           
                    }

                    // Pokud má hráč uživatelský účet, smažeme ho jako první
                    if (!string.IsNullOrEmpty(uzivatelskeJmeno))
                    {
                        Uzivatel uzivatelHrac = new Uzivatel();
                        uzivatelHrac.UzivatelskeJmeno = uzivatelskeJmeno;

                        DatabaseRegistrace.DeleteUzivatel(conn, uzivatelHrac);
                    }

                    // Poté smažeme samotného hráče
                    DatabaseHraci.OdeberHrace(conn, vybranyHrac);

                    // Odebereme hráče z kolekce
                    HraciData.Remove(vybranyHrac);
                }

                // Úspěch
                MessageBox.Show(
                    $"Hráč {vybranyHrac.Jmeno} {vybranyHrac.Prijmeni} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání hráče:\n{ex.Message}",
                                "Databázová chyba",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání hráče:\n{ex.Message}",
                                "Chyba",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
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
                        hrac.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        hrac.RodneCislo = "";

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

                    // Pozice hráče z číselníku (text)
                    if (reader["POZICENAHRISTI"] != DBNull.Value)
                        hrac.PoziceNaHristi = reader["POZICENAHRISTI"].ToString();
                    else
                        hrac.PoziceNaHristi = "Neznámá";

                    // DATUMOPATRENI
                    if (reader["DATUMOPATRENI"] != DBNull.Value)
                    {
                        DateTime datum = Convert.ToDateTime(reader["DATUMOPATRENI"]).Date;
                        if (datum == new DateTime(1900, 1, 1) || datum == DateTime.MinValue)
                            hrac.DatumOpatreniText = "Bez opatření";
                        else
                            hrac.DatumOpatreniText = datum.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        hrac.DatumOpatreniText = "Bez opatření";
                    }

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
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgHraci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání hráče klávesou Delete není povoleno", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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