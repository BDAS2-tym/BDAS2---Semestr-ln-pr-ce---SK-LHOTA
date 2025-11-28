using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno zobrazující statistické grafy načtené z databáze
    /// Koláčový graf hráčů a trenérů a sloupcový graf podílu gólů podle pozic
    /// </summary>
    public partial class GrafyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        /// <summary>
        /// Data pro koláčový graf poměru hráčů a trenérů
        /// </summary>
        public SeriesCollection PomeryHracuTreneru { get; set; }

        /// <summary>
        /// Data pro sloupcový graf podílu gólů podle pozic
        /// </summary>
        public SeriesCollection IndexyPozic { get; set; }

        /// <summary>
        /// Popisky X-osy pro sloupcový graf
        /// </summary>
        public List<string> NazvyPozic { get; set; }

        /// <summary>
        /// Konstruktor okna grafů, načítá data z databáze a vytváří obě datové série
        /// </summary>
        public GrafyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Koláčový graf: hráči vs trenéři 
            int pocetHracu = DatabaseHraci.GetPocetHracu();
            int pocetTreneru = DatabaseTreneri.GetPocetTreneru();

            // Zabrání úplně prázdnému grafu
            if (pocetHracu == 0 && pocetTreneru == 0)
                pocetHracu = 1;

            PomeryHracuTreneru = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Hráči",
                    Values = new ChartValues<int> { pocetHracu },
                    Fill = new SolidColorBrush(Color.FromRgb(72,118,255)),
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Trenéři",
                    Values = new ChartValues<int> { pocetTreneru },
                    Fill = new SolidColorBrush(Color.FromRgb(255,99,71)),
                    DataLabels = true
                }
            };

            // Sloupcový graf: procenta gólů podle pozic
            NazvyPozic = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };
            var hodnotyPozic = new ChartValues<double>();

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    foreach (string pozice in NazvyPozic)
                    {
                        using (var cmd = new OracleCommand("SELECT PKG_HRACI.F_GOLY_PROCENTA_POZICE(:p) FROM DUAL", conn))
                        {
                            cmd.Parameters.Add("p", pozice);
                            object result = cmd.ExecuteScalar();

                            double hodnota = result == DBNull.Value ? 0 : Convert.ToDouble(result);
                            hodnotyPozic.Add(hodnota);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načtení dat pro graf gólů podle pozic\n{ex.Message}",
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            IndexyPozic = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Podíl gólů (%)",
                    Values = hodnotyPozic,
                    DataLabels = true,
                    LabelPoint = v => v.Y.ToString("F2") + " %",
                    MaxColumnWidth = 60
                }
            };

            DataContext = this;
        }

        /// <summary>
        /// Zavře okno a vrátí uživatele do hlavního okna
        /// </summary>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }
    }
}
