﻿Imports System.IO

Public Class CategoryManager
    Private options As New List(Of String)
    Private categName As String
    Private categFileLoc As String

    Public Sub CreatingFile(fileLoc As String)
        categFileLoc = fileLoc
        categName = "Enter name here"
        txtboxName.Text = categName.ToUpper
        UpdateList()
    End Sub

    Public Sub EditingFile(fileLoc As String, categoryName As String, listedOptions As List(Of String))
        categFileLoc = fileLoc
        For Each line In listedOptions
            options.Add(line)
        Next
        categName = categoryName

        txtboxName.Text = categName.ToUpper
        UpdateList()
    End Sub

    Private Sub UpdateList()
        lstbxOptionList.Items.Clear()
        categName = txtboxName.Text
        For Each element In options
            lstbxOptionList.Items.Add(element)
        Next

    End Sub

    Private Sub CheckIfAnySelected()

        If (lstbxOptionList.SelectedIndex <> -1) Then
            btnRemoveOption.IsEnabled = True
        Else
            btnRemoveOption.IsEnabled = False
        End If

    End Sub

    Private Sub Window_Initialized(sender As Object, e As EventArgs)
        txtboxName.Text = Nothing
        options.Clear()
        UpdateList()
        CheckIfAnySelected()
    End Sub

    Private Sub btnAddOption_Click(sender As Object, e As RoutedEventArgs) Handles btnAddOption.Click
        If (txtboxNewOption.Text.Length > 0) Then
            options.Add(txtboxNewOption.Text)
            txtboxNewOption.Text = Nothing
        End If
        UpdateList()
    End Sub

    Private Sub btnRemoveOption_Click(sender As Object, e As RoutedEventArgs) Handles btnRemoveOption.Click
        'If (lstbxOptionList.SelectedIndex <> -1) Then
        options.RemoveAt(lstbxOptionList.SelectedIndex)
        UpdateList()

        'End If
    End Sub

    Private Sub lstbxOptionList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstbxOptionList.SelectionChanged
        CheckIfAnySelected()
    End Sub

    Private Sub btnSaveCateg_Click(sender As Object, e As RoutedEventArgs) Handles btnSaveCateg.Click
        Dim fileLoc As String = categFileLoc + categName + ".txt"
        Dim writer As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(fileLoc, False)

        For Each element In options
            writer.WriteLine(element)
        Next

        writer.Close()
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Application.Current.MainWindow.Show()
    End Sub
End Class
