using BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Code-behind obsahuje jen obsluhu "okenních" akcí (drag/minimize/close)
    /// a předání hesel z PasswordBox do ViewModelu
    /// </summary>
    public partial class RegistraceOkno : Window
    {
        public RegistraceOkno()
        {
            InitializeComponent();

            var vm = new RegistraceOknoViewModel();

            vm.RequestClose += () => Close();
            vm.RequestMinimize += () => WindowState = WindowState.Minimized;

            DataContext = vm;
        }

        private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as RegistraceOknoViewModel;
            if (vm == null) return;

            vm.Heslo = txtPass.Password;
        }

        private void TxtPass2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as RegistraceOknoViewModel;
            if (vm == null) return;

            vm.Heslo2 = txtPass2.Password;
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
