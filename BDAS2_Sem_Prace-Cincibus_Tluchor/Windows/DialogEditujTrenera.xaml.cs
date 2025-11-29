using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Dialog pro editaci trenéra
    /// Načte údaje trenéra do formuláře a umožní jejich úpravu
    /// Po potvrzení provede validaci a uloží změny do databáze
    /// </summary>
    public partial class DialogEditujTrenera : Window
    {
        private Trener editovanyTrener;
        private TreneriOkno treneriOkno;
        private string puvodniRodneCislo;

        /// <summary>
        /// Inicializuje dialog s předvyplněnými údaji trenéra
        /// </summary>
        public DialogEditujTrenera(Trener trener, TreneriOkno okno)
        {
            InitializeComponent();

            editovanyTrener = trener;
            treneriOkno = okno;

            puvodniRodneCislo = trener.RodneCislo;
            tboxRodneCislo.Text = trener.RodneCislo;
            tboxJmeno.Text = trener.Jmeno;
            tboxPrijmeni.Text = trener.Prijmeni;
            tboxTelCislo.Text = trener.TelefonniCislo;
            tboxLicence.Text = trener.TrenerskaLicence;
            tboxSpecializace.Text = trener.Specializace;
            iudPraxe.Value = trener.PocetLetPraxe;
        }

        /// <summary>
        /// Zavře dialog a vrátí uživatele zpět do hlavního okna
        /// </summary>
        private void BtnUkonci_Click(object sender, RoutedEventArgs e)
        {
            HlavniOkno hlavniOkno = new HlavniOkno();
            hlavniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Zpracuje úpravu trenéra
        /// Provádí validaci vstupů, update trenéra a uložení do databáze
        /// </summary>
        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vstupy z formuláře
                string rodneCislo = tboxRodneCislo.Text.Trim();
                string jmeno = tboxJmeno.Text.Trim();
                string prijmeni = tboxPrijmeni.Text.Trim();
                string telCislo = tboxTelCislo.Text.Trim();
                string licence = tboxLicence.Text.Trim();
                string specializace = tboxSpecializace.Text.Trim();
                string praxeText = iudPraxe.Value.ToString();

                // Validace vstupů
                Validator.ValidujRodneCislo(rodneCislo);
                Validator.ValidujJmeno(jmeno);
                Validator.ValidujPrijmeni(prijmeni);
                Validator.ValidujTelefon(telCislo);
                Validator.ValidujTrenerskouLicenci(licence);
                Validator.ValidujPocetLetPraxeTrenera(praxeText);
                Validator.ValidujSpecializaciTrenera(specializace);

                int praxe = int.Parse(praxeText);

                // Uložení do objektu editovaného trenéra
                editovanyTrener.RodneCislo = rodneCislo;
                editovanyTrener.Jmeno = jmeno;
                editovanyTrener.Prijmeni = prijmeni;
                editovanyTrener.TelefonniCislo = telCislo;
                editovanyTrener.TrenerskaLicence = licence;

                if (string.IsNullOrWhiteSpace(specializace))
                {
                    editovanyTrener.Specializace = null;
                }
                else
                {
                    editovanyTrener.Specializace = specializace;
                }

                editovanyTrener.PocetLetPraxe = praxe;

                // zápis do databáze
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseTreneri.UpdateTrener(conn, editovanyTrener, puvodniRodneCislo);
                }

                // Aktualizace datagridu
                treneriOkno.dgTreneri.Items.Refresh();

                MessageBox.Show("Úprava byla úspěšně provedena", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při úpravě trenéra\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
