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
            string vybranaPozice = cbPozice.SelectedItem as string;

            // -----------------------------
            // Rodné číslo
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(rodneCislo))
            {
                // TryParse používáme na ověření, že je to číslo
                // Parametr "out _" znamená, že výsledek převodu zahazujeme
                if (rodneCislo.Length != 10 || !long.TryParse(rodneCislo, out _))
                {
                    throw new NonValidDataException("Rodné číslo musí mít přesně 10 číslic");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.RodneCislo.ToString() == rodneCislo;
                });
            }

            // Jméno
            if (!string.IsNullOrWhiteSpace(jmeno))
            {
                vysledek = vysledek.Where(h =>
                {
                    return h.Jmeno != null &&
                           h.Jmeno.Contains(jmeno, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Příjmení
            if (!string.IsNullOrWhiteSpace(prijmeni))
            {
                vysledek = vysledek.Where(h =>
                {
                    return h.Prijmeni != null &&
                           h.Prijmeni.Contains(prijmeni, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Telefonní číslo
            if (!string.IsNullOrWhiteSpace(telefonCislo))
            {
                //Obsahuje telefonní číslo nějaký znak, který NENÍ číslice
                if (telefonCislo.Any(c => !char.IsDigit(c)))
                {
                    throw new NonValidDataException("Telefon může obsahovat pouze číslice");
                }

                if (telefonCislo.Length > 12)
                {
                    throw new NonValidDataException("Telefonní číslo může mít maximálně 12 číslic");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.TelefonniCislo != null &&
                           h.TelefonniCislo.Contains(telefonCislo);
                });
            }

            // Počet gólů
            if (!string.IsNullOrWhiteSpace(tGoly.Text))
            {
                int goly;

                if (!int.TryParse(tGoly.Text, out goly) || goly < 0)
                {
                    throw new NonValidDataException("Počet gólů musí být nezáporné číslo");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetVstrelenychGolu == goly;
                });
            }

            // Žluté karty
            if (!string.IsNullOrWhiteSpace(tZlute.Text))
            {
                int zlute;

                if (!int.TryParse(tZlute.Text, out zlute) || zlute < 0)
                {
                    throw new NonValidDataException("Počet žlutých karet musí být nezáporné číslo");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetZlutychKaret == zlute;
                });
            }

            // Červené karty
            if (!string.IsNullOrWhiteSpace(tCervene.Text))
            {
                int cerveneKarty;

                if (!int.TryParse(tCervene.Text, out cerveneKarty) || cerveneKarty < 0)
                {
                    throw new NonValidDataException("Počet červených karet musí být nezáporné číslo");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.PocetCervenychKaret == cerveneKarty;
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
                string datumVybrane = dpOpatreni.SelectedDate.Value.ToString("dd.MM.yyyy");

                vysledek = vysledek.Where(h =>
                {
                    return h.DatumOpatreniText != null && h.DatumOpatreniText != "Bez opatření" && h.DatumOpatreniText == datumVybrane;
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
                int delka;

                if (!int.TryParse(tDelkaTrestu.Text, out delka) || delka < 0)
                {
                    throw new NonValidDataException("Délka trestu musí být nezáporné číslo");
                }

                vysledek = vysledek.Where(h =>
                {
                    return h.DelkaTrestu == delka;
                });
            }

            return vysledek;
        }
    }
}
