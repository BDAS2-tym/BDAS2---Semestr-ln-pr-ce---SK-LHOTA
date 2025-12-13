using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs
{
    /// <summary>
    /// ViewModel pro dialog "Najdi uživatelské účty"
    /// Drží hodnoty z formuláře, provádí filtraci nad předanou kolekcí a vrací výsledek
    /// </summary>
    public class DialogNajdiUzivatelskeUctyViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Uzivatel> _uzivateleData;

        /// <summary>
        /// Seznam rolí pro ComboBox.
        /// </summary>
        public ObservableCollection<string> RoleList { get; } = new ObservableCollection<string>
        {
            "Hrac",
            "Trener",
            "Admin",
            "Uživatel",
            "Host"
        };

        /// <summary>
        /// Výsledek filtrování vrácený volajícímu oknu.
        /// </summary>
        public IEnumerable<Uzivatel> VyfiltrovaniUzivatele { get; private set; } = Enumerable.Empty<Uzivatel>();

        /// <summary>
        /// Požadavek na zavření dialogu
        /// </summary>
        public event Action<bool>? RequestClose;

        private string _jmeno = "";
        /// <summary>
        /// Filtr: uživatelské jméno (contains, ignore-case).
        /// </summary>
        public string Jmeno
        {
            get { return _jmeno; }
            set { _jmeno = value; OnPropertyChanged(); }
        }

        private string _email = "";
        /// <summary>
        /// Filtr: e-mail (contains, ignore-case).
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string? _selectedRole;
        /// <summary>
        /// Filtr: role (equals, ignore-case).
        /// </summary>
        public string? SelectedRole
        {
            get { return _selectedRole; }
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        private string _rodneCislo = "";
        /// <summary>
        /// Filtr: rodné číslo (přesná shoda, 10 číslic).
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        private DateTime? _datumPrihlaseni;
        /// <summary>
        /// Filtr: datum posledního přihlášení (porovnává se pouze Date)
        /// </summary>
        public DateTime? DatumPrihlaseni
        {
            get { return _datumPrihlaseni; }
            set { _datumPrihlaseni = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Spustí filtrování
        /// </summary>
        public RelayCommand NajdiCommand { get; }

        /// <summary>
        /// Resetuje formulář
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// Vytvoří ViewModel a uloží zdrojovou kolekci pro filtrování.
        /// </summary>
        public DialogNajdiUzivatelskeUctyViewModel(ObservableCollection<Uzivatel> uzivatele)
        {
            if (uzivatele == null)
            {
                _uzivateleData = new ObservableCollection<Uzivatel>();
            }
            else
            {
                _uzivateleData = uzivatele;
            }

            NajdiCommand = new RelayCommand(_ => Najdi());
            ResetCommand = new RelayCommand(_ => Reset());

            Reset();
        }

        /// <summary>
        /// Vynuluje všechny filtry
        /// </summary>
        private void Reset()
        {
            Jmeno = "";
            Email = "";
            SelectedRole = null;
            RodneCislo = "";
            DatumPrihlaseni = null;
        }

        /// <summary>
        /// Provede filtrování, uloží výsledek a požádá o zavření dialogu
        /// </summary>
        private void Najdi()
        {
            try
            {
                VyfiltrovaniUzivatele = FiltrujUzivatele();
                RequestClose?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba filtrování", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Aplikuje jednotlivé filtry 
        /// </summary>
        private IEnumerable<Uzivatel> FiltrujUzivatele()
        {
            IEnumerable<Uzivatel> vysledek = _uzivateleData;

            string jmeno = (Jmeno ?? "").Trim();
            string email = (Email ?? "").Trim();
            string rodne = (RodneCislo ?? "").Trim();

            string role = "";
            if (SelectedRole != null)
            {
                role = SelectedRole.Trim();
            }

            DateTime? datum = DatumPrihlaseni;

            if (jmeno.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.UzivatelskeJmeno != null &&
                    u.UzivatelskeJmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase));
            }

            if (email.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.Email != null &&
                    u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
            }

            if (role.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.Role != null &&
                    u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
            }

            if (rodne.Length > 0)
            {
                if (rodne.Length != 10 || rodne.Any(ch => !char.IsDigit(ch)))
                {
                    throw new Exception("Rodné číslo musí mít přesně 10 číslic.");
                }

                vysledek = vysledek.Where(u =>
                    u.RodneCislo != null &&
                    u.RodneCislo == rodne);
            }

            if (datum.HasValue)
            {
                DateTime hledane = datum.Value.Date;

                vysledek = vysledek.Where(u =>
                    u.PosledniPrihlaseni.Date == hledane);
            }

            return vysledek;
        }
    }
}
