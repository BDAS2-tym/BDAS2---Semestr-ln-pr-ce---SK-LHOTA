using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialogové okno přidání hráče
    /// </summary>
    public class DialogPridejHraceViewModel : ViewModelBase
    {
        /// <summary>
        /// Reference na kolekci hráčů z hlavního okna (DataGrid)
        /// Po úspěšném vložení do DB se nový hráč přidá i sem, aby se UI ihned aktualizovalo
        /// </summary>
        private readonly ObservableCollection<Hrac> _hraciData;

        /// <summary>
        /// Seznam pozic dostupných v ComboBoxu
        /// Drží se jako List, protože se nemění za běhu dialogu a je jednoduchý na binding
        /// </summary>
        public List<Pozice> PoziceList { get; } = new List<Pozice>()
        {
            new Pozice { Id = 1, Nazev = "Brankář" },
            new Pozice { Id = 2, Nazev = "Obránce" },
            new Pozice { Id = 3, Nazev = "Záložník" },
            new Pozice { Id = 4, Nazev = "Útočník" }
        };

        private Pozice _selectedPozice;

        /// <summary>
        /// Aktuálně vybraná pozice z ComboBoxu
        /// Používá se pro uložení IdPozice a také pro textový název do sloupce v DataGridu
        /// </summary>
        public Pozice SelectedPozice
        {
            get { return _selectedPozice; }
            set
            {
                _selectedPozice = value;
                OnPropertyChanged();
            }
        }

        private string _rodneCislo = "";

        /// <summary>
        /// Rodné číslo zadávané uživatelem.
        /// Binding probíhá přímo na TextBox, hodnoty se validují před uložením
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

        private string _jmeno = "";

        /// <summary>
        /// Jméno hráče zadávané uživatelem
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

        private string _prijmeni = "";

        /// <summary>
        /// Příjmení hráče zadávané uživatelem.
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

        private string _telefonniCislo = "";

        /// <summary>
        /// Telefonní číslo hráče.
        /// Kontrola formátu probíhá v Validatoru
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

        private int _pocetGolu = 0;

        /// <summary>
        /// Počet gólů hráče.
        /// V UI je zadáván přes IntegerUpDown
        /// </summary>
        public int PocetGolu
        {
            get { return _pocetGolu; }
            set
            {
                _pocetGolu = value;
                OnPropertyChanged();
            }
        }

        private int _pocetZlutychKaret = 0;

        /// <summary>
        /// Počet žlutých karet hráče
        /// </summary>
        public int PocetZlutychKaret
        {
            get { return _pocetZlutychKaret; }
            set
            {
                _pocetZlutychKaret = value;
                OnPropertyChanged();
            }
        }

        private int _pocetCervenychKaret = 0;

        /// <summary>
        /// Počet červených karet hráče
        /// </summary>
        public int PocetCervenychKaret
        {
            get { return _pocetCervenychKaret; }
            set
            {
                _pocetCervenychKaret = value;
                OnPropertyChanged();
            }
        }

        private bool _maOpatreni = false;

        /// <summary>
        /// Indikuje, zda má hráč disciplinární opatření
        /// </summary>
        public bool MaOpatreni
        {
            get { return _maOpatreni; }
            set
            {
                _maOpatreni = value;
                OnPropertyChanged();

                if (_maOpatreni == false)
                {
                    DatumOpatreni = null;
                    DelkaTrestu = 1;
                    DuvodOpatreni = "";
                }
            }
        }

        private DateTime? _datumOpatreni = null;

        /// <summary>
        /// Datum disciplinárního opatření
        /// Vyplňuje se pouze pokud je MaOpatreni = true
        /// </summary>
        public DateTime? DatumOpatreni
        {
            get { return _datumOpatreni; }
            set
            {
                _datumOpatreni = value;
                OnPropertyChanged();
            }
        }

        private int _delkaTrestu = 1;

        /// <summary>
        /// Délka trestu ve dnech
        /// Vyplňuje se pouze pokud je MaOpatreni = true
        /// </summary>
        public int DelkaTrestu
        {
            get { return _delkaTrestu; }
            set
            {
                _delkaTrestu = value;
                OnPropertyChanged();
            }
        }

        private string _duvodOpatreni = "";

        /// <summary>
        /// Textový důvod opatření (volitelné)
        /// </summary>
        public string DuvodOpatreni
        {
            get { return _duvodOpatreni; }
            set
            {
                _duvodOpatreni = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Příkaz pro potvrzení přidání hráče
        /// </summary>
        public RelayCommand PridejCommand { get; }

        /// <summary>
        /// Příkaz pro reset formuláře na výchozí hodnoty
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Inicializace ViewModelu
        /// </summary>
        /// <param name="hraciData">Kolekce hráčů, která je navázaná na DataGrid v okně Hráči.</param>
        public DialogPridejHraceViewModel(ObservableCollection<Hrac> hraciData)
        {
            _hraciData = hraciData;
            SelectedPozice = PoziceList[0];

            PridejCommand = new RelayCommand(_ => Pridej());
            ResetCommand = new RelayCommand(_ => Reset());
        }

        /// <summary>
        /// Resetuje hodnoty formuláře do výchozího stavu.
        /// Odpovídá chování původního code-behind (vymazání polí, nastavení defaultů)
        /// </summary>
        private void Reset()
        {
            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            TelefonniCislo = "";
            SelectedPozice = PoziceList[0];
            PocetCervenychKaret = 0;
            PocetGolu = 0;
            PocetZlutychKaret = 0;
            MaOpatreni = false;
        }

        /// <summary>
        /// Provede přidání nového hráče
        /// Metoda provádí validaci vstupů, vytvoří objekt Hrac, zapíše jej do databáze
        /// a následně přidá do kolekce navázané na DataGrid
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

                int idPozice = SelectedPozice.Id;

                // Validace
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);

                Validator.ValidujCeleCislo(PocetGolu.ToString(), "Počet gólů");
                Validator.ValidujCeleCislo(PocetZlutychKaret.ToString(), "Počet žlutých karet");
                Validator.ValidujCeleCislo(PocetCervenychKaret.ToString(), "Počet červených karet");

                if (MaOpatreni)
                {
                    Validator.ValidujDatum(DatumOpatreni, "Datum disciplinárního opatření");
                    Validator.ValidujCeleCislo(DelkaTrestu.ToString(), "Délka trestu");
                }

                // vytvoreni Hrace
                Hrac novyHrac = new Hrac(
                    rodneCislo,
                    jmeno,
                    prijmeni,
                    telCislo,
                    PocetGolu,
                    PocetZlutychKaret,
                    PocetCervenychKaret,
                    idPozice
                );

                novyHrac.PoziceNaHristi = SelectedPozice.Nazev;

                if (MaOpatreni)
                {
                    if (DatumOpatreni == null)
                    {
                        throw new Exception("Datum disciplinárního opatření nesmí být prázdné.");
                    }

                    novyHrac.DatumOpatreni = DatumOpatreni.Value;
                    novyHrac.DelkaTrestu = DelkaTrestu;

                    string duvod;
                    if (DuvodOpatreni != null)
                    {
                        duvod = DuvodOpatreni.Trim();
                    }
                    else
                    {
                        duvod = "";
                    }

                    novyHrac.DuvodOpatreni = duvod;
                    novyHrac.DatumOpatreniText = novyHrac.DatumOpatreni.ToString("dd.MM.yyyy");
                }
                else
                {
                    novyHrac.DatumOpatreni = DateTime.MinValue;
                    novyHrac.DelkaTrestu = 0;
                    novyHrac.DuvodOpatreni = null;
                    novyHrac.DatumOpatreniText = "Bez opatření";
                }

                // DB uložení
                var conn = DatabaseManager.GetConnection();

                if (Validator.ExistujeRodneCislo(conn, rodneCislo))
                {
                    MessageBox.Show("Hráč s tímto rodným číslem již existuje!",
                        "Duplicitní rodné číslo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                DatabaseHraci.AddHrac(conn, novyHrac);

                // Aktualizace UI kolekce
                _hraciData.Add(novyHrac);

                MessageBox.Show("Hráč byl úspěšně přidán",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přidávání hráče:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
