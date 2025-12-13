using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialogové okno editace hráče
    /// Řeší naplnění formuláře, validaci vstupů, uložení změn do databáze
    /// </summary>
    public class DialogEditujHraceViewModel : ViewModelBase
    {
        /// <summary>
        /// editovaný hráč – stejný objekt, který je zobrazený v DataGridu
        /// </summary>
        private readonly Hrac _editovanyHrac;

        /// <summary>
        /// Původní rodné číslo (před editací) – používá se pro update v DB
        /// pokud uživatel rodné číslo změní
        /// </summary>
        private readonly string _puvodniRodneCislo;

        /// <summary>
        /// Volitelný callback, který si může View předat pro manuální refresh gridu (např. Items.Refresh())
        /// </summary>
        private readonly Action? _requestRefreshGrid;

        /// <summary>
        /// Seznam pozic pro ComboBox
        /// </summary>
        public List<Pozice> PoziceList { get; } = new()
        {
            new Pozice { Id = 1, Nazev = "Brankář" },
            new Pozice { Id = 2, Nazev = "Obránce" },
            new Pozice { Id = 3, Nazev = "Záložník" },
            new Pozice { Id = 4, Nazev = "Útočník" }
        };

        private Pozice _selectedPozice;

        /// <summary>
        /// Aktuálně vybraná pozice hráče v ComboBoxu
        /// </summary>
        public Pozice SelectedPozice
        {
            get => _selectedPozice;
            set { _selectedPozice = value; OnPropertyChanged(); }
        }

        private string _rodneCislo = "";

        /// <summary>
        /// Rodné číslo (TextBox)
        /// </summary>
        public string RodneCislo
        {
            get => _rodneCislo;
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        private string _jmeno = "";

        /// <summary>
        /// Jméno (TextBox)
        /// </summary>
        public string Jmeno
        {
            get => _jmeno;
            set { _jmeno = value; OnPropertyChanged(); }
        }

        private string _prijmeni = "";

        /// <summary>
        /// Příjmení (TextBox)
        /// </summary>
        public string Prijmeni
        {
            get => _prijmeni;
            set { _prijmeni = value; OnPropertyChanged(); }
        }

        private string _telefonniCislo = "";

        /// <summary>
        /// Telefonní číslo (TextBox)
        /// </summary>
        public string TelefonniCislo
        {
            get => _telefonniCislo;
            set { _telefonniCislo = value; OnPropertyChanged(); }
        }

        private int _pocetGolu;

        /// <summary>
        /// Počet vstřelených gólů (IntegerUpDown / TextBox dle UI)
        /// </summary>
        public int PocetGolu
        {
            get => _pocetGolu;
            set { _pocetGolu = value; OnPropertyChanged(); }
        }

        private int _pocetZlutychKaret;

        /// <summary>
        /// Počet žlutých karet
        /// </summary>
        public int PocetZlutychKaret
        {
            get => _pocetZlutychKaret;
            set { _pocetZlutychKaret = value; OnPropertyChanged(); }
        }

        private int _pocetCervenychKaret;

        /// <summary>
        /// Počet červených karet
        /// </summary>
        public int PocetCervenychKaret
        {
            get => _pocetCervenychKaret;
            set { _pocetCervenychKaret = value; OnPropertyChanged(); }
        }

        private bool _maOpatreni;

        /// <summary>
        /// zda má hráč disciplinární opatření
        /// </summary>
        public bool MaOpatreni
        {
            get => _maOpatreni;
            set
            {
                _maOpatreni = value;
                OnPropertyChanged();

                if (!_maOpatreni)
                {
                    DatumOpatreni = null;
                    DelkaTrestu = 1;
                    DuvodOpatreni = "";
                }
            }
        }

        private DateTime? _datumOpatreni;

        /// <summary>
        /// Datum opatření (DatePicker)
        /// </summary>
        public DateTime? DatumOpatreni
        {
            get => _datumOpatreni;
            set { _datumOpatreni = value; OnPropertyChanged(); }
        }

        private int _delkaTrestu = 1;

        /// <summary>
        /// Délka trestu (dny)
        /// </summary>
        public int DelkaTrestu
        {
            get => _delkaTrestu;
            set { _delkaTrestu = value; OnPropertyChanged(); }
        }

        private string _duvodOpatreni = "";

        /// <summary>
        /// Důvod disciplinárního opatření (volitelný text)
        /// </summary>
        public string DuvodOpatreni
        {
            get => _duvodOpatreni;
            set { _duvodOpatreni = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Command pro uložení změn (tlačítko Edituj)
        /// </summary>
        public RelayCommand EditujCommand { get; }

        /// <summary>
        /// Command pro zavření dialogu bez uložení (tlačítko Zruš)
        /// </summary>
        public RelayCommand ZrusCommand { get; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu
        /// </summary>
        public event Action<bool>? RequestClose;

        /// <summary>
        /// Vytvoří ViewModel pro editaci konkrétního hráče a naplní formulář
        /// </summary>
        /// <param name="editovanyHrac">Hráč, který se bude editovat </param>
        /// <param name="requestRefreshGrid">Volitelný callback pro refresh DataGridu po uložení</param>
        public DialogEditujHraceViewModel(Hrac editovanyHrac, Action? requestRefreshGrid = null)
        {
            _editovanyHrac = editovanyHrac;
            _puvodniRodneCislo = editovanyHrac.RodneCislo;
            _requestRefreshGrid = requestRefreshGrid;

            // naplnění polí z hráče 
            if (editovanyHrac.RodneCislo != null)
                RodneCislo = editovanyHrac.RodneCislo;
            else
                RodneCislo = "";

            if (editovanyHrac.Jmeno != null)
                Jmeno = editovanyHrac.Jmeno;
            else
                Jmeno = "";

            if (editovanyHrac.Prijmeni != null)
                Prijmeni = editovanyHrac.Prijmeni;
            else
                Prijmeni = "";

            if (editovanyHrac.TelefonniCislo != null)
                TelefonniCislo = editovanyHrac.TelefonniCislo;
            else
                TelefonniCislo = "";

            PocetGolu = editovanyHrac.PocetVstrelenychGolu;
            PocetZlutychKaret = editovanyHrac.PocetZlutychKaret;
            PocetCervenychKaret = editovanyHrac.PocetCervenychKaret;

            // pozice podle názvu 
            Pozice found = null;
            for (int i = 0; i < PoziceList.Count; i++)
            {
                if (PoziceList[i].Nazev == editovanyHrac.PoziceNaHristi)
                {
                    found = PoziceList[i];
                    break;
                }
            }
            if (found != null)
                SelectedPozice = found;
            else
                SelectedPozice = PoziceList[0];

            // opatření
            bool maOpatreni = editovanyHrac.DelkaTrestu > 0;
            if (!maOpatreni)
            {
                if (!string.IsNullOrEmpty(editovanyHrac.DuvodOpatreni))
                    maOpatreni = true;
            }

            MaOpatreni = maOpatreni;

            if (maOpatreni)
            {
                if (editovanyHrac.DatumOpatreni == DateTime.MinValue)
                    DatumOpatreni = DateTime.Today;
                else
                    DatumOpatreni = editovanyHrac.DatumOpatreni;

                if (editovanyHrac.DelkaTrestu > 0)
                    DelkaTrestu = editovanyHrac.DelkaTrestu;
                else
                    DelkaTrestu = 1;

                if (editovanyHrac.DuvodOpatreni != null)
                    DuvodOpatreni = editovanyHrac.DuvodOpatreni;
                else
                    DuvodOpatreni = "";
            }
            else
            {
                DatumOpatreni = null;
                DelkaTrestu = 1;
                DuvodOpatreni = "";
            }

            EditujCommand = new RelayCommand(_ => Edituj());
            ZrusCommand = new RelayCommand(_ => RequestClose?.Invoke(false));
        }

        /// <summary>
        /// Provede validaci, naplní zpět vlastnosti do editovaného hráče,
        /// uloží změny do DB a zavře dialog s výsledkem OK
        /// </summary>
        private void Edituj()
        {
            try
            {
                string rodneCislo;
                if (RodneCislo != null)
                    rodneCislo = RodneCislo.Trim();
                else
                    rodneCislo = "";

                string jmeno;
                if (Jmeno != null)
                    jmeno = Jmeno.Trim();
                else
                    jmeno = "";

                string prijmeni;
                if (Prijmeni != null)
                    prijmeni = Prijmeni.Trim();
                else
                    prijmeni = "";

                string telCislo;
                if (TelefonniCislo != null)
                    telCislo = TelefonniCislo.Trim();
                else
                    telCislo = "";

                // validace 
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);

                Validator.ValidujCeleCislo(PocetGolu.ToString(), "Počet gólů");
                Validator.ValidujCeleCislo(PocetZlutychKaret.ToString(), "Počet žlutých karet");
                Validator.ValidujCeleCislo(PocetCervenychKaret.ToString(), "Počet červených karet");

                if (MaOpatreni)
                {
                    Validator.ValidujDatum(DatumOpatreni, "Datum opatření");
                    Validator.ValidujCeleCislo(DelkaTrestu.ToString(), "Délka trestu");
                }

                _editovanyHrac.RodneCislo = rodneCislo;
                _editovanyHrac.Jmeno = jmeno;
                _editovanyHrac.Prijmeni = prijmeni;
                _editovanyHrac.TelefonniCislo = telCislo;

                _editovanyHrac.PocetVstrelenychGolu = PocetGolu;
                _editovanyHrac.PocetZlutychKaret = PocetZlutychKaret;
                _editovanyHrac.PocetCervenychKaret = PocetCervenychKaret;

                if (SelectedPozice != null)
                {
                    _editovanyHrac.IdPozice = SelectedPozice.Id;
                    _editovanyHrac.PoziceNaHristi = SelectedPozice.Nazev;
                }

                if (MaOpatreni)
                {
                    // DatumOpatreni je nullable 
                    if (DatumOpatreni == null)
                        throw new Exception("Datum opatření nesmí být prázdné!");

                    _editovanyHrac.DatumOpatreni = DatumOpatreni.Value;
                    _editovanyHrac.DelkaTrestu = DelkaTrestu;

                    string duvod;
                    if (DuvodOpatreni != null)
                        duvod = DuvodOpatreni.Trim();
                    else
                        duvod = "";

                    _editovanyHrac.DuvodOpatreni = duvod;
                    _editovanyHrac.DatumOpatreniText = _editovanyHrac.DatumOpatreni.ToString("dd.MM.yyyy");
                }
                else
                {
                    _editovanyHrac.DatumOpatreni = DateTime.MinValue;
                    _editovanyHrac.DelkaTrestu = 0;
                    _editovanyHrac.DuvodOpatreni = null;
                    _editovanyHrac.DatumOpatreniText = "Bez opatření";
                }

                // DB update
                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseHraci.UpdateHrac(conn, _editovanyHrac, _puvodniRodneCislo);

                if (_requestRefreshGrid != null)
                    _requestRefreshGrid.Invoke();

                MessageBox.Show("Změny byly úspěšně uloženy",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání hráče:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
