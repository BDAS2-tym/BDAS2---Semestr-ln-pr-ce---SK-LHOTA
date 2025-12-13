using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro okno tréninků
    /// data pro DataGrid a obsluhuje tlačítka přes příkazy
    /// </summary>
    public class TreninkyOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// Všechna data načtená z databáze
        /// </summary>
        public ObservableCollection<TreninkView> TreninkyData { get; private set; }

        private ObservableCollection<TreninkView> _zobrazovaneTreninky;

        /// <summary>
        /// Kolekce, která je zobrazená v DataGridu
        /// Při vyhledávání se nastaví filtrovaná kolekce
        /// </summary>
        public ObservableCollection<TreninkView> ZobrazovaneTreninky
        {
            get { return _zobrazovaneTreninky; }
            private set
            {
                _zobrazovaneTreninky = value;
                OnPropertyChanged();
            }
        }

        private TreninkView _selectedTrenink;

        /// <summary>
        /// Vybraný trénink v DataGridu
        /// </summary>
        public TreninkView SelectedTrenink
        {
            get { return _selectedTrenink; }
            set
            {
                _selectedTrenink = value;
                OnPropertyChanged();

                _odeberCommand.RaiseCanExecuteChanged();
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _jeVyhledavaniAktivni;

        /// <summary>
        /// Indikuje, že je aktivní vyhledávací mód
        /// </summary>
        public bool JeVyhledavaniAktivni
        {
            get { return _jeVyhledavaniAktivni; }
            private set
            {
                _jeVyhledavaniAktivni = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModify));
            }
        }

        private bool _canAddRemoveEdit;

        /// <summary>
        /// Oprávnění pro přidávání, mazání a úpravu
        /// </summary>
        public bool CanAddRemoveEdit
        {
            get { return _canAddRemoveEdit; }
            private set
            {
                _canAddRemoveEdit = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModify));
            }
        }

        private bool _canSeeRodneCislo;

        /// <summary>
        /// Ovládá viditelnost sloupce Rodné číslo
        /// </summary>
        public bool CanSeeRodneCislo
        {
            get { return _canSeeRodneCislo; }
            private set
            {
                _canSeeRodneCislo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Vypíná úpravy během vyhledávání
        /// </summary>
        public bool CanModify
        {
            get
            {
                if (CanAddRemoveEdit == true && JeVyhledavaniAktivni == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ICommand PridejCommand { get { return _pridejCommand; } }
        public ICommand OdeberCommand { get { return _odeberCommand; } }
        public ICommand NajdiCommand { get { return _najdiCommand; } }
        public ICommand ZpetCommand { get { return _zpetCommand; } }
        public ICommand StatistikyCommand { get { return _statistikyCommand; } }
        public ICommand EditCommand { get { return _editCommand; } }
        public ICommand ClearFilterCommand { get { return _clearFilterCommand; } }

        private readonly AppRelayCommand _pridejCommand;
        private readonly AppRelayCommand _odeberCommand;
        private readonly AppRelayCommand _najdiCommand;
        private readonly AppRelayCommand _zpetCommand;
        private readonly AppRelayCommand _statistikyCommand;
        private readonly AppRelayCommand _editCommand;
        private readonly AppRelayCommand _clearFilterCommand;

        /// <summary>
        /// View si otevře dialogy přes tyto události
        /// </summary>
        public event Action RequestOpenPridejDialog;
        public event Action RequestOpenNajdiDialog;
        public event Action RequestOpenStatistikyDialog;
        public event Action RequestBack;
        public event Action<TreninkView> RequestOpenEditDialog;

        /// <summary>
        /// Inicializace ViewModelu
        /// </summary>
        public TreninkyOknoViewModel()
        {
            TreninkyData = new ObservableCollection<TreninkView>();
            _zobrazovaneTreninky = new ObservableCollection<TreninkView>();

            ApplyRightsFromRole();
            NactiTreninky();

            ZobrazovaneTreninky = TreninkyData;
            JeVyhledavaniAktivni = false;

            _pridejCommand = new AppRelayCommand(
                OtevriPridejDialog,
                () =>
                {
                    if (CanModify == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            );

            _odeberCommand = new AppRelayCommand(
                OdeberSelectedTrenink,
                () =>
                {
                    if (CanModify == true && SelectedTrenink != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            );

            _najdiCommand = new AppRelayCommand(OtevriNajdiDialog);

            _statistikyCommand = new AppRelayCommand(OtevriStatistikyDialog);

            _zpetCommand = new AppRelayCommand(Zpet);

            _editCommand = new AppRelayCommand(
                EditSelectedTrenink,
                () =>
                {
                    if (CanModify == true && SelectedTrenink != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            );

            _clearFilterCommand = new AppRelayCommand(ClearFilter);

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Načte práva podle role přihlášeného uživatele
        /// </summary>
        private void ApplyRightsFromRole()
        {
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = "host";
            if (uzivatel != null)
            {
                if (uzivatel.Role != null)
                {
                    role = uzivatel.Role.ToLower();
                }
                else
                {
                    role = "host";
                }
            }
            else
            {
                role = "host";
            }

            if (role == "trener" || role == "hrac" || role == "host" || role == "uzivatel")
            {
                CanSeeRodneCislo = false;
            }
            else
            {
                CanSeeRodneCislo = true;
            }

            if (role == "hrac" || role == "host" || role == "uzivatel")
            {
                CanAddRemoveEdit = false;
            }
            else
            {
                CanAddRemoveEdit = true;
            }
        }

        /// <summary>
        /// Předá View požadavek na otevření dialogu přidání
        /// </summary>
        private void OtevriPridejDialog()
        {
            if (RequestOpenPridejDialog != null)
            {
                RequestOpenPridejDialog.Invoke();
            }
            else
            {
            }
        }

        /// <summary>
        /// Předá View požadavek na otevření dialogu hledání
        /// </summary>
        private void OtevriNajdiDialog()
        {
            if (RequestOpenNajdiDialog != null)
            {
                RequestOpenNajdiDialog.Invoke();
            }
            else
            {
            }
        }

        /// <summary>
        /// Předá View požadavek na otevření statistik
        /// </summary>
        private void OtevriStatistikyDialog()
        {
            if (RequestOpenStatistikyDialog != null)
            {
                RequestOpenStatistikyDialog.Invoke();
            }
            else
            {
            }
        }

        /// <summary>
        /// Návrat zpět do hlavního okna
        /// </summary>
        private void Zpet()
        {
            if (RequestBack != null)
            {
                RequestBack.Invoke();
            }
            else
            {
            }
        }

        /// <summary>
        /// Otevření editace vybraného tréninku
        /// </summary>
        private void EditSelectedTrenink()
        {
            if (CanAddRemoveEdit == false)
            {
                MessageBox.Show("Nemáte oprávnění upravovat tréninky",
                    "Omezení přístupu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
            }

            if (JeVyhledavaniAktivni == true)
            {
                MessageBox.Show("Ve vyhledávacím módu nelze upravovat záznamy",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
            }

            if (SelectedTrenink == null)
            {
                MessageBox.Show("Prosím vyberte trénink, který chcete editovat",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
            }

            if (RequestOpenEditDialog != null)
            {
                RequestOpenEditDialog.Invoke(SelectedTrenink);
            }
            else
            {
            }
        }

        /// <summary>
        /// Aplikuje filtr a přepne okno do vyhledávacího módu
        /// </summary>
        public void ApplyFilter(ObservableCollection<TreninkView> filtrovane)
        {
            if (filtrovane == null)
            {
                return;
            }
            else
            {
            }

            if (filtrovane.Count == 0)
            {
                MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry",
                    "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
            }

            MessageBox.Show(
                "Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            ZobrazovaneTreninky = filtrovane;
            JeVyhledavaniAktivni = true;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Zruší filtr a vrátí zpět původní kolekci
        /// </summary>
        public void ClearFilter()
        {
            JeVyhledavaniAktivni = false;
            ZobrazovaneTreninky = TreninkyData;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Smaže vybraný trénink po potvrzení uživatele
        /// </summary>
        private void OdeberSelectedTrenink()
        {
            if (CanAddRemoveEdit == false)
            {
                MessageBox.Show("Nemáte oprávnění mazat tréninky",
                    "Omezení přístupu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
            }

            if (JeVyhledavaniAktivni == true)
            {
                MessageBox.Show("Ve vyhledávacím módu nelze mazat záznamy",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
            }

            if (SelectedTrenink == null)
            {
                MessageBox.Show("Prosím vyberte trénink, který chcete odebrat",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
            }

            var potvrzeni = MessageBox.Show(
                "Opravdu chcete odebrat trénink trenéra " + SelectedTrenink.Prijmeni + " v " + SelectedTrenink.Datum + "?",
                "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }
            else
            {
            }

            try
            {
                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                DatabaseTreninky.DeleteTrenink(conn, SelectedTrenink);

                TreninkyData.Remove(SelectedTrenink);
                ZobrazovaneTreninky.Remove(SelectedTrenink);

                SelectedTrenink = null;

                MessageBox.Show("Trénink byl úspěšně odebrán",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Chyba databáze při mazání tréninku:\n" + ex.Message,
                    "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala neočekávaná chyba při mazání tréninku:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Načte tréninky z pohledu TRENINKY_VIEW
        /// </summary>
        private void NactiTreninky()
        {
            try
            {
                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT * FROM TRENINKY_VIEW", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        TreninkyData.Clear();

                        while (reader.Read())
                        {
                            TreninkView trenink = new TreninkView();

                            if (reader["RODNE_CISLO"] != DBNull.Value)
                            {
                                trenink.RodneCislo = reader["RODNE_CISLO"].ToString();
                            }
                            else
                            {
                                trenink.RodneCislo = "";
                            }

                            if (reader["PRIJMENI"] != DBNull.Value)
                            {
                                trenink.Prijmeni = reader["PRIJMENI"].ToString();
                            }
                            else
                            {
                                trenink.Prijmeni = "";
                            }

                            if (reader["POPIS"] != DBNull.Value)
                            {
                                trenink.Popis = reader["POPIS"].ToString();
                            }
                            else
                            {
                                trenink.Popis = "Volitelné nezadáno !";
                            }

                            if (reader["MISTO"] != DBNull.Value)
                            {
                                trenink.Misto = reader["MISTO"].ToString();
                            }
                            else
                            {
                                trenink.Misto = "";
                            }

                            if (reader["DATUM"] != DBNull.Value)
                            {
                                trenink.Datum = Convert.ToDateTime(reader["DATUM"]);
                            }
                            else
                            {
                                trenink.Datum = DateTime.MinValue;
                            }

                            TreninkyData.Add(trenink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání tréninků:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
