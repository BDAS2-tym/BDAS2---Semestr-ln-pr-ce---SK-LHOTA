using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialog pro úpravu trenéra
    /// Code-behind pouze nastaví DataContext a zavře dialog podle RequestClose
    /// </summary>
    public partial class DialogEditujTrenera : Window
    {
        /// <summary>
        /// Vytvoří dialog a předá trenéra do ViewModelu
        /// </summary>
        /// <param name="trener">Vybraný trenér z DataGridu</param>
        /// <param name="okno">Okenní instance Trenéři kvůli refresh tabulky</param>
        public DialogEditujTrenera(Trener trener, TreneriOkno okno)
        {
            InitializeComponent();

            var vm = new DialogEditujTreneraViewModel(
                trener,
                requestRefreshGrid: () => okno.dgTreneri.Items.Refresh()
            );

            vm.RequestClose += okResult =>
            {
                DialogResult = okResult;
                Close();
            };

            DataContext = vm;
        }
    }
}
