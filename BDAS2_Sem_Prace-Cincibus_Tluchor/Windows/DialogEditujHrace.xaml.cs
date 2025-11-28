using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Windows;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialogové okno umožňující editaci existujícího hráče
    /// Po načtení předvyplní hodnoty do všech vstupních polí, umožňuje změnit statistiky,
    /// osobní údaje i disciplinární opatření. Po potvrzení uloží změny do databáze
    /// </summary>
    public partial class DialogEditujHrace : Window
    {
        private Hrac editovanyHrac;
        private HraciOkno hraciOkno;
        private string puvodniRodneCislo;

        /// <summary>
        /// Konstruktor dialogu pro editaci hráče
        /// Načte všechny informace z objektu hráče a zobrazí je ve formuláři
        /// </summary>
        /// <param name="editovanyHrac">Hráč, který se má upravit</param>
        /// <param name="hraciOkno">Hlavní okno s tabulkou hráčů, které se má po editaci aktualizovat</param>
        public DialogEditujHrace(Hrac editovanyHrac, HraciOkno hraciOkno)
        {
            InitializeComponent();
            this.editovanyHrac = editovanyHrac;
            this.hraciOkno = hraciOkno;

        // Naplnění seznamu pozic
        cbPozice.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };

            // Načti základní údaje hráče 
            tboxRodneCislo.Text = editovanyHrac.RodneCislo;
            tboxJmeno.Text = editovanyHrac.Jmeno;
            tboxPrijmeni.Text = editovanyHrac.Prijmeni;
            tboxTelCislo.Text = editovanyHrac.TelefonniCislo;
            cbPozice.SelectedItem = editovanyHrac.PoziceNaHristi;
            iudPocetGolu.Value = editovanyHrac.PocetVstrelenychGolu;
            iudPocetZlutychKaret.Value = editovanyHrac.PocetZlutychKaret;
            iudPocetCervenychKaret.Value = editovanyHrac.PocetCervenychKaret;

            // ULOŽÍME PŮVODNÍ RODNÉ ČÍSLO
            puvodniRodneCislo = editovanyHrac.RodneCislo;

            // Načti opatření (pokud má)
            bool maOpatreni = editovanyHrac.DelkaTrestu > 0 || !string.IsNullOrEmpty(editovanyHrac.DuvodOpatreni);

            if (maOpatreni)
            {
                chkMaOpatreni.IsChecked = true;
                spOpatreni.Visibility = Visibility.Visible;

                // Datum opatření
                if (editovanyHrac.DatumOpatreni == DateTime.MinValue)
                    dpDatumOpatreni.SelectedDate = DateTime.Today;
                else
                    dpDatumOpatreni.SelectedDate = editovanyHrac.DatumOpatreni;

                // Délka trestu
                if (editovanyHrac.DelkaTrestu > 0)
                    iudDelkaTrestu.Value = editovanyHrac.DelkaTrestu;
                else
                    iudDelkaTrestu.Value = 1;

                // Důvod
                tboxDuvodOpatreni.Text = editovanyHrac.DuvodOpatreni;
            }
            else
            {
                chkMaOpatreni.IsChecked = false;
                spOpatreni.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Zobrazí panel pro disciplinární opatření
        /// pokud uživatel zaškrtne políčko "Má opatření"
        /// </summary>
        private void chkMaOpatreni_Checked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Skryje panel s disciplinárním opatřením
        /// a resetuje jeho hodnoty, pokud je pole odškrtnuto
        /// </summary>
        private void chkMaOpatreni_Unchecked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Collapsed;
            dpDatumOpatreni.SelectedDate = null;
            iudDelkaTrestu.Value = 1;
            tboxDuvodOpatreni.Clear();
        }

        /// <summary>
        /// Uloží změny hráče po validaci vstupů
        /// Provádí aktualizaci údajů hráče, disciplinárního opatření
        /// a zápis změn do databáze přes uloženou proceduru
        /// </summary>
        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validace
                string rodneCislo = tboxRodneCislo.Text.Trim();
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();

                string pozice = "";
                if (cbPozice.SelectedItem != null)
                {
                    pozice = cbPozice.SelectedItem.ToString();
                }

                // Použití centralizovaných validačních metod
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);

                Validator.ValidujCeleCislo(iudPocetGolu.Value.ToString(), "Počet gólů");
                Validator.ValidujCeleCislo(iudPocetZlutychKaret.Value.ToString(), "Počet žlutých karet");
                Validator.ValidujCeleCislo(iudPocetCervenychKaret.Value.ToString(), "Počet červených karet");

                // Zjištění, zda má hráč disciplinární opatření
                bool maOpatreni = chkMaOpatreni.IsChecked == true;

                if (maOpatreni)
                {
                    // Validace opatření
                    Validator.ValidujDatum(dpDatumOpatreni.SelectedDate, "Datum opatření");
                    Validator.ValidujCeleCislo(iudDelkaTrestu.Value.ToString(), "Délka trestu");

                    if (string.IsNullOrWhiteSpace(tboxDuvodOpatreni.Text))
                    {
                        throw new Exception("Důvod opatření nesmí být prázdný.");
                    }
                }

                // Přepsání hodnot
                editovanyHrac.RodneCislo = rodneCislo;
                editovanyHrac.Jmeno = jmeno;
                editovanyHrac.Prijmeni = prijmeni;
                editovanyHrac.TelefonniCislo = telCislo;
                editovanyHrac.PoziceNaHristi = pozice;
                editovanyHrac.PocetVstrelenychGolu = (int)iudPocetGolu.Value;
                editovanyHrac.PocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                editovanyHrac.PocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

                // Zpracování disciplinárního opatření
                if (maOpatreni)
                {
                    editovanyHrac.DatumOpatreni = dpDatumOpatreni.SelectedDate.Value;
                    editovanyHrac.DelkaTrestu = (int)iudDelkaTrestu.Value;
                    editovanyHrac.DuvodOpatreni = tboxDuvodOpatreni.Text.Trim();
                    editovanyHrac.DatumOpatreniText = editovanyHrac.DatumOpatreni.ToString("dd.MM.yyyy");
                }
                else
                {
                    editovanyHrac.DatumOpatreni = DateTime.MinValue;
                    editovanyHrac.DelkaTrestu = 0;
                    editovanyHrac.DuvodOpatreni = null;
                    editovanyHrac.DatumOpatreniText = "Bez opatření";
                }

                // Uložení do databáze
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseHraci.UpdateHrac(conn, editovanyHrac, puvodniRodneCislo);
                }

                // Aktualizace tabulky
                hraciOkno.dgHraci.Items.Refresh();

                MessageBox.Show("Změny byly úspěšně uloženy", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání hráče:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
