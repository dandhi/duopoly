<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Start
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        btnContinue = New Button()
        btnRoll = New Button()
        Button1 = New Button()
        Label1 = New Label()
        SuspendLayout()
        ' 
        ' btnContinue
        ' 
        btnContinue.BackColor = Color.FromArgb(CByte(7), CByte(11), CByte(14))
        btnContinue.Cursor = Cursors.No
        btnContinue.Enabled = False
        btnContinue.FlatAppearance.BorderColor = Color.FromArgb(CByte(7), CByte(11), CByte(14))
        btnContinue.FlatAppearance.BorderSize = 0
        btnContinue.FlatStyle = FlatStyle.Flat
        btnContinue.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnContinue.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        btnContinue.Location = New Point(198, 256)
        btnContinue.Margin = New Padding(2)
        btnContinue.Name = "btnContinue"
        btnContinue.Size = New Size(200, 25)
        btnContinue.TabIndex = 94
        btnContinue.Text = "Continue"
        btnContinue.UseVisualStyleBackColor = False
        ' 
        ' btnRoll
        ' 
        btnRoll.AccessibleRole = AccessibleRole.Sound
        btnRoll.BackColor = Color.FromArgb(CByte(7), CByte(11), CByte(14))
        btnRoll.FlatAppearance.BorderSize = 0
        btnRoll.FlatStyle = FlatStyle.Flat
        btnRoll.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRoll.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        btnRoll.Location = New Point(198, 227)
        btnRoll.Margin = New Padding(2)
        btnRoll.Name = "btnRoll"
        btnRoll.Size = New Size(200, 25)
        btnRoll.TabIndex = 93
        btnRoll.Text = "New Game"
        btnRoll.UseVisualStyleBackColor = False
        ' 
        ' Button1
        ' 
        Button1.BackColor = Color.FromArgb(CByte(7), CByte(11), CByte(14))
        Button1.Cursor = Cursors.No
        Button1.FlatAppearance.BorderColor = Color.FromArgb(CByte(7), CByte(11), CByte(14))
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Flat
        Button1.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button1.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        Button1.Location = New Point(198, 285)
        Button1.Margin = New Padding(2)
        Button1.Name = "Button1"
        Button1.Size = New Size(200, 25)
        Button1.TabIndex = 95
        Button1.Text = "Quit to Desktop"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 38F, FontStyle.Bold)
        Label1.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        Label1.Location = New Point(162, 139)
        Label1.Name = "Label1"
        Label1.Size = New Size(275, 68)
        Label1.TabIndex = 96
        Label1.Text = "Monopoly"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Start
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(71), CByte(23), CByte(143))
        ClientSize = New Size(584, 440)
        Controls.Add(Label1)
        Controls.Add(Button1)
        Controls.Add(btnContinue)
        Controls.Add(btnRoll)
        Name = "Start"
        Text = "Start"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnContinue As Button
    Friend WithEvents btnRoll As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
End Class
