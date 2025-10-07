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

        // Kolekce sponzorů pro DataGrid
        public ObservableCollection<Sponzor> SponzoriData { get; set; } = new ObservableCollection<Sponzor>();

        public SponzoriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Nastaví datový kontext pro binding (např. DataGrid)
            this.DataContext = this;

            // Načte data z Oracle
            NactiSponzory();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        private void NactiSponzory()
        {
            string connString = "User Id=st72870;Password=databaze;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)))";

            try
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT IDSPONZOR, JMENO, SPONZOROVANACASTKA FROM SPONZORI";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        SponzoriData.Clear();

                        while (reader.Read())
                        {
                            SponzoriData.Add(new Sponzor
                            {
                                IdSponzor = GetValueOrDefault(reader, "IDSPONZOR", 0),
                                Jmeno = GetValueOrDefault(reader, "JMENO", string.Empty),
                                SponzorovanaCastka = GetValueOrDefault(reader, "SPONZOROVANACASTKA", 0m)
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

        /// <summary>
        /// Vrátí hodnotu ze sloupce nebo výchozí, pokud je NULL (DBNull.Value)
        /// </summary>
        private static T GetValueOrDefault<T>(OracleDataReader reader, string columnName, T defaultValue)
        {
            object value = reader[columnName];
            if (value == DBNull.Value || value == null)
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
