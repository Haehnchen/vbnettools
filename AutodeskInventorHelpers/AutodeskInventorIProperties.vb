'$Id$
'$HeadURL$

Imports Inventor
''' <summary>
''' A class holder for getting Autodesk Inventor IProperties
''' To use this class:
'''  - Install DeveloperTools found in your Applicationpath: Autodesk\Inventor 20xx\SDK\DeveloperTools.msi
'''  - Add Reference on Autodesk.Inventor.Interop to get the Inventor Namespace
''' 
''' find more on:
''' http://modthemachine.typepad.com/my_weblog/2010/02/accessing-iproperties.html
''' </summary>
''' <remarks></remarks>
Public Class AutodeskInventorIProperties

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FullFileName">A Autodesk Inventor File (eg. IAM, IPT, IDW, ...)</param>
    ''' <returns>Hashtable with custom values</returns>
    ''' <remarks></remarks>
    Shared Function GetIProperties(ByVal FullFileName As String) As Hashtable

        'Create a Empty return values
        Dim ReturnIProperties As New Hashtable

        'Exit if file doesnt exists
        If Not System.IO.File.Exists(FullFileName) Then
            Return ReturnIProperties
        End If

        'Create a new InventorObject
        Dim InventorObject As New ApprenticeServerComponent

        'A simple debug for illegal drawings
        Try

            'Open the file it must be Autodesk Inventor File (eg IDW, IAM, IPT, ...) no further check is down
            'Please filter your files before
            Dim InventorDocument As ApprenticeServerDocument = InventorObject.Open(FullFileName)

            'We are working through every "design tracking property" (PropertySet) to find IProperties
            'You will find all custom and most used IProperties on "Design Tracking Properties" but it is better to search
            'on the whole PropertySet.
            'All available "design tracking property" (PropertySet)
            ' - Inventor Summary Information
            ' - Inventor Document Summary Information
            ' - Design Tracking Properties
            ' - Inventor User Defined Properties
            'get it with oPropSets.Item("Design Tracking Properties") 
            For Each oPropSet As PropertySet In InventorDocument.PropertySets

                'I dont want all IProperties fields and custom names for better usage
                'there is no search / find function in the api so we must do that on our own
                'Feel free to modify it
                For Each oProp As [Property] In oPropSet
                    If oProp.Name = "Part Number" Then ReturnIProperties.Add("partnr", oProp.Value)
                    If oProp.Name = "Author" Then ReturnIProperties.Add("autor", oProp.Value)
                    If oProp.Name = "Designer" Then ReturnIProperties.Add("designer", oProp.Value)
                    If oProp.Name = "Description" Then ReturnIProperties.Add("description", oProp.Value)
                Next

            Next

            'Unset the object
            'I had some memory leaks when working with tens of thousands files
            InventorDocument.Close()
            InventorDocument = Nothing

        Catch ex As Exception
            'Debug.WriteLine(datei)
        End Try

        'also unset the ApprenticeServerComponent
        InventorObject.Close()
        InventorObject = Nothing

        Return ReturnIProperties


    End Function
    ''' <summary>
    ''' On every drawing Autodesk Inventor include a thumbnail
    ''' We can also get that picture out of the IProperties and convert it to System.Drawing.Image
    ''' You must add a reference to Microsoft.VisualBasic.Compatibility
    ''' 
    ''' I dont use it anymore:
    ''' To read Thumbnails out of every file use this: http://www.espend.de/node/34 it works
    ''' with every file that has a thumbnail handler in the windows explorer
    ''' </summary>
    ''' <param name="FullFileName">A Autodesk Inventor File (eg. IAM, IPT, IDW, ...)</param>
    ''' <returns>The thumbnail of the Inventor file as Image</returns>
    ''' <remarks></remarks>
    Shared Function GetThumbnail(ByVal FullFileName As String) As System.Drawing.Image
        Try
            Dim InventorObject As New ApprenticeServerComponent
            Dim InventorDocument As ApprenticeServerDocument = InventorObject.Open(FullFileName)
            Dim value As Object = InventorDocument.PropertySets.Item("Inventor Summary Information").Item("Thumbnail").Value

            InventorDocument.Close() : InventorDocument = Nothing
            InventorObject.Close() : InventorObject = Nothing

            'change it if you need that stuff
            Return Nothing
            'Return Microsoft.VisualBasic.Compatibility.VB6.Support.IPictureDispToImage(value)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
