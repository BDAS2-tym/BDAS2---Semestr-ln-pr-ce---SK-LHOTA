using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro správu uživatelských účtů
    /// Zobrazuje přehled všech uživatelů a umožňuje přidávat, upravovat, mazat a emulovat účty
    /// </summary>
    public partial class NastaveniUzivateleOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;
        private bool zavrenoTlacitkem = false;

        /// <summary>
        /// Kolekce uživatelů pro DataGrid
        /// </summary>
        private static ObservableCollection<Uzivatel> UzivateleData = new ObservableCollection<Uzivatel>();

        /// <summary>
        /// Konstruktor – načte data po otevření okna
        /// </summary>
        public NastaveniUzivateleOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
            NactiUzivatele();
            DataContext = this;
            NastavPrava();
        }

        /// <summary>
        /// Načte uživatele z databáze a naplní DataGrid
        /// </summary>
        private void NactiUzivatele()
        {
            try
            {
                UzivateleData.Clear();

                OracleConnection conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("SELECT * FROM PREHLED_UZIVATELSKE_UCTY", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Uzivatel uzivatel = new Uzivatel();

                        if (reader["UZIVATELSKEJMENO"] != DBNull.Value)
                            uzivatel.UzivatelskeJmeno = reader["UZIVATELSKEJMENO"].ToString();
                        else
                            uzivatel.UzivatelskeJmeno = "";

                        if (reader["ROLE"] != DBNull.Value)
                            uzivatel.Role = reader["ROLE"].ToString();
                        else
                            uzivatel.Role = "";

                        if (reader["EMAIL"] != DBNull.Value)
                            uzivatel.Email = reader["EMAIL"].ToString();
                        else
                            uzivatel.Email = "";

                        if (reader["RODNE_CISLO"] != DBNull.Value)
                            uzivatel.RodneCislo = reader["RODNE_CISLO"].ToString();
                        else
                            uzivatel.RodneCislo = "";

                        if (reader["POSLEDNIPRIHLASENI"] != DBNull.Value)
                            uzivatel.PosledniPrihlaseni = Convert.ToDateTime(reader["POSLEDNIPRIHLASENI"]);
                        else
                            uzivatel.PosledniPrihlaseni = DateTime.MinValue;

                        UzivateleData.Add(uzivatel);
                    }
                }

                dgUzivatele.ItemsSource = UzivateleData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání uživatelů:\n{ex.Message}",
                    "Chyba databáze", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Přepne přihlášení na jiného uživatele (emulace účtu)
        /// </summary>
        private void BtnPrepnout_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Uzivatel uzivatel = (Uzivatel)btn.DataContext;

            var potvrzeni = MessageBox.Show($"Opravdu se chcete přepnout na účet: {uzivatel.UzivatelskeJmeno}?", "Potvrzení přepnutí",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            HlavniOkno.NastavPrihlaseneho(uzivatel);

            MessageBox.Show(
                $"Nyní jste přihlášen jako: {uzivatel.UzivatelskeJmeno} ({uzivatel.Role})",
                "Úspěšné přepnutí",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            HlavniOkno noveOkno = new HlavniOkno();
            noveOkno.Show();

            if (hlavniOkno != null)
            {
                hlavniOkno.Close();
            }

            zavrenoTlacitkem = true;
            this.Close();
        }

        /// <summary>
        /// Otevře okno pro registraci nového uživatele
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            RegistraceOkno registraceOkno = new RegistraceOkno();
            registraceOkno.ShowDialog();
            NactiUzivatele();
        }

        /// <summary>
        /// Odebere vybraného uživatele z databáze
        /// Kontroluje také, aby nebyl odstraněn právě přihlášený (emulovaný) účet
        /// </summary>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Uzivatel vybranyUzivatel = dgUzivatele.SelectedItem as Uzivatel;

            if (vybranyUzivatel == null)
            {
                MessageBox.Show("Vyberte uživatele, kterého chcete odebrat", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Uzivatel prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();

            if (prihlaseny != null &&
                vybranyUzivatel.UzivatelskeJmeno == prihlaseny.UzivatelskeJmeno)
            {
                MessageBox.Show(
                    "Nelze odstranit účet, který je právě aktivní (emulovaný)!",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error
                );
                return;
            }

            var potvrzeni = MessageBox.Show(
                $"Opravdu chcete odebrat uživatele {vybranyUzivatel.UzivatelskeJmeno}?",
                "Potvrzení odstranění",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                DatabaseRegistrace.DeleteUzivatel(conn, vybranyUzivatel);

                UzivateleData.Remove(vybranyUzivatel);

                MessageBox.Show(
                    $"Uživatel {vybranyUzivatel.UzivatelskeJmeno} byl úspěšně odebrán.",
                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chyba při odstraňování uživatele:\n{ex.Message}",
                    "Chyba databáze",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnZpet_Click(object sender, System.EventArgs e)
        {
            zavrenoTlacitkem = true;
            NastaveniOkno.Instance.Show();
            this.Hide();
        }

        private void BtnEdituj_Click(object sender, RoutedEventArgs e)
        {
            Uzivatel vybrany = dgUzivatele.SelectedItem as Uzivatel;

            if (vybrany == null)
            {
                MessageBox.Show("Vyberte uživatele, kterého chcete upravit.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EditUzivatelOkno okno = new EditUzivatelOkno(vybrany);
            bool? vysledek = okno.ShowDialog();

            if (vysledek == true)
            {
                NactiUzivatele();
                MessageBox.Show("Změny byly úspěšně uloženy.", "Aktualizace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DgTreninky_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                MessageBox.Show("Smazání uživatele klávesou Delete není povoleno", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (e.Key == Key.Space)
            {
                dgUzivatele.UnselectAll();
                dgUzivatele.Focusable = false;
                Keyboard.ClearFocus();
                dgUzivatele.Focusable = true;
            }
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiUzivatelskeUcty dialog = new DialogNajdiUzivatelskeUcty(UzivateleData);

            bool? vysledek = dialog.ShowDialog();

            if (vysledek == true)
            {
                if (!dialog.VyfiltrovaniUzivatele.Any())
                {
                    MessageBox.Show("Nenašly se žádné záznamy",
                        "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show(
                    "Vyhledávací mód je aktivní. Pro návrat stiskněte CTRL + X",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                dgUzivatele.ItemsSource = new ObservableCollection<Uzivatel>(dialog.VyfiltrovaniUzivatele);
                jeVyhledavaniAktivni = true;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (jeVyhledavaniAktivni && Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                dgUzivatele.ItemsSource = UzivateleData;
                jeVyhledavaniAktivni = false;
                e.Handled = true;
            }
        }

        private void BtnNotifikace_Click(object sender, RoutedEventArgs e)
        {
            DialogZpravaOkno okno = new DialogZpravaOkno(UzivateleData);
            okno.ShowDialog();
        }

        private void NastavPrava()
        {
            Uzivatel uzivatel = HlavniOkno.GetPrihlasenyUzivatel();

            string role;

            if (uzivatel != null && uzivatel.Role != null)
            {
                role = uzivatel.Role.ToLower();
            }
            else
            {
                role = "host";
            }

            foreach (var sloupec in dgUzivatele.Columns)
            {
                sloupec.Visibility = Visibility.Visible;
            }

            if (role == "admin")
            {
                return;
            }

            foreach (var sloupec in dgUzivatele.Columns)
            {
                if (sloupec.Header != null)
                {
                    string header = sloupec.Header.ToString().ToLower();

                    if (header.Contains("rodné") || header.Contains("rodne"))
                    {
                        sloupec.Visibility = Visibility.Collapsed;
                    }

                    if (header.Contains("akce"))
                    {
                        sloupec.Visibility = Visibility.Collapsed;
                    }
                }
            }

            if (role == "trener")
            {
                btnPridej.IsEnabled = false;
                btnEdituj.IsEnabled = false;
                btnOdeber.IsEnabled = false;
                btnNajdi.IsEnabled = false;
                btnNotifikace.IsEnabled = true;

                btnPridej.Opacity = 0.2;
                btnEdituj.Opacity = 0.2;
                btnOdeber.Opacity = 0.2;
                btnNajdi.Opacity = 0.2;

                return;
            }

            btnPridej.IsEnabled = false;
            btnEdituj.IsEnabled = false;
            btnOdeber.IsEnabled = false;
            btnNajdi.IsEnabled = false;
            btnNotifikace.IsEnabled = false;

            btnPridej.Opacity = 0.2;
            btnEdituj.Opacity = 0.2;
            btnOdeber.Opacity = 0.2;
            btnNajdi.Opacity = 0.2;
            btnNotifikace.Opacity = 0.2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!zavrenoTlacitkem)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
