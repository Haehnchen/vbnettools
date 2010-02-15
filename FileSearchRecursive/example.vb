Module example
    'or in single line: Dim _Files As New FileSearch(New IO.DirectoryInfo("d:\Media\"), "*.nfo")
    Dim RecursiveScan As New FileSearch

    Sub Main()
        Dim RootDir As String = "C:\Windows"
        Console.WriteLine("Scanning: " & RootDir)
        RecursiveScan.Search(RootDir, "*.exe")

        'print out scan result
        Console.WriteLine(RecursiveScan.Files.Count & " Files found, press key for details")
        Console.ReadKey()

        'walk through ArrayList of Files
        For Each File As System.IO.FileInfo In RecursiveScan.Files
            Console.WriteLine(File.FullName & " - " & File.Length & " - " & File.LastWriteTime)
        Next

        Console.ReadKey()

        'you can also use this method to scan recursive but there is no error handling
        'System.IO.Directory.GetFiles(RootDir, "*.exe", IO.SearchOption.AllDirectories)
    End Sub

End Module
