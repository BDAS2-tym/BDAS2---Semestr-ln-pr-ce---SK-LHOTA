using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    /// <summary>
    /// View pro správu trenérů
    /// View drží jen UI logiku a napojení na ViewModel
    /// </summary>
    public partial class TreneriOkno : Window
    {
        private readonly HlavniOkno _hlavniOkno;
        private bool _zavrenoTlacitkem;

        private TreneriOknoViewModel Vm
        {
            get { return (TreneriOknoViewModel)DataContext; }
        }

        public TreneriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            _hlavniOkno = hlavniOkno;
            _zavrenoTlacitkem = false;

            TreneriOknoViewModel vm = new TreneriOknoViewModel();

            vm.RequestOpenPridejDialog += () =>
            {
                DialogPridejTrenera dialog = new DialogPridejTrenera(vm.TreneriData);
                dialog.ShowDialog();
            };

            vm.RequestOpenNajdiDialog += () =>
            {
                DialogNajdiTrenera dialog = new DialogNajdiTrenera(vm.TreneriData);
                bool? ok = dialog.ShowDialog();

                if (ok == true)
                {
                    vm.ApplyFilter(new ObservableCollection<Trener>(dialog.VyfiltrovaniTreneri));
                }
            };

            vm.RequestOpenEditDialog += (trener) =>
            {
                DialogEditujTrenera dialog = new DialogEditujTrenera(trener, this);
                dialog.ShowDialog();
            };

            vm.RequestExportTop3 += () =>
            {
                try
                {
                    Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                    dialog.Title = "Uložit TOP 3 trenéry";
                    dialog.Filter = "Textový soubor (*.txt)|*.txt";

                    bool? ok = dialog.ShowDialog();

                    if (ok == true)
                    {
                        Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
                        DatabaseTreneri.ExportTop3TreneriDoSouboru(dialog.FileName, uzivatel);

                        MessageBox.Show("TOP 3 trenéři byli úspěšně exportováni",
                            "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Chyba při exportu:\n" + ex.Message,
                        "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            vm.RequestBack += () =>
            {
                _zavrenoTlacitkem = true;
                Close();
                _hlavniOkno.Show();
                _hlavniOkno.txtPocetTreneru.Text = DatabaseTreneri.GetPocetTreneru().ToString();
            };

            DataContext = vm;

            NastavViditelnostSloupcuProUzivatele();
        }

        /// <summary>
        /// Skryje citlivé sloupce podle role
        /// Je to UI rozhodnutí, proto zůstává ve View
        /// </summary>
        private void NastavViditelnostSloupcuProUzivatele()
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

            RodneCisloSloupec.Visibility = Visibility.Visible;
            TelefonniCisloSloupec.Visibility = Visibility.Visible;

            if (role == "hrac" || role == "trener")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;
                TelefonniCisloSloupec.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_zavrenoTlacitkem == false)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// CTRL + X ukončí vyhledávací mód
        /// </summary>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Vm.JeVyhledavaniAktivni == true && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                Vm.ClearFilter();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Dvojklik otevře editaci vybraného trenéra přes příkaz z ViewModelu
        /// </summary>
        private void DgTreneri_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vm.EditCommand.Execute(null);
        }

        /// <summary>
        /// Blokuje Delete a ruší výběr na Space
        /// </summary>
        private void DgTreneri_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                MessageBox.Show("Smazání trenéra klávesou Delete není povoleno",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (e.Key == Key.Space)
            {
                dgTreneri.UnselectAll();
                dgTreneri.Focusable = false;
                Keyboard.ClearFocus();
                dgTreneri.Focusable = true;
            }
        }
    }
}
