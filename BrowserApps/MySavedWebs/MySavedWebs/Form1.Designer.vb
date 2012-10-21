<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PnlMain = New System.Windows.Forms.Panel
        Me.BtnRefresh = New System.Windows.Forms.Button
        Me.lnklblHome = New System.Windows.Forms.LinkLabel
        Me.CmbSub = New System.Windows.Forms.ComboBox
        Me.lblSub = New System.Windows.Forms.Label
        Me.lblMain = New System.Windows.Forms.Label
        Me.CmbMain = New System.Windows.Forms.ComboBox
        Me.SplitCMain = New System.Windows.Forms.SplitContainer
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.WbWindow = New System.Windows.Forms.WebBrowser
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FullScreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.PnlMain.SuspendLayout()
        Me.SplitCMain.Panel1.SuspendLayout()
        Me.SplitCMain.Panel2.SuspendLayout()
        Me.SplitCMain.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PnlMain
        '
        Me.PnlMain.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.PnlMain.Controls.Add(Me.BtnRefresh)
        Me.PnlMain.Controls.Add(Me.lnklblHome)
        Me.PnlMain.Controls.Add(Me.CmbSub)
        Me.PnlMain.Controls.Add(Me.lblSub)
        Me.PnlMain.Controls.Add(Me.lblMain)
        Me.PnlMain.Controls.Add(Me.CmbMain)
        Me.PnlMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.PnlMain.Location = New System.Drawing.Point(0, 24)
        Me.PnlMain.Name = "PnlMain"
        Me.PnlMain.Size = New System.Drawing.Size(773, 34)
        Me.PnlMain.TabIndex = 1
        '
        'BtnRefresh
        '
        Me.BtnRefresh.BackColor = System.Drawing.Color.Transparent
        Me.BtnRefresh.BackgroundImage = Global.MySavedWebs.My.Resources.Resources.gnome_starthere
        Me.BtnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnRefresh.Dock = System.Windows.Forms.DockStyle.Right
        Me.BtnRefresh.ForeColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.BtnRefresh.Location = New System.Drawing.Point(731, 0)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(42, 34)
        Me.BtnRefresh.TabIndex = 5
        Me.BtnRefresh.UseVisualStyleBackColor = False
        '
        'lnklblHome
        '
        Me.lnklblHome.AutoSize = True
        Me.lnklblHome.Location = New System.Drawing.Point(541, 10)
        Me.lnklblHome.Name = "lnklblHome"
        Me.lnklblHome.Size = New System.Drawing.Size(84, 13)
        Me.lnklblHome.TabIndex = 4
        Me.lnklblHome.TabStop = True
        Me.lnklblHome.Text = "Google Desktop"
        '
        'CmbSub
        '
        Me.CmbSub.FormattingEnabled = True
        Me.CmbSub.Location = New System.Drawing.Point(356, 7)
        Me.CmbSub.Name = "CmbSub"
        Me.CmbSub.Size = New System.Drawing.Size(173, 21)
        Me.CmbSub.TabIndex = 3
        '
        'lblSub
        '
        Me.lblSub.AutoSize = True
        Me.lblSub.Location = New System.Drawing.Point(280, 11)
        Me.lblSub.Name = "lblSub"
        Me.lblSub.Size = New System.Drawing.Size(71, 13)
        Me.lblSub.TabIndex = 2
        Me.lblSub.Text = "Sub Category"
        '
        'lblMain
        '
        Me.lblMain.AutoSize = True
        Me.lblMain.Location = New System.Drawing.Point(20, 10)
        Me.lblMain.Name = "lblMain"
        Me.lblMain.Size = New System.Drawing.Size(75, 13)
        Me.lblMain.TabIndex = 1
        Me.lblMain.Text = "Main Category"
        '
        'CmbMain
        '
        Me.CmbMain.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.CmbMain.Location = New System.Drawing.Point(101, 7)
        Me.CmbMain.Name = "CmbMain"
        Me.CmbMain.Size = New System.Drawing.Size(173, 21)
        Me.CmbMain.TabIndex = 0
        '
        'SplitCMain
        '
        Me.SplitCMain.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.SplitCMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitCMain.Location = New System.Drawing.Point(0, 58)
        Me.SplitCMain.Name = "SplitCMain"
        '
        'SplitCMain.Panel1
        '
        Me.SplitCMain.Panel1.Controls.Add(Me.ListBox1)
        '
        'SplitCMain.Panel2
        '
        Me.SplitCMain.Panel2.Controls.Add(Me.WbWindow)
        Me.SplitCMain.Size = New System.Drawing.Size(773, 349)
        Me.SplitCMain.SplitterDistance = 199
        Me.SplitCMain.TabIndex = 2
        '
        'ListBox1
        '
        Me.ListBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ListBox1.CausesValidation = False
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.ForeColor = System.Drawing.SystemColors.Info
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.IntegralHeight = False
        Me.ListBox1.Location = New System.Drawing.Point(0, 0)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(199, 349)
        Me.ListBox1.TabIndex = 0
        '
        'WbWindow
        '
        Me.WbWindow.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WbWindow.Location = New System.Drawing.Point(0, 0)
        Me.WbWindow.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WbWindow.Name = "WbWindow"
        Me.WbWindow.ScriptErrorsSuppressed = True
        Me.WbWindow.Size = New System.Drawing.Size(570, 349)
        Me.WbWindow.TabIndex = 0
        Me.WbWindow.Url = New System.Uri("http://127.0.0.1:4664/&s=Y7UY4qkUyGku4RFgQ95SU_IUcdI", System.UriKind.Absolute)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ViewToolStripMenuItem, Me.RefreshToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(773, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FullScreenToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'FullScreenToolStripMenuItem
        '
        Me.FullScreenToolStripMenuItem.Name = "FullScreenToolStripMenuItem"
        Me.FullScreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11
        Me.FullScreenToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.FullScreenToolStripMenuItem.Text = "FullScreen"
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(57, 20)
        Me.RefreshToolStripMenuItem.Text = "&Refresh"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.StatusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 407)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(773, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 429)
        Me.Controls.Add(Me.SplitCMain)
        Me.Controls.Add(Me.PnlMain)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FrmMain"
        Me.Text = "My Saved Webs"
        Me.PnlMain.ResumeLayout(False)
        Me.PnlMain.PerformLayout()
        Me.SplitCMain.Panel1.ResumeLayout(False)
        Me.SplitCMain.Panel2.ResumeLayout(False)
        Me.SplitCMain.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PnlMain As System.Windows.Forms.Panel
    Friend WithEvents SplitCMain As System.Windows.Forms.SplitContainer
    Friend WithEvents CmbSub As System.Windows.Forms.ComboBox
    Friend WithEvents lblSub As System.Windows.Forms.Label
    Friend WithEvents lblMain As System.Windows.Forms.Label
    Friend WithEvents CmbMain As System.Windows.Forms.ComboBox
    Friend WithEvents WbWindow As System.Windows.Forms.WebBrowser
    Friend WithEvents lnklblHome As System.Windows.Forms.LinkLabel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FullScreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BtnRefresh As System.Windows.Forms.Button
    Friend WithEvents RefreshToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
