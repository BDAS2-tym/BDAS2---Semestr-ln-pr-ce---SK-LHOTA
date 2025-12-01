using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
    /// Interaction logic for DialogVazebniTabulkaClenove.xaml
    /// </summary>
    public partial class DialogVazebniTabulkaClenove : Window
    {
        public ObservableCollection<ClenKlubu> VsichniClenove { get; set; } = new ObservableCollection<ClenKlubu>();

        public ObservableCollection<ClenKlubu> VybraniClenove { get; set; } = new ObservableCollection<ClenKlubu>();

        public DialogVazebniTabulkaClenove()
        {
            InitializeComponent();
            NactiCleny();

            // Nastavení DataContextu
            DataContext = this;
        }

        public DialogVazebniTabulkaClenove(ObservableCollection<ClenKlubu> vazbyClenove)
        {
            InitializeComponent();
            NactiCleny();

            // Vyfiltrování a zobrazení pouze těch členů, kteří ještě nejsou ve vazbě se sponzorem (gridVazby)
            VybraniClenove = new ObservableCollection<ClenKlubu>(vazbyClenove);
            ObservableCollection<ClenKlubu> ostatniNezarazeni = new ObservableCollection<ClenKlubu>
            (
                VsichniClenove.Where(vsichni => !VybraniClenove.Any(vybrani => vybrani.IdClenKlubu == vsichni.IdClenKlubu))
            );

            VsichniClenove.Clear();
            foreach (ClenKlubu clen in ostatniNezarazeni)
            {
                VsichniClenove.Add(clen);
            }

            // Nastavení DataContextu
            DataContext = this;
        }

        /// <summary>
        /// Metoda slouží k nastavení DialogResult na TRUE a zároveň zavře dané dialogové okno
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k nastavení DialogResult na FALSE a zároveň zavře dané dialogové okno
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnZrusit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Metoda načte sponzory z databáze přes DatabaseManager a naplní tabulku VŠICHNI ČLENOVÉ
        /// </summary>
        private void NactiCleny()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM HRACI_OPATRENI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                VsichniClenove.Clear();

                while (reader.Read())
                {
                    ClenKlubu clenKlubu = new ClenKlubu();

                    // IDCLENKLUBU - NOT NULL
                    if (reader["IDCLENKLUBU"] != DBNull.Value)
                        clenKlubu.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);

                    // RODNE_CISLO - NOT NULL
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        clenKlubu.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        clenKlubu.RodneCislo = 0L;

                    // JMENO - NOT NULL
                    if (reader["JMENO"] != DBNull.Value)
                        clenKlubu.Jmeno = reader["JMENO"].ToString();
                    else
                        clenKlubu.Jmeno = "";

                    // PRIJMENI - NOT NULL
                    if (reader["PRIJMENI"] != DBNull.Value)
                        clenKlubu.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        clenKlubu.Prijmeni = "";                   

                    VsichniClenove.Add(clenKlubu);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání sponzorů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k přesunutí členů z tabulky VŠICHNI ČLENOVÉ do tabulky SPONZOROVANÍ ČLENOVÉ
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPresunVsechny_Click(object sender, RoutedEventArgs e)
        {
            var vybraniClenove = gridVsechny.SelectedItems.Cast<ClenKlubu>().ToList();
            if (vybraniClenove.Count == 0)
            {
                MessageBox.Show("Není vybraný žádný člen pro přesun.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Přesunutí všech vybraných členů
            foreach (ClenKlubu vybranyclen in vybraniClenove)
            {
                // Kontrola, jestli už přesouvaný člen neexistuje v tabulce Sponzorovaní Členové
                if (VybraniClenove.Any(c => c.IdClenKlubu == vybranyclen.IdClenKlubu))
                {
                    MessageBox.Show($"Vybraný člen {vybranyclen.Jmeno} {vybranyclen.Prijmeni} nemůže být přesunut, protože už existuje v tabulce {gridVazby.Name}!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VybraniClenove.Add(vybranyclen);
                VsichniClenove.Remove(vybranyclen);
            }
        }

        /// <summary>
        /// Metoda slouží k přesunutí členů z tabulky SPONZOROVANÍ ČLENOVÉ do tabulky VŠICHNI ČLENOVÉ
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPresunVazby_Click(object sender, RoutedEventArgs e)
        {
            var vybraniClenove = gridVazby.SelectedItems.Cast<ClenKlubu>().ToList();
            if (vybraniClenove.Count == 0)
            {
                MessageBox.Show("Není vybraný žádný člen pro přesun.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Přesunutí všech vybraných členů
            foreach (ClenKlubu vybranyclen in vybraniClenove)
            {
                // Kontrola, jestli už přesouvaný člen neexistuje v tabulce Všichni Členové
                if (VsichniClenove.Any(c => c.IdClenKlubu == vybranyclen.IdClenKlubu))
                {
                    MessageBox.Show($"Vybraný člen {vybranyclen.Jmeno} {vybranyclen.Prijmeni} nemůže být přesunut, protože už existuje v tabulce {gridVsechny.Name}!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VybraniClenove.Remove(vybranyclen);
                VsichniClenove.Add(vybranyclen);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void gridVazby_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání vazby klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                gridVazby.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                gridVazby.Focusable = false;
                Keyboard.ClearFocus();
                gridVazby.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void gridVsechny_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání vazby klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                gridVsechny.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                gridVsechny.Focusable = false;
                Keyboard.ClearFocus();
                gridVsechny.Focusable = true;
            }
        }
    }
}
