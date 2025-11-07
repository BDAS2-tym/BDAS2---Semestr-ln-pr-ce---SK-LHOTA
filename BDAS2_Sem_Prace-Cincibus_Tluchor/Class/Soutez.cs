using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class
{
    public class Soutez
    {
        /// <summary>
        /// Jedinečné ID soutěže
        /// </summary>
        public int IdSoutez {  get; set; }

        /// <summary>
        /// Datum začátku soutěže
        /// </summary>
        public DateOnly StartDatum {  get; set; }

        /// <summary>
        /// Datum konce soutěže
        /// </summary>
        public DateOnly KonecDatum { get; set; }

        /// <summary>
        /// Konkrétní typ soutěže
        /// </summary>
        public string TypSouteze { get; set; }

        /// <summary>
        /// Prázdný konstruktor
        /// </summary>
        public Soutez() { }

        /// <summary>
        /// Parametrický konstruktor pro vytvoření objektu Soutez
        /// </summary>
        /// <param name="idSoutez">ID soutěže</param>
        /// <param name="startDatum">Začátek soutěže</param>
        /// <param name="konecDatum">Konec soutěže</param>
        /// <param name="typSouteze">Konkrétní typ soutěže</param>
        public Soutez (int idSoutez, DateOnly startDatum, DateOnly konecDatum, string typSouteze)
        {
            IdSoutez = idSoutez;
            StartDatum = startDatum;
            KonecDatum = konecDatum;
            TypSouteze = typSouteze;
        }

        public override string ToString()
        {
            return $"{TypSouteze}";
        }
    }
}
