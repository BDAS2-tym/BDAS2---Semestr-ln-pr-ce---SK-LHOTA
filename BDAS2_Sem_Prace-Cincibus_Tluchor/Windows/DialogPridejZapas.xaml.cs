using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
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
    /// Interaction logic for DialogPridejZapas.xaml
    /// </summary>
    public partial class DialogPridejZapas : Window
    {
        private ObservableCollection<Zapas> zapasyData;
        private List<VysledekZapasu> vysledkyData;
        private List<Soutez> souteze = new List<Soutez>();
        private List<string> stavyZapasu = new List<string>();
        private const string StavOdehrano = "Odehráno";
        private const string StavBudeSeHrat = "Bude se hrát";

        public DialogPridejZapas(ObservableCollection<Zapas> zapasyData, List<VysledekZapasu> vysledkyData)
        {
            InitializeComponent();

            this.zapasyData = zapasyData;
            this.vysledkyData = vysledkyData;

            NaplnCbSoutez();
            NaplnCbStavZapasu();
        }

        /// <summary>
        /// Metoda vyresetuje Combobox a DateTimePicker
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbSoutez.SelectedItem = cbStavZapasu.SelectedItem = null;
            dtpDatumZapasu.Value = DateTime.Now;
            tboxDomaciTym.Clear();
            tboxHosteTym.Clear();
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých soutěží
        /// </summary>
        private void NaplnCbSoutez()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM SOUTEZE_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                souteze.Clear();

                while (reader.Read())
                {
                    Soutez soutez = new Soutez();

                    // IDSOUTEZ - NOT NULL
                    if (reader["IDSOUTEZ"] != DBNull.Value)
                        soutez.IdSoutez = Convert.ToInt32(reader["IDSOUTEZ"]);

                    // NAZEVSOUTEZE - NOT NULL
                    if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                        soutez.TypSouteze = reader["NAZEVSOUTEZE"].ToString();
                    else
                        soutez.TypSouteze = "Nenalezeno!";

                    souteze.Add(soutez);
                }

                cbSoutez.ItemsSource = souteze;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soutěží:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých stavů zápasů
        /// </summary>
        private void NaplnCbStavZapasu()
        {
            try
            {
                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM STAV_ZAPASU_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                stavyZapasu.Clear();

                while (reader.Read())
                {
                    // STAVZAPASU - NOT NULL
                    if (reader["STAVZAPASU"] != DBNull.Value)
                        stavyZapasu.Add(reader["STAVZAPASU"].ToString());
                    else
                        stavyZapasu.Add("Nenalezeno");                  
                }

                cbStavZapasu.ItemsSource = stavyZapasu;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání stavů zápasu:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k validaci vstupních dat
        /// </summary>
        /// <exception cref="NonValidDataException">Výjimka se vystaví, pokud jsou vstupní data nevalidní</exception>
        private void ValidujData()
        {
            if (String.IsNullOrWhiteSpace(tboxHosteTym.Text) || String.IsNullOrWhiteSpace(tboxHosteTym.Text)) 
            {
                throw new NonValidDataException("Domací tým ani tým hostů nemůže být prázdný ani NULL!");
            }

            if(String.Equals(tboxDomaciTym.Text, tboxHosteTym.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NonValidDataException("Domácí tým nemůže být stejný jako tým hostů!");
            }

            if (dtpDatumZapasu.Value == null)
            {
                throw new NonValidDataException("Datum zápasu nemůže být NULL!");
            }

            if (cbSoutez.SelectedItem == null)
            {
                throw new NonValidDataException("Vybraná soutěž nemůže být NULL!");
            }

            if (cbStavZapasu.SelectedItem == null)
            {
                throw new NonValidDataException("Vybraný stav nemůže být NULL!");
            }
        }

        /// <summary>
        /// Metoda slouží k přidání nového zápasu a výsledku daného zápasu do tabulky a zároveň také do databáze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnPridej_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                Zapas pridanyZapas = new Zapas();

                string? vybranyStavZapasu = cbStavZapasu.SelectedItem as String;
                Soutez? vybranaSoutez = cbSoutez.SelectedItem as Soutez;
                if (vybranaSoutez != null && !String.IsNullOrEmpty(vybranyStavZapasu) && dtpDatumZapasu.Value.HasValue)
                {
                    pridanyZapas.Soutez = vybranaSoutez;
                    pridanyZapas.StavZapasu = vybranyStavZapasu;
                    pridanyZapas.DomaciTym = tboxDomaciTym.Text;
                    pridanyZapas.HosteTym  = tboxHosteTym.Text;
                    pridanyZapas.Datum = (DateTime)dtpDatumZapasu.Value;

                    if (String.Equals(pridanyZapas.StavZapasu, StavOdehrano, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("Budete přesměrováni na dialog pro přidáni výsledku zápasu.", "Přesměrování", MessageBoxButton.OK, MessageBoxImage.Information);

                        DialogPridejVysledekZapasu dialogPridejVysledekZapasu = new DialogPridejVysledekZapasu(pridanyZapas);
                        bool? vysledekDiaOkna = dialogPridejVysledekZapasu.ShowDialog();
                        if (vysledekDiaOkna == null || vysledekDiaOkna == false)
                        {
                            throw new NonValidDataException("Nic nebylo přidáno, protože jste přerušili zadávání výsledku zápasu!");
                        }

                        using (var conn = DatabaseManager.GetConnection())
                        {
                            VysledekZapasu pridanyVysledek = dialogPridejVysledekZapasu.PridavanyVysledek;

                            pridanyZapas.Vysledek = pridanyVysledek.Vysledek;

                            conn.Open();

                            // Nastavení přihlášeného uživatele pro logování
                            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                            // Přidání zápasu
                            DatabaseZapasy.AddZapas(conn, pridanyZapas);

                            // Získání ID ze stejné session
                            int? idZapas = DatabaseZapasy.GetCurrentId(conn);
                            if (idZapas == null)
                            {
                                throw new NullReferenceException("ID zápasu nemůže být NULL! Nastala chyba u spojení s databází...");
                            }

                            pridanyZapas.IdZapas = (int)idZapas;

                            zapasyData.Add(pridanyZapas);
                            
                            pridanyVysledek.IdZapasu = pridanyZapas.IdZapas;
                            pridanyVysledek.Zapas = pridanyZapas;

                            DatabaseVysledkyZapasu.AddVysledekZapasu(conn, pridanyVysledek);

                            vysledkyData.Add(pridanyVysledek);

                            MessageBox.Show("Zápas a jeho výsledek byly úspěšně přidány!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        this.Close();
                    }

                    else
                    {
                        using (var conn = DatabaseManager.GetConnection())
                        {
                            conn.Open();

                            // Nastavení přihlášeného uživatele pro logování
                            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                            // Přidání zápasu
                            DatabaseZapasy.AddZapas(conn, pridanyZapas);

                            // Získání ID ze stejné session
                            int? idZapas = DatabaseZapasy.GetCurrentId(conn);
                            if (idZapas == null)
                            {
                                throw new NullReferenceException("ID zápasu nemůže být NULL! Nastala chyba u spojení s databází...");
                            }

                            pridanyZapas.IdZapas = (int)idZapas;

                            zapasyData.Add(pridanyZapas);

                            MessageBox.Show("Zápas byl úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        this.Close();
                    }                      
                }
            }

            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k nastavení stavu zápas, dle uvedeného data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDatumZapasu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(dtpDatumZapasu.Value == null)
            {
                return;
            }

            DateTime vybraneDatum = dtpDatumZapasu.Value.Value;

            if (vybraneDatum < DateTime.Now)
            {
                // Stav- Odehráno
                cbStavZapasu.SelectedItem = StavOdehrano;
            }

            else
            {
                // Stav- Bude se hrát
                cbStavZapasu.SelectedItem = StavBudeSeHrat;
            }
        }
    }
}
