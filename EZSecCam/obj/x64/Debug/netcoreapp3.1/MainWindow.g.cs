﻿#pragma checksum "..\..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C6A79651E34DAB68EA381F5EE00043757537D566"
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
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem StartWebcamMenuItem;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem HaarcascadeFaceDetectionMenuItem;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CaffeDNNFaceDetectionMenuItem;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem YoloV3DNNFaceDetectionMenuItem;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem FilterMenuItem;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem StartServerMenuItem;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ConnectMenuItem;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ConnectSettingsMenuItem;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image WebcamImage;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label InfoLabel;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ProgressLabel;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider ConfidenceSlider;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label StatusLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/EZSecCam;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MainWindow.xaml"
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
            this.StartWebcamMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 14 "..\..\..\..\MainWindow.xaml"
            this.StartWebcamMenuItem.Click += new System.Windows.RoutedEventHandler(this.StartWebcamMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.HaarcascadeFaceDetectionMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 18 "..\..\..\..\MainWindow.xaml"
            this.HaarcascadeFaceDetectionMenuItem.Click += new System.Windows.RoutedEventHandler(this.HaarcascadeFaceDetectionMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CaffeDNNFaceDetectionMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 19 "..\..\..\..\MainWindow.xaml"
            this.CaffeDNNFaceDetectionMenuItem.Click += new System.Windows.RoutedEventHandler(this.CaffeDNNFaceDetectionMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.YoloV3DNNFaceDetectionMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 20 "..\..\..\..\MainWindow.xaml"
            this.YoloV3DNNFaceDetectionMenuItem.Click += new System.Windows.RoutedEventHandler(this.YoloV3DNNFaceDetectionMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FilterMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 23 "..\..\..\..\MainWindow.xaml"
            this.FilterMenuItem.Click += new System.Windows.RoutedEventHandler(this.FilterMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.StartServerMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 26 "..\..\..\..\MainWindow.xaml"
            this.StartServerMenuItem.Click += new System.Windows.RoutedEventHandler(this.StartServerMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ConnectMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 27 "..\..\..\..\MainWindow.xaml"
            this.ConnectMenuItem.Click += new System.Windows.RoutedEventHandler(this.ConnectMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ConnectSettingsMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 28 "..\..\..\..\MainWindow.xaml"
            this.ConnectSettingsMenuItem.Click += new System.Windows.RoutedEventHandler(this.ConnectSettingsMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.WebcamImage = ((System.Windows.Controls.Image)(target));
            return;
            case 10:
            this.InfoLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.ProgressLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 12:
            this.ConfidenceSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 43 "..\..\..\..\MainWindow.xaml"
            this.ConfidenceSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.ConfidenceSlider_OnChange);
            
            #line default
            #line hidden
            return;
            case 13:
            this.StatusLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

