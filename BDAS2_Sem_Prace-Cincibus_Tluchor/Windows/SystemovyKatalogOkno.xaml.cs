using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno zobrazuje systémový katalog databáze Oracle
    /// Umožňuje filtrovat objekty (tabulky, pohledy, funkce, indexy atd.)
    /// a otevřít detail konkrétního vybraného objektu
    /// </summary>
    public partial class SystemovyKatalogOkno : Window
    {
        /// <summary>
        /// Kompletní seznam všech načtených objektů z databáze
        /// </summary>
        public List<SystemovyObjekt> KatalogData { get; set; }

        /// <summary>
        /// Seznam objektů, které se právě zobrazují podle aktivního filtru
        /// </summary>
        public List<SystemovyObjekt> FiltrovanaData { get; set; }

        private HlavniOkno hlavniOkno;

        public SystemovyKatalogOkno()
        {
            InitializeComponent();

            // Načtení objektů z databáze
            KatalogData = DatabaseSystemovyKatalog.GetSystemoveObjekty();

            // Bez počátečního filtru se zobrazí všechny objekty
            FiltrovanaData = KatalogData;

            dgSystemovyKatalog.ItemsSource = FiltrovanaData;

            DataContext = this;
        }

        /// <summary>
        /// Zavře aktuální okno a otevře nové okno s nastavením
        /// </summary>
        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
            this.Close();
        }

        /// <summary>
        /// Událost reaguje na změnu zaškrtnutí filtračních checkboxů
        /// provede filtrování
        /// </summary>
        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Aplikuje aktivní filtry na seznam objektů
        /// Pokud není zaškrtnut žádný filtr, zobrazí se všechny objekty
        /// </summary>
        private void Filter()
        {
            // Zjištění, zda není vybraný žádný typ objektu
            bool nicZaskrtnute =
                chkTables.IsChecked == false &&
                chkViews.IsChecked == false &&
                chkFunctions.IsChecked == false &&
                chkProcedures.IsChecked == false &&
                chkPackages.IsChecked == false &&
                chkTriggers.IsChecked == false &&
                chkSequences.IsChecked == false &&
                chkIndexes.IsChecked == false &&
                chkConstraints.IsChecked == false;

            // Pokud není vybrán žádný filtr, zobrazují se všechny objekty
            if (nicZaskrtnute)
            {
                FiltrovanaData = KatalogData.ToList();
                dgSystemovyKatalog.ItemsSource = FiltrovanaData;
                dgSystemovyKatalog.Items.Refresh();
                return;
            }

            // Nový seznam odpovídající aktivním filtrům
            var list = new List<SystemovyObjekt>();

            // Postupně kontrolujeme každý objekt z katalogu
            foreach (var o in KatalogData)
            {
                bool add = false;

                // Každý checkbox představuje jeden typ objektu
                if (chkTables.IsChecked == true && o.TypObjektu == "TABLE")
                    add = true;

                if (chkViews.IsChecked == true && o.TypObjektu == "VIEW")
                    add = true;

                if (chkFunctions.IsChecked == true && o.TypObjektu == "FUNCTION")
                    add = true;

                if (chkProcedures.IsChecked == true && o.TypObjektu == "PROCEDURE")
                    add = true;

                if (chkPackages.IsChecked == true &&
                    (o.TypObjektu == "PACKAGE" || o.TypObjektu == "PACKAGE BODY"))
                    add = true;

                if (chkTriggers.IsChecked == true && o.TypObjektu == "TRIGGER")
                    add = true;

                if (chkSequences.IsChecked == true && o.TypObjektu == "SEQUENCE")
                    add = true;

                if (chkIndexes.IsChecked == true && o.TypObjektu == "INDEX")
                    add = true;

                if (chkConstraints.IsChecked == true && o.TypObjektu.StartsWith("CONSTRAINT"))
                    add = true;

                // Pokud objekt odpovídá filtru, přidáme ho
                if (add) 
                {
                    list.Add(o);
                }
        
            }

            // Aktualizace zobrazených dat
            FiltrovanaData = list;
            dgSystemovyKatalog.ItemsSource = FiltrovanaData;
            dgSystemovyKatalog.Items.Refresh();
        }

        /// <summary>
        /// Otevře nové okno s podrobným náhledem objektu, pokud uživatel dvakrát klikne na řádek
        /// </summary>
        private void dgSystemovyKatalog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSystemovyKatalog.SelectedItem is SystemovyObjekt obj)
            {
                // Zobrazení detailu objektu
                new DetailObjektuOkno(obj).ShowDialog();

                // Po návratu se obnoví datagrid (může být aktualizován počet řádků nebo kód)
                dgSystemovyKatalog.Items.Refresh();
            }
        }
    }
}
