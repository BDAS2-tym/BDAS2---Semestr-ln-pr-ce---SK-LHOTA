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
        private HlavniOkno hlavniOkno;
        private bool jeVyhledavaniAktivni = false;

        /// <summary>
        /// Kolekce uživatelů pro DataGrid
        /// </summary>
        private static ObservableCollection<Uzivatel> UzivateleData = new ObservableCollection<Uzivatel>();

        /// <summary>
        /// Konstruktor – načte data po otevření okna
        /// </summary>
        public NastaveniUzivateleOkno()
        {
            InitializeComponent();
            NactiUzivatele();
            DataContext = this;
        }

        /// <summary>
        /// Načte uživatele z databáze a naplní DataGrid
        /// </summary>
        private void NactiUzivatele()
        {
            try
            {
                UzivateleData.Clear();

                using var conn = DatabaseManager.GetConnection();
                conn.Open();

                using var cmd = new OracleCommand("SELECT * FROM PREHLED_UZIVATELSKE_UCTY", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Uzivatel uzivatel = new Uzivatel();

                    // Uživatelské jméno
                    if (reader["UZIVATELSKEJMENO"] != DBNull.Value)
                        uzivatel.UzivatelskeJmeno = reader["UZIVATELSKEJMENO"].ToString();
                    else
                        uzivatel.UzivatelskeJmeno = "";

                    // Role
                    if (reader["ROLE"] != DBNull.Value)
                        uzivatel.Role = reader["ROLE"].ToString();
                    else
                        uzivatel.Role = "";

                    // Email
                    if (reader["EMAIL"] != DBNull.Value)
                        uzivatel.Email = reader["EMAIL"].ToString();
                    else
                        uzivatel.Email = "";

                    if (reader["RODNE_CISLO"] != DBNull.Value)
                        uzivatel.RodneCislo = reader["RODNE_CISLO"].ToString();
                    else
                        uzivatel.RodneCislo = "";

                    // Poslední přihlášení
                    if (reader["POSLEDNIPRIHLASENI"] != DBNull.Value)
                        uzivatel.PosledniPrihlaseni = Convert.ToDateTime(reader["POSLEDNIPRIHLASENI"]);
                    else
                        uzivatel.PosledniPrihlaseni = DateTime.MinValue;

                    UzivateleData.Add(uzivatel);
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
            Uzivatel u = (Uzivatel)btn.DataContext;

            var potvrzeni = MessageBox.Show($"Opravdu se chcete přepnout na účet: {u.UzivatelskeJmeno}?", "Potvrzení přepnutí",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

            HlavniOkno.NastavPrihlaseneho(u);

            MessageBox.Show($"Nyní jste přihlášen jako: {u.UzivatelskeJmeno} ({u.Role})", "Úspěšné přepnutí", MessageBoxButton.OK, MessageBoxImage.Information);

            HlavniOkno hlavniOkno = new HlavniOkno();
            hlavniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Otevře okno pro registraci nového uživatele
        /// </summary>
        private void BtnPridej_Click(object sender, RoutedEventArgs e)
        {
            RegistraceOkno registraceOkno = new RegistraceOkno();
            registraceOkno.ShowDialog();
            NactiUzivatele(); // Obnoví seznam
        }

        /// <summary>
        /// Odebere vybraného uživatele z databáze
        /// Kontroluje také, aby nebyl odstraněn právě přihlášený (emulovaný) účet
        /// </summary>
        private void BtnOdeber_Click(object sender, RoutedEventArgs e)
        {
            Uzivatel vybranyUzivatel = dgUzivatele.SelectedItem as Uzivatel;

            // Kontrola výběru
            if (vybranyUzivatel == null)
            {
                MessageBox.Show("Vyberte uživatele, kterého chcete odebrat", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrola, zda se nesnažíš smazat aktuálně emulovaný účet
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

            // Potvrzení smazání
            var potvrzeni = MessageBox.Show(
                $"Opravdu chcete odebrat uživatele {vybranyUzivatel.UzivatelskeJmeno}?",
                "Potvrzení odstranění",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            // Provedení smazání
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();

                    DatabaseAppUser.SetAppUser(conn, HlavniOkno.GetPrihlasenyUzivatel());
                    DatabaseRegistrace.DeleteUzivatel(conn, vybranyUzivatel);

                    UzivateleData.Remove(vybranyUzivatel);
                }

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


        /// <summary>
        /// Metoda slouží k vrácení se na okno nastavení
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Otevře okno pro editaci vybraného uživatele
        /// </summary>
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
                NactiUzivatele(); // Obnoví DataGrid po úpravě
                MessageBox.Show("Změny byly úspěšně uloženy.", "Aktualizace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Metoda slouží k zamezení zmáčknutí klávesy DELETE, aby nešel smazat záznam z datagridu.
        /// Také slouží k zrušení výběru při zmáčknutí klávesy Spacebar
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void DgTreninky_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Zrušení akce mazání
                e.Handled = true;
                MessageBox.Show("Smazání uživatele klávesou Delete není povoleno", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Zrušení výběru řádku při zmáčknutí klávesy Spacebar
            if (e.Key == Key.Space)
            {
                dgUzivatele.UnselectAll();

                // Odstranění Focus Rectangle na dané buňce
                dgUzivatele.Focusable = false;
                Keyboard.ClearFocus();
                dgUzivatele.Focusable = true;
            }
        }

        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiUzivatelskeUcty dialog =
                new DialogNajdiUzivatelskeUcty(UzivateleData);

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


    }
}
