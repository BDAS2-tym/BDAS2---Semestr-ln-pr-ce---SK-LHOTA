using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Interaction logic for DialogNajdiZaznam.xaml
    /// </summary>
    public partial class DialogNajdiZaznam : Window
    {
        private ObservableCollection<Zaznam> zaznamyData;
        private List<Uzivatel> uzivatele = new List<Uzivatel>();
        private List<string> operace;
        private List<string> tabulky = new List<string>();

        public IEnumerable<Zaznam> VyfiltrovaneZaznamy { get; private set; }

        public DialogNajdiZaznam(ObservableCollection<Zaznam> zaznamyData)
        {
            InitializeComponent();           

            // Vlastní definování jednotlivých druhů operací
            operace = new List<string>
                        {
                             "INSERT",
                             "DELETE",
                             "UPDATE"
                        };

            NaplnCbUzivatel();
            NaplnCbTabulky();
            cbOperace.ItemsSource = operace;


            this.zaznamyData = zaznamyData;
        }

        /// <summary>
        /// Metoda vyresetuje textová pole
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbUzivatel.SelectedItem = cbOperace.SelectedItem = cbTabulka.SelectedItem = null;
            dtpDatumDo.Value = dtpDatumOd.Value = null;
        }

        private void btnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaneZaznamy = FiltrujZaznamy();
                DialogResult = true;
                this.Close();
            }

            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při filtrování :\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }        
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých uživatelů
        /// </summary>
        private void NaplnCbUzivatel()
        {
            try
            {
                uzivatele.Clear();

                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM PREHLED_UZIVATELSKE_UCTY", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Uzivatel uzivatel = new Uzivatel();

                    // Uživatelské jméno
                    if (reader["UZIVATELSKEJMENO"] != DBNull.Value)
                        uzivatel.UzivatelskeJmeno = reader["UZIVATELSKEJMENO"].ToString();
                    else
                        uzivatel.UzivatelskeJmeno = "";

                    // Role
                    if (reader["ROLE"] != DBNull.Value)
                        uzivatel.Role = reader["ROLE"].ToString();
                    else
                        uzivatel.Role = "";

                    // Email
                    if (reader["EMAIL"] != DBNull.Value)
                        uzivatel.Email = reader["EMAIL"].ToString();
                    else
                        uzivatel.Email = "";

                    // Rodné číslo
                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        uzivatel.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        uzivatel.RodneCislo = "";

                    // Poslední přihlášení
                    if (reader["POSLEDNIPRIHLASENI"] != DBNull.Value)
                        uzivatel.PosledniPrihlaseni = Convert.ToDateTime(reader["POSLEDNIPRIHLASENI"]);
                    else
                        uzivatel.PosledniPrihlaseni = DateTime.MinValue;

                    uzivatele.Add(uzivatel);
                }

                cbUzivatel.ItemsSource = uzivatele;
                cbUzivatel.DisplayMemberPath = "UzivatelskeJmeno";
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání uživatelů:\n{ex.Message}",
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k naplnění Comboboxu s daty jednotlivých tabulek v databázi
        /// </summary>
        private void NaplnCbTabulky()
        {
            try
            {
                tabulky.Clear();

                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM TABULKY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string? jmenoTabulky = String.Empty;

                    // Jméno tabulky
                    if (reader["TABULKA"] != DBNull.Value)
                        jmenoTabulky = reader["TABULKA"].ToString();

                    if(jmenoTabulky != null)
                        tabulky.Add(jmenoTabulky);
                }

                cbTabulka.ItemsSource = tabulky;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání tabulek:\n{ex.Message}",
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k filtrování všech záznamů podle zadaných kritérií
        /// </summary>
        /// <returns>Vrací kolekci IEnumerable vyfiltrovaných záznamů</returns>
        private IEnumerable<Zaznam> FiltrujZaznamy()
        {
            if (dtpDatumOd.Value != null && dtpDatumDo.Value != null)
            {
                if (dtpDatumOd.Value > dtpDatumDo.Value)
                {
                    throw new NonValidDataException("Datum od nemůže být později než datum do!");
                }
            }

            var vysledkyFiltrovani = zaznamyData.AsEnumerable();

            Uzivatel? vybranyUzivatel = cbUzivatel.SelectedItem as Uzivatel;
            string? vybranaOperace = cbOperace.SelectedItem as String;
            string? vybranaTabulka = cbTabulka.SelectedItem as String;

            // Uživatel
            if (vybranyUzivatel != null)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Uzivatel != null &&
                    z.Uzivatel.UzivatelskeJmeno.Contains(vybranyUzivatel.UzivatelskeJmeno, StringComparison.OrdinalIgnoreCase));
            }

            // Operace
            if (!string.IsNullOrWhiteSpace(vybranaOperace))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Operace != null &&
                    z.Operace.Contains(vybranaOperace, StringComparison.OrdinalIgnoreCase));
            }

            // Tabulka
            if (!string.IsNullOrWhiteSpace(vybranaTabulka))
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Tabulka != null &&
                    z.Tabulka.Contains(vybranaTabulka, StringComparison.OrdinalIgnoreCase));
            }

            // Datum od
            if (dtpDatumOd.Value != null)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Cas.Date >= dtpDatumOd.Value.Value.Date);
            }

            // Datum do
            if (dtpDatumDo.Value != null)
            {
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z =>
                    z.Cas.Date <= dtpDatumDo.Value.Value.Date);
            }

            return vysledkyFiltrovani;
        }
    }
}
