using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro odesílání hromadných zpráv uživatelům.
    /// Umožňuje vybrat příjemce, zapsat text a odeslat zprávy e-mailem.
    /// </summary>
    public partial class DialogZpravaOkno : Window
    {
        /// <summary>
        /// Kolekce uživatelů načtená z hlavního okna.
        /// </summary>
        private IEnumerable<Uzivatel> uzivatele;

        /// <summary>
        /// Konstruktor okna. Naplní ListBox seznamem uživatelů
        /// </summary>
        public DialogZpravaOkno(IEnumerable<Uzivatel> uzivatele)
        {
            InitializeComponent();
            this.uzivatele = uzivatele;

            // U každého uživatele zruší předchozí zaškrtnutí, tedy vždy bude při otevření chekbox NEzaškrtnuty
            foreach (Uzivatel uzivatel in uzivatele)
            {
                uzivatel.JeVybran = false;
            }

            // Přiřadí ListBoxu seznam uživatelů
            lbPrijemci.ItemsSource = uzivatele;

        }

        /// <summary>
        /// Zavře okno bez odesílání
        /// </summary>
        private void BtnZrusit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Hlavní logika pro odeslání e-mailové zprávy všem vybraným uživatelům
        /// Provádí validaci
        /// a následně pro každého vybraného uživatele volá odeslání e-mailu.
        /// </summary>
        /// <param name="sender">Tlačítko Odeslat.</param>
        private void BtnOdeslat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Uzivatel> prijemci = ZiskejVybranePrijemce();

                if (prijemci.Count == 0)
                {
                    MessageBox.Show("Musíte vybrat alespoň jednoho příjemce", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (String.IsNullOrWhiteSpace(txtZprava.Text))
                {
                    MessageBox.Show("Zpráva nesmí být prázdná", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int uspesne = 0;
                int neuspesne = 0;

                foreach (Uzivatel prijemce in prijemci)
                {
                    if (String.IsNullOrWhiteSpace(prijemce.Email))
                    {
                        MessageBox.Show(
                            "Uživatel '" + prijemce.UzivatelskeJmeno + "' nemá vyplněný e-mail. Zpráva nebude odeslána",
                            "Chybějící e-mail",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        neuspesne++;
                    }
                    else
                    {
                        bool vysledek = OdesliEmail(prijemce.Email, txtZprava.Text);

                        if (vysledek == true)
                        {
                            uspesne++;
                        }
                        else
                        {
                            neuspesne++;
                        }
                    }
                }

                MessageBox.Show(
                    "Odesílání dokončeno.\n\nÚspěšně odesláno: " + uspesne +
                    "\nNepodařilo se odeslat: " + neuspesne,
                    "Výsledek odesílání",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Neočekávaná chyba během odesílání:\n" + ex.Message,
                    "Chyba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odesílá e-mail přes SMTP server seznam.cz
        /// Vrací true při úspěchu, false při neúspěchu
        /// </summary>
        /// <param name="emailPrijemce">E-mail příjemce</param>
        /// <param name="text">Tělo zprávy</param>
        /// <returns>True/False podle výsledku odeslání</returns>
        private bool OdesliEmail(string emailPrijemce, string text)
        {
            try
            {
                // Vytvoří nový e-mailový objekt, do kterého budeme zapisovat údaje
                MailMessage msg = new MailMessage();

                msg.To.Add(emailPrijemce);

                // Nastaví odesílatele e-mailu – oficiální klubový e-mail
                msg.From = new MailAddress("infosklhota@seznam.cz");
                msg.Subject = "Oznámení klubu SK Lhota";

                msg.Body =
                    text +
                    "\n\n-----------------------------------------\n" +
                    "SK Lhota – automatické oznámení\n" +
                    "Tento e-mail byl odeslán z informačního systému klubu\n" +
                    "Na tento e-mail prosím NEODPOVÍDEJTE, není monitorován\n" +
                    "V případě potřeby kontaktujte trenéra nebo vedení klubu\n" +
                    "-----------------------------------------";

                // Vytvoří SMTP klienta a nastaví server Seznamu + port 587 (TLS)
                SmtpClient smtp = new SmtpClient("smtp.seznam.cz", 587);

                // Zapne SSL šifrování – bez toho Seznam nepovolí připojení
                smtp.EnableSsl = true;

                smtp.Timeout = 5000;
                smtp.Credentials = new NetworkCredential("infosklhota@seznam.cz", "fotbalovyklubsklhota");

                // Samotné odeslání e-mailu
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Zpráva pro " + emailPrijemce + " nebyla odeslána.\nChyba: " + ex.Message,
                    "Chyba odesílání",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
        }

        /// <summary>
        /// Získá všechny uživatele, kteří mají vlastnost <c>JeVybran</c> nastavenu na true
        /// Používá se jako zdroj pro skutečné odeslání zprávy
        /// </summary>
        /// <returns>List uživatelů, kteří mají být příjemci</returns>
        private List<Uzivatel> ZiskejVybranePrijemce()
        {
            return uzivatele.Where(u => u.JeVybran == true).ToList();
        }

    }
}
