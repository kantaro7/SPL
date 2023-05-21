using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPL.Artifact.Application.Common
{
   public class Validations
    {
        public static bool validacion55Caracteres(string text)
        {
            bool Error = false;
   
            if (text.Length > 55)
            { 
                Error = true;
            }
            return Error;
        }
        
        
       

        public static bool validacion50Caracteres(string text)
        {
            bool Error = false;

            if (text.Length > 50)
            {
                Error = true;
            }
            return Error;
        }

        public static bool validacionCaracteresNoSeriePruebasConsutla(string text)
        {
            bool Error = false;
    
            if (!String.IsNullOrEmpty(text))
            {
                var regex = @"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$";
                var match = Regex.Match(text, regex, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
               
                    Error = true;
                }
            }
            return Error;
        }

    }
}
