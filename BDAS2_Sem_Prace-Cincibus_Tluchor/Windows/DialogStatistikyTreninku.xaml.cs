using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Interaction logic for DialogStatistikyTreninku.xaml
    /// </summary>
    public partial class DialogStatistikyTreninku : Window
    {
        public ObservableCollection<DenTreninku> StatistikyTreninku { get; set; } = new ObservableCollection<DenTreninku>();

        public DialogStatistikyTreninku()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// Metoda slouží k zavření dialogového okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda slouží k zobrazení statistik tréninkových dnů v konkrétním měsíci získaných z databáze
        /// </summary>
        /// <param name="mesic">Vybraný měsíc</param>
        private void ZobrazStatistikyTreninku(string? mesic)
        {
            try
            {
                DateTime? datum = PrevedMesicNaCeleDatum(mesic);
                if (datum == null)
                {
                    throw new NonValidDataException("Měsíc nemůže být prázdný ani NULL!");
                }

                StatistikyTreninku.Clear();
                List<string> dnyVTydnu = new List<string> { "Pondělí", "Úterý", "Středa", "Čtvrtek", "Pátek", "Sobota", "Neděle" };
                foreach (var den in dnyVTydnu)
                {
                    StatistikyTreninku.Add(new DenTreninku 
                    { 
                        Den = den,
                        Pocet = 0, 
                        Procenta = 0.0 
                    });
                }

                int maxPocet = 0;
                int minPocet = int.MaxValue;

                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("PKG_TRENINKY.SP_VYPOCITEJ_ZATEZ_TRENINK_DNU", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_datum", OracleDbType.Date).Value = datum;

                    var outputParam = cmd.Parameters.Add("v_info", OracleDbType.Varchar2);
                    outputParam.Direction = System.Data.ParameterDirection.Output;
                    outputParam.Size = 32767;

                    cmd.ExecuteNonQuery();

                    string? report = outputParam.Value?.ToString();
                    if (string.IsNullOrWhiteSpace(report))
                    {
                        return;
                    }

                    // Parsování celého reportu z databáze
                    string[] lines = report.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (string.IsNullOrEmpty(trimmed))
                        {
                            continue;
                        }

                        int indexColon = trimmed.IndexOf(':');
                        if (indexColon <= 0)
                        {
                            continue;
                        }

                        string leva = trimmed.Substring(0, indexColon).Trim();
                        string prava = trimmed.Substring(indexColon + 1).Trim();

                        string den = "";
                        int pocet = 0;

                        int indexPar = prava.IndexOf('(');

                        // Normální dny v týdnu
                        if (indexPar > 0 && !leva.StartsWith("Nejvíce") && !leva.StartsWith("Nejméně"))
                        {
                            den = leva;
                            string pocetStr = prava.Substring(0, indexPar).Trim();
                            int.TryParse(pocetStr, out pocet);

                            var denTreninku = StatistikyTreninku
                                .FirstOrDefault(d => d.Den.Trim().Equals(den.Trim(), StringComparison.OrdinalIgnoreCase));

                            if (denTreninku != null)
                                denTreninku.Pocet = pocet;
                        }

                        // Nejvíce/Nejméně tréninků
                        else if (leva.StartsWith("Nejvíce tréninků") || leva.StartsWith("Nejméně tréninků"))
                        {
                            den = leva;

                            if (indexPar >= 0)
                            {
                                string pocetStr = prava.Substring(indexPar + 1).Replace(")", "").Trim();
                                int.TryParse(pocetStr, out pocet);
                            }

                            if (leva.StartsWith("Nejvíce tréninků"))
                            {
                                maxPocet = pocet;
                            }

                            else
                            {
                                minPocet = pocet;
                            }
                        }
                    }
                }

                int celkemTreninku = StatistikyTreninku.Sum(d => d.Pocet);
                if (celkemTreninku > 0)
                {
                    foreach (var d in StatistikyTreninku)
                    {
                        d.Procenta = Math.Round((double)d.Pocet / celkemTreninku * 100, 1);
                    }
                }

                //Speciální dny - Nejvíce/Nejméně tréninků
                if (StatistikyTreninku.Any(x => x.Pocet != 0))
                {
                    StatistikyTreninku.Add(new DenTreninku
                    {
                        Den = "Nejvíce tréninků",
                        Pocet = maxPocet,
                        Procenta = 0
                    });

                    if (minPocet != int.MaxValue)
                    {
                        StatistikyTreninku.Add(new DenTreninku
                        {
                            Den = "Nejméně tréninků",
                            Pocet = minPocet,
                            Procenta = 0
                        });
                    } 
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání tréninků:\n{ex.Message}",
                    "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Pomocná metoda slouží k převodu měsíce v na celé datum
        /// </summary>
        /// <param name="mesic">Vybraný měsíc</param>
        /// <returns>Vrací celé datum s konkrétním měsícem, pokud je měsíc nevalidní vrací NULL</returns>
        private static DateTime? PrevedMesicNaCeleDatum(string? mesic)
        {
            if (string.IsNullOrEmpty(mesic))
            {
                return null;
            }

            var culture = new CultureInfo("cs-CZ");
            int monthNumber = DateTime.ParseExact(mesic, "MMMM", culture).Month;
            int cilovyRok = DateTime.Now.Year;

            return new DateTime(cilovyRok, monthNumber, 1);
        }

        /// <summary>
        /// Metoda slouží k zobrazení statistik tréninkových dnů při vybrání měsíce
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void CbMesic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem? vybranyMesic = cbMesic.SelectedItem as ComboBoxItem;
            if (vybranyMesic != null)
            {
                ZobrazStatistikyTreninku(vybranyMesic.Content.ToString());
            }
        }
    }
}
