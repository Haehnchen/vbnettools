'$Id$
'$HeadURL$

Imports MySql.Data.MySqlClient

Public Class MySqlClientClass
    Private _Connection As New MySqlConnection
    Private _ConnectionInfo As ClassConnectionInfo
    Public Sub New(ByVal ConnectionInfo As ClassConnectionInfo)
        _ConnectionInfo = ConnectionInfo
    End Sub

    Function Connect()
        Try
            If _Connection.State = ConnectionState.Closed Then
                _Connection.ConnectionString = "DATABASE=" & _ConnectionInfo.Database & ";" & _
                "SERVER=" & _ConnectionInfo.Server & ";user id=" & _ConnectionInfo.User & _
                ";password=" & _ConnectionInfo.Password & _
                ";port=" & _ConnectionInfo.Port & ";charset=utf8;Allow Zero Datetime=true"
                _Connection.Open()
            End If

        Catch ex As Exception
            MsgBox("Error Connecting to the database: " & ex.Message)
            Return False
        End Try
        Return False
    End Function
    Public Sub Disconnect()
        Try
            _Connection.Close()
            _Connection.Dispose()

        Catch myerror As MySqlException

        End Try
    End Sub
    ''' <summary>
    ''' Will insert the values of the SetFields and WhereDefinition.
    ''' If we get any Duplicate Keys we update the fields out of SetFields.
    ''' </summary>
    ''' <param name="table">Database table to work with</param>
    ''' <param name="SetFields">The field values to insert</param>
    ''' <param name="WhereDefinition">Should contains any Key Field of the Table</param>
    ''' <remarks></remarks>
    Sub InsertOnDuplicateKey(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition))
        Dim oCommand As MySqlCommand = _Connection.CreateCommand

        Dim AllFields As List(Of FieldDefinition) = Me.ConcatFields(WhereDefinition, SetFields)

        oCommand.CommandText = "INSERT INTO `" & table & "` (" & Me.JoinFieldName(AllFields) & _
        ") VALUES (" & Me.JoinParameterName(AllFields) & _
        ") ON DUPLICATE KEY UPDATE " & Me.JoinValues(SetFields)

        Me.FieldsToParameters(AllFields, oCommand)

        oCommand.ExecuteNonQuery()
        oCommand.Dispose()
        oCommand = Nothing

    End Sub
    ''' <summary>
    ''' Will insert the values in the SetField with no further duplication or update check.
    ''' if we get any error (duplicate keys,...) the error will ignored and we get back the primary key of the row
    ''' </summary>
    ''' <param name="table">Database table</param>
    ''' <param name="SetField">The fields to insert</param>
    ''' <returns>The primary key of the row</returns>
    ''' <remarks></remarks>
    Function Insert(ByVal table As String, ByVal SetField As FieldDefinition) As String
        'Dim helperList As New List(Of mysql2.FieldDefinition) : helperList.Add(WhereDefinition)

        Dim SetFieldList As List(Of MySqlClientClass.FieldDefinition) = Me.SingleFieldToList(SetField)
        Dim oCommand As MySqlCommand = Me._Connection.CreateCommand

        oCommand.CommandText = "INSERT IGNORE " & table & Me.SetCommand(SetFieldList) & "; Select _rowid from " & table & " " & Me.WhereCommand(SetFieldList)

        Me.FieldsToParameters(SetFieldList, oCommand)
        '   oCommand.ExecuteNonQuery()
        Dim sqlres As String = Me.CommandToDatatable(oCommand).ToSingleString
        oCommand.Dispose()
        oCommand = Nothing
        Return sqlres
    End Function
    ''' <summary>
    ''' Delete rows that match the WhereDefinition. by security: on default only one row will be delete (LIMIT 1)
    ''' Set Limit to false if you want to delete more than one row
    ''' </summary>
    ''' <param name="table">Database table</param>
    ''' <param name="WhereDefinition">Where Fields of DELETE command</param>
    ''' <param name="Limit">Delete only one row</param>
    ''' <remarks></remarks>
    Sub Delete(ByVal table As String, ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal Limit As Boolean = True)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        oCommand.CommandText = "DELETE FROM " & table & Me.WhereCommand(WhereDefinition)
        Me.FieldsToParameters(WhereDefinition, oCommand)
        If Limit = True Then oCommand.CommandText &= " LIMIT 1"
        oCommand.ExecuteNonQuery()
        oCommand.Dispose()
        oCommand = Nothing
    End Sub
    ''' <summary>
    ''' A Simple Database select statement
    ''' </summary>
    ''' <param name="table">Database table</param>
    ''' <param name="InputFields">Comma-separated values of input fields for SELECT statement</param>
    ''' <param name="WhereDefinition">Filter by Where statement</param>
    ''' <returns>SQLResult</returns>
    ''' <remarks></remarks>
    Public Function [Select](ByVal table As String, Optional ByVal InputFields As String = "*", Optional ByVal WhereDefinition As List(Of FieldDefinition) = Nothing) As SQLResult
        Dim WhereStr As String = ""

        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        If Not IsNothing(WhereDefinition) Then
            WhereStr = Me.WhereCommand(WhereDefinition)
            For Each PD As FieldDefinition In WhereDefinition
                oCommand.Parameters.AddWithValue(PD.FieldName, PD.Value)
            Next
        End If

        oCommand.CommandText = "SELECT " & InputFields & " FROM " & table & WhereStr

        Return Me.CommandToDatatable(oCommand)

    End Function
    ''' <summary>
    ''' Insert/Update field to table with one Primary key and auto_increment and return the primary key as integer
    ''' </summary>
    ''' <param name="table">table in database</param>
    ''' <param name="SetFields">SET defintion of sql statement</param>
    ''' <param name="WhereDefinition">Where defintion of sql statement</param>
    ''' <param name="ForceUpdate">if primary keys exists force update of row?</param>
    ''' <returns>ID of where clause as integer</returns>
    ''' <remarks></remarks>
    Function InsertAutoIncrement(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal ForceUpdate As Boolean = False) As Integer
        'Dim helperList As New List(Of mysql2.FieldDefinition) : helperList.Add(WhereDefinition)

        Dim res As MySqlClientClass.SQLResult = Me.Select(table, "Count(*)", WhereDefinition)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        Dim sqlStr As String = ""
        If res.ToSingleInteger > 0 Then
            'need Update

            If ForceUpdate = True Then
                oCommand.CommandText = "UPDATE " & table & Me.SetCommand(SetFields) & Me.WhereCommand(WhereDefinition)
                'Me.FieldsToParameters(WhereDefinition, oCommand)
                'oCommand.Parameters.AddWithValue(WhereDefinition.FieldName, WhereDefinition.Value)
            Else
                Return Me.Select(table, "_rowid", WhereDefinition).ToSingleInteger
            End If
        Else
            'need insert"

            oCommand.CommandText = "INSERT " & table & Me.SetCommand(ConcatFields(WhereDefinition, SetFields))
        End If


        Me.FieldsToParameters(SetFields, oCommand)
        Me.FieldsToParameters(WhereDefinition, oCommand)

        oCommand.ExecuteNonQuery()

        Return Me.Select(table, "_rowid", WhereDefinition).ToSingleInteger

    End Function

    ''' <summary>
    ''' Insert/Update Value to table with more then one Primary key field and return Fields as SQLResult (row)
    ''' </summary>
    ''' <param name="table">table in database</param>
    ''' <param name="SetFields">SET defintion of sql statement</param>
    ''' <param name="WhereDefinition">Where defintion of sql statement</param>
    ''' <param name="ForceUpdate">if primary keys exists force update of row?</param>
    ''' <returns>SQLResult of where clause</returns>
    ''' <remarks></remarks>
    Function InsertWithMorePrimaryKeys(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal ForceUpdate As Boolean = False) As SQLResult
        Dim res As MySqlClientClass.SQLResult = Me.Select(table, "Count(*)", WhereDefinition)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        Dim sqlStr As String = ""
        If res.ToSingleInteger > 0 Then
            'Debug.WriteLine("need update")
            'need Update
            If ForceUpdate = True Then
                'Debug.WriteLine("updated")
                oCommand.CommandText = "UPDATE " & table & Me.SetCommand(SetFields) & Me.WhereCommand(WhereDefinition)
            Else
                Return Me.Select(table, Me.GetPrimaryKeys(WhereDefinition), WhereDefinition)
            End If
        Else
            'need insert"
            oCommand.CommandText = "INSERT " & table & Me.SetCommand(ConcatFields(WhereDefinition, SetFields))
        End If

        Me.FieldsToParameters(WhereDefinition, oCommand)
        Me.FieldsToParameters(SetFields, oCommand)

        oCommand.ExecuteNonQuery()

        Return Me.Select(table, Me.GetPrimaryKeys(WhereDefinition), WhereDefinition)


    End Function

#Region "MySQL Command Helpers"
    Private Function GenSQLCommand() As MySqlCommand
        Dim Cmd As New MySqlCommand
        Cmd.Connection = _Connection
        Return Cmd
    End Function
    Private Function SetCommand(ByVal FieldParamList As List(Of FieldDefinition)) As String
        Dim strWhere As String = "" : Dim strSQL As String = ""

        For Each PD As FieldDefinition In FieldParamList
            If strWhere.Length > 0 Then strWhere &= " , "
            strWhere &= PD.FieldName & " = ?" & PD.FieldName
        Next

        If strWhere.Length > 0 Then strSQL &= " SET " & strWhere

        Return strSQL
    End Function
    Private Function WhereCommand(ByVal WhereParamList As List(Of FieldDefinition)) As String
        Dim strWhere As String = "" : Dim strSQL As String = ""

        For Each PD As FieldDefinition In WhereParamList
            If strWhere.Length > 0 Then strWhere &= " AND "
            strWhere &= "(" & PD.FieldName & " = ?" & PD.FieldName & ")"
        Next

        If strWhere.Length > 0 Then strSQL &= " Where " & strWhere

        Return strSQL
    End Function
    Private Function CommandToDatatable(ByVal oCommand As MySqlCommand)
        Dim oDataset As New DataSet
        Dim oAdapter As New MySqlDataAdapter
        oAdapter.SelectCommand = oCommand
        oAdapter.Fill(oDataset)

        Dim ret As New SQLResult(oDataset)
        oDataset.Dispose()
        oAdapter.Dispose()

        Return ret
    End Function
    Private Function GetPrimaryKeys(ByVal WhereDefinition As List(Of FieldDefinition)) As String
        Dim back As String = ""
        For Each field As MySqlClientClass.FieldDefinition In WhereDefinition
            If back.Length > 0 Then back &= " , "
            back &= field.FieldName
        Next
        Return back
    End Function
#End Region
#Region "MySQL Field Builders"
    Private Function JoinFieldName(ByVal Definition As List(Of FieldDefinition)) As String
        Dim back As String = ""
        For Each field As FieldDefinition In Definition
            If back.Length > 0 Then back += ","
            back += "`" & field.FieldName & "`"
        Next
        Return back
    End Function
    Private Function JoinParameterName(ByVal Definition As List(Of FieldDefinition)) As String
        Dim back As String = ""
        For Each field As FieldDefinition In Definition
            If back.Length > 0 Then back += ","
            back += "?" & field.FieldName
        Next
        Return back
    End Function
    Private Function JoinValues(ByVal Definition As List(Of FieldDefinition)) As String
        Dim back As String = ""
        For Each field As FieldDefinition In Definition
            If back.Length > 0 Then back += ","
            back += field.FieldName & "=VALUES(`" & field.FieldName & "`)"
        Next
        Return back
    End Function

    Private Sub FieldsToParameters(ByVal FieldsDef As List(Of FieldDefinition), ByRef oCommand As MySqlCommand)
        For Each PD As FieldDefinition In FieldsDef
            If Not oCommand.Parameters.Contains(PD.FieldName) Then
                If IsNothing(PD.Value) Then
                    oCommand.Parameters.AddWithValue(PD.FieldName, "")
                Else
                    oCommand.Parameters.AddWithValue(PD.FieldName, PD.Value)
                End If
            End If
        Next
    End Sub
    Private Sub FieldsToDefinition(ByVal FieldsDef As List(Of FieldDefinition), ByRef NewFieldDef As List(Of FieldDefinition))
        For Each wField As FieldDefinition In FieldsDef
            NewFieldDef.Add(wField)
        Next
    End Sub
    Private Function ConcatFields(ByVal ParamArray Args() As List(Of FieldDefinition)) As List(Of FieldDefinition)

        Dim backList As New List(Of FieldDefinition)

        For i As Integer = 0 To Args.GetUpperBound(0)
            FieldsToDefinition(Args(i), backList)
        Next

        Return backList

    End Function
    Private Function SingleFieldToList(ByVal fields As MySqlClientClass.FieldDefinition) As List(Of MySqlClientClass.FieldDefinition)
        Dim tList As New List(Of MySqlClientClass.FieldDefinition)
        tList.Add(fields)
        Return tList
    End Function
#End Region
#Region "HelperDef"
    Structure FieldDefinition
        Dim FieldName As String
        Dim Value As String
        Sub New(ByVal Fieldname As String, ByVal Value As String)
            MyClass.FieldName = Fieldname
            MyClass.Value = Value
        End Sub
    End Structure
    Structure ClassConnectionInfo
        Dim User As String
        Dim Password As String
        Dim Server As String
        Dim Database As String
        Dim Port As Integer
        Public Sub New(ByVal Database As String, _
                        Optional ByVal Server As String = "localhost", _
                        Optional ByVal User As String = "root", _
                        Optional ByVal Password As String = "", _
                        Optional ByVal Port As Integer = 3306)

            MyClass.Database = Database
            MyClass.Server = Server
            MyClass.User = User
            MyClass.Password = Password
            MyClass.Port = Port

        End Sub
    End Structure
    Public Class SQLResult
        Dim _sqlresult As New DataSet
        Public Sub New(ByVal sqlresult As DataSet)
            _sqlresult = sqlresult
        End Sub
        Function ToDataTable() As DataTable
            Return _sqlresult.Tables.Item(0)
        End Function
        ''' <summary>
        ''' Return the first item in the first row
        ''' </summary>
        ''' <returns>string</returns>
        ''' <remarks></remarks>
        Function ToSingleString() As String
            Return _sqlresult.Tables.Item(0).Rows(0).Item(0)
        End Function

        ''' <summary>
        ''' Return the first item in the first row as integer. works only for numberic field!
        ''' </summary>
        ''' <returns>integer</returns>
        ''' <remarks></remarks>
        Function ToSingleInteger() As Integer
            Dim ret As Integer = 0
            Integer.TryParse(_sqlresult.Tables.Item(0).Rows(0).Item(0), ret)
            Return ret
        End Function
        Function ToDataSet() As DataSet
            Return _sqlresult
        End Function
    End Class
    Public Class ClassConnectionInfo1
        Private _User As String
        Private _Password As String
        Private _Server As String
        Private _Database As String
        Private _Port As Integer
        Public Sub New(ByVal Database As String, _
                        Optional ByVal Server As String = "localhost", _
                        Optional ByVal User As String = "root", _
                        Optional ByVal Password As String = "", _
                        Optional ByVal Port As Integer = 3306)

            _Database = Database
            _Server = Server
            _User = User
            _Password = Password
            _Port = Port
        End Sub
        ''' <summary>
        ''' default user: root
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property User() As String
            Get
                Return _User
            End Get
            Set(ByVal value As String)
                _User = value
            End Set
        End Property
        ''' <summary>
        ''' default no password
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Property Password() As String
            Get
                Return _Password
            End Get
            Set(ByVal value As String)
                _Password = value
            End Set
        End Property
        ''' <summary>
        ''' default: localhost
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Server() As String
            Get
                Return _Server
            End Get
            Set(ByVal value As String)
                _Server = value
            End Set
        End Property
        ''' <summary>
        ''' name of database
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Database() As String
            Get
                Return _Database
            End Get
            Set(ByVal value As String)
                _Database = value
            End Set
        End Property
        ''' <summary>
        ''' default 3306
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Port() As Integer
            Get
                Return _Port
            End Get
            Set(ByVal value As Integer)
                _Port = value
            End Set
        End Property


    End Class
#End Region


End Class

