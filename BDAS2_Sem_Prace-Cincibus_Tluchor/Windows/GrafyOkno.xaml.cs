using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Media;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class GrafyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        public SeriesCollection PomeryHracuTreneru { get; set; }
        public SeriesCollection PomeryPozic { get; set; }

        public GrafyOkno(HlavniOkno hlavniOkno)
        {
            // Nastavení kontextu dat pro binding do XAML
           // DataContext = this;

            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Načtení aktuálních hodnot z databáze
            int pocetHracu = 0;
            int pocetTreneru = 0;

            try
            {
                pocetHracu = DatabaseHraci.GetPocetHracu();
                pocetTreneru = DatabaseTreneri.GetPocetTreneru();
            }
            catch
            {
                MessageBox.Show("Nepodařilo se načíst data z databáze pro graf.",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Zabránění prázdnému grafu
            if (pocetHracu == 0 && pocetTreneru == 0)
            {
                pocetHracu = 1;
            }

            // Koláčový graf – Poměr hráčů a trenérů
            PomeryHracuTreneru = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Hráči",
                    Values = new ChartValues<int> { pocetHracu },
                    Fill = Brushes.CornflowerBlue,
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Trenéři",
                    Values = new ChartValues<int> { pocetTreneru },
                    Fill = Brushes.OrangeRed,
                    DataLabels = true
                }
            };

            // Nastavení kontextu dat pro binding do XAML
            DataContext = this;
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }
    }
}
