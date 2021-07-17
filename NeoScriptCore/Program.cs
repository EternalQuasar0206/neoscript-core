using System;
using System.IO;
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
            else if (k.StartsWith("parse "))
            {
                write(InitialParser.GetJavaScript(File.ReadAllLines(k.Replace("parse ", "")), true));

                Main(null);
            }
            else if (k.StartsWith("safe-parse "))
            {
                write(InitialParser.GetJavaScript(File.ReadAllLines(k.Replace("parse ", "")), false));

                Main(null);
            }
            else if (k.StartsWith("com-list "))
            {
                InternalDoc.ComList(k.Replace("com-list ", ""));
                Main(null);
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
