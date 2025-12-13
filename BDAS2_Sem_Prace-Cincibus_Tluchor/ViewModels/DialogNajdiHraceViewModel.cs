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
    /// <summary>
    /// ViewModel dialogu pro vyhledávání hráčů (filtrace podle více kritérií)
    /// </summary>
    public class DialogNajdiHraceViewModel : ViewModelBase
    {
        /// <summary>
        /// kolekce hráčů, nad kterou se provádí filtrace
        /// </summary>
        private readonly ObservableCollection<Hrac> _hraciData;

        /// <summary>
        /// Seznam pozic pro výběr v ComboBoxu
        /// </summary>
        public List<string> PoziceList { get; } = new()
        {
            "Brankář",
            "Obránce",
            "Záložník",
            "Útočník"
        };

        /// <summary>
        /// Výsledek filtrace hráčů (nastaví se po stisku Najdi)
        /// </summary>
        public IEnumerable<Hrac> VyfiltrovaniHraci { get; private set; } = Enumerable.Empty<Hrac>();

        /// <summary>
        /// Povolení filtru rodného čísla dle role uživatele
        /// </summary>
        public bool IsRodneCisloEnabled { get; private set; } = true;

        /// <summary>
        /// Opacity prvku rodného čísla (pro vizuální zablokování)
        /// </summary>
        public double RodneCisloOpacity { get; private set; } = 1.0;

        /// <summary>
        /// Tooltip pro pole rodného čísla při omezení práv
        /// </summary>
        public string RodneCisloToolTip { get; private set; } = "";

        /// <summary>
        /// Povolení filtru telefonu dle role uživatele
        /// </summary>
        public bool IsTelefonEnabled { get; private set; } = true;

        /// <summary>
        /// Opacity prvku telefonu (pro vizuální zablokování)
        /// </summary>
        public double TelefonOpacity { get; private set; } = 1.0;

        /// <summary>
        /// Tooltip pro pole telefonu při omezení práv
        /// </summary>
        public string TelefonToolTip { get; private set; } = "";

        /// <summary>
        /// Rodné číslo (filtr)
        /// </summary>
        private string _rodneCislo = "";

        /// <summary>
        /// Rodné číslo hráče pro filtraci
        /// </summary>
        public string RodneCislo { get => _rodneCislo; set { _rodneCislo = value; OnPropertyChanged(); } }

        /// <summary>
        /// Jméno (filtr)
        /// </summary>
        private string _jmeno = "";

        /// <summary>
        /// Jméno hráče pro filtraci
        /// </summary>
        public string Jmeno { get => _jmeno; set { _jmeno = value; OnPropertyChanged(); } }

        /// <summary>
        /// Příjmení (filtr)
        /// </summary>
        private string _prijmeni = "";

        /// <summary>
        /// Příjmení hráče pro filtraci
        /// </summary>
        public string Prijmeni { get => _prijmeni; set { _prijmeni = value; OnPropertyChanged(); } }

        /// <summary>
        /// Telefon (filtr)
        /// </summary>
        private string _telefon = "";

        /// <summary>
        /// Telefonní číslo hráče pro filtraci
        /// </summary>
        public string Telefon { get => _telefon; set { _telefon = value; OnPropertyChanged(); } }

        /// <summary>
        /// Goly (filtr)
        /// </summary>
        private string _goly = "";

        /// <summary>
        /// Počet vstřelených gólů pro filtraci 
        /// </summary>
        public string Goly { get => _goly; set { _goly = value; OnPropertyChanged(); } }

        /// <summary>
        /// Žluté karty (filtr)
        /// </summary>
        private string _zlute = "";

        /// <summary>
        /// Počet žlutých karet pro filtraci 
        /// </summary>
        public string Zlute { get => _zlute; set { _zlute = value; OnPropertyChanged(); } }

        /// <summary>
        /// Červené karty (filtr)
        /// </summary>
        private string _cervene = "";

        /// <summary>
        /// Počet červených karet pro filtraci 
        /// </summary>
        public string Cervene { get => _cervene; set { _cervene = value; OnPropertyChanged(); } }

        /// <summary>
        /// Vybraná pozice (filtr)
        /// </summary>
        private string? _selectedPozice = null;

        /// <summary>
        /// Vybraná pozice hráče pro filtraci
        /// </summary>
        public string? SelectedPozice { get => _selectedPozice; set { _selectedPozice = value; OnPropertyChanged(); } }

        /// <summary>
        /// Datum opatření (filtr)
        /// </summary>
        private DateTime? _datumOpatreni = null;

        /// <summary>
        /// Datum opatření pro filtraci 
        /// </summary>
        public DateTime? DatumOpatreni { get => _datumOpatreni; set { _datumOpatreni = value; OnPropertyChanged(); } }

        /// <summary>
        /// Důvod opatření (filtr)
        /// </summary>
        private string _duvod = "";

        /// <summary>
        /// Důvod opatření pro filtraci 
        /// </summary>
        public string Duvod { get => _duvod; set { _duvod = value; OnPropertyChanged(); } }

        /// <summary>
        /// Délka trestu (filtr)
        /// </summary>
        private string _delkaTrestu = "";

        /// <summary>
        /// Délka trestu pro filtraci 
        /// </summary>
        public string DelkaTrestu { get => _delkaTrestu; set { _delkaTrestu = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command pro spuštění filtrace
        /// </summary>
        public RelayCommand NajdiCommand { get; }

        /// <summary>
        /// Command pro reset filtrů
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu (true = potvrdit, false = zrušit)
        /// </summary>
        public event Action<bool>? RequestClose;

        /// <summary>
        /// Inicializuje ViewModel a nastaví role-based práva a příkazy
        /// </summary>
        /// <param name="hraciData">Kolekce hráčů pro filtrování.</param>
        public DialogNajdiHraceViewModel(ObservableCollection<Hrac> hraciData)
        {
            _hraciData = hraciData;

            ApplyRoleRights();

            NajdiCommand = new RelayCommand(_ => Najdi());
            ResetCommand = new RelayCommand(_ => Reset());
        }

        /// <summary>
        /// Nastaví dostupnost filtrů podle role přihlášeného uživatele
        /// </summary>
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

        /// <summary>
        /// Vynuluje všechny filtry do výchozího stavu
        /// </summary>
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

        /// <summary>
        /// Spustí filtrování, uloží výsledek a požádá o zavření dialogu
        /// </summary>
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

        /// <summary>
        /// Provede filtraci hráčů podle vyplněných kritérií
        /// </summary>
        /// <returns>Kolekce hráčů odpovídající filtrům.</returns>
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
                vysledek = vysl﻿edek = vysledek.Where(h => h.Prijmeni != null && h.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase));
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
