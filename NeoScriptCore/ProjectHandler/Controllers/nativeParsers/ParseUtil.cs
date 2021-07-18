using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class ParseUtil
    {
        public static string InBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\<(.+?)\>").Groups[1].Value).Replace(">", "").Replace("<", "");
        }

        public static string InSquareBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\[(.+?)\]").Groups[1].Value).Replace("]", "").Replace("[", "");
        }

        public static string InRoundBrackets(string i)
        {
            return String.Join("", Regex.Match(i, @"\((.+?)\)").Groups[1].Value).Replace(")", "").Replace("(", "");
        }

        public static string RemoveFirst(string firstOc, string baseString)
        {
            return new Regex(Regex.Escape(firstOc)).Replace(baseString, "", 1);
        }

        public static string ReplaceFirst(string firstOc, string baseString, string toReplace)
        {
            return new Regex(Regex.Escape(firstOc)).Replace(baseString, toReplace, 1);
        }
    }
}
