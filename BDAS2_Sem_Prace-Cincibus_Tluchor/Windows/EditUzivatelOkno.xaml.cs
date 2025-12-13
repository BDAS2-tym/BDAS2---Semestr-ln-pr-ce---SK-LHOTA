using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Code-behind řeší jen vytvoření ViewModelu a okenní akce (Drag/Close/Minimize).
    /// </summary>
    public partial class EditUzivatelOkno : Window
    {
        public EditUzivatelOkno(Uzivatel editovanyUzivatel)
        {
            InitializeComponent();

            var vm = new EditUzivatelOknoViewModel(editovanyUzivatel);

            vm.RequestMinimize += () =>
            {
                WindowState = WindowState.Minimized;
            };

            vm.RequestClose += ok =>
            {
                DialogResult = ok;
                Close();
            };

            DataContext = vm;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
