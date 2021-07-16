using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class RootParser
    {
        static Regex inBrackets = new Regex(@"\((.*)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex inQuotes = new Regex("\"(.*)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static string GetJavaScript(string[] NeoScript)
        {
            //Generic Blockers
            bool IsInLoop = false;
            bool IsInFunction = false;
            bool IsInElement = false;
            bool IsInJS = false;

            //Specific Blockers
            bool ElementCreation = false;
            bool MStringCreation = false;
            /* Syntax Parsed JavaScript */
            string mid = string.Empty;

            /* Final Parsed JavaScript */
            string fjs = string.Empty;

            foreach(string i in NeoScript)
            {
                //===========================================
                // STANDARD LANGUAGE FUNCTIONS
                //===========================================
                /* Multiline String 
                 * Example:
                 * 
                 * mstring MyMulilineString
                 *  MyTextLine1
                 *  MyTextLine2
                 *  MyTextLine3
                 * end-mstring
                 */
                if (i.StartsWith("mstring ") && IsInElement == false && MStringCreation == false)
                {
                    IsInElement = true;
                    MStringCreation = true;
                    mid += "var " + i.Substring(8) + "= `";
                }
                else if (i.StartsWith("end-mstring") && MStringCreation == true)
                {
                    MStringCreation = false;
                    IsInElement = false;
                    mid += "`;".Replace("end-mstring ", "");
                }
                /* For Loop 
                 * Example:
                 * 
                 * for var i = 0, i < 9, i++
                 * print(i)
                 * end-for
                 */
                else if (i.StartsWith("for "))
                {
                    IsInLoop = true;
                    mid += "for(" + i.Replace("for ", "").Replace(",", ";") + ") {";
                }
                else if (i.StartsWith("end-for"))
                {
                    IsInLoop = false;
                    mid += "}";
                }

                /* Function
                 * Example:
                 * 
                 * function MyFunction(arg1, arg2)
                 *  print(arg1 + arg2)
                 * end-function
                 */
                else if (i.StartsWith("function") && IsInFunction == false)
                {
                    IsInFunction = true;
                    mid += i + "{";
                }
                else if (i.StartsWith("end-function"))
                {
                    IsInFunction = false;
                    mid += "}";
                }

                //===========================================
                // NEOSCRIPT+QUASARSTACK FUNCTIONS
                //===========================================
                /* elem
                 * Example:
                 * 
                 * elem<ElementType> MyElement
                 *  id: "MyId",
                 *  class: "MyClass",
                 *  content: "MyContent
                 *  
                 */
                else if (i.StartsWith("elem"))
                {
                    var elemtype = String.Join("", Regex.Matches(i, @"\<(.+?)\>")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value));

                    ElementCreation = true;
                    IsInElement = true;
                    mid += "var " + i.
                    Replace(elemtype, "").Replace("elem", "").Replace(">", "").Replace("<", "").Replace(" ", "")
                    + " = $qs.element.new('" + elemtype + "', {";
                }
                else if (i.StartsWith("end-elem"))
                {
                    ElementCreation = false;
                    IsInElement = false;
                    mid += "});";
                }
                //===========================================
                // OTHER
                //===========================================
                else if (IsInElement && MStringCreation && !i.StartsWith("mstring"))
                {
                    mid += "\n" + i;
                }

                else if (IsInElement && ElementCreation)
                {
                    mid += i + ",";
                }

                else if((IsInLoop || IsInFunction) 
                    && !i.StartsWith("end-mstring") && !i.StartsWith("mstring"))
                {
                    mid += i + ";";
                }

                else if (!IsInLoop && !IsInFunction && !IsInJS && MStringCreation
                    && !i.StartsWith("end-mstring") && !i.StartsWith("mstring"))
                {
                    mid += i + ";";
                }
            }

            fjs = mid;

            return fjs.Replace("\t", "")
            .Replace(";;", ";")
            .Replace("};", "}")
            .Replace("print", "_neoScriptSafeInject") //print({argument}) => _neoScriptSafeInject({argument}) 
            .Replace("fullRender", "_neoScriptInject"); //fullRender({ argument}) => _neoScriptPrint({argument})
        }
    }
}
