using System;
using System.IO;
using NeoScriptCore.ProjectHandler.Models;
using NeoScriptCore.ProjectHandler.Controllers;
using System.Text.RegularExpressions;
using System.Linq;

namespace NeoScriptCore
{
    public class Program
    {
        public static string neo_info = "NeoScript v1.0.0";
        public static void write(string w)
        {
            if (w != "")
            {
                Console.WriteLine(w);
            }
        }
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            write(neo_info);
            write("  ");
            neo_info = "";

            string k = Console.ReadLine();

            if(k.StartsWith("create-project"))
            {
                var target = k.Replace("create-project ", "");

                write(ProjectMaker.NewProject(target));

                Main(null);
            }
            else if (k.StartsWith("neoscript-parse "))
            {
                write(InitialParser.GetJavaScript(File.ReadAllLines(k.Replace("neoscript-parse ", ""))));

                Main(null);
            }
            else if (k.StartsWith("popper"))
            {
                var s = "MyLongString<StringIWant>ASAS";

                var arrStr = String.Join(";", Regex.Matches(s, @"\<(.+?)\>")
                                    .Cast<Match>()
                                    .Select(m => m.Groups[1].Value));

                write(arrStr);
            }
            else
            {
                Error.NotSpecifiedError(k);
                Main(null);
            }
        }
    }

    public static class Error
    {
        public static void NotSpecifiedError(string k) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Program.write("Error: The command " + k + " is not specified.");
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}
