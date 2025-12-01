using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs
{
    /// <summary>
    /// Dialog sloužící k filtrování uživatelských účtů podle zadaných kritérií.
    /// Filtrování probíhá nad předanou kolekcí v paměti.
    /// </summary>
    public partial class DialogNajdiUzivatelskeUcty : Window
    {
        private readonly ObservableCollection<Uzivatel> uzivateleData;

        /// <summary>
        /// Kolekce vyfiltrovaných uživatelů, vrácená volajícímu oknu.
        /// </summary>
        public IEnumerable<Uzivatel> VyfiltrovaniUzivatele { get; private set; }

        /// <summary>
        /// Inicializuje dialog a uloží kolekci, nad kterou se bude filtrovat.
        /// </summary>
        public DialogNajdiUzivatelskeUcty(ObservableCollection<Uzivatel> uzivatele)
        {
            InitializeComponent();
            uzivateleData = uzivatele;
        }

        /// <summary>
        /// Resetuje všechny vstupní ovládací prvky do výchozího stavu.
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            tJmeno.Text = string.Empty;
            tEmail.Text = string.Empty;
            cbRole.SelectedIndex = -1;
            tRodneCislo.Text = string.Empty;
            dpPrihlaseniDatum.SelectedDate = null;
        }

        /// <summary>
        /// Provede filtrování dat a zavře dialog s výsledkem.
        /// </summary>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaniUzivatele = FiltrujUzivatele();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba filtrování", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hlavní metoda, která provede jednotlivé kroky filtrování nad dostupnými daty.
        /// </summary>
        private IEnumerable<Uzivatel> FiltrujUzivatele()
        {
            IEnumerable<Uzivatel> vysledek = uzivateleData;

            string jmeno = tJmeno.Text.Trim();
            string email = tEmail.Text.Trim();
            string rodne = tRodneCislo.Text.Trim();

            // Získání role z ComboBoxu
            string role = string.Empty;
            if (cbRole.SelectedItem is ComboBoxItem itemRole)
                role = itemRole.Content.ToString().Trim();

            // Získání data posledního přihlášení
            DateTime? datumPrihlaseni = dpPrihlaseniDatum.SelectedDate;

            // Filtrování podle uživatelského jména
            if (jmeno.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.UzivatelskeJmeno != null &&
                    u.UzivatelskeJmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrování podle e-mailu
            if (email.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.Email != null &&
                    u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrování podle role
            if (role.Length > 0)
            {
                vysledek = vysledek.Where(u =>
                    u.Role != null &&
                    u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrování podle rodného čísla
            if (rodne.Length > 0)
            {
                if (rodne.Length != 10 || rodne.Any(ch => !Char.IsDigit(ch)))
                {
                    throw new Exception("Rodné číslo musí mít přesně 10 číslic.");
                }

                vysledek = vysledek.Where(u =>
                    u.RodneCislo != null &&
                    u.RodneCislo == rodne);
            }

            // Filtrování podle data posledního přihlášení
            if (datumPrihlaseni.HasValue)
            {
                DateTime hledaneDatum = datumPrihlaseni.Value.Date;

                vysledek = vysledek.Where(u =>
                    u.PosledniPrihlaseni.Date == hledaneDatum);
            }

            return vysledek;
        }
    }
}
