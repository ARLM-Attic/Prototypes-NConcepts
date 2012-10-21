using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMSharp
{
    public partial class FrmMainMdiParent : Form
    {
        //////////////////////////////////////////////////////////////////////
        // Func: FrmMainMdiParent
        // Note: Constructor By Default Launches 3 form windows 
        //       ~JagLogViewer is not started by Default because it is not
        //       used as often        
        //////////////////////////////////////////////////////////////////////
        public FrmMainMdiParent()
        {
            InitializeComponent();

            /* Initialize Instance Variables used only to add a Number to the
               title of a newly Created Form */
            m_iFuelConsoleInstances = 0;
            m_iJagLogViewerInstances = 0;
            m_iServerToolInstances = 0;
            m_iSMLogViewerInstances = 0;

            /* Create 3 Child Window Instances ~ by default*/            
            CreateChildWindowInstance("FuelConsole");
            CreateChildWindowInstance("ServerTool");
            CreateChildWindowInstance("SMLogViewer");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawEllipse(new Pen(Color.Blue,6), 10, 10, 20, 20);
            g.Dispose();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            base.OnPaintBackground(e);
            g.DrawEllipse(new Pen(Color.Blue), 10, 10, 10, 10);
        }

        private void ToolStripMenuItem_File_New_Click(object sender, EventArgs e)
        {
            FrmNewOpen NewOpenWindow = new FrmNewOpen();
            NewOpenWindow.MdiParent = this;
            NewOpenWindow.Show();
        }
        //////////////////////////////////////////////////////////////////////
        // Func: CreateChildWindowInstance
        // Desc: Creates a Child Window (Mdi Child Object) of a Form within
        //       the Main MDI Parent Form     
        //       
        // Prms: strWindowType can be either:
        //       -JagLogViewer
        //       -ServerTool
        //       -FuelConsole
        //       -SMLogViewer        
        //////////////////////////////////////////////////////////////////////
        public void CreateChildWindowInstance(string strWindowType)
        {

            switch (strWindowType)
            {
                case "JagLogViewer":

                    FrmJagLogViewer FrmJagLogViewerInstance = new FrmJagLogViewer();
                    FrmJagLogViewerInstance.MdiParent = this;

                    if (m_iJagLogViewerInstances != 0)
                    {
                        FrmJagLogViewerInstance.Text = FrmJagLogViewerInstance.Text + m_iJagLogViewerInstances.ToString();
                    }

                    FrmJagLogViewerInstance.Show();
                    m_iJagLogViewerInstances++;
                    break;

                case "ServerTool":

                    FrmServerTool FrmServerToolInstance = new FrmServerTool();
                    FrmServerToolInstance.MdiParent = this;

                    if (m_iServerToolInstances != 0)
                    {
                        FrmServerToolInstance.Text = FrmServerToolInstance.Text + m_iServerToolInstances.ToString();
                    }

                    FrmServerToolInstance.Show();
                    m_iServerToolInstances++;
                    break;

                case "FuelConsole":

                    FrmFuelConsole FrmFuelConsoleInstance = new FrmFuelConsole();
                    FrmFuelConsoleInstance.MdiParent = this;

                    if (m_iFuelConsoleInstances != 0)
                    {
                        FrmFuelConsoleInstance.Text = FrmFuelConsoleInstance.Text + m_iFuelConsoleInstances.ToString();
                    }

                    FrmFuelConsoleInstance.Show();
                    m_iFuelConsoleInstances++;
                    break;

                case "SMLogViewer":

                    FrmSMLogViewer FrmSMLogViewerInstance = new FrmSMLogViewer();
                    FrmSMLogViewerInstance.MdiParent = this;

                    if (m_iSMLogViewerInstances != 0)
                    {
                        FrmSMLogViewerInstance.Text = FrmSMLogViewerInstance.Text + m_iSMLogViewerInstances.ToString();
                    }

                    FrmSMLogViewerInstance.Show();
                    m_iSMLogViewerInstances++;
                    break;

                default:
                    return;
            }

        }
        //////////////////////////////////////////////////////////////////////              
        // Note: Refresh here so that the Background Logo Gets Redrawn
        //////////////////////////////////////////////////////////////////////
        private void FrmMainMdiParent_SizeChanged(object sender, EventArgs e)
        {
            //this.Refresh();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void arrangeIconsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ActiveMdiChild.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }                      
        
    }
}