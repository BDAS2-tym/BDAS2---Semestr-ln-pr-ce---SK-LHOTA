using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro okno Nastavení
    /// Drží stav dostupnosti (IsEnabled/Opacity) jednotlivých položek nastavení
    /// a vystavuje příkazy pro navigaci do podoken
    /// </summary>
    public class NastaveniOknoViewModel : ViewModelBase
    {

        /// <summary>
        /// Požadavek na otevření okna Binární obsah
        /// </summary>
        public event Action? RequestOpenBinarniObsah;

        /// <summary>
        /// Požadavek na otevření okna Uživatelské účty
        /// </summary>
        public event Action? RequestOpenUzivatele;

        /// <summary>
        /// Požadavek na otevření okna Systémový katalog
        /// </summary>
        public event Action? RequestOpenSystemovyKatalog;

        /// <summary>
        /// Požadavek na otevření okna Záznamy změn (log)
        /// </summary>
        public event Action? RequestOpenZmeny;

        /// <summary>
        /// Požadavek na návrat zpět (typicky do hlavního okna / předchozí obrazovky)
        /// </summary>
        public event Action? RequestBack;


        /// <summary>
        /// Určuje, zda je položka "Binární obsah" dostupná pro aktuální roli
        /// </summary>
        private bool _canBinarniObsah;
        /// <summary>
        /// Dostupnost funkce Binární obsah
        /// Bind na IsEnabled tlačítka
        /// </summary>
        public bool CanBinarniObsah
        {
            get { return _canBinarniObsah; }
            private set { _canBinarniObsah = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Určuje, zda je položka "Uživatelské účty" dostupná pro aktuální roli
        /// </summary>
        private bool _canUzivatele;
        /// <summary>
        /// Dostupnost funkce Uživatelské účty.
        /// Bind na IsEnabled tlačítka.
        /// </summary>
        public bool CanUzivatele
        {
            get { return _canUzivatele; }
            private set { _canUzivatele = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Určuje, zda je položka "Systémový katalog" dostupná pro aktuální roli.
        /// </summary>
        private bool _canSystemovyKatalog;
        /// <summary>
        /// Dostupnost funkce Systémový katalog.
        /// Bind na IsEnabled tlačítka.
        /// </summary>
        public bool CanSystemovyKatalog
        {
            get { return _canSystemovyKatalog; }
            private set { _canSystemovyKatalog = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Určuje, zda je položka "Záznamy změn" dostupná pro aktuální roli.
        /// </summary>
        private bool _canZmeny;
        /// <summary>
        /// Dostupnost funkce Záznamy změn.
        /// Bind na IsEnabled tlačítka.
        /// </summary>
        public bool CanZmeny
        {
            get { return _canZmeny; }
            private set { _canZmeny = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Průhlednost tlačítka pro Binární obsah (1.0 zapnuto, 0.2 vypnuto).
        /// </summary>
        private double _binarniObsahOpacity = 0.2;
        /// <summary>
        /// Bind na Opacity tlačítka "Binární obsah".
        /// </summary>
        public double BinarniObsahOpacity
        {
            get { return _binarniObsahOpacity; }
            private set { _binarniObsahOpacity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Průhlednost tlačítka pro Uživatelské účty (1.0 zapnuto, 0.2 vypnuto).
        /// </summary>
        private double _uzivateleOpacity = 0.2;
        /// <summary>
        /// Bind na Opacity tlačítka "Uživatelské účty".
        /// </summary>
        public double UzivateleOpacity
        {
            get { return _uzivateleOpacity; }
            private set { _uzivateleOpacity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Průhlednost tlačítka pro Systémový katalog (1.0 zapnuto, 0.2 vypnuto).
        /// </summary>
        private double _systemovyKatalogOpacity = 0.2;
        /// <summary>
        /// Bind na Opacity tlačítka "Systémový katalog".
        /// </summary>
        public double SystemovyKatalogOpacity
        {
            get { return _systemovyKatalogOpacity; }
            private set { _systemovyKatalogOpacity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Průhlednost tlačítka pro Záznamy změn (1.0 zapnuto, 0.2 vypnuto).
        /// </summary>
        private double _zmenyOpacity = 0.2;
        /// <summary>
        /// Bind na Opacity tlačítka "Záznamy změn".
        /// </summary>
        public double ZmenyOpacity
        {
            get { return _zmenyOpacity; }
            private set { _zmenyOpacity = value; OnPropertyChanged(); }
        }

        // ----- Commands -----

        /// <summary>
        /// Příkaz pro otevření okna Binární obsah.
        /// </summary>
        public RelayCommand OpenBinarniObsahCommand { get; private set; }

        /// <summary>
        /// Příkaz pro otevření okna Uživatelské účty.
        /// </summary>
        public RelayCommand OpenUzivateleCommand { get; private set; }

        /// <summary>
        /// Příkaz pro otevření okna Systémový katalog.
        /// </summary>
        public RelayCommand OpenSystemovyKatalogCommand { get; private set; }

        /// <summary>
        /// Příkaz pro otevření okna Záznamy změn.
        /// </summary>
        public RelayCommand OpenZmenyCommand { get; private set; }

        /// <summary>
        /// Příkaz pro návrat zpět.
        /// </summary>
        public RelayCommand BackCommand { get; private set; }


        public NastaveniOknoViewModel()
        {
            NastavPravaPodleRole();

            OpenBinarniObsahCommand = new RelayCommand(_ => RequestOpenBinarniObsah?.Invoke(), _ => CanBinarniObsah);
            OpenUzivateleCommand = new RelayCommand(_ => RequestOpenUzivatele?.Invoke(), _ => CanUzivatele);
            OpenSystemovyKatalogCommand = new RelayCommand(_ => RequestOpenSystemovyKatalog?.Invoke(), _ => CanSystemovyKatalog);
            OpenZmenyCommand = new RelayCommand(_ => RequestOpenZmeny?.Invoke(), _ => CanZmeny);
            BackCommand = new RelayCommand(_ => RequestBack?.Invoke(), _ => true);
        }

        /// <summary>
        /// Zjistí roli přihlášeného uživatele a nastaví dostupnost položek nastavení
        /// Pokud uživatel není přihlášen, bere se role "host".
        /// </summary>
        private void NastavPravaPodleRole()
        {
            // Default stav: vše zakázané
            VypniVse();

            Uzivatel prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            string role = "host";

            if (prihlaseny != null)
            {
                if (prihlaseny.Role != null)
                {
                    role = prihlaseny.Role.ToLower();
                }
            }

            // Admin má plný přístup
            if (role == "admin")
            {
                ZapniVse();
                return;
            }

            // Trenér má přístup jen k vybraným částem
            if (role == "trener")
            {
                ZapniBinarniObsah();
                ZapniUzivatele();
                return;
            }

            // Ostatní role nic nezapínají (zůstane default vypnuto)
        }

        /// <summary>
        /// Nastaví všechny funkce jako nedostupné (disabled) a ztmaví tlačítka (opacity 0.2).
        /// </summary>
        private void VypniVse()
        {
            CanBinarniObsah = false;
            CanUzivatele = false;
            CanSystemovyKatalog = false;
            CanZmeny = false;

            BinarniObsahOpacity = 0.2;
            UzivateleOpacity = 0.2;
            SystemovyKatalogOpacity = 0.2;
            ZmenyOpacity = 0.2;
        }

        /// <summary>
        /// Nastaví všechny funkce jako dostupné (enabled) a nastaví opacity na 1.0.
        /// Používá se pro roli admin.
        /// </summary>
        private void ZapniVse()
        {
            CanBinarniObsah = true;
            CanUzivatele = true;
            CanSystemovyKatalog = true;
            CanZmeny = true;

            BinarniObsahOpacity = 1.0;
            UzivateleOpacity = 1.0;
            SystemovyKatalogOpacity = 1.0;
            ZmenyOpacity = 1.0;
        }

        /// <summary>
        /// Povolí pouze binární obsah (IsEnabled + opacity).
        /// </summary>
        private void ZapniBinarniObsah()
        {
            CanBinarniObsah = true;
            BinarniObsahOpacity = 1.0;
        }

        /// <summary>
        /// Povolí pouze správu uživatelů (IsEnabled + opacity).
        /// </summary>
        private void ZapniUzivatele()
        {
            CanUzivatele = true;
            UzivateleOpacity = 1.0;
        }
    }
}
