using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Debugging;
using Inherit;
using Sample;

namespace Samples
{
    public partial class MainTestForm : Form
    {
        public MainTestForm()
        {
            InitializeComponent();
        }

        private void MainTestForm_Load(object sender, EventArgs e)
        {

        }


        #region "Dynamicaly generated demos"


        private void RunSampleConfigDemo(string configFilePath)
        {
            bool isTempConfigFile = string.IsNullOrEmpty(configFilePath);

            WriteStartResult("SAMPLE DEMO");
            try
            {
                string newConfigFilePath = "";
                // Create temp config file if no config specified.
                if (isTempConfigFile)
                {
                    CreateSampleConfigFile(out newConfigFilePath);
                    WriteResult("Created new config file! Path={0}\r\n", newConfigFilePath);
                    configFilePath = newConfigFilePath;
                }

                Configuration config = LoadConfigFile(configFilePath, isTempConfigFile);

                WriteResult("Loaded config file. Results:\r\n");

                ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("SampleConfigurationGroup");

                WriteResult("\tSectionGroups[0]={0}.\r\n", sectionGroup.Name);

                //SampleConfigurationSection section = config.Sections["sample"] as SampleConfigurationSection;
                SampleConfigurationSection section = sectionGroup.Sections["sampleConfigurationSection"] as SampleConfigurationSection;

                WriteResult("\tsection.Samples: {0}\r\n", section.Samples);
                WriteResult("\tsection.Bars[0].Crackle: {0}\r\n", section.Bars[0].Crackle);

                WriteResult("\tFoo.Baz.Height: {0}\r\n", section.Foo.Baz.Height);
                WriteResult("\tFoo.Baz.Width: {0}\r\n", section.Foo.Baz.Width);
                WriteResult("\tFoo.Baz.Depth: {0}\r\n", section.Foo.Baz.Depth);

                // Delete config file if temp.
                if (isTempConfigFile)
                {
                    DeleteConfigFile(configFilePath);
                }
            }
            catch (Exception ex)
            {
                WriteResult("\r\nException Occured! {0}\r\n", ex);
            }
            finally
            {
                WriteEndResult();
            }
            
        }


        private void RunSchoolRegistryConfigDemo(string configFilePath)
        {
            bool isTempConfigFile = string.IsNullOrEmpty(configFilePath);

            WriteStartResult("SCHOOLS DEMO");

            try
            {
                string newConfigFilePath = "";
                // Create temp config file if no config specified.
                if (isTempConfigFile)
                {
                    CreateSchoolRegistryConfigFile(out newConfigFilePath);
                    WriteResult("Created new config file! Path={0}\r\n", newConfigFilePath);
                    configFilePath = newConfigFilePath;
                }

                Configuration config = LoadConfigFile(configFilePath, isTempConfigFile);

                WriteResult("Loaded config file. Results:\r\n");

                SchoolRegistrySection section = config.Sections["school"] as SchoolRegistrySection;

                WriteResult("\tSchool name: {0}\r\n", section.SchoolName);
                WriteResult("\tNumber of professors: {0}\r\n", section.Professors.Count);
                WriteResult("\tNumber of students: {0}\r\n", section.Students.Count);
                WriteResult("\tName of student '0': {0}\r\n", section.Students[0].Name);

                // Delete config file if temp.
                if (isTempConfigFile)
                {
                    DeleteConfigFile(configFilePath);
                }
            }
            catch (Exception ex)
            {
                WriteResult("\r\nException Occured! {0}\r\n", ex);
            }
            finally
            {
                WriteEndResult();
            }
        }

        private void CreateSchoolRegistryConfigFile(out string newConfigFilePath)
        {
            newConfigFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".config");
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = newConfigFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            // Create configuration
            SchoolRegistrySection section = new SchoolRegistrySection();
            config.Sections.Add("school", section);

            Professor flimflop = new Professor { Name = "Dr. Flimflop", YearOfBirth = 1968 };
            section.Professors.Add(flimflop);
            Professor mania = new Professor { Name = "Dr. Maniä", YearOfBirth = 1972 };
            section.Professors.Add(mania);

            Student johnson = new Student { Name = "Johnson", YearOfBirth = 1989 };
            section.Students.Add(johnson);
            mania.Students.Add(johnson);

            config.Save();

        }

        private void CreateSampleConfigFile(out string newConfigFilePath)
        {
            newConfigFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".config");
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = newConfigFilePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            // Create configuration
            ConfigurationSectionGroup sectionGroup = new ConfigurationSectionGroup();
            //"SampleConfigurationGroup"

            SampleConfigurationSection section = new SampleConfigurationSection();

            config.SectionGroups.Add("SampleConfigurationGroup", sectionGroup);

            sectionGroup.Sections.Add("sampleConfigurationSection", section);
            section.Foo.Baz = new CustomType { Height = 42, Width = 314, Depth = 313 };

            section.Bars.Add(new Bar { Crackle = 3.14F, Snap = true });
            section.Bars.Add(new Bar { Crackle = 2.71828F, Snap = false });
 
            config.Save();

        }
        #endregion


        #region "Helper methods"

        private void DeleteConfigFile(string tempConfigFile)
        {
            if (File.Exists(tempConfigFile))
                File.Delete(tempConfigFile);
            WriteResult("Deleted temp config file. Path={0}\r\n", tempConfigFile);
        }

        private void WriteStartResult(string resultName)
        {
            ConsoleTextBox.AppendText(string.Format("\r\n===== {0} =====\r\n", resultName));
        }

        private void WriteResult(string format, params object[] vals)
        {
            ConsoleTextBox.AppendText(string.Format(format, vals));
        }

        private void WriteEndResult()
        {
            ConsoleTextBox.AppendText("*** Demo Complete! **\r\n\r\n");
        }

        private Configuration LoadConfigFile(string configFilePath, bool isTempConfigFile)
        {
            //if (System.IO.Path.IsPathRooted(configFilePath))

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };

            if (!isTempConfigFile)
            {
                fileMap.ExeConfigFilename = Environment.CurrentDirectory + @"\SampleConfigFiles\" + configFilePath;
            }

            if (!File.Exists(fileMap.ExeConfigFilename))
            {
                throw new Exception("Configuration file could not be found :" + fileMap.ExeConfigFilename);
                //send("Configuration file required :" + fileMap.ExeConfigFilename, LogLevel.Error);
                //return;
            }
            else
            {
                WriteResult("Config file '{0}' found!", fileMap.ExeConfigFilename);
            }

            // Load configuration
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }

        #endregion



        



        private void ClearConsoleButton_Click(object sender, EventArgs e)
        {
            ConsoleTextBox.Clear();
        }


        private void RunDemoButton_Click(object sender, EventArgs e)
        {

            try
            {

                switch ((string)DemoSelectComboBox.SelectedItem)
                {
                    case "Sample Demo":
                        RunSampleConfigDemo("");
                        break;
                    case "School Demo":
                        RunSchoolRegistryConfigDemo("");
                        break;
                    case "Basic Demo - Config File":
                        throw new Exception("Not yet implemented.");
                        break;
                    case "Sample Demo - Config File":
                        RunSampleConfigDemo("SampleConfigDemo01.config");
                       // RunSampleConfigDemo_File();
                        break;
                    case "School Demo - Config File":
                        throw new Exception("Not yet implemented.");
                        break;
                    default:
                        throw new Exception("Invalid demo selection.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleTextBox.AppendText("\r\nException Occured: " + ex + "\r\n");
            }
        }
    }
}
