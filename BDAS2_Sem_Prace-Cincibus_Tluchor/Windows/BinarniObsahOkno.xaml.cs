using BDAS2_Sem_Prace_Cincibus_Tluchor.Class;
using BDAS2_Sem_Prace_Cincibus_Tluchor.Windows.Search_Dialogs;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using MigraDoc.DocumentObjectModel.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Themes;

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
        private bool zavrenoTlacitkem = false;

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
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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
            zavrenoTlacitkem = true;
            NastaveniOkno.Instance.Show();
            this.Hide();
        }

        /// <summary>
        /// Ukončí aplikaci stistknutím X
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!zavrenoTlacitkem)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Načte všechny soubory z pohledu BINARNI_OBSAH_ROLE_VIEW
        /// </summary>
        private void NactiBinarniObsah()
        {
            ObsahData.Clear();

            try
            {
                var conn = DatabaseManager.GetConnection();

                string sql = "SELECT * FROM BINARNI_OBSAH_ROLE_VIEW ORDER BY DATUMNAHRANI DESC";

                using (var cmd = new OracleCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string roleText = reader["ROLE"] == DBNull.Value ? "Neznámá role" : reader["ROLE"].ToString();

                        BinarniObsah zaznam = new BinarniObsah
                        {
                            IdObsah = Convert.ToInt32(reader["IDBINARNIOBSAH"]),
                            NazevSouboru = reader["NAZEVSOUBORU"] == DBNull.Value ? "" : reader["NAZEVSOUBORU"].ToString(),
                            TypSouboru = reader["TYPSOUBORU"] == DBNull.Value ? "" : reader["TYPSOUBORU"].ToString(),
                            PriponaSouboru = reader["PRIPONASOUBORU"] == DBNull.Value ? "" : reader["PRIPONASOUBORU"].ToString(),
                            DatumNahrani = reader["DATUMNAHRANI"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATUMNAHRANI"]),
                            DatumModifikace = reader["DATUMMODIFIKACE"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATUMMODIFIKACE"]),
                            Operace = reader["OPERACE"] == DBNull.Value ? "" : reader["OPERACE"].ToString(),
                            IdUzivatelskyUcet = reader["IDUZIVATELSKYUCET"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IDUZIVATELSKYUCET"]),
                            Uzivatel = roleText
                        };

                        ObsahData.Add(zaznam);
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

                nazevSouboru = Path.GetFileNameWithoutExtension(cesta);
                priponaSouboru = Path.GetExtension(cesta).TrimStart('.').ToLower();

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

                obsahSouboru = File.ReadAllBytes(cesta);

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
                    imgNahled.Source = null;
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
                MessageBox.Show("Nejdříve vyberte soubor", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                int idUzivatelskyUcet = ZiskejIdUzivatelskyUcet(prihlaseny.Role);

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
        /// Vrací ID role podle jejího názvu z tabulky ROLE
        /// </summary>
        private int ZiskejIdRole(string nazevRole)
        {
            try
            {
                var conn = DatabaseManager.GetConnection();
                string sql = "SELECT IDROLE FROM ROLE_VIEW WHERE LOWER(NAZEVROLE) = LOWER(:nazevRole)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazevRole", OracleDbType.Varchar2).Value = nazevRole;
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při zjišťování role: " + ex.Message);
            }
            return -1;
        }

        /// <summary>
        /// Vrátí ID uživatelského účtu podle role
        /// </summary>
        private int ZiskejIdUzivatelskyUcet(string role)
        {
            int idUzivatel = 0;
            try
            {
                var conn = DatabaseManager.GetConnection();
                string sql = @"SELECT IDUZIVATELSKYUCET
                               FROM PREHLED_UZIVATELSKE_UCTY
                               WHERE ROLE = :role
                               FETCH FIRST 1 ROWS ONLY";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("role", OracleDbType.Varchar2).Value = role;
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        idUzivatel = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při získávání ID uživatele: " + ex.Message);
            }
            return idUzivatel;
        }

        /// <summary>
        /// Přejmenuje soubor po dvojkliku v Datagridu
        /// </summary>
        private void DgBinarniObsah_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;

            if (vybranyRadek == null)
                return;

            string novyNazev = Interaction.InputBox(
                "Zadejte nový název souboru:",
                "Přejmenování",
                vybranyRadek.NazevSouboru
            );

            if (string.IsNullOrWhiteSpace(novyNazev))
                return;

            if (novyNazev.Length > 50)
            {
                MessageBox.Show("Název souboru je příliš dlouhý (max. 50 znaků)");
                return;
            }

            try
            {
                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                if (prihlaseny == null)
                {
                    MessageBox.Show("Není přihlášen žádný uživatel!");
                    return;
                }

                int idRole = ZiskejIdRole(prihlaseny.Role);
                int idRoleUzivatel = ZiskejIdUzivatelskyUcet(prihlaseny.Role);

                DatabaseBinarniObsah.RenameBinarniObsah(
                    vybranyRadek.IdObsah,
                    novyNazev,
                    idRoleUzivatel
                );

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

            var potvrzeni = MessageBox.Show("Opravdu chcete nahradit vybraný soubor novým?", "Nahrazení souboru", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

            try
            {
                BtnVybratSoubor_Click(sender, e);
                if (obsahSouboru == null)
                {
                    MessageBox.Show("Nebyl vybrán žádný nový soubor", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                int idUzivatelskyUcet = ZiskejIdUzivatelskyUcet(prihlaseny.Role);

                DatabaseBinarniObsah.UpdateBinarniObsah(
                    vybranyRadek.IdObsah,
                    obsahSouboru,
                    "uprava",
                    idUzivatelskyUcet
                );

                MessageBox.Show("Soubor byl úspěšně nahrazen", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (vybranyRadek == null)
            {
                MessageBox.Show("Vyberte soubor ke smazání", "Chybějící výběr", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var potvrzeni = MessageBox.Show($"Opravdu chcete smazat soubor '{vybranyRadek.NazevSouboru}'?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (potvrzeni != MessageBoxResult.Yes)
                return;

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
                var conn = DatabaseManager.GetConnection();
                string sql = "SELECT OBSAH, PRIPONASOUBORU FROM BINARNI_OBSAH_ROLE_VIEW WHERE IDBINARNIOBSAH = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = vybranyRadek.IdObsah;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // BLOB z DB
                            byte[] data = reader["OBSAH"] as byte[];

                            // PŘÍPONA
                            string pripona = "";

                            if (reader["PRIPONASOUBORU"] != DBNull.Value && reader["PRIPONASOUBORU"] != null)
                            {
                                pripona = reader["PRIPONASOUBORU"].ToString();
                                if (pripona != null)
                                {
                                    pripona = pripona.Trim().ToLower();
                                }
                            }

                            string fileName = vybranyRadek.NazevSouboru;
                            if (!string.IsNullOrEmpty(pripona))
                            {
                                fileName = fileName + "." + pripona;
                            }

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.FileName = fileName;               
                            sfd.Filter = "Všechny soubory|*.*";

                            if (sfd.ShowDialog() == true)
                            {
                                File.WriteAllBytes(sfd.FileName, data);
                                MessageBox.Show("Soubor uložen", "Úspěch",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Zobrazí obrázek vybraného souboru, pokud je to obrázek
        /// </summary>
        private void BtnZobrazitObrazek_Click(object sender, RoutedEventArgs e)
        {
            var vybranyRadek = dgBinarniObsah.SelectedItem as BinarniObsah;
            if (vybranyRadek == null)
                return;

            try
            {
                var conn = DatabaseManager.GetConnection();
                string sql = "SELECT OBSAH FROM BINARNI_OBSAH_ROLE_VIEW WHERE IDBINARNIOBSAH = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = vybranyRadek.IdObsah;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] data = reader["OBSAH"] as byte[];
                            if (data != null && data.Length > 0)
                            {
                                BitmapImage img = new BitmapImage();
                                using (var ms = new MemoryStream(data))
                                {
                                    img.BeginInit();
                                    img.CacheOption = BitmapCacheOption.OnLoad;
                                    img.StreamSource = ms;
                                    img.EndInit();
                                }
                                imgNahled.Source = img;
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
