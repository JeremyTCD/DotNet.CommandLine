﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JeremyTCD.DotNet.CommandLine {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JeremyTCD.DotNet.CommandLine.Strings", typeof(Strings).GetTypeInfo().Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid arguments.
        /// </summary>
        public static string Exception_InvalidArguments {
            get {
                return ResourceManager.GetString("Exception_InvalidArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is an invalid value for option &quot;-{1}&quot;.
        /// </summary>
        public static string Exception_InvalidOptionValue {
            get {
                return ResourceManager.GetString("Exception_InvalidOptionValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Malformed arguments &quot;{0}&quot;.
        /// </summary>
        public static string Exception_MalformedArguments {
            get {
                return ResourceManager.GetString("Exception_MalformedArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Option &quot;-{0}&quot; does not exist.
        /// </summary>
        public static string Exception_OptionDoesNotExist {
            get {
                return ResourceManager.GetString("Exception_OptionDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; should not be null.
        /// </summary>
        public static string Exception_ParameterShouldBeNull {
            get {
                return ResourceManager.GetString("Exception_ParameterShouldBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property &quot;{0}&quot; cannot hold multiple values.
        /// </summary>
        public static string Exception_PropertyCannotHoldMultipleValues {
            get {
                return ResourceManager.GetString("Exception_PropertyCannotHoldMultipleValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property &quot;{0}&quot; does not have an OptionAttribute.
        /// </summary>
        public static string Exception_PropertyDoesNotHaveOptionAttribute {
            get {
                return ResourceManager.GetString("Exception_PropertyDoesNotHaveOptionAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property &quot;{0}&quot; is not a flag.
        /// </summary>
        public static string Exception_PropertyIsNotAFlag {
            get {
                return ResourceManager.GetString("Exception_PropertyIsNotAFlag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type &quot;{0}&quot; does not have a CommandAttribute.
        /// </summary>
        public static string Exception_TypeDoesNotHaveCommandAttribute {
            get {
                return ResourceManager.GetString("Exception_TypeDoesNotHaveCommandAttribute", resourceCulture);
            }
        }
    }
}
