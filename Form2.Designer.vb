<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formGameOver
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(formGameOver))
        btnContinue = New Button()
        PictureBox3 = New PictureBox()
        PictureBox1 = New PictureBox()
        PictureBox2 = New PictureBox()
        PictureBox4 = New PictureBox()
        lbl4th = New Label()
        lbl2nd = New Label()
        lbl3rd = New Label()
        lbl1st = New Label()
        pbxDie2 = New PictureBox()
        PictureBox5 = New PictureBox()
        PictureBox6 = New PictureBox()
        PictureBox7 = New PictureBox()
        Label1 = New Label()
        CType(PictureBox3, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox4, ComponentModel.ISupportInitialize).BeginInit()
        CType(pbxDie2, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox5, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox6, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox7, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnContinue
        ' 
        btnContinue.BackColor = Color.FromArgb(CByte(4), CByte(11), CByte(14))
        btnContinue.FlatAppearance.BorderSize = 0
        btnContinue.FlatStyle = FlatStyle.Flat
        btnContinue.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnContinue.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        btnContinue.Location = New Point(448, 399)
        btnContinue.Margin = New Padding(2)
        btnContinue.Name = "btnContinue"
        btnContinue.Size = New Size(120, 25)
        btnContinue.TabIndex = 95
        btnContinue.Text = "Continue"
        btnContinue.UseVisualStyleBackColor = False
        ' 
        ' PictureBox3
        ' 
        PictureBox3.BackColor = Color.FromArgb(CByte(71), CByte(23), CByte(143))
        PictureBox3.Location = New Point(-5, 382)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(594, 61)
        PictureBox3.TabIndex = 96
        PictureBox3.TabStop = False
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.White
        PictureBox1.Location = New Point(240, 183)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(120, 260)
        PictureBox1.TabIndex = 97
        PictureBox1.TabStop = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.BackColor = Color.Gainsboro
        PictureBox2.Location = New Point(120, 223)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(120, 220)
        PictureBox2.TabIndex = 98
        PictureBox2.TabStop = False
        ' 
        ' PictureBox4
        ' 
        PictureBox4.BackColor = Color.Gainsboro
        PictureBox4.Location = New Point(360, 263)
        PictureBox4.Name = "PictureBox4"
        PictureBox4.Size = New Size(120, 180)
        PictureBox4.TabIndex = 99
        PictureBox4.TabStop = False
        ' 
        ' lbl4th
        ' 
        lbl4th.BackColor = Color.FromArgb(CByte(71), CByte(23), CByte(143))
        lbl4th.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold)
        lbl4th.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        lbl4th.Location = New Point(240, 399)
        lbl4th.Name = "lbl4th"
        lbl4th.Size = New Size(120, 25)
        lbl4th.TabIndex = 100
        lbl4th.Text = "4th - CPU3"
        lbl4th.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' lbl2nd
        ' 
        lbl2nd.BackColor = Color.FromArgb(CByte(37), CByte(7), CByte(107))
        lbl2nd.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold)
        lbl2nd.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        lbl2nd.Location = New Point(120, 195)
        lbl2nd.Name = "lbl2nd"
        lbl2nd.Size = New Size(120, 25)
        lbl2nd.TabIndex = 101
        lbl2nd.Text = "CPU1"
        lbl2nd.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' lbl3rd
        ' 
        lbl3rd.BackColor = Color.FromArgb(CByte(37), CByte(7), CByte(107))
        lbl3rd.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold)
        lbl3rd.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        lbl3rd.Location = New Point(360, 235)
        lbl3rd.Name = "lbl3rd"
        lbl3rd.Size = New Size(120, 25)
        lbl3rd.TabIndex = 102
        lbl3rd.Text = "CPU2"
        lbl3rd.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' lbl1st
        ' 
        lbl1st.BackColor = Color.FromArgb(CByte(212), CByte(175), CByte(55))
        lbl1st.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold)
        lbl1st.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        lbl1st.Location = New Point(276, 163)
        lbl1st.Name = "lbl1st"
        lbl1st.Size = New Size(48, 17)
        lbl1st.TabIndex = 103
        lbl1st.Text = "Player"
        lbl1st.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' pbxDie2
        ' 
        pbxDie2.BackColor = Color.White
        pbxDie2.Image = CType(resources.GetObject("pbxDie2.Image"), Image)
        pbxDie2.Location = New Point(274, 183)
        pbxDie2.Name = "pbxDie2"
        pbxDie2.Size = New Size(50, 50)
        pbxDie2.SizeMode = PictureBoxSizeMode.StretchImage
        pbxDie2.TabIndex = 104
        pbxDie2.TabStop = False
        ' 
        ' PictureBox5
        ' 
        PictureBox5.BackColor = Color.Gainsboro
        PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), Image)
        PictureBox5.Location = New Point(156, 223)
        PictureBox5.Name = "PictureBox5"
        PictureBox5.Size = New Size(50, 50)
        PictureBox5.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox5.TabIndex = 105
        PictureBox5.TabStop = False
        ' 
        ' PictureBox6
        ' 
        PictureBox6.BackColor = Color.Gainsboro
        PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), Image)
        PictureBox6.Location = New Point(394, 263)
        PictureBox6.Name = "PictureBox6"
        PictureBox6.Size = New Size(50, 50)
        PictureBox6.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox6.TabIndex = 106
        PictureBox6.TabStop = False
        ' 
        ' PictureBox7
        ' 
        PictureBox7.BackColor = Color.Transparent
        PictureBox7.Image = CType(resources.GetObject("PictureBox7.Image"), Image)
        PictureBox7.Location = New Point(274, 134)
        PictureBox7.Name = "PictureBox7"
        PictureBox7.Size = New Size(50, 50)
        PictureBox7.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox7.TabIndex = 107
        PictureBox7.TabStop = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 28F, FontStyle.Bold)
        Label1.ForeColor = Color.FromArgb(CByte(243), CByte(245), CByte(247))
        Label1.Location = New Point(189, 46)
        Label1.Name = "Label1"
        Label1.Size = New Size(221, 51)
        Label1.TabIndex = 108
        Label1.Text = "Game Over"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' formGameOver
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(37), CByte(7), CByte(107))
        ClientSize = New Size(584, 440)
        Controls.Add(Label1)
        Controls.Add(PictureBox6)
        Controls.Add(PictureBox5)
        Controls.Add(pbxDie2)
        Controls.Add(lbl1st)
        Controls.Add(lbl3rd)
        Controls.Add(lbl2nd)
        Controls.Add(lbl4th)
        Controls.Add(btnContinue)
        Controls.Add(PictureBox3)
        Controls.Add(PictureBox1)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox4)
        Controls.Add(PictureBox7)
        Name = "formGameOver"
        Text = "Form2"
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox4, ComponentModel.ISupportInitialize).EndInit()
        CType(pbxDie2, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox5, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox6, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox7, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnContinue As Button
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents lbl4th As Label
    Friend WithEvents lbl2nd As Label
    Friend WithEvents lbl3rd As Label
    Friend WithEvents pbxDie2 As PictureBox
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents PictureBox6 As PictureBox
    Friend WithEvents PictureBox7 As PictureBox
    Friend WithEvents Label1 As Label
    Public WithEvents lbl1st As Label
End Class
