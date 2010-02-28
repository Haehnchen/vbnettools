Imports SharpPcap
Imports SharpPcap.Packets
Imports System.Net

''' <summary>
''' The extended Version of HamachiBroadcastFix with GUI and Auto Device Selector.
''' it will send network broadcast message from one network device to another
''' Background:
''' since windows 7 a broadcast message will only send to the first network device out of the
''' routing table which orders by the metric value under the TCP/IP options.
''' 
''' thx to Zolid / Daniel Vinberg for console version
''' </summary>
''' <remarks></remarks>
Public Class HamachiBroadcastFixGUI
    Dim WithEvents indevice As SharpPcap.LivePcapDevice
    Private Shared _ipAddress As String
    Private Shared outdevice As SharpPcap.LivePcapDevice
    Private Shared alldevices As SharpPcap.LivePcapDeviceList
    Public Delegate Sub Logger(ByVal LogStr As String)

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text &= " - " & My.Application.Info.Version.ToString
        alldevices = LivePcapDeviceList.Instance

        BuildForm()
        If cbInput.SelectedIndex >= 0 And cbOutput.SelectedIndex >= 0 Then start()
    End Sub

    Sub BuildForm()
        For Each device In alldevices

            Dim DevIP As String = GetIP(device.Addresses)

            'input device
            cbInput.Items.Add(device.Description & " - " & DevIP)
            If device.Addresses.Count > 0 Then
                If IsLocalIpAddress(DevIP) = True Then
                    cbInput.SelectedIndex = cbInput.Items.Count - 1
                End If
            End If

            'output device
            cbOutput.Items.Add(device.Description & " - " & DevIP)
            If device.Addresses.Count > 0 Then
                If device.Description.ToLower.Contains("hamachi") Or DevIP.StartsWith("5.") Or _
                   device.Description.ToLower.Contains("tunngle") Or DevIP.StartsWith("7.") Then
                    cbOutput.SelectedIndex = cbOutput.Items.Count - 1
                End If
            End If

        Next

        'load settings
        If Not My.Settings.outdeviceText = "" Then
            If cbOutput.Items.Contains(My.Settings.outdeviceText) Then
                cbOutput.SelectedIndex = cbOutput.Items.IndexOf(My.Settings.outdeviceText)
            End If
        End If

        If Not My.Settings.indeviceText = "" Then
            If cbInput.Items.Contains(My.Settings.indeviceText) Then
                cbInput.SelectedIndex = cbInput.Items.IndexOf(My.Settings.indeviceText)
            End If
        End If
    End Sub

    Private Sub indevice_OnPacketArrival(ByVal sender As Object, ByVal e As SharpPcap.CaptureEventArgs) Handles indevice.OnPacketArrival
        If TypeOf e.Packet Is UDPPacket Then
            Dim p As UDPPacket = DirectCast(e.Packet, UDPPacket)
            If (p.DestinationAddress.ToString.CompareTo("255.255.255.255") = 0) Then
                p.SourceAddress = System.Net.IPAddress.Parse(_ipAddress)
                p.ComputeUDPChecksum()
                p.ComputeIPChecksum()
                'Dim [date] As DateTime = p.PcapHeader.Date
                'Dim packetLength As UInt32 = p.PcapHeader.PacketLength
                outdevice.Open()
                Try
                    outdevice.SendPacket(p)
                    InvokeHelper(GetIP(indevice.Addresses) & " -> " & _ipAddress & " Broadcast packet forwarded successfully.")
                Catch ex As Exception
                    InvokeHelper("-- " & ex.Message)
                    Debug.WriteLine(ex.Message)
                End Try
                outdevice.Close()

            End If
        End If
    End Sub
#Region "CustomFunctions"
    Function GetIP(ByVal Device As System.Collections.ObjectModel.ReadOnlyCollection(Of SharpPcap.PcapAddress))
        For Each Ip In Device
            'we only want ipv4 addresses
            If Ip.Addr.ipAddress.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                Return Ip.Addr.ipAddress.ToString
            End If
        Next
        Return "0.0.0.0"
    End Function
    Sub AddLogText(ByVal str As String)
        ListBox1.Items.Add(Date.Now.ToString("H:mm") & ": " & str)
        If ListBox1.Items.Count > 5 Then ListBox1.Items.RemoveAt(0)
    End Sub
    Sub InvokeHelper(ByVal str As String)
        'we dont wont kill our app if invoke fails
        Try
            Invoke(New Logger(AddressOf AddLogText), str)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub
    Sub Start()
        Try
            If cbInput.SelectedIndex < 0 Then
                MsgBox("Select Input Device")
                Exit Sub
            End If

            If cbOutput.SelectedIndex < 0 Then
                MsgBox("Select Output Device")
                Exit Sub
            End If

            'get the right device
            outdevice = alldevices.Item(cbOutput.SelectedIndex)
            indevice = alldevices.Item(cbInput.SelectedIndex)
            _ipAddress = GetIP(outdevice.Addresses)

            'settings
            My.Settings.outdeviceText = cbOutput.Items(cbOutput.SelectedIndex)
            My.Settings.indeviceText = cbInput.Items(cbInput.SelectedIndex)
            My.Settings.Save()


            AddHandler indevice.OnPacketArrival, New PacketArrivalEventHandler(AddressOf indevice_OnPacketArrival)
            AddLogText("Listening on " & GetIP(indevice.Addresses) & " sending to " & _ipAddress)

            'start capturing
            indevice.Open()
            indevice.StartCapture()

            'Form handling
            tssColorStatus.BackColor = Color.Green
            tssRunning.Text = "Running..."
            cmdStart.Enabled = False
            cmdStop.Enabled = True

        Catch ex As Exception
            InvokeHelper("-- " & ex.Message)
            Debug.WriteLine(ex.Message)
        End Try
    End Sub
    Sub Stopit()
        tssRunning.Text = "Stopped"
        tssColorStatus.BackColor = Color.Red
        cmdStop.Enabled = False
        cmdStart.Enabled = True

        If Not indevice Is Nothing Then
            Try
                AddLogText("Stopped")
                indevice.StopCapture()
                indevice.Close()
                indevice = Nothing
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
        End If
    End Sub
    ''' <summary>
    ''' The following method checks if a given host name or IP address is local. First, it gets
    ''' all IP addresses of the given host, then it gets all IP addresses of the local computer
    ''' and finally it compares both lists. If any host IP equals to any of local IPs, the host
    ''' is a local IP. It also checks whether the host is a loopback address (localhost / 127.0.0.1).
    ''' 
    ''' http://www.csharp-examples.net/local-ip/
    ''' </summary>
    ''' <param name="host">IP to check</param>
    ''' <returns>true on local addresses</returns>
    ''' <remarks></remarks>
    Public Shared Function IsLocalIpAddress(ByVal host As String) As Boolean
        Try
            ' get host IP addresses
            Dim hostIPs As IPAddress() = Dns.GetHostAddresses(host)
            ' get local IP addresses
            Dim localIPs As IPAddress() = Dns.GetHostAddresses(Dns.GetHostName())

            ' test if any host IP equals to any local IP or to localhost
            For Each hostIP As IPAddress In hostIPs
                ' is localhost
                If IPAddress.IsLoopback(hostIP) Then
                    Return True
                End If
                ' is local address
                For Each localIP As IPAddress In localIPs
                    If hostIP.Equals(localIP) Then
                        Return True
                    End If
                Next
            Next
        Catch ex As Exception
        End Try
        Return False
    End Function
#End Region
    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        start()
    End Sub
    Private Sub cmdStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        Stopit()
    End Sub
    Private Sub cbOutput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOutput.SelectedIndexChanged
        Stopit()
    End Sub
    Private Sub cbInput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbInput.SelectedIndexChanged
        Stopit()
    End Sub
    Private Sub HamachiBroadcastFixGUI_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub
    Private Sub HamachiBroadcastFixGUI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Stopit()
    End Sub
    Private Sub tssLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tssLink.Click
        System.Diagnostics.Process.Start("http://www.espend.de/node/127/?version=" & My.Application.Info.Version.ToString)
    End Sub
End Class
