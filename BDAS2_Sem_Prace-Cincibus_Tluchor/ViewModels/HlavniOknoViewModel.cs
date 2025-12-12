using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    public enum MainNavigationTarget
    {
        Hraci, Treneri, Treninky, Kontrakty, Opatreni, Sponzori, Grafy, Souteze, Zapasy, Nastaveni
    }

    public class HlavniOknoViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _timer;

        public event Action<MainNavigationTarget>? RequestNavigate;
        public event Action? RequestLogout;

        public HlavniOknoViewModel()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) => UpdateClock();
            _timer.Start();
            UpdateClock();

            PocetHracu = DatabaseHraci.GetPocetHracu();
            PocetTreneru = DatabaseTreneri.GetPocetTreneru();

            ApplyRights();

            OpenHraciCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Hraci), () => CanHraci);
            OpenTreneriCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Treneri), () => CanTreneri);
            OpenTreninkyCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Treninky), () => CanTreninky);
            OpenKontraktyCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Kontrakty), () => CanKontrakty);
            OpenOpatreniCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Opatreni), () => CanOpatreni);
            OpenSponzoriCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Sponzori), () => CanSponzori);
            OpenGrafyCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Grafy), () => CanGrafy);
            OpenSoutezeCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Souteze), () => CanSouteze);
            OpenZapasyCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Zapasy), () => CanZapasy);
            OpenNastaveniCommand = new RelayCommand(() => RequestNavigate?.Invoke(MainNavigationTarget.Nastaveni));

            LogoutCommand = new RelayCommand(() => RequestLogout?.Invoke());
        }

        // texty nahoře + boxy
        public string CasText { get; private set; } = "00:00:00";
        public string DatumText { get; private set; } = "00.00.0000";
        public int PocetHracu { get; private set; }
        public int PocetTreneru { get; private set; }
        public string PrihlasenyUzivatelText { get; private set; } = "Nepřihlášen";

        // práva
        public bool CanHraci { get; private set; }
        public bool CanTreneri { get; private set; }
        public bool CanTreninky { get; private set; }
        public bool CanSponzori { get; private set; }
        public bool CanKontrakty { get; private set; }
        public bool CanOpatreni { get; private set; }
        public bool CanSouteze { get; private set; }
        public bool CanGrafy { get; private set; }
        public bool CanZapasy { get; private set; }

        // commands
        public RelayCommand OpenHraciCommand { get; }
        public RelayCommand OpenTreneriCommand { get; }
        public RelayCommand OpenTreninkyCommand { get; }
        public RelayCommand OpenKontraktyCommand { get; }
        public RelayCommand OpenOpatreniCommand { get; }
        public RelayCommand OpenSponzoriCommand { get; }
        public RelayCommand OpenGrafyCommand { get; }
        public RelayCommand OpenSoutezeCommand { get; }
        public RelayCommand OpenZapasyCommand { get; }
        public RelayCommand OpenNastaveniCommand { get; }
        public RelayCommand LogoutCommand { get; }

        private void UpdateClock()
        {
            CasText = DateTime.Now.ToString("HH:mm:ss");
            DatumText = DateTime.Now.ToString("dd.MM.yyyy");
            OnPropertyChanged(nameof(CasText));
            OnPropertyChanged(nameof(DatumText));
        }

        private void ApplyRights()
        {
            var u = HlavniOkno.GetPrihlasenyUzivatel();
            if (u == null)
            {
                u = new Uzivatel { UzivatelskeJmeno = "Host", Role = "host" };
                HlavniOkno.NastavPrihlaseneho(u);
            }

            var role = (u.Role ?? "host").ToLower();
            PrihlasenyUzivatelText = $"{u.UzivatelskeJmeno} ({role})";
            OnPropertyChanged(nameof(PrihlasenyUzivatelText));

            CanHraci = CanTreneri = CanTreninky = CanSponzori = CanKontrakty = CanOpatreni = CanSouteze = CanGrafy = false;
            CanZapasy = false;

            switch (role)
            {
                case "admin":
                    CanHraci = CanTreneri = CanTreninky = CanSponzori = CanKontrakty = CanOpatreni = CanSouteze = CanGrafy = CanZapasy = true;
                    break;
                case "trener":
                    CanHraci = CanTreneri = CanOpatreni = CanKontrakty = CanGrafy = CanZapasy = true;
                    break;
                case "hrac":
                    CanHraci = CanTreneri = CanGrafy = CanTreninky = CanOpatreni = CanZapasy = true;
                    break;
                case "uzivatel":
                    CanGrafy = CanZapasy = CanTreninky = true;
                    break;
                default:
                    CanZapasy = true;
                    break;
            }

            OnPropertyChanged(nameof(CanHraci));
            OnPropertyChanged(nameof(CanTreneri));
            OnPropertyChanged(nameof(CanTreninky));
            OnPropertyChanged(nameof(CanSponzori));
            OnPropertyChanged(nameof(CanKontrakty));
            OnPropertyChanged(nameof(CanOpatreni));
            OnPropertyChanged(nameof(CanSouteze));
            OnPropertyChanged(nameof(CanGrafy));
            OnPropertyChanged(nameof(CanZapasy));
        }
    }
}
