using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for DialogKonciciKontrakty.xaml
    /// </summary>
    public partial class DialogKonciciKontrakty : Window
    {
        public ObservableCollection<KonciciKontrakt> KonciciKontrakty { get; set; } = new ObservableCollection<KonciciKontrakt>();

        public DialogKonciciKontrakty()
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
        /// Metoda slouží k naplnění kolekce končících kontraktů a naplnění DataGridu
        /// <param name="pocetDni">Počet dní ode dneška, kdy kontrakty mají skončit</param>
        /// </summary>
        private void ZobrazKonciciKontrakty(int pocetDni)
        {
            KonciciKontrakty.Clear();

            try
            {
                var conn = DatabaseManager.GetConnection();

                using (var cmd = new OracleCommand("PKG_KONTRAKTY.SP_KONTROLA_KONCICICH_KONTRAKTU", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_pocet_dni_do_konce_platnosti", OracleDbType.Int32).Value = pocetDni;
                    var jsonParam = new OracleParameter("v_json_vystup", OracleDbType.Clob);
                    jsonParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(jsonParam);

                    cmd.ExecuteNonQuery();

                    string jsonString;

                    using (var clob = (OracleClob)jsonParam.Value)
                    {
                        jsonString = clob.Value;
                    }

                    var rawItems = JsonSerializer.Deserialize<List<string>>(jsonString);

                    KonciciKontrakty.Clear();

                    if (rawItems != null)
                    {
                        foreach (var item in rawItems)
                        {
                            var kontrakt = JsonSerializer.Deserialize<KonciciKontrakt>(item);

                            if (kontrakt != null)
                            {
                                KonciciKontrakty.Add(kontrakt);
                            }
                        }
                    }


                    if(KonciciKontrakty.Count == 0)
                    {
                        MessageBox.Show($"Nebyly nalezeny žádné kontrakty, které by končili v době {pocetDni} dní", "Not found", MessageBoxButton.OK , MessageBoxImage.Information);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání kontraktů:\n{ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda slouží k předání počtu dní pro zobrazení končících kontraktů
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnZobraz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(iudPocetDni.Value.ToString(), out int pocetDni))
                {
                    throw new FormatException("Nastala chyba při formátování počtu dní. Počet dní musí být celé číslo!");
                }

                ZobrazKonciciKontrakty(pocetDni);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
