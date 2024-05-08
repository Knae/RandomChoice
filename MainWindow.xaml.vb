Imports System.IO

Class MainWindow
    Private defFileLoc As String = My.Computer.FileSystem.CurrentDirectory + "\data\choices.txt"
    Private choices As New List(Of String)()

    Private Sub Grid_Initialized(sender As Object, e As EventArgs)
        'LoadFile(defFileLoc)
    End Sub

    'Public Sub LoadFile(targetLoc As String)

    '    Dim readLine As String = Nothing
    '    If (My.Computer.FileSystem.FileExists(targetLoc)) Then
    '        Dim fileReader As StreamReader
    '        fileReader = My.Computer.FileSystem.OpenTextFileReader(targetLoc)

    '        While (fileReader.Peek() >= 0)
    '            readLine = fileReader.ReadLine()
    '            choices.Add(readLine)
    '            readLine = Nothing
    '        End While

    '        Dim categoryName As String = System.IO.Path.GetFileNameWithoutExtension(targetLoc)
    '        lblCategory.Content = categoryName.ToUpper
    '        fileReader.Close()
    '    End If
    'End Sub
    'Populate the list of category elements and the category name with the
    'information passed to this window
    Public Sub PassOptions(categoryName As String, listedOptions As List(Of String))
        For Each line In listedOptions
            choices.Add(line)
        Next
        lblCategory.Content = categoryName.ToUpper
    End Sub

    Private Sub btnDecide_Click(sender As Object, e As RoutedEventArgs) Handles btnDecide.Click
        ChooseRandom()
    End Sub
    'Select a random element from the category
    Private Sub ChooseRandom()
        Dim numberOfChoices As Integer = choices.Count
        'Generate a random integer between 0 and (numberOfChoices - 1).
        Dim randomChoice As Integer = (Rnd() * (numberOfChoices - 1))
        lblResult.Content = choices.ElementAt(randomChoice)
        lblResult.UpdateLayout()
    End Sub

    Private Sub btnReturnToMenu_Click(sender As Object, e As RoutedEventArgs) Handles btnReturnToMenu.Click
        Close()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Application.Current.MainWindow.Show()
    End Sub
End Class
