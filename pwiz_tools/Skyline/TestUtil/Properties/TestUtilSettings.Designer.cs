﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pwiz.SkylineTestUtil.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.9.0.0")]
    internal sealed partial class TestUtilSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TestUtilSettings defaultInstance = ((TestUtilSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TestUtilSettings())));
        
        public static TestUtilSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShowPreview {
            get {
                return ((bool)(this["ShowPreview"]));
            }
            set {
                this["ShowPreview"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point PreviewFormLocation {
            get {
                return ((global::System.Drawing.Point)(this["PreviewFormLocation"]));
            }
            set {
                this["PreviewFormLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size PreviewFormSize {
            get {
                return ((global::System.Drawing.Size)(this["PreviewFormSize"]));
            }
            set {
                this["PreviewFormSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PreviewFormMaximized {
            get {
                return ((bool)(this["PreviewFormMaximized"]));
            }
            set {
                this["PreviewFormMaximized"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ManualSizePreview {
            get {
                return ((bool)(this["ManualSizePreview"]));
            }
            set {
                this["ManualSizePreview"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int OldImageSource {
            get {
                return ((int)(this["OldImageSource"]));
            }
            set {
                this["OldImageSource"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color ImageDiffColor {
            get {
                return ((global::System.Drawing.Color)(this["ImageDiffColor"]));
            }
            set {
                this["ImageDiffColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127")]
        public int ImageDiffAlpha {
            get {
                return ((int)(this["ImageDiffAlpha"]));
            }
            set {
                this["ImageDiffAlpha"] = value;
            }
        }
    }
}
