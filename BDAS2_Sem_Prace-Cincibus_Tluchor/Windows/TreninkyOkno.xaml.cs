using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
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
    /// Interakční logika pro TreninkyOkno.xaml
    /// </summary>
    public partial class TreninkyOkno : Window
    {

        private HlavniOkno hlavniOkno;

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

        private void NastavViditelnostSloupcuProUzivatele()
        {
            // Zjistíme, kdo je přihlášený
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = uzivatel.Role.ToLower();

            // Nejdřív zobrazíme
            RodneCisloSloupec.Visibility = Visibility.Visible;

            // Pokud je to hráč, uživatel nebo trenér tyto sloupce a funkce tlačítek schováme
            if (role == "hrac" || role == "trener" || role == "uzivatel")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;

                btnPridej.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnPridej.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
            }
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        private void DgTreninky_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TreninkView vybranyTrenink = dgTreninky.SelectedItem as TreninkView;

            if (vybranyTrenink == null)
            {
                MessageBox.Show("Prosím vyberte trénink, který chcete editovat! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogEditujTrenink dialogEditujTrenink = new DialogEditujTrenink(vybranyTrenink, this);
            dialogEditujTrenink.ShowDialog();   

        }

        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejTrenink dialogPridejTrenink = new DialogPridejTrenink(TreneriOkno.TreneriData);
            dialogPridejTrenink.ShowDialog();
        }

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
                // Smazání z databáze
                DatabaseTreninky.DeleteTrenink(vybranyTrenink);

                // Aktualizace DataGridu (odebrání z kolekce)
                TreninkyData.Remove(vybranyTrenink);

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

                    // Rodné číslo (NOT NULL) 
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        trenink.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        trenink.RodneCislo = 0L;

                    // Příjmení (NOT NULL) 
                    if (reader["PRIJMENI"] != DBNull.Value)
                        trenink.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        trenink.Prijmeni = "";

                    // Trénink -> Popis (volitelný sloupec) 
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
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dgTreninky_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
