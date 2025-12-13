using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    public partial class DialogNajdiTrenera : Window
    {
        public IEnumerable<Trener> VyfiltrovaniTreneri
        {
            get
            {
                DialogNajdiTreneraViewModel vm = DataContext as DialogNajdiTreneraViewModel;
                if (vm == null)
                {
                    return new List<Trener>();
                }

                if (vm.VyfiltrovaniTreneri == null)
                {
                    return new List<Trener>();
                }

                return vm.VyfiltrovaniTreneri;
            }
        }

     
        public DialogNajdiTrenera()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new DialogNajdiTreneraViewModel();
            }
        }

        public DialogNajdiTrenera(ObservableCollection<Trener> treneriData) : this()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            DialogNajdiTreneraViewModel vm = new DialogNajdiTreneraViewModel(treneriData);
            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
