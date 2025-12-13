using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro okno Editace uživatele
    /// Načítá role z databáze, drží hodnoty formuláře, validuje vstupy
    /// a ukládá změny do databáze (UpdateUzivatel)
    /// </summary>
    public class EditUzivatelOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// Položka role pro ComboBox 
        /// </summary>
        public class RoleItem
        {
            /// <summary>
            /// ID role z databáze (ROLE_VIEW.IDROLE)
            /// </summary>
            public int IdRole { get; set; }

            /// <summary>
            /// Zobrazený název role (ROLE_VIEW.NAZEVROLE)
            /// </summary>
            public string NazevRole { get; set; } = "";

            public override string ToString()
            {
                return NazevRole;
            }
        }

        private readonly Uzivatel _uzivatel;
        private readonly string _stareJmeno;

        /// <summary>
        /// Událost pro zavření okna (View zavře okno)
        /// </summary>
        public event Action<bool>? RequestClose;

        /// <summary>
        /// Událost pro minimalizaci okna (View nastaví WindowState).
        /// </summary>
        public event Action? RequestMinimize;

        /// <summary>
        /// Seznam rolí pro ComboBox
        /// </summary>
        public ObservableCollection<RoleItem> RoleList { get; } = new ObservableCollection<RoleItem>();

        private RoleItem? _selectedRole;
        /// <summary>
        /// Aktuálně vybraná role v ComboBoxu.
        /// Změna role zároveň nastaví povolení rodného čísla
        /// </summary>
        public RoleItem? SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                UpdateRodneCisloState();
            }
        }

        private string _uzivatelskeJmeno = "";
        /// <summary>
        /// Uživatelské jméno účtu
        /// </summary>
        public string UzivatelskeJmeno
        {
            get { return _uzivatelskeJmeno; }
            set { _uzivatelskeJmeno = value; OnPropertyChanged(); }
        }

        private string _email = "";
        /// <summary>
        /// E-mail účtu
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _noveHeslo = "";
        /// <summary>
        /// Nové heslo (volitelné). Pokud je prázdné, heslo se nemění
        /// </summary>
        public string NoveHeslo
        {
            get { return _noveHeslo; }
            set { _noveHeslo = value; OnPropertyChanged(); }
        }

        private string _rodneCislo = "";
        /// <summary>
        /// Rodné číslo uživatele (povinné pro role hráč/trenér)
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        private bool _rodneCisloEnabled;
        /// <summary>
        /// Určuje, zda lze rodné číslo editovat (jen pro hráč/trenér).
        /// </summary>
        public bool RodneCisloEnabled
        {
            get { return _rodneCisloEnabled; }
            private set { _rodneCisloEnabled = value; OnPropertyChanged(); }
        }

        private double _rodneCisloOpacity = 1.0;
        /// <summary>
        /// Opacity pole rodného čísla (vizuální indikace zamčení)
        /// </summary>
        public double RodneCisloOpacity
        {
            get { return _rodneCisloOpacity; }
            private set { _rodneCisloOpacity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Příkaz pro uložení změn
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Příkaz pro zavření okna bez uložení
        /// </summary>
        public RelayCommand CloseCommand { get; }

        /// <summary>
        /// Příkaz pro minimalizaci okna
        /// </summary>
        public RelayCommand MinimizeCommand { get; }

        /// <summary>
        /// Vytvoří VM pro editaci konkrétního uživatele
        /// </summary>
        public EditUzivatelOknoViewModel(Uzivatel editovanyUzivatel)
        {
            if (editovanyUzivatel == null)
            {
                editovanyUzivatel = new Uzivatel();
            }

            _uzivatel = editovanyUzivatel;

            string stare = "";
            if (_uzivatel.UzivatelskeJmeno != null)
            {
                stare = _uzivatel.UzivatelskeJmeno;
            }
            _stareJmeno = stare;

            SaveCommand = new RelayCommand(_ => Save());
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke(false));
            MinimizeCommand = new RelayCommand(_ => RequestMinimize?.Invoke());

            NactiRoleZDb();
            NaplnFormularZUzivatele();
            UpdateRodneCisloState();
        }

        /// <summary>
        /// Načte role z databáze a naplní RoleList
        /// </summary>
        private void NactiRoleZDb()
        {
            RoleList.Clear();

            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT IDROLE, NAZEVROLE FROM ROLE_VIEW ORDER BY IDROLE", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RoleItem item = new RoleItem();

                        if (reader["IDROLE"] != DBNull.Value)
                        {
                            item.IdRole = Convert.ToInt32(reader["IDROLE"]);
                        }

                        if (reader["NAZEVROLE"] != DBNull.Value)
                        {
                            item.NazevRole = reader["NAZEVROLE"].ToString();
                        }
                        else
                        {
                            item.NazevRole = "";
                        }

                        RoleList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání rolí\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Naplní vlastnosti VM hodnotami z editovaného uživatele a předvybere roli.
        /// </summary>
        private void NaplnFormularZUzivatele()
        {
            if (_uzivatel.UzivatelskeJmeno != null)
            {
                UzivatelskeJmeno = _uzivatel.UzivatelskeJmeno;
            }
            else
            {
                UzivatelskeJmeno = "";
            }

            if (_uzivatel.Email != null)
            {
                Email = _uzivatel.Email;
            }
            else
            {
                Email = "";
            }

            if (_uzivatel.RodneCislo != null)
            {
                RodneCislo = _uzivatel.RodneCislo;
            }
            else
            {
                RodneCislo = "";
            }

            // heslo se neplní nikdy (volitelné zadání)
            NoveHeslo = "";

            // předvýběr role
            string roleUzivatele = "";
            if (_uzivatel.Role != null)
            {
                roleUzivatele = NormalizeRole(_uzivatel.Role);
            }

            foreach (var r in RoleList)
            {
                string roleItem = NormalizeRole(r.NazevRole);
                if (roleItem == roleUzivatele)
                {
                    SelectedRole = r;
                    break;
                }
            }
        }

        /// <summary>
        /// Aktualizuje povolení rodného čísla podle vybrané role.
        /// </summary>
        private void UpdateRodneCisloState()
        {
            if (SelectedRole == null)
            {
                RodneCisloEnabled = false;
                RodneCisloOpacity = 0.4;
                return;
            }

            string role = NormalizeRole(SelectedRole.NazevRole);

            bool musi = false;
            if (role == "hrac" || role == "trener")
            {
                musi = true;
            }

            if (musi)
            {
                RodneCisloEnabled = true;
                RodneCisloOpacity = 1.0;
            }
            else
            {
                RodneCisloEnabled = false;
                RodneCisloOpacity = 0.4;

                RodneCislo = "";
            }
        }

        /// <summary>
        /// Validuje vstupy a uloží změny do databáze.
        /// </summary>
        private void Save()
        {
            try
            {
                string jmeno = (UzivatelskeJmeno ?? "").Trim();
                string email = (Email ?? "").Trim();
                string rodne = (RodneCislo ?? "").Trim();
                string heslo = (NoveHeslo ?? "").Trim();

                if (SelectedRole == null)
                {
                    MessageBox.Show("Musíte vybrat roli uživatele",
                        "Varování", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string novaRole = NormalizeRole(SelectedRole.NazevRole);

                if (jmeno == "" || jmeno.Contains(" ") || jmeno.Length < 3)
                {
                    MessageBox.Show("Neplatné uživatelské jméno",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool emailOK = Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                if (!emailOK)
                {
                    MessageBox.Show("Zadejte platný e-mail",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool roleMusiZadatRodneCislo = false;
                if (novaRole == "hrac" || novaRole == "trener")
                {
                    roleMusiZadatRodneCislo = true;
                }

                string? rodneDoDb = null;

                if (roleMusiZadatRodneCislo)
                {
                    if (rodne == "")
                    {
                        MessageBox.Show("Pro roli hráč nebo trenér musíte zadat rodné číslo",
                            "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Validator.ValidujRodneCislo(rodne);
                    rodneDoDb = rodne;
                }
                else
                {
                    rodneDoDb = null;
                }

                _uzivatel.UzivatelskeJmeno = jmeno;
                _uzivatel.Email = email;
                _uzivatel.RodneCislo = rodneDoDb;
                _uzivatel.Role = novaRole;

                // volitelná změna hesla
                if (heslo != "")
                {
                    string salt = PasswordHasher.GenerateSalt();
                    string hash = PasswordHasher.HashPassword(heslo, salt);

                    _uzivatel.Heslo = hash;
                    _uzivatel.Salt = salt;
                }

                OracleConnection conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                DatabaseRegistrace.UpdateUzivatel(conn, _uzivatel, _stareJmeno);

                MessageBox.Show("Změny byly úspěšně uloženy",
                    "Uloženo", MessageBoxButton.OK, MessageBoxImage.Information);

                RequestClose?.Invoke(true);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20100)
                {
                    MessageBox.Show("Pro hráče/trenéra je rodné číslo povinné",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20101)
                {
                    MessageBox.Show("Člen s tímto rodným číslem neexistuje",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20102)
                {
                    MessageBox.Show("Typ člena nesedí (musí být hráč / trenér)",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ex.Number == 20103)
                {
                    MessageBox.Show("Hráč/trenér už má vytvořený účet",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Databázová chyba\n" + ex.Message,
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání dat\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Normalizuje název role (malá písmena + bez diakritiky),
        /// aby šla porovnávat role z DB / UI jednotně
        /// </summary>
        private string NormalizeRole(string role)
        {
            string r = role;
            if (r == null)
            {
                r = "";
            }

            r = r.ToLowerInvariant();

            r = r.Replace("á", "a");
            r = r.Replace("é", "e");
            r = r.Replace("í", "i");
            r = r.Replace("ó", "o");
            r = r.Replace("ú", "u");
            r = r.Replace("ů", "u");
            r = r.Replace("č", "c");
            r = r.Replace("ř", "r");
            r = r.Replace("š", "s");
            r = r.Replace("ť", "t");
            r = r.Replace("ň", "n");
            r = r.Replace("ý", "y");
            r = r.Replace("ž", "z");

            return r.Trim();
        }
    }
}
