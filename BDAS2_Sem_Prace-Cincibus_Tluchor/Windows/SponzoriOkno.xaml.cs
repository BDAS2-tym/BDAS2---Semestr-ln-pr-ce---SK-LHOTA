using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class SponzoriOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        // Kolekce sponzorů pro DataGrid (binding v XAML)
        public ObservableCollection<Sponzor> SponzoriData { get; set; } = new ObservableCollection<Sponzor>();

        public SponzoriOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            DataContext = this; // propojení s DataGridem

            NactiSponzory();
        }
        
        /// <summary>
        /// Metoda slouží k vrácení se na hlavní okno aplikace
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        /// <summary>
        /// Metoda načte sponzory z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiSponzory()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM SPONZORI_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                SponzoriData.Clear();

                while (reader.Read())
                {
                    int idSponzor = Convert.ToInt32(reader["IDSPONZOR"]);
                    var existujiciSponzor = SponzoriData.FirstOrDefault(s => s.IdSponzor == idSponzor);

                    if (existujiciSponzor == null)
                    {
                        // Create new sponsor
                        existujiciSponzor = new Sponzor
                        {
                            IdSponzor = idSponzor,
                            Jmeno = reader["JMENO_SPONZORA"] != DBNull.Value ? reader["JMENO_SPONZORA"].ToString() : "",
                            SponzorovanaCastka = reader["SPONZOROVANACASTKA"] != DBNull.Value ? Convert.ToInt64(reader["SPONZOROVANACASTKA"]) : 0,
                            SponzorovaniClenove = new List<ClenKlubu>(),
                            SponzorovaneSouteze = new List<Soutez>()
                        };

                        SponzoriData.Add(existujiciSponzor);
                    }

                    // Add member if all fields are non-null
                    if (reader["IDCLENKLUBU"] != DBNull.Value &&
                        reader["JMENO_CLENA"] != DBNull.Value &&
                        reader["PRIJMENI_CLENA"] != DBNull.Value &&
                        reader["RODNE_CISLO"] != DBNull.Value)
                    {
                        var clen = new ClenKlubu
                        {
                            IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]),
                            Jmeno = reader["JMENO_CLENA"].ToString(),
                            Prijmeni = reader["PRIJMENI_CLENA"].ToString(),
                            RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"])
                        };

                        // Avoid duplicates
                        if (!existujiciSponzor.SponzorovaniClenove.Any(c => c.IdClenKlubu == clen.IdClenKlubu))
                            existujiciSponzor.SponzorovaniClenove.Add(clen);
                    }

                    // Add competition if all fields are non-null
                    if (reader["IDSOUTEZ"] != DBNull.Value &&
                        reader["STARTDATUM"] != DBNull.Value &&
                        reader["KONECDATUM"] != DBNull.Value &&
                        reader["NAZEVSOUTEZE"] != DBNull.Value)
                    {
                        var soutez = new Soutez
                        {
                            IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]),
                            StartDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["STARTDATUM"])),
                            KonecDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["KONECDATUM"])),
                            TypSouteze = reader["NAZEVSOUTEZE"].ToString()
                        };

                        if (!existujiciSponzor.SponzorovaneSouteze.Any(s => s.IdSoutez == soutez.IdSoutez))
                            existujiciSponzor.SponzorovaneSouteze.Add(soutez);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání sponzorů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
