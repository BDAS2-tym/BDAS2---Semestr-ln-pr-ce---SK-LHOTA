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
    /// Interaction logic for DialogVazebniTabulkaSouteze.xaml
    /// </summary>
    public partial class DialogVazebniTabulkaSouteze : Window
    {
        public ObservableCollection<Soutez> VsechnySouteze { get; set; } = new ObservableCollection<Soutez>();

        public ObservableCollection<Soutez> VybraneSouteze { get; set; } = new ObservableCollection<Soutez>();

        public DialogVazebniTabulkaSouteze()
        {
            InitializeComponent();
            NactiSouteze();

            // Nastavení DataContextu
            DataContext = this;
        }

        public DialogVazebniTabulkaSouteze(ObservableCollection<Soutez> vazbySouteze)
        {
            InitializeComponent();
            NactiSouteze();

            // Vyfiltrování a zobrazení pouze těch členů, kteří ještě nejsou ve vazbě se sponzorem (gridVazby)
            VybraneSouteze = new ObservableCollection<Soutez>(vazbySouteze);
            ObservableCollection<Soutez> ostatniNezarazene = new ObservableCollection<Soutez>
            (
                VsechnySouteze.Where(vsechny => !VybraneSouteze.Any(vybrane => vybrane.IdSoutez == vsechny.IdSoutez))
            );

            VsechnySouteze.Clear();
            foreach (Soutez soutez in ostatniNezarazene)
            {
                VsechnySouteze.Add(soutez);
            }

            // Nastavení DataContextu
            DataContext = this;
        }

        /// <summary>
        /// Metoda načte soutěže z databáze přes DatabaseManager a naplní tabulku VŠECHNY SOUTĚŽE
        /// </summary>
        private void NactiSouteze()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM SOUTEZE_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                VsechnySouteze.Clear();

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

                    // NAZEVSOUTEZE - NOT NULL
                    if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                        soutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                    else
                        soutez.TypSouteze = "";

                    VsechnySouteze.Add(soutez);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání sponzorů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k nastavení DialogResult na TRUE a zároveň zavře dané dialogové okno
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k nastavení DialogResult na FALSE a zároveň zavře dané dialogové okno
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnZrusit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k přesunutí soutěží z tabulky VŠECHNY SOUTĚŽE do tabulky SPONZOROVANÉ SOUTĚŽE
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPresunVsechny_Click(object sender, RoutedEventArgs e)
        {
            var vybraneSouteze = gridVsechny.SelectedItems.Cast<Soutez>().ToList();
            if (vybraneSouteze.Count == 0)
            {
                MessageBox.Show("Není vybraná žádná soutěž pro přesun.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Přesunutí všech vybraných soutěží
            foreach (Soutez vybranaSoutez in vybraneSouteze)
            {
                // Kontrola, jestli už přesouvaná soutěž neexistuje v tabulce Sponzorované Soutěže
                if (VybraneSouteze.Any(c => c.IdSoutez == vybranaSoutez.IdSoutez))
                {
                    MessageBox.Show($"Vybraný člen {vybranaSoutez.TypSouteze} nemůže být přesunut, protože už existuje v tabulce {gridVsechny.Name}!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VybraneSouteze.Add(vybranaSoutez);
                VsechnySouteze.Remove(vybranaSoutez);
            }
        }

        /// <summary>
        /// Metoda slouží k přesunutí soutěží z tabulky SPONZOROVANÉ SOUTĚŽE do tabulky VŠECHNY SOUTĚŽE
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPresunVazby_Click(object sender, RoutedEventArgs e)
        {
            var vybraneSouteze = gridVazby.SelectedItems.Cast<Soutez>().ToList();
            if (vybraneSouteze.Count == 0)
            {
                MessageBox.Show("Není vabraná žádná soutěž pro přesun.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Přesunutí všech vybraných soutěží
            foreach (Soutez vybranaSoutez in vybraneSouteze)
            {
                // Kontrola, jestli už přesouvaná soutěž neexistuje v tabulce Všechny Soutěže
                if (VsechnySouteze.Any(c => c.IdSoutez == vybranaSoutez.IdSoutez))
                {
                    MessageBox.Show($"Vybraná soutěž {vybranaSoutez.TypSouteze} nemůže být přesunuta, protože už existuje v tabulce {gridVsechny.Name}!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VybraneSouteze.Remove(vybranaSoutez);
                VsechnySouteze.Add(vybranaSoutez);
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
