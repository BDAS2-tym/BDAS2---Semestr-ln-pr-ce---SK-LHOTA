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
    /// Interaction logic for LogTableOkno.xaml
    /// </summary>
    public partial class LogTableOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;
        private bool zavrenoTlacitkem = false;


        // Kolekce soutěží pro DataGrid (binding v XAML)
        public ObservableCollection<Zaznam> ZaznamyData { get; set; } = new ObservableCollection<Zaznam>();

        public LogTableOkno(HlavniOkno hlavniOkno)
        {
            // TODO Dodělat zobrazení do tabulky

            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Propojení kolekce s DataGridem
            DataContext = this;

            NactiZaznamy();
        }

        /// <summary>
        /// Metoda načte záznamy z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiZaznamy()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM LOG_TABLE_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                ZaznamyData.Clear();

                while (reader.Read())
                {
                    Zaznam zaznam = new Zaznam();

                    // IDLOG - NOT NULL
                    if (reader["IDLOG"] != DBNull.Value)
                        zaznam.IdZaznam = Convert.ToInt32(reader["IDLOG"]);

                    // OPERACE - NOT NULL
                    if (reader["OPERACE"] != DBNull.Value)
                        zaznam.Operace = reader["OPERACE"].ToString();
                    else
                        zaznam.Operace = "";

                    // CAS - NOT NULL
                    if (reader["CAS"] != DBNull.Value)
                        zaznam.Cas = reader.GetOracleTimeStamp(reader.GetOrdinal("CAS")).Value;

                    // UZIVATEL - NOT NULL
                    if (reader["UZIVATEL"] != DBNull.Value)
                    {
                        Uzivatel uzivatel = new Uzivatel
                        {
                            UzivatelskeJmeno = reader["UZIVATEL"].ToString()
                        };

                        zaznam.Uzivatel = uzivatel;
                    }

                    // TABULKA - NOT NULL
                    if (reader["TABULKA"] != DBNull.Value)
                        zaznam.Tabulka = reader["TABULKA"].ToString();
                    else
                        zaznam.Tabulka = "";

                    ZaznamyData.Add(zaznam);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soutěží:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k vrácení se na okno nastavení
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnZpet_Click(object sender, RoutedEventArgs e)
        {
            zavrenoTlacitkem = true;    // označíme, že zavírání je úmyslné
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Ukončí aplikaci stistknutím X
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (zavrenoTlacitkem == false)
            {
                // zavřeno přes X → ukončit aplikaci
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dgZaznamy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;
                MessageBox.Show("Smazání záznamu klávesou Delete není povoleno", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgZaznamy.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgZaznamy.Focusable = false;
                Keyboard.ClearFocus();
                dgZaznamy.Focusable = true;
            }           
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované záznamy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiZaznam dialogNajdiZaznam = new DialogNajdiZaznam(ZaznamyData);
            bool? vysledekDiaOkna = dialogNajdiZaznam.ShowDialog();            

            if(vysledekDiaOkna == true)
            {
                if (dialogNajdiZaznam.VyfiltrovaneZaznamy.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry.", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgZaznamy.ItemsSource = new ObservableCollection<Zaznam>(dialogNajdiZaznam.VyfiltrovaneZaznamy);               
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
                dgZaznamy.ItemsSource = ZaznamyData;
                e.Handled = true;
            }
        }
    }
}
