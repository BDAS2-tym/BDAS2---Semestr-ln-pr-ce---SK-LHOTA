using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Oracle.ManagedDataAccess.Client;
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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interakční logika pro Kontrakty.xaml
    /// </summary>
    public partial class KontraktyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        // Kolekce kontraktů pro DataGrid (binding v XAML)
        public ObservableCollection<Kontrakt> KontraktyData { get; set; } = new ObservableCollection<Kontrakt>();

        public KontraktyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;            

            // Propojení kolekce s DataGridem
            DataContext = this;

            NactiKontrakty();
        }

        /// <summary>
        /// Metoda slouží k vrácení se na hlavní okno aplikace
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        /// <summary>
        /// Metoda načte kontrakty z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiKontrakty()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM KONTRAKTY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                KontraktyData.Clear();

                while (reader.Read())
                {   
                    Kontrakt kontrakt = new Kontrakt();

                    // IDCLENKLUBU - NOT NULL
                    if (reader["IDCLENKLUBU"] != DBNull.Value)
                        kontrakt.IdClena = Convert.ToInt32(reader["IDCLENKLUBU"]);

                    // DATUMZACATKU - NOT NULL
                    if (reader["DATUMZACATKU"] != DBNull.Value)
                        kontrakt.DatumZacatku = DateOnly.FromDateTime(Convert.ToDateTime(reader["DATUMZACATKU"]));

                    // DATUMKONCE - NOT NULL
                    if (reader["DATUMKONCE"] != DBNull.Value)
                        kontrakt.DatumKonce = DateOnly.FromDateTime(Convert.ToDateTime(reader["DATUMKONCE"]));

                    // PLAT - NOT NULL
                    if (reader["PLAT"] != DBNull.Value)
                        kontrakt.Plat = Convert.ToInt32(reader["Plat"]);
                    else
                        kontrakt.Plat = 0;

                    // CISLONAAGENTA - NOT NULL
                    if (reader["CISLONAAGENTA"] != DBNull.Value)
                        kontrakt.TelCisloNaAgenta = reader["CISLONAAGENTA"].ToString();
                    else
                        kontrakt.TelCisloNaAgenta = "";

                    // VYSTUPNIKLAUZULE - NULL
                    if (reader["VYSTUPNIKLAUZULE"] != DBNull.Value)
                        kontrakt.VystupniKlauzule = Convert.ToInt32(reader["VYSTUPNIKLAUZULE"]);
                    else
                        kontrakt.VystupniKlauzule = 0;

                    // Přidání hráče, pokud nejsou všechny atributy NULL
                    if (reader["IDCLENKLUBU"] != DBNull.Value && reader["JMENO"] != DBNull.Value && reader["PRIJMENI"] != DBNull.Value)
                    {
                        kontrakt.KontraktHrace = new Hrac
                        {
                            IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]),
                            Jmeno = reader["JMENO"].ToString(),
                            Prijmeni = reader["PRIJMENI"].ToString(),
                            RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"])
                        };
                    }

                    KontraktyData.Add(kontrakt);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání kontraktů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k přidání kontraktu do tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejKontrakt dialogPridejKontrakt = new DialogPridejKontrakt(KontraktyData);
            dialogPridejKontrakt.ShowDialog();
        }

        /// <summary>
        /// Metoda slouží k odebrání kontraktu z tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Kontrakt? vybranyKontrakt = dgKontrakty.SelectedItem as Kontrakt;
            if(vybranyKontrakt == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte kontrakt, který chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            MessageBoxResult potvrzeni = MessageBox.Show($"Opravdu chcete odebrat kontrakt hráče " +
                $"{vybranyKontrakt.KontraktHrace.Jmeno} {vybranyKontrakt.KontraktHrace.Prijmeni}?", "Potvrzení odebrání",
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

                    // Odebrání kontraktu
                    DatabaseKontrakty.OdeberKontrakt(conn, vybranyKontrakt);

                    KontraktyData.Remove(vybranyKontrakt);
                }

                // Úspěch
                MessageBox.Show(
                    $"Kontrakt hráče {vybranyKontrakt.KontraktHrace.Jmeno} {vybranyKontrakt.KontraktHrace.Prijmeni} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání kontraktu:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání kontraktu:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení editovacího dialogu kontraktu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgKontrakty_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (jeVyhledavaniAktivni)
            {
                e.Handled = true;
                return;
            }

            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // Získání objektu DataGrid a jeho potomků, aby se DoubleClick uplatňoval pouze na řádky a ne na ColumnHeader
            while (dep != null && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridRow row)
            {
                Kontrakt? vybranyKontrakt = (Kontrakt)row.Item;
                if (vybranyKontrakt == null)
                {
                    MessageBox.Show("Prosím vyberte kontrakt, který chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogEditujKontrakt dialogEditujKontrakt = new DialogEditujKontrakt(vybranyKontrakt, this);
                dialogEditujKontrakt.ShowDialog();
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgKontrakty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání kontraktu klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if(e.Key == Key.Space)
            {
                dgKontrakty.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgKontrakty.Focusable = false;
                Keyboard.ClearFocus();
                dgKontrakty.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované kontrakty zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiKontrakt dialogNajdiKontrakt = new DialogNajdiKontrakt(KontraktyData);
            bool? vysledekDiaOkna = dialogNajdiKontrakt.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiKontrakt.VyfiltrovaneKontrakty.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné kontrakty se zadanými filtry.", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgKontrakty.ItemsSource = new ObservableCollection<Kontrakt>(dialogNajdiKontrakt.VyfiltrovaneKontrakty);
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
                dgKontrakty.ItemsSource = KontraktyData;
                btnPridej.IsEnabled = btnOdeber.IsEnabled = true;
                e.Handled = true;
            }
        }
    }
}
