using System;
using System.Collections.ObjectModel;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class SponzoriOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        // Kolekce sponzorů pro DataGrid (binding v XAML)
        public ObservableCollection<Sponzor> SponzoriData { get; set; } = new ObservableCollection<Sponzor>();

        public SponzoriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            DataContext = this; // propojení s DataGridem

            NactiSponzory();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        /// <summary>
        /// Načte sponzory z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiSponzory()
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    string sql = "SELECT IDSPONZOR, JMENO, SPONZOROVANACASTKA FROM SPONZORI";

                    using (var cmd = new OracleCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        SponzoriData.Clear();

                        while (reader.Read())
                        {
                            SponzoriData.Add(new Sponzor
                            {
                                IdSponzor = reader["IDSPONZOR"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IDSPONZOR"]),
                                Jmeno = reader["JMENO"] == DBNull.Value ? "" : reader["JMENO"].ToString(),
                                SponzorovanaCastka = reader["SPONZOROVANACASTKA"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["SPONZOROVANACASTKA"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání sponzorů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
