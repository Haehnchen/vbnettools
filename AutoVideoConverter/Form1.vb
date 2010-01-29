Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1
#Region "Declarations"
    Dim BaseDir As String = System.Environment.CurrentDirectory
    Dim MEncoderEncodingStr As String = """%in%"" %scale% -profile %profil% -o ""%out%"""
    Dim MPlayerVideoInfoStr As String = "-vo null -ao null -frames 0 -identify ""%in%"""
    Structure sVideoSize
        Dim Width As Integer
        Dim Height As Integer
    End Structure
#End Region
#Region "Form Functions"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim NeededFiles() As String = {BaseDir & "\mplayer\mencoder.conf", BaseDir & "\mencoder.exe", BaseDir & "\mplayer.exe"}
        For Each NeededFile As String In NeededFiles
            If Not File.Exists(NeededFile) Then MsgBox(NeededFile & " not found", MsgBoxStyle.Critical) : End
        Next

        ReadMEncoderProfiles()

        chksound.Checked = My.Settings.SoundDisabled
        cmbprofil.SelectedIndex = cmbprofil.Items.IndexOf(My.Settings.DefaultProfil)
        cmbzoom.SelectedIndex = cmbzoom.Items.IndexOf(My.Settings.DefaultPercent)
        txtautosize.Text = My.Settings.MaxPixel

        If cmbprofil.SelectedIndex < 0 And cmbprofil.Items.Count > 1 Then cmbprofil.SelectedIndex = 0
        If cmbzoom.SelectedIndex < 0 And cmbzoom.Items.Count > 1 Then cmbzoom.SelectedIndex = 0
    End Sub
    Private Sub butOpenFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butOpenFiles.Click
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            For Each FileName As String In OpenFileDialog1.FileNames
                StartEncoding(FileName, cmbzoom.Text, chksound.Checked)
            Next
        End If
    End Sub
    Private Sub chksound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chksound.CheckedChanged, ckDebug.CheckedChanged
        My.Settings.SoundDisabled = chksound.Checked
    End Sub
    Private Sub cmbprofil_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbprofil.SelectedIndexChanged
        My.Settings.DefaultProfil = cmbprofil.Text
    End Sub
    Private Sub cmbzoom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbzoom.SelectedIndexChanged
        My.Settings.DefaultPercent = cmbzoom.Text
    End Sub
    Private Sub tbFiles_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tbFiles.DragDrop
        Dim s As Array = CType(e.Data.GetData(DataFormats.FileDrop), Array)
        For Each FileName As String In s
            StartEncoding(FileName, cmbzoom.Text, chksound.Checked)
        Next
    End Sub
    Private Sub txtautosize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtautosize.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub
    Private Sub tbFiles_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tbFiles.DragEnter
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub txtautosize_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtautosize.TextChanged
        My.Settings.MaxPixel = Convert.ToInt32(txtautosize.Text)
    End Sub
    Private Sub tbInfo_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbInfo.Enter
        txtInfo.Text = My.Application.Info.AssemblyName & " Version:" & vbCrLf
        txtInfo.Text &= My.Application.Info.Version.ToString & vbCrLf
        txtInfo.Text &= vbCrLf & "mencoder/mplayer Version:" & vbCrLf
        txtInfo.Text &= ConsoleGetStd("\mencoder.exe")
    End Sub
#End Region
#Region "Functions"
    ''' <summary>
    ''' Generate mencoder command and execute it to convert the video
    ''' </summary>
    ''' <param name="VideoFilePath">Path to the source video file</param>
    ''' <param name="ScaleProcent">optional: reduce video resolution by x percent</param>
    ''' <param name="DisableSound">optional: remove sound on destination video?</param>
    ''' <remarks></remarks>
    Sub StartEncoding(ByVal VideoFilePath As String, Optional ByVal ScaleProcent As String = "auto", Optional ByVal DisableSound As Boolean = False)
        Dim Cmd As String = MEncoderEncodingStr
        Cmd = Cmd.Replace("%profil%", cmbprofil.Items(cmbprofil.SelectedIndex).ToString)

        If ScaleProcent = "auto" Then
            Dim VideoSize As sVideoSize = MPlayerGetVideosize(VideoFilePath, BaseDir & "\")
            ScaleProcent = VideoResizeProcent(VideoSize, txtautosize.Text)
            If Not ScaleProcent > 0 Then ScaleProcent = 50
        End If

        Cmd = Cmd.Replace("%scale%", "-vf scale -zoom -xy " & Convert.ToInt32(ScaleProcent) / 100)

        'Generate output filepath
        Dim DestinationFile As String = ""
        Dim SelectedProfil As String = cmbprofil.Items(cmbprofil.SelectedIndex).ToString

        'Generate a file extension depending on selected profil
        If SelectedProfil.Contains("xvid") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.avi"
        If SelectedProfil.Contains("x264") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.mp4"
        If SelectedProfil.Contains("mpeg") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.mpg"
        If SelectedProfil.Contains("flv") Or SelectedProfil.Contains("flash") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.flv"
        If SelectedProfil.Contains("f4v") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.f4v"
        If SelectedProfil.Contains("wmv") = True Then DestinationFile = VideoFilePath.Substring(0, VideoFilePath.LastIndexOf(".")) & "_neu.wmv"


        'error if profilname extension is unknown
        If DestinationFile = "" Then
            MsgBox("DestinationFile error")
            Exit Sub
        End If

        Cmd = Cmd.Replace("%in%", VideoFilePath)
        Cmd = Cmd.Replace("%out%", DestinationFile)

        'disable sound or not
        If DisableSound = True Then Cmd = Cmd & " -nosound"
        If ckDebug.Checked = True Then MsgBox(Cmd)

        Dim AppMencoder As New System.Diagnostics.Process()
        With AppMencoder
            .StartInfo.FileName = BaseDir & "\mencoder.exe"
            .StartInfo.Arguments = Cmd

            If ckDebug.Checked = False Then
                .Start()
            Else
                .StartInfo.RedirectStandardInput = True
                .StartInfo.RedirectStandardOutput = True
                .StartInfo.RedirectStandardError = True
                .StartInfo.UseShellExecute = False
                .StartInfo.CreateNoWindow = True

                .Start()
                .WaitForExit()

                Dim StdOut As System.IO.StreamReader = .StandardOutput
                Dim source As String = StdOut.ReadToEnd
                MsgBox(source)

            End If
        End With

        'old save dialog
        'If txtfold.Text.Length > 0 Then
        '    If Not cmbprofil.Items(cmbprofil.SelectedIndex).ToString.IndexOf("mpeg") Then Cmd = Cmd.Replace("%out%", txtfold.Text & ".mpg")
        '    If Not cmbprofil.Items(cmbprofil.SelectedIndex).ToString.IndexOf("xvid") Then Cmd = Cmd.Replace("%out%", txtfold.Text & ".avi")
        'Else
        '    Cmd = Cmd.Replace("%out%", VideoFilePath)
        'End If

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="cmd"></param>
    ''' <returns></returns>
    Function ConsoleGetStd(ByVal FileName As String, Optional ByVal cmd As String = "")
        Dim AppMencoder As New System.Diagnostics.Process()
        With AppMencoder
            .StartInfo.FileName = BaseDir & "\mencoder.exe"
            .StartInfo.Arguments = cmd

            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True

            .Start()
            .WaitForExit()

            Dim StdOut As System.IO.StreamReader = .StandardOutput
            Dim source As String = StdOut.ReadToEnd
            Return source
        End With
    End Function
    ''' <summary>
    ''' Calculate a procent value on which the video must be reduce to match
    ''' then max pixelsize
    ''' </summary>
    ''' <param name="VideoSize">sVideoSize Object</param>
    ''' <param name="MaxPixel">maximal Pixel size. use Width x Height to calc the wanted video resolution</param>
    ''' <returns>returns procent value on which the video resolution must be reduce; returns 100 if no resize is necessary</returns>
    Function VideoResizeProcent(ByVal VideoSize As sVideoSize, Optional ByVal MaxPixel As Integer = 300000) As Integer
        If VideoSize.Width > 0 And VideoSize.Width > 0 Then
            Dim Pixels As Integer = VideoSize.Width * VideoSize.Height
            If Pixels > MaxPixel Then
                'calculate procent value to resize and round values to 95,90,85,...
                Return Math.Round((MaxPixel / Pixels * 100) / 5) * 5
            Else
                'the source video is smaller than the wanted size
                Return 100
            End If
        End If
        Return 0
    End Function
    ''' <summary>
    ''' Parse available convert profiles out of mencoder.conf
    ''' </summary>
    Sub ReadMEncoderProfiles()
        Dim objReader As New StreamReader("mplayer\mencoder.conf")
        Dim source As String = objReader.ReadToEnd()
        objReader.Close()

        Dim mc As MatchCollection
        mc = Regex.Matches(source, "\[(.*?)\]")

        For i As Integer = 0 To mc.Count - 1
            cmbprofil.Items.Add(mc(i).Groups(1).Value)
        Next

    End Sub
    ''' <summary>
    ''' Get the width and height of a video with the help of mplayer.exe
    ''' </summary>
    ''' <param name="VideoFile">Fullpath to videofile</param>
    ''' <param name="MPlayerDir">optional the path of mplayer.exe</param>
    ''' <returns>sVideoSize Object</returns>
    Function MPlayerGetVideosize(ByVal VideoFile As String, Optional ByVal MPlayerDir As String = "") As sVideoSize

        Dim MPlayerApp As New System.Diagnostics.Process()
        With MPlayerApp
            .StartInfo.FileName = MPlayerDir & "mplayer.exe"
            .StartInfo.Arguments = MPlayerVideoInfoStr.Replace("%in%", VideoFile)
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True

            .Start()
            .WaitForExit()
        End With

        Dim StdOut As System.IO.StreamReader = MPlayerApp.StandardOutput
        Dim source As String = StdOut.ReadToEnd

        Dim b As New sVideoSize
        b.Width = Regex.Match(source, "ID_VIDEO_WIDTH=(.*?)\n").Groups(1).ToString()
        b.Height = Regex.Match(source, "ID_VIDEO_HEIGHT=(.*?)\n").Groups(1).ToString()
        Return b

        'to extract all video info
        'Dim mc As MatchCollection
        'mc = Regex.Matches(source, "ID_(.*?)=(.*?)\n")

        'For i As Integer = 0 To mc.Count - 1
        '    MsgBox(mc(i).Groups(2).Value)
        'Next
    End Function
#End Region

End Class

