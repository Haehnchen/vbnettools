<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HamachiBroadcastFixGUI
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HamachiBroadcastFixGUI))
        Me.gpInput = New System.Windows.Forms.GroupBox
        Me.cbInput = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbOutput = New System.Windows.Forms.ComboBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cmdStart = New System.Windows.Forms.Button
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.tssColorStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssRunning = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tssLink = New System.Windows.Forms.ToolStripStatusLabel
        Me.cmdStop = New System.Windows.Forms.Button
        Me.gpInput.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gpInput
        '
        Me.gpInput.Controls.Add(Me.cbInput)
        Me.gpInput.Location = New System.Drawing.Point(12, 12)
        Me.gpInput.Name = "gpInput"
        Me.gpInput.Size = New System.Drawing.Size(453, 58)
        Me.gpInput.TabIndex = 0
        Me.gpInput.TabStop = False
        Me.gpInput.Text = "Input Device (LAN-Adapter)"
        '
        'cbInput
        '
        Me.cbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbInput.FormattingEnabled = True
        Me.cbInput.Location = New System.Drawing.Point(18, 19)
        Me.cbInput.Name = "cbInput"
        Me.cbInput.Size = New System.Drawing.Size(406, 21)
        Me.cbInput.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbOutput)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 76)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(453, 54)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Output Device (VPN-Adapter eg. Hamachi/Tunngle)"
        '
        'cbOutput
        '
        Me.cbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbOutput.FormattingEnabled = True
        Me.cbOutput.Location = New System.Drawing.Point(18, 19)
        Me.cbOutput.Name = "cbOutput"
        Me.cbOutput.Size = New System.Drawing.Size(406, 21)
        Me.cbOutput.TabIndex = 0
        '
        'ListBox1
        '
        Me.ListBox1.Enabled = False
        Me.ListBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.HorizontalScrollbar = True
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(18, 19)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(406, 64)
        Me.ListBox1.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ListBox1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 136)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(453, 96)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Log"
        '
        'cmdStart
        '
        Me.cmdStart.Location = New System.Drawing.Point(391, 238)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(76, 26)
        Me.cmdStart.TabIndex = 3
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tssColorStatus, Me.tssRunning, Me.ToolStripStatusLabel2, Me.tssLink})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 277)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(479, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tssColorStatus
        '
        Me.tssColorStatus.Name = "tssColorStatus"
        Me.tssColorStatus.Size = New System.Drawing.Size(16, 17)
        Me.tssColorStatus.Text = "   "
        '
        'tssRunning
        '
        Me.tssRunning.Name = "tssRunning"
        Me.tssRunning.Size = New System.Drawing.Size(66, 17)
        Me.tssRunning.Text = "tssRunning"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(321, 17)
        Me.ToolStripStatusLabel2.Spring = True
        '
        'tssLink
        '
        Me.tssLink.IsLink = True
        Me.tssLink.Name = "tssLink"
        Me.tssLink.Size = New System.Drawing.Size(61, 17)
        Me.tssLink.Text = "espend.de"
        '
        'cmdStop
        '
        Me.cmdStop.Location = New System.Drawing.Point(317, 238)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.Size = New System.Drawing.Size(68, 26)
        Me.cmdStop.TabIndex = 5
        Me.cmdStop.Text = "Stop"
        Me.cmdStop.UseVisualStyleBackColor = True
        '
        'HamachiBroadcastFixGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(479, 299)
        Me.Controls.Add(Me.cmdStop)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gpInput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "HamachiBroadcastFixGUI"
        Me.Text = "HamachiBroadcastFixGUI"
        Me.gpInput.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gpInput As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbInput As System.Windows.Forms.ComboBox
    Friend WithEvents cbOutput As System.Windows.Forms.ComboBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents cmdStop As System.Windows.Forms.Button
    Friend WithEvents tssRunning As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssColorStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tssLink As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel

End Class
