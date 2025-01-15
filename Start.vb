Public Class Start
    Private Sub btnRoll_Click(sender As Object, e As EventArgs) Handles btnRoll.Click
        Form1.Show()
        Me.Dispose()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

    Private Sub Start_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim filePath As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\save.txt" 'Replace with your file path
        Dim lineCount As Integer = 0

        Using sr As New IO.StreamReader(filePath)
            While Not sr.EndOfStream
                sr.ReadLine()
                lineCount += 1
            End While
        End Using

        If lineCount > 1 Then
            btnContinue.Enabled = True
        End If

    End Sub

    Private Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        Form1.Show()
        Form1.loaded = True
        Form1.Form1_Load(sender, e)
        Me.Dispose()
    End Sub
End Class