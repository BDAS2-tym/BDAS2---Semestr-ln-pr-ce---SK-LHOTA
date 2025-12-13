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
    /// ViewModel pro okno Hráči
    /// Drží kolekce pro DataGrid a obsluhuje příkazy pro práci s hráči
    /// </summary>
    public class HraciOknoViewModel : ViewModelBase
    {
        /// <summary>
        /// seznam hráčů načtený z databáze
        /// </summary>
        public ObservableCollection<Hrac> HraciData { get; } = new ObservableCollection<Hrac>();

        private ObservableCollection<Hrac> _zobrazovaniHraci = new ObservableCollection<Hrac>();

        /// <summary>
        /// Kolekce, která je přímo navázaná na DataGrid
        /// Při filtrování se  nastaví filtrovaná kolekce, jinak se ukazuje HraciData
        /// </summary>
        public ObservableCollection<Hrac> ZobrazovaniHraci
        {
            get { return _zobrazovaniHraci; }
            private set
            {
                _zobrazovaniHraci = value;
                OnPropertyChanged();
            }
        }

        private Hrac _selectedHrac;

        /// <summary>
        /// Aktuálně vybraný hráč v DataGridu
        /// Používá se pro odebrání a editaci
        /// </summary>
        public Hrac SelectedHrac
        {
            get { return _selectedHrac; }
            set
            {
                _selectedHrac = value;
                OnPropertyChanged();

                _odeberCommand.RaiseCanExecuteChanged();
                _editCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// True pokud je aktivní vyhledávací režim a zobrazuje se filtrovaný seznam
        /// </summary>
        public bool JeVyhledavaniAktivni { get; private set; }

        /// <summary>
        /// Práva pro přidání, odebrání a editaci podle role přihlášeného uživatele
        /// </summary>
        public bool CanAddRemoveEdit { get; private set; } = true;

        /// <summary>
        /// Commandy pro View
        /// Vrací se jako ICommand kvůli bindingu v XAML
        /// </summary>
        public ICommand PridejCommand { get { return _pridejCommand; } }
        public ICommand OdeberCommand { get { return _odeberCommand; } }
        public ICommand NajdiCommand { get { return _najdiCommand; } }
        public ICommand TopStrelciCommand { get { return _topStrelciCommand; } }
        public ICommand ZpetCommand { get { return _zpetCommand; } }
        public ICommand EditCommand { get { return _editCommand; } }

        private readonly AppRelayCommand _pridejCommand;
        private readonly AppRelayCommand _odeberCommand;
        private readonly AppRelayCommand _najdiCommand;
        private readonly AppRelayCommand _topStrelciCommand;
        private readonly AppRelayCommand _zpetCommand;
        private readonly AppRelayCommand _editCommand;

        /// <summary>
        /// Události, kterými si ViewModel řekne View o otevření dialogu nebo návrat zpět
        /// </summary>
        public event Action RequestOpenPridejDialog;
        public event Action RequestOpenNajdiDialog;
        public event Action RequestOpenTopStrelciDialog;
        public event Action RequestBack;
        public event Action<Hrac> RequestOpenEditDialog;

        /// <summary>
        /// Konstruktor načte data a připraví příkazy
        /// </summary>
        public HraciOknoViewModel()
        {
            ApplyRightsFromRole();
            NactiHrace();

            ZobrazovaniHraci = HraciData;

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
                    return CanAddRemoveEdit && !JeVyhledavaniAktivni;
                }
            );

            _odeberCommand = new AppRelayCommand(
                () =>
                {
                    OdeberSelectedHrace();
                },
                () =>
                {
                    return CanAddRemoveEdit && !JeVyhledavaniAktivni && SelectedHrac != null;
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

            _topStrelciCommand = new AppRelayCommand(
                () =>
                {
                    if (RequestOpenTopStrelciDialog != null)
                    {
                        RequestOpenTopStrelciDialog.Invoke();
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
                    if (!CanAddRemoveEdit)
                    {
                        MessageBox.Show("Nemáte oprávnění upravovat hráče",
                            "Omezení přístupu", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (SelectedHrac == null)
                    {
                        MessageBox.Show("Prosím vyberte hráče, kterého chcete upravit!",
                            "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (RequestOpenEditDialog != null)
                    {
                        RequestOpenEditDialog.Invoke(SelectedHrac);
                    }
                },
                () =>
                {
                    return CanAddRemoveEdit && SelectedHrac != null;
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
            var uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = "host";

            if (uzivatel != null)
            {
                if (uzivatel.Role != null)
                {
                    role = uzivatel.Role.ToLower();
                }
            }

            if (role == "hrac" || role == "trener" || role == "host")
            {
                CanAddRemoveEdit = false;
            }
            else
            {
                CanAddRemoveEdit = true;
            }

            OnPropertyChanged(nameof(CanAddRemoveEdit));
        }

        /// <summary>
        /// Zapne vyhledávací režim a nastaví filtrované výsledky do DataGridu
        /// </summary>
        public void ApplyFilter(ObservableCollection<Hrac> filtrovani)
        {
            if (filtrovani == null)
            {
                MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry",
                    "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (filtrovani.Count == 0)
            {
                MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry",
                    "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show(
                "Pokud je vyhledávací mód aktivní nemůžete přidávat ani odebírat vyhledaná data. " +
                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            ZobrazovaniHraci = filtrovani;
            JeVyhledavaniAktivni = true;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Vypne vyhledávací režim a vrátí zpět kompletní seznam
        /// </summary>
        public void ClearFilter()
        {
            JeVyhledavaniAktivni = false;
            ZobrazovaniHraci = HraciData;

            _pridejCommand.RaiseCanExecuteChanged();
            _odeberCommand.RaiseCanExecuteChanged();
            _editCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Odebere aktuálně vybraného hráče včetně navázaného uživatelského účtu
        /// </summary>
        private void OdeberSelectedHrace()
        {
            if (SelectedHrac == null)
            {
                MessageBox.Show("Prosím, vyberte hráče, kterého chcete odebrat!",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var potvrzeni = MessageBox.Show(
                "Opravdu chcete odebrat hráče " + SelectedHrac.Jmeno + " " + SelectedHrac.Prijmeni + "?",
                "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                string sql =
                    "SELECT UZIVATELSKEJMENO " +
                    "FROM PREHLED_UZIVATELSKE_UCTY " +
                    "WHERE RODNE_CISLO = :rodnecislo";

                string uzivatelskeJmeno = null;

                var cmdFind = new OracleCommand(sql, conn);
                cmdFind.Parameters.Add(":rodnecislo", OracleDbType.Varchar2).Value = SelectedHrac.RodneCislo;

                object result = cmdFind.ExecuteScalar();
                if (result != null)
                {
                    uzivatelskeJmeno = result.ToString();
                }

                if (!string.IsNullOrEmpty(uzivatelskeJmeno))
                {
                    var uzivatelHrac = new Uzivatel();
                    uzivatelHrac.UzivatelskeJmeno = uzivatelskeJmeno;

                    DatabaseRegistrace.DeleteUzivatel(conn, uzivatelHrac);
                }

                DatabaseHraci.OdeberHrace(conn, SelectedHrac);

                HraciData.Remove(SelectedHrac);
                SelectedHrac = null;

                MessageBox.Show("Hráč byl úspěšně odebrán",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Chyba databáze při mazání hráče:\n" + ex.Message,
                    "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala neočekávaná chyba při mazání hráče:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Načte hráče z databázového view a naplní HraciData
        /// </summary>
        private void NactiHrace()
        {
            try
            {
                var conn = DatabaseManager.GetConnection();
                var cmd = new OracleCommand("SELECT * FROM HRACI_OPATRENI_VIEW", conn);
                var reader = cmd.ExecuteReader();

                HraciData.Clear();

                while (reader.Read())
                {
                    var hrac = new Hrac();

                    object rcObj = reader["RODNE_CISLO"];
                    if (rcObj != null && rcObj != DBNull.Value)
                    {
                        hrac.RodneCislo = rcObj.ToString();
                    }
                    else
                    {
                        hrac.RodneCislo = "";
                    }

                    object jmenoObj = reader["JMENO"];
                    if (jmenoObj != null && jmenoObj != DBNull.Value)
                    {
                        hrac.Jmeno = jmenoObj.ToString();
                    }
                    else
                    {
                        hrac.Jmeno = "";
                    }

                    object prijmeniObj = reader["PRIJMENI"];
                    if (prijmeniObj != null && prijmeniObj != DBNull.Value)
                    {
                        hrac.Prijmeni = prijmeniObj.ToString();
                    }
                    else
                    {
                        hrac.Prijmeni = "";
                    }

                    object telObj = reader["TELEFONNICISLO"];
                    if (telObj != null && telObj != DBNull.Value)
                    {
                        hrac.TelefonniCislo = telObj.ToString();
                    }
                    else
                    {
                        hrac.TelefonniCislo = "000000000";
                    }

                    object golyObj = reader["POCETVSTRELENYCHGOLU"];
                    if (golyObj == DBNull.Value)
                    {
                        hrac.PocetVstrelenychGolu = 0;
                    }
                    else
                    {
                        hrac.PocetVstrelenychGolu = Convert.ToInt32(golyObj);
                    }

                    object zluteObj = reader["POCET_ZLUTYCH_KARET"];
                    if (zluteObj == DBNull.Value)
                    {
                        hrac.PocetZlutychKaret = 0;
                    }
                    else
                    {
                        hrac.PocetZlutychKaret = Convert.ToInt32(zluteObj);
                    }

                    object cerveneObj = reader["POCET_CERVENYCH_KARET"];
                    if (cerveneObj == DBNull.Value)
                    {
                        hrac.PocetCervenychKaret = 0;
                    }
                    else
                    {
                        hrac.PocetCervenychKaret = Convert.ToInt32(cerveneObj);
                    }

                    object poziceObj = reader["POZICENAHRISTI"];
                    if (poziceObj != null && poziceObj != DBNull.Value)
                    {
                        hrac.PoziceNaHristi = poziceObj.ToString();
                    }
                    else
                    {
                        hrac.PoziceNaHristi = "Neznámá";
                    }

                    object delkaObj = reader["DELKATRESTU"];
                    if (delkaObj == DBNull.Value)
                    {
                        hrac.DelkaTrestu = 0;
                    }
                    else
                    {
                        hrac.DelkaTrestu = Convert.ToInt32(delkaObj);
                    }

                    object duvodObj = reader["DUVOD"];
                    if (duvodObj == DBNull.Value)
                    {
                        hrac.DuvodOpatreni = null;
                    }
                    else
                    {
                        hrac.DuvodOpatreni = duvodObj.ToString();
                    }

                    object datumObj = reader["DATUMOPATRENI"];
                    if (datumObj != DBNull.Value)
                    {
                        DateTime datum = Convert.ToDateTime(datumObj).Date;

                        if (datum == new DateTime(1900, 1, 1) || datum == DateTime.MinValue)
                        {
                            hrac.DatumOpatreniText = "Bez opatření";
                        }
                        else
                        {
                            hrac.DatumOpatreniText = datum.ToString("dd.MM.yyyy");
                        }
                    }
                    else
                    {
                        hrac.DatumOpatreniText = "Bez opatření";
                    }

                    HraciData.Add(hrac);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání hráčů:\n" + ex.Message,
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
