using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbUzivatel.SelectedItem = cbOperace.SelectedItem = cbTabulka.SelectedItem = null;
            dtpDatumDo.Value = dtpDatumOd.Value = null;
        }

        /// <summary>
        /// Metoda slouží k vyfiltrování záznamů a nastavení DialogResult na true
        /// </summary>
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

                var conn = DatabaseManager.GetConnection();

                using var cmd = new OracleCommand("SELECT * FROM PREHLED_UZIVATELSKE_UCTY", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Uzivatel uzivatel = new Uzivatel();

                    uzivatel.UzivatelskeJmeno = reader["UZIVATELSKEJMENO"] != DBNull.Value ? reader["UZIVATELSKEJMENO"].ToString() : "";
                    uzivatel.Role = reader["ROLE"] != DBNull.Value ? reader["ROLE"].ToString() : "";
                    uzivatel.Email = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : "";
                    uzivatel.RodneCislo = reader["RODNE_CISLO"] != DBNull.Value ? reader["RODNE_CISLO"].ToString() : "";
                    uzivatel.PosledniPrihlaseni = reader["POSLEDNIPRIHLASENI"] != DBNull.Value ? Convert.ToDateTime(reader["POSLEDNIPRIHLASENI"]) : DateTime.MinValue;

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

                var conn = DatabaseManager.GetConnection();

                using var cmd = new OracleCommand("SELECT * FROM TABULKY_VIEW", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string jmenoTabulky = reader["TABULKA"] != DBNull.Value ? reader["TABULKA"].ToString() : "";
                    if (!string.IsNullOrEmpty(jmenoTabulky))
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
            if (dtpDatumOd.Value != null && dtpDatumDo.Value != null && dtpDatumOd.Value > dtpDatumDo.Value)
            {
                throw new NonValidDataException("Datum od nemůže být později než datum do!");
            }

            var vysledkyFiltrovani = zaznamyData.AsEnumerable();

            Uzivatel vybranyUzivatel = cbUzivatel.SelectedItem as Uzivatel;
            string vybranaOperace = cbOperace.SelectedItem as string;
            string vybranaTabulka = cbTabulka.SelectedItem as string;

            if (vybranyUzivatel != null)
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z => z.Uzivatel != null && z.Uzivatel.UzivatelskeJmeno.Contains(vybranyUzivatel.UzivatelskeJmeno, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(vybranaOperace))
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z => z.Operace != null && z.Operace.Contains(vybranaOperace, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(vybranaTabulka))
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z => z.Tabulka != null && z.Tabulka.Contains(vybranaTabulka, StringComparison.OrdinalIgnoreCase));

            if (dtpDatumOd.Value != null)
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z => z.Cas.Date >= dtpDatumOd.Value.Value.Date);

            if (dtpDatumDo.Value != null)
                vysledkyFiltrovani = vysledkyFiltrovani.Where(z => z.Cas.Date <= dtpDatumDo.Value.Value.Date);

            return vysledkyFiltrovani;
        }
    }
}
