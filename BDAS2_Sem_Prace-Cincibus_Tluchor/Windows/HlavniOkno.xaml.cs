using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class HlavniOkno : Window
    {
        private DispatcherTimer timer;

        public HlavniOkno()
        {
            InitializeComponent();

            // Timer pro aktuální čas a datum
            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            //Nastavení počtu hráčů při načtení okna
            txtPocetHracu.Text = DatabaseHraci.GetPocetHracu().ToString();

            //Nastavení počtu trenérů při načtení okna
            txtPocetTreneru.Text = DatabaseTreneri.GetPocetTreneru().ToString();
            
        }

        private void BtnHraci_Click(object sender, RoutedEventArgs e)
        {
            new HraciOkno(this).Show();
            this.Hide();
        }

        private void BtnTreneri_Click(object sender, RoutedEventArgs e)
        {
            new TreneriOkno(this).Show();
            this.Hide();
        }

        private void BtnTreninky_Click(object sender, RoutedEventArgs e)
        {
            new TreninkyOkno(this).Show();
            this.Hide();
        }

        private void BtnKontrakty_Click(object sender, RoutedEventArgs e)
        {
            new KontraktyOkno(this).Show();
            this.Hide();
        }

        private void BtnOpatreni_Click(object sender, RoutedEventArgs e)
        {
            new OpatreniOkno(this).Show();
            this.Hide();
        }

        private void BtnSponzori_Click(object sender, RoutedEventArgs e)
        {
            new SponzoriOkno(this).Show();
            this.Hide();
        }

        private void BtnGrafy_Click(object sender, RoutedEventArgs e)
        {
            new GrafyOkno(this).Show();
            this.Hide();
        }

        private void BtnSouteze_Click(object sender, RoutedEventArgs e)
        {
            new SoutezeOkno(this).Show();
            this.Hide();
        }

        private void BtnZapasy_Click(object sender, RoutedEventArgs e)
        {
            new ZapasyOkno(this).Show();
            this.Hide();
        }

        private void BtnOdhlaseni_Click(object sender, RoutedEventArgs e)
        {
            new PrihlaseniOkno(this).Show();
            this.Hide();

        }

        // ------------------- TIMER -------------------

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtCas.Text = DateTime.Now.ToString("HH:mm:ss");
            txtDatum.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
