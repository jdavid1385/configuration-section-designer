using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;

namespace ConfigurationSectionDesigner
{
    // Inform our Package that we're providing a Code Generator
    [ProvideCodeGenerator( typeof( CsdFileGenerator ), "CsdFileGenerator",
        "Generates implementations of the configuration describled in .csd files", true )]
    internal sealed partial class ConfigurationSectionDesignerPackage
    {
    }

    [Guid( "2FF11172-2CCC-4b42-8AD0-5E6400EB8728" )]
    public class CsdFileGenerator : VsMultipleFileGenerator<string>
    {
        internal static string ToolVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// An adaptor class on the TemplatedCodeGenerator in order to
        /// access the otherwise protected method GenerateCode, which we
        /// need to access from outside the class.
        /// 
        /// This class is used to run text transformations as simply as
        /// possible.
        /// </summary>
        private class TemplateCodeGenerator : TemplatedCodeGenerator, ITextTemplatingCallback
        {
            private bool errorOccured = false;
            private string errorMessage = null;

            public new byte[] GenerateCode( string inputFileName, string inputFileContent )
            {
                byte[] code = base.GenerateCode( inputFileName, inputFileContent );

                if( errorOccured )
                {
                    errorOccured = false;
                    string msg = errorMessage;
                    errorMessage = null;
                    throw new Exception( msg );
                }

                return code;
            }

            void ITextTemplatingCallback.ErrorCallback( bool warning, string message, int line, int column )
            {
                if( !warning && !errorOccured )
                {
                    errorOccured = true;
                    errorMessage = message;
                }

                base.ErrorCallback( warning, message, line, column );
            }
        }

        private TemplateCodeGenerator _templateCodeGenerator;
        private TemplateCodeGenerator TemplateGenerator
        {
            get
            {
                if( _templateCodeGenerator == null )
                {
                    _templateCodeGenerator = new TemplateCodeGenerator();
                    _templateCodeGenerator.SetSite( Site );
                }
                return _templateCodeGenerator;
            }
        }

        private CodeFileGenerator CodeFileGenerator
        {
            get
            {
                return new CodeFileGenerator( CodeProvider, RootNamespace );
            }
        }

        private string CodeFileExtension
        {
            get
            {
                string codeFileExtension = CodeProvider.FileExtension;
                if( codeFileExtension.StartsWith( "." ) )
                    codeFileExtension = codeFileExtension.Substring( 1 );
                return codeFileExtension;
            }
        }

        /// <summary>
        /// Returns a list of file extensions to generate or preserve
        /// </summary>
        /// <returns>A list of file extensions to generate or preserve</returns>
        public override IEnumerator<string> GetEnumerator()
        {
            // "Borrow" a List<>'s Enumerator to do our job for us
            List<string> fileExtensionList = new List<string>();

            fileExtensionList.Add( CodeFileExtension );
            fileExtensionList.Add( "diagram" );
            fileExtensionList.Add( "config" );
            fileExtensionList.Add( "xsd" );
            return fileExtensionList.GetEnumerator();
        }

        /// <summary>
        /// Returns the filename that matches the file extension given.
        /// </summary>
        /// <param name="fileExtension">One of the file extensions from the <see cref="GetEnumerator()"/> method.</param>
        /// <returns>The name of the input file plus the file extension.</returns>
        protected override string GetFileName( string fileExtension )
        {
            FileInfo fi = new FileInfo( InputFilePath );
            return string.Format( "{0}.{1}", fi.Name, fileExtension );
        }

        /// <summary>
        /// Generates the contents of the given file extension.
        /// If null is returned, it means to preserve the existing file instead
        /// of generating a new one.
        /// </summary>
        /// <param name="fileExtension">One of the file extensions from the <see cref="GetEnumerator()"/> method.</param>
        /// <returns>The generated content for the given file extension, or null.</returns>
        private byte[] GenerateAllContent( string fileExtension )
        {
            string inputFileContent;

            // For debugging purposes, it is better to be able to edit the .tt files without having to
            // recompile the solution to test changes. During release, use the embedded .tt files
            // instead.
            switch( fileExtension )
            {
                case "config":
                    inputFileContent = File.ReadAllText( Path.Combine( TextTemplateFolder, "ConfigurationSectionDesignerSample.tt" ) );
                    break;

                case "xsd":
                    inputFileContent = File.ReadAllText( Path.Combine( TextTemplateFolder, "ConfigurationSectionDesignerSchema.tt" ) );
                    break;

                // Preserve the diagram and cs files, don't write anything to them
                case "diagram":
                    return null;

                default:
                    if( fileExtension == CodeFileExtension )
                        return null;
                    else if( fileExtension == string.Format( "{0}-gen", CodeFileExtension ) )
                    {
                        return CodeFileGenerator.GenerateCode( InputFilePath );
                    }
                    else
                        throw new ApplicationException( "Unhandled file content" );
            }

            // Replace our input file name placeholder with the real input file name
            // so the text transformer knows which .csd file to work on.
            inputFileContent = inputFileContent.Replace( "$inputFileName$", InputFilePath );
            return TemplateGenerator.GenerateCode( InputFilePath, inputFileContent );
        }

        public override byte[] GenerateContent( string element )
        {
            return GenerateAllContent( element );
        }

        /// <summary>
        /// Implements the IVsSingleFileGenerator.DefaultExtension method. 
        /// Returns the extension of the generated file
        /// </summary>
        /// <param name="pbstrDefaultExtension">Out parameter, will hold the extension that is to be given to the output file name. The returned extension must include a leading period</param>
        /// <returns>S_OK if successful, E_FAIL if not</returns>
        public override int DefaultExtension(out string pbstrDefaultExtension)
        {
            try
            {
                pbstrDefaultExtension = string.Format(".csd.{0}", CodeFileExtension);
                return S_OK;
            }
            catch (Exception)
            {
                pbstrDefaultExtension = string.Empty;
                return E_FAIL;
            }

        }
        /*
        public override string GetDefaultExtension()
        {
            return string.Format( ".csd.{0}", CodeFileExtension );
        }
        */
        public override byte[] GenerateDefaultContent()
        {
            return GenerateAllContent( string.Format( "{0}-gen", CodeFileExtension ) );
        }

        private string _textTemplateFolder;
        private string TextTemplateFolder
        {
            get
            {
                if( _textTemplateFolder == null )
                {
#if DEBUG
                    _textTemplateFolder = Path.GetFullPath( @"..\..\..\DslSetup\TextTemplates" );
#else
                    // Fetch the install location from the registry
                    RegistryKey SOFTWARE = Registry.LocalMachine.OpenSubKey( "SOFTWARE" );
                    RegistryKey CSD = SOFTWARE.OpenSubKey( "ConfigurationSectionDesigner" );
                    if( CSD != null )
                        _textTemplateFolder = string.Format( @"{0}\TextTemplates", CSD.GetValue( "InstallLocation" ) as string );
                    else
                        throw new InvalidOperationException( "Could not find TextTemplate directory. Try reinstalling the Configuration Section Designer." );
#endif
                }
                return _textTemplateFolder;
            }
        }

    }
}
