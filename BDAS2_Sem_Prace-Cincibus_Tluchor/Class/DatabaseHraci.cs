using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    internal static class DatabaseHraci
    {
        private static OracleConnection GetConnection()
        {
            return DatabaseManager.GetConnection(); // využijeme metodu z DatabaseManager
        }

        public static void AddHrac(Hrac hrac)
        {
            if (string.IsNullOrWhiteSpace(hrac.PoziceNaHristi))
                throw new Exception("Nelze vložit hráče bez pozice na hřišti!");

            using var conn = GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Nové ID člena
                using var cmdSeq = new OracleCommand("SELECT SEKV_CLENKLUBU.NEXTVAL FROM DUAL", conn);
                cmdSeq.Transaction = transaction;
                int idClenKlubu = Convert.ToInt32(cmdSeq.ExecuteScalar());

                // Vložení do CLENOVE_KLUBU
                using var cmdClen = new OracleCommand(@"
                    INSERT INTO CLENOVE_KLUBU
                    (IDCLENKLUBU, RODNE_CISLO, JMENO, PRIJMENI, TELEFONNICISLO, TYPCLENA)
                    VALUES (:id, :rodne, :jmeno, :prijmeni, :tel, 'Hrac')", conn);
                cmdClen.Transaction = transaction;
                cmdClen.Parameters.Add(":id", idClenKlubu);
                cmdClen.Parameters.Add(":rodne", hrac.RodneCislo);
                cmdClen.Parameters.Add(":jmeno", hrac.Jmeno);
                cmdClen.Parameters.Add(":prijmeni", hrac.Prijmeni);
                cmdClen.Parameters.Add(":tel", hrac.TelefonniCislo);
                cmdClen.ExecuteNonQuery();

                // Získání ID pozice z POZICE_HRAC podle názvu
                using var cmdPozice = new OracleCommand(
                    "SELECT ID_POZICE FROM POZICE_HRAC WHERE NAZEV_POZICE = :nazev", conn);
                cmdPozice.Transaction = transaction;
                cmdPozice.Parameters.Add(":nazev", hrac.PoziceNaHristi);
                object result = cmdPozice.ExecuteScalar();
                if (result == null)
                    throw new Exception($"Pozice '{hrac.PoziceNaHristi}' neexistuje v tabulce pozic!");
                int idPozice = Convert.ToInt32(result);

                // Vložení do HRACI
                using var cmdHrac = new OracleCommand(@"
            INSERT INTO HRACI
            (IDCLENKLUBU, POZICENAHRISTI, POCETVSTRELENYCHGOLU,
             POCET_ZLUTYCH_KARET, POCET_CERVENYCH_KARET, ID_POZICE)
             VALUES (:idClen, :pozice, :goly, :zlute, :cervene, :idPozice)", conn);
                cmdHrac.Transaction = transaction;
                cmdHrac.Parameters.Add(":idClen", idClenKlubu);
                cmdHrac.Parameters.Add(":pozice", hrac.PoziceNaHristi);
                cmdHrac.Parameters.Add(":goly", hrac.PocetVstrelenychGolu);
                cmdHrac.Parameters.Add(":zlute", hrac.PocetZlutychKaret);
                cmdHrac.Parameters.Add(":cervene", hrac.PocetCervenychKaret);
                cmdHrac.Parameters.Add(":idPozice", idPozice);
                cmdHrac.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void UpdateHrac(Hrac hrac)
        {
            using var conn = GetConnection();
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                // UPDATE CLENOVE_KLUBU
                using var cmdClen = new OracleCommand(
                @"UPDATE CLENOVE_KLUBU
                    SET JMENO = :jmeno,
                    PRIJMENI = :prijmeni,
                    TELEFONNICISLO = :tel
                WHERE RODNE_CISLO = :rodne", conn);
                cmdClen.Transaction = transaction;
                cmdClen.Parameters.Add(":jmeno", hrac.Jmeno);
                cmdClen.Parameters.Add(":prijmeni", hrac.Prijmeni);
                cmdClen.Parameters.Add(":tel", hrac.TelefonniCislo);
                cmdClen.Parameters.Add(":rodne", hrac.RodneCislo);
                cmdClen.ExecuteNonQuery();

                // Získání IDCLENKLUBU podle rodného čísla
                using var cmdId = new OracleCommand(
                    "SELECT IDCLENKLUBU FROM CLENOVE_KLUBU WHERE RODNE_CISLO = :rodne", conn);
                cmdId.Transaction = transaction;
                cmdId.Parameters.Add(":rodne", hrac.RodneCislo);
                int idClen = Convert.ToInt32(cmdId.ExecuteScalar());

                // UPDATE HRACI
                using var cmdHrac = new OracleCommand(
                @"UPDATE HRACI
                    SET POZICENAHRISTI = :pozice,
                    POCETVSTRELENYCHGOLU = :goly,
                    POCET_ZLUTYCH_KARET = :zlute,
                    POCET_CERVENYCH_KARET = :cervene
                WHERE IDCLENKLUBU = :idClen", conn);
                cmdHrac.Transaction = transaction;
                cmdHrac.Parameters.Add(":pozice", hrac.PoziceNaHristi);
                cmdHrac.Parameters.Add(":goly", hrac.PocetVstrelenychGolu);
                cmdHrac.Parameters.Add(":zlute", hrac.PocetZlutychKaret);
                cmdHrac.Parameters.Add(":cervene", hrac.PocetCervenychKaret);
                cmdHrac.Parameters.Add(":idClen", idClen);
                cmdHrac.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw; 
            }
        }

        public static void OdeberHrace(Hrac hrac)
        {
            if (hrac == null)
                throw new ArgumentNullException(nameof(hrac), "Objekt hráče nesmí být null.");

            using var conn = DatabaseManager.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Získání ID člena podle rodného čísla
                int idClenKlubu;
                using (var cmdId = new OracleCommand(
                    "SELECT IDCLENKLUBU FROM CLENOVE_KLUBU WHERE RODNE_CISLO = :rodneCislo", conn))
                {
                    cmdId.Transaction = transaction;
                    cmdId.Parameters.Add(":rodneCislo", OracleDbType.Decimal).Value = hrac.RodneCislo;
                    object result = cmdId.ExecuteScalar();

                    if (result == null)
                        throw new Exception($"Člen s rodným číslem {hrac.RodneCislo} nebyl nalezen.");
                    idClenKlubu = Convert.ToInt32(result);
                }

                //  Smazání vazeb ze SPONZORI_CLENOVE (vazební tabulka sponzor–člen)
                using (var cmdSponzor = new OracleCommand(
                    "DELETE FROM SPONZORI_CLENOVE WHERE IDCLENKLUBU = :idClen", conn))
                {
                    cmdSponzor.Transaction = transaction;
                    cmdSponzor.Parameters.Add(":idClen", OracleDbType.Int32).Value = idClenKlubu;
                    cmdSponzor.ExecuteNonQuery();
                }

                // Smazání z HRACI_KONTRAKTY (vazební tabulka hráč–kontrakt)
                using (var cmdVazba = new OracleCommand(
                    "DELETE FROM HRACI_KONTRAKTY WHERE IDCLENKLUBU = :idClen", conn))
                {
                    cmdVazba.Transaction = transaction;
                    cmdVazba.Parameters.Add(":idClen", OracleDbType.Int32).Value = idClenKlubu;
                    cmdVazba.ExecuteNonQuery();
                }

                // Smazání z HRACI (subtyp)
                using (var cmdHrac = new OracleCommand(
                    "DELETE FROM HRACI WHERE IDCLENKLUBU = :idClen", conn))
                {
                    cmdHrac.Transaction = transaction;
                    cmdHrac.Parameters.Add(":idClen", OracleDbType.Int32).Value = idClenKlubu;
                    cmdHrac.ExecuteNonQuery();
                }

                // Smazání z CLENOVE_KLUBU (supertyp)
                using (var cmdClen = new OracleCommand(
                    "DELETE FROM CLENOVE_KLUBU WHERE IDCLENKLUBU = :idClen", conn))
                {
                    cmdClen.Transaction = transaction;
                    cmdClen.Parameters.Add(":idClen", OracleDbType.Int32).Value = idClenKlubu;
                    cmdClen.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (OracleException ex)
            {
                transaction.Rollback();
                throw new Exception($"Databázová chyba při mazání hráče: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Chyba při mazání hráče: {ex.Message}", ex);
            }
        }






    }

}
