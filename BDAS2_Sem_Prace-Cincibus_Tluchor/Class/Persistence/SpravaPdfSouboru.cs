using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Internals;
using MigraDoc.Rendering;
using Oracle.ManagedDataAccess.Client;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Persistence
{
    /// <summary>
    /// Třída obsahuje metody potřebné ke správě PDF souborů
    /// </summary>
    public class SpravaPdfSouboru
    {
        /// <summary>
        /// Metoda slouží k uložení kontraktu hráče do souboru PDF
        /// </summary>
        /// <param name="cestaProSoubor">Cesta pro uložení PDF souboru</param>
        /// <param name="kontrakt">Kontrakt, ze kterého chceme exportovat údaje</param>
        /// <exception cref="ArgumentException">Výjimka se vystaví, pokud je zadaná cesta pro soubor prázdná nebo NULL</exception>
        /// <exception cref="ArgumentNullException">Výjimka se vystaví, pokud je některý z parametrů NULL</exception>
        /// <exception cref="IOException">Výjimka se vystaví, pokud nastane chyba při zapisování dat do souboru</exception>
        public static void UlozHracuvKontrakt(string cestaProSoubor, Kontrakt? kontrakt)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(cestaProSoubor))
                {
                    throw new ArgumentException("Cesta pro soubor nesmí být prázdná ani NULL!");
                }

                if (kontrakt is null)
                {
                    throw new ArgumentNullException("Kontrakt nemůže být NULL!");
                }

                // Dokument
                Document doc = new Document();
                Section sekce = doc.AddSection();
                sekce.PageSetup.PageFormat = PageFormat.A4;
                sekce.PageSetup.TopMargin = Unit.FromCentimeter(2);
                sekce.PageSetup.BottomMargin = Unit.FromCentimeter(2);
                sekce.PageSetup.LeftMargin = Unit.FromCentimeter(2);
                sekce.PageSetup.RightMargin = Unit.FromCentimeter(2);

                // Logo
                Paragraph logo = sekce.AddParagraph();
                logo.Format.Alignment = ParagraphAlignment.Right;
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Logo_SK_Lhota.png");

                MigraDoc.DocumentObjectModel.Shapes.Image img = logo.AddImage(imagePath);
                img.Width = Unit.FromCentimeter(3.2);
                img.LockAspectRatio = true;
                logo.Format.SpaceAfter = Unit.FromCentimeter(0.5);

                // Nadpis
                Paragraph nadpis = sekce.AddParagraph("KONTRAKT HRÁČE");
                nadpis.Format.Font.Name = "Times New Roman";
                nadpis.Format.Font.Size = 22;
                nadpis.Format.Font.Bold = true;
                nadpis.Format.Alignment = ParagraphAlignment.Center;
                nadpis.Format.SpaceAfter = Unit.FromCentimeter(0.5);

                // Oddělovač
                Paragraph line = sekce.AddParagraph();
                line.Format.Borders.Bottom.Width = Unit.FromPoint(2);
                line.Format.Borders.Bottom.Color = Colors.DarkGray;
                line.Format.SpaceAfter = Unit.FromCentimeter(0.35);

                // Osobní údaje hráče
                Paragraph osobniUdaje = sekce.AddParagraph("Osobní údaje:");
                osobniUdaje.Format.Font.Name = "Times New Roman";
                osobniUdaje.Format.Font.Size = 14;
                osobniUdaje.Format.Font.Bold = true;
                osobniUdaje.Format.SpaceAfter = Unit.FromCentimeter(0.2);

                PridejInfo(sekce, "Jméno", kontrakt.KontraktHrace.Jmeno, ParagraphAlignment.Left);
                PridejInfo(sekce, "Příjmení", kontrakt.KontraktHrace.Prijmeni, ParagraphAlignment.Left);
                PridejInfo(sekce, "Rodné číslo", kontrakt.KontraktHrace.RodneCislo.ToString(), ParagraphAlignment.Left);
                PridejInfo(sekce, "Telefonní číslo agenta", kontrakt.TelCisloNaAgenta, ParagraphAlignment.Left);

                // Platové podmínky
                Paragraph platove = sekce.AddParagraph("Finanční podmínky:");
                platove.Format.Font.Name = "Times New Roman";
                platove.Format.Font.Size = 14;
                platove.Format.Font.Bold = true;
                platove.Format.SpaceBefore = Unit.FromCentimeter(0.5);
                platove.Format.SpaceAfter = Unit.FromCentimeter(0.2);

                PridejInfo(sekce, "Měsíční plat", kontrakt.Plat + " Kč", ParagraphAlignment.Left);
                PridejInfo(sekce, "Výstupní klauzule", kontrakt.VystupniKlauzule + " Kč", ParagraphAlignment.Left);

                // Platnost kontraktu
                Paragraph platnost = sekce.AddParagraph("Platnost kontraktu:");
                platnost.Format.Font.Name = "Times New Roman";
                platnost.Format.Font.Size = 14;
                platnost.Format.Font.Bold = true;
                platnost.Format.SpaceBefore = Unit.FromCentimeter(0.5);
                platnost.Format.SpaceAfter = Unit.FromCentimeter(0.2);

                PridejInfo(sekce, "Od", kontrakt.DatumZacatku.ToString("dd.MM.yyyy"), ParagraphAlignment.Left);
                PridejInfo(sekce, "Do", kontrakt.DatumKonce.ToString("dd.MM.yyyy"), ParagraphAlignment.Left);

                // Přidání podmínek kontraktu do PDF
                Paragraph podminky = sekce.AddParagraph("Podmínky klubu:");
                podminky.Format.Font.Name = "Times New Roman";
                podminky.Format.Font.Size = 14;
                podminky.Format.Font.Bold = true;
                podminky.Format.SpaceBefore = Unit.FromCentimeter(1.7);
                podminky.Format.SpaceAfter = Unit.FromCentimeter(1);

                // Pole s body kontraktu
                (string nadpis, string text)[] body = new (string, string)[]
                {
                    ("Trénink a soutěžní povinnosti", "Hráč se zavazuje účastnit se všech tréninků, přípravných zápasů a oficiálních soutěžních utkání dle pokynů klubu."),
                    ("Zdravotní a lékařské podmínky", "Hráč se zavazuje udržovat optimální fyzickou kondici a pravidelně podstupovat lékařské prohlídky.\r\nHráč je povinen informovat klub o jakýchkoli zdravotních problémech, které mohou ovlivnit jeho výkonnost."),
                    ("Chování", "Hráč se zavazuje respektovat etický kodex klubu, včetně jednání na veřejnosti, sociálních sítích a mediálních vystoupeních.\r\nNepřijatelné chování (např. násilí, porušování zákonů) může být důvodem k ukončení kontraktu."),
                    ("Omezení činnosti mimo klub", "Hráč nesmí během platnosti kontraktu vykonávat placené sportovní aktivity pro jiné kluby nebo organizace bez písemného souhlasu klubu."),
                    ("Bezpečnost a pravidla", "Hráč se zavazuje dodržovat všechny interní pokyny klubu týkající se bezpečnosti, tréninku a sportovních aktivit."),
                    ("Plat a bonusy", "Plat hráče může být upraven podle sponzorských smluv a disciplinárních opatření:\r\n - Pokud hráč získá tři a více sponzorských smluv, jeho měsíční plat bude zvýšen o 5 %.\r\n - V případě udělení disciplinárního opatření bude měsíční plat snížen o 5 %."),
                    ("Změny kontraktu", "Všechny změny kontraktu musí být provedeny písemně a potvrzeny oběma stranami.")
                };

                // Vložení každého bodu do PDF s nadpisem tučně
                for (int i = 0; i < body.Length; i++)
                {
                    // Nadpis bodu (tučně)
                    Paragraph nadpisPar = sekce.AddParagraph();
                    nadpisPar.Format.Font.Name = "Times New Roman";
                    nadpisPar.Format.Font.Size = 12;
                    nadpisPar.Format.Font.Bold = true;
                    nadpisPar.Format.SpaceBefore = Unit.FromCentimeter(0.5);
                    nadpisPar.Format.SpaceAfter = Unit.FromCentimeter(0.2);
                    nadpisPar.Format.LeftIndent = Unit.FromCentimeter(1);
                    nadpisPar.AddText($"{i + 1}. {body[i].nadpis}");

                    // Text bodu (normální)
                    Paragraph textPar = sekce.AddParagraph();
                    textPar.Format.Font.Name = "Times New Roman";
                    textPar.Format.Font.Size = 11;
                    textPar.Format.SpaceAfter = Unit.FromCentimeter(0.5);
                    textPar.Format.LeftIndent = Unit.FromCentimeter(1.5);
                    textPar.AddText(body[i].text);
                }

                // Místo a datum
                Paragraph datumPodpisu = sekce.AddParagraph("Kontrakt byl podepsán v ________________________     dne ________________________");
                datumPodpisu.Format.Font.Name = "Times New Roman";
                datumPodpisu.Format.Font.Size = 12;
                datumPodpisu.Format.SpaceBefore = Unit.FromCentimeter(1);

                // Podpisy
                Paragraph podpisy = sekce.AddParagraph();
                podpisy.Format.SpaceBefore = Unit.FromCentimeter(1.5);
                podpisy.Format.Font.Name = "Times New Roman";
                podpisy.Format.Font.Size = 12;
                podpisy.AddText("Podpis hráče: ______________________");
                podpisy.Format.SpaceAfter = Unit.FromCentimeter(1);

                Paragraph podpisZastupce = sekce.AddParagraph();
                podpisZastupce.Format.Font.Name = "Times New Roman";
                podpisZastupce.Format.Font.Size = 12;
                podpisZastupce.AddText("Podpis zástupce klubu: ______________________");

                // Patička
                Paragraph paticka = sekce.Footers.Primary.AddParagraph();
                paticka.AddText($"Kontrakt byl vystaven dne: {DateTime.Now:dd.MM.yyyy}");
                paticka.Format.Alignment = ParagraphAlignment.Right;
                paticka.Format.Font.Size = 9;
                paticka.Format.Font.Name = "Times New Roman";

                // Render PDF
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                renderer.PdfDocument.Save(cestaProSoubor);
            }

            catch (IOException ex)
            {
                throw new IOException($"Nastala chyba při ukládání do souboru: {ex.Message}");
            }
        }

        /// <summary>
        /// Metoda slouží k uložení kontraktu hráče do databáze
        /// </summary>
        /// <param name="cestaKSouboru">Cesta importovaného souboru</param>
        /// <exception cref="Exception">Obecná výjimka</exception>
        public static void NahrajHracuvKontrakt(string cestaKSouboru)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(cestaKSouboru))
                {
                    throw new ArgumentException("Cesta k souboru nesmí být prázdná ani NULL!");
                }

                var prihlaseny = HlavniOkno.GetPrihlasenyUzivatel();
                if (prihlaseny == null)
                {
                    throw new Exception("Není přihlášen žádný uživatel!");
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
                            throw new Exception("Pro danou roli neexistuje žádný účet v databázi!");
                        }

                        idUzivatelskyUcet = Convert.ToInt32(result);
                    }
                }

                byte[] obsah = File.ReadAllBytes(cestaKSouboru);

                DatabaseBinarniObsah.AddBinarniObsah(Path.GetFileNameWithoutExtension(cestaKSouboru), GetMimeType(cestaKSouboru),
                    Path.GetExtension(cestaKSouboru).TrimStart('.').ToLower(), obsah, "vlozeni", idUzivatelskyUcet);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Pomocná metoda slouží k přidání textu do vybraných sekcí a také upravuje formát zarovnání
        /// </summary>
        /// <param name="sekce">Název sekce v dokumentu</param>
        /// <param name="label">Štítek pro text</param>
        /// <param name="text">Přidávaný text</param>
        /// <param name="zarovnani">Styl zarovnání</param>
        private static void PridejInfo(Section sekce, string label, string text, ParagraphAlignment zarovnani)
        {
            Paragraph p = sekce.AddParagraph();
            p.Format.Font.Name = "Times New Roman";
            p.Format.Alignment = zarovnani;
            p.Format.Font.Size = 11;
            p.Format.SpaceAfter = Unit.FromCentimeter(0.3);
            p.Format.SpaceBefore = Unit.FromCentimeter(0.3);
            p.AddFormattedText($"{label}: ", TextFormat.Bold);
            p.AddText(text);
        }

        /// <summary>
        /// Vrací MIME typ podle přípony souboru
        /// </summary>
        private static string GetMimeType(string cesta)
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

        /// <summary>
        /// Vrátí ID role podle jejího názvu z tabulky ROLE
        /// </summary>
        private static int ZiskejIdRole(string nazevRole)
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
                throw new Exception(ex.Message);
            }

            return -1;
        }
    }
}
