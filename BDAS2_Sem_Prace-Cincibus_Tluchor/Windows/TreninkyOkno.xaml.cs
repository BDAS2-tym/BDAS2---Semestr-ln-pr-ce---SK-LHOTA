using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interakční logika pro TreninkyOkno.xaml
    /// </summary>
    public partial class TreninkyOkno : Window
    {

        private HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        // Kolekce tréninků pro DataGrid
        public static ObservableCollection<TreninkView> TreninkyData { get; set; } = new ObservableCollection<TreninkView>();

        public TreninkyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem
            
            NactiTreninky();
            NastavViditelnostSloupcuProUzivatele();
        }

        /// <summary>
        /// Nastaví viditelnost datových sloupců a možnost úprav podle role přihlášeného uživatele
        /// </summary>
        private void NastavViditelnostSloupcuProUzivatele()
        {
            // Zjistíme, kdo je přihlášený
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = uzivatel.Role.ToLower();

            // Nejdřív zobrazíme
            RodneCisloSloupec.Visibility = Visibility.Visible;

            // Pokud je to hráč, uživatel nebo trenér tyto sloupce a funkce tlačítek schováme
            if (role == "hrac" || role == "host" || role == "uzivatel")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;

                btnPridej.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované záznamy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiTrenink dialogNajdiTrenink = new DialogNajdiTrenink(TreninkyData);
            bool? vysledekDiaOkna = dialogNajdiTrenink.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiTrenink.VyfiltrovaneTreninky.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgTreninky.ItemsSource = new ObservableCollection<TreninkView>(dialogNajdiTrenink.VyfiltrovaneTreninky);
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
                dgTreninky.ItemsSource = TreninkyData;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Zavře okno a vrátí uživatele do hlavního menu
        /// </summary>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        /// <summary>
        /// Otevře dialog pro úpravu vybraného tréninku
        /// </summary>
        private void DgTreninky_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TreninkView vybranyTrenink = dgTreninky.SelectedItem as TreninkView;

            if (vybranyTrenink == null)
            {
                MessageBox.Show("Prosím vyberte trénink, který chcete editovat! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = uzivatel.Role.ToLower();

            if (role == "hrac" || role == "host" || role == "uzivatel")
            {
                MessageBox.Show("Nemáte oprávnění upravovat tréninky",
                                "Omezení přístupu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            DialogEditujTrenink dialogEditujTrenink = new DialogEditujTrenink(vybranyTrenink, this);
            dialogEditujTrenink.ShowDialog();   

        }

        /// <summary>
        /// Otevře dialog pro přidání nového tréninku
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejTrenink dialogPridejTrenink = new DialogPridejTrenink(TreneriOkno.TreneriData);
            dialogPridejTrenink.ShowDialog();
        }

        /// <summary>
        /// Smaže vybraný trénink po potvrzení uživatele
        /// </summary>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            TreninkView vybranyTrenink = dgTreninky.SelectedItem as TreninkView;

            if (vybranyTrenink == null)
            {
                MessageBox.Show("Prosím, vyberte trénink, který chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            var potvrzeni = MessageBox.Show($"Opravdu chcete odebrat trénink trenéra {vybranyTrenink.Prijmeni} v {vybranyTrenink.Datum}?", "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Odebrání tréninku
                    DatabaseTreninky.DeleteTrenink(conn, vybranyTrenink);

                    TreninkyData.Remove(vybranyTrenink);
                }

                // Úspěch
                MessageBox.Show(
                    $"Trénink trenéra {vybranyTrenink.Prijmeni} v {vybranyTrenink.Datum} byl úspěšně odebrán",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání tréninku:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání tréninku:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Načte všechny tréninky z databázového pohledu TRENINKY_VIEW
        /// Ukládá je do kolekce TreninkyData pro zobrazení v DataGridu
        /// </summary>
        private void NactiTreninky()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM TRENINKY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                // Vyprázdníme kolekci, aby se nepřidávaly duplicitní záznamy
                TreninkyData.Clear();

                while (reader.Read())
                {
                    TreninkView trenink = new TreninkView();

                    // RODNE_CISLO - NOT NULL
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        trenink.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        trenink.RodneCislo = "";

                    // Příjmení (NOT NULL) 
                    if (reader["PRIJMENI"] != DBNull.Value)
                        trenink.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        trenink.Prijmeni = "";

                    // Trénink - Popis (volitelný sloupec) 
                    if (reader["POPIS"] != DBNull.Value)
                        trenink.Popis = reader["POPIS"].ToString();
                    else
                        trenink.Popis = "Volitelné nezadáno !";

                    // Místo (NOT NULL)
                    if (reader["MISTO"] != DBNull.Value)
                        trenink.Misto = reader["MISTO"].ToString();
                    else
                        trenink.Misto = "";

                    // DATUM (NOT NULL)
                    if (reader["DATUM"] != DBNull.Value)
                        trenink.Datum = Convert.ToDateTime(reader["DATUM"]);
                    else
                        trenink.Datum = DateTime.MinValue;

                    // Přidáme trénink do kolekce pro DataGrid
                    TreninkyData.Add(trenink);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání tréninku:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgTreninky_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání tréninku klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgTreninky.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgTreninky.Focusable = false;
                Keyboard.ClearFocus();
                dgTreninky.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu pro zjištění statistik tréninků
        /// </summary>
        /// <param name="sender">param</param>
        /// <param name="e">eventArgs</param>
        private void BtnStatistikyTreninku_Click(object sender, RoutedEventArgs e)
        {
            DialogStatistikyTreninku dialogStatistikyTreninku = new DialogStatistikyTreninku();
            dialogStatistikyTreninku.ShowDialog();
        }
    }
}
