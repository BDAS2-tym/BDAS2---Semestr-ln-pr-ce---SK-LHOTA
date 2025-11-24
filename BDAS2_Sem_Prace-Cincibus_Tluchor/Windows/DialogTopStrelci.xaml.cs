using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno zobrazující TOP 3 střelce z databázového pohledu
    /// </summary>
    public partial class DialogTopStrelci : Window
    {
        /// <summary>
        /// Inicializuje dialog a načte data do DataGridu
        /// </summary>
        public DialogTopStrelci()
        {
            InitializeComponent();
            NactiTopStrelce();
        }

        /// <summary>
        /// Načte TOP střelce z databáze a zobrazí v DataGridu
        /// </summary>
        private void NactiTopStrelce()
        {
            dgTopStrelci.ItemsSource = DatabaseHraci.GetTopStrelci();
        }

        /// <summary>
        /// Zavře dialog
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
