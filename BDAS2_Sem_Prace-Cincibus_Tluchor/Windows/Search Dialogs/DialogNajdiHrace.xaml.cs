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
    /// Dialogové okno sloužící k filtrování hráčů podle různých kritérií
    /// </summary>
    public partial class DialogNajdiHrace : Window
    {
        private ObservableCollection<Hrac> hraciData;
        private List<string> pozice = new List<string>();

        /// <summary>
        /// Výsledek filtrování
        /// </summary>
        public IEnumerable<Hrac> VyfiltrovaniHraci { get; private set; }

        public DialogNajdiHrace(ObservableCollection<Hrac> hraciData)
        {
            InitializeComponent();
            this.hraciData = hraciData;
            NaplnCbPozice();
        }

        /// <summary>
        /// Resetuje celý formulář
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            cbPozice.SelectedItem = null;
            tRodneCislo.Text = "";
            tJmeno.Text = "";
            tPrijmeni.Text = "";
            tTelefon.Text = "";
            tGoly.Text = "";
            tZlute.Text = "";
            tCervene.Text = "";
            dpOpatreni.SelectedDate = null;
            tDuvod.Text = "";
            tDelkaTrestu.Text = "";
        }

        /// <summary>
        /// Potvrdí filtraci a vrátí výsledky do hlavního okna
        /// </summary>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VyfiltrovaniHraci = FiltrujHrace();
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
        /// Naplní ComboBox s pozicemi hráčů
        /// </summary>
        private void NaplnCbPozice()
        {
            pozice = new List<string>
            {
                "Brankář",
                "Obránce",
                "Záložník",
                "Útočník"
            };

            cbPozice.ItemsSource = pozice;
        }

        /// <summary>
        /// Provede filtrování hráčů dle zadaných kritérií
        /// </summary>
        private IEnumerable<Hrac> FiltrujHrace()
        {
            IEnumerable<Hrac> vysledek = hraciData;

            string jmeno = tJmeno.Text;
            string prijmeni = tPrijmeni.Text;
            string rodneCislo = tRodneCislo.Text;
            string telefonCislo = tTelefon.Text;

            string vybranaPozice = null;
            if (cbPozice.SelectedItem != null)
            {
                vybranaPozice = cbPozice.SelectedItem.ToString();
            }

            // Rodné číslo
            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                // Validace rodného čísla
                Validator.ValidujRodneCislo(rodneCislo);

                vysledek = vysledek.Where(h =>
                {
                    return h.RodneCislo != null && h.RodneCislo == rodneCislo;
                });
            }

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                // Validace jména
                Validator.ValidujJmeno(jmeno);

                vysledek = vysledek.Where(h =>
                {
                    return h.Jmeno != null &&
                           h.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                Validator.ValidujPrijmeni(prijmeni);

                vysledek = vysledek.Where(h =>
                {
                    return h.Prijmeni != null &&
                           h.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Telefonní číslo
            if (!string.IsNullOrWhiteSpace(telefonCislo))
            {
                Validator.ValidujTelefon(telefonCislo, 1, 12);

                vysledek = vysledek.Where(h =>
                {
                    return h.TelefonniCislo != null &&
                           h.TelefonniCislo.Contains(telefonCislo);
                });
            }

            // Počet gólů
            if (!string.IsNullOrWhiteSpace(tGoly.Text))
            {
                Validator.ValidujCeleCislo(tGoly.Text, "Počet gólů");

                int goly = int.Parse(tGoly.Text);

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetVstrelenychGolu == goly;
                });
            }

            // Žluté karty
            if (!string.IsNullOrWhiteSpace(tZlute.Text))
            {
                Validator.ValidujCeleCislo(tZlute.Text, "Počet žlutých karet");

                int zlute = int.Parse(tZlute.Text);

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetZlutychKaret == zlute;
                });
            }

            // Červené karty
            if (!string.IsNullOrWhiteSpace(tCervene.Text))
            {
                Validator.ValidujCeleCislo(tCervene.Text, "Počet červených karet");

                int cervene = int.Parse(tCervene.Text);

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetCervenychKaret == cervene;
                });
            }

            // Pozice
            if (!string.IsNullOrWhiteSpace(vybranaPozice))
            {
                vysledek = vysledek.Where(h =>
                {
                    return h.PoziceNaHristi == vybranaPozice;
                });
            }

            // Datum opatření
            if (dpOpatreni.SelectedDate != null)
            {
                Validator.ValidujDatum(dpOpatreni.SelectedDate, "Datum opatření");

                string datumVybrane = dpOpatreni.SelectedDate.Value.ToString("dd.MM.yyyy");

                vysledek = vysledek.Where(h =>
                {
                    return h.DatumOpatreniText != null &&
                           h.DatumOpatreniText != "Bez opatření" &&
                           h.DatumOpatreniText == datumVybrane;
                });
            }

            // Důvod opatření
            if (!string.IsNullOrWhiteSpace(tDuvod.Text))
            {
                string duvod = tDuvod.Text;

                vysledek = vysledek.Where(h =>
                {
                    return h.DuvodOpatreni != null &&
                           h.DuvodOpatreni.Contains(duvod, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Délka trestu
            if (!string.IsNullOrWhiteSpace(tDelkaTrestu.Text))
            {
                Validator.ValidujCeleCislo(tDelkaTrestu.Text, "Délka trestu");

                int delka = int.Parse(tDelkaTrestu.Text);

                vysledek = vysledek.Where(h =>
                {
                    return h.DelkaTrestu == delka;
                });
            }

            return vysledek;
        }

    }
}
