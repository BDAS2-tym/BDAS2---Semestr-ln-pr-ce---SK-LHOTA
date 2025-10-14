using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    /// <summary>
    /// Interakční logika pro HraciOkno.xaml
    /// </summary>
    public partial class HraciOkno : Window
    {
        private HlavniOkno hlavniOkno;

        //Kolekce hráčů pro DataGrid
        public ObservableCollection<Hrac> HraciData { get; set; } = new ObservableCollection<Hrac>();

        public HraciOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem

            NactiHrace();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        private void btnPridejDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejHrace dialogPridejHrace = new DialogPridejHrace(HraciData);
            dialogPridejHrace.ShowDialog();
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e) {
            DialogNajdiHrace dialogNajdiHrace = new DialogNajdiHrace();
            dialogNajdiHrace.ShowDialog();
        }

        private void btnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Hrac vybranyHrac = dgHraci.SelectedItem as Hrac;

            if (vybranyHrac == null)
            {
                MessageBox.Show("Prosím vyberte hráče, kterého chcete odebrat! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            /// Zeptáme se uživatele a okamžitě ukončíme, pokud neklikne Ano
            if (MessageBox.Show(
                    $"Opravdu chcete odebrat hráče {vybranyHrac.Jmeno} {vybranyHrac.Prijmeni}?",
                    "Potvrzení odebrání",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                // Zahájení transakce
                using var transaction = conn.BeginTransaction();

                try
                {
                    // Smazání z tabulky HRACI
                    using (var cmdHrac = new OracleCommand(
                        "DELETE FROM HRACI WHERE IDCLENKLUBU = " +
                        "(SELECT IDCLENKLUBU" +
                        " FROM CLENOVE_KLUBU" +
                        " WHERE RODNE_CISLO = :rodneCislo)", conn))
                    {
                        cmdHrac.Parameters.Add(new OracleParameter("rodneCislo", vybranyHrac.RodneCislo));
                        cmdHrac.ExecuteNonQuery(); // příkaz pošle databázi a provede ho
                    }

                    // Smazání z tabulky CLENOVE_KLUBU
                    using (var cmdClen = new OracleCommand(
                        "DELETE FROM CLENOVE_KLUBU WHERE RODNE_CISLO = :rodneCislo", conn))
                    {
                        cmdClen.Parameters.Add(new OracleParameter("rodneCislo", vybranyHrac.RodneCislo));
                        cmdClen.ExecuteNonQuery(); 
                    }

                    // Commit transakce
                    transaction.Commit(); // Všechno proběhlo v pořádku – potvrzení změny natrvalo

                    // Odebrání z ObservableCollection
                    HraciData.Remove(vybranyHrac);

                    MessageBox.Show("Hráč byl úspěšně odebrán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    // Pokud dojde k chybě, rollback
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při odebírání hráče:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NactiHrace()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM HRACI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                HraciData.Clear();

                while (reader.Read())
                {
                    Hrac hrac = new Hrac();

                    // RODNE_CISLO - NOT NULL
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        hrac.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        hrac.RodneCislo = 0L;

                    // JMENO - NOT NULL
                    if (reader["JMENO"] != DBNull.Value)
                        hrac.Jmeno = reader["JMENO"].ToString();
                    else
                        hrac.Jmeno = "";

                    // PRIJMENI - NOT NULL
                    if (reader["PRIJMENI"] != DBNull.Value)
                        hrac.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        hrac.Prijmeni = "";

                    // TELEFONNICISLO - NOT NULL
                    if (reader["TELEFONNICISLO"] != DBNull.Value)
                        hrac.TelefonniCislo = reader["TELEFONNICISLO"].ToString();
                    else
                        hrac.TelefonniCislo = "";

                    // POCETVSTRELENYCHGOLU - může být NULL
                    if (reader["POCETVSTRELENYCHGOLU"] != DBNull.Value)
                        hrac.PocetVstrelenychGolu = Convert.ToInt32(reader["POCETVSTRELENYCHGOLU"]);
                    else
                        hrac.PocetVstrelenychGolu = 0;

                    // POCETZLUTYCHKARET - může být NULL
                    if (reader["POCET_ZLUTYCH_KARET"] != DBNull.Value)
                        hrac.PocetZlutychKaret = Convert.ToInt32(reader["POCET_ZLUTYCH_KARET"]);
                    else
                        hrac.PocetZlutychKaret = 0;

                    // POCETCERVENYCHKARET - může být NULL
                    if (reader["POCET_CERVENYCH_KARET"] != DBNull.Value)
                        hrac.PocetCervenychKaret = Convert.ToInt32(reader["POCET_CERVENYCH_KARET"]);
                    else
                        hrac.PocetCervenychKaret = 0;

                    // NAZEV_POZICE - číselník, nesmí být NULL
                    if (reader["NAZEV_POZICE"] != DBNull.Value)
                        hrac.PoziceNaHristi = reader["NAZEV_POZICE"].ToString();
                    else
                        hrac.PoziceNaHristi = "Neznámá"; // default, pokud by bylo NULL

                    HraciData.Add(hrac);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}