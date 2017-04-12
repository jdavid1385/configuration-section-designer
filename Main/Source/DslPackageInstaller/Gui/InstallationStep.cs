using System;
using System.Windows.Forms;
using SharpSetup.Base;
using SharpSetup.UI.Controls;
using SharpSetup.UI.Forms.Modern;
using Gui.Properties;
using System.IO;

namespace Gui
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class InstallationStep : ModernActionStep
    {
        InstallationMode mode;
        public InstallationStep(InstallationMode mode)
        {
            InitializeComponent();
            this.mode = mode;
        }

        private void InstallationStep_Entered(object sender, EventArgs e)
        {
            ipProgress.StartListening();
            try
            {
                if (mode == InstallationMode.Uninstall)
                {
                    MsiConnection.Instance.Uninstall();
                    /*
                    try
                    {
                        MsiConnection.Instance.Open(new Guid("{c9c6369c-6339-4e28-b721-1acc6cf05d0a}"));
                        MsiConnection.Instance.Uninstall();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Uninstall");
                    }
                    */
                    if (File.Exists(Resources.MainMsiFile))
                        MsiConnection.Instance.Open(Resources.MainMsiFile, true);
                }
                else if (mode == InstallationMode.Install)
                {
                    //// Lines below used when loading CSD MSI as sub-MSI.
                    //MsiConnection.Instance.EnableLogging = true;
                    //MsiConnection.Instance.LogFile = "c:\\test.log";
                    //MsiConnection.Instance.Features.Add
                    //MsiConnection.Instance.SaveAs("MainInstall");
                    //MsiConnection.Instance.Open("ConfigurationSectionDesigner.msi", false);
                    //MsiConnection.Instance.Install("");
                    //MsiConnection.Instance.OpenSaved("MainInstall");
                    
                   MsiConnection.Instance.Install();
                }
                else
                    MessageBox.Show("Unknown mode");
            }
            catch (MsiException mex)
            {
                if (mex.ErrorCode != (uint)InstallError.UserExit)
                    MessageBox.Show("Installation failed: " + mex.Message);
                Wizard.Finish();
            }
            ipProgress.StopListening();
            Wizard.NextStep();
        }

        public override bool CanClose()
        {
            return false;
        }
    }
}
