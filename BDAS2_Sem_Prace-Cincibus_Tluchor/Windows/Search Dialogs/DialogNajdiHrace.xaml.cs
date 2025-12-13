using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    public partial class DialogNajdiHrace : Window
    {
        public IEnumerable<Hrac> VyfiltrovaniHraci
            => ((DialogNajdiHraceViewModel)DataContext).VyfiltrovaniHraci;

        public DialogNajdiHrace(ObservableCollection<Hrac> hraciData)
        {
            InitializeComponent();

            var vm = new DialogNajdiHraceViewModel(hraciData);
            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
