using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno představující hlavní nabídku nastavení aplikace
    /// Umožňuje uživateli přecházet do jednotlivých podoken:
    /// - správa binárního obsahu
    /// - správa uživatelů
    /// - systémový katalog
    /// - logovací tabulku
    /// </summary>
    public partial class NastaveniOkno : Window
    {
        private readonly HlavniOkno hlavniOkno;

        public NastaveniOkno(HlavniOkno hlavniOkno)
        {
            InitializeComponent();

            this.hlavniOkno = hlavniOkno;

            NastavPrava();
        }

        /// <summary>
        /// Nastaví viditelnost a dostupnost tlačítek v okně Nastavení
        /// podle role aktuálně přihlášeného uživatele
        /// </summary>
        private void NastavPrava()
        {
            Uzivatel prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
            string role = "host";

            if (prihlaseny != null && prihlaseny.Role != null)
            {
                role = prihlaseny.Role.ToLower();
            }

            // Nejprve deaktivujeme všechna tlačítka
            VypniVse();

            // ADMIN – plný přístup
            if (role == "admin")
            {
                ZapniVse();
                return;
            }

            // TRENÉR 
            if (role == "trener")
            {
                Zapni(btnBinarniObsah, btnUzivatele);
                return;
            }

            // HRÁČ 
            if (role == "hrac")
            {
                return;
            }

            // UŽIVATEL 
            if (role == "uzivatel")
            {
                return;
            }

            // HOST 
            if (role == "host")
            {
                return;
            }
        }

        /// <summary>
        /// Deaktivuje všechna tlačítka v okně Nastavení a upraví jejich průhlednost
        /// </summary>
        private void VypniVse()
        {
            btnBinarniObsah.IsEnabled = false;
            btnUzivatele.IsEnabled = false;
            btnSystemovyKatalog.IsEnabled = false;
            btnZmeny.IsEnabled = false;

            btnBinarniObsah.Opacity = 0.2;
            btnUzivatele.Opacity = 0.2;
            btnSystemovyKatalog.Opacity = 0.2;
            btnZmeny.Opacity = 0.2;
        }

        /// <summary>
        /// Aktivuje všechna tlačítka v okně Nastavení
        /// </summary>
        private void ZapniVse()
        {
            btnBinarniObsah.IsEnabled = true;
            btnUzivatele.IsEnabled = true;
            btnSystemovyKatalog.IsEnabled = true;
            btnZmeny.IsEnabled = true;

            btnBinarniObsah.Opacity = 1;
            btnUzivatele.Opacity = 1;
            btnSystemovyKatalog.Opacity = 1;
            btnZmeny.Opacity = 1;
        }

        /// <summary>
        /// Aktivuje pouze vybraná tlačítka
        /// </summary>
        /// <param name="tlacitka">Tlačítka, která mají být zapnuta</param>
        private void Zapni(params Button[] tlacitka)
        {
            foreach (Button btn in tlacitka)
            {
                btn.IsEnabled = true;
                btn.Opacity = 1;
            }
        }

        /// <summary>
        /// Otevře okno pro správu binárního obsahu 
        /// </summary>
        private void BtnBinarniObsah_Click(object sender, RoutedEventArgs e)
        {
            BinarniObsahOkno binarniObsahOkno = new BinarniObsahOkno();
            binarniObsahOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Otevře okno pro správu uživatelů 
        /// </summary>
        private void BtnUzivatele_Click(object sender, RoutedEventArgs e)
        {
            NastaveniUzivateleOkno uzivateleOkno = new NastaveniUzivateleOkno();
            uzivateleOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Otevře okno se systémovým katalogem
        /// </summary>
        private void BtnSystemovyKatalog_Click(object sender, RoutedEventArgs e)
        {
            SystemovyKatalogOkno systemovyKatalogOkno = new SystemovyKatalogOkno();
            systemovyKatalogOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Vrátí uživatele zpět do hlavního okna aplikace
        /// </summary>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            HlavniOkno hlavniOkno = new HlavniOkno();
            hlavniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Otevře okno s logem změn, které zobrazuje auditní historii databáze
        /// Předává hlavní okno kvůli přihlášenému uživateli
        /// </summary>
        private void BtnZmeny_Click(object sender, RoutedEventArgs e)
        {
            LogTableOkno logTableOkno = new LogTableOkno(hlavniOkno);
            logTableOkno.Show();
            this.Close();
        }
    }
}
