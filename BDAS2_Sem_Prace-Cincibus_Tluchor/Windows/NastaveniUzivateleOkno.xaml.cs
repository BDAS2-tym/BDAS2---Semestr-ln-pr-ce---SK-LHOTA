using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno správy uživatelských účtů.
    /// - vytvoření a napojení ViewModelu
    /// </summary>
    public partial class NastaveniUzivateleOkno : Window
    {
        /// <summary>
        /// Reference na hlavní okno aplikace 
        /// </summary>
        private readonly HlavniOkno _hlavniOkno;

        /// <summary>
        /// Indikuje, zda bylo okno ukončeno řízeně tlačítkem/navigací
        /// Pokud ne, zavření přes X ukončí celou aplikaci
        /// </summary>
        private bool _zavrenoTlacitkem;

        /// <summary>
        /// Konstruktor okna
        /// Vytvoří ViewModel, napojí eventy a nastaví DataContext
        /// </summary>
        /// <param name="hlavniOkno">Aktuální instance hlavního okna.</param>
        public NastaveniUzivateleOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            _hlavniOkno = hlavniOkno;
            _zavrenoTlacitkem = false;

            var vm = new NastaveniUzivateleOknoViewModel(_hlavniOkno);

            /// <summary>
            /// Požadavek na otevření registrace (přidání uživatele)
            /// Po zavření registrace se znovu načtou data do gridu
            /// </summary>
            vm.RequestOpenRegistrace += () =>
            {
                RegistraceOkno registraceOkno = new RegistraceOkno();
                registraceOkno.ShowDialog();
                vm.NactiUzivatele();
            };

            /// <summary>
            /// Požadavek na otevření dialogu editace uživatele
            /// Po úspěšném uložení se znovu načtou data a zobrazí se informace
            /// </summary>
            vm.RequestOpenEditUzivatel += (u) =>
            {
                EditUzivatelOkno okno = new EditUzivatelOkno(u);
                bool? vysledek = okno.ShowDialog();

                if (vysledek == true)
                {
                    vm.NactiUzivatele();
                    MessageBox.Show("Změny byly úspěšně uloženy.", "Aktualizace",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            /// <summary>
            /// Požadavek na otevření dialogu vyhledávání uživatelských účtů.
            /// Pokud jsou nalezeny výsledky, přepne se okno do vyhledávacího módu
            /// (data v gridu se nahradí vyfiltrovanou kolekcí)
            /// </summary>
            vm.RequestOpenNajdiDialog += () =>
            {
                DialogNajdiUzivatelskeUcty dialog = new DialogNajdiUzivatelskeUcty(vm.UzivateleData);
                bool? vysledek = dialog.ShowDialog();

                if (vysledek == true)
                {
                    if (!dialog.VyfiltrovaniUzivatele.Any())
                    {
                        MessageBox.Show("Nenašly se žádné záznamy",
                            "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBox.Show("Vyhledávací mód je aktivní. Pro návrat stiskněte CTRL + X",
                        "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    vm.ApplySearchResults(new ObservableCollection<Uzivatel>(dialog.VyfiltrovaniUzivatele));
                }
            };

            /// <summary>
            /// Požadavek na otevření dialogu pro notifikace / zprávu
            /// Dialog pracuje nad aktuální kolekcí uživatelů
            /// </summary>
            vm.RequestOpenNotifikace += () =>
            {
                DialogZpravaOkno okno = new DialogZpravaOkno(vm.UzivateleData);
                okno.ShowDialog();
            };

            /// <summary>
            /// Požadavek na návrat do okna Nastavení
            /// Používá se Instance, aby se šlo vracet mezi podokny
            /// </summary>
            vm.RequestBack += () =>
            {
                _zavrenoTlacitkem = true;
                NastaveniOkno.Instance.Show();
                Hide();
            };

            /// <summary>
            /// Požadavek navrat do hlavního okna (pro emulaci/přepnutí účtu)
            /// Nové hlavní okno se otevře a původní se zavře
            /// </summary>
            vm.RequestRestartMain += () =>
            {
                HlavniOkno noveOkno = new HlavniOkno();
                noveOkno.Show();

                if (_hlavniOkno != null)
                {
                    _hlavniOkno.Close();
                }
            };

            /// <summary>
            /// Požadavek na zavření tohoto okna (řízené ukončení)
            /// </summary>
            vm.RequestCloseWindow += () =>
            {
                _zavrenoTlacitkem = true;
                Close();
            };

            // Napojení VM na View
            DataContext = vm;

            /// <summary>
            /// Po načtení okna nastavíme viditelnost sloupců v DataGridu.
            /// </summary>
            Loaded += (_, __) =>
            {
                ApplyColumnVisibility(vm);
            };
        }

        /// <summary>
        /// Nastaví viditelnost sloupců DataGridu podle práv uživatele (stav ve ViewModelu)
        /// Typicky:
        /// - citlivé sloupce (Rodné číslo) se skryjí pro ne-admin role
        /// - sloupec Akce (Emulovat) se může skrýt pro role bez oprávnění
        /// </summary>
        private void ApplyColumnVisibility(NastaveniUzivateleOknoViewModel vm)
        {
            // Default: vše viditelné
            foreach (var sloupec in dgUzivatele.Columns)
            {
                sloupec.Visibility = Visibility.Visible;
            }

            if (vm.ShowSensitiveColumns && vm.ShowAkceColumn)
            {
                return;
            }

            foreach (var sloupec in dgUzivatele.Columns)
            {
                if (sloupec.Header != null)
                {
                    string header = sloupec.Header.ToString().ToLower();

                    // Skrytí citlivých údajů (rodné číslo)
                    if (!vm.ShowSensitiveColumns)
                    {
                        if (header.Contains("rodné") || header.Contains("rodne"))
                        {
                            sloupec.Visibility = Visibility.Collapsed;
                        }
                    }

                    // Skrytí akčního sloupce (Emulovat / Akce)
                    if (!vm.ShowAkceColumn)
                    {
                        if (header.Contains("akce"))
                        {
                            sloupec.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Zavření okna
        /// Pokud nebylo zavřeno řízeně, zavření přes X ukončí celou aplikaci
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_zavrenoTlacitkem)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Klávesové zkratky a blokace nechtěných akcí v DataGridu
        /// - DELETE: zakázat mazání
        /// - SPACE: odznačit řádek a odstranit focus
        /// </summary>
        private void DgUzivatele_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                MessageBox.Show("Smazání uživatele klávesou Delete není povoleno",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (e.Key == Key.Space)
            {
                dgUzivatele.UnselectAll();
                dgUzivatele.Focusable = false;
                Keyboard.ClearFocus();
                dgUzivatele.Focusable = true;
            }
        }
    }
}
