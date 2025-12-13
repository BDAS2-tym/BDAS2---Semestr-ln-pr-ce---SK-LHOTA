using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro okno trenérů
    /// </summary>
    public class TreneriOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// kolekce trenérů načtená z databáze
        /// </summary>
        public ObservableCollection<Trener> TreneriData { get; private set; }

        private ObservableCollection<Trener> _zobrazovaniTreneri;

        /// <summary>
        /// Kolekce navázaná na DataGrid
        /// Při vyhledávání se přepne na filtrovaný seznam
        /// </summary>
        public ObservableCollection<Trener> ZobrazovaniTreneri
        {
            get { return _zobrazovaniTreneri; }
            private set
            {
                _zobrazovaniTreneri = value;
                OnPropertyChanged();
            }
        }

        private Trener _selectedTrener;

        /// <summary>
        /// Aktuálně vybraný trenér v tabulce
        /// </summary>
        public Trener SelectedTrener
        {
            get { return _selectedTrener; }
            set
            {
                _selectedTrener = value;
                OnPropertyChanged();

                _odeberCommand.RaiseCanExecuteChanged();
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Určuje, zda je aktivní vyhledávací mód
        /// </summary>
        public bool JeVyhledavaniAktivni { get; private set; }

        /// <summary>
        /// Práva pro přidání, odebrání a editaci podle role uživatele
        /// </summary>
        public bool CanAddRemoveEdit { get; private set; }

        public ICommand PridejCommand { get { return _pridejCommand; } }
        public ICommand OdeberCommand { get { return _odeberCommand; } }
        public ICommand NajdiCommand { get { return _najdiCommand; } }
        public ICommand ExportTopTreneriCommand { get { return _exportTopTreneriCommand; } }
        public ICommand ZpetCommand { get { return _zpetCommand; } }
        public ICommand EditCommand { get { return _editCommand; } }

        private readonly AppRelayCommand _pridejCommand;
        private readonly AppRelayCommand _odeberCommand;
        private readonly AppRelayCommand _najdiCommand;
        private readonly AppRelayCommand _exportTopTreneriCommand;
        private readonly AppRelayCommand _zpetCommand;
        private readonly AppRelayCommand _editCommand;

        /// <summary>
        /// Požadavek na otevření dialogu přidání
        /// </summary>
        public event Action RequestOpenPridejDialog;

        /// <summary>
        /// Požadavek na otevření dialogu hledání
        /// </summary>
        public event Action RequestOpenNajdiDialog;

        /// <summary>
        /// Požadavek na export TOP 3 trenérů
        /// View řeší SaveFileDialog, VM jen požádá o akci
        /// </summary>
        public event Action RequestExportTop3;

        /// <summary>
        /// Požadavek na návrat do hlavního okna
        /// </summary>
        public event Action RequestBack;

        /// <summary>
        /// Požadavek na otevření edit dialogu pro konkrétního trenéra
        /// </summary>
        public event Action<Trener> RequestOpenEditDialog;

        /// <summary>
        /// Inicializace ViewModelu
        /// </summary>
        public TreneriOknoViewModel()
        {
            TreneriData = new ObservableCollection<Trener>();
            _zobrazovaniTreneri = new ObservableCollection<Trener>();

            ApplyRightsFromRole();
            NactiTrenery();

            ZobrazovaniTreneri = TreneriData;

            _pridejCommand = new AppRelayCommand(
                () =>
                {
                    if (RequestOpenPridejDialog != null)
                    {
                        RequestOpenPridejDialog.Invoke();
                    }
                },
                () =>
                {
                    if (CanAddRemoveEdit == false) { return false; }
                    if (JeVyhledavaniAktivni == true) { return false; }
                    return true;
                }
            );

            _odeberCommand = new AppRelayCommand(
                () => OdeberSelectedTrenera(),
                () =>
                {
                    if (CanAddRemoveEdit == false) { return false; }
                    if (JeVyhledavaniAktivni == true) { return false; }
                    if (SelectedTrener == null) { return false; }
                    return true;
                }
            );

            _najdiCommand = new AppRelayCommand(
                () =>
                {
                    if (RequestOpenNajdiDialog != null)
                    {
                        RequestOpenNajdiDialog.Invoke();
                    }
                }
            );

            _exportTopTreneriCommand = new AppRelayCommand(
                () =>
                {
                    if (RequestExportTop3 != null)
                    {
                        RequestExportTop3.Invoke();
                    }
                }
            );

            _zpetCommand = new AppRelayCommand(
                () =>
                {
                    if (RequestBack != null)
                    {
                        RequestBack.Invoke();
                    }
                }
            );

            _editCommand = new AppRelayCommand(
                () =>
                {
                    if (CanAddRemoveEdit == false)
                    {
                        MessageBox.Show("Nemáte oprávnění upravovat trenéry",
                            "Omezení přístupu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (SelectedTrener == null)
                    {
                        MessageBox.Show("Prosím vyberte trenéra, kterého chcete editovat",
                            "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (RequestOpenEditDialog != null)
                    {
                        RequestOpenEditDialog.Invoke(SelectedTrener);
                    }
                },
                () =>
                {
                    if (CanAddRemoveEdit == false) { return false; }
                    if (SelectedTrener == null) { return false; }
                    if (JeVyhledavaniAktivni == true) { return false; }
                    return true;
                }
            );

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Nastaví práva podle role přihlášeného uživatele
        /// </summary>
        private void ApplyRightsFromRole()
        {
            Uzivatel u = HlavniOkno.GetPrihlasenyUzivatel();

            string role = "host";
            if (u != null)
            {
                if (u.Role != null)
                {
                    role = u.Role.ToLower();
                }
            }

            CanAddRemoveEdit = true;

            if (role == "hrac" || role == "trener" || role == "uzivatel" || role == "host")
            {
                CanAddRemoveEdit = false;
            }

            OnPropertyChanged(nameof(CanAddRemoveEdit));
        }

        /// <summary>
        /// Přepne DataGrid na filtrované výsledky a aktivuje vyhledávací mód
        /// </summary>
        public void ApplyFilter(ObservableCollection<Trener> filtrovani)
        {
            if (filtrovani == null)
            {
                return;
            }

            if (filtrovani.Count == 0)
            {
                MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry",
                    "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show(
                "Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data " +
                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            ZobrazovaniTreneri = filtrovani;
            JeVyhledavaniAktivni = true;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Zruší vyhledávací mód a vrátí DataGrid na původní kolekci
        /// </summary>
        public void ClearFilter()
        {
            JeVyhledavaniAktivni = false;
            ZobrazovaniTreneri = TreneriData;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Odebere vybraného trenéra včetně případného uživatelského účtu
        /// </summary>
        private void OdeberSelectedTrenera()
        {
            if (SelectedTrener == null)
            {
                MessageBox.Show("Prosím vyberte trenéra, kterého chcete odebrat",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult potvrzeni = MessageBox.Show(
                "Opravdu chcete odebrat trenéra " + SelectedTrener.Jmeno + " " + SelectedTrener.Prijmeni + " ?",
                "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                string sqlFindAcc =
                    "SELECT UZIVATELSKEJMENO " +
                    "FROM PREHLED_UZIVATELSKE_UCTY " +
                    "WHERE RODNE_CISLO = :rc";

                string uzivatelskeJmeno = null;

                using (OracleCommand cmdFind = new OracleCommand(sqlFindAcc, conn))
                {
                    cmdFind.Parameters.Add(":rc", OracleDbType.Varchar2).Value = SelectedTrener.RodneCislo;

                    object result = cmdFind.ExecuteScalar();

                    if (result != null)
                    {
                        uzivatelskeJmeno = result.ToString();
                    }
                }

                if (string.IsNullOrEmpty(uzivatelskeJmeno) == false)
                {
                    Uzivatel uzivatelTrener = new Uzivatel();
                    uzivatelTrener.UzivatelskeJmeno = uzivatelskeJmeno;

                    DatabaseRegistrace.DeleteUzivatel(conn, uzivatelTrener);
                }

                DatabaseTreneri.OdeberTrenera(conn, SelectedTrener);

                TreneriData.Remove(SelectedTrener);
                SelectedTrener = null;

                MessageBox.Show("Trenér byl úspěšně odebrán",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Chyba databáze při mazání trenéra:\n" + ex.Message,
                    "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala neočekávaná chyba při mazání trenéra:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Načte trenéry z databáze a naplní kolekci pro DataGrid
        /// </summary>
        private void NactiTrenery()
        {
            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT * FROM TRENERI_VIEW", conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    TreneriData.Clear();

                    while (reader.Read())
                    {
                        Trener trener = new Trener();

                        if (reader["RODNE_CISLO"] != DBNull.Value) { trener.RodneCislo = reader["RODNE_CISLO"].ToString(); }
                        else { trener.RodneCislo = ""; }

                        if (reader["JMENO"] != DBNull.Value) { trener.Jmeno = reader["JMENO"].ToString(); }
                        else { trener.Jmeno = ""; }

                        if (reader["PRIJMENI"] != DBNull.Value) { trener.Prijmeni = reader["PRIJMENI"].ToString(); }
                        else { trener.Prijmeni = ""; }

                        if (reader["TELEFONNICISLO"] != DBNull.Value) { trener.TelefonniCislo = reader["TELEFONNICISLO"].ToString(); }
                        else { trener.TelefonniCislo = "000000000"; }

                        if (reader["TRENERSKALICENCE"] != DBNull.Value) { trener.TrenerskaLicence = reader["TRENERSKALICENCE"].ToString(); }
                        else { trener.TrenerskaLicence = ""; }

                        if (reader["SPECIALIZACE"] != DBNull.Value) { trener.Specializace = reader["SPECIALIZACE"].ToString(); }
                        else { trener.Specializace = "Volitelné nezadáno"; }

                        if (reader["POCETLETPRAXE"] != DBNull.Value) { trener.PocetLetPraxe = Convert.ToInt32(reader["POCETLETPRAXE"]); }
                        else { trener.PocetLetPraxe = 0; }

                        TreneriData.Add(trener);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání trenérů:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
