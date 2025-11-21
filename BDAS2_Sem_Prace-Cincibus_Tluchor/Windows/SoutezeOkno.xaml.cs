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
    /// Interakční logika pro SoutezeOkno.xaml
    /// </summary>
    public partial class SoutezeOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        // Kolekce soutěží pro DataGrid (binding v XAML)
        public ObservableCollection<Soutez> SoutezeData {  get; set; } = new ObservableCollection<Soutez>();

        public SoutezeOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Propojení kolekce s DataGridem
            DataContext = this;

            NactiSouteze();
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
        /// Metoda načte soutěže z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiSouteze()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM SOUTEZE_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                SoutezeData.Clear();

                while (reader.Read())
                {
                    Soutez soutez = new Soutez();

                    // IDSOUTEZ - NOT NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value)
                        soutez.IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]);

                    // STARTDATUM - NOT NULL
                    if (reader["STARTDATUM"] != DBNull.Value)
                        soutez.StartDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["STARTDATUM"]));

                    // KONECDATUM - NOT NULL
                    if (reader["KONECDATUM"] != DBNull.Value)
                        soutez.KonecDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["KONECDATUM"]));

                    // NAZEVSOUTEZE- NOT NULL
                    if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                        soutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                    else
                        soutez.TypSouteze = "";

                    SoutezeData.Add(soutez);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soutěží:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgSouteze_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání soutěže klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgSouteze.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgSouteze.Focusable = false;
                Keyboard.ClearFocus();
                dgSouteze.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k přidání soutěže do tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejSoutez dialogPridejSoutez = new DialogPridejSoutez(SoutezeData);
            dialogPridejSoutez.ShowDialog();
        }

        /// <summary>
        /// Metoda slouží k odebrání soutěže z tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Soutez? vybranaSoutez = dgSouteze.SelectedItem as Soutez;
            if (vybranaSoutez == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte soutěž, kterou chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            MessageBoxResult potvrzeni = MessageBox.Show($"Opravdu chcete odebrat soutěž " +
                $"{vybranaSoutez.TypSouteze}?", "Potvrzení odebrání",
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
                    DatabaseSouteze.OdeberSoutez(conn, vybranaSoutez);

                    SoutezeData.Remove(vybranaSoutez);
                }

                // Úspěch
                MessageBox.Show(
                    "Soutěž byla úspěšně odebrána.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání soutěže:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání soutěže:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }

        /// <summary>
        /// Metoda slouží k zobrazení editovacího dialogu soutěže
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgSouteze_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                Soutez? vybranaSoutez= (Soutez)row.Item;
                if (vybranaSoutez == null)
                {
                    MessageBox.Show("Prosím vyberte soutěž, kterou chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogEditujSoutez dialogEditujSoutez = new DialogEditujSoutez(vybranaSoutez, this);
                dialogEditujSoutez.ShowDialog();
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu, který zobrazuje celkové sponzorované částky jednotlivých soutěží
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnCastkySoutezi_Click(object sender, RoutedEventArgs e)
        {
            DialogSponzorovaneCastkySoutezi castky = new DialogSponzorovaneCastkySoutezi();
            castky.ShowDialog();
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
                dgSouteze.ItemsSource = SoutezeData;
                btnPridej.IsEnabled = btnOdeber.IsEnabled = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované soutěže zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiSoutez dialogNajdiSoutez = new DialogNajdiSoutez(SoutezeData);
            bool? vysledekDiaOkna = dialogNajdiSoutez.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiSoutez.VyfiltrovaneSouteze.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné soutěže se zadanými filtry.", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgSouteze.ItemsSource = new ObservableCollection<Soutez>(dialogNajdiSoutez.VyfiltrovaneSouteze);
                jeVyhledavaniAktivni = true;

                btnPridej.IsEnabled = btnOdeber.IsEnabled = false;
            }
        }
    }
}
