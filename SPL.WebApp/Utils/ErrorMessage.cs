using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.WebApp.Utils
{
    public static class ErrorMessage
    {
     
        public static string invalidNumber
        {
            get { return _invalidNumber; }
            set { _invalidNumber = value; }
        }

        static string _invalidNumber = "Solo se permiten números";
    }
}
