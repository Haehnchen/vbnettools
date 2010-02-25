'$Id$
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
                ";port=" & _ConnectionInfo.Port & ";charset=utf8"
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
    Sub InsOnDupWithMoreKeys(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition))
        'insert into constr_file_info (`rID`,`pID`,`fID`,`Size`) VALUES ('2','2','1','5555') ON DUPLICATE KEY UPDATE `Size`=VALUES(`Size`)
        'insert into constr_file_info (rID,pID,fID,Size) VALUES ('2','2','1','5555') ON DUPLICATE KEY UPDATE `Size`=VALUES(`Size`); select rID,pID,fID from constr_file_info where rID='2' AND pID='2' AND fID='1'

        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        Dim h1 As New List(Of FieldDefinition)

        Me.FieldsToDefinition(WhereDefinition, h1)
        Me.FieldsToDefinition(SetFields, h1)

        Dim str As String = "INSERT INTO `" & table & "` (" & JoinFieldName(h1) & _
        ") VALUES (" & JoinParameterName(h1) & _
        ") ON DUPLICATE KEY UPDATE " & JoinValues(SetFields)

        oCommand.CommandText = str
        Me.FieldsToParameters(h1, oCommand)

        oCommand.ExecuteNonQuery()
        oCommand.Dispose()
        oCommand = Nothing

    End Sub
    Sub Delete(ByVal table As String, ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal Limit As Boolean = True)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        oCommand.CommandText = "DELETE FROM " & table & Me.WhereCommand(WhereDefinition)
        Me.FieldsToParameters(WhereDefinition, oCommand)
        If Limit = True Then oCommand.CommandText &= " LIMIT 1"
        oCommand.ExecuteNonQuery()
        oCommand.Dispose()
        oCommand = Nothing

    End Sub
    Function UniSingleKey(ByVal table As String, ByVal SetField As FieldDefinition) As String
        'Dim helperList As New List(Of mysql2.FieldDefinition) : helperList.Add(WhereDefinition)

        Dim SetFieldList As List(Of MySqlClientClass.FieldDefinition) = SingleFieldToList(SetField)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand

        oCommand.CommandText = "INSERT IGNORE " & table & Me.SetCommand(SetFieldList) & "; Select _rowid from " & table & " " & Me.WhereCommand(SetFieldList)

        Me.FieldsToParameters(SetFieldList, oCommand)
        '   oCommand.ExecuteNonQuery()
        Dim sqlres As String = Me.CommandToDatatable(oCommand).ToSingleString
        oCommand.Dispose()
        oCommand = Nothing
        Return sqlres
    End Function

    Public Function SelectCommand(ByVal table As String, Optional ByVal InputFields As String = "*", Optional ByVal WhereParamList As List(Of FieldDefinition) = Nothing) As SQLResult
        Dim WhereStr As String = ""

        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        If Not IsNothing(WhereParamList) Then
            WhereStr = Me.WhereCommand(WhereParamList)
            For Each PD As FieldDefinition In WhereParamList
                oCommand.Parameters.AddWithValue(PD.FieldName, PD.Value)
            Next
        End If

        oCommand.CommandText = "SELECT " & InputFields & " FROM " & table & WhereStr

        Return Me.CommandToDatatable(oCommand)

    End Function
    Private Function GenSQLCommand() As MySqlCommand
        Dim Cmd As New MySqlCommand
        Cmd.Connection = _Connection
        Return Cmd
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
    Private Function WhereCommand(ByVal WhereParamList As List(Of FieldDefinition)) As String
        Dim strWhere As String = "" : Dim strSQL As String = ""

        For Each PD As FieldDefinition In WhereParamList
            If strWhere.Length > 0 Then strWhere &= " AND "
            strWhere &= "(" & PD.FieldName & " = ?" & PD.FieldName & ")"
        Next

        If strWhere.Length > 0 Then strSQL &= " Where " & strWhere

        Return strSQL
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

    Private Function GetPrimaryKeys(ByVal WhereDefinition As List(Of FieldDefinition)) As String
        Dim back As String = ""
        For Each field As MySqlClientClass.FieldDefinition In WhereDefinition
            If back.Length > 0 Then back &= " , "
            back &= field.FieldName
        Next
        Return back
    End Function

    Private Function SingleFieldToList(ByVal fields As MySqlClientClass.FieldDefinition) As List(Of MySqlClientClass.FieldDefinition)
        Dim tList As New List(Of MySqlClientClass.FieldDefinition)
        tList.Add(fields)
        Return tList
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
    Function InsUpdSingleKey(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal ForceUpdate As Boolean = False) As Integer
        'Dim helperList As New List(Of mysql2.FieldDefinition) : helperList.Add(WhereDefinition)

        Dim res As MySqlClientClass.SQLResult = Me.SelectCommand(table, "Count(*)", WhereDefinition)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        Dim sqlStr As String = ""
        If res.ToSingleInteger > 0 Then
            'need Update

            If ForceUpdate = True Then
                oCommand.CommandText = "UPDATE " & table & Me.SetCommand(SetFields) & Me.WhereCommand(WhereDefinition)
                FieldsToParameters(WhereDefinition, oCommand)
                'oCommand.Parameters.AddWithValue(WhereDefinition.FieldName, WhereDefinition.Value)
            Else
                Return Me.SelectCommand(table, "_rowid", WhereDefinition).ToSingleInteger
            End If
        Else
            'need insert"
            oCommand.CommandText = "INSERT " & table & Me.SetCommand(SetFields)
        End If


        Me.FieldsToParameters(SetFields, oCommand)

        oCommand.ExecuteNonQuery()


        Return Me.SelectCommand(table, "_rowid", WhereDefinition).ToSingleInteger

    End Function
    ''' <summary>
    ''' Insert/Update Value to table with more then one Primary key and return Fields as SQLResult (row)
    ''' </summary>
    ''' <param name="table">table in database</param>
    ''' <param name="SetFields">SET defintion of sql statement</param>
    ''' <param name="WhereDefinition">Where defintion of sql statement</param>
    ''' <param name="ForceUpdate">if primary keys exists force update of row?</param>
    ''' <returns>SQLResult of where clause</returns>
    ''' <remarks></remarks>
    Function InsUpdWithMoreKeys(ByVal table As String, ByVal SetFields As List(Of FieldDefinition), ByVal WhereDefinition As List(Of FieldDefinition), Optional ByVal ForceUpdate As Boolean = False) As SQLResult
        Dim res As MySqlClientClass.SQLResult = Me.SelectCommand(table, "Count(*)", WhereDefinition)
        Dim oCommand As MySqlCommand = _Connection.CreateCommand
        Dim sqlStr As String = ""
        If res.ToSingleInteger > 0 Then
            'Debug.WriteLine("need update")
            'need Update
            If ForceUpdate = True Then
                'Debug.WriteLine("updated")
                oCommand.CommandText = "UPDATE " & table & Me.SetCommand(SetFields) & Me.WhereCommand(WhereDefinition)
            Else
                Return Me.SelectCommand(table, Me.GetPrimaryKeys(WhereDefinition), WhereDefinition)
            End If
        Else
            'need insert"
            'Debug.WriteLine("need insert")
            Dim h1 As New List(Of FieldDefinition)

            Me.FieldsToDefinition(WhereDefinition, h1)
            Me.FieldsToDefinition(SetFields, h1)

            oCommand.CommandText = "INSERT " & table & Me.SetCommand(h1)
        End If

        Me.FieldsToParameters(WhereDefinition, oCommand)
        Me.FieldsToParameters(SetFields, oCommand)

        ' Debug.WriteLine(oCommand.CommandText)
        Try


            oCommand.ExecuteNonQuery()
        Catch ex As Exception

        End Try

        Return Me.SelectCommand(table, Me.GetPrimaryKeys(WhereDefinition), WhereDefinition)


    End Function


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

    '    MySqlCommand oCommand = oConn.CreateCommand(); 
    'oCommand.CommandText = "select * from cust_customer where id=?id"; 

    'MySqlParameter oParam = oCommand.Parameters.Add("?id", MySqlDbType.Int32); 
    'oParam.Value = ld; 

    'oCommand.Connection = oConn; 
    'DataSet oDataSet = new DataSet(); 
    'MySqlDataAdapter oAdapter = new MySqlDataAdapter(); 
    'oAdapter.SelectCommand = oCommand; 
    'oAdapter.Fill(oDataSet); 
    'oConn.Close(); 
    'return oDataSet; 




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

