using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Windows
{
    /// <summary>
    /// Okno pro správu binárního obsahu v databázi
    /// Umožňuje vkládat, mazat, nahrazovat, stahovat soubory a  zobrazovat obrázky
    /// </summary>
    public partial class BinarniObsahOkno : Window
    {
        private byte[] obsahSouboru;
        private string nazevSouboru;
        private string priponaSouboru;
        private bool jeVyhledavaniAktivni = false;

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
        }

        /// <summary>
        /// Metoda slouží k zobrazení dialogu k filtrování a následně vyfiltrované záznamy zobrazí v Datagridu
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void BtnNajdi_Click(object sender, RoutedEventArgs e)
        {
            DialogNajdiBinarniObsah dialogNajdiBinarniObsah = new DialogNajdiBinarniObsah(ObsahData);
            bool? vysledekDiaOkna = dialogNajdiBinarniObsah.ShowDialog();

            if (vysledekDiaOkna == true)
            {
                if (dialogNajdiBinarniObsah.VyfiltrovanyObsah.Count() == 0)
                {
                    MessageBox.Show("Nenašly se žádné záznamy se zadanými filtry", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show("Pokud je vyhledávací mód aktivní nemůžete přidávat, odebírat ani upravovat vyhledaná data. " +
                                "Pro ukončení vyhledávacího módu stiskněte klávesy CTRL X", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                dgBinarniObsah.ItemsSource = new ObservableCollection<BinarniObsah>(dialogNajdiBinarniObsah.VyfiltrovanyObsah);
                jeVyhledavaniAktivni = true;
            }
        }

        /// <summary>
        /// Metoda slouží k zrušení vyhledávacího módu, pokud se zmáčkne klávesa CTRL + X
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Zrušení vyhledávacího módu při zmáčknutí klávesy CTRL + X
            if (jeVyhledavaniAktivni && (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X))
            {
                jeVyhledavaniAktivni = false;
                dgBinarniObsah.ItemsSource = ObsahData;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Tlačítko Zpět (vrátí se do Nastavení)
        /// </summary>
        private void BtnZpet_Click(object sender, System.EventArgs e) 
        { 
            NastaveniOkno nastaveniOkno = new NastaveniOkno(hlavniOkno);
            nastaveniOkno.Show();
            this.Close();
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

                    // SQL dotaz na view, seřazené od nejnovějšího
                    string sql = "SELECT * FROM BINARNI_OBSAH_ROLE_VIEW ORDER BY DATUMNAHRANI DESC";

                    using (var cmd = new OracleCommand(sql, conn)) // Vytvoří SQL příkaz včetně připojení
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roleText;

                            // ROLE
                            if (reader["ROLE"] == DBNull.Value)
                            {
                                roleText = "Neznámá role";
                            }
                            else 
                            {
                                roleText = reader["ROLE"].ToString();
                            }

                            BinarniObsah zaznam = new BinarniObsah();

                            zaznam.IdObsah = Convert.ToInt32(reader["IDBINARNIOBSAH"]);

                            // NAZEV SOUBORU
                            if (reader["NAZEVSOUBORU"] == DBNull.Value)
                                zaznam.NazevSouboru = "";
                            else
                                zaznam.NazevSouboru = reader["NAZEVSOUBORU"].ToString();

                            // TYP SOUBORU
                            if (reader["TYPSOUBORU"] == DBNull.Value)
                                zaznam.TypSouboru = "";
                            else
                                zaznam.TypSouboru = reader["TYPSOUBORU"].ToString();

                            // PRIPONA
                            if (reader["PRIPONASOUBORU"] == DBNull.Value)
                                zaznam.PriponaSouboru = "";
                            else
                                zaznam.PriponaSouboru = reader["PRIPONASOUBORU"].ToString();

                            // DATUM NAHRANI
                            if (reader["DATUMNAHRANI"] == DBNull.Value)
                                zaznam.DatumNahrani = DateTime.MinValue;
                            else
                                zaznam.DatumNahrani = Convert.ToDateTime(reader["DATUMNAHRANI"]);

                            // DATUM MODIFIKACE
                            if (reader["DATUMMODIFIKACE"] == DBNull.Value)
                                zaznam.DatumModifikace = DateTime.MinValue;
                            else
                                zaznam.DatumModifikace = Convert.ToDateTime(reader["DATUMMODIFIKACE"]);

                            // OPERACE
                            if (reader["OPERACE"] == DBNull.Value)
                                zaznam.Operace = "";
                            else
                                zaznam.Operace = reader["OPERACE"].ToString();

                            // ID UZIVATELSKY UCET
                            if (reader["IDUZIVATELSKYUCET"] == DBNull.Value)
                                zaznam.IdUzivatelskyUcet = 0;
                            else
                                zaznam.IdUzivatelskyUcet = Convert.ToInt32(reader["IDUZIVATELSKYUCET"]);

                            // ROLE 
                            zaznam.Uzivatel = roleText;

                            // Přidání do kolekce
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
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Všechny soubory|*.*" };

            if (dialog.ShowDialog() == true)
            {
                string cesta = dialog.FileName;

                // Název souboru bez přípony
                nazevSouboru = Path.GetFileNameWithoutExtension(cesta);

                // Přípona bez tečky
                priponaSouboru = Path.GetExtension(cesta).TrimStart('.').ToLower();

                // Kontrola délek
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

                // Určení MIME typu
                string mimeType = GetMimeType(cesta);
                tboxNazev.Text = nazevSouboru;
                tboxPripona.Text = priponaSouboru;
                tboxTyp.Text = mimeType;

                // Načtení souboru do byte[]
                obsahSouboru = File.ReadAllBytes(cesta);

                // Zobrazení náhledu, pokud je to obrázek
                if (mimeType.StartsWith("image"))
                {
                    try
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri(cesta);
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        imgNahled.Source = img;
                    }
                    catch
                    {
                        imgNahled.Source = null;
                        MessageBox.Show("Obrázek se nepodařilo načíst", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    // Pokud to není obrázek, vyčistit náhled
                    imgNahled.Source = null;
                }

                // Stavová hláška
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
                MessageBox.Show("Nejdříve vyberte soubor", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrola maximální délky textových polí podle omezení databáze
            if (tboxNazev.Text.Length > 50 || tboxTyp.Text.Length > 50 || tboxPripona.Text.Length > 10)
            {
                MessageBox.Show("Některá pole přesahují maximální délku");
                return;
            }

            try
            {
                // Získání přihlášeného uživatele z hlavního okna, vrací string
                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                if (prihlaseny == null)
                {
                    MessageBox.Show("Není přihlášen žádný uživatel");
                    return;
                }

                // Získání ID role podle string (Admin, Host, ...) z databáze ROLE
                int idRole = ZiskejIdRole(prihlaseny.Role);

                int idUzivatelskyUcet = 0;

                // Otevření připojení k databázi
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

                MessageBox.Show("Soubor byl uložen", "Uložení", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        if (result != null) {
                            return Convert.ToInt32(result);
                        }
                          
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
        private void DgBinarniObsah_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek == null)
            {
                return;
            } 

            string novyNazev = Interaction.InputBox("Zadejte nový název souboru:", "Přejmenování", vybranyRadek.NazevSouboru);

            // Pokud uživatel stiskne Storno nebo nezadá nic, ukončíme akci
            if (string.IsNullOrWhiteSpace(novyNazev))
            {
                return;
            } 

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
                        cmd.Parameters.Add("id", vybranyRadek.IdObsah);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Název byl změněn", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek == null)
            {
                MessageBox.Show("Vyberte soubor, který chcete nahradit", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Nebyl vybrán žádný nový soubor", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Získáme ID role pro uživatele 
                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                int idRole = ZiskejIdRole(prihlaseny.Role);

                int idUzivatelskyUcet = 0;
                using (var conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT IDUZIVATELSKYUCET FROM UZIVATELSKE_UCTY WHERE IDROLE = :idRole FETCH FIRST 1 ROWS ONLY";

                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("idRole", idRole);
                        object result = cmd.ExecuteScalar();
                        idUzivatelskyUcet = Convert.ToInt32(result);
                    }
                }

                // UPDATE 
                DatabaseBinarniObsah.UpdateBinarniObsah(
                    vybranyRadek.IdObsah,
                    obsahSouboru,
                    "uprava",
                    idUzivatelskyUcet
                );

                MessageBox.Show("Soubor byl úspěšně nahrazen", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek  == null)
            {
                MessageBox.Show("Vyberte soubor ke smazání", "Chybějící výběr", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var potvrzeni = MessageBox.Show($"Opravdu chcete smazat soubor '{vybranyRadek.NazevSouboru}'?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (potvrzeni != MessageBoxResult.Yes) 
            {
                return;
            }

            try
            {
                DatabaseBinarniObsah.DeleteBinarniObsah(vybranyRadek.IdObsah);
                MessageBox.Show("Soubor byl smazán", "Smazání úspěšné", MessageBoxButton.OK, MessageBoxImage.Information);

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
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek == null)
            {
                MessageBox.Show("Vyberte soubor ke stažení", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        cmd.Parameters.Add("id", vybranyRadek.IdObsah);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] data = (byte[])reader["OBSAH"];
                                string pripona = reader["PRIPONASOUBORU"].ToString();

                                // Otevře dialog pro výběr, kam se má soubor uložit
                                SaveFileDialog dialog = new SaveFileDialog
                                {
                                    FileName = vybranyRadek.NazevSouboru + "." + pripona
                                };


                                if (dialog.ShowDialog() == true)
                                {
                                    File.WriteAllBytes(dialog.FileName, data);
                                    MessageBox.Show("Soubor byl uložen na disk", "Uložení souboru", MessageBoxButton.OK, MessageBoxImage.Information);

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
            // Získá vybraný řádek z DataGridu a pokusí se ho převést na objekt BinarniObsah
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek == null)
            {
                MessageBox.Show("Nejdříve vyberte soubor z tabulky", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string pripona = vybranyRadek.PriponaSouboru.ToLower();

            // Kontrola, zda soubor má příponu podporovaného typu obrázku
            if (pripona != "jpg" && pripona != "jpeg" && pripona != "png" && pripona != "gif" && pripona != "bmp")
            {
                MessageBox.Show("Tento typ souboru nelze zobrazit jako obrázek", "Nepodporovaný formát", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Pokud typ není podporovaný - clear náhled obrázku
                imgNahled.Source = null;
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
                        cmd.Parameters.Add("id", vybranyRadek.IdObsah);

                        // Spuštění a načtení výsledku s režimem pro čtení velkých dat
                        using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess))
                        {
                            if (reader.Read())
                            {
                                // Získá BLOB z databáze
                                var blob = reader.GetOracleBlob(0);

                                if (blob == null || blob.IsNull)
                                {
                                    MessageBox.Show("Soubor neobsahuje žádná data");
                                    return;
                                }

                                // Vytvoří byte pole o velikosti BLOBu
                                byte[] data = new byte[blob.Length];

                                // Načte všechna data z BLOBu do pole byte[]
                                blob.Read(data, 0, (int)blob.Length);

                                // Pomocí MemoryStreamu zobrazí obrázek
                                using (MemoryStream ms = new MemoryStream(data))
                                {
                                    BitmapImage img = new BitmapImage();
                                    img.BeginInit();
                                    img.CacheOption = BitmapCacheOption.OnLoad;
                                    img.StreamSource = ms;
                                    img.EndInit();
                                    img.Freeze();
                                    imgNahled.Source = img;
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
