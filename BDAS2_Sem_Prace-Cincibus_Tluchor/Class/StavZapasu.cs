using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class StavZapasu
    {
        /// <summary>
        /// Slovník pro definici stavů a jejich ID
        /// </summary>
        public Dictionary<int, string> StavyZapasu { get; private set; }

        /// <summary>
        /// Parametrický konstruktor pro naplnění slovníku
        /// </summary>
        /// <param name="conn">OracleConnection pro připojení do Oracle databáze</param>
        public StavZapasu(OracleConnection conn)
        {
            StavyZapasu = new Dictionary<int, string>();
            using var cmd = new OracleCommand("SELECT * FROM STAV_ZAPASU_VIEW", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int? id = null;
                string? nazev = null;

                // IDSTAV - NOT NULL
                if (reader["IDSTAV"] != DBNull.Value)
                    id = Convert.ToInt32(reader["IDSTAV"]);

                // STAVZAPASU - NOT NULL
                if (reader["STAVZAPASU"] != DBNull.Value)
                    nazev = reader["STAVZAPASU"].ToString();

                if (id != null && nazev != null)
                    StavyZapasu.Add((int)id, nazev);
            }
        }
    }
}
