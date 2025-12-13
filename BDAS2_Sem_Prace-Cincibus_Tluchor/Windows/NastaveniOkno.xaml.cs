using System;
using System.Windows;
using System.Windows.Input;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class NastaveniOkno : Window
    {

        public static NastaveniOkno Instance { get; private set; }

        private readonly HlavniOkno hlavniOkno;

        public NastaveniOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            this.hlavniOkno = hlavniOkno;

            // nastaví aktuální instanci
            Instance = this;

            var vm = new NastaveniOknoViewModel();

            vm.RequestOpenBinarniObsah += Vm_RequestOpenBinarniObsah;
            vm.RequestOpenUzivatele += Vm_RequestOpenUzivatele;
            vm.RequestOpenSystemovyKatalog += Vm_RequestOpenSystemovyKatalog;
            vm.RequestOpenZmeny += Vm_RequestOpenZmeny;
            vm.RequestBack += Vm_RequestBack;

            DataContext = vm;
        }

        /// <summary>
        /// Umožní přesouvání okna podržením levého tlačítka myši (WindowStyle=None)
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Vm_RequestOpenBinarniObsah()
        {
            var okno = new BinarniObsahOkno();
            okno.Show();
            Hide();
        }

        private void Vm_RequestOpenUzivatele()
        {
            var okno = new NastaveniUzivateleOkno(hlavniOkno);
            okno.Show();
            Hide();
        }

        private void Vm_RequestOpenSystemovyKatalog()
        {
            var okno = new SystemovyKatalogOkno();
            okno.Show();
            Hide();
        }

        private void Vm_RequestOpenZmeny()
        {
            var okno = new LogTableOkno(hlavniOkno);
            okno.Show();
            Hide();
        }

        private void Vm_RequestBack()
        {
            if (hlavniOkno != null)
            {
                hlavniOkno.Show();
            }
            Hide();
        }

        /// <summary>
        /// Při zavření okna vynuluje instanci
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (ReferenceEquals(Instance, this))
            {
                Instance = null;
            }
        }
    }
}
