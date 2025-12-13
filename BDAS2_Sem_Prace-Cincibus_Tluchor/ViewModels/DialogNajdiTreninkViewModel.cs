using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs
{
    /// <summary>
    /// ViewModel dialogu pro vyhledávání (filtrování) tréninků podle zadaných kritérií
    /// </summary>
    public class DialogNajdiTreninkViewModel : ViewModelBase
    {
        /// <summary>
        /// Zdrojová kolekce tréninků, nad kterou se provádí filtrování
        /// </summary>
        private readonly ObservableCollection<TreninkView> _treninkyData;

        /// <summary>
        /// Výsledek filtrování tréninků 
        /// </summary>
        public IEnumerable<TreninkView> VyfiltrovaneTreninky { get; private set; }

        /// <summary>
        /// Příkaz pro spuštění filtrování
        /// </summary>
        public RelayCommand NajdiCommand { get; private set; }

        /// <summary>
        /// Příkaz pro reset všech filtrů do výchozího stavu
        /// </summary>
        public RelayCommand ResetCommand { get; private set; }

        /// <summary>
        /// Událost pro zavření dialogu z ViewModelu (true = potvrdit/OK, false = zrušit)
        /// </summary>
        public event Action<bool> RequestClose;

        /// <summary>
        /// Indikuje, zda aktuální uživatel může filtrovat podle rodného čísla (jen admin)
        /// </summary>
        private bool _canFilterRodneCislo;

        /// <summary>
        /// True pokud je uživatel admin a může filtrovat podle rodného čísla trenéra
        /// </summary>
        public bool CanFilterRodneCislo
        {
            get { return _canFilterRodneCislo; }
            private set { _canFilterRodneCislo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Tooltip pro pole rodného čísla při omezení práv (ne-admin)
        /// </summary>
        public string RodneCisloToolTip
        {
            get
            {
                if (CanFilterRodneCislo)
                {
                    return "";
                }

                return "Pouze admin může filtrovat podle rodného čísla";
            }
        }

        /// <summary>
        /// Rodné číslo trenéra pro filtrování (povoleno jen admin)
        /// </summary>
        private string _rodneCislo;

        /// <summary>
        /// Rodné číslo z formuláře (filtr)
        /// </summary>
        public string RodneCislo
        {
            get { return _rodneCislo; }
            set { _rodneCislo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Příjmení trenéra pro filtrování
        /// </summary>
        private string _prijmeni;

        /// <summary>
        /// Příjmení z formuláře (filtr)
        /// </summary>
        public string Prijmeni
        {
            get { return _prijmeni; }
            set { _prijmeni = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Datum tréninku pro filtrování
        /// </summary>
        private DateTime? _datum;

        /// <summary>
        /// Datum z formuláře (filtr)
        /// </summary>
        public DateTime? Datum
        {
            get { return _datum; }
            set { _datum = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Čas ve formátu HH:mm pro filtrování
        /// </summary>
        private string _casText;

        /// <summary>
        /// Čas z formuláře (filtr, očekává se HH:mm).
        /// </summary>
        public string CasText
        {
            get { return _casText; }
            set { _casText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Místo tréninku pro filtrování.
        /// </summary>
        private string _misto;

        /// <summary>
        /// Místo z formuláře (filtr).
        /// </summary>
        public string Misto
        {
            get { return _misto; }
            set { _misto = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Popis tréninku pro filtrování.
        /// </summary>
        private string _popis;

        /// <summary>
        /// Popis z formuláře (filtr).
        /// </summary>
        public string Popis
        {
            get { return _popis; }
            set { _popis = value; OnPropertyChanged(); }
        }

        public DialogNajdiTreninkViewModel(ObservableCollection<TreninkView> treninkyData)
        {
            if (treninkyData == null)
            {
                _treninkyData = new ObservableCollection<TreninkView>();
            }
            else
            {
                _treninkyData = treninkyData;
            }

            NastavPravaPodleRole();

            NajdiCommand = new RelayCommand(_ => Najdi());
            ResetCommand = new RelayCommand(_ => Reset());

            Reset();
        }


        public DialogNajdiTreninkViewModel()
            : this(new ObservableCollection<TreninkView>())
        {
        }

        /// <summary>
        /// Nastaví práva filtrování rodného čísla podle role přihlášeného uživatele
        /// </summary>
        private void NastavPravaPodleRole()
        {
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role = "host";
            if (uzivatel != null)
            {
                if (uzivatel.Role != null)
                {
                    role = uzivatel.Role.ToLower();
                }
            }

            if (role == "admin")
            {
                CanFilterRodneCislo = true;
            }
            else
            {
                CanFilterRodneCislo = false;
            }

            OnPropertyChanged(nameof(RodneCisloToolTip));
        }

        /// <summary>
        /// Vymaže všechny filtry do výchozího stavu.
        /// </summary>
        private void Reset()
        {
            RodneCislo = "";
            Prijmeni = "";
            Datum = null;
            CasText = "";
            Misto = "";
            Popis = "";
        }

        /// <summary>
        /// Spustí filtrování, uloží výsledek a požádá o zavření dialogu s výsledkem OK.
        /// </summary>
        private void Najdi()
        {
            try
            {
                VyfiltrovaneTreninky = FiltrujTreninky();

                if (RequestClose != null)
                {
                    RequestClose.Invoke(true);
                }
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
        /// Provede filtrování tréninků podle vyplněných kritérií ve ViewModelu.
        /// </summary>
        /// <returns>Kolekce tréninků odpovídající filtrům.</returns>
        private IEnumerable<TreninkView> FiltrujTreninky()
        {
            IEnumerable<TreninkView> vysledek = _treninkyData;

            string rodneCislo = RodneCislo;
            if (rodneCislo == null) rodneCislo = "";
            rodneCislo = rodneCislo.Trim();

            string prijmeni = Prijmeni;
            if (prijmeni == null) prijmeni = "";
            prijmeni = prijmeni.Trim();

            string casText = CasText;
            if (casText == null) casText = "";
            casText = casText.Trim();

            string misto = Misto;
            if (misto == null) misto = "";
            misto = misto.Trim();

            string popis = Popis;
            if (popis == null) popis = "";
            popis = popis.Trim();

            // Rodné číslo (jen admin)
            if (CanFilterRodneCislo)
            {
                if (!string.IsNullOrWhiteSpace(rodneCislo))
                {
                    if (rodneCislo.Length != 10)
                    {
                        throw new NonValidDataException("Rodné číslo musí mít přesně 10 číslic");
                    }

                    if (!rodneCislo.All(char.IsDigit))
                    {
                        throw new NonValidDataException("Rodné číslo může obsahovat pouze číslice");
                    }

                    vysledek = vysledek.Where(t =>
                        t.RodneCislo != null &&
                        t.RodneCislo.ToString() == rodneCislo);
                }
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                if (!prijmeni.All(char.IsLetter))
                {
                    throw new NonValidDataException("Příjmení může obsahovat pouze písmena");
                }

                vysledek = vysledek.Where(t =>
                    t.Prijmeni != null &&
                    t.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase));
            }

            // Datum
            if (Datum != null)
            {
                DateTime d = Datum.Value.Date;

                vysledek = vysledek.Where(t =>
                {
                    return t.Datum.Date == d;
                });
            }

            // Čas (HH:mm)
            if (!string.IsNullOrWhiteSpace(casText))
            {
                TimeSpan cas;
                if (!TimeSpan.TryParseExact(casText, "hh\\:mm", null, out cas))
                {
                    throw new NonValidDataException("Čas musí být ve formátu HH:mm");
                }

                vysledek = vysledek.Where(t =>
                {
                    return t.Datum.ToString("HH:mm") == casText;
                });
            }

            // Místo
            if (!string.IsNullOrWhiteSpace(misto))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Misto != null &&
                           t.Misto.Contains(misto, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Popis
            if (!string.IsNullOrWhiteSpace(popis))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Popis != null &&
                           t.Popis.Contains(popis, StringComparison.OrdinalIgnoreCase);
                });
            }

            return vysledek;
        }
    }
}
