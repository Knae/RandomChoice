Imports System.IO

Public Class CategoryManager
    Private options As New List(Of String)
    Private categName As String
    Private categFileLoc As String
    Private isNewFile As Boolean = False


    Private Sub txtboxName_GotFocus(sender As Object, e As RoutedEventArgs) Handles txtboxName.GotFocus
        If (txtboxName.Text = ("Enter name here").ToUpper) Then
            txtboxName.Text = Nothing
        End If
    End Sub

    Private Sub txtboxNewOption_GotFocus(sender As Object, e As RoutedEventArgs) Handles txtboxNewOption.GotFocus
        If (Not txtboxNewOption.Text Is Nothing) Then
            txtboxNewOption.Text = Nothing
        End If
    End Sub

    Public Sub CreatingFile(fileLoc As String)
        categFileLoc = fileLoc
        categName = "Enter name here"
        txtboxName.Text = categName.ToUpper
        isNewFile = True
        UpdateList()
    End Sub

    Public Sub EditingFile(fileLoc As String, categoryName As String, listedOptions As List(Of String))
        categFileLoc = fileLoc
        For Each line In listedOptions
            options.Add(line)
        Next
        categName = categoryName

        txtboxName.Text = categName.ToUpper
        txtboxName.IsEnabled = False
        UpdateList()
    End Sub

    Private Sub UpdateList()
        lstbxOptionList.Items.Clear()
        categName = txtboxName.Text
        For Each element In options
            lstbxOptionList.Items.Add(element)
        Next

    End Sub
    'Only enable the remove button if an option is selected
    Private Sub CheckIfAnySelected()

        If (lstbxOptionList.SelectedIndex <> -1) Then
            btnRemoveOption.IsEnabled = True
        Else
            btnRemoveOption.IsEnabled = False
        End If

    End Sub

    'On initialize, make sure everything is empty before we do anything
    Private Sub Window_Initialized(sender As Object, e As EventArgs)
        txtboxName.Text = Nothing
        options.Clear()
        UpdateList()
        CheckIfAnySelected()
    End Sub
    'Add a new element as long as the text box is not empty
    Private Sub BtnAddOption_Click(sender As Object, e As RoutedEventArgs) Handles btnAddOption.Click
        If (Not txtboxNewOption.Text Is Nothing) Then
            options.Add(txtboxNewOption.Text)
            txtboxNewOption.Text = Nothing
        End If
        UpdateList()
    End Sub

    'Remove the current selected element. Button should be disabled if none are selected
    Private Sub BtnRemoveOption_Click(sender As Object, e As RoutedEventArgs) Handles btnRemoveOption.Click
        'If (lstbxOptionList.SelectedIndex <> -1) Then
        options.RemoveAt(lstbxOptionList.SelectedIndex)
        UpdateList()

        'End If
    End Sub

    'Attempt to delete the current category
    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Dim fileLoc As String = categFileLoc + categName + ".txt"

        If (My.Computer.FileSystem.FileExists(fileLoc)) Then
            '
            Dim confirm As MessageBoxResult = MessageBox.Show("Delete this category?", "Confirm", MessageBoxButton.YesNo)

            If (confirm = MessageBoxResult.Yes) Then
                My.Computer.FileSystem.DeleteFile(fileLoc)
                Close()
            End If
        Else
            'Log that file does not exists somehow
        End If
    End Sub

    Private Sub lstbxOptionList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstbxOptionList.SelectionChanged
        CheckIfAnySelected()
    End Sub
    'Save changes by writing/overwriting each line into the file
    Private Sub btnSaveCateg_Click(sender As Object, e As RoutedEventArgs) Handles btnSaveCateg.Click
        Dim fileLoc As String = categFileLoc + categName + ".txt"
        Dim writer As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(fileLoc, False)

        For Each element In options
            writer.WriteLine(element)
        Next

        writer.Close()
        Close()
    End Sub
    'Close this dialog
    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        Close()
    End Sub
    'If the this window is closed, show the main window.
    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Application.Current.MainWindow.Show()
    End Sub
    'If an item is double-clicked, open a dialog box to change the text
    Private Sub lstbxOptionList_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstbxOptionList.MouseDoubleClick
        If (lstbxOptionList.SelectedIndex <> -1) Then
            Dim inputResult As Object = InputBox("Enter new text:", "Change entry", options(lstbxOptionList.SelectedIndex).ToString)
            If (Not inputResult = Nothing) Then
                options(lstbxOptionList.SelectedIndex) = inputResult.ToString
                UpdateList()
            End If
        Else
            'No valied selected option
        End If
    End Sub

End Class