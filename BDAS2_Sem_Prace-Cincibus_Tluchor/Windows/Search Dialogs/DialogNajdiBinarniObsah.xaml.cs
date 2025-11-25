using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Dialogové okno sloužící k filtrování záznamů binárního obsahu podle zadaných kritérií
    /// </summary>
    public partial class DialogNajdiBinarniObsah : Window
    {
        /// <summary>
        /// Kolekce všech souborů binárního obsahu k filtrování
        /// </summary>
        private ObservableCollection<BinarniObsah> obsahData;

        /// <summary>
        /// Výsledky filtrů, které se vrací zpět do datagridu po zavření dialogu
        /// </summary>
        public IEnumerable<BinarniObsah> VyfiltrovanyObsah { get; private set; }

        /// <summary>
        /// Konstruktor dialogu pro filtrování binárního obsahu
        /// </summary>
        /// <param name="obsahData">Kolekce záznamů binárních souborů</param>
        public DialogNajdiBinarniObsah(ObservableCollection<BinarniObsah> obsahData)
        {
            InitializeComponent();
            this.obsahData = obsahData;
        }

        /// <summary>
        /// Resetuje všechna vstupní pole ve formuláři do původního stavu
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tboxNazev.Text = "";
            tboxTyp.Text = "";
            tboxPripona.Text = "";
            cbOperace.SelectedIndex = -1;
            cbUzivatel.SelectedIndex = -1;
            dpNahrani.SelectedDate = null;
            dpZmeny.SelectedDate = null;
        }

        /// <summary>
        /// Spustí filtrování podle zadaných hodnot a vrátí seznam výsledků do datagridu
        /// </summary>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovanyObsah = FiltrujObsah();
                DialogResult = true;
                this.Close();
            }
            catch (NonValidDataException ex)
            {
                MessageBox.Show(ex.Message, "Nevalidní data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při filtrování:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Provede filtraci podle hodnot z textových polí, comboboxů a datových polí
        /// </summary>
        /// <returns>Kolekce vyfiltrovaných záznamů</returns>
        private IEnumerable<BinarniObsah> FiltrujObsah()
        {
            // Kolekci s původními daty, která se bude postupně filtrovat
            IEnumerable<BinarniObsah> vysledek = obsahData;

            string nazev = tboxNazev.Text;
            string typ = tboxTyp.Text;
            string pripona = tboxPripona.Text;

            string operace = null;

            // Kontrola, zda je v combo boxu něco vybrané
            if (cbOperace.SelectedItem != null)
            {
                ComboBoxItem item = cbOperace.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    operace = item.Content.ToString();
                }
                   
            }

            string uzivatel = null;

            // Kontrola, zda je v combo boxu něco vybrané
            if (cbUzivatel.SelectedItem != null)
            {
                ComboBoxItem item = cbUzivatel.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    uzivatel = item.Content.ToString();
                }
                 
            }

            // Název
            if (!string.IsNullOrWhiteSpace(nazev))
            {
                vysledek = vysledek.Where(o =>
                    o.NazevSouboru != null &&
                    o.NazevSouboru.Contains(nazev, StringComparison.OrdinalIgnoreCase));
            }

            // Typ (MIME)
            if (!string.IsNullOrWhiteSpace(typ))
            {
                vysledek = vysledek.Where(o =>
                    o.TypSouboru != null &&
                    o.TypSouboru.Contains(typ, StringComparison.OrdinalIgnoreCase));
            }

            // Přípona
            if (!string.IsNullOrWhiteSpace(pripona))
            {
                if (pripona.Length > 10)
                    throw new NonValidDataException("Přípona může mít maximálně 10 znaků");

                vysledek = vysledek.Where(o =>
                    o.PriponaSouboru != null &&
                    o.PriponaSouboru.Contains(pripona, StringComparison.OrdinalIgnoreCase));
            }

            // Operace
            if (!string.IsNullOrWhiteSpace(operace))
            {
                vysledek = vysledek.Where(o =>
                    o.Operace != null &&
                    o.Operace.Equals(operace, StringComparison.OrdinalIgnoreCase));
            }

            // Uživatel / role
            if (!string.IsNullOrWhiteSpace(uzivatel))
            {
                vysledek = vysledek.Where(o =>
                    o.Uzivatel != null &&
                    o.Uzivatel.Equals(uzivatel, StringComparison.OrdinalIgnoreCase));
            }

            // Datum nahrání
            if (dpNahrani.SelectedDate != null)
            {
                DateTime datum = dpNahrani.SelectedDate.Value.Date;

                vysledek = vysledek.Where(o =>
                    o.DatumNahrani.Date == datum);
            }

            // Datum změny
            if (dpZmeny.SelectedDate != null)
            {
                DateTime datum = dpZmeny.SelectedDate.Value.Date;

                vysledek = vysledek.Where(o =>
                    o.DatumModifikace.Date == datum);
            }

            return vysledek;
        }

        private void BtnZrusit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
