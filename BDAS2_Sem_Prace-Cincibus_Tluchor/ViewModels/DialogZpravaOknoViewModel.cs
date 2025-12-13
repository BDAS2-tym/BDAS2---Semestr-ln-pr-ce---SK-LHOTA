using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.ViewModels
{
    /// <summary>
    /// ViewModel pro dialog hromadné zprávy/notifikace uživatelům
    /// seznam uživatelů, text zprávy a příkazy Odeslat/Zrušit
    /// </summary>
    public class DialogZpravaOknoViewModel : ViewModelBase
    {
        private readonly IEnumerable<Uzivatel> _uzivatele;

        /// <summary>
        /// Seznam uživatelů zobrazený v ListBoxu (výběr příjemců)
        /// </summary>
        public IEnumerable<Uzivatel> Uzivatele
        {
            get { return _uzivatele; }
        }

        private string _zprava = "";
        /// <summary>
        /// Text zprávy, který se odešle vybraným příjemcům
        /// </summary>
        public string Zprava
        {
            get { return _zprava; }
            set { _zprava = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Požadavek na zavření okna (DialogResult)
        /// </summary>
        public event Action<bool>? RequestClose;

        /// <summary>
        /// Příkaz pro odeslání zprávy vybraným uživatelům
        /// </summary>
        public RelayCommand OdeslatCommand { get; }

        /// <summary>
        /// Příkaz pro zavření dialogu bez odeslání
        /// </summary>
        public RelayCommand ZrusitCommand { get; }

        /// <summary>
        /// Inicializace ViewModelu a vynulování výběru (JeVybran = false).
        /// </summary>
        public DialogZpravaOknoViewModel(IEnumerable<Uzivatel> uzivatele)
        {
            if (uzivatele == null)
            {
                _uzivatele = new List<Uzivatel>();
            }
            else
            {
                _uzivatele = uzivatele;
            }

            foreach (Uzivatel u in _uzivatele)
            {
                u.JeVybran = false;
            }

            Zprava = "";

            OdeslatCommand = new RelayCommand(_ => Odeslat());
            ZrusitCommand = new RelayCommand(_ => RequestClose?.Invoke(false));
        }

        /// <summary>
        /// Hlavní logika odeslání: validace + iterace přes vybrané uživatele.
        /// </summary>
        private void Odeslat()
        {
            try
            {
                List<Uzivatel> prijemci = ZiskejVybranePrijemce();

                if (prijemci.Count == 0)
                {
                    MessageBox.Show("Musíte vybrat alespoň jednoho příjemce",
                        "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Zprava))
                {
                    MessageBox.Show("Zpráva nesmí být prázdná",
                        "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int uspesne = 0;
                int neuspesne = 0;

                foreach (Uzivatel prijemce in prijemci)
                {
                    if (string.IsNullOrWhiteSpace(prijemce.Email))
                    {
                        MessageBox.Show(
                            "Uživatel '" + prijemce.UzivatelskeJmeno + "' nemá vyplněný e-mail. Zpráva nebude odeslána",
                            "Chybějící e-mail",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        neuspesne++;
                        continue;
                    }

                    bool vysledek = OdesliEmail(prijemce.Email, Zprava);

                    if (vysledek)
                    {
                        uspesne++;
                    }
                    else
                    {
                        neuspesne++;
                    }
                }

                MessageBox.Show(
                    "Odesílání dokončeno.\n\nÚspěšně odesláno: " + uspesne +
                    "\nNepodařilo se odeslat: " + neuspesne,
                    "Výsledek odesílání",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                RequestClose?.Invoke(true);
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
        /// Odešle e-mail přes SMTP (Seznam).
        /// Vrací true při úspěchu, false při chybě.
        /// </summary>
        private bool OdesliEmail(string emailPrijemce, string text)
        {
            try
            {
                string smtpUser = "infosklhota@seznam.cz";

                string smtpPass = "fotbalovyklubsklhota";

                if (smtpPass == null)
                {
                    smtpPass = "";
                }

                if (smtpPass.Trim().Length == 0)
                {
                    MessageBox.Show(
                        "Chybí SMTP heslo v App.config (AppSettings: SmtpPassword).",
                        "Chyba konfigurace",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }

                MailMessage msg = new MailMessage();
                msg.To.Add(emailPrijemce);
                msg.From = new MailAddress(smtpUser);
                msg.Subject = "Oznámení klubu SK Lhota";

                msg.Body =
                    text +
                    "\n\n-----------------------------------------\n" +
                    "SK Lhota – automatické oznámení\n" +
                    "Tento e-mail byl odeslán z informačního systému klubu\n" +
                    "Na tento e-mail prosím NEODPOVÍDEJTE, není monitorován\n" +
                    "V případě potřeby kontaktujte trenéra nebo vedení klubu\n" +
                    "-----------------------------------------";

                SmtpClient smtp = new SmtpClient("smtp.seznam.cz", 587);
                smtp.EnableSsl = true;
                smtp.Timeout = 5000;
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);

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
        /// Vrátí uživatele, kteří mají JeVybran == true.
        /// </summary>
        private List<Uzivatel> ZiskejVybranePrijemce()
        {
            return _uzivatele.Where(u => u.JeVybran == true).ToList();
        }
    }
}
