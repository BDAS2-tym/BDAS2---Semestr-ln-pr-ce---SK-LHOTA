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
    /// Interaction logic for DialogPridejSponzora.xaml
    /// </summary>
    public partial class DialogPridejSponzora : Window
    {
        private ObservableCollection<Sponzor> sponzoriData;
        public ObservableCollection<ClenKlubu> SponzorovaniClenove { get; set; }
        public ObservableCollection<Soutez> SponzorovaneSouteze { get; set; }

        public DialogPridejSponzora(ObservableCollection<Sponzor> sponzoriData)
        {
            InitializeComponent();
            SponzorovaniClenove = new ObservableCollection<ClenKlubu>();
            SponzorovaneSouteze = new ObservableCollection<Soutez>();
            this.sponzoriData = sponzoriData;

            // Nastavení DataContextu
            DataContext = this;
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

            if(resultCastka > 0 && (SponzorovaniClenove.Count == 0 && SponzorovaneSouteze.Count == 0))
            {
                throw new NonValidDataException("Musejí být přiřazeny nějaké sponzorované soutěže nebo členové, protože je vyplněná sponzorovaná částka!");
            }

            if (resultCastka == 0 && (SponzorovaniClenove.Count > 0 || SponzorovaneSouteze.Count > 0))
            {
                throw new NonValidDataException("Sponzorovaní členové nebo soutěže nesmějí být vyplněni, protože sponzorovaná částka je 0 Kč!");
            }
        }

        /// <summary>
        /// Metoda slouží k přidání nového sponzora do tabulky a zároveň také do databáze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                Sponzor pridanySponzor = new Sponzor();
                pridanySponzor.Jmeno = tboxJmenoSponzora.Text;
                pridanySponzor.SponzorovanaCastka = Convert.ToInt64(tboxCastka.Text);
                pridanySponzor.SponzorovaniClenove = SponzorovaniClenove.ToList();
                pridanySponzor.SponzorovaneSouteze = SponzorovaneSouteze.ToList();

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // 🔹 Nastavení přihlášeného uživatele pro logování
                    DatabaseSponzori.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // 🔹 Přidání sponzora
                    DatabaseSponzori.AddSponzor(conn, pridanySponzor);

                    // 🔹 Získání ID ze stejné session
                    int? idSponzor = DatabaseSponzori.GetCurrentId(conn);
                    if (idSponzor == null)
                        throw new NullReferenceException("ID sponzora nemůže být NULL! Nastala chyba u spojení s databází...");

                    pridanySponzor.IdSponzor = (int)idSponzor;

                    // 🔹 Vložení vazeb do dalších tabulek
                    if (pridanySponzor.SponzorovaniClenove.Count > 0)
                    {
                        foreach (ClenKlubu clen in pridanySponzor.SponzorovaniClenove)
                            DatabaseSponzoriClenove.AddSponzoriClenove(clen, pridanySponzor);
                    }

                    if (pridanySponzor.SponzorovaneSouteze.Count > 0)
                    {
                        foreach (Soutez soutez in pridanySponzor.SponzorovaneSouteze)
                            DatabaseSponzoriSouteze.AddSponzoriSouteze(soutez, pridanySponzor);
                    }
                }

                sponzoriData.Add(pridanySponzor);

                MessageBox.Show("Sponzor byl úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if(SponzorovaniClenove.Count > 0)
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