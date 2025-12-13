using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogPridejHrace : Window
    {
        public DialogPridejHrace(ObservableCollection<Hrac> hraciData)
        {
            InitializeComponent();

            var vm = new DialogPridejHraceViewModel(hraciData);
            vm.RequestClose += (ok) =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
