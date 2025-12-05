using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogEditujUzivatele.xaml
    /// </summary>
    public partial class DialogEditujUzivatele : Window
    {
        public DialogEditujUzivatele()
        {
            InitializeComponent();
            dtpPosledniPrihlaseni.Background = new SolidColorBrush(Colors.LightGray);
            dtpPosledniPrihlaseni.IsEnabled = false;
        }

        /// <summary>
        /// Metoda vymaže textová pole a ComboBoxy
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxUzivJmeno.Clear();
            tboxHeslo.Clear();
            cbOpravneni.SelectedItem = null;
        }

        /// <summary>
        /// Metoda upraví vybraného uživatele
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();
            }

            catch(NonValidDataException ex) 
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if (String.IsNullOrWhiteSpace(tboxUzivJmeno.Text))
            {
                throw new NonValidDataException("Uživatelské jméno nemůže být NULL ani prázdné!");
            }

            if(tboxHeslo is not null)
            {
                // Kontrola, zda heslo obsahuje alespoň jedno velké písmeno
                if (!tboxHeslo.Text.Any(Char.IsUpper))
                {
                    throw new NonValidDataException("V heslu chybí alespoň jedno velké písmeno!");
                }

                // Kontrola, zda heslo obsahuje alespoň jedno číslo
                if (!tboxHeslo.Text.Any(Char.IsNumber))
                {
                    throw new NonValidDataException("V heslu chybí alespoň jedno číslo!");
                }
            }

            if(cbOpravneni.SelectedItem is null)
            {
                throw new NonValidDataException("Není vybráno žádné oprávnění/role!");
            }
        }

        /// <summary>
        /// Načtení datumu z databáze do dtpPosledniPrihlaseni
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dtpPosledniPrihlaseni_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {

                dtpPosledniPrihlaseni.Value = DateTime.Now;

                dtpPosledniPrihlaseni.InvalidateVisual();
            }), DispatcherPriority.ContextIdle);
        }
    }
}
