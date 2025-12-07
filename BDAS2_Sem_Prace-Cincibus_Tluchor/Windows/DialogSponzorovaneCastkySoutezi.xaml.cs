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
    /// Interaction logic for DialogSponzorovaneCastkySoutezi.xaml
    /// </summary>
    public partial class DialogSponzorovaneCastkySoutezi : Window
    {
        public ObservableCollection<object> Souteze { get; set; } = new ObservableCollection<object>();

        public DialogSponzorovaneCastkySoutezi()
        {
            InitializeComponent();

            NactiSouteze();
            DataContext = this;
        }

        /// <summary>
        /// Metoda načte sponzorvané soutěže a jejich celkové částky z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiSouteze()
        {
            try
            {
                var conn = DatabaseManager.GetConnection();


                using var cmd = new OracleCommand("SELECT * FROM CASTKY_SPONZOROVANYCH_SOUTEZI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                Souteze.Clear();

                while (reader.Read())
                {
                    Soutez soutez = new Soutez();

                    // IDSOUTEZ - NOT NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value)
                        soutez.IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]);

                    // NAZEVSOUTEZE- NOT NULL
                    if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                        soutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                    else
                        soutez.TypSouteze = "";

                    // CELKOVACASTKA - NOT NULL
                    long celkovaCastka = 0;
                    if (reader["CELKOVACASTKA"] != DBNull.Value)
                        celkovaCastka = Convert.ToInt64(reader["CELKOVACASTKA"]);
                    else
                        celkovaCastka = 0L;

                     // Vytvoření anonymního objektu
                     var zobrazData = new
                     {
                         soutez.TypSouteze,
                         CelkovaCastka = celkovaCastka
                      };

                    Souteze.Add(zobrazData);
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
        private void dgSouteze_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
