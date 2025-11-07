using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public VysledekZapasu PridavanyVysledek { get; set; } = new VysledekZapasu();

        private Zapas pridavanyZapas;
        private const string HintText = "např. 3:2";

        public DialogPridejVysledekZapasu(Zapas pridavanyZapas)
        {
            InitializeComponent();

            this.pridavanyZapas = pridavanyZapas;
            tboxDomaciTym.Text = pridavanyZapas.DomaciTym;
            tboxHosteTym.Text = pridavanyZapas.HosteTym;

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

            tboxVysledek.Text = pridavanyZapas.Vysledek;
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

        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                PridavanyVysledek.Vysledek = tboxVysledek.Text;
                PridavanyVysledek.PocetGolyDomaci = Convert.ToInt32(iudPocetGolyDomaci.Value);
                PridavanyVysledek.PocetGolyHoste = Convert.ToInt32(iudPocetGolyHoste.Value);
                PridavanyVysledek.PocetZlutychKaret = Convert.ToInt32(iudPocetZlutychKaret.Value);
                PridavanyVysledek.PocetCervenychKaret = Convert.ToInt32(iudPocetCervenychKaret.Value);

                this.DialogResult = true;
                this.Close();
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

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if (String.IsNullOrEmpty(tboxVysledek.Text))
            {
                throw new NonValidDataException("Výsledek nemůže být prázdný ani NULL!");
            }

            if(!Regex.IsMatch(tboxVysledek.Text, @"^[0-9]{1,2}:[0-9]{1,2}$"))
            {
                throw new NonValidDataException("Výsledek není ve validním formátu!");
            }

            // Rozdělení výsledku na části góly Domácí a Hosté
            string[] casti = tboxVysledek.Text.Split(':');

            int golyDomaci = int.Parse(casti[0]);
            int golyHoste = int.Parse(casti[1]);

            if (iudPocetGolyDomaci.Value != golyDomaci)
            {
                throw new NonValidDataException("Góly se musí shodovat ve výsledku a v počtu gólů domácích!");
            }

            if (iudPocetGolyHoste.Value != golyHoste)
            {
                throw new NonValidDataException("Góly se musí shodovat ve výsledku a v počtu gólů hosté!");
            }

            if(iudPocetZlutychKaret.Value < 0 || iudPocetCervenychKaret.Value < 0)
            {
                throw new NonValidDataException("Počet žlutých ani červených karet nemůže být záporný!");
            }

            if (String.IsNullOrWhiteSpace(tboxHosteTym.Text) || String.IsNullOrWhiteSpace(tboxHosteTym.Text))
            {
                throw new NonValidDataException("Domací tým ani tým hostů nemůže být prázdný ani NULL!");
            }

            if (String.Equals(tboxDomaciTym.Text, tboxHosteTym.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NonValidDataException("Domácí tým nemůže být stejný jako tým hostů!");
            }
        }
    }
}
