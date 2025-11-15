using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
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
    /// Interakční logika pro DialogEditujHrace.xaml
    /// </summary>
    public partial class DialogEditujHrace : Window
    {
        private Hrac editovanyHrac;
        private HraciOkno hraciOkno;    

        public DialogEditujHrace(Hrac editovanyHrac, HraciOkno hraciOkno)
        {
            InitializeComponent();
            this.editovanyHrac = editovanyHrac;
            this.hraciOkno = hraciOkno;


            cbPozice.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };

            tboxRodneCislo.Text = editovanyHrac.RodneCislo.ToString();
            tboxJmeno.Text = editovanyHrac.Jmeno;
            tboxPrijmeni.Text = editovanyHrac.Prijmeni;
            tboxTelCislo.Text = editovanyHrac.TelefonniCislo;
            cbPozice.SelectedItem = editovanyHrac.PoziceNaHristi.ToString();
            iudPocetGolu.Value = editovanyHrac.PocetVstrelenychGolu;
            iudPocetZlutychKaret.Value = editovanyHrac.PocetZlutychKaret;
            iudPocetCervenychKaret.Value = editovanyHrac.PocetCervenychKaret;

        }

        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // --- VALIDACE RODNÉHO ČÍSLA ---
                if (!long.TryParse(tboxRodneCislo.Text.Trim(), out long rodneCislo))
                {
                    MessageBox.Show("Rodné číslo může obsahovat pouze číslice! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Délka rodného čísla (10 číslic)
                if (rodneCislo.ToString().Length != 10)
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic bez lomítka! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- VALIDACE TEXTOVÝCH POLÍ ---
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string pozice = cbPozice.SelectedItem.ToString();

                if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(prijmeni) || 
                    string.IsNullOrWhiteSpace(telCislo) || string.IsNullOrWhiteSpace(pozice))
                {
                    MessageBox.Show("Prosím vyplňte všechna pole! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- VALIDACE ČÍSELNÝCH HODNOT ---
                int pocetGolu = (int)iudPocetGolu.Value;
                int pocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                int pocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                if (pocetGolu < 0 || pocetZlutychKaret < 0 || pocetCervenychKaret < 0)
                {
                    MessageBox.Show("Počet gólů a karet nesmí být záporný !", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // --- NASTAVENÍ HODNOT DO EDITOVANEHO HRACE ---
                editovanyHrac.RodneCislo = rodneCislo;
                editovanyHrac.Jmeno = jmeno;
                editovanyHrac.Prijmeni = prijmeni;
                editovanyHrac.TelefonniCislo = telCislo;
                editovanyHrac.PoziceNaHristi = pozice;
                editovanyHrac.PocetVstrelenychGolu = pocetGolu;
                editovanyHrac.PocetZlutychKaret = pocetZlutychKaret;
                editovanyHrac.PocetCervenychKaret = pocetCervenychKaret;

                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    // Nastavení přihlášeného uživatele pro logování
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                    // Editování hráče
                    DatabaseHraci.UpdateHrac(conn, editovanyHrac);

                    hraciOkno.dgHraci.Items.Refresh();
                }

                MessageBox.Show("Hráč byl úspěšně editován! ", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání hráče:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}