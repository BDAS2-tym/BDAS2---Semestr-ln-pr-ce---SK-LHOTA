using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Tools;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro úpravu uživatele – umožňuje editovat údaje 
    /// </summary>
    public partial class EditUzivatelOkno : Window
    {
        private Uzivatel uzivatel;
        private string stareJmeno;

        /// <summary>
        /// Konstruktor – přijme uživatele, jehož data se budou upravovat
        /// </summary>
        /// <param name="u">Objekt uživatele pro úpravu</param>
        public EditUzivatelOkno(Uzivatel editovanyUzivatel)
        {
            InitializeComponent();
            this.uzivatel = editovanyUzivatel;
            this.stareJmeno = editovanyUzivatel.UzivatelskeJmeno;
            NaplnFormular();
        }

        /// <summary>
        /// Naplní formulář aktuálními údaji o uživateli
        /// </summary>
        private void NaplnFormular()
        {
            txtUser.Text = uzivatel.UzivatelskeJmeno;
            txtEmail.Text = uzivatel.Email;
            txtRodneCislo.Text = uzivatel.RodneCislo;
        }

        /// <summary>
        /// Ověří formát rodného čísla – musí mít 10 číslic nebo může být prázdné
        /// </summary>
        /// <param name="rodneCislo">Rodné číslo zadané uživatelem</param>
        /// <returns>True, pokud je rodné číslo platné, jinak false</returns>
        private bool OverRodneCislo(string rodneCislo)
        {
            if (string.IsNullOrEmpty(rodneCislo))
                return true;

            if (rodneCislo.Length != 10)
                return false;

            // Každý znak musí být číslo 0–9
            foreach (char c in rodneCislo)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Zpracuje kliknutí na tlačítko "Uložit změny".
        /// Ověří vstupy, volitelně vytvoří nové heslo (hash + salt),
        /// a uloží změny do databáze.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jmeno = txtUser.Text.Trim();
                string email = txtEmail.Text.Trim();
                string rodneCislo = txtRodneCislo.Text.Trim();
                string heslo = txtHeslo.Text.Trim();

                // Ověření povinných polí
                if (jmeno == "")
                {
                    MessageBox.Show("Zadej uživatelské jméno!");
                    return;
                }

                if (email == "")
                {
                    MessageBox.Show("Zadej e-mail!");
                    return;
                }

                if (!OverRodneCislo(rodneCislo))
                {
                    MessageBox.Show("Rodné číslo musí mít 10 číslic!");
                    return;
                }

                // Uložení změn do objektu
                uzivatel.UzivatelskeJmeno = jmeno;
                uzivatel.Email = email;
                uzivatel.RodneCislo = rodneCislo;

                // Pokud bylo zadáno nové heslo, vygeneruje se nový salt a hash
                if (heslo != "")
                {
                    string salt = PasswordHasher.GenerateSalt(); // salt = náhodný text spojený k heslu
                    string hash = PasswordHasher.HashPassword(heslo, salt); // hash = otisk hesla + salt
                    uzivatel.Heslo = hash;
                    uzivatel.Salt = salt;
                }

                // Aktualizace dat v databázi
                DatabaseRegistrace.UpdateUzivatel(uzivatel, stareJmeno);

                MessageBox.Show("Změny byly uloženy.");
                DialogResult = true; // informuje rodičovské okno o úspěchu
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        /// <summary>
        /// Zavře okno po kliknutí na tlačítko "Zavřít"
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Minimalizuje okno po kliknutí na tlačítko "-"
        /// </summary>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Umožní okno tažením myší po pozadí
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
