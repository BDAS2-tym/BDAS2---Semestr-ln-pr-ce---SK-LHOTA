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

            // Zjištění role s kontrolou null
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();
            string role = "host"; // fallback

            if (uzivatel != null && uzivatel.Role != null)
            {
                role = uzivatel.Role.ToLower();
            }

            // Pokud není admin - zakázat filtrování/zadání rodného čísla
            if (role != "admin")
            {
                tRodneCislo.IsEnabled = false;
                tRodneCislo.Opacity = 0.5;
                tRodneCislo.ToolTip = "Pouze admin může filtrovat podle rodného čísla";

                tTelefon.IsEnabled = false;
                tTelefon.Opacity = 0.5;
                tTelefon.ToolTip = "Pouze admin může filtrovat podle telefonního čísla";
            }
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

            string rodneCislo = tRodneCislo.Text.Trim();
            string jmeno = tJmeno.Text.Trim();
            string prijmeni = tPrijmeni.Text.Trim();
            string telefon = tTelefon.Text.Trim();
            string licence = tLicence.Text.Trim();
            string specializace = tSpecializace.Text.Trim();
            string praxeText = tPraxe.Text.Trim();

            // Rodné číslo
            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                Validator.ValidujRodneCislo(rodneCislo);

                vysledek = vysledek.Where(t =>
                    t.RodneCislo != null &&
                    t.RodneCislo == rodneCislo);
            }

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                Validator.ValidujJmeno(jmeno);

                vysledek = vysledek.Where(t =>
                    t.Jmeno != null &&
                    t.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase));
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                Validator.ValidujPrijmeni(prijmeni);

                vysledek = vysledek.Where(t =>
                    t.Prijmeni != null &&
                    t.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase));
            }

            // Telefon
            if (!string.IsNullOrWhiteSpace(telefon))
            {
                Validator.ValidujTelefon(telefon);

                vysledek = vysledek.Where(t =>
                    t.TelefonniCislo != null &&
                    t.TelefonniCislo.Contains(telefon));
            }

            // Licence (pouze validace délky)
            if (!string.IsNullOrWhiteSpace(licence))
            {
                Validator.ValidujTrenerskouLicenci(licence);

                vysledek = vysledek.Where(t =>
                    t.TrenerskaLicence != null &&
                    t.TrenerskaLicence.Contains(licence, StringComparison.OrdinalIgnoreCase));
            }

            // Specializace (nepovinné, jen pokud vyplněno)
            if (!string.IsNullOrWhiteSpace(specializace))
            {
                Validator.ValidujSpecializaciTrenera(specializace);

                vysledek = vysledek.Where(t =>
                    t.Specializace != null &&
                    t.Specializace.Contains(specializace, StringComparison.OrdinalIgnoreCase));
            }

            // Praxe
            if (!string.IsNullOrWhiteSpace(praxeText))
            {
                Validator.ValidujPocetLetPraxeTrenera(praxeText);

                int praxe = int.Parse(praxeText);

                vysledek = vysledek.Where(t => t.PocetLetPraxe == praxe);
            }

            return vysledek;
        }

    }
}
