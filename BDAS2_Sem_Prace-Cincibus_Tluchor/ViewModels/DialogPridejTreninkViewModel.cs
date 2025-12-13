using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialog přidání tréninku.
    /// Drží hodnoty formuláře, načítá trenéry z DB a po validaci vloží nový trénink
    /// </summary>
    public class DialogPridejTreninkViewModel : ViewModelBase
    {
        /// <summary>
        /// Maximální délka popisu (omezení kvůli DB/validaci)
        /// </summary>
        private const int MaxPopis = 30;

        /// <summary>
        /// Kolekce tréninků v nadřazeném okně – po přidání se do ní vloží nový záznam
        /// </summary>
        private readonly ObservableCollection<TreninkView> _treninkyData;

        /// <summary>
        /// Seznam trenérů pro ComboBox 
        /// </summary>
        public ObservableCollection<TrenerItem> Treneri { get; private set; } = new ObservableCollection<TrenerItem>();

        private TrenerItem _selectedTrener;

        /// <summary>
        /// Aktuálně vybraný trenér z ComboBoxu
        /// </summary>
        public TrenerItem SelectedTrener
        {
            get { return _selectedTrener; }
            set { _selectedTrener = value; OnPropertyChanged(); }
        }

        private DateTime? _selectedDateTime;

        /// <summary>
        /// Datum a čas tréninku vybrané uživatelem
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get { return _selectedDateTime; }
            set { _selectedDateTime = value; OnPropertyChanged(); }
        }

        private string _misto = "";

        /// <summary>
        /// Místo konání tréninku (TextBox)
        /// </summary>
        public string Misto
        {
            get { return _misto; }
            set
            {
                if (value == null)
                {
                    _misto = "";
                }
                else
                {
                    _misto = value;
                }

                OnPropertyChanged();
            }
        }

        private string _popis = "";

        /// <summary>
        /// Popis / náplň tréninku (volitelný)
        /// </summary>
        public string Popis
        {
            get { return _popis; }
            set
            {
                string text;

                if (value == null)
                {
                    text = "";
                }
                else
                {
                    text = value;
                }

                text = text.Trim();

                if (text.Length > MaxPopis)
                {
                    text = text.Substring(0, MaxPopis);
                }

                _popis = text;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command pro přidání tréninku (tlačítko "Přidej")
        /// </summary>
        public RelayCommand PridejCommand { get; private set; }

        /// <summary>
        /// Command pro reset formuláře (tlačítko "Reset")
        /// </summary>
        public RelayCommand ResetCommand { get; private set; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu (true = OK, false = Cancel).
        /// </summary>
        public event Action<bool> RequestClose;

        public DialogPridejTreninkViewModel(ObservableCollection<TreninkView> treninkyData, bool designTime)
        {
            _treninkyData = treninkyData;

            SelectedDateTime = DateTime.Now;
            Misto = "";
            Popis = "";
            SelectedTrener = null;

            // Commandy nastav 
            PridejCommand = new RelayCommand(_ => Pridej());
            ResetCommand = new RelayCommand(_ => Reset());

            if (!designTime)
            {
                NactiTreneryZDb();
            }
        }

        public DialogPridejTreninkViewModel(ObservableCollection<TreninkView> treninkyData)
            : this(treninkyData, false)
        {
        }

        /// <summary>
        /// Resetuje hodnoty formuláře do výchozího stavu.
        /// </summary>
        private void Reset()
        {
            Popis = "";
            SelectedDateTime = DateTime.Now;
            Misto = "";
            SelectedTrener = null;
        }

        /// <summary>
        /// Načte trenéry z databáze do kolekce Treneri
        /// </summary>
        private void NactiTreneryZDb()
        {
            Treneri.Clear();

            try
            {
                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT RODNE_CISLO, PRIJMENI FROM TRENERI_VIEW", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string rc;
                        if (reader["RODNE_CISLO"] != DBNull.Value)
                        {
                            rc = reader["RODNE_CISLO"].ToString();
                        }
                        else
                        {
                            rc = "";
                        }

                        string prijmeni;
                        if (reader["PRIJMENI"] != DBNull.Value)
                        {
                            prijmeni = reader["PRIJMENI"].ToString();
                        }
                        else
                        {
                            prijmeni = "";
                        }

                        Treneri.Add(new TrenerItem(prijmeni, rc));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání trenérů:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Validuje vstupy a uloží nový trénink do DB i do kolekce
        /// </summary>
        private void Pridej()
        {
            try
            {
                if (SelectedTrener == null)
                {
                    throw new Exception("Vyberte trenéra");
                }

                Validator.ValidujDatum(SelectedDateTime, "Datum tréninku");

                string misto = Misto;
                if (misto == null)
                {
                    misto = "";
                }
                misto = misto.Trim();
                Validator.ValidujMistoTreninku(misto);

                string popis = Popis;
                if (popis == null)
                {
                    popis = "";
                }
                popis = popis.Trim();
                Validator.ValidujPopisTreninku(popis);

                string rc = SelectedTrener.RodneCislo;
                if (rc == null)
                {
                    rc = "";
                }
                rc = rc.Trim();
                Validator.ValidujRodneCislo(rc);

                if (SelectedDateTime == null)
                {
                    throw new Exception("Datum tréninku není vyplněné");
                }

                TreninkView novyTrenink = new TreninkView();
                novyTrenink.Prijmeni = SelectedTrener.Prijmeni;
                novyTrenink.RodneCislo = rc;
                novyTrenink.Datum = SelectedDateTime.Value;
                novyTrenink.Misto = misto;
                novyTrenink.Popis = popis;

                var conn = DatabaseManager.GetConnection();

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseTreninky.AddTrenink(conn, novyTrenink);

                _treninkyData.Add(novyTrenink);

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Položka pro ComboBox trenérů.
        /// </summary>
        public class TrenerItem
        {
            /// <summary>
            /// Příjmení trenéra.
            /// </summary>
            public string Prijmeni { get; private set; }

            /// <summary>
            /// Rodné číslo trenéra.
            /// </summary>
            public string RodneCislo { get; private set; }

            /// <summary>
            /// Text zobrazovaný v ComboBoxu (např. "Novák (0000000000)").
            /// </summary>
            public string Display { get; private set; }

            /// <summary>
            /// Vytvoří položku trenéra a připraví Display text.
            /// </summary>
            public TrenerItem(string prijmeni, string rodneCislo)
            {
                if (prijmeni == null)
                {
                    prijmeni = "";
                }

                if (rodneCislo == null)
                {
                    rodneCislo = "";
                }

                Prijmeni = prijmeni;
                RodneCislo = rodneCislo;
                Display = prijmeni + " (" + rodneCislo + ")";
            }
        }
    }
}
