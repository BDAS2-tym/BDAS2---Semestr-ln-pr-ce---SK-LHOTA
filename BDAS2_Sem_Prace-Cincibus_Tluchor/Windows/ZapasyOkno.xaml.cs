using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interakční logika pro ZapasyOkno.xaml
    /// </summary>
    public partial class ZapasyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        // Kolekce zápasů pro DataGrid (binding v XAML)
        public ObservableCollection<Zapas> ZapasyData { get; set; } = new ObservableCollection<Zapas>();

        // Kolekce výsledků
        public List<VysledekZapasu> VysledkyData { get; set; } = new List<VysledekZapasu>();

        public ZapasyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Propojení kolekce s DataGridem
            DataContext = this;

            NactiZapasy();
            NactiVysledky();

            NastavViditelnostSloupcuProUzivatele();
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

            // Pokud je to hráč nebo trenér tyto sloupce a funkce tlačítek schováme
            if (role == "hrac" || role == "host" || role == "trener" || role == "uzivatel")
            {
                
                btnPridej.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
            }
        }

        /// <summary>
        /// Metoda slouží k vrácení se na hlavní okno aplikace
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            hlavniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Metoda načte zapasy z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiZapasy()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM ZAPASY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                ZapasyData.Clear();

                while (reader.Read())
                {
                    Zapas zapas = new Zapas();

                    // IDZAPAS - NOT NULL
                    if (reader["IDZAPAS"] != DBNull.Value)
                        zapas.IdZapas = Convert.ToInt32(reader["IDZAPAS"]);

                    // DATUM - NOT NULL
                    if (reader["DATUM"] != DBNull.Value)
                        zapas.Datum = reader.GetDateTime(reader.GetOrdinal("DATUM"));

                    // DOMACITYM - NOT NULL
                    if (reader["DOMACITYM"] != DBNull.Value)
                        zapas.DomaciTym = reader["DOMACITYM"].ToString();
                    else
                        zapas.DomaciTym = "";

                    // HOSTETYM - NOT NULL
                    if (reader["HOSTETYM"] != DBNull.Value)
                        zapas.HosteTym = reader["HOSTETYM"].ToString();
                    else
                        zapas.HosteTym = "";

                    // CISLONAAGENTA - NOT NULL
                    if (reader["STAVZAPASU"] != DBNull.Value)
                        zapas.StavZapasu = reader["STAVZAPASU"].ToString();
                    else
                        zapas.StavZapasu = "";

                    // VYSLEDEK - NULL
                    if (reader["VYSLEDEK"] != DBNull.Value)
                        zapas.Vysledek = reader["VYSLEDEK"].ToString();
                    else
                        zapas.Vysledek = "";

                    // Přidání soutěže, pokud nejsou všechny atributy NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value && reader["NAZEVSOUTEZE"] != DBNull.Value)
                    {
                        zapas.Soutez = new Soutez
                        {
                            IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]),
                            TypSouteze = reader["NAZEVSOUTEZE"].ToString()
                        };
                    }

                    ZapasyData.Add(zapas);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání zapasů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda načte výsledky zápasů z databáze přes DatabaseManager
        /// </summary>
        private void NactiVysledky()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM VYSLEDKY_ZAPASU_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                VysledkyData.Clear();

                while (reader.Read())
                {
                    VysledekZapasu vysledekZapasu = new VysledekZapasu();

                    // IDZAPAS - NOT NULL
                    if (reader["IDZAPAS"] != DBNull.Value)
                        vysledekZapasu.IdZapasu = Convert.ToInt32(reader["IDZAPAS"]);

                    // POCETZLUTYCHKARET - NOT NULL
                    if (reader["POCETZLUTYCHKARET"] != DBNull.Value)
                        vysledekZapasu.PocetZlutychKaret = Convert.ToInt32(reader["POCETZLUTYCHKARET"]);
                    else
                        vysledekZapasu.PocetZlutychKaret = 0;

                    // POCETCERVENYCHKARET - NOT NULL
                    if (reader["POCETCERVENYCHKARET"] != DBNull.Value)
                        vysledekZapasu.PocetCervenychKaret = Convert.ToInt32(reader["POCETCERVENYCHKARET"]);
                    else
                        vysledekZapasu.PocetCervenychKaret = 0;

                    // POCETGOLYDOMACITYM - NOT NULL
                    if (reader["POCETGOLYDOMACITYM"] != DBNull.Value)
                        vysledekZapasu.PocetGolyDomaci = Convert.ToInt32(reader["POCETGOLYDOMACITYM"]);
                    else
                        vysledekZapasu.PocetGolyDomaci = 0;

                    // POCETGOLYHOSTETYM - NOT NULL
                    if (reader["POCETGOLYHOSTETYM"] != DBNull.Value)
                        vysledekZapasu.PocetGolyHoste = Convert.ToInt32(reader["POCETGOLYHOSTETYM"]);
                    else
                        vysledekZapasu.PocetGolyHoste = 0;

                    // Přidání zápasu, pokud se daný zápas nachází v kolekci
                    if (ZapasyData.Any(zapas => zapas.IdZapas == vysledekZapasu.IdZapasu))
                    {
                        Zapas? nalezenyZapas = ZapasyData.FirstOrDefault(zapas => zapas.IdZapas == vysledekZapasu.IdZapasu);

                        if (nalezenyZapas != null)
                        {
                            vysledekZapasu.Zapas = nalezenyZapas;
                            vysledekZapasu.Vysledek = nalezenyZapas.Vysledek;
                        }
                    }

                    VysledkyData.Add(vysledekZapasu);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání výsledků zápasů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsluha tlačítka "Info"
        /// Zavolá PL/SQL funkci F_STAV_ZAPASU_SHRNUTI a zobrazí její výsledek
        /// </summary>
        /// <param name="sender">Odesílatel události (tlačítko Info)</param>
        /// <param name="e">Argumenty události click</param>
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                string vysledekFunkce = DatabaseZapasy.GetShrnutiStavuZapasu(conn);

                MessageBox.Show(vysledekFunkce, "Přehled stavu zápasů", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chyba při získání shrnutí zápasů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgZapasy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání zápasu klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgZapasy.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgZapasy.Focusable = false;
                Keyboard.ClearFocus();
                dgZapasy.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k přidání zápasu do tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejZapas dialogPridejZapas = new DialogPridejZapas(ZapasyData, VysledkyData);
            dialogPridejZapas.ShowDialog();
        }

        /// <summary>
        /// Metoda slouží k odebrání zápasu z tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Zapas? vybranyZapas = dgZapasy.SelectedItem as Zapas;
            if (vybranyZapas == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte zápas, který chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            MessageBoxResult potvrzeni = MessageBox.Show($"Opravdu chcete odebrat zápas " +
                $"{vybranyZapas.DomaciTym} - {vybranyZapas.HosteTym}?", "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni == MessageBoxResult.No)
            {
                return;
            }

            // Smazání z databáze
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Odebrání soutěže
                    DatabaseZapasy.OdeberZapas(conn, vybranyZapas);

                    ZapasyData.Remove(vybranyZapas);

                    VysledekZapasu? vybranyVysledek = VysledkyData.Find(vysledek => vysledek.IdZapasu == vybranyZapas.IdZapas);
                    if(vybranyVysledek != null)
                    {
                        VysledkyData.Remove(vybranyVysledek);
                    }
                }

                // Úspěch
                MessageBox.Show(
                    "Zápas byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání zápasu:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání zápasu:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu o podrobnostech ohledně výsledku zápasu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void Vysledek_Click(object sender, MouseButtonEventArgs e)
        {
            if (jeVyhledavaniAktivni)
            {
                e.Handled = true;
                return;
            }

            // Zjištění role přihlášeného uživatele
            var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            if (prihlaseny == null) return;

            string role = prihlaseny.Role.ToLower();

            // Role, které NEMAJÍ povoleno editovat výsledek zápasu
            if (role == "host" || role == "uzivatel" || role == "trener" || role == "hrac")
            {
                MessageBox.Show(
                    "Vaše role nemá oprávnění editovat výsledek zápasu.",
                    "Přístup zakázán",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Ukončit metodu
            }

            // Dále pokračuje jen v roli Admin
            // Získání vybraného zápasu z TextBlocku
            if (sender is TextBlock tb && tb.DataContext is Zapas vybranyZapas)
            {
                //Kontrola, zda kliknutý zápas má výsledek
                VysledekZapasu? vysledek = VysledkyData.FirstOrDefault(v => v.IdZapasu == vybranyZapas.IdZapas);
                if(vysledek != null)
                {
                    DialogEditujVysledekZapasu dialogEditujVysledekZapasu = new DialogEditujVysledekZapasu(vysledek, vybranyZapas);
                    dialogEditujVysledekZapasu.ShowDialog();

                    if(dialogEditujVysledekZapasu != null && dialogEditujVysledekZapasu.DialogResult == true)
                    {
                        vybranyZapas.Vysledek = dialogEditujVysledekZapasu.EditovanyVysledek.Vysledek;
                        dgZapasy.Items.Refresh();
                    }
                }               
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení editovacího dialogu zápasu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgZapasy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // Získání objektu DataGrid a jeho potomků, aby se DoubleClick uplatňoval pouze na řádky a ne na ColumnHeader
            while (dep != null && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = uzivatel.Role.ToLower();

            if (role == "hrac" ||  role == "host" || role == "trener" || role == "uzivatel")
            {
                MessageBox.Show("Nemáte oprávnění upravovat zápasy",
                                "Omezení přístupu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (dep is DataGridRow row)
            {
                Zapas? vybranyZapas = (Zapas)row.Item;
                if (vybranyZapas == null)
                {
                    MessageBox.Show("Prosím vyberte zápas, který chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogEditujZapas dialogEditujZapas = new DialogEditujZapas(vybranyZapas, VysledkyData, this);
                dialogEditujZapas.ShowDialog();
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované zápasy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiZapas dialogNajdiZapas = new DialogNajdiZapas(ZapasyData);
            bool? vysledekDiaOkna = dialogNajdiZapas.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiZapas.VyfiltrovaneZapasy.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné zápasy se zadanými filtry.", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat ani odebírat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgZapasy.ItemsSource = new ObservableCollection<Zapas>(dialogNajdiZapas.VyfiltrovaneZapasy);
                jeVyhledavaniAktivni = true;

                btnPridej.IsEnabled = btnOdeber.IsEnabled = false;
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
                dgZapasy.ItemsSource = ZapasyData;
                btnPridej.IsEnabled = btnOdeber.IsEnabled = true;
                e.Handled = true;
            }
        }
    }
}
