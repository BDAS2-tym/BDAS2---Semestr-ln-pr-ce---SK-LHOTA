using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation.Peers;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor
{
    public partial class TreneriOkno : Window
    {
        private HlavniOkno hlavniOkno;
        public ObservableCollection<Trener> TreneriData { get; set; }

        public TreneriOkno(HlavniOkno hlavniOkno)
        {
            // 1. Nejprve inicializujte komponenty z XAML
            InitializeComponent();

            // 2. Nyní, když je DataGrid vytvořen, inicializujte kolekci dat
            // a nastavte DataContext.
            TreneriData = new ObservableCollection<Trener>();
            this.DataContext = this;

            // 3. Naplňte kolekci daty
            LoadTreneriData();

            this.hlavniOkno = hlavniOkno;
            
        }

        private void BtnZpet_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            hlavniOkno.Show();
        }

        public void LoadTreneriData()
        {
        TreneriData.Add(new Trener(1, "Petr", "Novák", "602123456", "Útočníci", 15, "UEFA A"));
            TreneriData.Add(new Trener(2, "Jana", "Svobodová", "777888999", "Brankáři", 10, "UEFA B"));
            TreneriData.Add(new Trener(3, "Tomáš", "Dvořák", "603555222", "Obrana", 8, "UEFA B"));
            TreneriData.Add(new Trener(4, "Lucie", "Krejčí", "731444555", "Kondiční", 5, "UEFA C"));
            TreneriData.Add(new Trener(5, "Jiří", "Černý", "608777111", "Mladší žáci", 20, "UEFA PRO"));
            TreneriData.Add(new Trener(6, "Marie", "Nováková", "605999333", "Starší žáci", 12, "UEFA A"));
            TreneriData.Add(new Trener(7, "Pavel", "Havelka", "775666777", "Taktika", 9, "UEFA B"));
            TreneriData.Add(new Trener(8, "Veronika", "Němcová", "724111222", "Technika", 6, "UEFA C"));
            TreneriData.Add(new Trener(9, "Martin", "Kučera", "604333888", "Dorost", 18, "UEFA A"));
            TreneriData.Add(new Trener(10, "Kateřina", "Benešová", "776999000", "Ženy", 14, "UEFA PRO"));
            TreneriData.Add(new Trener(11, "Lukáš", "Pospíšil", "606444111", "Útočníci", 7, "UEFA B"));
            TreneriData.Add(new Trener(12, "Petra", "Zemanová", "732555666", "Brankáři", 11, "UEFA B"));
            TreneriData.Add(new Trener(13, "Jan", "Kratochvíl", "607888999", "Obrana", 4, "UEFA C"));
            TreneriData.Add(new Trener(14, "Eva", "Horáková", "778222333", "Kondiční", 16, "UEFA A"));
            TreneriData.Add(new Trener(15, "Filip", "Urban", "601555666", "Mladší žáci", 22, "UEFA PRO"));
            TreneriData.Add(new Trener(16, "Daniela", "Šimková", "733777888", "Starší žáci", 13, "UEFA A"));
            TreneriData.Add(new Trener(17, "Vojtěch", "Procházka", "609000111", "Taktika", 8, "UEFA B"));
            TreneriData.Add(new Trener(18, "Michaela", "Fialová", "779333444", "Technika", 5, "UEFA C"));
            TreneriData.Add(new Trener(19, "Ondřej", "Kolář", "600666777", "Dorost", 19, "UEFA PRO"));
            TreneriData.Add(new Trener(20, "Hana", "Veselá", "734888999", "Ženy", 15, "UEFA A"));
            TreneriData.Add(new Trener(21, "Tomáš", "Král", "602111222", "Útočníci", 10, "UEFA B"));
            TreneriData.Add(new Trener(22, "Tereza", "Kovářová", "777444555", "Brankáři", 6, "UEFA C"));
            TreneriData.Add(new Trener(23, "Adam", "Růžička", "603888999", "Obrana", 17, "UEFA PRO"));
            TreneriData.Add(new Trener(24, "Barbora", "Poláková", "731111222", "Kondiční", 12, "UEFA A"));
            TreneriData.Add(new Trener(25, "David", "Němec", "608444555", "Mladší žáci", 9, "UEFA B"));
            TreneriData.Add(new Trener(26, "Lenka", "Holubová", "605777888", "Starší žáci", 14, "UEFA"));
            TreneriData.Add(new Trener(27, "Jakub", "Beneš", "775000111", "Taktika", 11, "UEFA B"));
            TreneriData.Add(new Trener(28, "Nikola", "Musilová", "724333444", "Technika", 3, "UEFA C"));
            TreneriData.Add(new Trener(29, "František", "Novotný", "604666777", "Dorost", 25, "UEFA PRO"));
            TreneriData.Add(new Trener(30, "Zuzana", "Blažková", "776111222", "Ženy", 2, "UEFA C"));
  
        }
    }

    public class Trener
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Telefon { get; set; }
        public string Specializace { get; set; }
        public int PocetLetPraxe { get; set; }
        public string TrenerLicence { get; set; }

        public Trener(int id, string jmeno, string prijmeni, string telefon, string specializace, int pocetLetPraxe, string trenerLicence)
        {
            Id = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Telefon = telefon;
            Specializace = specializace;
            PocetLetPraxe = pocetLetPraxe;
            TrenerLicence = trenerLicence;
        }
    }
}
