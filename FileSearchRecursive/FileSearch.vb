'Option Strict On
'Option Explicit On
Imports System.IO
Imports System.Collections.Specialized
''' <summary>
''' Recursive Scan a Directory for Files with possibility to use File/Directory Masks
''' 
''' http://www.vbforums.com/showthread.php?t=341919
''' </summary>
''' <remarks></remarks>
Public Class FileSearch
    Private Const DefaultFileMask As String = "*.*"
    Private Const DefaultDirectoryMask As String = "*"
#Region " Member Variables "
    Private _InitialDirectory As DirectoryInfo
    Private _DirectoryMasks As StringCollection
    Private _FileMasks As StringCollection
    Private _Directories As New ArrayList
    Private _Files As New ArrayList
#End Region
#Region " Properites "

    Public Property InitialDirectory() As DirectoryInfo
        Get
            Return _InitialDirectory
        End Get
        Set(ByVal Value As DirectoryInfo)
            _InitialDirectory = Value
        End Set
    End Property

    Public Property DirectoryMask() As StringCollection
        Get
            Return _DirectoryMasks
        End Get
        Set(ByVal Value As StringCollection)
            _DirectoryMasks = Value
        End Set
    End Property
    ''' <summary>
    ''' FileMask to filter Files
    ''' </summary>
    ''' <value>*.zip or *.zip;*.jpg;*.bmp or </value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FileMask() As StringCollection
        Get
            Return _FileMasks
        End Get
        Set(ByVal Value As StringCollection)
            _FileMasks = Value
        End Set
    End Property
    ''' <summary>
    ''' List all Directories that has been found; use Search() first
    ''' </summary>
    ''' <value></value>
    ''' <returns>ArrayList with Directories</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Directories() As ArrayList
        Get
            Return _Directories
        End Get
    End Property
    ''' <summary>
    ''' List all Files that has been found; use Search() first
    ''' </summary>
    ''' <value>ArrayList with Files</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Files() As ArrayList
        Get
            Return _Files
        End Get
    End Property
#End Region
#Region " Constructors "
    Public Sub New()

    End Sub

    Public Sub New( _
        ByVal BaseDirectory As String)
        Me.New(New DirectoryInfo(BaseDirectory))
    End Sub
    Public Sub New( _
        ByVal InitialDirectory As DirectoryInfo)
        _InitialDirectory = InitialDirectory
    End Sub
#End Region

    ''' <summary>
    ''' Perfomer Recursive Search
    ''' </summary>
    ''' <param name="InitalDirectory">Parent directory</param>
    ''' <param name="FileMask">FileMask to filter Files: "*.zip" or "*.zip;*.jpg;*.bmp"</param>
    ''' <param name="DirectoryMask">Makes for Files</param>
    ''' <remarks></remarks>
    Public Overloads Sub Search(ByVal InitalDirectory As String, Optional ByVal FileMask As String = Nothing, Optional ByVal DirectoryMask As String = Nothing)
        Search(New DirectoryInfo(InitalDirectory), FileMask, DirectoryMask)
    End Sub
    Public Overloads Sub Search( _
         Optional ByVal InitalDirectory As DirectoryInfo = Nothing, _
         Optional ByVal FileMask As String = Nothing, _
         Optional ByVal DirectoryMask As String = Nothing)
        _Files = New ArrayList
        _Directories = New ArrayList
        If Not IsNothing(InitalDirectory) Then
            _InitialDirectory = InitalDirectory
        End If
        If IsNothing(_InitialDirectory) Then
            Throw New ArgumentException("A Directory Must be specified!", "Directory")
        End If
        If IsNothing(FileMask) OrElse FileMask.Length = 0 Then
            _FileMasks = New StringCollection
            _FileMasks.Add(DefaultFileMask)
        Else
            _FileMasks = ParseMask(FileMask)
        End If
        If IsNothing(DirectoryMask) OrElse DirectoryMask.Length > 0 Then
            _DirectoryMasks = New StringCollection
            _DirectoryMasks.Add(DefaultDirectoryMask)
        Else
            _DirectoryMasks = ParseMask(DirectoryMask)
        End If
        DoSearch(_InitialDirectory)
    End Sub
    Private Sub DoSearch(ByVal BaseDirectory As DirectoryInfo)
        Try
            For Each fm As String In _FileMasks
                Try 'added for directory/file which are longer then 255
                    Files.AddRange(BaseDirectory.GetFiles(fm))
                Catch ex As Exception

                End Try
            Next
        Catch u As UnauthorizedAccessException

        End Try

        Try
            Dim Directories As New ArrayList
            For Each dm As String In _DirectoryMasks
                Try 'added for directory/file which are longer then 255
                    Directories.AddRange(BaseDirectory.GetDirectories(dm))
                    _Directories.AddRange(Directories)
                Catch ex As Exception

                End Try
            Next
            For Each di As DirectoryInfo In Directories
                DoSearch(di)
            Next
        Catch u As UnauthorizedAccessException
        End Try
    End Sub
    Private Shared Function ParseMask(ByVal Mask As String) As StringCollection
        If IsNothing(Mask) Then
            Return Nothing
        End If
        Mask = Mask.Trim(";"c)
        If Mask.Length = 0 Then
            Return Nothing
        End If
        Dim Masks As New StringCollection

        Masks.AddRange(Mask.Split(";"c))
        Return Masks

    End Function

    Protected Overrides Sub Finalize()
        _Files = Nothing
        _Directories = Nothing
        MyBase.Finalize()
    End Sub
End Class
