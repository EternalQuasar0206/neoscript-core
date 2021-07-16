using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NeoScriptCore.ProjectHandler.Controllers
{
    public static class ProjectMaker
    {
        public static string QuasarStackJS_Version = "1.0.0";
        public static string JQuery_Version = "3.6.0";
        public static string NewProject(string targetDirectory)
        {
            string response = string.Empty;

            if (Directory.Exists(targetDirectory))
            {
                try
                {
                    /* NeoScript Header File */
                    File.WriteAllText(targetDirectory + "/app.nsh", "//NeoScript Header File - Use it to " +
                        "make the project map\n" +
                        "[controllers]=./Controllers/\n" +
                        "[views]=./Views/\n" +
                        "[models]=./Models/\n" +
                        "[output]=./Output/\n" +
                        "[index]=./Views/index.nsv\n");

                    /* NeoScript Controllers Folder */
                    Directory.CreateDirectory(targetDirectory + "/Controllers/");

                    /* NeoScript Views Folder */
                    Directory.CreateDirectory(targetDirectory + "/Views/");
                    File.WriteAllText(targetDirectory + "/Views/index.ns", "print(\"Hello World!\")");

                    /* NeoScript Models Folder */
                    Directory.CreateDirectory(targetDirectory + "/Models/");

                    response = "Success: The project was created";
                }
                catch(Exception e)
                {
                    response = "Error: " + e;
                }
            }
            else
            {
                response = "Error: Target directory doesn't exist";
            }

            return response;
        }
    }
}
