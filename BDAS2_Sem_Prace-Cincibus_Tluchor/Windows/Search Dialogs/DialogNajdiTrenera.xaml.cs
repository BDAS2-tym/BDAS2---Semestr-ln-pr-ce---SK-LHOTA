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
    /// Dialogové okno sloužící k filtrování trenérů podle různých kritérií
    /// </summary>
    public partial class DialogNajdiTrenera : Window
    {
        private ObservableCollection<Trener> treneriData;

        /// <summary>
        /// Výsledek filtrování trenérů
        /// </summary>
        public IEnumerable<Trener> VyfiltrovaniTreneri { get; private set; }

        public DialogNajdiTrenera(ObservableCollection<Trener> treneriData)
        {
            InitializeComponent();
            this.treneriData = treneriData;
        }

        /// <summary>
        /// Resetuje všechny vstupní prvky
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tRodneCislo.Text = "";
            tJmeno.Text = "";
            tPrijmeni.Text = "";
            tTelefon.Text = "";
            tLicence.Text = "";
            tSpecializace.Text = "";
            tPraxe.Text = "";
        }

        /// <summary>
        /// Spustí filtraci trenérů a pošle výsledek zpět do hlavního okna
        /// </summary>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaniTreneri = FiltrujTrenery();
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
        /// Hlavní metoda pro filtrování trenérů podle zadaných vstupů
        /// </summary>
        private IEnumerable<Trener> FiltrujTrenery()
        {
            IEnumerable<Trener> vysledek = treneriData;

            string rodneCislo = tRodneCislo.Text;
            string jmeno = tJmeno.Text;
            string prijmeni = tPrijmeni.Text;
            string telefon = tTelefon.Text;
            string licence = tLicence.Text;
            string specializace = tSpecializace.Text;
            string praxeText = tPraxe.Text;

            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                // Musí mít přesně 10 číslic
                if (rodneCislo.Length != 10) 
                {
                    throw new NonValidDataException("Rodné číslo musí mít přesně 10 číslic");
                }

                // Musí být jen číslice
                if (!rodneCislo.All(char.IsDigit)) 
                {
                    throw new NonValidDataException("Rodné číslo může obsahovat pouze číslice");
                }

                // Filtr pro rodné číslo
                vysledek = vysledek.Where(t => t.RodneCislo.ToString() == rodneCislo);
            }
   

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Jmeno != null &&
                           t.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Prijmeni != null &&
                           t.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Telefonní číslo
            if (!string.IsNullOrWhiteSpace(telefon))
            {
                if (telefon.Any(c => !char.IsDigit(c)))
                {
                    throw new NonValidDataException("Telefon může obsahovat pouze číslice");
                }

                if (telefon.Length > 12)
                {
                    throw new NonValidDataException("Telefonní číslo může mít maximálně 12 číslic");
                }

                vysledek = vysledek.Where(t =>
                {
                    return t.TelefonniCislo != null &&
                           t.TelefonniCislo.Contains(telefon);
                });
            }

            // Trenérská licence
            if (!string.IsNullOrWhiteSpace(licence))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.TrenerskaLicence != null &&
                           t.TrenerskaLicence.Contains(licence, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Specializace
            if (!string.IsNullOrWhiteSpace(specializace))
            {
                vysledek = vysledek.Where(t =>
                {
                    return t.Specializace != null &&
                           t.Specializace.Contains(specializace, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Počet let praxe
            if (!string.IsNullOrWhiteSpace(praxeText))
            {
                int praxe;

                if (!int.TryParse(praxeText, out praxe) || praxe < 0)
                {
                    throw new NonValidDataException("Počet let praxe musí být nezáporné číslo");
                }

                vysledek = vysledek.Where(t =>
                {
                    return t.PocetLetPraxe == praxe;
                });
            }

            return vysledek;
        }
    }
}
