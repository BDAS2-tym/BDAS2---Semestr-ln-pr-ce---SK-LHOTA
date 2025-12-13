using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogEditujTrenink : Window
    {
        public DialogEditujTrenink(TreninkView trenink, TreninkyOkno treninkyOkno)
        {
            InitializeComponent();

            Action refresh;
            if (treninkyOkno != null)
            {
                refresh = () => { treninkyOkno.dgTreninky.Items.Refresh(); };
            }
            else
            {
                refresh = () => { };
            }

            var vm = new DialogEditujTreninkViewModel(trenink, refresh);

            vm.RequestClose += ok =>
            {
                try
                {
                    DialogResult = ok;
                }
                catch (InvalidOperationException)
                {
   
                }

                Close();
            };

            DataContext = vm;
        }
    }
}
