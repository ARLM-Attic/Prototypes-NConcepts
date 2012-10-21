using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

//////////////////////////////////////////////////////////////////////
// Cmpt: SMTools
// Desc: Responsible for all the functionallity that was previously in
//       SMTools.exe
//////////////////////////////////////////////////////////////////////
namespace SMSharp
{
    public partial class SMTools : Component
    {
        public SMTools()
        {
            InitializeComponent();
        }

        public SMTools(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
