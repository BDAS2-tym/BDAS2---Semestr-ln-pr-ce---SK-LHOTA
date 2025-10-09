using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Data.SqlClient;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class TreneriOkno : Window
    {
        private HlavniOkno hlavniOkno;

        // Kolekce trenérů pro DataGrid
        public ObservableCollection<Trener> TreneriData { get; set; } = new ObservableCollection<Trener>();

        public TreneriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            DataContext = this; // propojení s DataGridem

            NactiTrenery();
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();

        }

        private void NactiTrenery()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM TRENERI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                TreneriData.Clear();

                while (reader.Read())
                {
                    TreneriData.Add(new Trener
                    {
                        IdClenKlubu = reader["IDCLENKLUBU"] != DBNull.Value ? Convert.ToInt32(reader["IDCLENKLUBU"]) : 0,
                        Jmeno = reader["JMENO"] != DBNull.Value ? reader["JMENO"].ToString() : string.Empty,
                        Prijmeni = reader["PRIJMENI"] != DBNull.Value ? reader["PRIJMENI"].ToString() : string.Empty,
                        RodneCislo = reader["RODNE_CISLO"] != DBNull.Value ? Convert.ToInt64(reader["RODNE_CISLO"]) : 0L,
                        TypClena = reader["TYPCLENA"] != DBNull.Value ? reader["TYPCLENA"].ToString() : string.Empty,
                        TelefonniCislo = reader["TELEFONNICISLO"] != DBNull.Value ? reader["TELEFONNICISLO"].ToString() : string.Empty,
                        TrenerskaLicence = reader["TRENERSKALICENCE"] != DBNull.Value ? reader["TRENERSKALICENCE"].ToString() : string.Empty,
                        Specializace = reader["SPECIALIZACE"] != DBNull.Value ? reader["SPECIALIZACE"].ToString() : string.Empty,
                        PocetLetPraxe = reader["POCETLETPRAXE"] != DBNull.Value ? Convert.ToInt32(reader["POCETLETPRAXE"]) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání trenérů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}