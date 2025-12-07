using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for DialogHierarchieSoutezi.xaml
    /// </summary>
    public partial class DialogHierarchieSoutezi : Window
    {
        public List<string>? HierarchieSoutezi { get; set; } = new List<string>();

        public DialogHierarchieSoutezi()
        {
            InitializeComponent();

            HierarchieSoutezi = NactiHierarchii();

            DataContext = this;
        }

        /// <summary>
        /// Metoda slouží k zavření dialogového okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k načtení hierarchie Soutěží --> Zápasů --> Výsledků zápasů z databáze
        /// </summary>
        /// <returns>Vrací textový seznam celé hierarchie, pokud se vyvolá vyjímka vrací NULL</returns>
        private List<string>? NactiHierarchii()
        {
            try
            {
                List<string> vysledky = new List<string>();

                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("PKG_SOUTEZE.SP_VYPIS_HIERARCHII_SOUTEZI", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string radek = reader.GetString(reader.GetOrdinal("TEXT_RADKU"));
                            vysledky.Add(radek);
                        }
                    }

                    if (vysledky.Count == 0)
                    {
                        MessageBox.Show($"Nebyly nalezeny žádné soutěže", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    return vysledky;

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soutěží/zápasů/výsledků:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
