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
        public static string GetJavaScript(string[] NeoScript, bool unsafeParse = false)
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
                /*========== BASIC LANGUAGE FUNCTIONS/UTILS ==========*/

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

                    if (i.StartsWith("obj "))
                    {
                        fjs.Add(i.Replace("obj", "var") + " = {");
                    }

                    if (i.StartsWith("array "))
                    {
                        fjs.Add(i.Replace("array", "var") + " = [");
                    }
                }

                else if (i.StartsWith("end-obj") || i.StartsWith("end-array"))
                {

                    ObjCreation = false;

                    if (i.StartsWith("end-obj"))
                    {
                        fjs.Add("};");
                    }

                    if (i.StartsWith("end-array"))
                    {
                        fjs.Add("];");
                    }
                }

                /* for
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
                 * loop[8]
                 * print("Test")
                 * end-loop
                 */
                else if (i.StartsWith("loop["))
                {
                    string instanceName = "nsLInstance_"
                    + new Random().Next(0, 999)
                    + new Random().Next(0, 999)
                    + DateTime.Now.Second;

                    fjs.Add("for(var "
                    + instanceName
                    + " = "
                    + InSquareBrackets(i)
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

                /* Print Action
                 * print("MyText")
                 */

                else if (i.StartsWith("print"))
                {
                    fjs.Add(i.Replace("print", " _neoScriptSafeInject") + ";");
                }

                /* Echo Action
                 * echo("MyHTML")
                 */

                else if (i.StartsWith("echo"))
                {
                    if (unsafeParse)
                    {
                        fjs.Add(i.Replace("echo", " _neoScriptInject") + ";");
                    }
                    else
                    {
                        fjs.Add(i.Replace("print", " _neoScriptSafeInject") + ";");
                    }
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

                /* function
                 * def MyFunction(arg1, arg2)
                 * console(arg1 + arg2)
                 * end-def
                 */

                else if (i.StartsWith("def"))
                {
                    fjs.Add(ReplaceFirst("def", i, "function") + "{");
                }

                else if (i.StartsWith("end-def"))
                {
                    fjs.Add("}");
                }

                /* if
                 * if (conditions)
                 *  console("Hi")
                 * else if (conditions)
                 *  console("Ho")
                 * else (conditions)
                 *  console("Ha")
                 * end-if
                 */

                else if (i.StartsWith("if"))
                {
                    fjs.Add(i + "{");
                }

                else if (i.StartsWith("else if"))
                {
                    fjs.Add("}" + i + "{");
                }

                else if (i.StartsWith("else"))
                {
                    fjs.Add("}" + i + "{");
                }

                else if (i.StartsWith("end-if"))
                {
                    fjs.Add("}");
                }

                /* while 
                 * while (conditions)
                 * console("Yay")
                 * end-while
                 */

                else if (i.StartsWith("while"))
                {
                    fjs.Add(i + "{");
                }

                else if (i.StartsWith("end-while"))
                {
                    fjs.Add("}");
                }

                /*========== DOM OPERATION + QUASARSTACK FUNCTIONS ==========*/

                //QuasarStack Environment Configurations
                else if (i.StartsWith("Environment<"))
                {
                    if (InBrackets(i) == "EnableImports")
                    {
                        fjs.Add("$qs.system.environment.enableExternalLoad = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "EnableStates")
                    {
                        fjs.Add("$qs.system.environment.enableSaveState = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "EnableVirtualization")
                    {
                        fjs.Add("$qs.system.environment.enableVirtualization = "
                            + InRoundBrackets(i) + ";");
                    }
                }

                //QuasarStack Application Configurations
                else if (i.StartsWith("Application<"))
                {
                    if (InBrackets(i) == "Title")
                    {
                        fjs.Add("$qs.app.appTitle = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "Icon")
                    {
                        fjs.Add("$qs.app.Icon = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "Author")
                    {
                        fjs.Add("$qs.app.appAuthor = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "Version")
                    {
                        fjs.Add("$qs.app.appVersion = "
                            + InRoundBrackets(i) + ";");
                    }
                    else if (InBrackets(i) == "Description")
                    {
                        fjs.Add("$qs.app.appDescription = "
                            + InRoundBrackets(i) + ";");
                    }
                }

                //QuasarStack DOM Element Creation
                else if (i.StartsWith("Element<"))
                {
                    ObjCreation = true;
                    var elemtype = InBrackets(i);
                    fjs.Add("var " + RemoveFirst("Element", i.
                    Replace(elemtype, "").Replace("<", "").Replace(">", "").Replace(" ", "")
                    + " = $qs.element.new('" + elemtype + "', {"));
                }
                else if (i.StartsWith("end-Element"))
                {
                    ObjCreation = false;
                    fjs.Add("});");
                }

                //QuasarStack DOM Element Management
                //DOM<Action>(Element, Arguments)
                else if (i.StartsWith("DOM<"))
                {
                    if (InBrackets(i) == "Destroy") //DOM<Destroy>("Element", Timeout)
                    {
                        fjs.Add(i.Replace("DOM<Destroy>", "$qs.element.destroy") + ";");
                    }

                    if (InBrackets(i) == "Clear") //DOM<Clear>("Element")
                    {
                        fjs.Add(i.Replace("DOM<Clear>", "$qs.element.clear") + ";");
                    }

                    if (InBrackets(i) == "Hide") //DOM<Hide>("Element")
                    {
                        fjs.Add(i.Replace("DOM<Hide>", "$qs.element.hide") + ";");
                    }

                    if (InBrackets(i) == "Show") //DOM<Show>("Element", "NewDisplayType")
                    {
                        fjs.Add(i.Replace("DOM<Show>", "$qs.element.show") + ";");
                    }

                    if (InBrackets(i) == "Update") //DOM<Update>("Element", New Content)
                    {
                        fjs.Add(i.Replace("DOM<Update>", "$qs.element.update") + ";");
                    }

                    if (InBrackets(i) == "Inject") //DOM<Inject>("Element", New Content)
                    {
                        fjs.Add(i.Replace("DOM<Inject>", "$qs.element.inject") + ";");
                    }

                    if (InBrackets(i) == "SetParent") //DOM<SetParent>("Target", "Origin")
                    {
                        fjs.Add(i.Replace("DOM<SetParent>", "$qs.element.setParent") + ";");
                    }

                    if (InBrackets(i) == "SetParent") //DOM<SetParent>("Target", "Origin")
                    {
                        fjs.Add(i.Replace("DOM<SetParent>", "$qs.element.setParent") + ";");
                    }

                    if (InBrackets(i) == "SetStyle") //DOM<SetStyle>("Target", style)
                    {
                        fjs.Add(i.Replace("DOM<SetStyle>", "$qs.element.setStyle") + ";");
                    }

                    if (InBrackets(i) == "AddStyle") //DOM<AddStyle>("Target", style)
                    {
                        fjs.Add(i.Replace("DOM<AddStyle>", "$qs.element.addStyle") + ";");
                    }

                    if (InBrackets(i) == "ForeColor") //DOM<ForeColor>("Target", color)
                    {
                        fjs.Add(i.Replace("DOM<ForeColor>", "$qs.element.foreColor") + ";");
                    }

                    if (InBrackets(i) == "BackColor") //DOM<BackColor>("Target", color)
                    {
                        fjs.Add(i.Replace("DOM<BackColor>", "$qs.element.backColor") + ";");
                    }

                    if (InBrackets(i) == "Resize") //DOM<Resize>("Target", New size[???x???])
                    {
                        fjs.Add(i.Replace("DOM<Resize>", "$qs.element.resize") + ";");
                    }

                    if (InBrackets(i) == "Scale") //DOM<Resize>("Target", New scale)
                    {
                        fjs.Add(i.Replace("DOM<Resize>", "$qs.element.scale") + ";");
                    }

                    if (InBrackets(i) == "Rotate") //DOM<Rotate>("Target", Deg)
                    {
                        fjs.Add(i.Replace("DOM<Rotate>", "$qs.element.rotate") + ";");
                    }

                    if (InBrackets(i) == "Rotate3d") //DOM<Rotate>("Target", x, y, z, angle)
                    {
                        fjs.Add(i.Replace("DOM<Rotate3d>", "$qs.element.rotate3d") + ";");
                    }

                    if (InBrackets(i) == "AnimationTime") //DOM<AnimationTime>("Target", time)
                    {
                        fjs.Add(i.Replace("DOM<AnimationTime>", "$qs.element.animationTime") + ";");
                    }
                }

                else if (i.StartsWith("DOMAll<"))
                {
                    if (InBrackets(i) == "Destroy") //DOMAll<Destroy>("elementAll", Timeout)
                    {
                        fjs.Add(i.Replace("DOMAll<Destroy>", "$qs.elementAll.destroy") + ";");
                    }

                    if (InBrackets(i) == "Clear") //DOMAll<Clear>("elementAll")
                    {
                        fjs.Add(i.Replace("DOMAll<Clear>", "$qs.elementAll.clear") + ";");
                    }

                    if (InBrackets(i) == "Hide") //DOMAll<Hide>("elementAll")
                    {
                        fjs.Add(i.Replace("DOMAll<Hide>", "$qs.elementAll.hide") + ";");
                    }

                    if (InBrackets(i) == "Show") //DOMAll<Show>("elementAll", "NewDisplayType")
                    {
                        fjs.Add(i.Replace("DOMAll<Show>", "$qs.elementAll.show") + ";");
                    }

                    if (InBrackets(i) == "Update") //DOMAll<Update>("elementAll", New Content)
                    {
                        fjs.Add(i.Replace("DOMAll<Update>", "$qs.elementAll.update") + ";");
                    }

                    if (InBrackets(i) == "Inject") //DOMAll<Inject>("elementAll", New Content)
                    {
                        fjs.Add(i.Replace("DOMAll<Inject>", "$qs.elementAll.inject") + ";");
                    }

                    if (InBrackets(i) == "SetParent") //DOMAll<SetParent>("Target", "Origin")
                    {
                        fjs.Add(i.Replace("DOMAll<SetParent>", "$qs.elementAll.setParent") + ";");
                    }

                    if (InBrackets(i) == "SetParent") //DOMAll<SetParent>("Target", "Origin")
                    {
                        fjs.Add(i.Replace("DOMAll<SetParent>", "$qs.elementAll.setParent") + ";");
                    }

                    if (InBrackets(i) == "SetStyle") //DOMAll<SetStyle>("Target", style)
                    {
                        fjs.Add(i.Replace("DOMAll<SetStyle>", "$qs.elementAll.setStyle") + ";");
                    }

                    if (InBrackets(i) == "AddStyle") //DOMAll<AddStyle>("Target", style)
                    {
                        fjs.Add(i.Replace("DOMAll<AddStyle>", "$qs.elementAll.addStyle") + ";");
                    }

                    if (InBrackets(i) == "ForeColor") //DOMAll<ForeColor>("Target", color)
                    {
                        fjs.Add(i.Replace("DOMAll<ForeColor>", "$qs.elementAll.foreColor") + ";");
                    }

                    if (InBrackets(i) == "BackColor") //DOMAll<BackColor>("Target", color)
                    {
                        fjs.Add(i.Replace("DOMAll<BackColor>", "$qs.elementAll.backColor") + ";");
                    }

                    if (InBrackets(i) == "Resize") //DOMAll<Resize>("Target", New size[???x???])
                    {
                        fjs.Add(i.Replace("DOMAll<Resize>", "$qs.elementAll.resize") + ";");
                    }

                    if (InBrackets(i) == "Scale") //DOMAll<Resize>("Target", New scale)
                    {
                        fjs.Add(i.Replace("DOMAll<Resize>", "$qs.elementAll.scale") + ";");
                    }

                    if (InBrackets(i) == "Rotate") //DOMAll<Rotate>("Target", Deg)
                    {
                        fjs.Add(i.Replace("DOMAll<Rotate>", "$qs.elementAll.rotate") + ";");
                    }

                    if (InBrackets(i) == "Rotate3d") //DOMAll<Rotate>("Target", x, y, z, angle)
                    {
                        fjs.Add(i.Replace("DOMAll<Rotate3d>", "$qs.elementAll.rotate3d") + ";");
                    }

                    if (InBrackets(i) == "AnimationTime") //DOMAll<AnimationTime>("Target", time)
                    {
                        fjs.Add(i.Replace("DOMAll<AnimationTime>", "$qs.elementAll.animationTime") + ";");
                    }
                }

                //Environment Methods
                else if (i.StartsWith("Environment<"))
                {
                    if (InBrackets(i) == "ViewportTrigger") //Environment<ViewportTrigger>(timeout, fullReset = false)
                    {
                        fjs.Add(i.Replace("Environment<ViewportTrigger>", "$qs.action.viewportTrigger") + ";");
                    }

                    if (InBrackets(i) == "Reset") //Environment<Reset>(fullReset = false)
                    {
                        fjs.Add(i.Replace("Environment<Reset>", "$qs.action.reset") + ";");
                    }

                    if (InBrackets(i) == "DisableZoom") //Environment<DisableZoom>(disableZoom = false)
                    {
                        fjs.Add(i.Replace("Environment<DisableZoom>", "$qs.action.disableZoom") + ";");
                    }
                }

                /* Update[interval]
                 * code
                 * end-Update
                 */
                
                else if (i.StartsWith("Update")) 
                {
                    fjs.Add(i.Replace("Update[" + InSquareBrackets(i) + "]", "$qs.action.update(" + InSquareBrackets(i) + ", `"));
                }
                else if (i.StartsWith("end-Update"))
                {
                    fjs.Add(i.Replace("end-Update", "`);"));
                }
                /*========== OTHERS ==========*/

                else if (i != "" && ObjCreation)
                {
                    fjs.Add(i + ",");
                }

                else if (i != "")
                {
                    fjs.Add(i + ";");
                }
            }

            // Final Parser Actions + Commands
            return String.Join("", fjs.ToArray())
            //DOM CORRECTIONS
            .Replace("DOM<Find>", "$qs.element.find") //DOM<Find>("target")
            .Replace("DOM<Pick>", "$qs.element.pick") //DOM<Pick>("target")
            .Replace("DOMAll<Find>", "$qs.elementAll.find") //DOMAll<Find>("target")
            .Replace("DOMAll<Pick>", "$qs.elementAll.pick") //DOMAll<Pick>("target")
            //Environment CORRECTIONS
            .Replace("Environment<Info>", "$qs.action.browserInfo()")
            .Replace("Environment<AppInfo>", "$qs.action.appInfo()");
        }

        static string InBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\<(.+?)\>").Groups[1].Value).Replace(">", "").Replace("<", "");
        }

        static string InSquareBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\[(.+?)\]").Groups[1].Value).Replace("]", "").Replace("[", "");
        }

        static string InRoundBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\((.+?)\)").Groups[1].Value).Replace(")", "").Replace("(", "");
        }

        static string RemoveFirst(string firstOc, string baseString)
        {
            return new Regex(Regex.Escape(firstOc)).Replace(baseString, "", 1);
        }

        static string ReplaceFirst(string firstOc, string baseString, string toReplace)
        {
            return new Regex(Regex.Escape(firstOc)).Replace(baseString, toReplace, 1);
        }
    }
}
