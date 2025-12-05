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
    /// Interaction logic for DialogEditujZapas.xaml
    /// </summary>
    public partial class DialogEditujZapas : Window
    {
        private List<VysledekZapasu> vysledkyData;
        private Zapas editovanyZapas;
        private ZapasyOkno zapasyOkno;
        private List<Soutez> souteze = new List<Soutez>();
        private List<string> stavyZapasu = new List<string>();
        private const string StavOdehrano = "Odehráno";
        private const string StavBudeSeHrat = "Bude se hrát";

        public DialogEditujZapas(Zapas editovanyZapas, List<VysledekZapasu> vysledkyData, ZapasyOkno zapasyOkno)
        {
            InitializeComponent();

            this.editovanyZapas = editovanyZapas;
            this.vysledkyData = vysledkyData;
            this.zapasyOkno = zapasyOkno;

            NaplnCbSoutez();
            NaplnCbStavZapasu();

            cbSoutez.SelectedItem = souteze.FirstOrDefault(soutez => soutez.IdSoutez == editovanyZapas.Soutez.IdSoutez);           
            cbStavZapasu.SelectedItem = stavyZapasu.FirstOrDefault(typ => typ.Equals(editovanyZapas.StavZapasu, StringComparison.InvariantCultureIgnoreCase));
            tboxDomaciTym.Text = editovanyZapas.DomaciTym;
            tboxHosteTym.Text = editovanyZapas.HosteTym;
            dtpDatumZapasu.Value = editovanyZapas.Datum;
        }

        /// <summary>
        /// Metoda slouží k zavření dialogového okna
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            if (String.Equals(tboxDomaciTym.Text, tboxHosteTym.Text, StringComparison.InvariantCultureIgnoreCase))
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
        /// Metoda slouží k editaci vybraného zápasu z tabulky a zároveň také v databázi
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidujData();

                string? vybranyStavZapasu = cbStavZapasu.SelectedItem as String;
                Soutez? vybranaSoutez = cbSoutez.SelectedItem as Soutez;
                if (vybranaSoutez != null && !String.IsNullOrEmpty(vybranyStavZapasu) && dtpDatumZapasu.Value.HasValue)
                {
                    editovanyZapas.Soutez = vybranaSoutez;
                    editovanyZapas.StavZapasu = vybranyStavZapasu;
                    editovanyZapas.DomaciTym = tboxDomaciTym.Text;
                    editovanyZapas.HosteTym = tboxHosteTym.Text;
                    editovanyZapas.Datum = (DateTime)dtpDatumZapasu.Value;

                    VysledekZapasu? hledanyVysledek = vysledkyData.Find(vys => vys.IdZapasu == editovanyZapas.IdZapas);

                    // Kontrola, zda editovaný zápas nastavený na ODEHRÁNO nemá ještě výsledek --> Otevře se dialog pro přidání výsledku
                    if (String.Equals(editovanyZapas.StavZapasu, StavOdehrano, StringComparison.InvariantCultureIgnoreCase)
                        && hledanyVysledek == null)
                    {
                        MessageBox.Show("Budete přesměrováni na dialog pro přidáni výsledku zápasu.", "Přesměrování", MessageBoxButton.OK, MessageBoxImage.Information);

                        DialogPridejVysledekZapasu dialogPridejVysledekZapasu = new DialogPridejVysledekZapasu(editovanyZapas);
                        bool? vysledekDiaOkna = dialogPridejVysledekZapasu.ShowDialog();
                        if (vysledekDiaOkna == null || vysledekDiaOkna == false)
                        {
                            throw new NonValidDataException("Nic nebylo editováno, protože jste přerušili zadávání výsledku zápasu!");
                        }

                        using (var conn = DatabaseManager.GetConnection())
                        {
                            VysledekZapasu pridanyVysledek = dialogPridejVysledekZapasu.PridavanyVysledek;

                            editovanyZapas.Vysledek = pridanyVysledek.Vysledek;

                            conn.Open();

                            // Nastavení přihlášeného uživatele pro logování
                            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                            // Editování zápasu
                            DatabaseZapasy.UpdateZapas(conn, editovanyZapas);

                            pridanyVysledek.IdZapasu = editovanyZapas.IdZapas;
                            pridanyVysledek.Zapas = editovanyZapas;

                            // Přidání výsledku zápasu
                            DatabaseVysledkyZapasu.AddVysledekZapasu(conn, pridanyVysledek);

                            vysledkyData.Add(pridanyVysledek);

                            zapasyOkno.dgZapasy.Items.Refresh();

                            MessageBox.Show("Zápas a jeho výsledek byly úspěšně editovány!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        this.Close();
                    }

                    // Kontrola, zda editovaný zápas nastavený na BUDE SE HRÁT má už výsledek --> zeptání se na smazání dosavadního výsledku
                    else if (String.Equals(editovanyZapas.StavZapasu, StavBudeSeHrat, StringComparison.InvariantCultureIgnoreCase)
                        && hledanyVysledek != null)
                    {
                        MessageBoxResult vysledekDiaOkna = MessageBox.Show($"Vámi vybraný zápas má už nastavený výsledek. " +
                           $"Pokud necháte stav na '{StavBudeSeHrat}' bude výsledek smazán! " +
                            $"Opravdu chcete zápas nastavit na stav '{StavBudeSeHrat}' ?", "Upozornění", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (vysledekDiaOkna == MessageBoxResult.No || vysledekDiaOkna == MessageBoxResult.None)
                        {
                            throw new NonValidDataException("Nic nebylo editováno!");
                        }

                        using (var conn = DatabaseManager.GetConnection())
                        {
                            conn.Open();

                            // Nastavení přihlášeného uživatele pro logování
                            DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());

                            // Editování zápasu
                            DatabaseZapasy.UpdateZapas(conn, editovanyZapas);

                            // Odebrání výsledku zápasu
                            DatabaseVysledkyZapasu.OdeberVysledekZapasu(conn, hledanyVysledek);

                            editovanyZapas.Vysledek = String.Empty;

                            vysledkyData.Remove(hledanyVysledek);

                            zapasyOkno.dgZapasy.Items.Refresh();

                            MessageBox.Show("Zápas byl úšpěšně editován a jeho výsledek smazán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            DatabaseZapasy.UpdateZapas(conn, editovanyZapas);

                            MessageBox.Show("Zápas byl úspěšně editován!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        zapasyOkno.dgZapasy.Items.Refresh();
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
            if (dtpDatumZapasu.Value == null)
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
