using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro okno správy uživatelských účtů.
    /// Drží data pro DataGrid, řeší práva podle role a vystavuje příkazy pro UI.
    /// </summary>
    public class NastaveniUzivateleOknoViewModel : ViewModelBase
    {
        private readonly HlavniOkno _hlavniOkno;

        /// <summary>
        /// Kompletní kolekce uživatelů (zdroj).
        /// </summary>
        public ObservableCollection<Uzivatel> UzivateleData { get; } = new ObservableCollection<Uzivatel>();

        private ObservableCollection<Uzivatel> _zobrazovaniUzivatele = new ObservableCollection<Uzivatel>();
        /// <summary>
        /// Kolekce, která je napojená na DataGrid (normál / vyhledávací mód).
        /// </summary>
        public ObservableCollection<Uzivatel> ZobrazovaniUzivatele
        {
            get { return _zobrazovaniUzivatele; }
            private set { _zobrazovaniUzivatele = value; OnPropertyChanged(); }
        }

        private Uzivatel _selectedUzivatel;
        /// <summary>
        /// Aktuálně vybraný uživatel v DataGridu.
        /// </summary>
        public Uzivatel SelectedUzivatel
        {
            get { return _selectedUzivatel; }
            set { _selectedUzivatel = value; OnPropertyChanged(); }
        }

        private bool _jeVyhledavaniAktivni;
        /// <summary>
        /// Indikuje aktivní vyhledávací mód (CTRL+X vrací zpět).
        /// </summary>
        public bool JeVyhledavaniAktivni
        {
            get { return _jeVyhledavaniAktivni; }
            private set { _jeVyhledavaniAktivni = value; OnPropertyChanged(); }
        }

        // --- práva / UI stav ---

        private string _role = "host";

        private bool _canPridej;
        public bool CanPridej { get { return _canPridej; } private set { _canPridej = value; OnPropertyChanged(); } }

        private bool _canOdeber;
        public bool CanOdeber { get { return _canOdeber; } private set { _canOdeber = value; OnPropertyChanged(); } }

        private bool _canEdituj;
        public bool CanEdituj { get { return _canEdituj; } private set { _canEdituj = value; OnPropertyChanged(); } }

        private bool _canNajdi;
        public bool CanNajdi { get { return _canNajdi; } private set { _canNajdi = value; OnPropertyChanged(); } }

        private bool _canNotifikace;
        public bool CanNotifikace { get { return _canNotifikace; } private set { _canNotifikace = value; OnPropertyChanged(); } }

        private double _pridejOpacity = 1;
        public double PridejOpacity { get { return _pridejOpacity; } private set { _pridejOpacity = value; OnPropertyChanged(); } }

        private double _odeberOpacity = 1;
        public double OdeberOpacity { get { return _odeberOpacity; } private set { _odeberOpacity = value; OnPropertyChanged(); } }

        private double _editujOpacity = 1;
        public double EditujOpacity { get { return _editujOpacity; } private set { _editujOpacity = value; OnPropertyChanged(); } }

        private double _najdiOpacity = 1;
        public double NajdiOpacity { get { return _najdiOpacity; } private set { _najdiOpacity = value; OnPropertyChanged(); } }

        private double _notifikaceOpacity = 1;
        public double NotifikaceOpacity { get { return _notifikaceOpacity; } private set { _notifikaceOpacity = value; OnPropertyChanged(); } }

        /// <summary>
        /// True => uživatel vidí citlivý sloupec Rodné číslo a Akce.
        /// View si podle toho nastaví viditelnost sloupců (v code-behindu).
        /// </summary>
        public bool ShowSensitiveColumns
        {
            get
            {
                if (_role == "admin")
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// True => zobrazit sloupec Akce (Emulovat)
        /// </summary>
        public bool ShowAkceColumn
        {
            get
            {
                if (_role == "admin")
                {
                    return true;
                }
                return false;
            }
        }

        // --- commands ---

        public RelayCommand PridejCommand { get; private set; }
        public RelayCommand OdeberCommand { get; private set; }
        public RelayCommand EditujCommand { get; private set; }
        public RelayCommand NajdiCommand { get; private set; }
        public RelayCommand NotifikaceCommand { get; private set; }
        public RelayCommand ZpetCommand { get; private set; }

        public RelayCommand CancelSearchCommand { get; private set; } // CTRL+X
        public RelayCommand EmulovatCommand { get; private set; }     

        // --- požadavky na View (otevírání oken/dialogů) ---

        public event Action? RequestOpenRegistrace;
        public event Action<Uzivatel>? RequestOpenEditUzivatel;
        public event Action? RequestOpenNajdiDialog;
        public event Action? RequestOpenNotifikace;
        public event Action? RequestBack;

        public event Action? RequestRestartMain;  
        public event Action? RequestCloseWindow;  

        /// <summary>
        /// Konstruktor – nastaví role, práva, načte uživatele
        /// </summary>
        public NastaveniUzivateleOknoViewModel(HlavniOkno hlavniOkno)
        {
            _hlavniOkno = hlavniOkno;

            NastavRoli();
            NastavPrava();

            PridejCommand = new RelayCommand(_ => Pridej(), _ => CanPridej);
            OdeberCommand = new RelayCommand(_ => Odeber(), _ => CanOdeber);
            EditujCommand = new RelayCommand(_ => Edituj(), _ => CanEdituj);
            NajdiCommand = new RelayCommand(_ => Najdi(), _ => CanNajdi);
            NotifikaceCommand = new RelayCommand(_ => Notifikace(), _ => CanNotifikace);
            ZpetCommand = new RelayCommand(_ => Zpet(), _ => true);

            CancelSearchCommand = new RelayCommand(_ => CancelSearch(), _ => JeVyhledavaniAktivni);

            EmulovatCommand = new RelayCommand(u => Emulovat(u as Uzivatel), _ => ShowAkceColumn);

            NactiUzivatele();
        }

        private void NastavRoli()
        {
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = "host";

            if (uzivatel != null)
            {
                if (uzivatel.Role != null)
                {
                    role = uzivatel.Role.ToLower();
                }
            }

            _role = role;

            OnPropertyChanged(nameof(ShowSensitiveColumns));
            OnPropertyChanged(nameof(ShowAkceColumn));
        }

        private void NastavPrava()
        {
            // defaultně všechno vypnout
            CanPridej = false;
            CanEdituj = false;
            CanOdeber = false;
            CanNajdi = false;
            CanNotifikace = false;

            PridejOpacity = 0.2;
            EditujOpacity = 0.2;
            OdeberOpacity = 0.2;
            NajdiOpacity = 0.2;
            NotifikaceOpacity = 0.2;

            if (_role == "admin")
            {
                CanPridej = true;
                CanEdituj = true;
                CanOdeber = true;
                CanNajdi = true;
                CanNotifikace = true;

                PridejOpacity = 1;
                EditujOpacity = 1;
                OdeberOpacity = 1;
                NajdiOpacity = 1;
                NotifikaceOpacity = 1;

                return;
            }

            if (_role == "trener")
            {
                // trenér může jen notifikace
                CanNotifikace = true;
                NotifikaceOpacity = 1;
                return;
            }

            // ostatní role nic
        }

        /// <summary>
        /// Načte uživatele z databáze, naplní UzivateleData a přepne grid do normálního režimu.
        /// </summary>
        public void NactiUzivatele()
        {
            try
            {
                UzivateleData.Clear();

                OracleConnection conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT * FROM PREHLED_UZIVATELSKE_UCTY", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Uzivatel uzivatel = new Uzivatel();

                        if (reader["UZIVATELSKEJMENO"] != DBNull.Value)
                            uzivatel.UzivatelskeJmeno = reader["UZIVATELSKEJMENO"].ToString();
                        else
                            uzivatel.UzivatelskeJmeno = "";

                        if (reader["ROLE"] != DBNull.Value)
                            uzivatel.Role = reader["ROLE"].ToString();
                        else
                            uzivatel.Role = "";

                        if (reader["EMAIL"] != DBNull.Value)
                            uzivatel.Email = reader["EMAIL"].ToString();
                        else
                            uzivatel.Email = "";

                        if (reader["RODNE_CISLO"] != DBNull.Value)
                            uzivatel.RodneCislo = reader["RODNE_CISLO"].ToString();
                        else
                            uzivatel.RodneCislo = "";

                        if (reader["POSLEDNIPRIHLASENI"] != DBNull.Value)
                            uzivatel.PosledniPrihlaseni = Convert.ToDateTime(reader["POSLEDNIPRIHLASENI"]);
                        else
                            uzivatel.PosledniPrihlaseni = DateTime.MinValue;

                        UzivateleData.Add(uzivatel);
                    }
                }

                ZobrazovaniUzivatele = UzivateleData;
                JeVyhledavaniAktivni = false;
                OnPropertyChanged(nameof(JeVyhledavaniAktivni));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání uživatelů:\n" + ex.Message,
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- akce ---

        private void Pridej()
        {
            RequestOpenRegistrace?.Invoke();
        }

        private void Edituj()
        {
            if (SelectedUzivatel == null)
            {
                MessageBox.Show("Vyberte uživatele, kterého chcete upravit.", "Upozornění",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RequestOpenEditUzivatel?.Invoke(SelectedUzivatel);
        }

        private void Odeber()
        {
            if (SelectedUzivatel == null)
            {
                MessageBox.Show("Vyberte uživatele, kterého chcete odebrat", "Upozornění",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Uzivatel prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            if (prihlaseny != null)
            {
                if (SelectedUzivatel.UzivatelskeJmeno == prihlaseny.UzivatelskeJmeno)
                {
                    MessageBox.Show("Nelze odstranit účet, který je právě aktivní (emulovaný)!",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var potvrzeni = MessageBox.Show(
                "Opravdu chcete odebrat uživatele " + SelectedUzivatel.UzivatelskeJmeno + "?",
                "Potvrzení odstranění",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseRegistrace.DeleteUzivatel(conn, SelectedUzivatel);

                UzivateleData.Remove(SelectedUzivatel);

                MessageBox.Show("Uživatel " + SelectedUzivatel.UzivatelskeJmeno + " byl úspěšně odebrán.",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při odstraňování uživatele:\n" + ex.Message,
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Najdi()
        {
            RequestOpenNajdiDialog?.Invoke();
        }

        private void Notifikace()
        {
            RequestOpenNotifikace?.Invoke();
        }

        private void Zpet()
        {
            RequestBack?.Invoke();
        }

        private void CancelSearch()
        {
            if (!JeVyhledavaniAktivni)
            {
                return;
            }

            ZobrazovaniUzivatele = UzivateleData;
            JeVyhledavaniAktivni = false;
            OnPropertyChanged(nameof(JeVyhledavaniAktivni));
        }

        /// <summary>
        /// View zavolá po úspěšném dialogu hledání.
        /// </summary>
        public void ApplySearchResults(ObservableCollection<Uzivatel> filtrovani)
        {
            if (filtrovani == null)
            {
                return;
            }

            ZobrazovaniUzivatele = filtrovani;
            JeVyhledavaniAktivni = true;
            OnPropertyChanged(nameof(JeVyhledavaniAktivni));
        }

        private void Emulovat(Uzivatel uzivatel)
        {
            if (uzivatel == null)
            {
                return;
            }

            var potvrzeni = MessageBox.Show(
                "Opravdu se chcete přepnout na účet: " + uzivatel.UzivatelskeJmeno + "?",
                "Potvrzení přepnutí",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            HlavniOkno.NastavPrihlaseneho(uzivatel);

            MessageBox.Show(
                "Nyní jste přihlášen jako: " + uzivatel.UzivatelskeJmeno + " (" + uzivatel.Role + ")",
                "Úspěšné přepnutí",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            RequestRestartMain?.Invoke();
            RequestCloseWindow?.Invoke();
        }
    }
}
