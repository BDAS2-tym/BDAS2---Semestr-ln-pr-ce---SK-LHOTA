using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialog přidání trenéra
    /// Řeší validaci, kontrolu duplicity a vložení trenéra do databáze
    /// </summary>
    public class DialogPridejTreneraViewModel : ViewModelBase
    {
        /// <summary>
        /// Kolekce trenérů navázaná na DataGrid v okně Trenéři
        /// Po úspěšném vložení do DB se nový trenér přidá i do kolekce teto
        /// </summary>
        private readonly ObservableCollection<Trener> _treneriData;

        private string _rodneCislo;
        /// <summary>
        /// Rodné číslo trenéra
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
        /// Specializace trenéra
        /// Pole je volitelné, validace se dělá jen pokud je vyplněné
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
        /// Příkaz pro potvrzení přidání trenéra
        /// </summary>
        public RelayCommand PridejCommand { get; }

        /// <summary>
        /// Příkaz pro reset formuláře
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Inicializace ViewModelu
        /// </summary>
        /// <param name="treneriData">Kolekce trenérů navázaná na DataGrid</param>
        public DialogPridejTreneraViewModel(ObservableCollection<Trener> treneriData)
        {
            _treneriData = treneriData;

            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            TelefonniCislo = "";
            Licence = "";
            Specializace = "";
            Praxe = 1;

            PridejCommand = new RelayCommand(_ => Pridej());
            ResetCommand = new RelayCommand(_ => Reset());
        }

        /// <summary>
        /// Vymaže formulář a nastaví výchozí hodnoty
        /// </summary>
        private void Reset()
        {
            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            TelefonniCislo = "";
            Licence = "";
            Specializace = "";
            Praxe = 1;
        }

        /// <summary>
        /// Provede validaci a přidání trenéra do databáze i do kolekce pro DataGrid
        /// </summary>
        private void Pridej()
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

                var conn = DatabaseManager.GetConnection();

                if (Validator.ExistujeRodneCislo(conn, rodneCislo))
                {
                    MessageBox.Show("Trenér s tímto rodným číslem již existuje!",
                        "Duplicitní rodné číslo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                else
                {
                }

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                Trener novyTrener = new Trener(
                    jmeno,
                    prijmeni,
                    rodneCislo,
                    telCislo,
                    licence,
                    specializace,
                    Praxe
                );

                DatabaseTreneri.AddTrener(conn, novyTrener);
                _treneriData.Add(novyTrener);

                MessageBox.Show("Trenér byl úspěšně přidán",
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
                MessageBox.Show("Došlo k chybě při přidávání trenéra:\n" + ex.Message,
                    "Chyba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
