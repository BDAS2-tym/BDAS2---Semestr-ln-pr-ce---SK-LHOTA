using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialog editace trenéra
    /// Zajišťuje validaci a update v databázi
    /// </summary>
    public class DialogEditujTreneraViewModel : ViewModelBase
    {
        /// <summary>
        /// Editovaný trenéra z DataGridu
        /// </summary>
        private readonly Trener _editovanyTrener;

        /// <summary>
        /// Původní rodné číslo kvůli update v databázi
        /// </summary>
        private readonly string _puvodniRodneCislo;

        /// <summary>
        /// Volitelný callback na refresh tabulky v okně Trenéři
        /// </summary>
        private readonly Action _requestRefreshGrid;

        private string _rodneCislo;
        /// <summary>
        /// Rodné číslo upravované v dialogu
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set
            {
                _rodneCislo = value;
                OnPropertyChanged();
            }
        }

        private string _jmeno;
        /// <summary>
        /// Jméno trenéra
        /// </summary>
        public string Jmeno
        {
            get { return _jmeno; }
            set
            {
                _jmeno = value;
                OnPropertyChanged();
            }
        }

        private string _prijmeni;
        /// <summary>
        /// Příjmení trenéra
        /// </summary>
        public string Prijmeni
        {
            get { return _prijmeni; }
            set
            {
                _prijmeni = value;
                OnPropertyChanged();
            }
        }

        private string _telefonniCislo;
        /// <summary>
        /// Telefonní číslo trenéra
        /// </summary>
        public string TelefonniCislo
        {
            get { return _telefonniCislo; }
            set
            {
                _telefonniCislo = value;
                OnPropertyChanged();
            }
        }

        private string _licence;
        /// <summary>
        /// Trenérská licence
        /// </summary>
        public string Licence
        {
            get { return _licence; }
            set
            {
                _licence = value;
                OnPropertyChanged();
            }
        }

        private string _specializace;
        /// <summary>
        /// Specializace trenéra, může být prázdná
        /// </summary>
        public string Specializace
        {
            get { return _specializace; }
            set
            {
                _specializace = value;
                OnPropertyChanged();
            }
        }

        private int _praxe;
        /// <summary>
        /// Počet let praxe
        /// </summary>
        public int Praxe
        {
            get { return _praxe; }
            set
            {
                _praxe = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Příkaz pro uložení změn
        /// </summary>
        public RelayCommand EditujCommand { get; }

        /// <summary>
        /// Příkaz pro zavření dialogu bez uložení
        /// </summary>
        public RelayCommand UkonciCommand { get; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Vytvoří ViewModel a načte data z předané instance trenéra
        /// </summary>
        /// <param name="trener">Editovaný trenér z tabulky</param>
        /// <param name="requestRefreshGrid">Akce pro refresh DataGridu</param>
        public DialogEditujTreneraViewModel(Trener trener, Action requestRefreshGrid)
        {
            _editovanyTrener = trener;
            _requestRefreshGrid = requestRefreshGrid;

            _puvodniRodneCislo = trener.RodneCislo;

            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            TelefonniCislo = "";
            Licence = "";
            Specializace = "";
            Praxe = 1;

            if (trener.RodneCislo != null)
            {
                RodneCislo = trener.RodneCislo;
            }
            else
            {
                RodneCislo = "";
            }

            if (trener.Jmeno != null)
            {
                Jmeno = trener.Jmeno;
            }
            else
            {
                Jmeno = "";
            }

            if (trener.Prijmeni != null)
            {
                Prijmeni = trener.Prijmeni;
            }
            else
            {
                Prijmeni = "";
            }

            if (trener.TelefonniCislo != null)
            {
                TelefonniCislo = trener.TelefonniCislo;
            }
            else
            {
                TelefonniCislo = "";
            }

            if (trener.TrenerskaLicence != null)
            {
                Licence = trener.TrenerskaLicence;
            }
            else
            {
                Licence = "";
            }

            if (trener.Specializace != null)
            {
                Specializace = trener.Specializace;
            }
            else
            {
                Specializace = "";
            }

            Praxe = trener.PocetLetPraxe;

            EditujCommand = new RelayCommand(_ => Edituj());
            UkonciCommand = new RelayCommand(_ => Ukonci());
        }

        /// <summary>
        /// Zavře dialog bez ukládání
        /// </summary>
        private void Ukonci()
        {
            if (RequestClose != null)
            {
                RequestClose.Invoke(false);
            }
            else
            {
            }
        }

        /// <summary>
        /// Provede validaci a uloží změny do databáze
        /// </summary>
        private void Edituj()
        {
            try
            {
                string rodneCislo;
                if (RodneCislo != null)
                {
                    rodneCislo = RodneCislo.Trim();
                }
                else
                {
                    rodneCislo = "";
                }

                string jmeno;
                if (Jmeno != null)
                {
                    jmeno = Jmeno.Trim();
                }
                else
                {
                    jmeno = "";
                }

                string prijmeni;
                if (Prijmeni != null)
                {
                    prijmeni = Prijmeni.Trim();
                }
                else
                {
                    prijmeni = "";
                }

                string telCislo;
                if (TelefonniCislo != null)
                {
                    telCislo = TelefonniCislo.Trim();
                }
                else
                {
                    telCislo = "";
                }

                string licence;
                if (Licence != null)
                {
                    licence = Licence.Trim();
                }
                else
                {
                    licence = "";
                }

                string specializace;
                if (Specializace != null)
                {
                    specializace = Specializace.Trim();
                }
                else
                {
                    specializace = "";
                }

                string praxeText = Praxe.ToString();

                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);
                Validator.ValidujTrenerskouLicenci(licence);
                Validator.ValidujPocetLetPraxeTrenera(praxeText);
                Validator.ValidujSpecializaciTrenera(specializace);

                _editovanyTrener.RodneCislo = rodneCislo;
                _editovanyTrener.Jmeno = jmeno;
                _editovanyTrener.Prijmeni = prijmeni;
                _editovanyTrener.TelefonniCislo = telCislo;
                _editovanyTrener.TrenerskaLicence = licence;

                if (string.IsNullOrWhiteSpace(specializace))
                {
                    _editovanyTrener.Specializace = null;
                }
                else
                {
                    _editovanyTrener.Specializace = specializace;
                }

                _editovanyTrener.PocetLetPraxe = Praxe;

                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseTreneri.UpdateTrener(conn, _editovanyTrener, _puvodniRodneCislo);

                if (_requestRefreshGrid != null)
                {
                    _requestRefreshGrid.Invoke();
                }
                else
                {
                }

                MessageBox.Show("Úprava byla úspěšně provedena",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při úpravě trenéra\n" + ex.Message,
                    "Chyba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
