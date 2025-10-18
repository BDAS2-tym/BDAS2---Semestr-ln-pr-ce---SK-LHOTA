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
                    Sponzor? existujiciSponzor = SponzoriData.First(spon => Convert.ToInt32(reader["IDSPONZOR"]) == spon.IdSponzor);
                    if (existujiciSponzor != null)
                    {
                        /* TODO */
                        //IClenKlubu novyClen = new IClenKlubu();
                        //if (reader["IDCLENKLUBU"] != DBNull.Value || reader["JMENO_CLENA"] != DBNull.Value || reader["PRIJMENI_CLENA"] != DBNull.Value || reader["RODNE_CISLO"] != DBNull.Value)
                        //{
                        //    novyClen.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);
                        //    novyClen.Jmeno = reader["JMENO_CLENA"].ToString();
                        //    novyClen.Prijmeni = reader["PRIJMENI_CLENA"].ToString();
                        //    novyClen.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                        //    existujiciSponzor.SponzorovaniClenove.Add(novyClen);
                        //}

                        Soutez novaSoutez = new Soutez();
                        if (reader["IDSOUTEZ"] != DBNull.Value || reader["STARTDATUM"] != DBNull.Value || reader["KONECDATUM"] != DBNull.Value || reader["NAZEVSOUTEZE"] != DBNull.Value)
                        {
                            novaSoutez.IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]);
                            novaSoutez.StartDatum = DateOnly.ParseExact(reader["STARTDATUM"].ToString(), "dd-MM-yy", CultureInfo.InvariantCulture);
                            novaSoutez.KonecDatum = DateOnly.ParseExact(reader["KONECDATUM"].ToString(), "dd-MM-yy", CultureInfo.InvariantCulture);
                            novaSoutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                            existujiciSponzor.SponzorovaneSouteze.Add(novaSoutez);
                        }
                    }

                    else
                    {
                        Sponzor sponzor = new Sponzor();
                        List<ClenKlubu> sponzorovaniClenove = new List<ClenKlubu>();
                        List<Soutez> sponzorovaneSouteze = new List<Soutez>();
                        sponzor.SponzorovaniClenove = sponzorovaniClenove;
                        sponzor.SponzorovaneSouteze = sponzorovaneSouteze;

                        // IDSPONZORA - NOT NULL
                        if (reader["IDSPONZOR"] != DBNull.Value)
                            sponzor.IdSponzor = Convert.ToInt32(reader["IDSPONZOR"]);
                        else
                            sponzor.IdSponzor = 0;

                        // JMENO_SPONZORA - NOT NULL
                        if (reader["JMENO_SPONZORA"] != DBNull.Value)
                                sponzor.Jmeno = (reader["JMENO_SPONZORA"]).ToString();
                        else
                            sponzor.Jmeno = "";

                        // SPONZOROVANACASTKA - NULL
                        if (reader["SPONZOROVANACASTKA"] != DBNull.Value)
                            sponzor.SponzorovanaCastka = Convert.ToInt64(reader["SPONZOROVANACASTKA"]);
                        else
                            sponzor.SponzorovanaCastka = 0L;

                        /* TODO */
                        // Přidání hodnot k atributům u člena
                        // Aby se člen přidal, nesmí být žádná z načtených hodnot NULL
                        //IClenKlubu clen = new IClenKlubu();
                        //if (reader["IDCLENKLUBU"] != DBNull.Value || reader["JMENO_CLENA"] != DBNull.Value || reader["PRIJMENI_CLENA"] != DBNull.Value || reader["RODNE_CISLO"] != DBNull.Value)
                        //{
                        //    clen.IdClenKlubu = Convert.ToInt32(reader["IDCLENKLUBU"]);
                        //    clen.Jmeno = reader["JMENO_CLENA"].ToString();
                        //    clen.Prijmeni = reader["PRIJMENI_CLENA"].ToString();
                        //    clen.RodneCislo = Convert.ToInt64(reader["RODNE_CISLO"]);
                        //    sponzorovaniClenove.Add(clen);
                        //}

                        // Přidání hodnot k atributům u soutěže
                        // Aby se soutěž přidala, nesmí být žádná z načtených hodnot NULL
                        Soutez soutez = new Soutez();
                        if(reader["IDSOUTEZ"] != DBNull.Value || reader["STARTDATUM"] != DBNull.Value || reader["KONECDATUM"] != DBNull.Value || reader["NAZEVSOUTEZE"] != DBNull.Value)
                        {
                            soutez.IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]);
                            soutez.StartDatum = DateOnly.ParseExact(reader["STARTDATUM"].ToString(), "dd-MM-yy", CultureInfo.InvariantCulture);
                            soutez.KonecDatum = DateOnly.ParseExact(reader["KONECDATUM"].ToString(), "dd-MM-yy", CultureInfo.InvariantCulture);
                            soutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                            sponzorovaneSouteze.Add(soutez);
                        }

                        SponzoriData.Add(sponzor);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání hráčů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
