using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    public partial class DialogNajdiTrenink : Window
    {
        public IEnumerable<TreninkView> VyfiltrovaneTreninky
        {
            get
            {
                DialogNajdiTreninkViewModel vm = DataContext as DialogNajdiTreninkViewModel;
                if (vm == null)
                {
                    return new List<TreninkView>();
                }

                if (vm.VyfiltrovaneTreninky == null)
                {
                    return new List<TreninkView>();
                }

                return vm.VyfiltrovaneTreninky;
            }
        }

        // kvůli designeru
        public DialogNajdiTrenink()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new DialogNajdiTreninkViewModel();
            }
        }

        public DialogNajdiTrenink(ObservableCollection<TreninkView> treninkyData) : this()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            DialogNajdiTreninkViewModel vm = new DialogNajdiTreninkViewModel(treninkyData);

            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
