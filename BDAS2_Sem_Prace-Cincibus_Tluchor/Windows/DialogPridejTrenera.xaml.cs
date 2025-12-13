using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno dialogu pro přidání trenéra
    /// Code-behind pouze nastaví DataContext a reaguje na požadavek na zavření
    /// </summary>
    public partial class DialogPridejTrenera : Window
    {
        /// <summary>
        /// Vytvoří dialog a předá kolekci trenérů do ViewModelu
        /// </summary>
        /// <param name="treneriData">Kolekce pro DataGrid v okně Trenéři</param>
        public DialogPridejTrenera(ObservableCollection<Trener> treneriData)
        {
            InitializeComponent();

            var vm = new DialogPridejTreneraViewModel(treneriData);

            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }
    }
}
