﻿#pragma checksum "..\..\WMenuMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8336A97D3E4E92C1F660EADB8C44CB9B"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Modeling;
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


namespace Modeling {
    
    
    /// <summary>
    /// WMenuMain
    /// </summary>
    public partial class WMenuMain : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_Back;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_Exit;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Main;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_ModelingMenu;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_Theory;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_Map;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\WMenuMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image im_Test;
        
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
            System.Uri resourceLocater = new System.Uri("/Modeling;component/wmenumain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\WMenuMain.xaml"
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
            
            #line 8 "..\..\WMenuMain.xaml"
            ((Modeling.WMenuMain)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 8 "..\..\WMenuMain.xaml"
            ((Modeling.WMenuMain)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.im_Back = ((System.Windows.Controls.Image)(target));
            
            #line 16 "..\..\WMenuMain.xaml"
            this.im_Back.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_Back_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.im_Exit = ((System.Windows.Controls.Image)(target));
            
            #line 19 "..\..\WMenuMain.xaml"
            this.im_Exit.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_Exit_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Main = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.im_ModelingMenu = ((System.Windows.Controls.Image)(target));
            
            #line 68 "..\..\WMenuMain.xaml"
            this.im_ModelingMenu.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_ModelingMenu_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.im_Theory = ((System.Windows.Controls.Image)(target));
            
            #line 70 "..\..\WMenuMain.xaml"
            this.im_Theory.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_Theory_MouseDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.im_Map = ((System.Windows.Controls.Image)(target));
            
            #line 72 "..\..\WMenuMain.xaml"
            this.im_Map.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_Map_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.im_Test = ((System.Windows.Controls.Image)(target));
            
            #line 74 "..\..\WMenuMain.xaml"
            this.im_Test.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.im_Test_MouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

