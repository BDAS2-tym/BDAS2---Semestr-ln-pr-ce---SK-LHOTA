using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Code-behind řeší pouze vytvoření ViewModelu a uzavření okna na požadavek z VM.
    /// </summary>
    public partial class DialogZpravaOkno : Window
    {
        public DialogZpravaOkno(IEnumerable<Uzivatel> uzivatele)
        {
            InitializeComponent();

            DialogZpravaOknoViewModel vm = new DialogZpravaOknoViewModel(uzivatele);

            vm.RequestClose += (ok) =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
