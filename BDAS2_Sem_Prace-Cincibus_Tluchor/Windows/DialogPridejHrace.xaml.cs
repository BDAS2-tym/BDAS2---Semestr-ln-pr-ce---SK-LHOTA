using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogPridejHrace : Window
    {
        private ObservableCollection<Hrac> HraciData;

        public DialogPridejHrace(ObservableCollection<Hrac> HraciData)
        {
            InitializeComponent();
            this.HraciData = HraciData;

            // Naplnění ComboBoxu pro pozice hráčů
            cbPozice.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };
    
        }

        // Reset všech polí
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxRodneCislo.Clear();
            tboxJmeno.Clear();
            tboxPrijmeni.Clear();
            tboxTelCislo.Clear();
            cbPozice.SelectedIndex = 0;
            iudPocetCervenychKaret.Value = iudPocetGolu.Value = iudPocetZlutychKaret.Value = 0;
        }

        // Přidání hráče
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long rodneCislo = long.Parse(tboxRodneCislo.Text);
                string jmeno = tboxJmeno.Text;
                string prijmeni = tboxPrijmeni.Text;
                string telCislo = tboxTelCislo.Text;
                string pozice = cbPozice.SelectedItem.ToString();
                int pocetGolu = (int)iudPocetGolu.Value;
                int pocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                int pocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                // Získání nového ID člena (sekvence)
                int idClenKlubu = Convert.ToInt32(new OracleCommand("SELECT SEKV_CLENKLUBU.NEXTVAL FROM DUAL", conn).ExecuteScalar());

                // Vložení do CLENOVE_KLUBU
                using (var cmdClen = new OracleCommand(
                    @"INSERT INTO CLENOVE_KLUBU
                      (idClenKlubu, rodne_cislo, jmeno, prijmeni, telefonniCislo, typClena)
                      VALUES (:id, :rodne, :jmeno, :prijmeni, :tel, 'Hrac')", conn))
                {
                    cmdClen.Parameters.Add(":id", idClenKlubu);
                    cmdClen.Parameters.Add(":rodne", rodneCislo);
                    cmdClen.Parameters.Add(":jmeno", jmeno);
                    cmdClen.Parameters.Add(":prijmeni", prijmeni);
                    cmdClen.Parameters.Add(":tel", telCislo);
                    cmdClen.ExecuteNonQuery();
                }

                // Získání ID pozice
                int idPozice = Convert.ToInt32(new OracleCommand(
                    "SELECT id_pozice FROM POZICE_HRAC WHERE nazev_pozice = :pozice", conn)
                { Parameters = { new OracleParameter(":pozice", pozice) } }
                    .ExecuteScalar());

                // Vložení do HRACI
                using (var cmdHrac = new OracleCommand(
                    @"INSERT INTO HRACI
                      (idClenKlubu, pocetVstrelenychGolu, pocet_zlutych_karet, pocet_cervenych_karet, id_pozice)
                      VALUES (:id, :goly, :zlute, :cervene, :pozice)", conn))
                {
                    cmdHrac.Parameters.Add(":id", idClenKlubu);
                    cmdHrac.Parameters.Add(":goly", pocetGolu);
                    cmdHrac.Parameters.Add(":zlute", pocetZlutychKaret);
                    cmdHrac.Parameters.Add(":cervene", pocetCervenychKaret);
                    cmdHrac.Parameters.Add(":pozice", idPozice);
                    cmdHrac.ExecuteNonQuery();
                }

                // Přidání do ObservableCollection 
                Hrac novyHrac = new Hrac(rodneCislo, jmeno, prijmeni, telCislo,
                    pocetGolu, pocetZlutychKaret, pocetCervenychKaret, pozice);
                HraciData.Add(novyHrac);

                MessageBox.Show("Hráč byl úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Chybný formát vstupu!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nastala chyba:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}