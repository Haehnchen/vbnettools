'$Id$
'$HeadURL$

Public Class TheClass
    Function GetExtension() As ArrayList
        Dim Ext As New ArrayList
        Ext.Add("pdf") : Ext.Add("doc")
        Return Ext
    End Function
    Function Version() As String
        Return My.Application.Info.Version.ToString()
    End Function
    Function Description() As String
        Return "Wow a Description at: " & Now
    End Function
    Function FunctionWithParamter(ByVal paramter As String) As String
        Return "You gave me a nice paramter: " & paramter
    End Function
End Class
