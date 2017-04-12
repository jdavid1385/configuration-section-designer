using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace ConfigurationSectionDesigner.CustomCode.CodeGeneration
{
    public class VsOutputWindowPaneManager
    {

        private OutputWindowPanes _outputPanes;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputWindowName"></param>
        /// <param name="msg"></param>
        public static void OutputWindowWriteLine(string outputWindowName, string msg)
        {
            OutputWindowWrite(outputWindowName, msg + "\r\n");
        }

        public static void OutputWindowWrite(string outputWindowName, string msg)
        {
            OutputWindowPane outPane = GetOutputWindowPane(outputWindowName, true);
            outPane.OutputString(msg);
        }

        public static void OutputWindowWriteFormat(string outputWindowName, string formatString)
        {
            OutputWindowPane outPane = GetOutputWindowPane(outputWindowName, true);
            outPane.OutputString(formatString);
        }




        private static OutputWindowPane GetOutputWindowPane(string name, bool show)
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            Window win = dte.Windows.Item(EnvDTEConstants.vsWindowKindOutput);
            if (show)
                win.Visible = true;
            OutputWindow ow = win.Object as OutputWindow;
            OutputWindowPane owpane = default(OutputWindowPane);
            try
            {
                owpane = ow.OutputWindowPanes.Item(name);
            }
            catch (System.Exception e)
            {
                owpane = ow.OutputWindowPanes.Add(name);
            }
            owpane.Activate();
            return owpane;
        }


        public VsOutputWindowPaneManager()
        {


        }




    }
}

