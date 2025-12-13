using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels.Search_Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Code-behind řeší pouze inicializaci ViewModelu a zavření dialogu.
    /// </summary>
    public partial class DialogNajdiUzivatelskeUcty : Window
    {
        /// <summary>
        /// Výsledek filtrování
        /// </summary>
        public IEnumerable<Uzivatel> VyfiltrovaniUzivatele
        {
            get
            {
                DialogNajdiUzivatelskeUctyViewModel vm = DataContext as DialogNajdiUzivatelskeUctyViewModel;
                if (vm == null)
                {
                    return new List<Uzivatel>();
                }
                return vm.VyfiltrovaniUzivatele;
            }
        }

        public DialogNajdiUzivatelskeUcty(ObservableCollection<Uzivatel> uzivatele)
        {
            InitializeComponent();

            DialogNajdiUzivatelskeUctyViewModel vm = new DialogNajdiUzivatelskeUctyViewModel(uzivatele);

            vm.RequestClose += (ok) =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
