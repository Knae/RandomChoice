Imports System.IO
Imports System.Linq.Expressions

Structure categoryFile
    Public fileName As String
    Public filePath As String
End Structure

Public Class Menu
    Public ReadOnly categoryLocation As String = My.Computer.FileSystem.CurrentDirectory + "\data\"
    Private fileList As New List(Of categoryFile)
    Private optionsInSelected As New List(Of String)
    Private lostFocusCheck As Boolean = False

    Private Sub UpdateInfo()
        Dim selectedIndex As Integer = lstbxCategoryList.SelectedIndex

        If (selectedIndex <> -1) Then
            optionsInSelected.Clear()
            txtblkCategoryInfo.Text = ""

            Dim selectedFile As categoryFile = fileList.Item(lstbxCategoryList.SelectedIndex)
            lblCategoryName.Text = selectedFile.fileName
            'Read the options
            Dim fileReader As StreamReader
            Dim readLine As String
            Dim displayedOptions As String = Nothing
            fileReader = My.Computer.FileSystem.OpenTextFileReader(selectedFile.filePath)

            While (fileReader.Peek() >= 0)
                readLine = fileReader.ReadLine()
                optionsInSelected.Add(readLine)
                displayedOptions += (readLine + vbNewLine)
                readLine = Nothing
            End While

            txtblkCategoryInfo.Text = displayedOptions
            fileReader.Close()
            btnLoadCategory.IsEnabled = True
        Else
            lblCategoryName.Text = Nothing
            txtblkCategoryInfo.Text = Nothing
            btnLoadCategory.IsEnabled = False
        End If

    End Sub

    Private Sub CheckDirectory()
        Dim readLine As String = Nothing
        fileList.Clear()
        lstbxCategoryList.Items.Clear()

        If (My.Computer.FileSystem.DirectoryExists(categoryLocation)) Then
            For Each file As String In My.Computer.FileSystem.GetFiles(categoryLocation)
                Dim newFile As categoryFile = New categoryFile()
                newFile.filePath = file
                newFile.fileName = System.IO.Path.GetFileNameWithoutExtension(file)
                fileList.Add(newFile)
                lstbxCategoryList.Items.Add(newFile.fileName)
            Next
        End If
    End Sub

    Private Sub wndMainMenu_Initialized(sender As Object, e As EventArgs) Handles wndMainMenu.Initialized
        CheckDirectory()
        UpdateInfo()
    End Sub

    Private Sub lstbxCategoryList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstbxCategoryList.SelectionChanged
        UpdateInfo()
    End Sub

    Private Sub lstbxCategoryList_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstbxCategoryList.MouseDoubleClick

        'Check if we're clicking on a text item in the list
        If (lstbxCategoryList.IsMouseCaptureWithin) Then
            UpdateInfo()
            Dim newRandomiser As New MainWindow
            newRandomiser.PassOptions(lblCategoryName.Text, optionsInSelected)
            newRandomiser.Show()
            Hide()
        End If

    End Sub

    Private Sub btnLoadCategory_Click(sender As Object, e As RoutedEventArgs) Handles btnLoadCategory.Click
        If (lstbxCategoryList.SelectedIndex <> -1) Then
            Dim newRandomiser As New MainWindow
            newRandomiser.PassOptions(lblCategoryName.Text, optionsInSelected)
            newRandomiser.Show()
            Hide()
        End If
    End Sub

    Private Sub btnCreateCategory_Click(sender As Object, e As RoutedEventArgs) Handles btnCreateCategory.Click
        Dim newCategoryEditor As New CategoryManager
        newCategoryEditor.CreatingFile(categoryLocation)
        newCategoryEditor.Show()
        Hide()
    End Sub

    Private Sub btnEditCategory_Click(sender As Object, e As RoutedEventArgs) Handles btnEditCategory.Click
        If (lstbxCategoryList.SelectedIndex <> -1) Then
            Dim newCategoryEditor As New CategoryManager
            Dim selectedFile As categoryFile = fileList.Item(lstbxCategoryList.SelectedIndex)
            newCategoryEditor.EditingFile(categoryLocation, selectedFile.fileName, optionsInSelected)
            newCategoryEditor.Show()
            Hide()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub wndMainMenu_IsVisibleChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles wndMainMenu.IsVisibleChanged
        CheckDirectory()
        UpdateInfo()
    End Sub
End Class
