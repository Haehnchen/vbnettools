'$Id$
'$HeadURL$'

Module Example

    Sub Main()
        'Our DLL has the Namespace ExampleDLL.TheClass, ExampleDLL ist generated automatically out of the filename
        Dim Addin As New AddinDLLReader("ExampleDLL.dll", "TheClass")

        'Some ready functions
        Console.WriteLine("Description: " & Addin.Description)
        Console.WriteLine("Version: " & Addin.Version)

        'call a known function that returns a ArrayList
        Console.WriteLine(vbNewLine & "GetExtension; returns ArrayList:")
        For Each Ext In Addin.RunMethod("GetExtension")
            Console.WriteLine(Ext)
        Next

        'show all function / sub of the class
        Console.WriteLine(vbNewLine & "The class contains this methodes:")
        For Each Met In Addin.GetMethodes
            Console.WriteLine(Met)
        Next

        'check if the class has a given function
        Console.WriteLine(vbNewLine & "checking if function/sub exists")
        If Addin.GetMethodes.Contains("Version") Then
            Console.WriteLine("Function ´Version´ was found")
        End If

        'call a function with a paramter
        Console.WriteLine(vbNewLine & "We are calling FunctionWithParamter with a single paramter:")
        Console.WriteLine("returns: " & Addin.RunMethod("FunctionWithParamter", New Object() {"MyParamter"}))

        Console.ReadKey()


        'the code above gave you this output
        'Description: Wow a Description at: 25.03.2010 14:31:36
        'Version: 1.0.0.0

        'GetExtension; returns ArrayList:
        '        pdf()
        '        doc()

        'The class contains this methodes:
        '        GetExtension()
        '        Version()
        '        Description()
        '        FunctionWithParamter()

        'checking if function/sub exists
        'Function ´Version´ was found

        'We are calling FunctionWithParamter with a single paramter:
        'returns: You gave me a nice paramter: MyParamter

    End Sub

End Module
