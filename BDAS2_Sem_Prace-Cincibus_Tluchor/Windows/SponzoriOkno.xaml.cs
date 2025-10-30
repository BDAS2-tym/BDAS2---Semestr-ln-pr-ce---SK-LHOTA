using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

            // Propojení kolekce s DataGridem
            DataContext = this;

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
                    Sponzor? existujiciSponzor = SponzoriData.FirstOrDefault(s => s.IdSponzor == idSponzor);

                    if (existujiciSponzor == null)
                    {
                        // Vytvoření nového sponzora
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

                    // Přidání člena, pokud nejsou všechny atributy NULL
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

                        // Zamezení duplikací sponzorů
                        if (!existujiciSponzor.SponzorovaniClenove.Any(c => c.IdClenKlubu == clen.IdClenKlubu))
                            existujiciSponzor.SponzorovaniClenove.Add(clen);
                    }

                    // Přidání soutěže, pokud nejsou všechny atributy NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value && reader["STARTDATUM"] != DBNull.Value && reader["KONECDATUM"] != DBNull.Value && reader["NAZEVSOUTEZE"] != DBNull.Value)
                    {
                        Soutez soutez = new Soutez
                        {
                            IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]),
                            StartDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["STARTDATUM"])),
                            KonecDatum = DateOnly.FromDateTime(Convert.ToDateTime(reader["KONECDATUM"])),
                            TypSouteze = reader["NAZEVSOUTEZE"].ToString()
                        };

                        // Zamezení duplikací sponzorů
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

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgSponzori_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání sponzora klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Metoda slouží k přidání sponzora do tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejSponzora dialogPridejSponzora = new DialogPridejSponzora(SponzoriData);
            dialogPridejSponzora.ShowDialog();
        }

        /// <summary>
        /// Metoda slouží k odebrání sponzora z tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Sponzor? vybranySponzor = dgSponzori.SelectedItem as Sponzor;

            if(vybranySponzor == null)
            {
                MessageBox.Show(
                    "Prosím, vyberte sponzora, kterého chcete odebrat!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Potvrzení od uživatele
            MessageBoxResult potvrzeni = MessageBox.Show($"Opravdu chcete odebrat sponzora {vybranySponzor.Jmeno}?", "Potvrzení odebrání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if(potvrzeni == MessageBoxResult.No)
            {
                return;
            }

            // Smazání z databáze
            try
            {
                // Odebrání všech vytvořených vazeb z vazební tabulky SPONZORI_CLENOVE
                if (vybranySponzor.SponzorovaniClenove.Count > 0)
                {
                    foreach (ClenKlubu clen in vybranySponzor.SponzorovaniClenove)
                    {
                        DatabaseSponzoriClenove.OdeberSponzoriClenove(clen, vybranySponzor);
                    }
                }

                // Odebrání všech vytvořených vazeb z vazební tabulky SPONZORI_SOUTEZE
                if (vybranySponzor.SponzorovaneSouteze.Count > 0)
                {
                    foreach (Soutez soutez in vybranySponzor.SponzorovaneSouteze)
                    {
                        DatabaseSponzoriSouteze.OdeberSponzoriSouteze(soutez, vybranySponzor);
                    }
                }

                DatabaseSponzori.OdeberSponzor(vybranySponzor);

                // Aktualizace DataGridu (odebrání z kolekce)
                SponzoriData.Remove(vybranySponzor);

                // Úspěch
                MessageBox.Show(
                    $"Sponzor {vybranySponzor.Jmeno} byl úspěšně odebrán.",
                    "Úspěch",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba databáze při mazání sponzora:\n{ex.Message}", "Databázová chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Nastala neočekávaná chyba při mazání sponzora:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zobrazení editovacího dialogu sponzora
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dgSponzori_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // Získání objektu DataGrid a jeho potomků, aby se DoubleClick uplatňoval pouze na řádky a ne na ColumnHeader
            while (dep != null && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridRow row)
            {
                Sponzor? vybranySponzor = (Sponzor)row.Item;
                if (vybranySponzor == null)
                {
                    MessageBox.Show("Prosím vyberte sponzora, kterého chcete upravit! ", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogEditujSponzora dialogEditujdialogEditujSponzora = new DialogEditujSponzora(vybranySponzor, this);
                dialogEditujdialogEditujSponzora.ShowDialog();
            }
        }
    }
}
