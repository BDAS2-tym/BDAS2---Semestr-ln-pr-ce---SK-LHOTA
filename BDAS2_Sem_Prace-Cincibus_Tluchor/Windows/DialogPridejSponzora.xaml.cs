using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogPridejSponzora.xaml
    /// </summary>
    public partial class DialogPridejSponzora : Window
    {
        private ObservableCollection<Sponzor> SponzoriData;

        public DialogPridejSponzora(ObservableCollection<Sponzor> sponzoriData)
        {
            InitializeComponent();
            SponzoriData = sponzoriData;
        }

        /// <summary>
        /// Metoda vymaže textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxJmenoSponzora.Clear();
            tboxCastka.Clear();
        }

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if (String.IsNullOrWhiteSpace(tboxJmenoSponzora.Text))
            {
                throw new NonValidDataException("Sponzor nemůže být NULL ani prázdné!");
            }

            if(!long.TryParse(tboxCastka.Text, out long resultCastka))
            {
                throw new FormatException("Sponzorovaná částka není celé číslo!");
            }

            if(resultCastka < 0)
            {
                throw new NonValidDataException("Sponzorovaná částka nemůže být záporná!");
            }
        }

        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();
            }

            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
