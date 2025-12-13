using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class HraciOkno : Window
    {
        private readonly HlavniOkno _hlavniOkno;
        private bool _zavrenoTlacitkem = false;

        private HraciOknoViewModel Vm => (HraciOknoViewModel)DataContext;

        public HraciOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            _hlavniOkno = hlavniOkno;

            var vm = new HraciOknoViewModel();
            vm.RequestOpenPridejDialog += () =>
            {
                var dialog = new DialogPridejHrace(vm.HraciData);
                dialog.ShowDialog();
            };

            vm.RequestOpenNajdiDialog += () =>
            {
                var dialog = new DialogNajdiHrace(vm.HraciData);
                bool? ok = dialog.ShowDialog();
                if (ok == true)
                {
                    vm.ApplyFilter(new ObservableCollection<Hrac>(dialog.VyfiltrovaniHraci));
                }
            };

            vm.RequestOpenTopStrelciDialog += () =>
            {
                new DialogTopStrelci().ShowDialog();
            };

            vm.RequestOpenEditDialog += (hrac) =>
            {
                var dialog = new DialogEditujHrace(hrac, this);
                dialog.ShowDialog();
            };

            vm.RequestBack += () =>
            {
                _zavrenoTlacitkem = true;
                Close();
                _hlavniOkno.Show();
                _hlavniOkno.txtPocetHracu.Text = DatabaseHraci.GetPocetHracu().ToString();
            };

            DataContext = vm;

            NastavViditelnostSloupcuProUzivatele();
        }

        private void NastavViditelnostSloupcuProUzivatele()
        {
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = uzivatel.Role.ToLower();

            RodneCisloSloupec.Visibility = Visibility.Visible;
            TelefonniCisloSloupec.Visibility = Visibility.Visible;

            if (role == "hrac" || role == "trener" || role == "host")
            {
                RodneCisloSloupec.Visibility = Visibility.Collapsed;
                TelefonniCisloSloupec.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_zavrenoTlacitkem == false)
                Application.Current.Shutdown();
        }

        // CTRL + X ukončí hledání 
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Vm.JeVyhledavaniAktivni && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                Vm.ClearFilter();
                e.Handled = true;
            }
        }

        // doubleclick = edit (Command)
        private void DgHraci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vm.EditCommand.Execute(null);
        }

        // blok DELETE 
        private void DgHraci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                MessageBox.Show("Smazání hráče klávesou Delete není povoleno",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (e.Key == Key.Space)
            {
                dgHraci.UnselectAll();
                dgHraci.Focusable = false;
                Keyboard.ClearFocus();
                dgHraci.Focusable = true;
            }
        }
    }
}
