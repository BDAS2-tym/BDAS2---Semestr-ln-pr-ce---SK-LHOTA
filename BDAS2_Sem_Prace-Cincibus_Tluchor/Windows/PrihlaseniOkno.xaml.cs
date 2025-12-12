using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro přihlášení uživatele do systému
    /// Umožňuje přihlášení pomocí účtu, registraci nového uživatele nebo vstup bez přihlášení jako host
    /// </summary>
    public partial class PrihlaseniOkno : Window
    {
        private HlavniOkno hlavniOkno;

        /// <summary>
        /// Konstruktor přihlašovacího okna
        /// </summary>
        public PrihlaseniOkno()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Konstruktor přihlašovacího okna
        /// </summary>
        /// <param name="hlavniOkno">Instance hlavního okna </param>
        public PrihlaseniOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();
            this.hlavniOkno = hlavniOkno;
        }

        /// <summary>
        /// Umožní tažení okna levým tlačítkem myši
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Minimalizuje aktuální přihlašovací okno
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Ukončí aplikaci po kliknutí na tlačítko Zavřít 
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Otevře okno první registrace nového uživatele a dostane roli 'Host'
        /// </summary>
        private void BtnRegistrace_Click(object sender, MouseButtonEventArgs e)
        {
            PrvniRegistracniOkno registracniOkno = new PrvniRegistracniOkno();
            registracniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Přihlásí uživatele jako hosta bez registrace a otevře hlavní okno
        /// </summary>
        private void BtnBezPrihlaseni_Click(object sender, RoutedEventArgs e)
        {
            Uzivatel host = new Uzivatel();
            host.UzivatelskeJmeno = "Host";
            host.Role = "host";

            HlavniOkno.NastavPrihlaseneho(host);

            HlavniOkno hlavni = new HlavniOkno();
            hlavni.Show();
            this.Close();
        }

        /// <summary>
        /// Přihlásí uživatele po zadání jména a hesla
        /// Ověřuje správnost hesla, načítá údaje z databáze a aktualizuje čas posledního přihlášení
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string txtUzivatelskeJmeno = txtUser.Text.Trim();
            string txtHeslo = txtPass.Password.Trim();

            if (string.IsNullOrEmpty(txtUzivatelskeJmeno) || string.IsNullOrEmpty(txtHeslo))
            {
                MessageBox.Show("Zadejte uživatelské jméno a heslo!", "Upozornění",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                OracleConnection conn = DatabaseManager.GetConnection();

                // SQL dotaz pro získání dat uživatele
                string sql = @"
                    SELECT 
                        UZIVATELSKEJMENO,
                        HESLO,
                        SALT,
                        ROLE,
                        EMAIL
                    FROM PREHLED_UZIVATELSKE_UCTY
                    WHERE LOWER(UZIVATELSKEJMENO) = LOWER(:uzivatelskejmeno)";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("uzivatelskejmeno", OracleDbType.Varchar2).Value = txtUzivatelskeJmeno;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string uzivatelskeJmeno = reader.GetString(0);
                            string ulozenyHash = reader.GetString(1);
                            string ulozenySalt = reader.GetString(2);
                            string role = reader.GetString(3);
                            string email = reader.GetString(4);

                            bool hesloOK = PasswordHasher.VerifyPassword(txtHeslo, ulozenyHash, ulozenySalt);

                            if (hesloOK)
                            {
                                // Aktualizace posledního přihlášení
                                DatabaseRegistrace.UpdatePosledniPrihlaseni(conn, uzivatelskeJmeno);

                                MessageBox.Show("Přihlášení úspěšné (" + role + ")", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                                Uzivatel prihlasenyUzivatel = new Uzivatel
                                {
                                    UzivatelskeJmeno = uzivatelskeJmeno,
                                    Role = role,
                                    Email = email
                                };

                                HlavniOkno.NastavPrihlaseneho(prihlasenyUzivatel);

                                HlavniOkno hlavni = new HlavniOkno();
                                hlavni.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Nesprávné heslo!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Uživatel neexistuje!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přihlášení: " + ex.Message, "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
