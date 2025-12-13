using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// hlavního menu (navigace mezi okny/sekcemi aplikace)
    /// </summary>
    public enum MainNavigationTarget
    {
        Hraci, Treneri, Treninky, Kontrakty, Sponzori, Grafy, Souteze, Zapasy, Nastaveni
    }

    /// <summary>
    /// ViewModel hlavního okna
    /// práva dle role a vyvolává požadavky na navigaci/odhlášení
    /// </summary>
    public class HlavniOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// Interní timer pro aktualizaci času a data v UI (každou sekundu)
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Požadavek na navigaci do konkrétní sekce aplikace
        /// </summary>
        public event Action<MainNavigationTarget>? RequestNavigate;

        /// <summary>
        /// Požadavek na odhlášení uživatele
        /// </summary>
        public event Action? RequestLogout;

        /// <summary>
        /// Inicializuje timer, načte počty z DB, aplikuje práva dle role a vytvoří příkazy menu.
        /// </summary>
        public HlavniOknoViewModel()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) => UpdateClock();
            _timer.Start();
            UpdateClock();

            PocetHracu = DatabaseHraci.GetPocetHracu();
            PocetTreneru = DatabaseTreneri.GetPocetTreneru();

            ApplyRights();

            OpenHraciCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Hraci), _ => CanHraci);
            OpenTreneriCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Treneri), _ => CanTreneri);
            OpenTreninkyCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Treninky), _ => CanTreninky);
            OpenKontraktyCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Kontrakty), _ => CanKontrakty);
            OpenSponzoriCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Sponzori), _ => CanSponzori);
            OpenGrafyCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Grafy), _ => CanGrafy);
            OpenSoutezeCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Souteze), _ => CanSouteze);
            OpenZapasyCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Zapasy), _ => CanZapasy);
            OpenNastaveniCommand = new RelayCommand(_ => RequestNavigate?.Invoke(MainNavigationTarget.Nastaveni), _ => true);

            LogoutCommand = new RelayCommand(_ => RequestLogout?.Invoke());
        }

        /// <summary>
        /// Aktuální čas ve formátu HH:mm:ss zobrazovaný v hlavičce
        /// </summary>
        public string CasText { get; private set; } = "00:00:00";

        /// <summary>
        /// Aktuální datum ve formátu dd.MM.yyyy zobrazované v hlavičce
        /// </summary>
        public string DatumText { get; private set; } = "00.00.0000";

        /// <summary>
        /// Počet hráčů (pro info box na hlavní obrazovce)
        /// </summary>
        public int PocetHracu { get; private set; }

        /// <summary>
        /// Počet trenérů (pro info box na hlavní obrazovce)
        /// </summary>
        public int PocetTreneru { get; private set; }

        /// <summary>
        /// Text „uživatel (role)“ zobrazovaný v UI
        /// </summary>
        public string PrihlasenyUzivatelText { get; private set; } = "Nepřihlášen";

        /// <summary>
        /// Právo přístupu do sekce hráčů
        /// </summary>
        public bool CanHraci { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce trenérů
        /// </summary>
        public bool CanTreneri { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce tréninků
        /// </summary>
        public bool CanTreninky { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce sponzorů
        /// </summary>
        public bool CanSponzori { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce kontraktů
        /// </summary>
        public bool CanKontrakty { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce soutěží
        /// </summary>
        public bool CanSouteze { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce grafů/statistik
        /// </summary>
        public bool CanGrafy { get; private set; }

        /// <summary>
        /// Právo přístupu do sekce zápasů
        /// </summary>
        public bool CanZapasy { get; private set; }

        /// <summary>
        /// Příkaz pro otevření sekce hráčů 
        /// </summary>
        public RelayCommand OpenHraciCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce trenérů 
        /// </summary>
        public RelayCommand OpenTreneriCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce tréninků 
        /// </summary>
        public RelayCommand OpenTreninkyCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce kontraktů 
        /// </summary>
        public RelayCommand OpenKontraktyCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce sponzorů 
        /// </summary>
        public RelayCommand OpenSponzoriCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce grafů 
        /// </summary>
        public RelayCommand OpenGrafyCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce soutěží 
        /// </summary>
        public RelayCommand OpenSoutezeCommand { get; }

        /// <summary>
        /// Příkaz pro otevření sekce zápasů 
        /// </summary>
        public RelayCommand OpenZapasyCommand { get; }

        /// <summary>
        /// Příkaz pro otevření nastavení 
        /// </summary>
        public RelayCommand OpenNastaveniCommand { get; }

        /// <summary>
        /// Příkaz pro odhlášení uživatele
        /// </summary>
        public RelayCommand LogoutCommand { get; }

        /// <summary>
        /// Aktualizuje CasText a DatumText podle aktuálního systémového času.
        /// </summary>
        private void UpdateClock()
        {
            CasText = DateTime.Now.ToString("HH:mm:ss");
            DatumText = DateTime.Now.ToString("dd.MM.yyyy");
            OnPropertyChanged(nameof(CasText));
            OnPropertyChanged(nameof(DatumText));
        }

        /// <summary>
        /// Nastaví přihlášeného uživatele (pokud chybí, vytvoří hosta) a aplikuje práva podle role
        /// Zároveň vyvolá notifikace pro bindingy práv a textu uživatele
        /// </summary>
        private void ApplyRights()
        {
            var u = HlavniOkno.GetPrihlasenyUzivatel();
            if (u == null)
            {
                u = new Uzivatel { UzivatelskeJmeno = "Host", Role = "host" };
                HlavniOkno.NastavPrihlaseneho(u);
            }

            string role;

            if (u.Role == null)
            {
                role = "host";
            }
            else
            {
                role = u.Role;
            }

            role = role.ToLower();

            PrihlasenyUzivatelText = u.UzivatelskeJmeno + " (" + role + ")";
            OnPropertyChanged(nameof(PrihlasenyUzivatelText));

            CanHraci = CanTreneri = CanTreninky = CanSponzori = CanKontrakty  = CanSouteze = CanGrafy = false;
            CanZapasy = false;

            switch (role)
            {
                case "admin":
                    CanHraci = CanTreneri = CanTreninky = CanSponzori = CanKontrakty  = CanSouteze = CanGrafy = CanZapasy = true;
                    break;
                case "trener":
                    CanHraci = CanTreneri = CanKontrakty = CanGrafy = CanZapasy = true;
                    break;
                case "hrac":
                    CanHraci = CanTreneri = CanGrafy = CanTreninky = CanZapasy = true;
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
            OnPropertyChanged(nameof(CanSouteze));
            OnPropertyChanged(nameof(CanGrafy));
            OnPropertyChanged(nameof(CanZapasy));
        }
    }
}
