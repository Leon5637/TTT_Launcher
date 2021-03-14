Option Strict On
Imports System.IO
Imports System.IO.Compression

Public Class Form1


    'Dim tb As List(Of TextBox)   '??? why doesn't this work
    Dim tb As New List(Of TextBox)
    Dim filePath As String = "garrysmod/cfg/server.cfg"
    Dim filecontent() As String
    Dim headLine As Integer
    Dim value As Integer
    Dim abort As Boolean
    Dim rect As Rectangle
    Dim custom_settings(5) As String
    Dim custom_settings_path As String = "garrysmod/cfg/custom_settings.cfg"
    Dim steamcmdu As New Process()
    Dim steamCMDPath As String


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

#If 1 Then
        'Prüfen ob benötigte Dateien vorhanden sind
        If System.IO.File.Exists(filePath) = False Or System.IO.File.Exists("srcds.exe") = False Then
            MessageBox.Show("Attention! Some required files could not be found. The programm might crash. Please move this Launcher into the main-directory of the game!")
            Button1.Enabled = False
            Exit Sub
            'Me.Close()
        End If
        Button1.Enabled = True
#End If
        filecontent = System.IO.File.ReadAllLines(filePath)



        'Liste mit allen Textboxen erstellen
        With tb
            .Add(TextBox1)
            .Add(TextBox2)
            .Add(TextBox3)
            .Add(TextBox4)
            .Add(TextBox5)
            .Add(TextBox6)
            .Add(TextBox7)
            .Add(TextBox8)
            .Add(TextBox9)
            .Add(TextBox10)
            .Add(TextBox11)
            .Add(TextBox12)
            .Add(TextBox13)
            .Add(TextBox14)
            .Add(TextBox15)
            .Add(TextBox16)
            .Add(TextBox17)
            .Add(TextBox18)
            .Add(TextBox19)
            .Add(TextBox20)
            .Add(TextBox21)
            .Add(TextBox22)
            .Add(TextBox23)
            .Add(TextBox24)
            .Add(TextBox25)
            .Add(TextBox26)
            .Add(TextBox27)
            .Add(TextBox28)
            .Add(TextBox29)
            .Add(TextBox30)
            .Add(TextBox31)
            .Add(TextBox32)
            .Add(TextBox33)
            .Add(TextBox34)
            .Add(TextBox35)
            .Add(TextBox36)
            .Add(TextBox37)
            .Add(TextBox38)
            .Add(TextBox39)
            .Add(TextBox40)
            .Add(TextBox41)
            .Add(TextBox42)
            .Add(TextBox43)
            .Add(TextBox44)
            .Add(TextBox45)
            .Add(TextBox46)

            .Add(TextBox60)
            .Add(TextBox62)
            .Add(TextBox81)
            .Add(TextBox80)
            .Add(TextBox61)
        End With

        'For i As Integer = 0 To 45
        '    tb(i).Text = (i + 1).ToString
        'Next

        Dim index As Integer = 0
        Dim currentTb As TextBox

        For Each currentTb In tb                                                        'schleife für alle 46 Textboxen durchgehen
            Try
                'auf leerzeile oder Kommentarzeile prüfen
                While filecontent(index).Trim = "" Or filecontent(index).Contains("//")      'so lange die schleife durchgehen bis weder Kommtar noch leerzeile und dabei schreibende zeile hochzählen (i2) 
                    index += 1
                End While

                If filecontent(index).Contains(""""c) Then
                    Dim startIndex As Integer = filecontent(index).IndexOf(""""c)
                    Dim endIndex As Integer = filecontent(index).LastIndexOf(""""c)
                    filecontent(index) = filecontent(index).Substring(startIndex, endIndex - startIndex)
                Else
                    filecontent(index) = filecontent(index).Split.Last
                End If

                filecontent(index) = filecontent(index).TrimStart(""""c)
                filecontent(index) = filecontent(index).TrimEnd(""""c)

                currentTb.Text = filecontent(index)                                     'die höchgezählte Zeile in die Textbox schreiben
                index += 1

            Catch ex As System.IndexOutOfRangeException
                If filecontent.Length <= index Then                                     'Falls die Konfigdatei nicht über genügend Zeilen verfügt
                    createNewConfig()

                    Form1_Load(Nothing, Nothing)
                    Exit Sub
                End If
            End Try



        Next

        'Authkey ausfindig machen
        If System.IO.File.Exists(custom_settings_path) Then
            If System.IO.File.ReadAllLines(custom_settings_path).Length >= 5 Then
                custom_settings = System.IO.File.ReadAllLines(custom_settings_path)
                'TextBox48.Text = System.IO.File.ReadAllText("garrysmod/cfg/authkey.cfg")
                For i As Integer = 0 To 4
                    tb((i + 46)).Text = custom_settings(i).Split.Last
                Next
            End If
        End If

        Try
            If TextBox60.Text = "" Then
                LinkLabel2.Visible = True
            Else
                LinkLabel2.Visible = False
            End If
        Catch ex As System.InvalidOperationException

        End Try

    End Sub

    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll

        Panel1.Top = -VScrollBar1.Value

        'Label64.Text = VScrollBar1.Value.ToString

        'Label65.Text = Panel1.Top.ToString

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs, Optional ByVal saveAndExit As Boolean = False) Handles Button1.Click

        Dim index As Integer = 0
        Dim filecontentOriginal() As String = System.IO.File.ReadAllLines(filePath)


        For i As Integer = 0 To 45                                          'schleife für alle 46 Textboxen durchgehen
            While filecontent(index).Trim = "" Or filecontent(index).Contains("//")      'so lange die schleife durchgehen bis weder Kommtar noch leerzeile und dabei schreibende zeile hochzählen (i2) 
                index += 1
            End While

            If i < 3 Then
                value = filecontentOriginal(index).IndexOf(""""c)
                filecontentOriginal(index) = filecontentOriginal(index).Remove(value, (filecontentOriginal(index).Length - (value))).Insert(value, """" + tb(i).Text + """")
            Else
                value = filecontentOriginal(index).LastIndexOf(" ")                        'alles vor dem Leerzeichen, hinter welchem der Wert folgt entfernen
                filecontentOriginal(index) = filecontentOriginal(index).Remove(value + 1, (filecontentOriginal(index).Length - (value + 1))).Insert(value + 1, tb(i).Text)
            End If

            index += 1


        Next

        'Schreiben aller überarbeiteter Zeilen in die cfg-Datei
        File.WriteAllLines(filePath, filecontentOriginal)

        'Nummer für die Kollektion festlegen 
        If CheckBox1.Checked = True Or TextBox80.Text = "" Then
            custom_settings(3) = "1809068913"
        Else
            custom_settings(3) = TextBox80.Text
        End If

        'RAM, Map-Name, Spieler-Anzahl und Authkey aus Textbox übernehmen
        custom_settings(4) = TextBox61.Text
        custom_settings(2) = TextBox81.Text
        custom_settings(1) = TextBox62.Text
        custom_settings(0) = TextBox60.Text

        'Server mit den entsprechenden Parametern starten
        If saveAndExit = False Then
            Process.Start("srcds.exe", "-Console -maxplayers " & custom_settings(1) & " -game garrysmod -mem_max_heapsize " & custom_settings(4) & " +gamemode terrortown +map " & custom_settings(2) & " -authkey " & custom_settings(0) & " +host_workshop_collection " & custom_settings(3))
        End If
        custom_settings(0) = "authkey " & custom_settings(0)
        custom_settings(1) = "players " & custom_settings(1)
        custom_settings(2) = "map " & custom_settings(2)
        custom_settings(3) = "collection " & custom_settings(3)
        custom_settings(4) = "ram-size " & custom_settings(4)
        File.WriteAllLines(custom_settings_path, custom_settings)

        If saveAndExit Then Exit Sub
        'Falls Herunterfahren aktiv, das Programm dazu auf zweiten Thread ausführen
        While Process.GetProcessesByName("srcds").Count = 0
            System.Threading.Thread.Sleep(100)
            ProgressBar1.Value += 10
        End While
        ProgressBar1.Value = 100
        If CheckBox2.Checked = True Then
            Button2.Enabled = True
            BackgroundWorker1.RunWorkerAsync()
        End If




    End Sub



    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        'Einfügen der Nummer für die TTT Kollektion von LeEHil
        If CheckBox1.Checked = True Then
            TextBox80.Enabled = False
            TextBox80.Text = "1809068913"
        Else
            TextBox80.Enabled = True
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Cancel-Button legt Abbruchbedingung für das spätere Herunterfahren fest und bricht ggf. aktiven Shutdown-timer ab
        System.Diagnostics.Process.Start("shutdown", "-a")
        Button2.Enabled = False
        abort = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("notepad", filePath)
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'Seperaten Vorgang auf zweiten Thread ausführen (damit Programm weiterläuft)
        'Solange der Server läuft hängt das Programm in der whileschleife fest (mit dem cancel-Button kann man diese verlassen)
        abort = False
        While Process.GetProcessesByName("srcds").Count > 0
            If abort = True Then
                ProgressBar1.Value = 0
                Exit Sub
            End If
        End While
        ProgressBar1.Value = 0
        System.Diagnostics.Process.Start("shutdown", "-s -t 60")
    End Sub

    Private Sub WheelScroll(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles SplitContainer1.Panel2.MouseWheel

        Dim scrollSpeed As Integer = 40
        Dim wheeldirection As Integer = e.Delta

        'Mausrichtung abfragen und je nachdem den Scrollbalken sowie den Inhalt verschieben
        If wheeldirection < 0 And Panel1.Top >= -(750 - scrollSpeed) Then
            Panel1.Top -= scrollSpeed
            VScrollBar1.Value += scrollSpeed
        ElseIf wheeldirection < 0 And Panel1.Top <= -(750 - scrollSpeed) Then
            VScrollBar1.Value = 750
        ElseIf wheeldirection > 0 And Panel1.Top <= -scrollSpeed Then
            Panel1.Top += scrollSpeed
            VScrollBar1.Value -= scrollSpeed
        ElseIf wheeldirection > 0 And Panel1.Top >= -scrollSpeed Then
            VScrollBar1.Value = 0
        End If
        'End If

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("http://steamcommunity.com/dev/apikey")
    End Sub

    Private Sub TextBox48_TextChanged(sender As Object, e As EventArgs) Handles TextBox60.TextChanged
        If TextBox60.Text = "" Then
            LinkLabel2.Visible = True
        Else
            LinkLabel2.Visible = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Update.Click

        Select Case MsgBox("This might actually take some time. Please wait until the progress is completed. " & vbCrLf & "If you already have the steamcmd.exe you can select it or have it downloaded. Do you want to download it now?", MsgBoxStyle.YesNoCancel, "TTT Update")
            Case MsgBoxResult.Cancel
                Exit Sub
            Case MsgBoxResult.No
                Dim fd As OpenFileDialog = New OpenFileDialog()
                fd.Title = "SteamCMD Directory"
                fd.InitialDirectory = "C:\"
                fd.Filter = "steamcmd (*.exe)|*.exe"
                fd.RestoreDirectory = True
                If fd.ShowDialog() = DialogResult.OK Then
                    steamCMDPath = fd.FileName
                Else
                    Exit Sub
                End If
            Case MsgBoxResult.Yes
                DownloadAndExtraxtSteamCMD()
        End Select

        If steamCMDPath <> Nothing And System.IO.File.Exists(steamCMDPath) = True Then
            steamcmdu.StartInfo.FileName = steamCMDPath
            steamcmdu.StartInfo.Arguments = "+login anonymous +force_install_dir " & """" & My.Computer.FileSystem.CurrentDirectory & """" & " +app_update 4020 validate +quit"
            steamcmdu.StartInfo.UseShellExecute = False
            steamcmdu.StartInfo.RedirectStandardOutput = False
            steamcmdu.StartInfo.CreateNoWindow = False
            steamcmdu.Start()
            'ProgressBar1.Value = 0
            'While Not steamcmdu.StandardOutput.ReadLine().Contains("Update State")
            '    Dim line As String = steamcmdu.StandardOutput.ReadLine()
            '    If line.Contains("Loading Steam API...OK") Then
            '        ProgressBar1.Value = 20
            '    ElseIf line.Contains("Logged in OK") Then
            '        ProgressBar1.Value = 50
            '        Exit While
            '    ElseIf line.Contains("downloading, progress:") Then
            '        Dim textArray() As String = Split(line)
            '        Dim progress As Integer
            '        For index As Integer = 0 To textArray.Length
            '            If textArray(index) = "progress:" Then
            '                If textArray(index + 1).IndexOf(".") <> -1 Then textArray(index + 1) = textArray(index + 1).Remove(textArray(index + 1).IndexOf("."))
            '                progress = Convert.ToInt32(textArray(index + 1))
            '            End If
            '        Next
            '        ProgressBar1.Value = Convert.ToInt32(10 + (progress * 0.9))
            '    End If
            'End While
            'steamcmdu.WaitForExit()
            BackgroundWorker2.RunWorkerAsync()
            ProgressBar1.Value = 0
        End If
    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        If steamcmdu.HasExited = False Then
            'MessageBox.Show(steamcmdu.StandardOutput.ReadLine())
            steamcmdu.WaitForExit()
        End If
        Me.Invoke(New MethodInvoker(Sub() Form1_Load(sender, e)))

    End Sub

    Private Sub DownloadAndExtraxtSteamCMD()
        steamCMDPath = Environment.ExpandEnvironmentVariables("%AppData%\steamcmd")
        If Not System.IO.Directory.Exists(steamCMDPath) Then
            System.IO.Directory.CreateDirectory(steamCMDPath)
        End If

        If Not System.IO.File.Exists(steamCMDPath & "\steamcmd.exe") Then
            My.Computer.Network.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", steamCMDPath & "\steamcmd.zip", String.Empty, String.Empty, False, 100000, True)
            Compression.ZipFile.ExtractToDirectory(steamCMDPath & "\steamcmd.zip", steamCMDPath)
            System.IO.File.Delete(steamCMDPath & "\steamcmd.zip")
        End If

        MessageBox.Show("steamcmd.exe successfully located at " & steamCMDPath, "steamcmd.exe")

        steamCMDPath = steamCMDPath & "\steamcmd.exe"

    End Sub

    Private Sub createNewConfig()
        Dim baseFile As Byte()
        baseFile = Convert.FromBase64String("Ly8gc2VydmVyIG5hbWUNCmhvc3RuYW1lICJbR0VSXVtBU1NBVUxUXSBUVFQtU2VydmVyIg0Kc3ZfY29udGFjdCAiYXNzYXVsdEBhc3NhdWx0LmRlIg0KDQovLyByY29uIHBhc3Nzd29yZA0KcmNvbl9wYXNzd29yZCANCg0KLy9ETkENCnR0dF9raWxsZXJfZG5hX3JhbmdlIDMwMA0KdHR0X2tpbGxlcl9kbmFfYmFzZXRpbWUgMTAwDQoNCg0KLy9QcmVwDQp0dHRfZmlyc3RwcmVwdGltZSA0NQ0KdHR0X3ByZXB0aW1lX3NlY29uZHMgNDANCnR0dF9wb3N0dGltZV9zZWNvbmRzIDIwDQoNCi8vUm91bmQgbGVuZ3RoDQp0dHRfaGFzdGUgMQ0KdHR0X2hhc3RlX3N0YXJ0aW5nX21pbnV0ZXMgNy41DQp0dHRfaGFzdGVfbWludXRlc19wZXJfZGVhdGggMS4wDQoNCnR0dF9yb3VuZHRpbWVfbWludXRlcyAxMA0KDQoNCg0KLy9NYXAgU3dpdGNoaW5nDQp0dHRfcm91bmRfbGltaXQgNg0KdHR0X3RpbWVfbGltaXRfbWludXRlcyA3NQ0KDQovL3R0dF9hbHdheXNfdXNlX21hcGN5Y2xlIDANCg0KDQovL1BsYXllciBDb3VudHMNCnR0dF9taW5pbXVtX3BsYXllcnMgMg0KdHR0X3RyYWl0b3JfcGN0IDAuMzUNCnR0dF90cmFpdG9yX21heCAzMg0KdHR0X2RldGVjdGl2ZV9wY3QgMC4xDQp0dHRfZGV0ZWN0aXZlX21heCAzMg0KdHR0X2RldGVjdGl2ZV9taW5fcGxheWVycyA4DQp0dHRfZGV0ZWN0aXZlX2thcm1hX21pbiA4MDANCg0KDQovL0thcm1hDQp0dHRfa2FybWEgMQ0KdHR0X2thcm1hX3N0cmljdCAxDQp0dHRfa2FybWFfc3RhcnRpbmcgMTAwMA0KdHR0X2thcm1hX21heCAxMDAwDQp0dHRfa2FybWFfcmF0aW8gMC4wMDENCnR0dF9rYXJtYV9raWxsX3BlbmFsdHkgMTUNCnR0dF9rYXJtYV9yb3VuZF9pbmNyZW1lbnQgNQ0KdHR0X2thcm1hX2NsZWFuX2JvbnVzIDMwDQp0dHRfa2FybWFfdHJhaXRvcmRtZ19yYXRpbyAwLjAwMDMNCnR0dF9rYXJtYV90cmFpdG9ya2lsbF9ib251cyA0MA0KdHR0X2thcm1hX2xvd19hdXRva2ljayAxDQp0dHRfa2FybWFfbG93X2Ftb3VudCAxMDANCnR0dF9rYXJtYV9sb3dfYmFuIDANCnR0dF9rYXJtYV9sb3dfYmFuX21pbnV0ZXMgNjANCnR0dF9rYXJtYV9wZXJzaXN0IDENCnR0dF9rYXJtYV9jbGVhbl9oYWxmIDAuMjUNCg0KLy9PdGhlcg0KdHR0X3Bvc3Ryb3VuZF9kbSAxDQp0dHRfbm9fbmFkZV90aHJvd19kdXJpbmdfcHJlcCAwDQp0dHRfd2VhcG9uX2NhcnJ5aW5nIDENCnR0dF93ZWFwb25fY2FycnlpbmdfcmFuZ2UgNTANCnR0dF90ZWxlcG9ydF90ZWxlZnJhZ3MgMQ0KdHR0X3JhZ2RvbGxfcGlubmluZyAxDQp0dHRfcmFnZG9sbF9waW5uaW5nX2lubm9jZW50cyAxDQp0dHRfdXNlX3dlYXBvbl9zcGF3bl9zY3JpcHRzIDENCnR0dF9zcGF3bl93YXZlX2ludGVydmFsIDINCg0KLy8gc2VydmVyIGxvZ2dpbmcNCmxvZyBvbg0Kc3ZfbG9nYmFucyAxDQpzdl9sb2dlY2hvIDENCnN2X2xvZ2ZpbGUgMQ0Kc3ZfbG9nX29uZWZpbGUgMA0KDQovLyBvcGVyYXRpb24NCnN2X2xhbiAwDQpzdl9yZWdpb24gMyAvL0V1cm9wYQ0KDQoNCmV4ZWMgYmFubmVkX3VzZXIuY2ZnDQpleGVjIGJhbm5lZF9pcC5jZmcgDQoNCg==")
        Dim fs As New FileStream(filePath, FileMode.Create)
        fs.Write(baseFile, 0, baseFile.Length)
        fs.Close()
    End Sub



    Private Sub frmProgramma_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim dialog As DialogResult
        If Button1.Enabled Then
            dialog = MessageBox.Show("Do you want to save changes?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        Else
            dialog = MessageBox.Show("Are you sure that you want to exit?", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
        End If

        If dialog = DialogResult.No Or dialog = DialogResult.OK Then
        ElseIf dialog = DialogResult.Cancel Then
            e.Cancel = True
        ElseIf dialog = DialogResult.Yes Then
            Button1_Click(sender, e, True)
        End If
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("steam://openurl/https://steamcommunity.com/sharedfiles/filedetails/?id=1809068913")
    End Sub


    'So würde man abfragen, ob die Maus Panel 1 betritt
    'Private Sub panel1_MouseLeave(sender As Object, e As System.EventArgs) Handles Panel1.MouseEnter

    'End Sub

    'so würde man abfragen ob die Maus das fenster betritt
    'Private Sub panel1_MouseLeave(sender As Object, e As System.EventArgs) Handles Me.MouseEnter

    'End Sub


End Class
