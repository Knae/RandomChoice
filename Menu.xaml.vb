Imports System.IO
Imports System.Linq.Expressions

Structure categoryFile
    Public fileName As String
    Public filePath As String
End Structure

Public Class Menu
    'Hardcoded the location of the data files. It'll be in the data folder of the program
    Public ReadOnly categoryLocation As String = My.Computer.FileSystem.CurrentDirectory + "\data\"
    Private fileList As New List(Of categoryFile)
    Private optionsInSelected As New List(Of String)
    Private lostFocusCheck As Boolean = False

    'Update the info panel with contents of the selected category
    Private Sub UpdateInfo()
        Dim selectedIndex As Integer = lstbxCategoryList.SelectedIndex

        'Onlly proceed if a category is selected
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
    'Make sure the directory exists
    Private Sub CheckDirectory()
        Dim readLine As String = Nothing

        fileList.Clear()
        lstbxCategoryList.Items.Clear()

        'If directory does not exist, create a new folder. Else, make a list of all the files in the folder
        If (My.Computer.FileSystem.DirectoryExists(categoryLocation)) Then
            For Each file As String In My.Computer.FileSystem.GetFiles(categoryLocation)
                Dim newFile As categoryFile = New categoryFile()
                newFile.filePath = file
                newFile.fileName = System.IO.Path.GetFileNameWithoutExtension(file)
                fileList.Add(newFile)
                lstbxCategoryList.Items.Add(newFile.fileName)
                'Next

                'fileList.Sort(Function(c1, c2) c1.fileName.CompareTo(c2.fileName))

                'For Each element In fileList
                'lstbxCategoryList.Items.Add(element.fileName)
            Next
        Else
            My.Computer.FileSystem.CreateDirectory(categoryLocation)
        End If
    End Sub

    Private Sub wndMainMenu_Initialized(sender As Object, e As EventArgs) Handles wndMainMenu.Initialized
        CheckDirectory()
        UpdateInfo()
    End Sub
    'Each time a new category is selected, update the info panel.
    Private Sub lstbxCategoryList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstbxCategoryList.SelectionChanged
        UpdateInfo()
    End Sub
    'If the user category is double-clicked on, open the randomiser window with that category
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
    'open the randomiser window with the current selected category
    Private Sub btnLoadCategory_Click(sender As Object, e As RoutedEventArgs) Handles btnLoadCategory.Click
        'If no category selected, do nothing
        If (lstbxCategoryList.SelectedIndex <> -1) Then
            Dim newRandomiser As New MainWindow
            newRandomiser.PassOptions(lblCategoryName.Text, optionsInSelected)
            newRandomiser.Show()
            Hide()
        End If
    End Sub
    'Opens the category manager to create a new category
    Private Sub btnCreateCategory_Click(sender As Object, e As RoutedEventArgs) Handles btnCreateCategory.Click
        Dim newCategoryEditor As New CategoryManager
        newCategoryEditor.CreatingFile(categoryLocation)
        newCategoryEditor.Show()
        Hide()
    End Sub
    'Open the category manager with the selected category for editing
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
    'When focus shifts back to the window, update the info panel. Just in case user edits the files
    'externally
    'TODO: This does not appear to work
    Private Sub wndMainMenu_IsVisibleChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles wndMainMenu.IsVisibleChanged
        CheckDirectory()
        UpdateInfo()
    End Sub
    'Open the data folder in window's file explorer
    Private Sub btnOpenFolder_Click(sender As Object, e As RoutedEventArgs) Handles btnOpenFolder.Click
        Process.Start("explorer.exe", My.Computer.FileSystem.CurrentDirectory + "\data\")
    End Sub

    Private Sub wndMainMenu_IsVisibleChanged(sender As Object, e As RoutedEventArgs) Handles wndMainMenu.GotFocus

    End Sub
End Class
