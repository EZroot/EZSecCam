﻿#pragma checksum "..\..\..\ConnectionWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0F653727FDFD89431F4BD71614AD0DC0C24FE2B7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EZSecCam;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace EZSecCam {
    
    
    /// <summary>
    /// ConnectionWindow
    /// </summary>
    public partial class ConnectionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IPInputTextBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PublicInputTextBox;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PublicExponentInputTextBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ApplySettingsButton;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\ConnectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConnectSettingsButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/EZSecCam;component/connectionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ConnectionWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.IPInputTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.PublicInputTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.PublicExponentInputTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.ApplySettingsButton = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\ConnectionWindow.xaml"
            this.ApplySettingsButton.Click += new System.Windows.RoutedEventHandler(this.ApplySettingsButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ConnectSettingsButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\ConnectionWindow.xaml"
            this.ConnectSettingsButton.Click += new System.Windows.RoutedEventHandler(this.ConnectSettingsButton_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

