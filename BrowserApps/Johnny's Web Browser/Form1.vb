Public Class Form1
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        OpenFileDialog1.ShowDialog()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        WebBrowser1.GoBack()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        WebBrowser1.GoForward()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        WebBrowser1.Stop()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        WebBrowser1.Refresh()
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Form2.Show()
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        WebBrowser1.GoHome()
        ComboBox1.Text = "about:blank"
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        naviga()
    End Sub
    Private Sub ComboBox1_Keydown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox1.KeyDown
        If e.KeyCode = Keys.Return Then
            naviga()
        End If
    End Sub
    Private Sub naviga()
        'controllo indirizzo
        If ComboBox1.Text.StartsWith("http://") = False Then
            ComboBox1.Text = "http://" & ComboBox1.Text
        End If
        WebBrowser1.ScriptErrorsSuppressed = True
        WebBrowser1.Url = New Uri(ComboBox1.Text)
        'ComboBox1.Refresh()
    End Sub
    Private Sub webBrowser1_Navigated(ByVal sender As Object, ByVal e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        ComboBox1.Text = WebBrowser1.Url.ToString()
    End Sub
    Private Sub webBrowser1_StatusTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles WebBrowser1.StatusTextChanged
        Dim testo As String = WebBrowser1.StatusText
        Dim maxlun As Integer = (Me.Size.Width - 250) / 5.9
        Dim lungh As Integer = Len(testo)
        Dim testovis As String = Mid$(testo, 1, maxlun)
        If lungh > maxlun Then
            Label2.Text = testovis & "..."
        Else
            Label2.Text = testovis
        End If
    End Sub
    Private Sub WebBrowser1_ProgressChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserProgressChangedEventArgs) Handles WebBrowser1.ProgressChanged
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = e.MaximumProgress
        ProgressBar1.Value = e.CurrentProgress
    End Sub
    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        WebBrowser1.Navigate(OpenFileDialog1.FileName)
    End Sub
    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Me.Text = "Johnny's Web Browser - " & WebBrowser1.DocumentTitle
        'ComboBox1.Text = WebBrowser1.Url.ToString()
        Button1.Enabled = WebBrowser1.CanGoBack
        Button2.Enabled = WebBrowser1.CanGoForward
    End Sub
    Public Event NewWindow As System.ComponentModel.CancelEventHandler
    Private Sub WebBrowser1_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WebBrowser1.NewWindow
        e.Cancel = True
        Dim NewURL As String
        NewURL = CType(sender, WebBrowser).StatusText
        Dim frm As New Form1
        frm.WebBrowser1.Navigate(NewURL)
        If NewURL.StartsWith("http") Then frm.Show() Else frm.Close()
        frm.ComboBox1.Text = NewURL
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim f As New Form1
        f.Show()
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        WebBrowser1.ShowPrintPreviewDialog()
    End Sub
    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        WebBrowser1.ShowSaveAsDialog()
    End Sub
End Class
