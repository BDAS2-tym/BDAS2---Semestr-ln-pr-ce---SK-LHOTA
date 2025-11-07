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
        /// Prázdný konstruktor pro naplnění slovníku
        /// </summary>
        public TypSouteze()
        {
            TypySoutezi = new Dictionary<int, string>();
            TypySoutezi.Add(1, "Liga");
            TypySoutezi.Add(2, "Pohár");
            TypySoutezi.Add(3, "Divize");
            TypySoutezi.Add(4, "Kraj");
            TypySoutezi.Add(5, "Okers");
        }
    }
}
