using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public static class DatabaseAppUser
    {
        /// <summary>
        /// Metoda slouží k nastavení přihlášeného uživatele, aby fungovala logování změn
        /// </summary>
        /// <param name="conn">Databázové připojení do Oracle Database</param>
        /// <param name="prihlasenyUzivatel">Aktuálně přihlášený uživatel</param>
        public static void SetAppUser(OracleConnection conn, Uzivatel prihlasenyUzivatel)
        {
            using (var cmd = new OracleCommand("pkg_user_context.set_user", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_user", OracleDbType.Varchar2).Value = prihlasenyUzivatel.UzivatelskeJmeno;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
