using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class InitialParser
    {
        public static string GetJavaScript(string[] NeoScript)
        {
            bool MStringEdit = false;
            bool ObjCreation = false;

            List<string> tScript = new();
            List<string> fjs = new();

            foreach (string i in NeoScript)
            {
                tScript.Add(i.Replace("\t", ""));
            }

            
            foreach (string i in tScript)
            {
                /* Basic Language Functions/Utils */

                /* Default var
                 * var MyVar = 0
                 */

                if (i.StartsWith("var "))
                {
                    fjs.Add(i + ";");
                }

                /* obj
                 * obj MyObject
                 * a: "1"
                 * b: "2"
                 * c: "3"
                 * end-object
                 */ 

                else if (i.StartsWith("obj ") || i.StartsWith("array "))
                {
                    ObjCreation = true;

                    if (i.StartsWith("obj ")) {
                        fjs.Add(i.Replace("obj", "var") + " = {");
                    }

                    if (i.StartsWith("array ")) {
                        fjs.Add(i.Replace("array", "var") + " = [");
                    }
                }

                else if (i.StartsWith("end-obj") || i.StartsWith("end-array"))
                {

                    ObjCreation = false;

                    if (i.StartsWith("end-obj")) {
                    fjs.Add("};");
                    }

                    if (i.StartsWith("end-array"))
                    {
                        fjs.Add("];");
                    }
                }

                /* for Loop
                 * for var i = 0, i < 9, i++
                 * print(i)
                 * end-for
                 * 
                 * for var i in a
                 * console(a[i])
                 * end-for
                 */

                else if (i.StartsWith("for "))
                {
                    fjs.Add("for(" + i.Replace("for ", "").Replace(",", ";") + ") {");
                }
                else if (i.StartsWith("end-for"))
                {
                    fjs.Add("}");
                }

                /* loop
                 * loop(8)
                 * print("Test")
                 * end-loop
                 */
                else if (i.StartsWith("loop("))
                {
                    string instanceName = "nsLInstance_"
                    + new Random().Next(0, 999)
                    + new Random().Next(0, 999)
                    + DateTime.Now.Second;

                    fjs.Add("for(var "
                    + instanceName
                    + " = "
                    + InRoundBrackets(i)
                    + ";"
                    + instanceName
                    + " > 0;"
                    + instanceName
                    + "--"
                    + "){");
                }
                else if (i.StartsWith("end-loop"))
                {
                    fjs.Add("}");
                }

                /* Console Action
                 * console("MyMessage", "error/warn/text (default text)")
                 */

                else if (i.StartsWith("console"))
                {
                    fjs.Add(i.Replace("console", "consPrint") + ";");
                }

                /* Multiline String
                 * mstring MyString
                 * value1
                 * value2
                 * end-mstring
                 */
                else if (i.StartsWith("mstring ") && !MStringEdit)
                {
                    fjs.Add("var " + i.Substring(8) + "= `");
                    MStringEdit = true;
                }

                else if (MStringEdit && !i.StartsWith("end-mstring"))
                {
                    fjs.Add(i + "\n");
                }

                else if (MStringEdit && i.StartsWith("end-mstring"))
                {
                    MStringEdit = false;
                    fjs.Add("`;");
                }

                /* Function
                 * function MyFunction(arg1, arg2)
                 * console(arg1 + arg2)
                 * end-function
                 */

                else if (i.StartsWith("function"))
                {
                    fjs.Add(i + "{");
                }

                else if (i.StartsWith("end-function"))
                {
                    fjs.Add("}");
                }

                else if (i != "" && ObjCreation)
                {
                    fjs.Add(i + ",");
                }

                else if (i != "")
                {
                    fjs.Add(i + ";");
                }

            }

            return String.Join("", fjs.ToArray());
        }

        static string InBrackets(string i)
        {
            return String.Join("", Regex.Matches(i, @"\<(.+?)\>")).Replace(">", "").Replace("<", "");
        }

        static string InRoundBrackets(string i)
        {
            return String.Join("", Regex.Matches(i, @"\((.+?)\)")).Replace(")", "").Replace("(", "");
        }
    }
}
