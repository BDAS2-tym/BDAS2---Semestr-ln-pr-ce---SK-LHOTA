using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro správu binárního obsahu v databázi.
    /// Umožňuje vkládat, mazat, nahrazovat, stahovat soubory a  zobrazovat obrázky
    /// </summary>
    public partial class BinarniObsahOkno : Window
    {
        private byte[] obsahSouboru;
        private string nazevSouboru;
        private string priponaSouboru;

        private HlavniOkno hlavniOkno;

        /// <summary>
        /// Kolekce s binárními soubory z databáze – slouží jako zdroj dat pro DataGrid
        /// </summary>
        public ObservableCollection<BinarniObsah> ObsahData { get; set; } = new ObservableCollection<BinarniObsah>();

        /// <summary>
        /// Konstruktor – inicializuje komponenty a načte obsah z databáze
        /// </summary>
        public BinarniObsahOkno()
        {
            InitializeComponent();
            dgBinarniObsah.ItemsSource = ObsahData;
            NactiBinarniObsah();
            this.Closed += BinarniObsahOkno_Closed; 
        }

        /// <summary>
        /// Po zavření okna otevře NastaveniOkno
        /// </summary>
        private void BinarniObsahOkno_Closed(object sender, System.EventArgs e)
        {
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
        }

        /// <summary>
        /// Načte všechny soubory z pohledu BINARNI_OBSAH_ROLE_VIEW
        /// </summary>
        private void NactiBinarniObsah()
        {
            ObsahData.Clear();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM BINARNI_OBSAH_ROLE_VIEW ORDER BY DATUMNAHRANI DESC";

                    using (var cmd = new OracleCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roleText;

                            if (reader["ROLE"] == DBNull.Value)
                            {
                                roleText = "Neznámá role";
                            }
                            else 
                            {
                                roleText = reader["ROLE"].ToString();
                            }

                            // Naplnění objektu hodnotami z databáze
                            BinarniObsah zaznam = new BinarniObsah();
                            zaznam.IdObsah = Convert.ToInt32(reader["IDBINARNIOBSAH"]);
                            zaznam.NazevSouboru = reader["NAZEVSOUBORU"].ToString();
                            zaznam.TypSouboru = reader["TYPSOUBORU"].ToString();
                            zaznam.PriponaSouboru = reader["PRIPONASOUBORU"].ToString();
                            zaznam.DatumNahrani = Convert.ToDateTime(reader["DATUMNAHRANI"]);
                            zaznam.DatumModifikace = Convert.ToDateTime(reader["DATUMMODIFIKACE"]);
                            zaznam.Operace = reader["OPERACE"].ToString();
                            zaznam.IdUzivatelskyUcet = Convert.ToInt32(reader["IDUZIVATELSKYUCET"]);
                            zaznam.Uzivatel = roleText;

                            ObsahData.Add(zaznam);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Otevře dialog pro výběr souboru z PC
        /// </summary>
        private void BtnVybratSoubor_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Všechny soubory|*.*" };

            if (dlg.ShowDialog() == true)
            {
                string cesta = dlg.FileName;
                nazevSouboru = Path.GetFileNameWithoutExtension(cesta);
                priponaSouboru = Path.GetExtension(cesta).TrimStart('.').ToLower();

                // Kontroly délek
                if (nazevSouboru.Length > 50)
                {
                    MessageBox.Show("Název souboru je příliš dlouhý (max. 50 znaků)");
                    return;
                }
                if (priponaSouboru.Length > 10)
                {
                    MessageBox.Show("Přípona souboru je příliš dlouhá (max. 10 znaků)");
                    return;
                }

                string mimeType = GetMimeType(cesta);
                tboxNazev.Text = nazevSouboru;
                tboxPripona.Text = priponaSouboru;
                tboxTyp.Text = mimeType;

                // Načtení obsahu souboru do pole bytů (pro uložení do databáze)
                obsahSouboru = File.ReadAllBytes(cesta);

                // Pokud je typ obrázek, zobrazí se náhled
                if (mimeType.StartsWith("image"))
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(cesta);
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    imgNahlad.Source = img;
                }
                else
                {
                    imgNahlad.Source = null;
                }

                txtStatus.Text = "Soubor načten";
            }
        }

        /// <summary>
        /// Uloží vybraný soubor do databáze jako nový záznam
        /// </summary>
        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (obsahSouboru == null)
            {
                MessageBox.Show("Nejdříve vyberte soubor");
                return;
            }

            if (tboxNazev.Text.Length > 50 || tboxTyp.Text.Length > 50 || tboxPripona.Text.Length > 10)
            {
                MessageBox.Show("Některá pole přesahují maximální délku");
                return;
            }

            try
            {
                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                if (prihlaseny == null)
                {
                    MessageBox.Show("Není přihlášen žádný uživatel");
                    return;
                }

                // Získáme ID role 
                int idRole = ZiskejIdRole(prihlaseny.Role);

                // Získáme ID uživatelského účtu podle role
                int idUzivatelskyUcet = 0;
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT IDUZIVATELSKYUCET FROM UZIVATELSKE_UCTY WHERE IDROLE = :idRole FETCH FIRST 1 ROWS ONLY";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("idRole", idRole);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Pro danou roli neexistuje žádný účet v databázi");
                            return;
                        }
                        idUzivatelskyUcet = Convert.ToInt32(result);
                    }
                }

                // Uložíme soubor do databáze
                DatabaseBinarniObsah.AddBinarniObsah(
                    tboxNazev.Text.Trim(),
                    tboxTyp.Text.Trim(),
                    tboxPripona.Text.Trim(),
                    obsahSouboru,
                    "vlozeni",
                    idUzivatelskyUcet
                );

                MessageBox.Show("Soubor byl uložen");
                txtStatus.Text = "Soubor uložen do databáze";
                NactiBinarniObsah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání: " + ex.Message);
            }
        }


        /// <summary>
        /// Vrátí ID role podle jejího názvu z tabulky ROLE
        /// </summary>
        private int ZiskejIdRole(string nazevRole)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT IDROLE FROM ROLE WHERE LOWER(NAZEVROLE) = LOWER(:nazevRole)";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("nazevRole", nazevRole);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při zjišťování role: " + ex.Message);
            }
            return -1;
        }

        /// <summary>
        /// Přejmenuje soubor po dvojkliku v Datagridu
        /// </summary>
        private void dgBinarniObsah_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vybrany = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybrany == null) return;

            string novyNazev = Interaction.InputBox("Zadejte nový název souboru:", "Přejmenování", vybrany.NazevSouboru);
            if (string.IsNullOrWhiteSpace(novyNazev)) return;

            if (novyNazev.Length > 50)
            {
                MessageBox.Show("Název souboru je příliš dlouhý (max. 50 znaků)");
                return;
            }

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "UPDATE BINARNI_OBSAH SET NAZEVSOUBORU = :nazev, DATUMMODIFIKACE = SYSDATE, OPERACE = 'uprava' WHERE IDBINARNIOBSAH = :id";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("nazev", novyNazev);
                        cmd.Parameters.Add("id", vybrany.IdObsah);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Název byl změněn");
                NactiBinarniObsah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při přejmenování: " + ex.Message);
            }
        }

        /// <summary>
        /// Nahrazení vybraného souboru jiným souborem
        /// </summary>
        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            var vybrany = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybrany == null)
            {
                MessageBox.Show("Vyberte soubor, který chcete nahradit");
                return;
            }

            // Potvrzovací dialog
            var potvrzeni = MessageBox.Show("Opravdu chcete nahradit vybraný soubor novým?", "Nahrazení souboru", MessageBoxButton.YesNo,  MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                // Otevře dialog pro výběr nového souboru
                BtnVybratSoubor_Click(sender, e);
                if (obsahSouboru == null)
                {
                    MessageBox.Show("Nebyl vybrán žádný nový soubor");
                    return;
                }

                // Aktualizace obsahu v databázi
                DatabaseBinarniObsah.UpdateBinarniObsah(vybrany.IdObsah, obsahSouboru, "uprava");
                MessageBox.Show("Soubor byl úspěšně nahrazen");
                NactiBinarniObsah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při nahrazování souboru: " + ex.Message);
            }
        }

        /// <summary>
        /// Smaže vybraný soubor z databáze po potvrzení
        /// </summary>
        private void BtnSmazatVybrany_Click(object sender, RoutedEventArgs e)
        {
            var vybrany = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybrany == null)
            {
                MessageBox.Show("Vyberte soubor ke smazání.");
                return;
            }

            var potvrzeni = MessageBox.Show($"Opravdu chcete smazat soubor '{vybrany.NazevSouboru}'?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (potvrzeni != MessageBoxResult.Yes) 
            {
                return;
            }

            try
            {
                DatabaseBinarniObsah.DeleteBinarniObsah(vybrany.IdObsah);
                MessageBox.Show("Soubor byl smazán.");
                NactiBinarniObsah();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při mazání: " + ex.Message);
            }
        }

        /// <summary>
        /// Umožňuje stažení souboru z databáze do PC
        /// </summary>
        private void BtnStahnoutVybrany_Click(object sender, RoutedEventArgs e)
        {
            var vybrany = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybrany == null)
            {
                MessageBox.Show("Vyberte soubor ke stažení");
                return;
            }

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT OBSAH, PRIPONASOUBORU FROM BINARNI_OBSAH WHERE IDBINARNIOBSAH = :id";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("id", vybrany.IdObsah);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] data = (byte[])reader["OBSAH"];
                                string pripona = reader["PRIPONASOUBORU"].ToString();

                                SaveFileDialog dlg = new SaveFileDialog
                                {
                                    FileName = vybrany.NazevSouboru + "." + pripona
                                };

                                if (dlg.ShowDialog() == true)
                                {
                                    File.WriteAllBytes(dlg.FileName, data);
                                    MessageBox.Show("Soubor byl uložen na disk");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při stahování: " + ex.Message);
            }
        }

        /// <summary>
        /// Zobrazí náhled obrázku, pokud má vybraný soubor podporovanou příponu
        /// </summary>
        private void BtnZobrazitObrazek_Click(object sender, RoutedEventArgs e)
        {
            var vybrany = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybrany == null)
            {
                MessageBox.Show("Nejdříve vyberte soubor z tabulky");
                return;
            }

            string pripona = vybrany.PriponaSouboru.ToLower();
            if (pripona != "jpg" && pripona != "jpeg" && pripona != "png" && pripona != "gif" && pripona != "bmp")
            {
                MessageBox.Show("Tento typ souboru nelze zobrazit jako obrázek");
                imgNahlad.Source = null;
                return;
            }

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT OBSAH FROM BINARNI_OBSAH WHERE IDBINARNIOBSAH = :id";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("id", vybrany.IdObsah);
                        using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess))
                        {
                            if (reader.Read())
                            {
                                var blob = reader.GetOracleBlob(0);
                                if (blob == null || blob.IsNull)
                                {
                                    MessageBox.Show("Soubor neobsahuje žádná data");
                                    return;
                                }

                                // Načtení binárního obsahu do paměti
                                byte[] data = new byte[blob.Length];
                                blob.Read(data, 0, (int)blob.Length);

                                using (MemoryStream ms = new MemoryStream(data))
                                {
                                    BitmapImage img = new BitmapImage();
                                    img.BeginInit();
                                    img.CacheOption = BitmapCacheOption.OnLoad;
                                    img.StreamSource = ms;
                                    img.EndInit();
                                    img.Freeze();
                                    imgNahlad.Source = img;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při zobrazování obrázku: " + ex.Message);
            }
        }

        /// <summary>
        /// Vrací MIME typ podle přípony souboru
        /// </summary>
        private string GetMimeType(string cesta)
        {
            string ext = Path.GetExtension(cesta).ToLower();
            if (ext == ".jpg" || ext == ".jpeg") return "image/jpeg";
            if (ext == ".png") return "image/png";
            if (ext == ".gif") return "image/gif";
            if (ext == ".bmp") return "image/bmp";
            if (ext == ".txt") return "text/plain";
            if (ext == ".pdf") return "application/pdf";
            if (ext == ".doc" || ext == ".docx") return "application/msword";
            if (ext == ".xls" || ext == ".xlsx") return "application/vnd.ms-excel";
            if (ext == ".ppt" || ext == ".pptx") return "application/vnd.ms-powerpoint";
            if (ext == ".zip") return "application/zip";
            if (ext == ".rar") return "application/x-rar-compressed";
            if (ext == ".mp3") return "audio/mpeg";
            if (ext == ".mp4") return "video/mp4";
            if (ext == ".avi") return "video/x-msvideo";
            return "application/octet-stream";
        }
    }
}
