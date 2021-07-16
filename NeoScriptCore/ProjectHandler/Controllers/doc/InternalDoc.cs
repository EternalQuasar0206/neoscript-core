using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoScriptCore;

namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class InternalDoc
    {
        public static void ComList(string args)
        {
            switch (args) 
            {
                case "basics":
                Console.ForegroundColor = ConsoleColor.Yellow;
                    Program.write("var | var MyVar = 'VarContent'\n\n" +
                    "obj | obj MyObj\n" +
                    "    |  a: 1,\n" +
                    "    |  b: 2\n" +
                    "    | end-obj\n\n" +
                    "array | array MyArray\n" +
                    "      |  1,\n" +
                    "      |  2\n" +
                    "      | end-array\n\n" +
                    "for | for var i = 0, i < 9, i++\n" +
                    "    |  print(i)\n" +
                    "    | end-for\n\n" +
                    "loop  | loop (5)\n" +
                    "      |  print('Hi')\n" +
                    "      | end-loop\n\n" +
                    "console  | console('MyMessage', 'error / warn / text(default text)')\n\n" +
                    "mstring | mstring MyString\n" +
                    "        |  string line 1\n" +
                    "        |  string line 2\n" +
                    "        |  string line 3\n" +
                    "        | end-mstring\n\n" +
                    "function | function MyFunction(arg1 + arg2)\n" +
                    "         |  console(arg1 + arg2)\n" +
                    "         | end-function\n"
                    );
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            }
        }
    }
}
