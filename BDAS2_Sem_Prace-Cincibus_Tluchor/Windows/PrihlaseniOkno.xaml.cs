using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class PrihlaseniOkno : Window
    {

        public PrihlaseniOkno()
        {
            InitializeComponent();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string uzivatel = txtUser.Text;
            string heslo = txtPass.Password;

            if (uzivatel == "admin" && heslo == "admin")
            {
                MessageBox.Show("Přihlášení bylo úspěšné!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                // Otevře hlavní okno a zavře přihlašovací
                HlavniOkno hlavni = new HlavniOkno();
                hlavni.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Neplatné přihlašovací údaje!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
