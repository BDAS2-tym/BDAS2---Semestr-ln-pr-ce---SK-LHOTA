using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs
{
    /// <summary>
    /// ViewModel dialogu pro vyhledávání (filtrování) trenérů podle zadaných kritérií
    /// </summary>
    public class DialogNajdiTreneraViewModel : ViewModelBase
    {
        /// <summary>
        /// kolekce trenérů, nad kterou se provádí filtrování
        /// </summary>
        private readonly ObservableCollection<Trener> _treneriData;

        /// <summary>
        /// Výsledek filtrování trenérů 
        /// </summary>
        public IEnumerable<Trener> VyfiltrovaniTreneri { get; private set; }

        /// <summary>
        /// Příkaz pro spuštění filtrování
        /// </summary>
        public RelayCommand NajdiCommand { get; private set; }

        /// <summary>
        /// Příkaz pro reset všech filtrů do výchozího stavu
        /// </summary>
        public RelayCommand ResetCommand { get; private set; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu (true = potvrdit/OK, false = zrušit)
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Určuje, zda uživatel smí filtrovat citlivé údaje (rodné číslo, telefon)
        /// </summary>
        private bool _canFilterSensitive;

        /// <summary>
        /// True pokud je uživatel admin a může filtrovat citlivé údaje
        /// </summary>
        public bool CanFilterSensitive
        {
            get { return _canFilterSensitive; }
            private set { _canFilterSensitive = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Rodné číslo z formuláře (filtr)
        /// </summary>
        private string _rodneCislo;

        /// <summary>
        /// Rodné číslo pro filtrování (povoleno jen admin).
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Jméno z formuláře (filtr).
        /// </summary>
        private string _jmeno;

        /// <summary>
        /// Jméno trenéra pro filtrování.
        /// </summary>
        public string Jmeno
        {
            get { return _jmeno; }
            set { _jmeno = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Příjmení z formuláře (filtr).
        /// </summary>
        private string _prijmeni;

        /// <summary>
        /// Příjmení trenéra pro filtrování.
        /// </summary>
        public string Prijmeni
        {
            get { return _prijmeni; }
            set { _prijmeni = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Telefon z formuláře (filtr).
        /// </summary>
        private string _telefon;

        /// <summary>
        /// Telefonní číslo pro filtrování (povoleno jen admin).
        /// </summary>
        public string Telefon
        {
            get { return _telefon; }
            set { _telefon = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Licence z formuláře (filtr).
        /// </summary>
        private string _licence;

        /// <summary>
        /// licence pro filtrování.
        /// </summary>
        public string Licence
        {
            get { return _licence; }
            set { _licence = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Specializace z formuláře (filtr).
        /// </summary>
        private string _specializace;

        /// <summary>
        /// Specializace trenéra pro filtrování.
        /// </summary>
        public string Specializace
        {
            get { return _specializace; }
            set { _specializace = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Praxe z formuláře (filtr, textová hodnota).
        /// </summary>
        private string _praxe;

        /// <summary>
        /// Počet let praxe pro filtrování (validuje se a převádí na int).
        /// </summary>
        public string Praxe
        {
            get { return _praxe; }
            set { _praxe = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Tooltip pro pole rodného čísla při omezení práv (ne-admin).
        /// </summary>
        public string RodneCisloToolTip
        {
            get
            {
                if (CanFilterSensitive)
                {
                    return "";
                }

                return "Pouze admin může filtrovat podle rodného čísla";
            }
        }

        /// <summary>
        /// Tooltip pro pole telefonu při omezení práv (ne-admin).
        /// </summary>
        public string TelefonToolTip
        {
            get
            {
                if (CanFilterSensitive)
                {
                    return "";
                }

                return "Pouze admin může filtrovat podle telefonního čísla";
            }
        }

        public DialogNajdiTreneraViewModel(ObservableCollection<Trener> treneriData)
        {
            if (treneriData == null)
            {
                _treneriData = new ObservableCollection<Trener>();
            }
            else
            {
                _treneriData = treneriData;
            }

            NastavPravaPodleRole();

            ResetCommand = new RelayCommand(_ => Reset());
            NajdiCommand = new RelayCommand(_ => Najdi());

            Reset();
        }

        public DialogNajdiTreneraViewModel()
            : this(new ObservableCollection<Trener>())
        {
        }

        /// <summary>
        /// Nastaví práva filtrování citlivých údajů podle role přihlášeného uživatele.
        /// </summary>
        private void NastavPravaPodleRole()
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

            if (role == "admin")
            {
                CanFilterSensitive = true;
            }
            else
            {
                CanFilterSensitive = false;
            }

            OnPropertyChanged(nameof(RodneCisloToolTip));
            OnPropertyChanged(nameof(TelefonToolTip));
        }

        /// <summary>
        /// Vymaže všechny filtry do výchozího stavu.
        /// </summary>
        private void Reset()
        {
            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            Telefon = "";
            Licence = "";
            Specializace = "";
            Praxe = "";
        }

        /// <summary>
        /// Spustí filtrování, uloží výsledek a požádá o zavření dialogu s výsledkem OK
        /// </summary>
        private void Najdi()
        {
            try
            {
                VyfiltrovaniTreneri = FiltrujTrenery();

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }
            }
            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při filtrování:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Provede filtrování trenérů podle vyplněných kritérií ve ViewModelu
        /// </summary>
        /// <returns>Kolekce trenérů odpovídající filtrům.</returns>
        private IEnumerable<Trener> FiltrujTrenery()
        {
            IEnumerable<Trener> vysledek = _treneriData;

            string rodneCislo = RodneCislo;
            if (rodneCislo == null) rodneCislo = "";
            rodneCislo = rodneCislo.Trim();

            string jmeno = Jmeno;
            if (jmeno == null) jmeno = "";
            jmeno = jmeno.Trim();

            string prijmeni = Prijmeni;
            if (prijmeni == null) prijmeni = "";
            prijmeni = prijmeni.Trim();

            string telefon = Telefon;
            if (telefon == null) telefon = "";
            telefon = telefon.Trim();

            string licence = Licence;
            if (licence == null) licence = "";
            licence = licence.Trim();

            string specializace = Specializace;
            if (specializace == null) specializace = "";
            specializace = specializace.Trim();

            string praxeText = Praxe;
            if (praxeText == null) praxeText = "";
            praxeText = praxeText.Trim();

            // Rodné číslo (jen admin)
            if (CanFilterSensitive)
            {
                if (!string.IsNullOrWhiteSpace(rodneCislo))
                {
                    Validator.ValidujRodneCislo(rodneCislo);

                    vysledek = vysledek.Where(t =>
                        t.RodneCislo != null &&
                        t.RodneCislo == rodneCislo);
                }
            }

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                Validator.ValidujJmeno(jmeno);

                vysledek = vysledek.Where(t =>
                    t.Jmeno != null &&
                    t.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase));
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                Validator.ValidujPrijmeni(prijmeni);

                vysledek = vysledek.Where(t =>
                    t.Prijmeni != null &&
                    t.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase));
            }

            // Telefon (jen admin)
            if (CanFilterSensitive)
            {
                if (!string.IsNullOrWhiteSpace(telefon))
                {
                    Validator.ValidujTelefon(telefon);

                    vysledek = vysledek.Where(t =>
                        t.TelefonniCislo != null &&
                        t.TelefonniCislo.Contains(telefon));
                }
            }

            // Licence
            if (!string.IsNullOrWhiteSpace(licence))
            {
                Validator.ValidujTrenerskouLicenci(licence);

                vysledek = vysledek.Where(t =>
                    t.TrenerskaLicence != null &&
                    t.TrenerskaLicence.Contains(licence, StringComparison.OrdinalIgnoreCase));
            }

            // Specializace
            if (!string.IsNullOrWhiteSpace(specializace))
            {
                Validator.ValidujSpecializaciTrenera(specializace);

                vysledek = vysledek.Where(t =>
                    t.Specializace != null &&
                    t.Specializace.Contains(specializace, StringComparison.OrdinalIgnoreCase));
            }

            // Praxe
            if (!string.IsNullOrWhiteSpace(praxeText))
            {
                Validator.ValidujPocetLetPraxeTrenera(praxeText);

                int praxe = int.Parse(praxeText);
                vysledek = vysledek.Where(t => t.PocetLetPraxe == praxe);
            }

            return vysledek;
        }
    }
}
