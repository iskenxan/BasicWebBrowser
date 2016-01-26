Imports System.IO



Public Class Writer
    'This objet writes a data about bookmarks to the text file using streamWriter
    Private url As String


    Public Function WriteData(data As String) As Boolean
        Dim sucess As Boolean = True
        Dim writer As StreamWriter
        writer = My.Computer.FileSystem.OpenTextFileWriter("bookmarks.txt", True)
        writer.WriteLine(data)
        writer.Close()

        Return sucess
    End Function
End Class
