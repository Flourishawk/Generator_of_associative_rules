﻿#pragma checksum "..\..\Final.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6FDDABA2DA5C2BED065362424AFBC26F52E1E85A5B759AAA5F94A53ED319F6A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// Final
    /// </summary>
    public partial class Final : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Logo1;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Logo2;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Logo3;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label timeText;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ruleText;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label stepText;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image mouse;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image hideImg;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainButtonBd;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\Final.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainButtonCSV;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/final.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Final.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Logo1 = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.Logo2 = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.Logo3 = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.timeText = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.ruleText = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.stepText = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.mouse = ((System.Windows.Controls.Image)(target));
            
            #line 39 "..\..\Final.xaml"
            this.mouse.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.mouse_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.hideImg = ((System.Windows.Controls.Image)(target));
            
            #line 55 "..\..\Final.xaml"
            this.hideImg.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MainButtonBd = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\Final.xaml"
            this.MainButtonBd.Click += new System.Windows.RoutedEventHandler(this.MainButtonBd_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.MainButtonCSV = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\Final.xaml"
            this.MainButtonCSV.Click += new System.Windows.RoutedEventHandler(this.MainButtonCSV_ClickAsync);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
