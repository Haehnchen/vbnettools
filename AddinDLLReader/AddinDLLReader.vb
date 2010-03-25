﻿'$Id$
'$HeadURL$

Imports System.Reflection
''' <summary>
''' A class that can dynamically include a remote .NET Framework dll
''' and call some functions at application runtime
''' 
''' You must known the Namespace of the remote file to include it.
'''
''' ExampleDLL.dll gives you ´ExampleDLL.TheClass´
'''  - ExampleDLL will autogenerated out of the filename
'''  - You only had to give the correct ClassName (TheClass)
''' </summary>
''' <remarks></remarks>
Class AddinDLLReader
    Private _aAssembly As [Assembly]
    Private _aClass As Type
    Private _aAddin As Object
    Private _File As System.IO.FileInfo
    Private _AssemblyName As String

    ''' <summary>
    ''' </summary>
    ''' <param name="FullDLLAddinPath">The path to the DLLFile</param>
    ''' <param name="ClassName">The Classname of the remote dll: ´ExampleDLL.TheClass´ = TheClass</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal FullDLLAddinPath As String, ByVal ClassName As String)
        _File = New System.IO.FileInfo(FullDLLAddinPath)
        _AssemblyName = Replace(_File.Name, _File.Extension, "")

        'the magic stuff: include and read a external dll 
        _aAssembly = Reflection.Assembly.LoadFile(_File.FullName)
        _aClass = _aAssembly.GetType(_AssemblyName & "." & ClassName)
        _aAddin = Activator.CreateInstance(_aClass)

    End Sub
    ''' <summary>
    ''' Returns a ArrayList with all Methodes of the class
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMethodes() As ArrayList
        Dim bArray As New ArrayList
        For Each k In _aClass.GetMethods()

            'hide unwanted: ToString, Equals, GetHashCode, GetType
            If k.IsHideBySig = False Then
                bArray.Add(k.Name)
            End If
        Next
        Return bArray
    End Function

    ''' <summary>
    ''' Run a given Method (eg. Function / Sub). You can pass any paramter as a object: New Object() {"MyParamter"}
    ''' </summary>
    ''' <param name="Method">Function / Sub to call</param>
    ''' <param name="Paramter">A object containing the paramter</param>
    ''' <returns>return value of the called function as object</returns>
    ''' <remarks></remarks>
    Function RunMethod(ByVal Method As String, Optional ByVal Paramter() As Object = Nothing) As Object
        'New Object() {"1111"}
        Dim aMethode As MethodInfo = _aClass.GetMethod(Method)
        Return aMethode.Invoke(_aAddin, Paramter)
    End Function
    ''' <summary>
    ''' Returns the filename of the DLL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DLLName() As String
        Get
            Return _File.Name
        End Get
    End Property
    ''' <summary>
    ''' Returns the FullPath of the DLL that were given by New()
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property FullPath() As String
        Get
            Return _File.FullName
        End Get
    End Property
    ''' <summary>
    ''' Returns the AssemblyName that was automatically generated
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property AssemblyName() As String
        Get
            Return _AssemblyName
        End Get
    End Property
    ''' <summary>
    ''' Example: Calls the function Version of the dll class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Version() As String
        Get
            Return RunMethod("Version").ToString
        End Get
    End Property
    ''' <summary>
    ''' Example: Calls the function Description of the dll class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Description() As String
        Get
            Return RunMethod("Description").ToString
        End Get
    End Property
End Class