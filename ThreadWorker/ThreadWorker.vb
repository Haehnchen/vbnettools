'$Id$
''' <summary>
''' Simple template for Threads usage
''' </summary>
''' <remarks></remarks>
Public MustInherit Class ThreadWorker
    Private _CurrentPosition As Double = 0
    Private _MaxThreads As Integer = 4

    Private _ThreadsArray As New List(Of Threading.Thread)
    Private _Items As New ArrayList
    Private _ItemsCount As Integer

    Public Event Finished()
    Public Event PercentDone(ByVal Percent As Double)
    Public Event ItemFinished(ByVal CurrentPosition As Integer, ByVal Thread As Integer, ByVal ReturnString As String)
#Region "Properties"
    ''' <summary>
    ''' How many Threads do we want? Default = 4
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MaxThreads() As Integer
        Get
            Return _MaxThreads
        End Get
        Set(ByVal value As Integer)
            _MaxThreads = value
        End Set
    End Property
    ''' <summary>
    ''' A list with Object to work on
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Items() As ArrayList
        Get
            Return _Items
        End Get
        Set(ByVal value As ArrayList)
            _Items = value
            _ItemsCount = _Items.Count
        End Set
    End Property
    ''' <summary>
    ''' Get the last finished item as integer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CurrentPostion() As Integer
        Get
            Return _CurrentPosition
        End Get
    End Property
    ''' <summary>
    ''' Get overall items count 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property ItemsCount() As Integer
        Get
            Return _ItemsCount
        End Get
    End Property
#End Region
    Sub Start()
        Call StartBefore()

        'add wanted threads to array for later usage
        For i As Integer = 0 To _MaxThreads - 1
            _ThreadsArray.Add(New Threading.Thread(AddressOf WorkerThread))
        Next

        'walk through array of threads and start each one
        For i As Integer = 0 To _ThreadsArray.Count - 1
            Dim myThread As Threading.Thread = _ThreadsArray.Item(i)
            myThread.Start(i)
            System.Threading.Thread.Sleep(10)
        Next

        'a simple thread the will raise the Finished() event if all done
        Dim FishinedThread As New Threading.Thread(AddressOf WaitForExitThreadEvent)
        FishinedThread.Start()

    End Sub
    ''' <summary>
    ''' return true if all threads are finished
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function HasExited() As Boolean
        Dim back As Boolean = True
        For Each myThread As Threading.Thread In _ThreadsArray
            If myThread.IsAlive = True Then
                Return False
            End If
        Next
        Return back
    End Function
    ''' <summary>
    ''' wait for all threads to be finished in parent thread
    ''' </summary>
    ''' <remarks></remarks>
    Sub WaitForExit()
        Do While Me.HasExited = False
            System.Threading.Thread.Sleep(500)
        Loop
    End Sub
    ''' <summary>
    ''' Helper for WaitForExit()
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WaitForExitThreadEvent()
        Call WaitForExit()
        RaiseEvent Finished()
    End Sub
    ''' <summary>
    ''' Sub started by threads and walk through array list
    ''' </summary>
    ''' <param name="Thread"></param>
    ''' <remarks></remarks>
    Private Sub WorkerThread(ByVal Thread As Integer)
        Do While _CurrentPosition < _ItemsCount
            'how much is done all ready in percent
            RaiseEvent PercentDone(Math.Round(_CurrentPosition / _ItemsCount * 100, 1))

            'for thread safety: before do something with this variable set new value 
            _CurrentPosition += 1

            'Calls WorkItem return text and raise the event
            RaiseEvent ItemFinished(_CurrentPosition - 1, Thread, WorkItem(_CurrentPosition - 1, Thread))
        Loop
    End Sub
    ''' <summary>
    ''' This function does working stuff; you must override it
    ''' </summary>
    ''' <param name="CurrentPosition">The Current Item as Integer</param>
    ''' <param name="Thread">The Thread which did the work</param>
    ''' <returns>a return string value that can use in ItemFinished</returns>
    ''' <remarks></remarks>
    MustOverride Function WorkItem(ByVal CurrentPosition As Integer, ByVal Thread As Integer) As String

    ''' <summary>
    ''' Optional: Function that is perfomed before we Start() our threads
    ''' </summary>
    ''' <remarks></remarks>
    Overridable Sub StartBefore()
        'nothing to do
    End Sub
End Class
