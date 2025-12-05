using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class TypSouteze
    {
        /// <summary>
        /// Slovník pro definici typu soutěží a jejich ID
        /// </summary>
        public Dictionary<int, string> TypySoutezi { get; private set; }

        /// <summary>
        /// Parametrický konstruktor pro naplnění slovníku
        /// </summary>
        /// <param name="conn">OracleConnection pro připojení do Oracle databáze</param>
        public TypSouteze(OracleConnection conn)
        {
            TypySoutezi = new Dictionary<int, string>();
            using var cmd = new OracleCommand("SELECT * FROM TYP_SOUTEZ_VIEW", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int? id = null;
                string? nazev = null;

                // IDTYPSOUTEZE - NOT NULL
                if (reader["IDTYPSOUTEZE"] != DBNull.Value)
                    id = Convert.ToInt32(reader["IDTYPSOUTEZE"]);

                // NAZEVSOUTEZE - NOT NULL
                if (reader["NAZEVSOUTEZE"] != DBNull.Value)
                    nazev = reader["NAZEVSOUTEZE"].ToString();

                if (id != null && nazev != null)
                    TypySoutezi.Add((int)id, nazev);
            }
        }
    }
}
