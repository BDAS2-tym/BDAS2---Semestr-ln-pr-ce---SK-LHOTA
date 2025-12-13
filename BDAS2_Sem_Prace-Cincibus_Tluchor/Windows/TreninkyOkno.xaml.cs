using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno tréninků
    /// Code-behind řeší jen otevření dialogů a pár klávesových věcí pro DataGrid
    /// </summary>
    public partial class TreninkyOkno : Window
    {
        private readonly HlavniOkno _hlavniOkno;
        private bool _zavrenoTlacitkem;

        /// <summary>
        /// Vytvoří okno a napojí ViewModel
        /// </summary>
        public TreninkyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            _hlavniOkno = hlavniOkno;
            _zavrenoTlacitkem = false;

            TreninkyOknoViewModel vm = new TreninkyOknoViewModel();

            vm.RequestBack += Vm_RequestBack;
            vm.RequestOpenNajdiDialog += Vm_RequestOpenNajdiDialog;
            vm.RequestOpenPridejDialog += Vm_RequestOpenPridejDialog;
            vm.RequestOpenEditDialog += Vm_RequestOpenEditDialog;
            vm.RequestOpenStatistikyDialog += Vm_RequestOpenStatistikyDialog;

            DataContext = vm;
        }

        private TreninkyOknoViewModel GetVm()
        {
            return DataContext as TreninkyOknoViewModel;
        }

        private void Vm_RequestBack()
        {
            _zavrenoTlacitkem = true;
            Close();
            _hlavniOkno.Show();
        }

        private void Vm_RequestOpenNajdiDialog()
        {
            TreninkyOknoViewModel vm = GetVm();
            if (vm == null)
            {
                return;
            }
            else
            {
            }

            DialogNajdiTrenink dialog = new DialogNajdiTrenink(vm.TreninkyData);
            bool? vysledek = dialog.ShowDialog();

            if (vysledek == true)
            {
                ObservableCollection<TreninkView> filtrovane = new ObservableCollection<TreninkView>(dialog.VyfiltrovaneTreninky);
                vm.ApplyFilter(filtrovane);
            }
            else
            {
            }
        }

        private void Vm_RequestOpenPridejDialog()
        {
            TreninkyOknoViewModel vm = GetVm();
            if (vm == null)
            {
                return;
            }

            DialogPridejTrenink dialog = new DialogPridejTrenink(vm.TreninkyData);
            dialog.ShowDialog();
        }


        private void Vm_RequestOpenEditDialog(TreninkView trenink)
        {
            DialogEditujTrenink dialog = new DialogEditujTrenink(trenink, this);
            dialog.ShowDialog();
        }

        private void Vm_RequestOpenStatistikyDialog()
        {
            DialogStatistikyTreninku dialog = new DialogStatistikyTreninku();
            dialog.ShowDialog();
        }

        /// <summary>
        /// Ukončí aplikaci při zavření přes X
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_zavrenoTlacitkem == false)
            {
                Application.Current.Shutdown();
            }
            else
            {
            }
        }

        /// <summary>
        /// Dvojklik na řádek spustí editaci přes ViewModel
        /// </summary>
        private void DgTreninky_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreninkyOknoViewModel vm = GetVm();
            if (vm == null)
            {
                return;
            }
            else
            {
            }

            if (vm.EditCommand != null)
            {
                vm.EditCommand.Execute(null);
            }
            else
            {
            }
        }

        /// <summary>
        /// Zablokuje Delete a řeší odznačení při Space
        /// </summary>
        private void DgTreninky_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;

                MessageBox.Show("Smazání tréninku klávesou Delete není povoleno",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
            }

            if (e.Key == Key.Space)
            {
                dgTreninky.UnselectAll();

                dgTreninky.Focusable = false;
                Keyboard.ClearFocus();
                dgTreninky.Focusable = true;
            }
            else
            {
            }
        }
    }
}
