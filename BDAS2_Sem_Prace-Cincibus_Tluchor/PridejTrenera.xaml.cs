<<<<<<< HEAD
﻿using System.Windows;
=======
﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
>>>>>>> df53511f18c22d4fbb99527265f1680cd246c043

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    
    public partial class PridejTrenera : Window
    {
<<<<<<< HEAD
        public PridejTrenera()
        {
            InitializeComponent();
=======
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
>>>>>>> df53511f18c22d4fbb99527265f1680cd246c043
        }

        private void Timer_Tick(object sender, EventArgs e) {
            txtCas.Text = DateTime.Now.ToString("HH:mm:ss");   // aktualni cas
            txtDatum.Text = DateTime.Now.ToString("dd.MM.yyyy"); // aktualni datum
        }

    }
}
