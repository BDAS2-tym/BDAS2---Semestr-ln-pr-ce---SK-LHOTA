using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for DialogEditujSponzora.xaml
    /// </summary>
    public partial class DialogEditujSponzora : Window
    {
        private Sponzor editovanySponzor;
        private SponzoriOkno sponzoriOkno;
        private List<ClenKlubu> stareVazbyClenove;
        private List<Soutez> stareVazbySouteze;

        public ObservableCollection<ClenKlubu> SponzorovaniClenove { get; set; } = new ObservableCollection<ClenKlubu>();
        public ObservableCollection<Soutez> SponzorovaneSouteze { get; set; } = new ObservableCollection<Soutez>();

        public DialogEditujSponzora(Sponzor editovanySponzor, SponzoriOkno sponzoriOkno)
        {
            InitializeComponent();
            // Nastavení DataContextu
            DataContext = this;

            this.editovanySponzor = editovanySponzor;
            this.sponzoriOkno = sponzoriOkno;

            tboxJmenoSponzora.Text = editovanySponzor.Jmeno;
            tboxCastka.Text = editovanySponzor.SponzorovanaCastka.ToString();
            if (editovanySponzor.SponzorovaniClenove.Count > 0)
            {
                foreach (ClenKlubu clen in editovanySponzor.SponzorovaniClenove)
                {
                    SponzorovaniClenove.Add(clen);
                }
            }

            if (editovanySponzor.SponzorovaneSouteze.Count > 0)
            {
                foreach (Soutez soutez in editovanySponzor.SponzorovaneSouteze)
                {
                    SponzorovaneSouteze.Add(soutez);
                }
            } 
        }

        /// <summary>
        /// Metoda slouží k zavření dialogového okna
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            if (!long.TryParse(tboxCastka.Text, out long resultCastka))
            {
                throw new FormatException("Sponzorovaná částka není celé číslo!");
            }

            if (resultCastka < 0)
            {
                throw new NonValidDataException("Sponzorovaná částka nemůže být záporná!");
            }

            if (resultCastka > 0 && (SponzorovaniClenove.Count == 0 && SponzorovaneSouteze.Count == 0))
            {
                throw new NonValidDataException("Musejí být přiřazeny nějaké sponzorované soutěže nebo členové, protože je vyplněná sponzorovaná částka!");
            }

            if (resultCastka == 0 && (SponzorovaniClenove.Count > 0 || SponzorovaneSouteze.Count > 0))
            {
                throw new NonValidDataException("Sponzorovaní členové nebo soutěže nesmějí být vyplněni, protože sponzorovaná částka je 0 Kč!");
            }
        }

        /// <summary>
        /// Metoda slouží k editaci vybraného sponzora z tabulky a zároveň také v databázi
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                editovanySponzor.Jmeno = tboxJmenoSponzora.Text;
                editovanySponzor.SponzorovanaCastka = Convert.ToInt64(tboxCastka.Text);
                editovanySponzor.SponzorovaniClenove = SponzorovaniClenove.ToList();
                editovanySponzor.SponzorovaneSouteze = SponzorovaneSouteze.ToList();

                var conn = DatabaseManager.GetConnection();
                

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Odebrání všech vazeb SPONZORI_CLENOVE u určitého sponzora
                    DatabaseSponzoriClenove.OdeberVsechnyVazbySponzoriClenove(conn, editovanySponzor);

                    // Odebrání všech vazeb SPONZORI_SOUTEZE u určitého sponzora
                    DatabaseSponzoriSouteze.OdeberVsechnyVazbySponzoriSouteze(conn, editovanySponzor);

                    DatabaseSponzori.UpdateSponzor(conn, editovanySponzor);

                    // Přidání všech nově vytvořených vazeb do vazební tabulky SPONZORI_CLENOVE
                    if (editovanySponzor.SponzorovaniClenove.Count > 0)
                    {
                        foreach (ClenKlubu clen in editovanySponzor.SponzorovaniClenove)
                        {
                            DatabaseSponzoriClenove.AddSponzoriClenove(conn, clen, editovanySponzor);
                        }
                    }

                    // Přidání všech nově vytvořených vazeb do vazební tabulky SPONZORI_SOUTEZE
                    if (editovanySponzor.SponzorovaneSouteze.Count > 0)
                    {
                        foreach (Soutez soutez in editovanySponzor.SponzorovaneSouteze)
                        {
                            DatabaseSponzoriSouteze.AddSponzoriSouteze(conn, soutez, editovanySponzor);
                        }
                    }
                
                sponzoriOkno.dgSponzori.Items.Refresh();
                MessageBox.Show("Sponzor byl úspěšně editován! ", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Metoda slouží k přiřazení vazeb mezi sponzorem a členy
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEditujVazbyClenove_Click(object sender, RoutedEventArgs e)
        {
            DialogVazebniTabulkaClenove dialogVazebniTabulkaClenove;
            if (SponzorovaniClenove.Count > 0)
            {
                dialogVazebniTabulkaClenove = new DialogVazebniTabulkaClenove(SponzorovaniClenove);
            }

            else
            {
                dialogVazebniTabulkaClenove = new DialogVazebniTabulkaClenove();
            }

            bool? vysledekDiaOkna = dialogVazebniTabulkaClenove.ShowDialog();
            if (vysledekDiaOkna == true)
            {
                SponzorovaniClenove.Clear();
                foreach (ClenKlubu clen in dialogVazebniTabulkaClenove.VybraniClenove)
                {
                    SponzorovaniClenove.Add(clen);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k přiřazení vazeb mezi sponzorem a soutěžemi
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEditujVazbySouteze_Click(object sender, RoutedEventArgs e)
        {
            DialogVazebniTabulkaSouteze dialogVazebniTabulkaSouteze;
            if (SponzorovaniClenove.Count > 0)
            {
                dialogVazebniTabulkaSouteze = new DialogVazebniTabulkaSouteze(SponzorovaneSouteze);
            }

            else
            {
                dialogVazebniTabulkaSouteze = new DialogVazebniTabulkaSouteze();
            }

            bool? vysledekDiaOkna = dialogVazebniTabulkaSouteze.ShowDialog();
            if (vysledekDiaOkna == true)
            {
                SponzorovaneSouteze.Clear();
                foreach (Soutez soutez in dialogVazebniTabulkaSouteze.VybraneSouteze)
                {
                    SponzorovaneSouteze.Add(soutez);
                }
            }
        }
    }
}