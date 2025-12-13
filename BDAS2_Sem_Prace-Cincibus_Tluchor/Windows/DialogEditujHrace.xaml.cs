using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialogové okno pro editaci hráče
    /// V MVVM zde zůstává pouze napojení ViewModelu a obsluha zavření dialogu
    /// </summary>
    public partial class DialogEditujHrace : Window
    {
        /// <summary>
        /// Vytvoří dialog a připraví DataContext
        /// </summary>
        /// <param name="editovanyHrac">Hráč, který se bude editovat</param>
        /// <param name="hraciOkno">Reference na okno s DataGridem, aby bylo možné po uložení provést refresh</param>
        public DialogEditujHrace(Hrac editovanyHrac, HraciOkno hraciOkno)
        {
            InitializeComponent();

            var vm = new DialogEditujHraceViewModel(
                editovanyHrac,
                requestRefreshGrid: () =>
                {
                    hraciOkno.dgHraci.Items.Refresh();
                }
            );


            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            // Napojení View na ViewModel přes DataContext kvůli bindingům v XAML
            DataContext = vm;
        }
    }
}
