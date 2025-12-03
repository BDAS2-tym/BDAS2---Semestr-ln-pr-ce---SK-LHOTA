using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using LiveCharts;
using LiveCharts.Wpf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class GrafyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        // Datová sada pro koláčový graf hráčů a trenérů
        public SeriesCollection PomeryHracuTreneru { get; set; }

        // Datová sada pro sloupcový graf vstřelených gólů podle pozic
        public SeriesCollection IndexyPozic { get; set; }

        // Textové popisky pozic pro osu X
        public List<string> NazvyPozic { get; set; }

        public GrafyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            int pocetHracu = DatabaseHraci.GetPocetHracu();
            int pocetTreneru = DatabaseTreneri.GetPocetTreneru();

            if (pocetHracu == 0 && pocetTreneru == 0)
            {
                MessageBox.Show(
                    "V databázi nejsou žádná data. Vložte hráče nebo trenéra, aby bylo možné zobrazit grafy.",
                    "Žádná data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                this.Close();
                hlavniOkno.Show();
                return;
            }

            // Vytvoření datové kolekce pro koláčový graf
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

            // Textové názvy pozic pro osu X grafu
            NazvyPozic = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };

            // Číselné ID pozic podle databázového číselníku
            int[] idPozic = { 1, 2, 3, 4 };

            // Kolekce hodnot pro sloupcový graf (procenta gólů)
            var hodnotyPozic = new ChartValues<double>();

            // Volání PL/SQL funkce pro každou pozici a načtení procentuálního podílu
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    for (int i = 0; i < idPozic.Length; i++)
                    {
                        using (var cmd = new OracleCommand("PKG_HRACI.F_GOLY_PROCENTA_POZICE", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Návratová hodnota funkce
                            cmd.Parameters.Add("return_value", OracleDbType.Double, ParameterDirection.ReturnValue);

                            // Parametr funkce představující ID pozice
                            cmd.Parameters.Add("p_pozice", OracleDbType.Int32).Value = idPozic[i];

                            cmd.ExecuteNonQuery();

                            // Převod OracleDecimal na double
                            double hodnota = ((Oracle.ManagedDataAccess.Types.OracleDecimal)
                                cmd.Parameters["return_value"].Value).ToDouble();

                            hodnotyPozic.Add(hodnota);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chyba při načtení dat pro graf gólů podle pozic:\n{ex.Message}",
                    "Chyba databáze",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            // Sestavení sloupcového grafu ze získaných hodnot
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

            // Nastavení datového kontextu pro binding do XAML
            DataContext = this;
        }

        // Vrácení zpět do hlavního okna
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }
    }
}
