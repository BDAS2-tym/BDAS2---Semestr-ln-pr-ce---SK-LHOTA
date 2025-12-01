using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    /// <summary>
    /// Hlavní okno aplikace – zobrazuje hlavní nabídku a funkce podle role přihlášeného uživatele
    /// </summary>
    public partial class HlavniOkno : Window
    {
        private DispatcherTimer timer; 
        private static Uzivatel prihlasenyUzivatel; 

        /// <summary>
        /// Konstruktor – nastaví časovač, načte údaje a zobrazí počet hráčů a trenérů
        /// </summary>
        public HlavniOkno()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Nastavení viditelnosti tlačítek podle přihlášeného uživatele
            NastavPrava();

            // Počet hráčů a trenérů se načte z databáze
            txtPocetHracu.Text = DatabaseHraci.GetPocetHracu().ToString();
            txtPocetTreneru.Text = DatabaseTreneri.GetPocetTreneru().ToString();
        }

        /// <summary>
        /// Uloží aktuálně přihlášeného uživatele do statické proměnné
        /// </summary>
        public static void NastavPrihlaseneho(Uzivatel uzivatel)
        {
            prihlasenyUzivatel = uzivatel;
        }

        /// <summary>
        /// Vrátí objekt aktuálně přihlášeného uživatele
        /// </summary>
        public static Uzivatel GetPrihlasenyUzivatel()
        {
            return prihlasenyUzivatel;
        }

        /// <summary>
        /// Nastaví, které části aplikace jsou přístupné podle role uživatele (např. admin, trenér, hráč atd.)
        /// </summary>
        private void NastavPrava()
        {
            // Pokud není přihlášený uživatel, nastaví se jako host
            if (prihlasenyUzivatel == null)
            {
                prihlasenyUzivatel = new Uzivatel
                {
                    UzivatelskeJmeno = "Host",
                    Role = "host"
                };
            }

            string role = prihlasenyUzivatel.Role.ToLower();

            // Zobrazení přihlášeného uživatele v rozhraní
            txtPrihlasenUzivatel.Text = $"{prihlasenyUzivatel.UzivatelskeJmeno} ({role})";

            // Nejprve všechna tlačítka vypneme
            VypniVse();

            // Podle role nastavíme přístupná tlačítka a funkce
            switch (role)
            {
                case "admin":
                    ZapniVse();
                    break;

                case "trener":
                    Zapni(btnHraci, btnTreneri, btnKontrakty, btnGrafy, btnZapasy, btnNastaveni);
                    break;

                case "hrac":
                    Zapni(btnHraci, btnTreneri, btnGrafy, btnTreninky, btnZapasy, btnGrafy);
                    break;

                case "uzivatel":
                    Zapni(btnGrafy, btnZapasy, btnTreninky);
                    break;

                case "host":
                    Zapni(btnZapasy);
                    break;
            }
        }

        /// <summary>
        /// Vypne všechna tlačítka (nepřístupné části)
        /// </summary>
        private void VypniVse()
        {
            btnHraci.IsEnabled = false;
            btnTreneri.IsEnabled = false;
            btnTreninky.IsEnabled = false;
            btnSponzori.IsEnabled = false;
            btnKontrakty.IsEnabled = false;
            btnSouteze.IsEnabled = false;
            btnGrafy.IsEnabled = false;
            btnZapasy.IsEnabled = false;
            btnNastaveni.IsEnabled = false;

            btnHraci.Opacity = 0.2;
            btnTreneri.Opacity = 0.2;
            btnTreninky.Opacity = 0.2;
            btnSponzori.Opacity = 0.2;
            btnKontrakty.Opacity = 0.2;
            btnSouteze.Opacity = 0.2;
            btnGrafy.Opacity = 0.2;
            btnZapasy.Opacity = 0.2;
            btnNastaveni.Opacity = 0.2;
        }

        /// <summary>
        /// Zapne všechna tlačítka (plný přístup pro administrátora)
        /// </summary>
        private void ZapniVse()
        {
            btnHraci.IsEnabled = true;
            btnTreneri.IsEnabled = true;
            btnSponzori.IsEnabled = true;
            btnKontrakty.IsEnabled = true;
            btnSouteze.IsEnabled = true;
            btnGrafy.IsEnabled = true;
            btnZapasy.IsEnabled = true;
            btnTreninky.IsEnabled = true;
            btnNastaveni.IsEnabled = true;

            btnHraci.Opacity = 1;
            btnTreneri.Opacity = 1;
            btnSponzori.Opacity = 1;
            btnKontrakty.Opacity = 1;
            btnSouteze.Opacity = 1;
            btnGrafy.Opacity = 1;
            btnZapasy.Opacity = 1;
            btnTreninky.Opacity = 1;
            btnNastaveni.Opacity = 1;
        }

        /// <summary>
        /// Zapne jen vybraná tlačítka (používá se pro role s omezeným přístupem)
        /// </summary>
        private void Zapni(params Button[] tlacitka)
        {
            foreach (var btn in tlacitka)
            {
                btn.IsEnabled = true;
                btn.Opacity = 1;
            }
        }

        /// <summary>
        /// Otevře okno s přehledem hráčů
        /// </summary>
        private void BtnHraci_Click(object sender, RoutedEventArgs e)
        {
            new HraciOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno s přehledem trenérů
        /// </summary>
        private void BtnTreneri_Click(object sender, RoutedEventArgs e)
        {
            new TreneriOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno s tréninky
        /// </summary>
        private void BtnTreninky_Click(object sender, RoutedEventArgs e)
        {
            new TreninkyOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno s kontrakty
        /// </summary>
        private void BtnKontrakty_Click(object sender, RoutedEventArgs e)
        {
            new KontraktyOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno se sponzory
        /// </summary>
        private void BtnSponzori_Click(object sender, RoutedEventArgs e)
        {
            new SponzoriOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno s grafy 
        /// </summary>
        private void BtnGrafy_Click(object sender, RoutedEventArgs e)
        {
            new GrafyOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno se soutěžemi
        /// </summary>
        private void BtnSouteze_Click(object sender, RoutedEventArgs e)
        {
            new SoutezeOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno se zápasy
        /// </summary>
        private void BtnZapasy_Click(object sender, RoutedEventArgs e)
        {
            new ZapasyOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Otevře okno nastavení
        /// </summary>
        private void BtnNastaveni_Click(object sender, RoutedEventArgs e)
        {
            new NastaveniOkno(this).Show();
            this.Hide();
        }

        /// <summary>
        /// Odhlásí aktuálního uživatele a vrátí ho na přihlašovací okno
        /// </summary>
        private void BtnOdhlaseni_Click(object sender, RoutedEventArgs e)
        {
            prihlasenyUzivatel = null; // odhlášení uživatele
            new PrihlaseniOkno(this).Show();
            this.Close();
        }

        /// <summary>
        /// Událost časovače – každou sekundu aktualizuje čas a datum na obrazovce
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            txtCas.Text = DateTime.Now.ToString("HH:mm:ss");
            txtDatum.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
