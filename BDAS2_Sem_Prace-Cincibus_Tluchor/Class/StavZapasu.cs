using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Prázdný konstruktor pro naplnění slovníku
        /// </summary>
        public StavZapasu()
        {
            StavyZapasu = new Dictionary<int, string>();
            StavyZapasu.Add(1, "Odehráno");
            StavyZapasu.Add(2, "Bude se hrát");
        }
    }
}
