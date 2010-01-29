<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.chksound = New System.Windows.Forms.CheckBox
        Me.cmbprofil = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbzoom = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.butOpenFiles = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tbFiles = New System.Windows.Forms.TabPage
        Me.tbConvert = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtautosize = New System.Windows.Forms.TextBox
        Me.lblautosize = New System.Windows.Forms.Label
        Me.ckDebug = New System.Windows.Forms.CheckBox
        Me.tbInfo = New System.Windows.Forms.TabPage
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtInfo = New System.Windows.Forms.TextBox
        Me.TabControl1.SuspendLayout()
        Me.tbFiles.SuspendLayout()
        Me.tbConvert.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tbInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'chksound
        '
        Me.chksound.AutoSize = True
        Me.chksound.Location = New System.Drawing.Point(8, 60)
        Me.chksound.Name = "chksound"
        Me.chksound.Size = New System.Drawing.Size(105, 17)
        Me.chksound.TabIndex = 1
        Me.chksound.Text = "Sound entfernen"
        Me.chksound.UseVisualStyleBackColor = True
        '
        'cmbprofil
        '
        Me.cmbprofil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbprofil.FormattingEnabled = True
        Me.cmbprofil.Location = New System.Drawing.Point(6, 6)
        Me.cmbprofil.Name = "cmbprofil"
        Me.cmbprofil.Size = New System.Drawing.Size(107, 21)
        Me.cmbprofil.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(119, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Profil"
        '
        'cmbzoom
        '
        Me.cmbzoom.FormattingEnabled = True
        Me.cmbzoom.Items.AddRange(New Object() {"auto", "25", "30", "40", "50", "60", "75", "80", "90", "100"})
        Me.cmbzoom.Location = New System.Drawing.Point(6, 33)
        Me.cmbzoom.Name = "cmbzoom"
        Me.cmbzoom.Size = New System.Drawing.Size(107, 21)
        Me.cmbzoom.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(119, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Größe in Prozent"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        '
        'butOpenFiles
        '
        Me.butOpenFiles.Location = New System.Drawing.Point(76, 65)
        Me.butOpenFiles.Name = "butOpenFiles"
        Me.butOpenFiles.Size = New System.Drawing.Size(141, 22)
        Me.butOpenFiles.TabIndex = 7
        Me.butOpenFiles.Text = "Hinzufügen und starten"
        Me.butOpenFiles.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(114, 145)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(178, 12)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Drag & Drop aus Windows-Explorer möglich"
        '
        'TabControl1
        '
        Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabControl1.Controls.Add(Me.tbFiles)
        Me.TabControl1.Controls.Add(Me.tbConvert)
        Me.TabControl1.Controls.Add(Me.tbInfo)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(309, 192)
        Me.TabControl1.TabIndex = 13
        '
        'tbFiles
        '
        Me.tbFiles.AllowDrop = True
        Me.tbFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbFiles.Controls.Add(Me.butOpenFiles)
        Me.tbFiles.Controls.Add(Me.Label3)
        Me.tbFiles.Location = New System.Drawing.Point(4, 25)
        Me.tbFiles.Name = "tbFiles"
        Me.tbFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tbFiles.Size = New System.Drawing.Size(301, 163)
        Me.tbFiles.TabIndex = 0
        Me.tbFiles.Text = "Videodatei/en"
        Me.tbFiles.UseVisualStyleBackColor = True
        '
        'tbConvert
        '
        Me.tbConvert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbConvert.Controls.Add(Me.GroupBox1)
        Me.tbConvert.Controls.Add(Me.Label2)
        Me.tbConvert.Controls.Add(Me.cmbprofil)
        Me.tbConvert.Controls.Add(Me.chksound)
        Me.tbConvert.Controls.Add(Me.cmbzoom)
        Me.tbConvert.Controls.Add(Me.Label1)
        Me.tbConvert.Location = New System.Drawing.Point(4, 25)
        Me.tbConvert.Name = "tbConvert"
        Me.tbConvert.Padding = New System.Windows.Forms.Padding(3)
        Me.tbConvert.Size = New System.Drawing.Size(301, 163)
        Me.tbConvert.TabIndex = 1
        Me.tbConvert.Text = "Convert-Settings"
        Me.tbConvert.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtautosize)
        Me.GroupBox1.Controls.Add(Me.lblautosize)
        Me.GroupBox1.Controls.Add(Me.ckDebug)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 84)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(288, 70)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Experten"
        '
        'txtautosize
        '
        Me.txtautosize.Location = New System.Drawing.Point(6, 19)
        Me.txtautosize.Name = "txtautosize"
        Me.txtautosize.Size = New System.Drawing.Size(107, 20)
        Me.txtautosize.TabIndex = 6
        '
        'lblautosize
        '
        Me.lblautosize.AutoSize = True
        Me.lblautosize.Location = New System.Drawing.Point(114, 22)
        Me.lblautosize.Name = "lblautosize"
        Me.lblautosize.Size = New System.Drawing.Size(149, 13)
        Me.lblautosize.TabIndex = 5
        Me.lblautosize.Text = "Maximale Pixel (Breite x Höhe)"
        '
        'ckDebug
        '
        Me.ckDebug.AutoSize = True
        Me.ckDebug.Location = New System.Drawing.Point(6, 45)
        Me.ckDebug.Name = "ckDebug"
        Me.ckDebug.Size = New System.Drawing.Size(58, 17)
        Me.ckDebug.TabIndex = 1
        Me.ckDebug.Text = "Debug"
        Me.ckDebug.UseVisualStyleBackColor = True
        '
        'tbInfo
        '
        Me.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbInfo.Controls.Add(Me.txtInfo)
        Me.tbInfo.Controls.Add(Me.Label4)
        Me.tbInfo.Location = New System.Drawing.Point(4, 25)
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(301, 163)
        Me.tbInfo.TabIndex = 2
        Me.tbInfo.Text = "Info"
        Me.tbInfo.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(168, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 12)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Daniel Espendiller - espend.de"
        '
        'txtInfo
        '
        Me.txtInfo.BackColor = System.Drawing.SystemColors.Control
        Me.txtInfo.Location = New System.Drawing.Point(3, 3)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfo.Size = New System.Drawing.Size(289, 138)
        Me.txtInfo.TabIndex = 10
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(309, 192)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AutoVideoConverter"
        Me.TabControl1.ResumeLayout(False)
        Me.tbFiles.ResumeLayout(False)
        Me.tbFiles.PerformLayout()
        Me.tbConvert.ResumeLayout(False)
        Me.tbConvert.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tbInfo.ResumeLayout(False)
        Me.tbInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chksound As System.Windows.Forms.CheckBox
    Friend WithEvents cmbprofil As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbzoom As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents butOpenFiles As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbFiles As System.Windows.Forms.TabPage
    Friend WithEvents tbConvert As System.Windows.Forms.TabPage
    Friend WithEvents ckDebug As System.Windows.Forms.CheckBox
    Friend WithEvents txtautosize As System.Windows.Forms.TextBox
    Friend WithEvents lblautosize As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbInfo As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox

End Class
