using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoScriptCore.ProjectHandler.Models
{
    public class QuasarStackModel
    {
        public string ApplicationTitle { get; set; }
        public string ApplicationIcon { get; set; }
        public List<string> StylesheetImports { get; set; }
        public List<string> ScriptImports { get; set; }
        public List<string> InternalStylesheets { get; set; }
        public List<string> InternalScripts { get; set; }
        public List<ImageImport> ImageImports { get; set; }
        public string BaseHtmlModel()
        {
            var masterScript = "<script type='text/javascript'>";
            var finalString = string.Empty;

            /* Default HTML+head header */
            finalString+="<!DOCTYPE HTML><html><head>";

            /* Applying internal data */

            // Internal Stylesheets
            if(InternalStylesheets.Any()) 
            {
                foreach(string i in InternalStylesheets) 
                {
                    finalString += "<style>" + i + "</style>";
                }
            }

            // Internal Scripts
            if(InternalScripts.Any())
            {
                foreach (string i in InternalScripts)
                {
                    finalString += "<script type='text/javascript'>" + i + "</script>";
                }
            }

            /* Applying Imports */
            if(ImageImports.Any())
            {
                foreach(ImageImport i in ImageImports)
                {
                    masterScript += " var importedImg_" + i.name + "= new Image();" +
                        "importedImg_" + i.name + ".src = '" + i.src + "'; ";
                }
            }

            /* Finishing Master Script and Head */
            masterScript += "</script>";
            finalString += masterScript + "</head>";

            /* Returning the HTML index */

            finalString += "</html>";
            return finalString;
        }
    }
}
