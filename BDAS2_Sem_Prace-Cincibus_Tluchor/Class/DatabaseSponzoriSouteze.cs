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
    internal static class DatabaseSponzoriSouteze
    {
        private static OracleConnection GetConnection()
        {
            return DatabaseManager.GetConnection(); // využijeme metodu z DatabaseManager
        }

        public static void AddSponzoriSouteze(Soutez soutez, Sponzor sponzor)
        {
            using var conn = GetConnection();
            conn.Open();

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
    }
}
