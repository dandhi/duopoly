Imports System.DirectoryServices.ActiveDirectory

Public Class formGameOver
    Public Sub formGameOver_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lbl1st.Text = Form1.places(3)
        lbl2nd.Text = Form1.places(2)
        lbl3rd.Text = Form1.places(1)
        lbl4th.Text = "4th - " + Form1.places(0)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        Dim filePath As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\save.txt"
        System.IO.File.WriteAllText(filePath, String.Empty) 'Clears the file contents

        Start.Show()
        Me.Dispose()
    End Sub
End Class