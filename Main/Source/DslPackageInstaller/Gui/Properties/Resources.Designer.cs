﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5446
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gui.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gui.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wizard has detected that newer version of Configuration Section Designer Installer is already installed on your computer. Setup cannot continue..
        /// </summary>
        internal static string DowngradeNotSupported {
            get {
                return ResourceManager.GetString("DowngradeNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}\application.exe.
        /// </summary>
        internal static string FinishStepCommand {
            get {
                return ResourceManager.GetString("FinishStepCommand", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap InstallationStepImg {
            get {
                object obj = ResourceManager.GetObject("InstallationStepImg", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConfigurationSectionDesigner.msi.
        /// </summary>
        internal static string MainMsiFile {
            get {
                return ResourceManager.GetString("MainMsiFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wizard will install Configuration Section Designer Installer on your computer..
        /// </summary>
        internal static string WelcomeStepGreetingInstall {
            get {
                return ResourceManager.GetString("WelcomeStepGreetingInstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wizard will modify Configuration Section Designer Installer on your computer..
        /// </summary>
        internal static string WelcomeStepGreetingModify {
            get {
                return ResourceManager.GetString("WelcomeStepGreetingModify", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wizard will reinstall Configuration Section Designer Installer on your computer..
        /// </summary>
        internal static string WelcomeStepGreetingReinstall {
            get {
                return ResourceManager.GetString("WelcomeStepGreetingReinstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wizard will uninstall Configuration Section Designer Installer from your computer..
        /// </summary>
        internal static string WelcomeStepGreetingUninstall {
            get {
                return ResourceManager.GetString("WelcomeStepGreetingUninstall", resourceCulture);
            }
        }
    }
}