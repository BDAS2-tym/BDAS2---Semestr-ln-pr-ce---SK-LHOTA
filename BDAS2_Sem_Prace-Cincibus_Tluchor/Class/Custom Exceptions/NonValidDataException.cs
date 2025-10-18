using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Custom_Exceptions
{
    class NonValidDataException : Exception
    {
        public NonValidDataException()
        { }

        public NonValidDataException(string message)
            : base(message)
        { }

        public NonValidDataException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
