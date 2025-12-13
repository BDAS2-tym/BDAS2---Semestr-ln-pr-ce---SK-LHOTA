using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    public class DialogNajdiHraceViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Hrac> _hraciData;

        public List<string> PoziceList { get; } = new()
        {
            "Brankář",
            "Obránce",
            "Záložník",
            "Útočník"
        };

        // výsledek filtru 
        public IEnumerable<Hrac> VyfiltrovaniHraci { get; private set; } = Enumerable.Empty<Hrac>();

        // role-based práva
        public bool IsRodneCisloEnabled { get; private set; } = true;
        public double RodneCisloOpacity { get; private set; } = 1.0;
        public string RodneCisloToolTip { get; private set; } = "";

        public bool IsTelefonEnabled { get; private set; } = true;
        public double TelefonOpacity { get; private set; } = 1.0;
        public string TelefonToolTip { get; private set; } = "";

        // hodnoty z formuláře
        private string _rodneCislo = "";
        public string RodneCislo { get => _rodneCislo; set { _rodneCislo = value; OnPropertyChanged(); } }

        private string _jmeno = "";
        public string Jmeno { get => _jmeno; set { _jmeno = value; OnPropertyChanged(); } }

        private string _prijmeni = "";
        public string Prijmeni { get => _prijmeni; set { _prijmeni = value; OnPropertyChanged(); } }

        private string _telefon = "";
        public string Telefon { get => _telefon; set { _telefon = value; OnPropertyChanged(); } }

        private string _goly = "";
        public string Goly { get => _goly; set { _goly = value; OnPropertyChanged(); } }

        private string _zlute = "";
        public string Zlute { get => _zlute; set { _zlute = value; OnPropertyChanged(); } }

        private string _cervene = "";
        public string Cervene { get => _cervene; set { _cervene = value; OnPropertyChanged(); } }

        private string? _selectedPozice = null;
        public string? SelectedPozice { get => _selectedPozice; set { _selectedPozice = value; OnPropertyChanged(); } }

        private DateTime? _datumOpatreni = null;
        public DateTime? DatumOpatreni { get => _datumOpatreni; set { _datumOpatreni = value; OnPropertyChanged(); } }

        private string _duvod = "";
        public string Duvod { get => _duvod; set { _duvod = value; OnPropertyChanged(); } }

        private string _delkaTrestu = "";
        public string DelkaTrestu { get => _delkaTrestu; set { _delkaTrestu = value; OnPropertyChanged(); } }

        // Commands
        public RelayCommand NajdiCommand { get; }
        public RelayCommand ResetCommand { get; }

        // zavření dialogu z VM
        public event Action<bool>? RequestClose;

        public DialogNajdiHraceViewModel(ObservableCollection<Hrac> hraciData)
        {
            _hraciData = hraciData;

            ApplyRoleRights();

            NajdiCommand = new RelayCommand(_ => Najdi());
            ResetCommand = new RelayCommand(_ => Reset());

        }

        private void ApplyRoleRights()
        {
            Uzivatel? uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = (uzivatel?.Role ?? "host").ToLower();

            if (role != "admin")
            {
                IsRodneCisloEnabled = false;
                RodneCisloOpacity = 0.5;
                RodneCisloToolTip = "Pouze admin může filtrovat podle rodného čísla";

                IsTelefonEnabled = false;
                TelefonOpacity = 0.5;
                TelefonToolTip = "Pouze admin může filtrovat podle telefonního čísla";
            }

            OnPropertyChanged(nameof(IsRodneCisloEnabled));
            OnPropertyChanged(nameof(RodneCisloOpacity));
            OnPropertyChanged(nameof(RodneCisloToolTip));

            OnPropertyChanged(nameof(IsTelefonEnabled));
            OnPropertyChanged(nameof(TelefonOpacity));
            OnPropertyChanged(nameof(TelefonToolTip));
        }

        private void Reset()
        {
            SelectedPozice = null;
            RodneCislo = "";
            Jmeno = "";
            Prijmeni = "";
            Telefon = "";
            Goly = "";
            Zlute = "";
            Cervene = "";
            DatumOpatreni = null;
            Duvod = "";
            DelkaTrestu = "";
        }

        private void Najdi()
        {
            try
            {
                VyfiltrovaniHraci = FiltrujHrace();
                RequestClose?.Invoke(true);
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

        private IEnumerable<Hrac> FiltrujHrace()
        {
            IEnumerable<Hrac> vysledek = _hraciData;

            string jmeno = (Jmeno ?? "").Trim();
            string prijmeni = (Prijmeni ?? "").Trim();
            string rodneCislo = (RodneCislo ?? "").Trim();
            string telefonCislo = (Telefon ?? "").Trim();

            // Rodné číslo
            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                Validator.ValidujRodneCislo(rodneCislo);
                vysledek = vysledek.Where(h => h.RodneCislo != null && h.RodneCislo == rodneCislo);
            }

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                Validator.ValidujJmeno(jmeno);
                vysledek = vysledek.Where(h => h.Jmeno != null && h.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase));
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                Validator.ValidujPrijmeni(prijmeni);
                vysledek = vysledek.Where(h => h.Prijmeni != null && h.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase));
            }

            // Telefonní číslo
            if (!string.IsNullOrWhiteSpace(telefonCislo))
            {
                Validator.ValidujTelefon(telefonCislo, 1, 12);
                vysledek = vysledek.Where(h => h.TelefonniCislo != null && h.TelefonniCislo.Contains(telefonCislo));
            }

            // Počet gólů
            if (!string.IsNullOrWhiteSpace(Goly))
            {
                Validator.ValidujCeleCislo(Goly, "Počet gólů");
                int goly = int.Parse(Goly);
                vysledek = vysledek.Where(h => h.PocetVstrelenychGolu == goly);
            }

            // Žluté karty
            if (!string.IsNullOrWhiteSpace(Zlute))
            {
                Validator.ValidujCeleCislo(Zlute, "Počet žlutých karet");
                int zlute = int.Parse(Zlute);
                vysledek = vysledek.Where(h => h.PocetZlutychKaret == zlute);
            }

            // Červené karty
            if (!string.IsNullOrWhiteSpace(Cervene))
            {
                Validator.ValidujCeleCislo(Cervene, "Počet červených karet");
                int cervene = int.Parse(Cervene);
                vysledek = vysledek.Where(h => h.PocetCervenychKaret == cervene);
            }

            // Pozice
            if (!string.IsNullOrWhiteSpace(SelectedPozice))
            {
                string poz = SelectedPozice!;
                vysledek = vysledek.Where(h => h.PoziceNaHristi == poz);
            }

            // Datum opatření
            if (DatumOpatreni != null)
            {
                Validator.ValidujDatum(DatumOpatreni, "Datum opatření");
                string datumVybrane = DatumOpatreni.Value.ToString("dd.MM.yyyy");

                vysledek = vysledek.Where(h =>
                    h.DatumOpatreniText != null &&
                    h.DatumOpatreniText != "Bez opatření" &&
                    h.DatumOpatreniText == datumVybrane);
            }

            // Důvod opatření
            if (!string.IsNullOrWhiteSpace(Duvod))
            {
                string duvod = Duvod.Trim();
                vysledek = vysledek.Where(h => h.DuvodOpatreni != null &&
                                               h.DuvodOpatreni.Contains(duvod, StringComparison.OrdinalIgnoreCase));
            }

            // Délka trestu
            if (!string.IsNullOrWhiteSpace(DelkaTrestu))
            {
                Validator.ValidujCeleCislo(DelkaTrestu, "Délka trestu");
                int delka = int.Parse(DelkaTrestu);
                vysledek = vysledek.Where(h => h.DelkaTrestu == delka);
            }

            return vysledek;
        }
    }
}
