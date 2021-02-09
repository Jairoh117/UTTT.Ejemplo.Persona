using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UTTT.Ejemplo.Persona.Control.Ctrl
{
    public class validaHtml
    {
        private readonly Regex htmlInjectionRegex = new Regex(@"<.*?>|&.*?;");
        public bool htmlInjectionValida(String _informacion, ref String _mensaje, String _etiquetaReferente, ref System.Web.UI.WebControls.TextBox _control)
        {
            bool isMatch = this.htmlInjectionRegex.IsMatch(_informacion);
            if (isMatch)
            {
                _mensaje = "Bloqueo de caracteres en el campo " + _etiquetaReferente.Replace(":", "");
                _control.Focus();
            }
            return isMatch;
        }
    }
}
