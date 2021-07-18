using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class ExpressionParser
    {
        public static string GetJavaScript(string NeoScript)
        {
            return NeoScript
            //DOM CORRECTIONS
            .Replace("DOM<Find>", "$qs.element.find") //DOM<Find>("target")
            .Replace("DOM<Pick>", "$qs.element.pick") //DOM<Pick>("target")
            .Replace("DOMAll<Find>", "$qs.elementAll.find") //DOMAll<Find>("target")
            .Replace("DOMAll<Pick>", "$qs.elementAll.pick") //DOMAll<Pick>("target")
            //Environment CORRECTIONS
            .Replace("Environment<Info>", "$qs.action.browserInfo()")
            .Replace("Environment<AppInfo>", "$qs.action.appInfo()")
            //Misc
            .Replace("Date<Now>", "$qs.date.now()")
            .Replace("Data<IsElement>", "$qs.data.check.element")
            .Replace("Data<IsEmail>", "$qs.data.check.email")
            .Replace("Data<IsPhone>", "$qs.data.check.phone")
            .Replace("Data<NumFilter>", "$qs.data.num.filter")
            .Replace("Data<NumRemove>", "$qs.data.num.remove")
            .Replace("Data<NumList>", "$qs.data.num.list");
        }
    }
}
