namespace SPL.Reports.Application.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Validations
    {
        public static bool validacion55Caracteres(string text) => text.Length > 55;

        public static bool validacionCaracteresNoSeriePruebasConsutla(string text) => !string.IsNullOrEmpty(text)
&& !Regex.Match(text, @"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$", RegexOptions.IgnoreCase).Success;
    }
}
