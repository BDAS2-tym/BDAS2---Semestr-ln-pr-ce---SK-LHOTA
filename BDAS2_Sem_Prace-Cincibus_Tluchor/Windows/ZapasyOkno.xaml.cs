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
    /// Interakční logika pro ZapasyOkno.xaml
    /// </summary>
    public partial class ZapasyOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        // Kolekce zápasů pro DataGrid (binding v XAML)
        public ObservableCollection<Zapas> ZapasyData { get; set; } = new ObservableCollection<Zapas>();

        // Kolekce výsledků
        public List<VysledekZapasu> VysledkyData { get; set; } = new List<VysledekZapasu>();

        public ZapasyOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;

            // Propojení kolekce s DataGridem
            DataContext = this;

            NactiZapasy();
            NactiVysledky();
        }

        /// <summary>
        /// Metoda slouží k vrácení se na hlavní okno aplikace
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            hlavniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Metoda načte zapasy z databáze přes DatabaseManager a naplní DataGrid
        /// </summary>
        private void NactiZapasy()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM ZAPASY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                ZapasyData.Clear();

                while (reader.Read())
                {
                    Zapas zapas = new Zapas();

                    // IDZAPAS - NOT NULL
                    if (reader["IDZAPAS"] != DBNull.Value)
                        zapas.IdZapas = Convert.ToInt32(reader["IDZAPAS"]);

                    // DATUM - NOT NULL
                    if (reader["DATUM"] != DBNull.Value)
                        zapas.Datum = reader.GetDateTime(reader.GetOrdinal("DATUM"));

                    // DOMACITYM - NOT NULL
                    if (reader["DOMACITYM"] != DBNull.Value)
                        zapas.DomaciTym = reader["DOMACITYM"].ToString();
                    else
                        zapas.DomaciTym = "";

                    // HOSTETYM - NOT NULL
                    if (reader["HOSTETYM"] != DBNull.Value)
                        zapas.HosteTym = reader["HOSTETYM"].ToString();
                    else
                        zapas.HosteTym = "";

                    // CISLONAAGENTA - NOT NULL
                    if (reader["STAVZAPASU"] != DBNull.Value)
                        zapas.StavZapasu = reader["STAVZAPASU"].ToString();
                    else
                        zapas.StavZapasu = "";

                    // VYSLEDEK - NULL
                    if (reader["VYSLEDEK"] != DBNull.Value)
                        zapas.Vysledek = reader["VYSLEDEK"].ToString();
                    else
                        zapas.Vysledek = "";

                    // Přidání soutěže, pokud nejsou všechny atributy NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value && reader["NAZEVSOUTEZE"] != DBNull.Value)
                    {
                        zapas.Soutez = new Soutez
                        {
                            IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]),
                            TypSouteze = reader["NAZEVSOUTEZE"].ToString()
                        };
                    }

                    ZapasyData.Add(zapas);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání zapasů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda načte výsledky zápasů z databáze přes DatabaseManager
        /// </summary>
        private void NactiVysledky()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM VYSLEDKY_ZAPASU_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                VysledkyData.Clear();

                while (reader.Read())
                {
                    VysledekZapasu vysledekZapasu = new VysledekZapasu();

                    // IDZAPAS - NOT NULL
                    if (reader["IDZAPAS"] != DBNull.Value)
                        vysledekZapasu.IdZapasu = Convert.ToInt32(reader["IDZAPAS"]);

                    // POCETZLUTYCHKARET - NOT NULL
                    if (reader["POCETZLUTYCHKARET"] != DBNull.Value)
                        vysledekZapasu.PocetZlutychKaret = Convert.ToInt32(reader["POCETZLUTYCHKARET"]);
                    else
                        vysledekZapasu.PocetZlutychKaret = 0;

                    // POCETCERVENYCHKARET - NOT NULL
                    if (reader["POCETCERVENYCHKARET"] != DBNull.Value)
                        vysledekZapasu.PocetCervenychKaret = Convert.ToInt32(reader["POCETCERVENYCHKARET"]);
                    else
                        vysledekZapasu.PocetCervenychKaret = 0;

                    // POCETGOLYDOMACITYM - NOT NULL
                    if (reader["POCETGOLYDOMACITYM"] != DBNull.Value)
                        vysledekZapasu.PocetGolyDomaci = Convert.ToInt32(reader["POCETGOLYDOMACITYM"]);
                    else
                        vysledekZapasu.PocetGolyDomaci = 0;

                    // POCETGOLYHOSTETYM - NOT NULL
                    if (reader["POCETGOLYHOSTETYM"] != DBNull.Value)
                        vysledekZapasu.PocetGolyHoste = Convert.ToInt32(reader["POCETGOLYHOSTETYM"]);
                    else
                        vysledekZapasu.PocetGolyHoste = 0;

                    // Přidání zápasu, pokud se daný zápas nachází v kolekci
                    if (ZapasyData.Any(zapas => zapas.IdZapas == vysledekZapasu.IdZapasu))
                    {
                        Zapas? nalezenyZapas = ZapasyData.FirstOrDefault(zapas => zapas.IdZapas == vysledekZapasu.IdZapasu);

                        if (nalezenyZapas != null)
                        {
                            vysledekZapasu.Zapas = nalezenyZapas;
                            vysledekZapasu.Vysledek = nalezenyZapas.Vysledek;
                        }
                    }

                    VysledkyData.Add(vysledekZapasu);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání výsledků zápasů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void dgZapasy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;

                MessageBox.Show("Smazání zápasu klávesou Delete není povoleno.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgZapasy.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgZapasy.Focusable = false;
                Keyboard.ClearFocus();
                dgZapasy.Focusable = true;
            }
        }

        /// <summary>
        /// Metoda slouží k přidání zápasu do tabulky
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            DialogPridejZapas dialogPridejZapas = new DialogPridejZapas(ZapasyData, VysledkyData);
            dialogPridejZapas.ShowDialog();
        }
    }
}
