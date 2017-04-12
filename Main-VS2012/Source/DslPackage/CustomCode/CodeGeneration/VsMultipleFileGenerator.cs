using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationSectionDesigner.CustomCode.CodeGeneration;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.CodeDom.Compiler;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;
using Microsoft.VisualStudio.Designer.Interfaces;

/*
 * Based on the code found here:
 * http://www.codeproject.com/KB/cs/VsMultipleFileGenerator.aspx
 */

namespace ConfigurationSectionDesigner
{
    [CLSCompliant(false)]
    public abstract class VsMultipleFileGenerator<TIterativeElement> : IEnumerable<TIterativeElement>, Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator, IObjectWithSite
    {
        #region Visual Studio Specific Fields
        private object _site;
        private ServiceProvider _serviceProvider = null;
        internal const int S_OK = 0; // VSConstants.S_OK
        internal const int E_FAIL = unchecked((int)0x80004005);

        internal const string vsOutputWindowPaneName = "ConfigurationSectionDesigner OUTPUT";

        #endregion


        #region Our Fields

        private string _bstrInputFileContents;
        private string _wszInputFilePath;
        private EnvDTE.Project _project;
        private CodeDomProvider _codeDomProvider;

        private List<string> _newFileNames;

        #endregion

        [CLSCompliant( false )]
        protected EnvDTE.Project Project
        {
            get
            {
                return _project;
            }
        }

        /// <summary>
        /// Returns a CodeDomProvider object for the language of the project containing
        /// the project item the generator was called on
        /// </summary>
        /// <returns>A CodeDomProvider object</returns>
        protected CodeDomProvider CodeProvider
        {
            get
            {
                if( _codeDomProvider == null )
                {
                    //Query for IVSMDCodeDomProvider/SVSMDCodeDomProvider for this project type
                    IVSMDCodeDomProvider provider = SiteServiceProvider.GetService( typeof( SVSMDCodeDomProvider ) ) as IVSMDCodeDomProvider;
                    if( provider != null )
                    {
                        _codeDomProvider = provider.CodeDomProvider as CodeDomProvider;
                    }
                    else
                    {
                        //In the case where no language specific CodeDom is available, fall back to C#
                        _codeDomProvider = CodeDomProvider.CreateProvider( "C#" );
                    }
                }
                return _codeDomProvider;
            }
        }

        /// <summary>
        ///  contains the name of the default namespace for the current solution or folder; it’s a useful piece of information to have for generating code.
        /// </summary>
        protected string RootNamespace
        {
            get
            {
                if( _project == null ) return null;
                //Diagnostics.DebugWrite("_project.Properties.Count={0}", _project.Properties.Count);
                //Diagnostics.DebugWrite("_project.Name={0}", _project.Name);
                EnvDTE.Property property = _project.Properties.Item( "RootNamespace" );
                if( property == null ) return null;

                return property.Value.ToString();
            }
        }

        /// <summary>
        /// contains the contents of the input file as a single string; this might seem redundant since we already know the path, but what can I say – it's convenient, saves one line of code.
        /// </summary>
        protected string InputFileContents
        {
            get
            {
                return _bstrInputFileContents;
            }
        }

        /// <summary>
        /// contains the path of the file for which content is being generated.
        /// </summary>
        protected string InputFilePath
        {
            get
            {
                return _wszInputFilePath;
            }
        }

        protected object Site
        {
            get { return _site; }
        }

        [CLSCompliant(false)]
        protected ServiceProvider SiteServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleServiceProvider = _site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    _serviceProvider = new ServiceProvider(oleServiceProvider);
                }
                return _serviceProvider;
            }
        }

        private Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress _generatorProgress;
        /// <summary>
        /// Interface to the VS shell object we use to tell our progress while we are generating.
        ///  is an interface that we can use to tell Visual Studio how long the operation will take. This is only useful if your 
        ///  custom tool does something that takes a long time.
        /// </summary>
        protected Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress GeneratorProgress
        {
            get
            {
                return _generatorProgress;
            }
        }

        public VsMultipleFileGenerator()
        {
            _newFileNames = new List<string>();
        }
        public abstract IEnumerator<TIterativeElement> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract string GetFileName( TIterativeElement element );
        public abstract byte[] GenerateContent( TIterativeElement element );
        

        public abstract byte[] GenerateDefaultContent();

        
        #region IObjectWithSite Members

        public void GetSite( ref Guid riid, out IntPtr ppvSite )
        {
            if( this._site == null )
            {
                throw new Win32Exception( -2147467259 );
            }

            IntPtr objectPointer = Marshal.GetIUnknownForObject( this._site );

            try
            {
                Marshal.QueryInterface( objectPointer, ref riid, out ppvSite );
                if( ppvSite == IntPtr.Zero )
                {
                    throw new Win32Exception( -2147467262 );
                }
            }
            finally
            {
                if( objectPointer != IntPtr.Zero )
                {
                    Marshal.Release( objectPointer );
                    objectPointer = IntPtr.Zero;
                }
            }
        }

        public void SetSite( object pUnkSite )
        {
            this._site = pUnkSite;
        }

        #endregion

        /// <summary>
        /// Implements the IVsSingleFileGenerator.DefaultExtension method. 
        /// Returns the extension of the generated file
        /// </summary>
        /// <param name="pbstrDefaultExtension">Out parameter, will hold the extension that is to be given to the output file name. The returned extension must include a leading period</param>
        /// <returns>S_OK if successful, E_FAIL if not</returns>
        public abstract  int DefaultExtension(out string pbstrDefaultExtension);

        /// <summary>
        /// Implements the IVsSingleFileGenerator.Generate method.
        /// Executes the transformation and returns the newly generated output file, whenever a custom tool is loaded, or the input file is saved
        /// </summary>
        /// <param name="wszInputFilePath">The full path of the input file. May be a null reference (Nothing in Visual Basic) in future releases of Visual Studio, so generators should not rely on this value</param>
        /// <param name="bstrInputFileContents">The contents of the input file. This is either a UNICODE BSTR (if the input file is text) or a binary BSTR (if the input file is binary). If the input file is a text file, the project system automatically converts the BSTR to UNICODE</param>
        /// <param name="wszDefaultNamespace">This parameter is meaningful only for custom tools that generate code. It represents the namespace into which the generated code will be placed. If the parameter is not a null reference (Nothing in Visual Basic) and not empty, the custom tool can use the following syntax to enclose the generated code</param>
        /// <param name="rgbOutputFileContents">[out] Returns an array of bytes to be written to the generated file. You must include UNICODE or UTF-8 signature bytes in the returned byte array, as this is a raw stream. The memory for rgbOutputFileContents must be allocated using the .NET Framework call, System.Runtime.InteropServices.AllocCoTaskMem, or the equivalent Win32 system call, CoTaskMemAlloc. The project system is responsible for freeing this memory</param>
        /// <param name="pcbOutput">[out] Returns the count of bytes in the rgbOutputFileContent array</param>
        /// <param name="pGenerateProgress">A reference to the IVsGeneratorProgress interface through which the generator can report its progress to the project system</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns E_FAIL</returns>
        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput,
            Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress pGenerateProgress)
        {

            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "");
            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, string.Format("------ CSD file generation started: Configuration document: {0} ------", InputFilePath));

            Diagnostics.DebugWrite("VsMultipleFileGenerator => VSMFG.");

            ProjectItem configurationSectionModelFile = null;

            _bstrInputFileContents = bstrInputFileContents;
            _wszInputFilePath = wszInputFilePath;
            _generatorProgress = pGenerateProgress;
            _newFileNames.Clear();

            // Look through all the projects in the solution
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            Diagnostics.DebugWrite("VSMFG.Generate >> dte.Solution.Projects.Count = {0}.", dte.Solution.Projects.Count);

            VsOutputWindowPaneManager.OutputWindowWrite(vsOutputWindowPaneName, "* Searching for configuration project handle... ");
            // [7296] FILE = ~\ConfigurationSectionDesigner\Debugging\Sample.csd.
            if (!VsHelper.TryFindProjectItemAndParentProject(InputFilePath, out _project, out configurationSectionModelFile) || _project == null)
            {
                VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "error: Unable to retrieve Visual Studio ProjectItem. File generation halted.");
                throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem. Try running the tool again.");
            }
            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "found!");

            // Check for source control integration
            if (VsHelper.IsItemUnderSourceControl(dte, configurationSectionModelFile))
            {
                VsHelper.CheckoutItem(dte, configurationSectionModelFile);
            }

            // now we can start our work, iterate across all the 'elements' in our source file 
            foreach (TIterativeElement element in this)
            {
                try
                {
                    // obtain a name for this target file
                    string fileName = GetFileName(element);

                    Diagnostics.DebugWrite("VSMFG.Generate >> Filename for current element in loop is '{0}'", fileName);

                    // add it to the tracking cache
                    _newFileNames.Add(fileName);
                    // fully qualify the file on the filesystem
                    string strFile = Path.Combine(wszInputFilePath.Substring(0, wszInputFilePath.LastIndexOf(Path.DirectorySeparatorChar)), fileName);

                    FileStream fs = null;
                    try
                    {
                        // generate our target file content
                        byte[] data = GenerateContent(element);

                        // if data is null, it means to ignore the contents of the generated file
                        if (data == null) continue;

                        if (File.Exists(strFile))
                        {
                            // If the file already exists, only save the data if the generated file is different than the existing file.
                            byte[] oldData = File.ReadAllBytes(strFile);
                            if (!Util.IsDataEqual(oldData, data))
                            {
                                Diagnostics.DebugWrite("VSMFG.Generate >> File data has not changed for file '{0}'. Re-using existing file...", strFile);
                                fs = File.Open(strFile, FileMode.Truncate);
                            }
                        }
                        else
                        {
                            // create the file
                            fs = File.Create(strFile);
                        }

                        if (fs != null)
                        {
                            // write it out to the stream
                            fs.Write(data, 0, data.Length);
                        }

                        // add the newly generated file to the solution, as a child of the source file
                        if (!configurationSectionModelFile.ProjectItems.Cast<ProjectItem>().Any(pi => pi.Name == fileName))
                        {
                            ProjectItem itm = configurationSectionModelFile.ProjectItems.AddFromFile(strFile);
                            /*
                             * Here you may wish to perform some addition logic such as, setting a custom tool for the target file if it
                             * is intented to perform its own generation process.
                             * Or, set the target file as an 'Embedded Resource' so that it is embedded into the final Assembly.
                         
                            EnvDTE.Property prop = itm.Properties.Item("CustomTool");
                            //// set to embedded resource
                            itm.Properties.Item("BuildAction").Value = 3;
                            if (String.IsNullOrEmpty((string)prop.Value) || !String.Equals((string)prop.Value, typeof(AnotherCustomTool).Name))
                            {
                                prop.Value = typeof(AnotherCustomTool).Name;
                            }
                            */
                        }
                    }
                    catch (Exception e)
                    {
                        //GeneratorProgress.GeneratorError( false, 0, string.Format( "{0}\n{1}", e.Message, e.StackTrace ), -1, -1 );
                        GeneratorProgress.GeneratorError(0, 0, string.Format("{0}\n{1}", e.Message, e.StackTrace), 0, 0);
                        if (File.Exists(strFile))
                        {
                            File.WriteAllText(strFile, "An exception occured while running the CsdFileGenerator on this file. See the Error List for details. E=" + e);
                        }
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    // This is here for debugging purposes, as setting a breakpoint here can be very helpful
                    Diagnostics.DebugWrite("VSMFG.Generate >> EXCEPTION: {0}", ex);
                    throw;
                }
            }

            // perform some clean-up, making sure we delete any old (stale) target-files
            VsOutputWindowPaneManager.OutputWindowWrite(vsOutputWindowPaneName, "* Cleaning up existing files... ");

            foreach (ProjectItem childItem in configurationSectionModelFile.ProjectItems)
            {
                string next;
                DefaultExtension(out next);

                if (!(childItem.Name.EndsWith(next) || _newFileNames.Contains(childItem.Name)))
                    // then delete it
                    childItem.Delete();
            }

            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "complete!");

            VsOutputWindowPaneManager.OutputWindowWrite(vsOutputWindowPaneName, "* Writing new files... ");
            // generate our default content for our 'single' file
            byte[] defaultData = null;
            try
            {
                defaultData = GenerateDefaultContent();
            }
            catch (Exception ex)
            {
                Diagnostics.DebugWrite("VSMFG.Generate >> EXCEPTION: {0}", ex);
                //GeneratorProgress.GeneratorError( false, 0, string.Format( "{0}\n{1}", ex.Message, ex.StackTrace ), -1, -1 );
                GeneratorProgress.GeneratorError(0, 0, string.Format("{0}\n{1}", ex.Message, ex.StackTrace), 0, 0);
            }
            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "complete!");

            if (defaultData == null)
                defaultData = new byte[0];

            // return our default data, so that Visual Studio may write it to disk.

            /*
             * You need to write the bytes of the generated file into this variable. However, you cannot 
             * do it directly (hence the IntPtr[] type) – instead, you must use the System.Runtime.InteropServices.AllocCoTaskMem 
             * allocator to create the memory and write type bytes in there.
             */
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(defaultData.Length);

            Marshal.Copy(defaultData, 0, rgbOutputFileContents[0], defaultData.Length);

            // must be set to the number of bytes that we wrote to rgbOutputFileContents.
            pcbOutput = (uint)defaultData.Length;

            VsOutputWindowPaneManager.OutputWindowWriteLine(vsOutputWindowPaneName, "========== CSD file generation: complete! ==========");

            return 0;
        }



        /// <summary>
        /// Method that will communicate an error via the shell callback mechanism
        /// </summary>
        /// <param name="level">Level or severity</param>
        /// <param name="message">Text displayed to the user</param>
        /// <param name="line">Line number of error</param>
        /// <param name="column">Column number of error</param>
        protected virtual void GeneratorError(uint level, string message, uint line, uint column)
        {
            Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress progress = GeneratorProgress;
            if (progress != null)
            {
                progress.GeneratorError(0, level, message, line, column);
            }
        }
    }
}

