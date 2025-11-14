using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    /// <summary>
    /// Třída pro práci s vazbami sponzori-souteze v databázi
    /// Obsahuje metody pro přidání a odstranění vazeb sponzori-souteze
    /// </summary>
    internal static class DatabaseSponzoriSouteze
    {
        /// <summary>
        /// Metoda slouží k přidání vazby SPONZORI_SOUTEZE do databáze
        /// </summary>
        /// <param name="soutez">Soutěž, ke které chceme přidat novou vazbu</param>
        /// <param name="sponzor">Sponzor, ke kterému chceme přidat novou vazbu</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void AddSponzoriSouteze(OracleConnection conn, Soutez soutez, Sponzor sponzor)
        {
            using (var cmd = new OracleCommand("PKG_SPONZORI_SOUTEZE.SP_ADD_SPONZORI_SOUTEZE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;
                cmd.Parameters.Add("v_id_soutez", OracleDbType.Int32).Value = soutez.IdSoutez;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ADD_SPONZORI_SOUTEZE: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání vazby SPONZORI_SOUTEZE z databáze
        /// </summary>
        /// <param name="soutez">Soutěž, u které chceme odebrat danou vazbu</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberSponzoriSouteze(OracleConnection conn, Soutez soutez, Sponzor sponzor)
        {
            using (var cmd = new OracleCommand("PKG_SPONZORI_SOUTEZE.SP_ODEBER_SPONZORI_SOUTEZE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;
                cmd.Parameters.Add("v_id_soutez", OracleDbType.Int32).Value = soutez.IdSoutez;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_SPONZORI_SOUTEZE: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Metoda slouží k odebrání všech vazeb SPONZORI_SOUTEZE u daného sponzora z databáze
        /// </summary>
        /// <param name="sponzor">Sponzor, u kterého chceme odebrat všechny jeho vazby</param>
        /// <param name="conn">Připojení do Oracle databáze</param>
        /// <exception cref="OracleException">Výjimka se vystaví, pokud nastane chyba při volání procedury</exception>
        public static void OdeberVsechnyVazbySponzoriSouteze(OracleConnection conn, Sponzor sponzor)
        {
            using (var cmd = new OracleCommand("PKG_SPONZORI_SOUTEZE.SP_ODEBER_VSECHNY_SPONZORI_SOUTEZE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Naplníme všechny parametry procedury
                cmd.Parameters.Add("v_id_sponzor", OracleDbType.Int32).Value = sponzor.IdSponzor;

                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (OracleException ex)
                {
                    throw new Exception($"Chyba při volání procedury SP_ODEBER_VSECHNY_SPONZORI_SOUTEZE: {ex.Message}", ex);
                }
            }
        }
    }
}
