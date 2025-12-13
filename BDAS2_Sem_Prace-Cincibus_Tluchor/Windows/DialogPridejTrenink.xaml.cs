using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogPridejTrenink : Window
    {
        // bezparametrický ctor kvůli Designeru
        public DialogPridejTrenink()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new DialogPridejTreninkViewModel(new ObservableCollection<TreninkView>(), true);
            }
        }

        public DialogPridejTrenink(ObservableCollection<TreninkView> treninkyData) : this()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            var vm = new DialogPridejTreninkViewModel(treninkyData, false);

            vm.RequestClose += ok =>
            {
                try
                {
                    DialogResult = ok; // jen pro ShowDialog()
                }
                catch (InvalidOperationException)
                {
                    // otevřeno přes Show() -> DialogResult nejde
                }

                Close();
            };

            DataContext = vm;
        }
    }
}
