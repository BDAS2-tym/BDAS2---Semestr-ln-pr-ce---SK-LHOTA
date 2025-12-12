using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class HlavniOkno : Window
    {
        private static Class.Uzivatel? prihlasenyUzivatel;

        public HlavniOkno()
        {
            InitializeComponent();

            var vm = new HlavniOknoViewModel();
            vm.RequestNavigate += Vm_RequestNavigate;
            vm.RequestLogout += Vm_RequestLogout;
            DataContext = vm;
        }

        public static void NastavPrihlaseneho(Class.Uzivatel uzivatel) => prihlasenyUzivatel = uzivatel;
        public static Class.Uzivatel? GetPrihlasenyUzivatel() => prihlasenyUzivatel;

        private void Vm_RequestLogout()
        {
            prihlasenyUzivatel = null;
            new PrihlaseniOkno(this).Show();
            Close();
        }

        private void Vm_RequestNavigate(MainNavigationTarget target)
        {
            Window w = target switch
            {
                MainNavigationTarget.Hraci => new HraciOkno(this),
                MainNavigationTarget.Treneri => new TreneriOkno(this),
                MainNavigationTarget.Treninky => new TreninkyOkno(this),
                MainNavigationTarget.Kontrakty => new KontraktyOkno(this),
                MainNavigationTarget.Sponzori => new SponzoriOkno(this),
                MainNavigationTarget.Grafy => new GrafyOkno(this),
                MainNavigationTarget.Souteze => new SoutezeOkno(this),
                MainNavigationTarget.Zapasy => new ZapasyOkno(this),
                MainNavigationTarget.Nastaveni => new NastaveniOkno(this),
                _ => new ZapasyOkno(this)
            };

            w.Show();
            Hide();
        }
    }
}
