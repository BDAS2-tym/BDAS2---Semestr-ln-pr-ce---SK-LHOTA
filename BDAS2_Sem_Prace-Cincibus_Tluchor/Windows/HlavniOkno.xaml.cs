using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{

    public partial class HlavniOkno : Window
    {
        private DispatcherTimer timer;
        private HraciOkno hraciOkno;
        private TreneriOkno treneriOkno;
        private TreninkyOkno treninkyOkno;

        public HlavniOkno()
        {
            InitializeComponent();
            this.hraciOkno = new HraciOkno(this);
            this.treneriOkno = new TreneriOkno(this);
            this.treninkyOkno = new TreninkyOkno(this);

            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void BtnHraci_Click(object sender, RoutedEventArgs e)
        {
            HraciOkno hraciOkno = new HraciOkno(this);
            hraciOkno.Show(); 
            this.Hide();
        }

        private void BtnTreneri_Click(object sender, RoutedEventArgs e) 
        {
            TreneriOkno treneriOkno = new TreneriOkno(this);
            treneriOkno.Show();
            this.Hide();
        }

        private void BtnTreninky_Click(object sender, RoutedEventArgs e)
        {
            TreninkyOkno treninkyOkno = new TreninkyOkno(this);
            treninkyOkno.Show();
            this.Hide();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtCas.Text = DateTime.Now.ToString("HH:mm:ss");   // aktualni cas
            txtDatum.Text = DateTime.Now.ToString("dd.MM.yyyy"); // aktualni datum
        }



    }
}
