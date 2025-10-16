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

                // Vyprázdníme kolekci, aby se nepřidávaly duplicitní záznamy
                TreneriData.Clear();

                while (reader.Read())
                {
                    Trener trener = new Trener();

                    //// ID člena klubu (NOT NULL) - pokud je NULL, nastavíme 0
                    //if (reader["IDCLENKLUBU"] != DBNull.Value)
                    //    trener.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);
                    //else
                    //    trener.IdClenKlubu = 0;

                    // Rodné číslo (NOT NULL) 
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        trener.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                    else
                        trener.RodneCislo = 0L;

                    // Jméno (NOT NULL)
                    if (reader["JMENO"] != DBNull.Value)
                        trener.Jmeno = reader["JMENO"].ToString();
                    else
                        trener.Jmeno = "";

                    // Příjmení (NOT NULL) 
                    if (reader["PRIJMENI"] != DBNull.Value)
                        trener.Prijmeni = reader["PRIJMENI"].ToString();
                    else
                        trener.Prijmeni = "";

                    // Telefonní číslo (NOT NULL) 
                    if (reader["TELEFONNICISLO"] != DBNull.Value)
                        trener.TelefonniCislo = reader["TELEFONNICISLO"].ToString();
                    else
                        trener.TelefonniCislo = "000000000";

                    // Trenérská licence (NOT NULL) 
                    if (reader["TRENERSKALICENCE"] != DBNull.Value)
                        trener.TrenerskaLicence = reader["TRENERSKALICENCE"].ToString();
                    else
                        trener.TrenerskaLicence = "";

                    // Specializace (volitelný sloupec) 
                    if (reader["SPECIALIZACE"] != DBNull.Value)
                        trener.Specializace = reader["SPECIALIZACE"].ToString();
                    else
                        trener.Specializace = "Volitelné nezadáno !";

                    // Počet let praxe (volitelný sloupec) 
                    if (reader["POCETLETPRAXE"] != DBNull.Value)
                        trener.PocetLetPraxe = Convert.ToInt32(reader["POCETLETPRAXE"]);
                    else
                        trener.PocetLetPraxe = 0;

                    // Přidáme trenéra do kolekce pro DataGrid
                    TreneriData.Add(trener);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání trenérů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}