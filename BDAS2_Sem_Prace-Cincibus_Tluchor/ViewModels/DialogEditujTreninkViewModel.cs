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
    /// ViewModel pro dialog editace tréninku
    /// Drží hodnoty formuláře a ukládá změny do databáze
    /// </summary>
    public class DialogEditujTreninkViewModel : ViewModelBase
    {
        private const int MaxPopis = 30;

        private readonly TreninkView _editovanyTrenink;
        private readonly Action _requestRefreshGrid;

        /// <summary>
        /// Položka pro ComboBox trenérů
        /// </summary>
        public class TrenerItem
        {
            public string Prijmeni { get; set; }
            public string RodneCislo { get; set; }
            public string Display { get; set; }
        }

        private ObservableCollection<TrenerItem> _treneri;

        /// <summary>
        /// Seznam trenérů pro ComboBox
        /// </summary>
        public ObservableCollection<TrenerItem> Treneri
        {
            get { return _treneri; }
            private set
            {
                _treneri = value;
                OnPropertyChanged();
            }
        }

        private TrenerItem _selectedTrener;

        /// <summary>
        /// Vybraný trenér v ComboBoxu
        /// </summary>
        public TrenerItem SelectedTrener
        {
            get { return _selectedTrener; }
            set
            {
                _selectedTrener = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _datumTreninku;

        /// <summary>
        /// Datum tréninku
        /// </summary>
        public DateTime? DatumTreninku
        {
            get { return _datumTreninku; }
            set
            {
                _datumTreninku = value;
                OnPropertyChanged();
            }
        }

        private string _misto;

        /// <summary>
        /// Místo tréninku
        /// </summary>
        public string Misto
        {
            get { return _misto; }
            set
            {
                _misto = value;
                OnPropertyChanged();
            }
        }

        private string _popis;

        /// <summary>
        /// Popis tréninku
        /// Text se drží max v délce MaxPopis
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
                else
                {
                }

                _popis = text;
                OnPropertyChanged();
            }
        }

        public RelayCommand EditujCommand { get; }
        public RelayCommand UkonciCommand { get; }

        /// <summary>
        /// Zavření dialogu z ViewModelu
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Inicializace ViewModelu a naplnění formuláře
        /// </summary>
        public DialogEditujTreninkViewModel(TreninkView trenink, Action requestRefreshGrid)
        {
            _editovanyTrenink = trenink;
            _requestRefreshGrid = requestRefreshGrid;

            _treneri = new ObservableCollection<TrenerItem>();

            if (trenink != null)
            {
                DatumTreninku = trenink.Datum;

                if (trenink.Misto != null)
                {
                    Misto = trenink.Misto;
                }
                else
                {
                    Misto = "";
                }

                if (trenink.Popis != null)
                {
                    Popis = trenink.Popis;
                }
                else
                {
                    Popis = "";
                }
            }
            else
            {
                DatumTreninku = null;
                Misto = "";
                Popis = "";
            }

            NactiTrenery();
            PredvyberTreneraDleTreninku();

            EditujCommand = new RelayCommand(_ => Edituj());
            UkonciCommand = new RelayCommand(_ => Ukonci());
        }

        /// <summary>
        /// Zavře dialog bez uložení
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
        /// Načte trenéry z databáze pro ComboBox
        /// </summary>
        private void NactiTrenery()
        {
            Treneri.Clear();

            try
            {
                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT * FROM TRENERI_VIEW", conn))
                {
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

                            TrenerItem item = new TrenerItem();
                            item.RodneCislo = rc;
                            item.Prijmeni = prijmeni;
                            item.Display = prijmeni + " (" + rc + ")";

                            Treneri.Add(item);
                        }
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
        /// Předvybere trenéra podle rodného čísla v tréninku
        /// </summary>
        private void PredvyberTreneraDleTreninku()
        {
            if (_editovanyTrenink == null)
            {
                return;
            }
            else
            {
            }

            string rcTreninku;
            if (_editovanyTrenink.RodneCislo != null)
            {
                rcTreninku = _editovanyTrenink.RodneCislo;
            }
            else
            {
                rcTreninku = "";
            }

            foreach (TrenerItem t in Treneri)
            {
                if (t.RodneCislo == rcTreninku)
                {
                    SelectedTrener = t;
                    break;
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// Provede validaci a uloží změny do objektu i databáze
        /// </summary>
        private void Edituj()
        {
            try
            {
                if (SelectedTrener == null)
                {
                    throw new Exception("Vyberte trenéra");
                }
                else
                {
                }

                Validator.ValidujDatum(DatumTreninku, "Datum tréninku");

                string misto;
                if (Misto != null)
                {
                    misto = Misto.Trim();
                }
                else
                {
                    misto = "";
                }

                Validator.ValidujMistoTreninku(misto);

                string popis;
                if (Popis != null)
                {
                    popis = Popis.Trim();
                }
                else
                {
                    popis = "";
                }

                Validator.ValidujPopisTreninku(popis);

                string rc;
                if (SelectedTrener.RodneCislo != null)
                {
                    rc = SelectedTrener.RodneCislo.Trim();
                }
                else
                {
                    rc = "";
                }

                Validator.ValidujRodneCislo(rc);

                string prijmeni;
                if (SelectedTrener.Prijmeni != null)
                {
                    prijmeni = SelectedTrener.Prijmeni.Trim();
                }
                else
                {
                    prijmeni = "";
                }

                if (DatumTreninku == null)
                {
                    throw new Exception("Datum tréninku nesmí být prázdné");
                }
                else
                {
                }

                _editovanyTrenink.Prijmeni = prijmeni;
                _editovanyTrenink.RodneCislo = rc;
                _editovanyTrenink.Datum = DatumTreninku.Value;
                _editovanyTrenink.Misto = misto;
                _editovanyTrenink.Popis = popis;

                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseTreninky.UpdateTrenink(conn, _editovanyTrenink);

                if (_requestRefreshGrid != null)
                {
                    _requestRefreshGrid.Invoke();
                }
                else
                {
                }

                MessageBox.Show("Trénink byl úspěšně upraven",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

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
                MessageBox.Show("Chyba při ukládání tréninku:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
