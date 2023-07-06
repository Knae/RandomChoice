Imports System.IO

Class MainWindow
    Private defFileLoc As String = My.Computer.FileSystem.CurrentDirectory + "\data\choices.txt"
    Private choices As New List(Of String)()

    Private Sub Grid_Initialized(sender As Object, e As EventArgs)
        LoadFile(defFileLoc)
    End Sub

    Public Sub LoadFile(targetLoc As String)

        Dim readLine As String = Nothing
        If (My.Computer.FileSystem.FileExists(targetLoc)) Then
            Dim fileReader As StreamReader
            fileReader = My.Computer.FileSystem.OpenTextFileReader(targetLoc)

            While (fileReader.Peek() >= 0)
                readLine = fileReader.ReadLine()
                choices.Add(readLine)
                readLine = Nothing
            End While

            Dim categoryName As String = System.IO.Path.GetFileNameWithoutExtension(targetLoc)
            lblCategory.Content = categoryName.ToUpper
        End If
    End Sub

    Private Sub btnDecide_Click(sender As Object, e As RoutedEventArgs) Handles btnDecide.Click

        ChooseRandom()
    End Sub

    Private Sub ChooseRandom()
        Dim numberOfChoices As Integer = choices.Count
        Dim randomChoice As Integer = (Rnd() * numberOfChoices)
        lblResult.Content = choices.ElementAt(randomChoice)
        lblResult.UpdateLayout()
    End Sub
End Class
