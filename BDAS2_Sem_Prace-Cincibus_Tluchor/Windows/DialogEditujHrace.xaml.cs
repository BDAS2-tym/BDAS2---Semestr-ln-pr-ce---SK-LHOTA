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
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string pozice = cbPozice.SelectedItem.ToString();
                string rodneCislo = tboxRodneCislo.Text.Trim();
                int pocetGolu = (int)iudPocetGolu.Value;
                int pocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                int pocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(prijmeni) ||
                    string.IsNullOrWhiteSpace(telCislo) || string.IsNullOrWhiteSpace(pozice))
                {
                    MessageBox.Show("Vyplňte prosím všechna povinná pole! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (pocetGolu < 0 || pocetZlutychKaret < 0 || pocetCervenychKaret < 0)
                {
                    MessageBox.Show("Počet gólů ani karet nesmí být záporný! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var transaction = conn.BeginTransaction();

                try
                {
                    // UPDATE tabulka CLENOVE_KLUBU
                    using (var cmdClen = new OracleCommand(
                        @"UPDATE CLENOVE_KLUBU  
                            SET JMENO = :jmeno,
                            PRIJMENI = :prijmeni,
                            TELEFONNICISLO = :tel
                            WHERE RODNE_CISLO = :rodneCislo", conn))
                    {
                        cmdClen.Parameters.Add(":jmeno", jmeno);
                        cmdClen.Parameters.Add(":prijmeni", prijmeni);
                        cmdClen.Parameters.Add(":tel", telCislo);
                        cmdClen.Parameters.Add(":rodneCislo", rodneCislo);
                        cmdClen.ExecuteNonQuery();
                    }

                    // Získání IDCLENKLUBU podle rodného čísla
                    int idClenKlubu = Convert.ToInt32(
                        new OracleCommand(
                            "SELECT IDCLENKLUBU FROM CLENOVE_KLUBU WHERE RODNE_CISLO = :rodne",
                            conn
                        )
                        {
                            Parameters = { new OracleParameter(":rodne", editovanyHrac.RodneCislo) }
                        }.ExecuteScalar()
                    );

                    // UPDATE tabulka HRACI
                    using (var cmdHrac = new OracleCommand(
                        @"UPDATE HRACI
                        SET POZICENAHRISTI = :pozice,
                        POCETVSTRELENYCHGOLU = :goly,
                        POCET_ZLUTYCH_KARET = :zlute,
                        POCET_CERVENYCH_KARET = :cervene
                        WHERE IDCLENKLUBU = :idClen", conn))
                    {
                        cmdHrac.Parameters.Add(":pozice", pozice);
                        cmdHrac.Parameters.Add(":goly", pocetGolu);
                        cmdHrac.Parameters.Add(":zlute", pocetZlutychKaret);
                        cmdHrac.Parameters.Add(":cervene", pocetCervenychKaret);
                        cmdHrac.Parameters.Add(":idClen", idClenKlubu);
                        cmdHrac.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    // Aktualizace Editovaného hráče
                    editovanyHrac.Jmeno = jmeno;
                    editovanyHrac.Prijmeni = prijmeni;
  
                    editovanyHrac.TelefonniCislo = telCislo;
                    editovanyHrac.PoziceNaHristi = pozice;
                    editovanyHrac.PocetVstrelenychGolu = pocetGolu;
                    editovanyHrac.PocetZlutychKaret = pocetZlutychKaret;
                    editovanyHrac.PocetCervenychKaret = pocetCervenychKaret;

                    hraciOkno.dgHraci.Items.Refresh(); // refresh datagridu

                    MessageBox.Show("Hráč byl úspěšně upraven.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Chyba při ukládání změn:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}