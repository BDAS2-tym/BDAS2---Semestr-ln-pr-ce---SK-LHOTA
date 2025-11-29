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
            hlavniOkno.ShowDialog();
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
