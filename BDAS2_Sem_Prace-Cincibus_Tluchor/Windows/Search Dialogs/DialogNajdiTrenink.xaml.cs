using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Dialog sloužící k vyhledávání tréninků
    /// </summary>
    public partial class DialogNajdiTrenink : Window
    {
        private ObservableCollection<TreninkView> treninkyData;

        /// <summary>
        /// Jedná se o kolekci tréninků, které odpovídají filtrům
        /// </summary>
        public IEnumerable<TreninkView> VyfiltrovaneTreninky { get; private set; }

        public DialogNajdiTrenink(ObservableCollection<TreninkView> treninkyData)
        {
            InitializeComponent();
            this.treninkyData = treninkyData;
        }

        /// <summary>
        /// Resetuje všechny vstupní prvky
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tRodneCislo.Text = "";
            tPrijmeni.Text = "";
            dpDatum.SelectedDate = null;
            tCas.Text = "";
            tMisto.Text = "";
            tPopis.Text = "";
        }

        /// <summary>
        /// Spustí filtrování
        /// </summary>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaneTreninky = FiltrujTreninky();
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
        /// Filtrace tréninků podle vstupů z formuláře
        /// </summary>
        private IEnumerable<TreninkView> FiltrujTreninky()
        {
            IEnumerable<TreninkView> vysledek = treninkyData;

            string rodneCislo = tRodneCislo.Text;
            string prijmeni = tPrijmeni.Text;
            string casText = tCas.Text;
            string misto = tMisto.Text;
            string popis = tPopis.Text;

            // Validace rodné číslo
            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                if (rodneCislo.Length != 10)
                {
                    throw new NonValidDataException("Rodné číslo musí mít přesně 10 číslic");
                }

                if (!rodneCislo.All(char.IsDigit))
                {
                    throw new NonValidDataException("Rodné číslo může obsahovat pouze číslice");
                }

                vysledek = vysledek.Where(t => t.RodneCislo.ToString() == rodneCislo);
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                if (!prijmeni.All(char.IsLetter))
                {
                    throw new NonValidDataException("Příjmení může obsahovat pouze písmena");
                }
                    
                vysledek = vysledek.Where(t =>
                    t.Prijmeni != null &&
                    t.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Datum
            if (dpDatum.SelectedDate != null)
            {
                DateTime d = dpDatum.SelectedDate.Value.Date;

                vysledek = vysledek.Where(t =>
                {
                    return t.Datum.Date == d;
                });
            }

            // Čas
            if (!string.IsNullOrWhiteSpace(casText))
            {
                // validace HH:mm
                TimeSpan cas;

                if (!TimeSpan.TryParseExact(casText, "hh\\:mm", null, out cas))
                {
                    throw new NonValidDataException("Čas musí být ve formátu HH:mm");
                }

                vysledek = vysledek.Where(t =>
                {
                    return t.Datum.ToString("HH:mm") == casText;
                });
            }

            // Místo
            if (!string.IsNullOrWhiteSpace(misto))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Misto != null &&
                           t.Misto.Contains(misto, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Popis
            if (!string.IsNullOrWhiteSpace(popis))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Popis != null &&
                           t.Popis.Contains(popis, StringComparison.OrdinalIgnoreCase);
                });
            }

            return vysledek;
        }
    }
}
