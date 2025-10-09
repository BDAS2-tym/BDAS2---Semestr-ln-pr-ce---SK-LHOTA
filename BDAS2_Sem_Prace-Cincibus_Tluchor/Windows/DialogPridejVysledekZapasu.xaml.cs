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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogPridejVysledekZapasu.xaml
    /// </summary>
    public partial class DialogPridejVysledekZapasu : Window
    {
        private const string HintText = "např. 3:2";
        public DialogPridejVysledekZapasu()
        {
            InitializeComponent();
            tboxVysledek.Text = HintText;
            tboxVysledek.Foreground = Brushes.Gray;

            /* Vlastní čárkovaná čára pod textem */
            var underline = new TextDecoration
            {
                Location = TextDecorationLocation.Underline,
                Pen = new Pen(Brushes.DimGray, 1)
                {
                    DashStyle = DashStyles.Dash
                },
                PenThicknessUnit = TextDecorationUnit.FontRecommended
            };

            tblockVysledek.TextDecorations = new TextDecorationCollection { underline };
        }

        /// <summary>
        /// Metoda slouží k odebrání hint textu a zapnutí normálního textu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void tboxVysledek_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tboxVysledek.Text == HintText)
            {
                tboxVysledek.Text = "";
                tboxVysledek.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Metoda slouží k přidání hint textu při ztracení focusu na textovém poli
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void tboxVysledek_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxVysledek.Text))
            {
                tboxVysledek.Text = HintText;
                tboxVysledek.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Metoda vyresetuje textová pole a IntegerUpDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxVysledek.Clear();
            tboxVysledek.Text = HintText;
            tboxVysledek.Foreground = Brushes.Gray;
            iudPocetCervenychKaret.Value = iudPocetZlutychKaret.Value = iudPocetGolyDomaci.Value = iudPocetGolyHoste.Value = 0;
        }

    }
}
