using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using System.Xml;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using System.Text.RegularExpressions;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;

/*
 * Based on the code found here:
 * http://www.codeproject.com/KB/cs/VsMultipleFileGenerator.aspx
 */

namespace ConfigurationSectionDesigner
{
    internal static class VsHelper
    {
        static VSDOCUMENTPRIORITY[] StandardDocumentPriority = new VSDOCUMENTPRIORITY[(int)VSDOCUMENTPRIORITY.DP_Standard];


        /// <summary>
        /// Get the current Hierarchy
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IVsHierarchy GetCurrentHierarchy(IServiceProvider provider)
        {
            DTE vs = (DTE)provider.GetService(typeof(DTE));

            if (vs == null) throw new InvalidOperationException("DTE not found.");

            return ToHierarchy(vs.SelectedItems.Item(1).ProjectItem.ContainingProject);
        }

        /// <summary>
        /// Get the hierarchy corresponding to a Project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IVsHierarchy ToHierarchy(EnvDTE.Project project)
        {
            if (project == null) throw new ArgumentNullException("project");
            string projectGuid = null;
            // DTE does not expose the project GUID that exists at in the msbuild project file.        
            // Cannot use MSBuild object model because it uses a static instance of the Engine,         
            // and using the Project will cause it to be unloaded from the engine when the         
            // GC collects the variable that we declare.  
            using (XmlReader projectReader = XmlReader.Create(project.FileName))
            {
                projectReader.MoveToContent();
                if (projectReader.NameTable != null)
                {
                    object nodeName = projectReader.NameTable.Add("ProjectGuid");
                    while (projectReader.Read())
                    {
                        if (Object.Equals(projectReader.LocalName, nodeName))
                        {
                            projectGuid = (String)projectReader.ReadElementContentAsString();
                            break;
                        }
                    }
                }
            }
            Debug.Assert(!String.IsNullOrEmpty(projectGuid));

            IServiceProvider serviceProvider = new ServiceProvider(project.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            return VsShellUtilities.GetHierarchy(serviceProvider, new Guid(projectGuid ?? ""));

        }

        /// <summary>
        /// Converts an EnvDTE.Project to a Visual Studio project hierarchy.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IVsProject3 ToVsProject(EnvDTE.Project project)
        {
            if (project == null) throw new ArgumentNullException("project");
            IVsProject3 vsProject = ToHierarchy(project) as IVsProject3;
            if (vsProject == null)
            {
                throw new ArgumentException("Project is not a VS project.");
            }
            return vsProject;
        }

        /// <summary>
        /// Converts a Visual Studio project hierarchy to an EnvDTE.Project.
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static EnvDTE.Project ToDteProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null) throw new ArgumentNullException("hierarchy");
            object prjObject = null;
            if (hierarchy.GetProperty(0xfffffffe, -2027, out prjObject) >= 0)
            {
                return (EnvDTE.Project)prjObject;
            }
            else
            {
                throw new ArgumentException("Hierarchy is not a project.");
            }
        }

        /// <summary>
        /// Converts a Visual Studio project hierarchy to an EnvDTE.Project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static EnvDTE.Project ToDteProject(IVsProject project)
        {
            if (project == null) throw new ArgumentNullException("project");
            return ToDteProject(project as IVsHierarchy);
        }



        /// <summary>
        /// Locates the corresponding EnvDTE.ProjectItem associated with <paramref name="file"/>.
        /// </summary>
        /// <param name="project">the project to search through.</param>
        /// <param name="file">the path of the file we are locating.</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem FindProjectItem(EnvDTE.Project project, string file)
        {
            return FindProjectItem(project.ProjectItems, file);
        }

        /// <summary>
        /// Locates the corresponding EnvDTE.ProjectItem associated with <paramref name="file"/>.
        /// </summary>
        /// <param name="items">the items to search through.</param>
        /// <param name="file">the path of the file we are locating.</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem FindProjectItem(EnvDTE.ProjectItems items, string file)
        {
            string atom = file.Substring(0, file.IndexOf("\\") + 1);
            foreach (EnvDTE.ProjectItem item in items)
            {
                //if ( item
                //if (item.ProjectItems.Count > 0)
                if (atom.StartsWith(item.Name))
                {
                    // then step in
                    EnvDTE.ProjectItem ritem = FindProjectItem(item.ProjectItems, file.Substring(file.IndexOf("\\") + 1));
                    if (ritem != null)
                        return ritem;
                }
                if (Regex.IsMatch(item.Name, file))
                {
                    return item;
                }
                if (item.ProjectItems.Count > 0)
                {
                    EnvDTE.ProjectItem ritem = FindProjectItem(item.ProjectItems, file.Substring(file.IndexOf("\\") + 1));
                    if (ritem != null)
                        return ritem;
                }
            }
            return null;
        }

        /// <summary>
        /// Locates the corresponding EnvDTE.ProjectItem's whos name matches the regex <paramref name="match"/>.
        /// </summary>
        /// <param name="items">the items to search through.</param>
        /// <param name="match">the regex match string.</param>
        /// <returns></returns>
        public static List<EnvDTE.ProjectItem> FindProjectItems(EnvDTE.ProjectItems items, string match)
        {
            List<EnvDTE.ProjectItem> values = new List<EnvDTE.ProjectItem>();

            foreach (EnvDTE.ProjectItem item in items)
            {
                if (Regex.IsMatch(item.Name, match))
                {
                    values.Add(item);
                }
                if (item.ProjectItems.Count > 0)
                {
                    values.AddRange(FindProjectItems(item.ProjectItems, match));
                }
            }
            return values;
        }


        /// <summary>
        /// Looks through the <paramref name="project"/> and all subitems / subprojects to find the project hosting the active configuration section designer window.
        /// </summary>
        /// <param name="project">the parent project.</param>
        /// <param name="configurationSectionModelFile">the project item represented by inputFilePath, if found (ex: MyProject\ConfigurationSection.csd).</param>
        /// <param name="inputFilePath">the file path of the project item.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        public static bool TryFindProjectItemWithFilePath(Project project, string inputFilePath, out ProjectItem configurationSectionModelFile)
        {
            return TryFindProjectItemWithFilePath(project, StandardDocumentPriority, inputFilePath, out  configurationSectionModelFile);
        }

        /// <summary>
        /// Looks through the <paramref name="project"/> and all subitems / subprojects to find the project hosting the active configuration section designer window.
        /// </summary>
        /// <param name="project">the parent project.</param>
        /// <param name="docPriority">Specifies the priority level of a document within a project.</param>
        /// <param name="configurationSectionModelFile">the project item represented by inputFilePath, if found (ex: MyProject\ConfigurationSection.csd).</param>
        /// <param name="inputFilePath">the file path of the project item.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        public static bool TryFindProjectItemWithFilePath(Project project, VSDOCUMENTPRIORITY[] docPriority, string inputFilePath, out ProjectItem configurationSectionModelFile)
        {
            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemWithFilePath (VH.TFP).");
            bool projectFound = false;
            configurationSectionModelFile = null;

            // Nothing useful for this project. Process the next one.
            if (string.IsNullOrEmpty(project.FileName) || !File.Exists(project.FileName))
            {
                Diagnostics.DebugWrite("VH.TFP >> Nothing useful found in this current dte.Solution.Projects item. Continuing loop...");
                return false;
            }

            // obtain a reference to the current project as an IVsProject type
            IVsProject vsProject = VsHelper.ToVsProject(project);
            Diagnostics.DebugWrite("VH.TFP >> IVsProject vsProject = VsHelper.ToVsProject( project='{0}' )", project.Name);

            int iFound = 0;
            uint itemId = 0;

            // this locates, and returns a handle to our source file, as a ProjectItem
            vsProject.IsDocumentInProject(inputFilePath, out iFound, docPriority, out itemId);
            Diagnostics.DebugWrite("VH.TFP >> vsProject.IsDocumentInProject(inputFilePath, out iFound={0}, pdwPriority, out itemId={1}).", iFound, itemId);

            // TODO: [abm] Below here, project build failed! Error was "type is not of VsProject". This only occured on a brand new 
            // project with brand new csd. After failed rebuild attempt of project, it all worked.
            // Find out why and fix (or create warning message) to guide future users! Not yet reproducible...

            // if this source file is found in this project
            if (iFound != 0 && itemId != 0)
            {
                Diagnostics.DebugWrite("VH.TFP >> (iFound != 0 && itemId != 0) == TRUE!!!");
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp = null;
                vsProject.GetItemContext(itemId, out oleSp);
                if (oleSp != null)
                {
                    Diagnostics.DebugWrite("VH.TFP >> vsProject.GetItemContext( itemId, out oleSp ) >> oleSp != null! Getting ServiceProvider sp...");
                    ServiceProvider sp = new ServiceProvider(oleSp);
                    // convert our handle to a ProjectItem
                    configurationSectionModelFile = sp.GetService(typeof(ProjectItem)) as ProjectItem;

                    if (configurationSectionModelFile != null)
                    {
                        Diagnostics.DebugWrite("VH.TFP >>  configurationSectionModelFile = sp.GetService( typeof( ProjectItem ) ) as ProjectItem is NOT null! Setting this._project to the project we were working on...");
                        // We now have what we need. Stop looking.
                        projectFound = true;
                    }
                }
            }
            return projectFound;
        }


        /// <summary>
        /// Recursively traverses the Solution hierarchy looking for the project item designated by <paramref name="filePath"/>, and 
        /// it's parent project.
        /// </summary>
        /// <remarks>
        /// The recursion in this method has gotten ugly and complex, but it appears to work. It may be nice to find a more elegant solution 
        /// to the problem of locating these items in the future. [a.moore]
        /// </remarks>
        /// <param name="filePath">The path of the file to locate.</param>
        /// <param name="parentProject">(out) the parent project of the file, if found.</param>
        /// <param name="fileProjectItem">(out) the project item of the file, if found.</param>
        /// <returns><c>true</c> if found, otherwise <c>false</c></returns>
        public static bool TryFindProjectItemAndParentProject(string filePath, out Project parentProject, out ProjectItem fileProjectItem)
        {
            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject (Sol Level) >> filePath = '{0}'.", filePath);
            parentProject = null;
            fileProjectItem = null;
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));
            foreach (Project project in dte.Solution.Projects)
            {
                Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> Current project = '{0}'.", project.Name ?? "<null>");
                if (project.ProjectItems != null && project.ProjectItems.Count > 0)
                {
                    Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> project.ProjectItems != null && Number of project items ({0}) > 0", project.ProjectItems.Count);
                    // First, look for the project at the solution level (No solution folders, subprojects, etc).
                    if (TryFindProjectItemWithFilePath(project, filePath, out fileProjectItem))
                    {
                        // The project was found at the solution level.
                        parentProject = project;
                        Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> PROJECT FOUND! Name='{0}'", project.Name);
                        return true;
                    }
                    else
                    {
                        // Project NOT found at the solution level. Start drilling down into sub project items...
                        Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> TryFindProjectItemWithFilePath(project, filePath, out fileProjectItem) == FALSE");
                        if (TryFindProjectItemAndParentProject(filePath, project.ProjectItems, out parentProject, out fileProjectItem))
                        {
                            // The project was found after drilling down.
                            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> PROJECT FOUND! Name='{0}'", project.Name);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Recursively traverses the Solution hierarchy, starting at <paramref name="currentProjectItems"/>, looking for 
        /// the project item designated by <paramref name="filePath"/>, and it's parent project.
        /// </summary>
        /// <remarks>
        /// The recursion in this method has gotten ugly and complex, but it appears to work. It may be nice to find a more elegant solution 
        /// to the problem of locating these items in the future. [a.moore]
        /// </remarks>
        /// <param name="filePath">The path of the file to locate.</param>
        /// <param name="currentProjectItems"></param>
        /// <param name="parentProject">(out) the parent project of the file, if found.</param>
        /// <param name="fileProjectItem">(out) the project item of the file, if found.</param>
        /// <returns><c>true</c> if found, otherwise <c>false</c></returns>
        private static bool TryFindProjectItemAndParentProject(string filePath, ProjectItems currentProjectItems, out Project parentProject, out ProjectItem fileProjectItem)
        {
            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject (ProjectItems Level) >> currentProjectItems.Count = '{0}'.", currentProjectItems.Count);
            parentProject = null;
            fileProjectItem = null;

            // Already checked by calling method, but leaving null check for future use...
            if (currentProjectItems != null)
            {
                foreach (ProjectItem projectItem in currentProjectItems)
                {
                    Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> Current projectItem = '{0}', projectItem.ProjectItems.Count={1},", projectItem.Name, (projectItem.ProjectItems != null ? projectItem.ProjectItems.Count.ToString() : "0"));
                    if (projectItem.SubProject != null) // Check SubProject.
                    {
                        Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> projectItem.SubProject EXISTS.");

                        // If item was matched to file or recursive call returned true, exit.
                        if ((!string.IsNullOrEmpty(projectItem.SubProject.FullName)
                            && TryFindProjectItemWithFilePath(projectItem.SubProject, filePath, out fileProjectItem)))
                        {
                            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> PROJECT FOUND in SubProject!");
                            parentProject = projectItem.SubProject;
                            return true;
                        }

                        // Look through the SubProject's project items.
                        if (projectItem.SubProject.ProjectItems != null
                            && TryFindProjectItemAndParentProject(filePath, projectItem.SubProject.ProjectItems, out parentProject, out fileProjectItem))
                        {
                            Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> PROJECT FOUND in SubProject.ProjectItems!");
                            return true;
                        }
                    }
                    else if (
                        projectItem.ProjectItems != null
                        && projectItem.ProjectItems.Count > 0
                        && TryFindProjectItemAndParentProject(filePath, projectItem.ProjectItems, out parentProject, out fileProjectItem)
                        ) // Check project items.
                    {
                        Diagnostics.DebugWrite("VsHelper.TryFindProjectItemAndParentProject >> PROJECT FOUND in projectItem!");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a list of all project references.
        /// </summary>
        /// <remarks>
        /// REF=http://www.codeproject.com/KB/macros/EnvDTE.aspx
        /// </remarks>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetReferences(Project project)
        {
            if (project.Object is VSLangProj.VSProject)
            {
                VSLangProj.VSProject vsproject = (VSLangProj.VSProject)project.Object;
                List<KeyValuePair<string, string>> list =
                   new List<KeyValuePair<string, string>>();
                foreach (VSLangProj.Reference reference in vsproject.References)
                {
                    if (reference.StrongName)
                        //System.Configuration, Version=2.0.0.0,
                        //Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A
                        list.Add(new KeyValuePair<string, string>(reference.Identity,
                            reference.Identity +
                            ", Version=" + reference.Version +
                            ", Culture=" + (string.IsNullOrEmpty(reference.Culture) ? "neutral" : reference.Culture) +
                            ", PublicKeyToken=" + reference.PublicKeyToken));
                    else
                        list.Add(new KeyValuePair<string, string>(
                                     reference.Identity, reference.Path));
                }
                return list;
            }
            else if (project.Object is VsWebSite.VSWebSite)
            {
                VsWebSite.VSWebSite vswebsite = (VsWebSite.VSWebSite)project.Object;
                //List<string> list = new List<string>();
                List<KeyValuePair<string, string>> list =
                   new List<KeyValuePair<string, string>>();
                foreach (VsWebSite.AssemblyReference reference in vswebsite.References)
                {
                    string value = "";
                    if (reference.FullPath != "")
                    {
                        FileInfo f = new FileInfo(reference.FullPath + ".refresh");
                        if (f.Exists)
                        {
                            using (FileStream stream = f.OpenRead())
                            {
                                using (StreamReader r = new StreamReader(stream))
                                {
                                    value = r.ReadToEnd().Trim();
                                }
                            }
                        }
                    }
                    if (value == "")
                    {
                        list.Add(new KeyValuePair<string, string>(reference.Name,
                                 reference.StrongName));
                    }
                    else
                    {
                        list.Add(new KeyValuePair<string, string>(reference.Name, value));
                    }
                }
                return list;
            }
            else
            {
                throw new Exception("Currently, system is only set up to " +
                                    "do references for normal projects.");
            }
        }

        /// <summary>
        /// Adds a reference to the selected project.
        /// </summary>
        /// <remarks>
        /// REF=http://www.codeproject.com/KB/macros/EnvDTE.aspx
        /// </remarks>
        /// <param name="project"></param>
        /// <param name="referenceStrIdentity"></param>
        /// <param name="browseUrl"></param>
        public static void AddProjectReference(Project project, string referenceStrIdentity, string browseUrl)
        {
            //browseUrl is either the File Path or the Strong Name
            //(System.Configuration, Version=2.0.0.0, Culture=neutral,
            //                       PublicKeyToken=B03F5F7F11D50A3A)
            string path = "";

            if (!browseUrl.StartsWith(referenceStrIdentity))
            {
                //it is a path
                path = browseUrl;
            }


            if (project.Object is VSLangProj.VSProject)
            {
                VSLangProj.VSProject vsproject = (VSLangProj.VSProject)project.Object;
                VSLangProj.Reference reference = null;
                try
                {
                    reference = vsproject.References.Find(referenceStrIdentity);
                }
                catch (Exception ex)
                {
                    //it failed to find one, so it must not exist. 
                    //But it decided to error for the fun of it. :)
                }
                if (reference == null)
                {
                    if (path == "")
                        vsproject.References.Add(browseUrl);
                    else
                        vsproject.References.Add(path);
                }
                else
                {
                    throw new Exception("Reference already exists.");
                }
            }
            else if (project.Object is VsWebSite.VSWebSite)
            {
                VsWebSite.VSWebSite vswebsite = (VsWebSite.VSWebSite)project.Object;
                VsWebSite.AssemblyReference reference = null;
                try
                {
                    foreach (VsWebSite.AssemblyReference r in vswebsite.References)
                    {
                        if (r.Name == referenceStrIdentity)
                        {
                            reference = r;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //it failed to find one, so it must not exist. 
                    //But it decided to error for the fun of it. :)
                }
                if (reference == null)
                {
                    if (path == "")
                        vswebsite.References.AddFromGAC(browseUrl);
                    else
                        vswebsite.References.AddFromFile(path);

                }
                else
                {
                    throw new Exception("Reference already exists.");
                }
            }
            else
            {
                throw new Exception("Currently, system is only set up " +
                          "to do references for normal projects.");
            }
        }

    }

    // See http://blogs.msdn.com/b/mshneer/archive/2009/12/07/interop-type-xxx-cannot-be-embedded-use-the-applicable-interface-instead.aspx
    // for more info on why we have this.
    public abstract class EnvDTEConstants
    {
        public const string vsDocumentKindText = "{8E7B96A8-E33D-11D0-A6D5-00C04FB67F6A}";

        public const string vsWindowKindOutput = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";

        public const string vsSolutionFolderType = "2150E333-8FDC-42a3-9474-1A3956D46DE8";
    }

    // TODO: Move to common area.
    public class Diagnostics
    {
        public const bool ForceReleaseModeDebugLogging = true;


        /*
        public static void DebugWriteLine(string format, params object[] args)
        {
            DebugWrite(format + "\r\n", args);
        }
        */
        public static void DebugWrite(string format, params object[] args)
        {
            if (ForceReleaseModeDebugLogging)
            {
                System.Diagnostics.Debugger.Log(0, "", string.Format(CultureInfo.InvariantCulture, format, args));
            }
            else
            {
                Debug.Write(string.Format(CultureInfo.InvariantCulture, format, args));
            }
        }


    }
}
