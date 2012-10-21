Imports System.Collections.ObjectModel
Public Class FrmMain

    'Saved Webs Riit Directory
    Const RootDir As String = "E:\HPinvent\Webs\"
    Const HomePage As String = "http://a9.com/"

    'State data for List items
    Private strCatMain As String = ""
    Private strCatSub As String = ""


    Private Sub FrmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        CatInitialize()
        CatSubInitialize()
        ListInitialize()
        ListBox1.Focus()

        Dim HomePageURI As New Uri(HomePage)
        'WbWindow.Url = HomePageURI

    End Sub

    Private Sub CatInitialize()

        Dim FolderList As New ArrayList
        Dim folder As String

        'Get the folders from the Root Directory
        Dim folders As ReadOnlyCollection(Of String)
        folders = My.Computer.FileSystem.GetDirectories(RootDir)

        For Each folder In folders
            folder = folder.Remove(0, RootDir.Length())
            FolderList.Add(folder)
        Next

        'Set the Global State for CmbMain
        strCatMain = FolderList(0).ToString()
        CmbMain.DataSource = FolderList.Clone()
        FolderList.Clear()

    End Sub

    Private Sub CatSubInitialize()

        Dim FolderList As New ArrayList
        Dim folder As String

        'Get the Folders from CatMain
        Dim folders As ReadOnlyCollection(Of String)
        folders = My.Computer.FileSystem.GetDirectories(RootDir & _
        strCatMain & "\")

        For Each folder In folders
            folder = folder.Remove(0, RootDir.Length() + strCatMain.Length() + 1)
            FolderList.Add(folder)
        Next

        'Set the Global State for CmbSub
        strCatSub = FolderList(0).ToString()
        CmbSub.DataSource = FolderList.Clone()
        FolderList.Clear()

    End Sub

    Private Sub ListInitialize()

        'Get Files according to CatSub
        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.GetFiles(RootDir & strCatMain & "\" & strCatSub & "\", FileIO.SearchOption.SearchTopLevelOnly _
        , "*.htm")

        Dim file As String
        Dim fileList As New ArrayList

        For Each file In files
            file = file.Remove(0, RootDir.Length() + strCatMain.Length() + strCatSub.Length() + 2)
            fileList.Add(file)
        Next

        'Bind them
        ListBox1.DataSource = fileList.Clone()
        fileList.Clear()

    End Sub

    Private Sub lnklblHome_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnklblHome.LinkClicked

        Dim GoogleHome As New Uri("http://127.0.0.1:4664/&s=Y7UY4qkUyGku4RFgQ95SU_IUcdI")
        WbWindow.Url = GoogleHome

    End Sub

    Private Sub CmbMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbMain.SelectedIndexChanged

        strCatMain = CmbMain.SelectedValue().ToString()
        CatSubInitialize()
        ListInitialize()
        ListBox1.Focus()

    End Sub

    Private Sub CmbSub_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSub.SelectedIndexChanged

        strCatSub = CmbSub.SelectedValue().ToString()
        ListInitialize()
        ListBox1.Focus()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        Dim tempStr As String = "file:///" & Uri.EscapeDataString(RootDir) & _
        Uri.EscapeDataString(strCatMain) & Uri.EscapeDataString("/") & _
        Uri.EscapeDataString(strCatSub) & Uri.EscapeDataString("/") & _
        Uri.EscapeDataString(ListBox1.SelectedValue().ToString())

        'Set Browser to File URL
        Dim tempURI As New Uri(tempStr)
        WbWindow.Url = tempURI

    End Sub

    Private Sub FullScreenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullScreenToolStripMenuItem.Click

        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
        Else
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub BtnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRefresh.Click

        strCatMain = ""
        strCatSub = ""

        CatInitialize()
        CatSubInitialize()
        ListInitialize()

        Dim HomePageURI As New Uri(HomePage)
        WbWindow.Url = HomePageURI

    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click

        Dim tmpArgs As New System.EventArgs()
        BtnRefresh_Click(Me, tmpArgs)

    End Sub
End Class
