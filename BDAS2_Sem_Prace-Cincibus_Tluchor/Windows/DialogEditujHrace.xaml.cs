using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    public partial class DialogEditujHrace : Window
    {
        private Hrac editovanyHrac;
        private HraciOkno hraciOkno;
        private string puvodniRodneCislo;

        public DialogEditujHrace(Hrac editovanyHrac, HraciOkno hraciOkno)
        {
            InitializeComponent();
            this.editovanyHrac = editovanyHrac;
            this.hraciOkno = hraciOkno;

            List<Pozice> poziceList = new List<Pozice>
            {
                new Pozice { Id = 1, Nazev = "Brankář" },
                new Pozice { Id = 2, Nazev = "Obránce" },
                new Pozice { Id = 3, Nazev = "Záložník" },
                new Pozice { Id = 4, Nazev = "Útočník" }
            };

            cbPozice.ItemsSource = poziceList;
            cbPozice.DisplayMemberPath = "Nazev";
            cbPozice.SelectedValuePath = "Id";
            cbPozice.SelectedIndex = 0;

            tboxRodneCislo.Text = editovanyHrac.RodneCislo;
            tboxJmeno.Text = editovanyHrac.Jmeno;
            tboxPrijmeni.Text = editovanyHrac.Prijmeni;
            tboxTelCislo.Text = editovanyHrac.TelefonniCislo;

            iudPocetGolu.Value = editovanyHrac.PocetVstrelenychGolu;
            iudPocetZlutychKaret.Value = editovanyHrac.PocetZlutychKaret;
            iudPocetCervenychKaret.Value = editovanyHrac.PocetCervenychKaret;

            foreach (Pozice p in poziceList)
            {
                if (p.Nazev == editovanyHrac.PoziceNaHristi)
                {
                    cbPozice.SelectedItem = p;
                    break;
                }
            }

            puvodniRodneCislo = editovanyHrac.RodneCislo;

            bool maOpatreni = editovanyHrac.DelkaTrestu > 0 || !string.IsNullOrEmpty(editovanyHrac.DuvodOpatreni);
            if (maOpatreni)
            {
                chkMaOpatreni.IsChecked = true;
                spOpatreni.Visibility = Visibility.Visible;
                dpDatumOpatreni.SelectedDate = editovanyHrac.DatumOpatreni == DateTime.MinValue ? DateTime.Today : editovanyHrac.DatumOpatreni;
                iudDelkaTrestu.Value = editovanyHrac.DelkaTrestu > 0 ? editovanyHrac.DelkaTrestu : 1;
                tboxDuvodOpatreni.Text = editovanyHrac.DuvodOpatreni;
            }
            else
            {
                chkMaOpatreni.IsChecked = false;
                spOpatreni.Visibility = Visibility.Collapsed;
            }
        }

        private void chkMaOpatreni_Checked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Visible;
        }

        private void chkMaOpatreni_Unchecked(object sender, RoutedEventArgs e)
        {
            spOpatreni.Visibility = Visibility.Collapsed;
            dpDatumOpatreni.SelectedDate = null;
            iudDelkaTrestu.Value = 1;
            tboxDuvodOpatreni.Clear();
        }

        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rodneCislo = tboxRodneCislo.Text.Trim();
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();

                editovanyHrac.IdPozice = (int)cbPozice.SelectedValue;
                editovanyHrac.PoziceNaHristi = ((Pozice)cbPozice.SelectedItem).Nazev;

                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);
                Validator.ValidujCeleCislo(iudPocetGolu.Value.ToString(), "Počet gólů");
                Validator.ValidujCeleCislo(iudPocetZlutychKaret.Value.ToString(), "Počet žlutých karet");
                Validator.ValidujCeleCislo(iudPocetCervenychKaret.Value.ToString(), "Počet červených karet");

                bool maOpatreni = chkMaOpatreni.IsChecked == true;
                if (maOpatreni)
                {
                    Validator.ValidujDatum(dpDatumOpatreni.SelectedDate, "Datum opatření");
                    Validator.ValidujCeleCislo(iudDelkaTrestu.Value.ToString(), "Délka trestu");
                }

                editovanyHrac.RodneCislo = rodneCislo;
                editovanyHrac.Jmeno = jmeno;
                editovanyHrac.Prijmeni = prijmeni;
                editovanyHrac.TelefonniCislo = telCislo;
                editovanyHrac.PocetVstrelenychGolu = (int)iudPocetGolu.Value;
                editovanyHrac.PocetZlutychKaret = (int)iudPocetZlutychKaret.Value;
                editovanyHrac.PocetCervenychKaret = (int)iudPocetCervenychKaret.Value;

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

                var conn = DatabaseManager.GetConnection();
                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseHraci.UpdateHrac(conn, editovanyHrac, puvodniRodneCislo);

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
