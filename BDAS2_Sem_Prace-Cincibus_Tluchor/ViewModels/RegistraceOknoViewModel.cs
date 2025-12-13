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
    /// ViewModel pro okno Registrace
    /// Obsahuje hodnoty formuláře, validaci, hashování hesla a zápis do Oracle DB
    /// </summary>
    public class RegistraceOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// Událost pro zavření okna (View ji obslouží voláním Close())
        /// </summary>
        public event Action? RequestClose;

        /// <summary>
        /// Událost pro minimalizaci okna (View ji obslouží nastavením WindowState)
        /// </summary>
        public event Action? RequestMinimize;

        /// <summary>
        /// Seznam rolí pro ComboBox
        /// </summary>
        public ObservableCollection<string> RoleList { get; } = new ObservableCollection<string>
        {
            "Uživatel",
            "Hráč",
            "Trenér",
            "Admin"
        };

        private string _uzivatelskeJmeno = "";
        /// <summary>
        /// Uživatelské jméno registrovaného účtu
        /// </summary>
        public string UzivatelskeJmeno
        {
            get { return _uzivatelskeJmeno; }
            set { _uzivatelskeJmeno = value; OnPropertyChanged(); }
        }

        private string _email = "";
        /// <summary>
        /// E-mail registrovaného účtu
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _heslo = "";
        /// <summary>
        /// Heslo (před hashováním). Předává se z PasswordBox přes code-behind
        /// </summary>
        public string Heslo
        {
            get { return _heslo; }
            set { _heslo = value; OnPropertyChanged(); }
        }

        private string _heslo2 = "";
        /// <summary>
        /// Kontrolní heslo (opakované heslo). Předává se z PasswordBox přes code-behind.
        /// </summary>
        public string Heslo2
        {
            get { return _heslo2; }
            set { _heslo2 = value; OnPropertyChanged(); }
        }

        private string _selectedRole = "";
        /// <summary>
        /// Vybraná role z ComboBoxu (text).
        /// </summary>
        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                UpdateRodneCisloVisibility();
            }
        }

        private string _rodneCislo = "";
        /// <summary>
        /// Rodné číslo – vyžadované pouze pro role Hráč/Trenér.
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        private Visibility _rodneCisloVisibility = Visibility.Collapsed;
        /// <summary>
        /// Viditelnost panelu Rodné číslo dle role
        /// </summary>
        public Visibility RodneCisloVisibility
        {
            get { return _rodneCisloVisibility; }
            private set { _rodneCisloVisibility = value; OnPropertyChanged(); }
        }

        private string _statusText = "";
        /// <summary>
        /// Stavová hláška zobrazená ve View (label)
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }
            private set { _statusText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Příkaz pro registraci (validace + DB insert)
        /// </summary>
        public RelayCommand RegisterCommand { get; }

        /// <summary>
        /// Příkaz pro zavření okna
        /// </summary>
        public RelayCommand CloseCommand { get; }

        /// <summary>
        /// Příkaz pro minimalizaci okna
        /// </summary>
        public RelayCommand MinimizeCommand { get; }

        /// <summary>
        /// Inicializace ViewModelu a příkazů
        /// </summary>
        public RegistraceOknoViewModel()
        {
            RegisterCommand = new RelayCommand(_ => Register());
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
            MinimizeCommand = new RelayCommand(_ => RequestMinimize?.Invoke());

            // výchozí stav
            SelectedRole = "";
            UpdateRodneCisloVisibility();
        }

        private void UpdateRodneCisloVisibility()
        {
            string role = NormalizeRole(SelectedRole);

            bool vyzadujeRodne = false;
            if (role == "hrac" || role == "trener")
            {
                vyzadujeRodne = true;
            }

            if (vyzadujeRodne)
            {
                RodneCisloVisibility = Visibility.Visible;
            }
            else
            {
                RodneCisloVisibility = Visibility.Collapsed;
            }
        }

        private void Register()
        {
            StatusText = "";

            string uziv = (UzivatelskeJmeno ?? "").Trim();
            string email = (Email ?? "").Trim();
            string heslo = (Heslo ?? "").Trim();
            string heslo2 = (Heslo2 ?? "").Trim();
            string roleRaw = SelectedRole;

            if (string.IsNullOrWhiteSpace(roleRaw))
            {
                MessageBox.Show("Vyberte roli!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(uziv) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(heslo))
            {
                MessageBox.Show("Vyplňte všechna povinná pole!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (uziv.Length < 3 || uziv.Length > 30)
            {
                MessageBox.Show("Uživatelské jméno musí mít 3–30 znaků.", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo.Length < 8 || heslo.Length > 15)
            {
                MessageBox.Show("Heslo musí mít 8–15 znaků.", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (heslo != heslo2)
            {
                MessageBox.Show("Hesla se neshodují!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (email.Length > 100)
            {
                MessageBox.Show("E-mail je příliš dlouhý (max 100 znaků)", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("E-mail není ve správném formátu!", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string normalizovanaRole = NormalizeRole(roleRaw);

            bool roleVyzadujeRodneCislo = false;
            if (normalizovanaRole == "hrac" || normalizovanaRole == "trener")
            {
                roleVyzadujeRodneCislo = true;
            }

            string? rcDoDb = null;

            if (roleVyzadujeRodneCislo)
            {
                string rc = (RodneCislo ?? "").Trim();

                if (string.IsNullOrWhiteSpace(rc))
                {
                    MessageBox.Show("Pro roli Hráč/Trenér musíte zadat rodné číslo",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    Validator.ValidujRodneCislo(rc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                rcDoDb = rc;
            }

            try
            {
                string salt = PasswordHasher.GenerateSalt();
                string hash = PasswordHasher.HashPassword(heslo, salt);

                Uzivatel novyUzivatel = new Uzivatel();
                novyUzivatel.UzivatelskeJmeno = uziv;
                novyUzivatel.Email = email;
                novyUzivatel.Heslo = hash;
                novyUzivatel.Salt = salt;
                novyUzivatel.Role = normalizovanaRole;
                novyUzivatel.PosledniPrihlaseni = DateTime.Now;
                novyUzivatel.RodneCislo = rcDoDb;

                OracleConnection conn = DatabaseManager.GetConnection();

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseRegistrace.AddUzivatel(conn, novyUzivatel);

                MessageBox.Show(
                    "Uživatel '" + uziv + "' byl úspěšně registrován.",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                RequestClose?.Invoke();
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20003)
                {
                    MessageBox.Show("Uživatelské jméno nebo email již existuje!", "Chyba",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ex.Number == 20100)
                {
                    MessageBox.Show("Rodné číslo je povinné pro hráče/trenéra",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ex.Number == 20101)
                {
                    MessageBox.Show("Člen s daným rodným číslem neexistuje v databázi",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ex.Number == 20103)
                {
                    MessageBox.Show("Hráč/trenér již má vytvořený účet!",
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Chyba při registraci: " + ex.Message, "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message, "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Normalizuje text role na interní hodnotu bez diakritiky (hrac/trener/uzivatel/admin)
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
