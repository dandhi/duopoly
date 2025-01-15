Imports System.ComponentModel.Design.Serialization
Imports System.DirectoryServices.ActiveDirectory
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports System.Runtime.ExceptionServices
Imports System.Security.Cryptography.X509Certificates

Public Class Form1
    Public loaded As Boolean = False
    Public firstPlace As String
    Public secondPlace As String
    Public thirdPlace As String
    Public fourthPlace As String
    Public places As String() = {"Player", "CPU 1", "CPU 2", "CPU 3"}
    ' Turn
    Dim turn As Integer = 0
    ' Movement
    Dim rolled As Boolean = False
    Dim direction As String() = {0, 0, 0, 0} ' 0 = left, 1 = down, 2 = right, 3 = up.
    Dim doubles As Integer = 0 ' Number of doubles rolled in a row.
    Dim position As String() = {0, 0, 0, 0} ' Current position of players.
    ' Jail
    Dim jailed As Array = {False, False, False, False}
    Dim sentence As Array = {1, 1, 1, 1}
    Dim jailFree As Array = {False, False, False, False}
    Dim jails() As PictureBox
    Dim free() As PictureBox
    ' Players
    Dim players() As PictureBox
    Dim Who As Integer = 0
    Public Lost As New List(Of Integer)
    ' Money, Rent, and Property
    Dim money As Array = {1500, 1500, 1500, 1500}
    Dim owned As Array = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    Dim price As String() = {0, 60, 0, 60, 0, 150, 100, 0, 100, 120, 0, 140, 150, 140, 160, 150, 180, 0, 180, 200, 0, 220, 0, 220, 240, 150, 260, 150, 260, 280, 0, 300, 300, 0, 320, 150, 0, 350, 0, 400}
    Dim rent As Array = {0, 2, 0, 4, 0, 0, 6, 0, 6, 8, 0, 10, 0, 10, 12, 0, 14, 0, 14, 16, 0, 18, 0, 18, 20, 0, 22, 0, 22, 24, 0, 26, 28, 0, 320, 0, 0, 350, 0, 400}
    Dim rent1 As Array = {0, 10, 0, 20, 0, 0, 30, 0, 30, 40, 0, 50, 0, 50, 60, 0, 70, 0, 70, 80, 0, 90, 0, 90, 100, 0, 110, 0, 110, 120, 0, 130, 130, 0, 150, 0, 0, 175, 0, 200}
    Dim rent2 As Array = {0, 30, 0, 60, 0, 0, 90, 0, 90, 100, 0, 150, 0, 150, 180, 0, 200, 0, 200, 220, 0, 250, 0, 250, 300, 0, 330, 0, 330, 360, 0, 390, 390, 0, 450, 0, 0, 500, 0, 600}
    Dim rent3 As Array = {0, 90, 0, 180, 0, 0, 270, 0, 270, 300, 0, 450, 0, 450, 500, 0, 550, 0, 550, 600, 0, 700, 0, 700, 750, 0, 800, 0, 800, 850, 0, 900, 900, 0, 1000, 0, 0, 1100, 0, 1400}
    Dim rent4 As Array = {0, 160, 0, 320, 0, 0, 400, 0, 400, 450, 0, 625, 0, 625, 700, 0, 750, 0, 750, 800, 0, 875, 0, 875, 925, 0, 975, 0, 975, 1025, 0, 1100, 1100, 0, 1200, 0, 0, 1300, 0, 1700}
    Dim rentHotel As Array = {0, 250, 0, 450, 0, 0, 550, 0, 550, 600, 0, 750, 0, 750, 900, 0, 950, 0, 950, 1000, 0, 1050, 0, 1050, 1100, 0, 1150, 0, 1150, 1200, 0, 1275, 1275, 0, 1400, 0, 0, 1500, 0, 2000}
    Dim ownedPlayer As New List(Of Integer)
    Dim ownedCPU1 As New List(Of Integer)
    Dim ownedCPU2 As New List(Of Integer)
    Dim ownedCPU3 As New List(Of Integer)

    Dim colours() As Color
    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Randomise random.
        Randomize()
        ' Add players to array.
        players = New PictureBox() {player, cpu1, cpu2, cpu3}
        jails = New PictureBox() {jailedPlayer, jailedCPU1, jailedCPU2, jailedCPU3}
        free = New PictureBox() {freePlayer, freeCPU1, freeCPU2, freeCPU3}
        ' Update colours.
        ReDim colours(0 To 3)
        colours(0) = System.Drawing.Color.FromArgb(3, 140, 255)
        colours(1) = System.Drawing.Color.FromArgb(255, 58, 59)
        colours(2) = System.Drawing.Color.FromArgb(25, 189, 18)
        colours(3) = System.Drawing.Color.FromArgb(255, 156, 1)
        ' Update money.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(3).ToString
        ' Check if loaded from save.
        If loaded = True Then
            Load()
        End If
    End Sub
    Sub Move(Total As Integer, Who As Integer)
        Dim totalRoll As Integer = Total
        While Total > 0
            If direction(Who) = 0 Then
                If players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Or players(Who).Bounds.IntersectsWith(pos10.Bounds) Then
                    players(Who).Left += 42
                Else
                    players(Who).Left += 35
                End If
            ElseIf direction(Who) = 1 Then
                If players(Who).Bounds.IntersectsWith(cornerTR.Bounds) Or players(Who).Bounds.IntersectsWith(right9.Bounds) Then
                    players(Who).Top += 42
                Else
                    players(Who).Top += 35
                End If
            ElseIf direction(Who) = 2 Then
                If players(Who).Bounds.IntersectsWith(cornerBR.Bounds) Or players(Who).Bounds.IntersectsWith(bottom9.Bounds) Then
                    players(Who).Left -= 42
                Else
                    players(Who).Left -= 35
                End If
            ElseIf direction(Who) = 3 Then
                If players(Who).Bounds.IntersectsWith(cornerBL.Bounds) Or players(Who).Bounds.IntersectsWith(left9.Bounds) Then
                    players(Who).Top -= 42
                Else
                    players(Who).Top -= 35
                End If
            End If

            Total -= 1
            position(Who) += 1
            If position(Who) > 39 Then
                position(Who) = 0
            End If

            ' Pass Go.
            If players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Then
                Bank(200)
            End If

            If players(Who).Bounds.IntersectsWith(cornerTR.Bounds) Then
                direction(Who) = 1
            ElseIf players(Who).Bounds.IntersectsWith(cornerBR.Bounds) Then
                direction(Who) = 2
            ElseIf players(Who).Bounds.IntersectsWith(cornerBL.Bounds) Then
                direction(Who) = 3
            ElseIf players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Then
                direction(Who) = 0
            End If

            If turn >= 3 Then
                turn = 0
            Else
                turn += 1
            End If
        End While

        ' Position events.
        If position(Who) = 30 Then ' Go to jail.
            Jail("Jailed")
        ElseIf position(Who) = 2 Or position(Who) = 17 Or position(Who) = 33 Then ' Community chest.
            Card("Community", totalRoll)
        ElseIf position(Who) = 7 Or position(Who) = 22 Or position(Who) = 36 Then ' Chance.
            Card("Chance", totalRoll)
        ElseIf position(Who) = 4 Then ' Income tax.
            Bank(-200)
        ElseIf position(Who) = 38 Then ' Luxury tax.
            Bank(-100)
        End If
        ' Check property
        If owned(position(Who)) > 0 Then
            If position(Who) = 12 Or position(Who) = 27 Then
                PayUtility(totalRoll)
            Else
                PayRent()
            End If
        ElseIf owned(position(Who)) = 0 Then
            If Who > 0 And money(Who) >= price(position(Who)) And price(position(Who)) > 0 Then
                Buy()
            ElseIf Who = 0 And money(Who) >= price(position(Who)) And price(position(Who)) > 0 Then
                btnBuy.Enabled = True
            End If
        End If
        ' Update leaderboard via bubble sort algorithm
        Dim tempMoney As Array = money()
        Dim tempTempMoney As Integer
        Dim tempPlace As String
        Dim last As Integer = tempMoney.Length - 1
        Dim swapped As Boolean = True
        Dim i As Integer
        While swapped = True
            swapped = False
            i = 0
            While i < last
                If tempMoney(i) > tempMoney(i + 1) Then
                    tempTempMoney = tempMoney(i)
                    tempMoney(i) = tempMoney(i + 1)
                    tempMoney(i + 1) = tempTempMoney
                    tempPlace = places(i)
                    places(i) = places(i + 1)
                    places(i + 1) = tempPlace
                    swapped = True
                End If
                i += 1
            End While
            last += 1
        End While
        ' Next
        If rolled = False Then
            If Who > 0 Then
                tmrReroll.Start()
            End If
        ElseIf Who = 0 Then
            btnEndTurn.Enabled = True
        Else
            tmrCooldown.Start()
        End If
    End Sub

    Sub Jail(Status As String)
        If Status = "Jailed" Then
            If jailFree(Who) = True Then
                free(Who).Enabled = False
                jailFree(Who) = False
            Else
                players(Who).Location = New Point(376, 40)
                jailed(Who) = True
                sentence(Who) = 2
                players(Who).Image = lstJail.Images(0)
            End If
        Else Status = "Free"
            jailed(Who) = False
            sentence(Who) = 0
            players(Who).Location = New Point(389, 25)
            direction(Who) = 1
            position(Who) = 10
            players(Who).Image = Nothing
        End If
        jails(Who).Visible = Not jails(Who).Visible
    End Sub

    Sub roll()
        rolled = False

        Dim Roll1 As Integer = Int((6) * Rnd() + 1)
        Dim Roll2 As Integer = Int((6) * Rnd() + 1)

        pbxDie1.Image = lstDie.Images(Roll1 - 1)
        pbxDie2.Image = lstDie.Images(Roll2 - 1)

        If jailed(Who) = True Then
            If Roll1 = Roll2 Then
                Jail("Free")
            Else
                sentence(Who) -= 1
                If sentence(Who) = 0 Then
                    Jail("Free")
                End If
            End If
            btnRoll.Enabled = False
            tmrCooldown.Start()
        ElseIf rolled = False Then
            If Roll1 = Roll2 Then
                doubles += 1
                btnRoll.Text = "Reroll"
                If doubles >= 3 Then
                    rolled = True
                    btnRoll.Text = "Roll"
                    doubles = 0
                    btnRoll.Enabled = False
                    tmrCooldown.Start()
                    Jail("Jailed")
                Else
                    Move(Roll1 + Roll2, Who)
                End If
            Else
                doubles = 0
                rolled = True
                btnRoll.Text = "Roll"
                btnRoll.Enabled = False
                Move(Roll1 + Roll2, Who)
            End If
        End If
    End Sub
    Sub Bank(bank As Integer)
        ' 0 = player, 1 = cpu1, 2 = cpu2, 3 = cpu3
        money(Who) += bank
        ' Update money.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(3).ToString
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnRoll.Click
        roll()
    End Sub
    Private Sub tmrCooldown_Tick(sender As Object, e As EventArgs) Handles tmrCooldown.Tick
        tmrCooldown.Stop()
        If Who >= 3 Then
            Who = 0
        Else
            Who += 1
            While Lost.Contains(Who)
                If Who >= 3 Then
                    Who = 0
                    turn = 0
                Else
                    Who += 1
                    turn += 1
                End If
            End While
        End If

        If Who = 0 Then
            btnRoll.Enabled = True
            btnRoll.Text = "Roll"
            If ownedPlayer.Count > 0 Then
                btnSell.Enabled = True
            End If

            ' Nooooo more upgrade hell.
            If ownedPlayer.Contains(1) And ownedPlayer.Contains(3) Then
                If owned(1) > 0 And owned(1) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(3) > 0 And owned(3) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(6) And ownedPlayer.Contains(8) And ownedPlayer.Contains(9) Then
                If owned(6) > 0 And owned(6) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(8) > 0 And owned(8) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(9) > 0 And owned(9) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(11) And ownedPlayer.Contains(13) And ownedPlayer.Contains(14) Then
                If owned(11) > 0 And owned(11) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(13) > 0 And owned(13) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(14) > 0 And owned(14) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(16) And ownedPlayer.Contains(18) And ownedPlayer.Contains(19) Then
                If owned(16) > 0 And owned(16) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(18) > 0 And owned(18) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(19) > 0 And owned(19) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(21) And ownedPlayer.Contains(23) And ownedPlayer.Contains(24) Then
                If owned(21) > 0 And owned(21) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(23) > 0 And owned(23) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(24) > 0 And owned(24) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(26) And ownedPlayer.Contains(28) And ownedPlayer.Contains(29) Then
                If owned(26) > 0 And owned(26) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(28) > 0 And owned(28) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(29) > 0 And owned(29) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(31) And ownedPlayer.Contains(32) And ownedPlayer.Contains(34) Then
                If owned(31) > 0 And owned(31) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(32) > 0 And owned(32) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(34) > 0 And owned(34) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If

            If ownedPlayer.Contains(37) And ownedPlayer.Contains(39) Then
                If owned(37) > 0 And owned(37) < 6 Then
                    btnUpgrade.Enabled = True
                End If
                If owned(39) > 0 And owned(39) < 6 Then
                    btnUpgrade.Enabled = True
                End If
            End If
        Else
            btnRoll.Enabled = False
            roll()
        End If
    End Sub

    Sub Buy()
        money(Who) -= price(position(Who))
        owned(position(Who)) = Who * 10 + 1
        btnBuy.Enabled = False
        ' Update money labels.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(2).ToString
        ' Update property colour.
        If position(Who) = 1 Then
            prop01.BackColor = colours(Who)
            price01.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 3 Then
            prop02.BackColor = colours(Who)
            price02.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 6 Then
            prop03.BackColor = colours(Who)
            price03.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 8 Then
            prop04.BackColor = colours(Who)
            price04.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 9 Then
            prop05.BackColor = colours(Who)
            price05.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 11 Then
            prop06.BackColor = colours(Who)
            price06.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 13 Then
            prop07.BackColor = colours(Who)
            price07.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 14 Then
            prop08.BackColor = colours(Who)
            price08.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 16 Then
            prop09.BackColor = colours(Who)
            price09.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 18 Then
            prop10.BackColor = colours(Who)
            price10.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 19 Then
            prop11.BackColor = colours(Who)
            price11.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 21 Then
            prop12.BackColor = colours(Who)
            price12.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 23 Then
            prop13.BackColor = colours(Who)
            price13.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 24 Then
            prop14.BackColor = colours(Who)
            price14.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 26 Then
            prop15.BackColor = colours(Who)
            price15.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 28 Then
            prop16.BackColor = colours(Who)
            price16.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 29 Then
            prop17.BackColor = colours(Who)
            price17.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 31 Then
            prop18.BackColor = colours(Who)
            price18.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 32 Then
            prop19.BackColor = colours(Who)
            price19.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 34 Then
            prop20.BackColor = colours(Who)
            price20.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 37 Then
            prop21.BackColor = colours(Who)
            price21.Text = "$" + rent(position(Who)).ToString
        ElseIf position(Who) = 39 Then
            prop22.BackColor = colours(Who)
            price22.Text = "$" + rent(position(Who)).ToString
        End If
        ' Add property to personal owned array.
        If Who = 0 Then
            ownedPlayer.Add(position(Who))
        ElseIf Who = 1 Then
            ownedCPU1.Add(position(Who))
        ElseIf Who = 2 Then
            ownedCPU2.Add(position(Who))
        ElseIf Who = 3 Then
            ownedCPU3.Add(position(Who))
        End If
        ' Enable upgrade and sell.
        If Who = 0 Then
            btnSell.Enabled = True
        End If
        If ownedPlayer.Contains(1) And ownedPlayer.Contains(3) Then
            If owned(1) > 0 And owned(1) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(3) > 0 And owned(3) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(6) And ownedPlayer.Contains(8) And ownedPlayer.Contains(9) Then
            If owned(6) > 0 And owned(6) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(8) > 0 And owned(8) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(9) > 0 And owned(9) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(11) And ownedPlayer.Contains(13) And ownedPlayer.Contains(14) Then
            If owned(11) > 0 And owned(11) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(13) > 0 And owned(13) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(14) > 0 And owned(14) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(16) And ownedPlayer.Contains(18) And ownedPlayer.Contains(19) Then
            If owned(16) > 0 And owned(16) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(18) > 0 And owned(18) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(19) > 0 And owned(19) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(21) And ownedPlayer.Contains(23) And ownedPlayer.Contains(24) Then
            If owned(21) > 0 And owned(21) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(23) > 0 And owned(23) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(24) > 0 And owned(24) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(26) And ownedPlayer.Contains(28) And ownedPlayer.Contains(29) Then
            If owned(26) > 0 And owned(26) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(28) > 0 And owned(28) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(29) > 0 And owned(29) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(31) And ownedPlayer.Contains(32) And ownedPlayer.Contains(34) Then
            If owned(31) > 0 And owned(31) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(32) > 0 And owned(32) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(34) > 0 And owned(34) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
        If ownedPlayer.Contains(37) And ownedPlayer.Contains(39) Then
            If owned(37) > 0 And owned(37) < 6 Then
                btnUpgrade.Enabled = True
            End If
            If owned(39) > 0 And owned(39) < 6 Then
                btnUpgrade.Enabled = True
            End If
        End If
    End Sub
    Sub Card(Card As String, totalRoll As Integer)
        If Card = "Chance" Then
            Dim filePath As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\chance.txt"
            Dim lines As String() = File.ReadAllLines(filePath)
            Dim rand As New Random()
            Dim randomIndex As Integer = rand.Next(lines.Length)
            lblEvent.Text = lines(randomIndex)

            If randomIndex = 0 Then
                ' Advance to Go (Collect $200)
                players(Who).Location = New Point(25, 25)
                position(Who) = 0
            ElseIf randomIndex = 1 Then
                'Advance to Illinois Ave—If you pass Go, collect $200
                If position(Who) > 24 Then
                    money(Who) += 200
                End If
                position(Who) = 24
                players(Who).Location = New Point(207, 390)
            ElseIf randomIndex = 2 Then
                ' Advance to St. Charles Place – If you pass Go, collect $200
                If position(Who) > 11 Then
                    money(Who) += 200
                End If
                position(Who) = 11
                players(Who).Location = New Point(389, 69)
            ElseIf randomIndex = 3 Then
                ' Advance token to random Utility. If unowned, you may buy it from the Bank. If owned, pay the rent.
                Dim utility As Integer = Int((2) * Rnd() + 1)
                If utility = 1 Then
                    players(Who).Location = New Point(389, 102)
                ElseIf utility = 2 Then
                    players(Who).Location = New Point(139, 390)
                End If
                If owned(position(Who)) > 0 Then
                    btnBuy.Enabled = True
                Else
                    PayUtility(totalRoll)
                End If
            ElseIf randomIndex = 4 Then
                ' Advance token to random Railroad And pay owner twice the rental To which they are otherwise entitled. If Railroad Is unowned, you may buy it from the Bank.
                Dim railroad As Integer = Int((4) * Rnd() + 1)
                If railroad = 1 Then
                    players(Who).Location = New Point(207, 25)
                ElseIf railroad = 2 Then
                    players(Who).Location = New Point(389, 207)
                ElseIf railroad = 3 Then
                    players(Who).Location = New Point(209, 390)
                ElseIf railroad = 4 Then
                    players(Who).Location = New Point(25, 209)
                End If
                If owned(position(Who)) > 0 Then
                    btnBuy.Enabled = True
                Else
                    PayUtility(totalRoll)
                End If
            ElseIf randomIndex = 5 Then
                ' Bank pays you dividend of $50
                money(Who) += 50
            ElseIf randomIndex = 6 Then
                ' Get Out of Jail Free
                jailFree(Who) = True
                free(Who).Visible = True
            ElseIf randomIndex = 7 Then
                ' Go Back 3 Spaces
                Dim Total As Integer = 3
                While Total > 0
                    If direction(Who) = 0 Then
                        If players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Or players(Who).Bounds.IntersectsWith(pos10.Bounds) Then
                            players(Who).Left -= 42
                        Else
                            players(Who).Left -= 35
                        End If
                    ElseIf direction(Who) = 1 Then
                        If players(Who).Bounds.IntersectsWith(cornerTR.Bounds) Or players(Who).Bounds.IntersectsWith(right9.Bounds) Then
                            players(Who).Top -= 42
                        Else
                            players(Who).Top -= 35
                        End If
                    ElseIf direction(Who) = 2 Then
                        If players(Who).Bounds.IntersectsWith(cornerBR.Bounds) Or players(Who).Bounds.IntersectsWith(bottom9.Bounds) Then
                            players(Who).Left += 42
                        Else
                            players(Who).Left += 35
                        End If
                    ElseIf direction(Who) = 3 Then
                        If players(Who).Bounds.IntersectsWith(cornerBL.Bounds) Or players(Who).Bounds.IntersectsWith(left9.Bounds) Then
                            players(Who).Top += 42
                        Else
                            players(Who).Top += 35
                        End If
                    End If

                    Total -= 1
                    position(Who) += 1
                    If position(Who) > 39 Then
                        position(Who) = 0
                    End If

                    ' Pass Go.
                    If players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Then
                        Bank(200)
                    End If

                    If players(Who).Bounds.IntersectsWith(cornerTR.Bounds) Then
                        direction(Who) = 1
                    ElseIf players(Who).Bounds.IntersectsWith(cornerBR.Bounds) Then
                        direction(Who) = 2
                    ElseIf players(Who).Bounds.IntersectsWith(cornerBL.Bounds) Then
                        direction(Who) = 3
                    ElseIf players(Who).Bounds.IntersectsWith(cornerTL.Bounds) Then
                        direction(Who) = 0
                    End If

                    If turn >= 3 Then
                        turn = 0
                    Else
                        turn += 1
                    End If
                End While
                ' Position events.
                If position(Who) = 30 Then ' Go to jail.
                    Jail("Jailed")
                ElseIf position(Who) = 7 Or position(Who) = 22 Or position(Who) = 36 Then ' Chance.
                ElseIf position(Who) = 4 Then ' Income tax.
                    Bank(-200)
                ElseIf position(Who) = 38 Then ' Luxury tax.
                    Bank(-100)
                End If
                ' Check property
                If owned(position(Who)) > 0 Then
                    PayRent()
                ElseIf owned(position(Who)) = 0 Then
                    If Who > 0 And money(Who) >= price(position(Who)) And price(position(Who)) > 0 Then
                        Buy()
                    ElseIf Who = 0 And money(Who) >= price(position(Who)) And price(position(Who)) > 0 Then
                        btnBuy.Enabled = True
                    End If
                End If
            ElseIf randomIndex = 8 Then
                ' Go to Jail–Go directly to Jail–Do not pass Go, do not collect $200
                Jail("Jailed")
            ElseIf randomIndex = 9 Then
                ' Make general repairs on all your property–For each house pay $25–For each hotel $100
                If Who = 0 Then
                    money(Who) -= ownedPlayer.Count * 25
                ElseIf Who = 1 Then
                    money(Who) -= ownedCPU1.Count * 25
                ElseIf Who = 2 Then
                    money(Who) -= ownedCPU2.Count * 25
                ElseIf Who = 3 Then
                    money(Who) -= ownedCPU3.Count * 25
                End If
            ElseIf randomIndex = 10 Then
                ' Pay poor tax of $15
                money(Who) -= 15
            ElseIf randomIndex = 11 Then
                ' Take a trip To Reading Railroad–If you pass Go, collect $200
                If position(Who) > 5 Then
                    money(Who) += 200
                End If
                position(Who) = 5
                players(Who).Location = New Point(207, 25)
            ElseIf randomIndex = 12 Then
                ' Take a walk on the Boardwalk–Advance token to Boardwalk
                If position(Who) > 39 Then
                    money(Who) += 200
                End If
                position(Who) = 39
                players(Who).Location = New Point(25, 69)
            ElseIf randomIndex = 13 Then
                ' You have been elected Chairman of the Board–Pay each player $50
                Dim toTake As Integer = Who + 1
                If toTake > 3 Then
                    toTake = 0
                End If
                While toTake <> Who
                    If Lost.Contains(toTake) Then
                    Else
                        money(toTake) += 50
                        money(Who) -= 50
                    End If
                    If toTake >= 3 Then
                        toTake = 0
                    Else
                        toTake += 1
                    End If
                End While
            ElseIf randomIndex = 14 Then
                ' Your building and loan matures—Collect $150
                money(Who) += 150
            ElseIf randomIndex = 15 Then
                ' You have won a crossword competition—Collect $100
                money(Who) += 100
            End If
        ElseIf Card = "Community" Then
            Dim filePath As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\community-chest.txt"
            Dim lines As String() = File.ReadAllLines(filePath)
            Dim rand As New Random()
            Dim randomIndex As Integer = rand.Next(lines.Length)
            lblEvent.Text = lines(randomIndex)

            If randomIndex = 0 Then
                ' Advance to Go (Collect $200)
                players(Who).Location = New Point(25, 25)
                position(Who) = 0
                money(Who) += 200
            ElseIf randomIndex = 1 Then
                ' Bank error in your favor—Collect $200
                money(Who) += 200
            ElseIf randomIndex = 2 Then
                ' Doctor's fee—Pay $50
                money(Who) -= 50
            ElseIf randomIndex = 3 Then
                ' From sale of stock you get $50
                money(Who) += 50
            ElseIf randomIndex = 4 Then
                ' Get out of jail free
                jailFree(Who) = True
                free(Who).Visible = True
            ElseIf randomIndex = 5 Then
                ' Go to Jail–Go directly to jail–Do not pass Go–Do not collect $200
                Jail("Jailed")
            ElseIf randomIndex = 6 Then
                ' Grand Opera Night—Collect $50 from every player for opening night seats
                Dim toTake As Integer = Who + 1
                If toTake > 3 Then
                    toTake = 0
                End If
                While toTake <> Who
                    If Lost.Contains(toTake) Then
                    Else
                        money(toTake) -= 50
                        money(Who) += 50
                    End If
                    If toTake >= 3 Then
                        toTake = 0
                    Else
                        toTake += 1
                    End If
                End While
            ElseIf randomIndex = 7 Then
                ' Holiday Fund matures—Receive $100
                money(Who) += 100
            ElseIf randomIndex = 8 Then
                ' Income tax refund–Collect $20
                money(Who) += 20
            ElseIf randomIndex = 9 Then
                ' It is your birthday—Collect $10
                money(Who) += 10
            ElseIf randomIndex = 10 Then
                ' Life insurance matures–Collect $100
                money(Who) += 100
            ElseIf randomIndex = 11 Then
                ' Pay hospital fees of $100
                money(Who) -= 100
            ElseIf randomIndex = 12 Then
                ' Pay school fees of $150
                money(Who) -= 150
            ElseIf randomIndex = 13 Then
                ' Receive $25 consultancy fee
                money(Who) += 25
            ElseIf randomIndex = 14 Then
                ' Pay $100 to the richest player
                ' Linear earching algorithm for richest player
                Dim max As Integer = money(0)
                Dim count As Integer = 0
                Dim richest As Integer = 0
                While count < money.Length
                    If max > money(count) And count <> Who Then
                        max = money(count)
                        richest = count
                    End If
                    count += 1
                End While
                money(Who) -= 100
                money(richest) += 100
            ElseIf randomIndex = 15 Then
                ' You have won second prize in a beauty contest–Collect $10
                money(Who) += 10
            ElseIf randomIndex = 16 Then
                ' You inherit $100
                money(Who) += 100
            End If
        End If
        ' Update money labels.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(3).ToString
    End Sub

    ' Pay rent for properties, utilities, and railroads and bankrupt.
    Sub PayRent()
        Dim rentPayable As Integer
        Dim rentPayTo As Integer
        ' If owned by player and not player.
        If owned(position(Who)) = 1 And Who <> 0 Then
            rentPayable = rent(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 2 And Who <> 0 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 3 And Who <> 0 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 4 And Who <> 0 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 5 And Who <> 0 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 6 And Who <> 0 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 0
            ' If owned by CPU1 and not CPU1.
        ElseIf owned(position(Who)) = 11 And Who <> 1 Then
            rentPayable = rent(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 12 And Who <> 1 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 13 And Who <> 1 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 14 And Who <> 1 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 15 And Who <> 1 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 16 And Who <> 1 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 1
            ' If owned by CPU2 and not CPU2.
        ElseIf owned(position(Who)) = 21 And Who <> 2 Then
            rentPayable = rent(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 22 And Who <> 2 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 23 And Who <> 2 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 24 And Who <> 2 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 25 And Who <> 2 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 26 And Who <> 2 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 2
            ' If owned by CPU3 and not CPU3.
        ElseIf owned(position(Who)) = 31 And Who <> 3 Then
            rentPayable = rent(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 32 And Who <> 3 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 33 And Who <> 3 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 34 And Who <> 3 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 35 And Who <> 3 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 36 And Who <> 3 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 3
        End If

        While money(Who) < rentPayable
            If Who = 0 Then
                If ownedPlayer.Count = 0 Then
                    players(0).Dispose()
                    Bankrupt()
                Else
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable
                    btnEndTurn.Enabled = False
                End If
            ElseIf Who = 1 Then
                If ownedCPU1.Count = 0 Then
                    players(1).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU1.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU1(toSell)).ToString().Last())
                    money(Who) -= price(ownedCPU1(toSell)) + 50 * upgraded ' Remove money.
                    lblMoneyCPU1.Text = "$" + money(Who).ToString  ' Upgrade money label.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU1(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU1(toSell)) = 0
                    ownedCPU1.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable

                End If
            ElseIf Who = 2 Then
                If ownedCPU2.Count = 0 Then
                    players(2).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU2.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU2(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU2(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU2(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU2(toSell)) = 0
                    ownedCPU2.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU2.Text = "$" + money(2).ToString + " - $" + rentPayable
                End If
            ElseIf Who = 3 Then
                If ownedCPU3.Count = 0 Then
                    players(3).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU3.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU3(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU3(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU3(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU3(toSell)) = 0
                    ownedCPU3.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU3.Text = "$" + money(3).ToString + " - $" + rentPayable
                End If
            End If
        End While

        If money(Who) > rentPayable Then
            money(Who) -= rentPayable
            money(rentPayTo) += rentPayable
        End If

        ' Update money.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(3).ToString

    End Sub
    Sub PayUtility(Roll As Integer)
        Dim rentPayable As Integer
        Dim rentPayTo As Integer
        If owned(position(Who)) < 10 And Who <> 0 Then
            If ownedPlayer.Contains(27 And 12) Then
                rentPayable -= 10 * Roll
            ElseIf ownedPlayer.Contains(27 Or 12) Then
                rentPayable -= 4 * Roll
            End If
            rentPayTo = 0
        ElseIf owned(position(Who)) < 20 And Who <> 1 Then
            If ownedCPU1.Contains(27 And 12) Then
                rentPayable -= 10 * Roll
            ElseIf ownedCPU1.Contains(27 Or 12) Then
                rentPayable -= 4 * Roll
            End If
            rentPayTo = 1
        ElseIf owned(position(Who)) < 30 And Who <> 2 Then
            If ownedCPU2.Contains(27 And 12) Then
                rentPayable -= 10 * Roll
            ElseIf ownedCPU2.Contains(27 Or 12) Then
                rentPayable -= 4 * Roll
            End If
            rentPayTo = 2
        ElseIf owned(position(Who)) < 40 And Who <> 3 Then
            If ownedCPU3.Contains(27 And 12) Then
                rentPayable -= 10 * Roll
            ElseIf ownedCPU3.Contains(27 Or 12) Then
                rentPayable -= 4 * Roll
            End If
            rentPayTo = 3
        End If
        ' If owned by player and not player.
        If owned(position(Who)) = 1 And Who <> 0 Then
            rentPayable = rent(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 2 And Who <> 0 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 3 And Who <> 0 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 4 And Who <> 0 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 5 And Who <> 0 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 0
        ElseIf owned(position(Who)) = 6 And Who <> 0 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 0
            ' If owned by CPU1 and not CPU1.
        ElseIf owned(position(Who)) = 11 And Who <> 1 Then
            rentPayable = rent(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 12 And Who <> 1 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 13 And Who <> 1 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 14 And Who <> 1 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 15 And Who <> 1 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 1
        ElseIf owned(position(Who)) = 16 And Who <> 1 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 1
            ' If owned by CPU2 and not CPU2.
        ElseIf owned(position(Who)) = 21 And Who <> 2 Then
            rentPayable = rent(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 22 And Who <> 2 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 23 And Who <> 2 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 24 And Who <> 2 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 25 And Who <> 2 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 2
        ElseIf owned(position(Who)) = 26 And Who <> 2 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 2
            ' If owned by CPU3 and not CPU3.
        ElseIf owned(position(Who)) = 31 And Who <> 3 Then
            rentPayable = rent(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 32 And Who <> 3 Then
            rentPayable = rent1(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 33 And Who <> 3 Then
            rentPayable = rent2(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 34 And Who <> 3 Then
            rentPayable = rent3(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 35 And Who <> 3 Then
            rentPayable = rent4(position(Who))
            rentPayTo = 3
        ElseIf owned(position(Who)) = 36 And Who <> 3 Then
            rentPayable = rentHotel(position(Who))
            rentPayTo = 3
        End If

        While money(Who) < rentPayable
            If Who = 0 Then
                If ownedPlayer.Count = 0 Then
                    players(0).Dispose()
                    Bankrupt()
                Else
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable
                    btnEndTurn.Enabled = False
                End If
            ElseIf Who = 1 Then
                If ownedCPU1.Count = 0 Then
                    players(1).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU1.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU1(toSell)).ToString().Last())
                    money(Who) -= price(ownedCPU1(toSell)) + 50 * upgraded ' Remove money.
                    lblMoneyCPU1.Text = "$" + money(Who).ToString  ' Upgrade money label.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU1(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU1(toSell)) = 0
                    ownedCPU1.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable

                End If
            ElseIf Who = 2 Then
                If ownedCPU2.Count = 0 Then
                    players(2).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU2.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU2(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU2(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU2(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU2(toSell)) = 0
                    ownedCPU2.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU2.Text = "$" + money(2).ToString + " - $" + rentPayable
                End If
            ElseIf Who = 3 Then
                If ownedCPU3.Count = 0 Then
                    players(3).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU3.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU3(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU3(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU3(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU3(toSell)) = 0
                    ownedCPU3.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU3.Text = "$" + money(3).ToString + " - $" + rentPayable
                End If
            End If
        End While

        If money(Who) > rentPayable Then
            money(Who) -= rentPayable
            money(rentPayTo) += rentPayable
        End If

        ' Update money labels.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(2).ToString
    End Sub
    Sub PayRailroad()
        Dim railroads As Integer = 0
        Dim rentPayTo As Integer
        If owned(position(Who)) < 10 And Who <> 0 Then
            If ownedPlayer.Contains(5) Then
                railroads += 1
            End If
            If ownedPlayer.Contains(15) Then
                railroads += 1
            End If
            If ownedPlayer.Contains(25) Then
                railroads += 1
            End If
            If ownedPlayer.Contains(35) Then
                railroads += 1
            End If
            rentPayTo = 0
        ElseIf owned(position(Who)) < 20 And Who <> 1 Then
            If ownedCPU1.Contains(5) Then
                railroads += 1
            End If
            If ownedCPU1.Contains(15) Then
                railroads += 1
            End If
            If ownedCPU1.Contains(25) Then
                railroads += 1
            End If
            If ownedCPU1.Contains(35) Then
                railroads += 1
            End If
            rentPayTo = 1
        ElseIf owned(position(Who)) < 30 And Who <> 2 Then
            If ownedCPU2.Contains(5) Then
                railroads += 1
            End If
            If ownedCPU2.Contains(15) Then
                railroads += 1
            End If
            If ownedCPU2.Contains(25) Then
                railroads += 1
            End If
            If ownedCPU2.Contains(35) Then
                railroads += 1
            End If
            rentPayTo = 2
        ElseIf owned(position(Who)) < 40 And Who <> 3 Then
            If ownedCPU3.Contains(5) Then
                railroads += 1
            End If
            If ownedCPU3.Contains(15) Then
                railroads += 1
            End If
            If ownedCPU3.Contains(25) Then
                railroads += 1
            End If
            If ownedCPU3.Contains(35) Then
                railroads += 1
            End If
            rentPayTo = 4
        End If
        Dim rentPayable As Integer
        If railroads = 1 Then
            rentPayable = 25
        ElseIf railroads = 2 Then
            rentPayable = 50
        ElseIf railroads = 3 Then
            rentPayable = 100
        ElseIf railroads = 4 Then
            rentPayable = 200
        End If

        While money(Who) < rentPayable
            If Who = 0 Then
                If ownedPlayer.Count = 0 Then
                    players(0).Dispose()
                    Bankrupt()
                Else
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable
                    btnEndTurn.Enabled = False
                End If
            ElseIf Who = 1 Then
                If ownedCPU1.Count = 0 Then
                    players(1).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU1.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU1(toSell)).ToString().Last())
                    money(Who) -= price(ownedCPU1(toSell)) + 50 * upgraded ' Remove money.
                    lblMoneyCPU1.Text = "$" + money(Who).ToString  ' Upgrade money label.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU1(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU1(toSell)) = 0
                    ownedCPU1.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU1.Text = "$" + money(1).ToString + " - $" + rentPayable

                End If
            ElseIf Who = 2 Then
                If ownedCPU2.Count = 0 Then
                    players(2).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU2.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU2(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU2(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU2(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU2(toSell)) = 0
                    ownedCPU2.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU2.Text = "$" + money(2).ToString + " - $" + rentPayable
                End If
            ElseIf Who = 3 Then
                If ownedCPU3.Count = 0 Then
                    players(3).Dispose()
                    lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
                    money(Who) = 0
                    Lost.Add(Who + 1)
                Else
                    Dim toSell As Integer = Int((ownedCPU3.Count) * Rnd() + 1)
                    Dim upgraded As Integer = Convert.ToInt32(owned(ownedCPU3(toSell)).ToString().Last())
                    money(Who) += price(ownedCPU3(toSell)) + 50 * upgraded ' Remove money.
                    ' Revert colour back
                    Dim tmpPos As Integer = ownedCPU3(toSell)
                    If tmpPos = 1 Then
                        prop01.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 3 Then
                        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
                    ElseIf tmpPos = 6 Then
                        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 8 Then
                        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 9 Then
                        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
                    ElseIf tmpPos = 11 Then
                        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 13 Then
                        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 14 Then
                        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
                    ElseIf tmpPos = 16 Then
                        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 18 Then
                        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 19 Then
                        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
                    ElseIf tmpPos = 21 Then
                        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 23 Then
                        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 24 Then
                        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
                    ElseIf tmpPos = 26 Then
                        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 28 Then
                        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 29 Then
                        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
                    ElseIf tmpPos = 31 Then
                        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 32 Then
                        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 34 Then
                        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
                    ElseIf tmpPos = 37 Then
                        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    ElseIf tmpPos = 39 Then
                        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
                    End If

                    ' Increase rent.
                    owned(ownedCPU3(toSell)) = 0
                    ownedCPU3.Remove(toSell)
                    ' Update money.
                    lblMoneyCPU3.Text = "$" + money(3).ToString + " - $" + rentPayable
                End If
            End If
        End While

        If money(Who) > rentPayable Then
            money(Who) -= rentPayable
            money(rentPayTo) += rentPayable
        End If
        ' Update money labels.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(2).ToString
    End Sub
    Sub Bankrupt()
        Lost.Add(Who + 1)
        Dim lose As Integer
        If Lost.Count < 4 Then
            While Lost.Count < 4
                lose = Int((4) * Rnd() + 1)
                If Lost.Contains(lose) Then
                Else
                    Lost.Add(lose)
                End If
            End While
        End If
        ' Place sorting algorithm.
        For i As Integer = 0 To 3
            Select Case Lost(i)
                Case 0
                    places(i) = "Player"
                Case 1
                    places(i) = "CPU 1"
                Case 2
                    places(i) = "CPU 2"
                Case 3
                    places(i) = "CPU 3"
            End Select
        Next



        formGameOver.Show()
        Me.Dispose()
    End Sub
    ' Save and Load
    Sub Save(Quit As Boolean)
        'Dim saveOwned As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\Save\owned.txt"
        'File.WriteAllLines(saveOwned, owned.Select(Function(n) n.ToString()))
        Dim saveFile As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\save.txt"
        Dim fileStream As New FileStream(saveFile, FileMode.Create, FileAccess.Write)
        Dim streamWriter As New StreamWriter(fileStream)

        ' Write 'turn' to new line.
        streamWriter.Write(turn.ToString())
        streamWriter.WriteLine()
        ' Write 'rolled' to new line.
        streamWriter.Write(rolled.ToString())
        streamWriter.WriteLine()
        ' Write 'direction' to new line.
        For Each item As String In direction
            streamWriter.Write(item & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'doubles' to new line.
        streamWriter.Write(doubles.ToString())
        streamWriter.WriteLine()
        ' Write 'position' to new line.
        For Each item As Integer In position
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'jailed' to new line.
        For Each item As Boolean In jailed
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'sentence' to new line.
        For Each item As Integer In sentence
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'jailFree' to new line.
        For Each item As Boolean In jailFree
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'Who' to new line.
        streamWriter.Write(Who.ToString())
        streamWriter.WriteLine()
        ' Write 'lost' to new line.
        For Each item As Integer In Lost
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'money' to new line.
        For Each item As Integer In money
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'owned' to new line.
        For Each item As Integer In owned
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'price' to new line.
        For Each item As Integer In price
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rent' to new line.
        For Each item As Integer In rent
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rent1' to new line.
        For Each item As Integer In rent1
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rent2' to new line.
        For Each item As Integer In rent2
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rent3' to new line.
        For Each item As Integer In rent3
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rent4' to new line.
        For Each item As Integer In rent4
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'rentHotel' to new line.
        For Each item As Integer In rentHotel
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'ownedPlayer' to new line.
        For Each item As Integer In ownedPlayer
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'ownedCPU1' to new line.
        For Each item As Integer In ownedCPU1
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'ownedCPU2' to new line.
        For Each item As Integer In ownedCPU2
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()
        ' Write 'ownedCPU3' to new line.
        For Each item As Integer In ownedCPU3
            streamWriter.Write(item.ToString() & " ")
        Next
        streamWriter.WriteLine()

        streamWriter.Close()
        fileStream.Close()

        If Quit = True Then
            Me.Dispose()
        End If
    End Sub
    Sub Load()
        Dim saveFile As String = "C:\Users\Win10-VIRT\Source\Repos\Duopoly\save.txt"
        Dim fileStream As New FileStream(saveFile, FileMode.Open, FileAccess.Read)
        Dim streamReader As New StreamReader(fileStream)

        ' Read line to 'turn'.
        turn = Integer.Parse(streamReader.ReadLine())
        ' Read line to 'rolled'.
        rolled = Boolean.Parse(streamReader.ReadLine())
        ' Read line to 'direction'.
        Dim directionLine As String = streamReader.ReadLine()
        direction = directionLine.Split(" ")
        ' Read line to 'doubles'.
        doubles = Integer.Parse(streamReader.ReadLine())
        ' Read line to 'position'
        Dim positionLine As String = streamReader.ReadLine()
        position = positionLine.Split(" ")
        Dim tempPosition As Array = positionLine.Split(" ")
        While tempPosition(0) > 0
            If direction(0) = 0 Then
                If players(0).Bounds.IntersectsWith(cornerTL.Bounds) Or players(0).Bounds.IntersectsWith(pos10.Bounds) Then
                    players(0).Left += 42
                Else
                    players(0).Left += 35
                End If
            ElseIf direction(Who) = 1 Then
                If players(0).Bounds.IntersectsWith(cornerTR.Bounds) Or players(0).Bounds.IntersectsWith(right9.Bounds) Then
                    players(0).Top += 42
                Else
                    players(0).Top += 35
                End If
            ElseIf direction(0) = 2 Then
                If players(0).Bounds.IntersectsWith(cornerBR.Bounds) Or players(0).Bounds.IntersectsWith(bottom9.Bounds) Then
                    players(0).Left -= 42
                Else
                    players(0).Left -= 35
                End If
            ElseIf direction(0) = 3 Then
                If players(0).Bounds.IntersectsWith(cornerBL.Bounds) Or players(0).Bounds.IntersectsWith(left9.Bounds) Then
                    players(0).Top -= 42
                Else
                    players(0).Top -= 35
                End If
            End If
            tempPosition(0) -= 1
        End While
        While tempPosition(1) > 0
            If direction(1) = 0 Then
                If players(1).Bounds.IntersectsWith(cornerTL.Bounds) Or players(1).Bounds.IntersectsWith(pos10.Bounds) Then
                    players(1).Left += 42
                Else
                    players(1).Left += 35
                End If
            ElseIf direction(Who) = 1 Then
                If players(1).Bounds.IntersectsWith(cornerTR.Bounds) Or players(1).Bounds.IntersectsWith(right9.Bounds) Then
                    players(1).Top += 42
                Else
                    players(1).Top += 35
                End If
            ElseIf direction(1) = 2 Then
                If players(1).Bounds.IntersectsWith(cornerBR.Bounds) Or players(1).Bounds.IntersectsWith(bottom9.Bounds) Then
                    players(1).Left -= 42
                Else
                    players(1).Left -= 35
                End If
            ElseIf direction(1) = 3 Then
                If players(1).Bounds.IntersectsWith(cornerBL.Bounds) Or players(1).Bounds.IntersectsWith(left9.Bounds) Then
                    players(1).Top -= 42
                Else
                    players(1).Top -= 35
                End If
            End If
            tempPosition(1) -= 1
        End While
        While tempPosition(2) > 0
            If direction(2) = 0 Then
                If players(2).Bounds.IntersectsWith(cornerTL.Bounds) Or players(2).Bounds.IntersectsWith(pos10.Bounds) Then
                    players(2).Left += 42
                Else
                    players(2).Left += 35
                End If
            ElseIf direction(Who) = 1 Then
                If players(2).Bounds.IntersectsWith(cornerTR.Bounds) Or players(2).Bounds.IntersectsWith(right9.Bounds) Then
                    players(2).Top += 42
                Else
                    players(2).Top += 35
                End If
            ElseIf direction(2) = 2 Then
                If players(2).Bounds.IntersectsWith(cornerBR.Bounds) Or players(2).Bounds.IntersectsWith(bottom9.Bounds) Then
                    players(2).Left -= 42
                Else
                    players(2).Left -= 35
                End If
            ElseIf direction(2) = 3 Then
                If players(2).Bounds.IntersectsWith(cornerBL.Bounds) Or players(2).Bounds.IntersectsWith(left9.Bounds) Then
                    players(2).Top -= 42
                Else
                    players(2).Top -= 35
                End If
            End If
            tempPosition(2) -= 1
        End While
        While tempPosition(3) > 0
            If direction(3) = 0 Then
                If players(3).Bounds.IntersectsWith(cornerTL.Bounds) Or players(3).Bounds.IntersectsWith(pos13.Bounds) Then
                    players(3).Left += 42
                Else
                    players(3).Left += 35
                End If
            ElseIf direction(Who) = 1 Then
                If players(3).Bounds.IntersectsWith(cornerTR.Bounds) Or players(3).Bounds.IntersectsWith(right9.Bounds) Then
                    players(3).Top += 42
                Else
                    players(3).Top += 35
                End If
            ElseIf direction(3) = 2 Then
                If players(3).Bounds.IntersectsWith(cornerBR.Bounds) Or players(3).Bounds.IntersectsWith(bottom9.Bounds) Then
                    players(3).Left -= 42
                Else
                    players(3).Left -= 35
                End If
            ElseIf direction(3) = 3 Then
                If players(3).Bounds.IntersectsWith(cornerBL.Bounds) Or players(3).Bounds.IntersectsWith(left9.Bounds) Then
                    players(3).Top -= 42
                Else
                    players(3).Top -= 35
                End If
            End If
            tempPosition(3) -= 1
        End While
        ' Read line to 'jailed'
        Dim jailedLine As String = streamReader.ReadLine()
        jailed = jailedLine.Split(" ")
        If jailed(0) = True Then
            players(0).Location = New Point(376, 40)
            players(0).Image = lstJail.Images(0)
        End If
        If jailed(1) = True Then
            players(1).Location = New Point(376, 40)
            players(1).Image = lstJail.Images(0)
        End If
        If jailed(2) = True Then
            players(2).Location = New Point(376, 40)
            players(2).Image = lstJail.Images(0)
        End If
        If jailed(3) = True Then
            players(3).Location = New Point(376, 40)
            players(3).Image = lstJail.Images(0)
        End If
        ' Read line to 'sentence'
        Dim sentenceLine As String = streamReader.ReadLine()
        sentence = sentenceLine.Split(" ")
        ' Read line to 'jailFree'
        Dim jailFreeLine As String = streamReader.ReadLine()
        jailFree = jailFreeLine.Split(" ")
        ' Read line to 'Who'
        Who = Integer.Parse(streamReader.ReadLine())
        ' Read line to 'lost'
        Dim lostLine As String = streamReader.ReadLine()
        For Each numStr As String In lostLine.Split(" ")
            If Not String.IsNullOrWhiteSpace(numStr) Then
                Lost.Add(Integer.Parse(numStr))
            End If
        Next
        Dim tempLost As Integer = Lost.Count
        Dim tempWho As Integer = 0
        While tempLost > 0
            players(Lost(tempWho)).Dispose()
            If Lost(tempWho) = 0 Then
                lblMoneyPlayer.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
            ElseIf Lost(tempWho) = 1 Then
                lblMoneyCPU1.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
            ElseIf Lost(tempWho) = 2 Then
                lblMoneyCPU2.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
            ElseIf Lost(tempWho) = 3 Then
                lblMoneyCPU3.ForeColor = System.Drawing.Color.FromArgb(71, 23, 143)
            End If
            tempLost -= 1
            tempWho += 1
        End While
        ' Read line to 'money'
        Dim moneyLine As String = streamReader.ReadLine()
        money = moneyLine.Split(" ")
        ' Read line to 'owned'
        Dim ownedLine As String = streamReader.ReadLine()
        owned = ownedLine.Split(" ")
        ' Read line to 'price'
        Dim priceLine As String = streamReader.ReadLine()
        price = priceLine.Split(" ")
        ' Read line to 'rent'
        Dim rentLine As String = streamReader.ReadLine()
        rent = rentLine.Split(" ")
        ' Read line to 'rent1'
        Dim rent1Line As String = streamReader.ReadLine()
        rent1 = rent1Line.Split(" ")
        ' Read line to 'rent2'
        Dim rent2Line As String = streamReader.ReadLine()
        rent2 = rent2Line.Split(" ")
        ' Read line to 'rent3'
        Dim rent3Line As String = streamReader.ReadLine()
        rent3 = rent3Line.Split(" ")
        ' Read line to 'rent4'
        Dim rent4Line As String = streamReader.ReadLine()
        rent4 = rent4Line.Split(" ")
        ' Read line to 'rentHotel'
        Dim rentHotelLine As String = streamReader.ReadLine()
        rentHotel = rentHotelLine.Split(" ")
        ' Read line to 'ownedPlayer'
        Dim ownedPlayerLine As String = streamReader.ReadLine().ToString
        For Each numStr As String In ownedPlayerLine.Split(" ")
            If Not String.IsNullOrWhiteSpace(numStr) Then
                ownedPlayer.Add(Integer.Parse(numStr))
            End If
        Next
        If ownedPlayer.Count > 0 Then
            If ownedPlayer.Contains(1) = True Then
                prop01.BackColor = colours(0)
                If owned(1) = 1 Then
                    price01.Text = "$" + rent(1).ToString
                ElseIf owned(1) = 2 Then
                    price01.Text = "$" + rent1(1).ToString
                ElseIf owned(1) = 3 Then
                    price01.Text = "$" + rent2(1).ToString
                ElseIf owned(1) = 4 Then
                    price01.Text = "$" + rent3(1).ToString
                ElseIf owned(1) = 5 Then
                    price01.Text = "$" + rent4(1).ToString
                ElseIf owned(1) = 6 Then
                    price01.Text = "$" + rentHotel(1).ToString
                End If
            End If
            If ownedPlayer.Contains(3) Then
                prop02.BackColor = colours(0)
                If owned(3) = 1 Then
                    price02.Text = "$" + rent(3).ToString
                ElseIf owned(3) = 2 Then
                    price02.Text = "$" + rent1(3).ToString
                ElseIf owned(3) = 3 Then
                    price02.Text = "$" + rent2(3).ToString
                ElseIf owned(3) = 4 Then
                    price02.Text = "$" + rent3(3).ToString
                ElseIf owned(3) = 5 Then
                    price02.Text = "$" + rent4(3).ToString
                ElseIf owned(3) = 6 Then
                    price02.Text = "$" + rentHotel(3).ToString
                End If
            End If
            If ownedPlayer.Contains(6) Then
                prop03.BackColor = colours(0)
                If owned(6) = 1 Then
                    price03.Text = "$" + rent(6).ToString
                ElseIf owned(6) = 2 Then
                    price03.Text = "$" + rent1(6).ToString
                ElseIf owned(6) = 3 Then
                    price03.Text = "$" + rent2(6).ToString
                ElseIf owned(6) = 4 Then
                    price03.Text = "$" + rent3(6).ToString
                ElseIf owned(6) = 5 Then
                    price03.Text = "$" + rent4(6).ToString
                ElseIf owned(6) = 6 Then
                    price03.Text = "$" + rentHotel(6).ToString
                End If
            End If
            If ownedPlayer.Contains(8) Then
                prop04.BackColor = colours(0)
                If owned(8) = 1 Then
                    price04.Text = "$" + rent(8).ToString
                ElseIf owned(8) = 2 Then
                    price04.Text = "$" + rent1(8).ToString
                ElseIf owned(8) = 3 Then
                    price04.Text = "$" + rent2(8).ToString
                ElseIf owned(8) = 4 Then
                    price04.Text = "$" + rent3(8).ToString
                ElseIf owned(8) = 5 Then
                    price04.Text = "$" + rent4(8).ToString
                ElseIf owned(8) = 6 Then
                    price04.Text = "$" + rentHotel(8).ToString
                End If
            End If
            If ownedPlayer.Contains(9) Then
                prop05.BackColor = colours(0)
                If owned(9) = 1 Then
                    price05.Text = "$" + rent(9).ToString
                ElseIf owned(9) = 2 Then
                    price05.Text = "$" + rent1(9).ToString
                ElseIf owned(9) = 3 Then
                    price05.Text = "$" + rent2(9).ToString
                ElseIf owned(9) = 4 Then
                    price05.Text = "$" + rent3(9).ToString
                ElseIf owned(9) = 5 Then
                    price05.Text = "$" + rent4(9).ToString
                ElseIf owned(9) = 6 Then
                    price05.Text = "$" + rentHotel(9).ToString
                End If
            End If
            If ownedPlayer.Contains(11) Then
                prop06.BackColor = colours(0)
                price06.Text = "$" + rent(11).ToString
                If owned(11) = 1 Then
                    price06.Text = "$" + rent(11).ToString
                ElseIf owned(11) = 2 Then
                    price06.Text = "$" + rent1(11).ToString
                ElseIf owned(11) = 3 Then
                    price06.Text = "$" + rent2(11).ToString
                ElseIf owned(11) = 4 Then
                    price06.Text = "$" + rent3(11).ToString
                ElseIf owned(11) = 5 Then
                    price06.Text = "$" + rent4(11).ToString
                ElseIf owned(11) = 6 Then
                    price06.Text = "$" + rentHotel(11).ToString
                End If
            End If
            If ownedPlayer.Contains(13) Then
                prop07.BackColor = colours(0)
                If owned(13) = 1 Then
                    price07.Text = "$" + rent(13).ToString
                ElseIf owned(13) = 2 Then
                    price07.Text = "$" + rent1(13).ToString
                ElseIf owned(13) = 3 Then
                    price07.Text = "$" + rent2(13).ToString
                ElseIf owned(13) = 4 Then
                    price07.Text = "$" + rent3(13).ToString
                ElseIf owned(13) = 5 Then
                    price07.Text = "$" + rent4(13).ToString
                ElseIf owned(13) = 6 Then
                    price07.Text = "$" + rentHotel(13).ToString
                End If
            End If
            If ownedPlayer.Contains(14) Then
                prop08.BackColor = colours(0)
                If owned(14) = 1 Then
                    price08.Text = "$" + rent(14).ToString
                ElseIf owned(14) = 2 Then
                    price08.Text = "$" + rent1(14).ToString
                ElseIf owned(14) = 3 Then
                    price08.Text = "$" + rent2(14).ToString
                ElseIf owned(14) = 4 Then
                    price08.Text = "$" + rent3(14).ToString
                ElseIf owned(14) = 5 Then
                    price08.Text = "$" + rent4(14).ToString
                ElseIf owned(14) = 6 Then
                    price08.Text = "$" + rentHotel(14).ToString
                End If
            End If
            If ownedPlayer.Contains(16) Then
                prop09.BackColor = colours(0)
                If owned(16) = 1 Then
                    price09.Text = "$" + rent(16).ToString
                ElseIf owned(16) = 2 Then
                    price09.Text = "$" + rent1(16).ToString
                ElseIf owned(16) = 3 Then
                    price09.Text = "$" + rent2(16).ToString
                ElseIf owned(16) = 4 Then
                    price09.Text = "$" + rent3(16).ToString
                ElseIf owned(16) = 5 Then
                    price09.Text = "$" + rent4(16).ToString
                ElseIf owned(16) = 6 Then
                    price09.Text = "$" + rentHotel(16).ToString
                End If
            End If
            If ownedPlayer.Contains(18) Then
                prop10.BackColor = colours(0)
                If owned(18) = 1 Then
                    price10.Text = "$" + rent(18).ToString
                ElseIf owned(18) = 2 Then
                    price10.Text = "$" + rent1(18).ToString
                ElseIf owned(18) = 3 Then
                    price10.Text = "$" + rent2(18).ToString
                ElseIf owned(18) = 4 Then
                    price10.Text = "$" + rent3(18).ToString
                ElseIf owned(18) = 5 Then
                    price10.Text = "$" + rent4(18).ToString
                ElseIf owned(18) = 6 Then
                    price10.Text = "$" + rentHotel(18).ToString
                End If
            End If
            If ownedPlayer.Contains(19) Then
                prop11.BackColor = colours(0)
                If owned(19) = 1 Then
                    price11.Text = "$" + rent(19).ToString
                ElseIf owned(19) = 2 Then
                    price11.Text = "$" + rent1(19).ToString
                ElseIf owned(19) = 3 Then
                    price11.Text = "$" + rent2(19).ToString
                ElseIf owned(19) = 4 Then
                    price11.Text = "$" + rent3(19).ToString
                ElseIf owned(19) = 5 Then
                    price11.Text = "$" + rent4(19).ToString
                ElseIf owned(19) = 6 Then
                    price11.Text = "$" + rentHotel(19).ToString
                End If
            End If
            If ownedPlayer.Contains(21) Then
                prop12.BackColor = colours(0)
                If owned(21) = 1 Then
                    price12.Text = "$" + rent(21).ToString
                ElseIf owned(21) = 2 Then
                    price12.Text = "$" + rent1(21).ToString
                ElseIf owned(21) = 3 Then
                    price12.Text = "$" + rent2(21).ToString
                ElseIf owned(21) = 4 Then
                    price12.Text = "$" + rent3(21).ToString
                ElseIf owned(21) = 5 Then
                    price12.Text = "$" + rent4(21).ToString
                ElseIf owned(21) = 6 Then
                    price12.Text = "$" + rentHotel(21).ToString
                End If
            End If
                If ownedPlayer.Contains(23) Then
                prop13.BackColor = colours(0)
                If owned(23) = 1 Then
                    price13.Text = "$" + rent(23).ToString
                ElseIf owned(23) = 2 Then
                    price13.Text = "$" + rent1(23).ToString
                ElseIf owned(23) = 3 Then
                    price13.Text = "$" + rent2(23).ToString
                ElseIf owned(23) = 4 Then
                    price13.Text = "$" + rent3(23).ToString
                ElseIf owned(23) = 5 Then
                    price13.Text = "$" + rent4(23).ToString
                ElseIf owned(23) = 6 Then
                    price13.Text = "$" + rentHotel(23).ToString
                End If
            End If
                If ownedPlayer.Contains(24) Then
                prop14.BackColor = colours(0)
                If owned(24) = 1 Then
                    price14.Text = "$" + rent(24).ToString
                ElseIf owned(24) = 2 Then
                    price14.Text = "$" + rent1(24).ToString
                ElseIf owned(24) = 3 Then
                    price14.Text = "$" + rent2(24).ToString
                ElseIf owned(24) = 4 Then
                    price14.Text = "$" + rent3(24).ToString
                ElseIf owned(24) = 5 Then
                    price14.Text = "$" + rent4(24).ToString
                ElseIf owned(24) = 6 Then
                    price14.Text = "$" + rentHotel(24).ToString
                End If
            End If
                If ownedPlayer.Contains(26) Then
                prop15.BackColor = colours(0)
                If owned(26) = 1 Then
                    price15.Text = "$" + rent(26).ToString
                ElseIf owned(26) = 2 Then
                    price15.Text = "$" + rent1(26).ToString
                ElseIf owned(26) = 3 Then
                    price15.Text = "$" + rent2(26).ToString
                ElseIf owned(26) = 4 Then
                    price15.Text = "$" + rent3(26).ToString
                ElseIf owned(26) = 5 Then
                    price15.Text = "$" + rent4(26).ToString
                ElseIf owned(26) = 6 Then
                    price15.Text = "$" + rentHotel(26).ToString
                End If
            End If
                If ownedPlayer.Contains(28) Then
                prop16.BackColor = colours(0)
                If owned(28) = 1 Then
                    price16.Text = "$" + rent(28).ToString
                ElseIf owned(28) = 2 Then
                    price16.Text = "$" + rent1(28).ToString
                ElseIf owned(28) = 3 Then
                    price16.Text = "$" + rent2(28).ToString
                ElseIf owned(28) = 4 Then
                    price16.Text = "$" + rent3(28).ToString
                ElseIf owned(28) = 5 Then
                    price16.Text = "$" + rent4(28).ToString
                ElseIf owned(28) = 6 Then
                    price16.Text = "$" + rentHotel(28).ToString
                End If
            End If
                If ownedPlayer.Contains(29) Then
                prop17.BackColor = colours(0)
                If owned(29) = 1 Then
                    price17.Text = "$" + rent(29).ToString
                ElseIf owned(29) = 2 Then
                    price17.Text = "$" + rent1(29).ToString
                ElseIf owned(29) = 3 Then
                    price17.Text = "$" + rent2(29).ToString
                ElseIf owned(29) = 4 Then
                    price17.Text = "$" + rent3(29).ToString
                ElseIf owned(29) = 5 Then
                    price17.Text = "$" + rent4(29).ToString
                ElseIf owned(29) = 6 Then
                    price17.Text = "$" + rentHotel(29).ToString
                End If
            End If
                If ownedPlayer.Contains(31) Then
                prop18.BackColor = colours(0)
                If owned(31) = 1 Then
                    price18.Text = "$" + rent(31).ToString
                ElseIf owned(31) = 2 Then
                    price18.Text = "$" + rent1(31).ToString
                ElseIf owned(31) = 3 Then
                    price18.Text = "$" + rent2(31).ToString
                ElseIf owned(31) = 4 Then
                    price18.Text = "$" + rent3(31).ToString
                ElseIf owned(31) = 5 Then
                    price18.Text = "$" + rent4(31).ToString
                ElseIf owned(31) = 6 Then
                    price18.Text = "$" + rentHotel(31).ToString
                End If
            End If
                If ownedPlayer.Contains(32) Then
                prop19.BackColor = colours(0)
                If owned(32) = 1 Then
                    price19.Text = "$" + rent(32).ToString
                ElseIf owned(32) = 2 Then
                    price19.Text = "$" + rent1(32).ToString
                ElseIf owned(32) = 3 Then
                    price19.Text = "$" + rent2(32).ToString
                ElseIf owned(32) = 4 Then
                    price19.Text = "$" + rent3(32).ToString
                ElseIf owned(32) = 5 Then
                    price19.Text = "$" + rent4(32).ToString
                ElseIf owned(32) = 6 Then
                    price19.Text = "$" + rentHotel(32).ToString
                End If
            End If
                If ownedPlayer.Contains(34) Then
                prop20.BackColor = colours(0)
                If owned(34) = 1 Then
                    price20.Text = "$" + rent(34).ToString
                ElseIf owned(34) = 2 Then
                    price20.Text = "$" + rent1(34).ToString
                ElseIf owned(34) = 3 Then
                    price20.Text = "$" + rent2(34).ToString
                ElseIf owned(34) = 4 Then
                    price20.Text = "$" + rent3(34).ToString
                ElseIf owned(34) = 5 Then
                    price20.Text = "$" + rent4(34).ToString
                ElseIf owned(34) = 6 Then
                    price20.Text = "$" + rentHotel(34).ToString
                End If
            End If
                If ownedPlayer.Contains(37) Then
                prop21.BackColor = colours(0)
                If owned(37) = 1 Then
                    price21.Text = "$" + rent(37).ToString
                ElseIf owned(37) = 2 Then
                    price21.Text = "$" + rent1(37).ToString
                ElseIf owned(37) = 3 Then
                    price21.Text = "$" + rent2(37).ToString
                ElseIf owned(37) = 4 Then
                    price21.Text = "$" + rent3(37).ToString
                ElseIf owned(37) = 5 Then
                    price21.Text = "$" + rent4(37).ToString
                ElseIf owned(37) = 6 Then
                    price21.Text = "$" + rentHotel(37).ToString
                End If
            End If
            If ownedPlayer.Contains(39) Then
                prop22.BackColor = colours(0)
                If owned(39) = 1 Then
                    price22.Text = "$" + rent(39).ToString
                ElseIf owned(39) = 2 Then
                    price22.Text = "$" + rent1(39).ToString
                ElseIf owned(39) = 3 Then
                    price22.Text = "$" + rent2(39).ToString
                ElseIf owned(39) = 4 Then
                    price22.Text = "$" + rent3(39).ToString
                ElseIf owned(39) = 5 Then
                    price22.Text = "$" + rent4(39).ToString
                ElseIf owned(39) = 6 Then
                    price22.Text = "$" + rentHotel(39).ToString
                End If
            End If
        End If



        ' Read line to 'ownedCPU1'
        Dim ownedCPU1Line As String = streamReader.ReadLine()
        For Each numStr As String In ownedCPU1Line.Split(" ")
            If Not String.IsNullOrWhiteSpace(numStr) Then
                ownedCPU1.Add(Integer.Parse(numStr))
            End If
        Next
        If ownedCPU1.Count > 0 Then
            If ownedCPU1.Contains(1) Then
                prop01.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(3) Then
                prop02.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(6) Then
                prop03.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(8) Then
                prop04.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(9) Then
                prop05.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(11) Then
                prop06.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(13) Then
                prop07.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(14) Then
                prop08.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(16) Then
                prop09.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(18) Then
                prop10.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(19) Then
                prop11.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(21) Then
                prop12.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(23) Then
                prop13.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(24) Then
                prop14.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(26) Then
                prop15.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(28) Then
                prop16.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(29) Then
                prop17.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(31) Then
                prop18.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(32) Then
                prop19.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(34) Then
                prop20.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(37) Then
                prop21.BackColor = colours(1)
            End If
            If ownedCPU1.Contains(39) Then
                prop22.BackColor = colours(1)
            End If
        End If

        ' Read line to 'ownedCPU2'
        Dim ownedCPU2Line As String = streamReader.ReadLine()
        For Each numStr As String In ownedCPU2Line.Split(" ")
            If Not String.IsNullOrWhiteSpace(numStr) Then
                ownedCPU2.Add(Integer.Parse(numStr))
            End If
        Next
        If ownedCPU2.Count > 0 Then
            If ownedCPU2.Contains(1) Then
                prop01.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(3) Then
                prop02.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(6) Then
                prop03.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(8) Then
                prop04.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(9) Then
                prop05.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(11) Then
                prop06.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(13) Then
                prop07.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(14) Then
                prop08.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(16) Then
                prop09.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(18) Then
                prop10.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(19) Then
                prop11.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(21) Then
                prop12.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(23) Then
                prop13.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(24) Then
                prop14.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(26) Then
                prop15.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(28) Then
                prop16.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(29) Then
                prop17.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(31) Then
                prop18.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(32) Then
                prop19.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(34) Then
                prop20.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(37) Then
                prop21.BackColor = colours(2)
            End If
            If ownedCPU2.Contains(39) Then
                prop22.BackColor = colours(2)
            End If
        End If

        ' Read line to 'ownedCPU3'
        Dim ownedCPU3Line As String = streamReader.ReadLine()
        For Each numStr As String In ownedCPU3Line.Split(" ")
            If Not String.IsNullOrWhiteSpace(numStr) Then
                ownedCPU3.Add(Integer.Parse(numStr))
            End If
        Next
        If ownedCPU3.Count > 0 Then
            If ownedCPU3.Contains(1) Then
                prop01.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(3) Then
                prop02.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(6) Then
                prop03.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(8) Then
                prop04.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(9) Then
                prop05.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(11) Then
                prop06.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(13) Then
                prop07.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(14) Then
                prop08.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(16) Then
                prop09.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(18) Then
                prop10.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(19) Then
                prop11.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(21) Then
                prop12.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(23) Then
                prop13.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(24) Then
                prop14.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(26) Then
                prop15.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(28) Then
                prop16.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(29) Then
                prop17.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(31) Then
                prop18.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(32) Then
                prop19.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(34) Then
                prop20.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(37) Then
                prop21.BackColor = colours(3)
            End If
            If ownedCPU3.Contains(39) Then
                prop22.BackColor = colours(3)
            End If
        End If

        streamReader.Close()
        fileStream.Close()

        ' Update money.
        lblMoneyPlayer.Text = "$" + money(0).ToString
        lblMoneyCPU1.Text = "$" + money(1).ToString
        lblMoneyCPU2.Text = "$" + money(2).ToString
        lblMoneyCPU3.Text = "$" + money(3).ToString
    End Sub

    Private Sub tmrReroll_Tick(sender As Object, e As EventArgs) Handles tmrReroll.Tick
        tmrReroll.Stop()
        roll()
    End Sub

    Private Sub btnBankrupt_Click(sender As Object, e As EventArgs) Handles btnBankrupt.Click
        Bankrupt()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Save(True)
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnBuy_Click(sender As Object, e As EventArgs) Handles btnBuy.Click
        If money(0) >= price(position(0)) Then
            Buy()
        End If
    End Sub

    Private Sub btnEndTurn_Click(sender As Object, e As EventArgs) Handles btnEndTurn.Click
        tmrCooldown.Start()
        btnEndTurn.Enabled = False
        btnUpgrade.Enabled = False
        btnSell.Enabled = False
    End Sub

    ' Upgrade and sell properties
    Private Sub btnUpgrade_Click(sender As Object, e As EventArgs) Handles btnUpgrade.Click
        ' Toggle upgrade button visibility.
        If ownedPlayer.Contains(1) And ownedPlayer.Contains(3) Then
            If owned(1) > 0 And owned(1) < 6 Then
                up01.Visible = Not up01.Visible
            End If
            If owned(3) > 0 And owned(3) < 6 Then
                up02.Visible = Not up02.Visible
            End If
        End If
        If ownedPlayer.Contains(6) And ownedPlayer.Contains(8) And ownedPlayer.Contains(9) Then
            If owned(6) > 0 And owned(6) < 6 Then
                up03.Visible = Not up03.Visible
            End If
            If owned(8) > 0 And owned(8) < 6 Then
                up04.Visible = Not up04.Visible
            End If
            If owned(9) > 0 And owned(9) < 6 Then
                up05.Visible = Not up05.Visible
            End If
        End If
        If ownedPlayer.Contains(11) And ownedPlayer.Contains(13) And ownedPlayer.Contains(14) Then
            If owned(11) > 0 And owned(11) < 6 Then
                up06.Visible = Not up06.Visible
            End If
            If owned(13) > 0 And owned(13) < 6 Then
                up07.Visible = Not up07.Visible
            End If
            If owned(14) > 0 And owned(14) < 6 Then
                up08.Visible = Not up08.Visible
            End If
        End If
        If ownedPlayer.Contains(16) And ownedPlayer.Contains(18) And ownedPlayer.Contains(19) Then
            If owned(16) > 0 And owned(16) < 6 Then
                up09.Visible = Not up09.Visible
            End If
            If owned(18) > 0 And owned(18) < 6 Then
                up10.Visible = Not up10.Visible
            End If
            If owned(19) > 0 And owned(19) < 6 Then
                up11.Visible = Not up11.Visible
            End If
        End If
        If ownedPlayer.Contains(21) And ownedPlayer.Contains(23) And ownedPlayer.Contains(24) Then
            If owned(21) > 0 And owned(21) < 6 Then
                up12.Visible = Not up12.Visible
            End If
            If owned(23) > 0 And owned(23) < 6 Then
                up13.Visible = Not up13.Visible
            End If
            If owned(24) > 0 And owned(24) < 6 Then
                up14.Visible = Not up14.Visible
            End If
        End If
        If ownedPlayer.Contains(26) And ownedPlayer.Contains(28) And ownedPlayer.Contains(29) Then
            If owned(26) > 0 And owned(26) < 6 Then
                up15.Visible = Not up15.Visible
            End If
            If owned(28) > 0 And owned(28) < 6 Then
                up16.Visible = Not up16.Visible
            End If
            If owned(29) > 0 And owned(29) < 6 Then
                up17.Visible = Not up17.Visible
            End If
        End If
        If ownedPlayer.Contains(31) And ownedPlayer.Contains(32) And ownedPlayer.Contains(34) Then
            If owned(31) > 0 And owned(31) < 6 Then
                up18.Visible = Not up18.Visible
            End If
            If owned(32) > 0 And owned(32) < 6 Then
                up19.Visible = Not up19.Visible
            End If
            If owned(34) > 0 And owned(34) < 6 Then
                up20.Visible = Not up20.Visible
            End If
        End If
        If ownedPlayer.Contains(37) And ownedPlayer.Contains(39) Then
            If owned(37) > 0 And owned(37) < 6 Then
                up21.Visible = Not up21.Visible
            End If
            If owned(39) > 0 And owned(39) < 6 Then
                up22.Visible = Not up22.Visible
            End If
        End If
        ' Disable sell buttons
        sell01.Visible = False
        sell02.Visible = False
        sell03.Visible = False
        sell04.Visible = False
        sell05.Visible = False
        sell06.Visible = False
        sell07.Visible = False
        sell08.Visible = False
        sell09.Visible = False
        sell10.Visible = False
        sell11.Visible = False
        sell12.Visible = False
        sell13.Visible = False
        sell14.Visible = False
        sell15.Visible = False
        sell16.Visible = False
        sell17.Visible = False
        sell18.Visible = False
        sell19.Visible = False
        sell20.Visible = False
        sell21.Visible = False
        sell22.Visible = False
    End Sub
    Private Sub btnSell_Click(sender As Object, e As EventArgs) Handles btnSell.Click
        ' Check if owned and enable sell button.
        If ownedPlayer.Contains(1) Then
            sell01.Visible = Not sell01.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(1).ToString().Last())
            sell01.Text = "$" + (price(1) + 50 * upgraded).ToString
        End If
        If ownedPlayer.Contains(3) Then
            sell02.Visible = Not sell02.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(3).ToString().Last())
            sell02.Text = "$" + (price(3) + 50 * upgraded).ToString
        End If
        If ownedPlayer.Contains(6) Then
            sell03.Visible = Not sell03.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(6).ToString().Last())
            sell03.Text = "$" + (price(6) + 50 * upgraded).ToString
        End If
        If ownedPlayer.Contains(8) Then
            sell04.Visible = Not sell04.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(8).ToString().Last())
            sell04.Text = "$" + (price(8) + 50 * upgraded).ToString
        End If
        If ownedPlayer.Contains(9) Then
            sell05.Visible = Not sell05.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(9).ToString().Last())
            sell05.Text = "$" + (price(9) + 50 * upgraded).ToString
        End If
        If ownedPlayer.Contains(11) Then
            sell06.Visible = Not sell06.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(11).ToString().Last())
            sell06.Text = "$" + (price(11) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(13) Then
            sell07.Visible = Not sell07.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(13).ToString().Last())
            sell07.Text = "$" + (price(13) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(14) Then
            sell08.Visible = Not sell08.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(14).ToString().Last())
            sell08.Text = "$" + (price(14) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(16) Then
            sell09.Visible = Not sell09.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(16).ToString().Last())
            sell09.Text = "$" + (price(16) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(18) Then
            sell10.Visible = Not sell10.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(18).ToString().Last())
            sell10.Text = "$" + (price(18) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(19) Then
            sell11.Visible = Not sell11.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(19).ToString().Last())
            sell11.Text = "$" + (price(19) + 100 * upgraded).ToString
        End If
        If ownedPlayer.Contains(21) Then
            sell12.Visible = Not sell12.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(21).ToString().Last())
            sell12.Text = "$" + (price(21) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(23) Then
            sell13.Visible = Not sell13.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(23).ToString().Last())
            sell13.Text = "$" + (price(23) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(24) Then
            sell14.Visible = Not sell14.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(24).ToString().Last())
            sell14.Text = "$" + (price(24) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(26) Then
            sell15.Visible = Not sell15.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(26).ToString().Last())
            sell15.Text = "$" + (price(26) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(28) Then
            sell16.Visible = Not sell16.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(28).ToString().Last())
            sell16.Text = "$" + (price(28) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(29) Then
            sell17.Visible = Not sell17.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(29).ToString().Last())
            sell17.Text = "$" + (price(29) + 150 * upgraded).ToString
        End If
        If ownedPlayer.Contains(31) Then
            sell18.Visible = Not sell18.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(31).ToString().Last())
            sell18.Text = "$" + (price(31) + 200 * upgraded).ToString
        End If
        If ownedPlayer.Contains(32) Then
            sell19.Visible = Not sell19.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(32).ToString().Last())
            sell19.Text = "$" + (price(32) + 200 * upgraded).ToString
        End If
        If ownedPlayer.Contains(34) Then
            sell20.Visible = Not sell20.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(34).ToString().Last())
            sell20.Text = "$" + (price(34) + 200 * upgraded).ToString
        End If
        If ownedPlayer.Contains(37) Then
            sell21.Visible = Not sell21.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(27).ToString().Last())
            sell21.Text = "$" + (price(37) + 200 * upgraded).ToString
        End If
        If ownedPlayer.Contains(39) Then
            sell22.Visible = Not sell22.Visible
            Dim upgraded As Integer = Convert.ToInt32(owned(39).ToString().Last())
            sell22.Text = "$" + (price(39) + 200 * upgraded).ToString
        End If
        If ownedPlayer.Contains(5) Then
            sellRR1.Visible = Not sellRR1.Visible
            sellRR1.Text = "$150"
        End If
        If ownedPlayer.Contains(15) Then
            sellRR2.Visible = Not sellRR2.Visible
            sellRR2.Text = "$150"
        End If
        If ownedPlayer.Contains(25) Then
            sellRR3.Visible = Not sellRR3.Visible
            sellRR3.Text = "$150"
        End If
        If ownedPlayer.Contains(35) Then
            sellRR4.Visible = Not sellRR4.Visible
            sellRR4.Text = "$150"
        End If
        If ownedPlayer.Contains(12) Then
            sellUT1.Visible = Not sellUT1.Visible
            sellUT1.Text = "$150"
        End If
        If ownedPlayer.Contains(27) Then
            sellUT2.Visible = Not sellUT2.Visible
            sellUT2.Text = "$150"
        End If
        ' Disable upgrade buttons
        up01.Visible = False
        up02.Visible = False
        up03.Visible = False
        up04.Visible = False
        up05.Visible = False
        up06.Visible = False
        up07.Visible = False
        up08.Visible = False
        up09.Visible = False
        up10.Visible = False
        up11.Visible = False
        up12.Visible = False
        up13.Visible = False
        up14.Visible = False
        up15.Visible = False
        up16.Visible = False
        up17.Visible = False
        up18.Visible = False
        up19.Visible = False
        up20.Visible = False
        up21.Visible = False
        up22.Visible = False
    End Sub



    ' Upgrade button hell below. Thet all do the same thing.
    Private Sub up01_Click(sender As Object, e As EventArgs) Handles up01.Click
        ' Pay for house.
        money(Who) -= 50 ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString ' Upgrade money label.
        ' Increase rent.
        owned(1) += 1
        If owned(1) >= 6 Then
            up01.Visible = False
        End If
        ' Update rent label
        If owned(1) = 2 Then
            price01.Text = "$" + rent1(1).ToString
        ElseIf owned(1) = 3 Then
            price01.Text = "$" + rent2(1).ToString
        ElseIf owned(1) = 4 Then
            price01.Text = "$" + rent3(1).ToString
        ElseIf owned(1) = 5 Then
            price01.Text = "$" + rent4(1).ToString
        ElseIf owned(1) = 6 Then
            price01.Text = "$" + rentHotel(1).ToString
        End If

    End Sub
    Private Sub up02_Click(sender As Object, e As EventArgs) Handles up02.Click
        money(Who) -= 50
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(3) += 1
        If owned(3) >= 6 Then
            up02.Visible = False
        End If
        If owned(3) = 2 Then
            price02.Text = "$" + rent1(3).ToString
        ElseIf owned(3) = 3 Then
            price02.Text = "$" + rent2(3).ToString
        ElseIf owned(3) = 4 Then
            price02.Text = "$" + rent3(3).ToString
        ElseIf owned(3) = 5 Then
            price02.Text = "$" + rent4(3).ToString
        ElseIf owned(3) = 6 Then
            price02.Text = "$" + rentHotel(3).ToString
        End If
    End Sub
    Private Sub up03_Click(sender As Object, e As EventArgs) Handles up03.Click
        money(Who) -= 50
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(6) += 1
        If owned(6) >= 6 Then
            up03.Visible = False
        End If
        If owned(6) = 2 Then
            price03.Text = "$" + rent1(6).ToString
        ElseIf owned(6) = 3 Then
            price03.Text = "$" + rent2(6).ToString
        ElseIf owned(6) = 4 Then
            price03.Text = "$" + rent3(6).ToString
        ElseIf owned(6) = 5 Then
            price03.Text = "$" + rent4(6).ToString
        ElseIf owned(6) = 6 Then
            price03.Text = "$" + rentHotel(6).ToString
        End If
    End Sub
    Private Sub up04_Click(sender As Object, e As EventArgs) Handles up04.Click
        money(Who) -= 50
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(8) += 1
        If owned(8) >= 6 Then
            up04.Visible = False
        End If
        If owned(8) = 2 Then
            price04.Text = "$" + rent1(8).ToString
        ElseIf owned(8) = 3 Then
            price04.Text = "$" + rent2(8).ToString
        ElseIf owned(8) = 4 Then
            price04.Text = "$" + rent3(8).ToString
        ElseIf owned(8) = 5 Then
            price04.Text = "$" + rent4(8).ToString
        ElseIf owned(8) = 6 Then
            price04.Text = "$" + rentHotel(8).ToString
        End If
    End Sub
    Private Sub up05_Click(sender As Object, e As EventArgs) Handles up05.Click
        money(Who) -= 50
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(9) += 1
        If owned(9) >= 6 Then
            up05.Visible = False
        End If
        If owned(9) = 2 Then
            price05.Text = "$" + rent1(9).ToString
        ElseIf owned(9) = 3 Then
            price05.Text = "$" + rent2(9).ToString
        ElseIf owned(9) = 4 Then
            price05.Text = "$" + rent3(9).ToString
        ElseIf owned(9) = 5 Then
            price05.Text = "$" + rent4(9).ToString
        ElseIf owned(9) = 6 Then
            price05.Text = "$" + rentHotel(9).ToString
        End If
    End Sub
    Private Sub up06_Click(sender As Object, e As EventArgs) Handles up06.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(11) += 1
        If owned(11) >= 6 Then
            up06.Visible = False
        End If
        If owned(11) = 2 Then
            price06.Text = "$" + rent1(11).ToString
        ElseIf owned(11) = 3 Then
            price06.Text = "$" + rent2(11).ToString
        ElseIf owned(11) = 4 Then
            price06.Text = "$" + rent3(11).ToString
        ElseIf owned(11) = 5 Then
            price06.Text = "$" + rent4(11).ToString
        ElseIf owned(11) = 6 Then
            price06.Text = "$" + rentHotel(11).ToString
        End If
    End Sub
    Private Sub up07_Click(sender As Object, e As EventArgs) Handles up07.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(13) += 1
        If owned(13) >= 6 Then
            up07.Visible = False
        End If
        If owned(13) = 2 Then
            price07.Text = "$" + rent1(13).ToString
        ElseIf owned(13) = 3 Then
            price07.Text = "$" + rent2(13).ToString
        ElseIf owned(13) = 4 Then
            price07.Text = "$" + rent3(13).ToString
        ElseIf owned(13) = 5 Then
            price07.Text = "$" + rent4(13).ToString
        ElseIf owned(13) = 6 Then
            price07.Text = "$" + rentHotel(13).ToString
        End If
    End Sub
    Private Sub up08_Click(sender As Object, e As EventArgs) Handles up08.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(14) += 1
        If owned(14) >= 6 Then
            up08.Visible = False
        End If
        If owned(14) = 2 Then
            price08.Text = "$" + rent1(14).ToString
        ElseIf owned(14) = 3 Then
            price08.Text = "$" + rent2(14).ToString
        ElseIf owned(14) = 4 Then
            price08.Text = "$" + rent3(14).ToString
        ElseIf owned(14) = 5 Then
            price08.Text = "$" + rent4(14).ToString
        ElseIf owned(14) = 6 Then
            price08.Text = "$" + rentHotel(14).ToString
        End If
    End Sub
    Private Sub up09_Click(sender As Object, e As EventArgs) Handles up09.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(16) += 1
        If owned(16) >= 6 Then
            up09.Visible = False
        End If
        If owned(16) = 2 Then
            price09.Text = "$" + rent1(16).ToString
        ElseIf owned(16) = 3 Then
            price09.Text = "$" + rent2(16).ToString
        ElseIf owned(16) = 4 Then
            price09.Text = "$" + rent3(16).ToString
        ElseIf owned(16) = 5 Then
            price09.Text = "$" + rent4(16).ToString
        ElseIf owned(16) = 6 Then
            price09.Text = "$" + rentHotel(16).ToString
        End If
    End Sub
    Private Sub up10_Click(sender As Object, e As EventArgs) Handles up10.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(18) += 1
        If owned(18) >= 6 Then
            up10.Visible = False
        End If
        If owned(18) = 2 Then
            price10.Text = "$" + rent1(18).ToString
        ElseIf owned(18) = 3 Then
            price10.Text = "$" + rent2(18).ToString
        ElseIf owned(18) = 4 Then
            price10.Text = "$" + rent3(18).ToString
        ElseIf owned(18) = 5 Then
            price10.Text = "$" + rent4(18).ToString
        ElseIf owned(18) = 6 Then
            price10.Text = "$" + rentHotel(18).ToString
        End If
    End Sub
    Private Sub up11_Click(sender As Object, e As EventArgs) Handles up11.Click
        money(Who) -= 100
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(19) += 1
        If owned(19) >= 6 Then
            up11.Visible = False
        End If
        If owned(19) = 2 Then
            price11.Text = "$" + rent1(19).ToString
        ElseIf owned(19) = 3 Then
            price11.Text = "$" + rent2(19).ToString
        ElseIf owned(19) = 4 Then
            price11.Text = "$" + rent3(19).ToString
        ElseIf owned(19) = 5 Then
            price11.Text = "$" + rent4(19).ToString
        ElseIf owned(19) = 6 Then
            price11.Text = "$" + rentHotel(19).ToString
        End If
    End Sub
    Private Sub up12_Click(sender As Object, e As EventArgs) Handles up12.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(21) += 1
        If owned(21) >= 6 Then
            up12.Visible = False
        End If
        If owned(21) = 2 Then
            price12.Text = "$" + rent1(21).ToString
        ElseIf owned(21) = 3 Then
            price12.Text = "$" + rent2(21).ToString
        ElseIf owned(21) = 4 Then
            price12.Text = "$" + rent3(21).ToString
        ElseIf owned(21) = 5 Then
            price12.Text = "$" + rent4(21).ToString
        ElseIf owned(21) = 6 Then
            price12.Text = "$" + rentHotel(21).ToString
        End If
    End Sub
    Private Sub up13_Click(sender As Object, e As EventArgs) Handles up13.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(23) += 1
        If owned(23) >= 6 Then
            up13.Visible = False
        End If
        If owned(23) = 2 Then
            price13.Text = "$" + rent1(23).ToString
        ElseIf owned(23) = 3 Then
            price13.Text = "$" + rent2(23).ToString
        ElseIf owned(23) = 4 Then
            price13.Text = "$" + rent3(23).ToString
        ElseIf owned(23) = 5 Then
            price13.Text = "$" + rent4(23).ToString
        ElseIf owned(23) = 6 Then
            price13.Text = "$" + rentHotel(23).ToString
        End If
    End Sub
    Private Sub up14_Click(sender As Object, e As EventArgs) Handles up14.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(24) += 1
        If owned(24) >= 6 Then
            up14.Visible = False
        End If
        If owned(24) = 2 Then
            price14.Text = "$" + rent1(24).ToString
        ElseIf owned(24) = 3 Then
            price14.Text = "$" + rent2(24).ToString
        ElseIf owned(24) = 4 Then
            price14.Text = "$" + rent3(24).ToString
        ElseIf owned(24) = 5 Then
            price14.Text = "$" + rent4(24).ToString
        ElseIf owned(24) = 6 Then
            price14.Text = "$" + rentHotel(24).ToString
        End If
    End Sub
    Private Sub up15_Click(sender As Object, e As EventArgs) Handles up15.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(26) += 1
        If owned(26) >= 6 Then
            up15.Visible = False
        End If
        If owned(26) = 2 Then
            price15.Text = "$" + rent1(26).ToString
        ElseIf owned(26) = 3 Then
            price15.Text = "$" + rent2(26).ToString
        ElseIf owned(26) = 4 Then
            price15.Text = "$" + rent3(26).ToString
        ElseIf owned(26) = 5 Then
            price15.Text = "$" + rent4(26).ToString
        ElseIf owned(26) = 6 Then
            price15.Text = "$" + rentHotel(26).ToString
        End If
    End Sub
    Private Sub up16_Click(sender As Object, e As EventArgs) Handles up16.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(28) += 1
        If owned(28) >= 6 Then
            up16.Visible = False
        End If
        If owned(28) = 2 Then
            price16.Text = "$" + rent1(28).ToString
        ElseIf owned(28) = 3 Then
            price16.Text = "$" + rent2(28).ToString
        ElseIf owned(28) = 4 Then
            price16.Text = "$" + rent3(28).ToString
        ElseIf owned(28) = 5 Then
            price16.Text = "$" + rent4(28).ToString
        ElseIf owned(28) = 6 Then
            price16.Text = "$" + rentHotel(28).ToString
        End If
    End Sub
    Private Sub up17_Click(sender As Object, e As EventArgs) Handles up17.Click
        money(Who) -= 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(29) += 1
        If owned(29) >= 6 Then
            up17.Visible = False
        End If
        If owned(29) = 2 Then
            price17.Text = "$" + rent1(29).ToString
        ElseIf owned(29) = 3 Then
            price17.Text = "$" + rent2(29).ToString
        ElseIf owned(29) = 4 Then
            price17.Text = "$" + rent3(29).ToString
        ElseIf owned(29) = 5 Then
            price17.Text = "$" + rent4(29).ToString
        ElseIf owned(29) = 6 Then
            price17.Text = "$" + rentHotel(29).ToString
        End If
    End Sub
    Private Sub up18_Click(sender As Object, e As EventArgs) Handles up18.Click
        money(Who) -= 200
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(31) += 1
        If owned(31) >= 6 Then
            up18.Visible = False
        End If
        If owned(31) = 2 Then
            price18.Text = "$" + rent1(31).ToString
        ElseIf owned(31) = 3 Then
            price18.Text = "$" + rent2(31).ToString
        ElseIf owned(31) = 4 Then
            price18.Text = "$" + rent3(31).ToString
        ElseIf owned(31) = 5 Then
            price18.Text = "$" + rent4(31).ToString
        ElseIf owned(31) = 6 Then
            price18.Text = "$" + rentHotel(31).ToString
        End If
    End Sub
    Private Sub up19_Click(sender As Object, e As EventArgs) Handles up19.Click
        money(Who) -= 200
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(32) += 1
        If owned(32) >= 6 Then
            up19.Visible = False
        End If
        If owned(32) = 2 Then
            price19.Text = "$" + rent1(32).ToString
        ElseIf owned(32) = 3 Then
            price19.Text = "$" + rent2(32).ToString
        ElseIf owned(32) = 4 Then
            price19.Text = "$" + rent3(32).ToString
        ElseIf owned(32) = 5 Then
            price19.Text = "$" + rent4(32).ToString
        ElseIf owned(32) = 6 Then
            price19.Text = "$" + rentHotel(32).ToString
        End If
    End Sub
    Private Sub up20_Click(sender As Object, e As EventArgs) Handles up20.Click
        money(Who) -= 200
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(34) += 1
        If owned(34) >= 6 Then
            up20.Visible = False
        End If
        If owned(34) = 2 Then
            price20.Text = "$" + rent1(34).ToString
        ElseIf owned(34) = 3 Then
            price20.Text = "$" + rent2(34).ToString
        ElseIf owned(34) = 4 Then
            price20.Text = "$" + rent3(34).ToString
        ElseIf owned(34) = 5 Then
            price20.Text = "$" + rent4(34).ToString
        ElseIf owned(34) = 6 Then
            price20.Text = "$" + rentHotel(34).ToString
        End If
    End Sub
    Private Sub up21_Click(sender As Object, e As EventArgs) Handles up21.Click
        money(Who) -= 200
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(37) += 1
        If owned(37) >= 6 Then
            up21.Visible = False
        End If
        If owned(37) = 2 Then
            price21.Text = "$" + rent1(37).ToString
        ElseIf owned(37) = 3 Then
            price21.Text = "$" + rent2(37).ToString
        ElseIf owned(37) = 4 Then
            price21.Text = "$" + rent3(37).ToString
        ElseIf owned(37) = 5 Then
            price21.Text = "$" + rent4(37).ToString
        ElseIf owned(37) = 6 Then
            price21.Text = "$" + rentHotel(37).ToString
        End If
    End Sub
    Private Sub up22_Click(sender As Object, e As EventArgs) Handles up22.Click
        money(Who) -= 200
        lblMoneyPlayer.Text = "$" + money(Who).ToString
        ' rent
        owned(39) += 1
        If owned(39) >= 6 Then
            up22.Visible = False
        End If
        If owned(39) = 2 Then
            price22.Text = "$" + rent1(39).ToString
        ElseIf owned(39) = 3 Then
            price22.Text = "$" + rent2(39).ToString
        ElseIf owned(39) = 4 Then
            price22.Text = "$" + rent3(39).ToString
        ElseIf owned(39) = 5 Then
            price22.Text = "$" + rent4(39).ToString
        ElseIf owned(39) = 6 Then
            price22.Text = "$" + rentHotel(39).ToString
        End If
    End Sub

    ' Sell button hell. They all do the same thing.
    Private Sub sell01_Click(sender As Object, e As EventArgs) Handles sell01.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(1).ToString().Last())
        money(Who) += price(1) + 50 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(1) = 0
        ownedPlayer.Remove(1)
        prop01.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
        sell01.Visible = False
        ' Revert price
        price01.Text = "$" + price(1).ToString

    End Sub
    Private Sub sell02_Click(sender As Object, e As EventArgs) Handles sell02.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(3).ToString().Last())
        money(Who) += price(3) + 50 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(3) = 0
        ownedPlayer.Remove(3)
        prop02.BackColor = System.Drawing.Color.FromArgb(255, 255, 0)
        sell02.Visible = False
        price02.Text = "$" + price(3).ToString
    End Sub
    Private Sub sell03_Click(sender As Object, e As EventArgs) Handles sell03.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(6).ToString().Last())
        money(Who) += price(6) + 50 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(6) = 0
        ownedPlayer.Remove(6)
        prop03.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
        sell03.Visible = False
        price03.Text = "$" + price(6).ToString
    End Sub
    Private Sub sell04_Click(sender As Object, e As EventArgs) Handles sell04.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(8).ToString().Last())
        money(Who) += price(8) + 50 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(8) = 0
        ownedPlayer.Remove(8)
        prop04.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
        sell04.Visible = False
        price04.Text = "$" + price(8).ToString
    End Sub
    Private Sub sell05_Click(sender As Object, e As EventArgs) Handles sell05.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(9).ToString().Last())
        money(Who) += price(9) + 50 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(9) = 0
        ownedPlayer.Remove(9)
        prop05.BackColor = System.Drawing.Color.FromArgb(0, 0, 255)
        sell05.Visible = False
        price05.Text = "$" + price(9).ToString
    End Sub
    Private Sub sell06_Click(sender As Object, e As EventArgs) Handles sell06.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(11).ToString().Last())
        money(Who) += price(11) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(11) = 0
        ownedPlayer.Remove(11)
        prop06.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
        sell06.Visible = False
        price06.Text = "$" + price(11).ToString
    End Sub
    Private Sub sell07_Click(sender As Object, e As EventArgs) Handles sell07.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(13).ToString().Last())
        money(Who) += price(13) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(13) = 0
        ownedPlayer.Remove(13)
        prop07.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
        sell07.Visible = False
        price07.Text = "$" + price(13).ToString
    End Sub
    Private Sub sell08_Click(sender As Object, e As EventArgs) Handles sell08.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(14).ToString().Last())
        money(Who) += price(14) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(14) = 0
        ownedPlayer.Remove(14)
        prop08.BackColor = System.Drawing.Color.FromArgb(255, 0, 255)
        sell08.Visible = False
        price08.Text = "$" + price(14).ToString
    End Sub
    Private Sub sell09_Click(sender As Object, e As EventArgs) Handles sell09.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(16).ToString().Last())
        money(Who) += price(16) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(16) = 0
        ownedPlayer.Remove(16)
        prop09.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
        sell09.Visible = False
        price09.Text = "$" + price(16).ToString
    End Sub
    Private Sub sell10_Click(sender As Object, e As EventArgs) Handles sell10.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(18).ToString().Last())
        money(Who) += price(18) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(18) = 0
        ownedPlayer.Remove(18)
        prop10.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
        sell11.Visible = False
        price10.Text = "$" + price(18).ToString
    End Sub
    Private Sub sell11_Click(sender As Object, e As EventArgs) Handles sell11.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(19).ToString().Last())
        money(Who) += price(19) + 100 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(19) = 0
        ownedPlayer.Remove(19)
        prop11.BackColor = System.Drawing.Color.FromArgb(128, 64, 0)
        sell11.Visible = False
        price11.Text = "$" + price(19).ToString
    End Sub
    Private Sub sell12_Click(sender As Object, e As EventArgs) Handles sell12.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(21).ToString().Last())
        money(Who) += price(21) + 150 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(21) = 0
        ownedPlayer.Remove(21)
        prop12.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
        sell12.Visible = False
        price12.Text = "$" + price(21).ToString
    End Sub
    Private Sub sell13_Click(sender As Object, e As EventArgs) Handles sell13.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(23).ToString().Last())
        money(Who) += price(23) + 150 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(23) = 0
        ownedPlayer.Remove(23)
        prop13.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
        sell13.Visible = False
        price13.Text = "$" + price(23).ToString
    End Sub
    Private Sub sell14_Click(sender As Object, e As EventArgs) Handles sell14.Click
        Dim upgraded As Integer = Convert.ToInt32(owned(24).ToString().Last())
        money(Who) += price(24) + 150 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(24) = 0
        ownedPlayer.Remove(24)
        prop14.BackColor = System.Drawing.Color.FromArgb(64, 0, 0)
        sell14.Visible = False
        price14.Text = "$" + price(24).ToString
    End Sub
    Private Sub sell15_Click(sender As Object, e As EventArgs) Handles sell15.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(26).ToString().Last())
        money(Who) += price(26) + 150 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(26) = 0
        ownedPlayer.Remove(26)
        prop15.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
        sell15.Visible = False
        price15.Text = "$" + price(26).ToString
    End Sub
    Private Sub sell16_Click(sender As Object, e As EventArgs) Handles sell16.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(28).ToString().Last())
        money(Who) += price(28) + 150 * upgraded ' Remove money..
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(28) = 0
        ownedPlayer.Remove(28)
        prop16.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
        sell16.Visible = False
        price16.Text = "$" + price(28).ToString
    End Sub
    Private Sub sell17_Click(sender As Object, e As EventArgs) Handles sell17.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(29).ToString().Last())
        money(Who) += price(29) + 150 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(29) = 0
        ownedPlayer.Remove(29)
        prop17.BackColor = System.Drawing.Color.FromArgb(64, 0, 64)
        sell17.Visible = False
        price17.Text = "$" + price(29).ToString
    End Sub
    Private Sub sell18_Click(sender As Object, e As EventArgs) Handles sell18.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(31).ToString().Last())
        money(Who) += price(31) + 200 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(31) = 0
        ownedPlayer.Remove(31)
        prop18.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
        sell18.Visible = False
        price18.Text = "$" + price(31).ToString
    End Sub
    Private Sub sell19_Click(sender As Object, e As EventArgs) Handles sell19.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(32).ToString().Last())
        money(Who) += price(32) + 200 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(32) = 0
        ownedPlayer.Remove(32)
        prop19.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
        sell19.Visible = False
        price19.Text = "$" + price(32).ToString
    End Sub
    Private Sub sell20_Click(sender As Object, e As EventArgs) Handles sell20.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(34).ToString().Last())
        money(Who) += price(34) + 200 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(34) = 0
        ownedPlayer.Remove(34)
        prop20.BackColor = System.Drawing.Color.FromArgb(128, 64, 64)
        sell20.Visible = False
        price20.Text = "$" + price(34).ToString
    End Sub
    Private Sub sell21_Click(sender As Object, e As EventArgs) Handles sell21.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(37).ToString().Last())
        money(Who) += price(37) + 200 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(37) = 0
        ownedPlayer.Remove(37)
        prop21.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
        sell21.Visible = False
        price21.Text = "$" + price(37).ToString
    End Sub
    Private Sub sell22_Click(sender As Object, e As EventArgs) Handles sell22.Click
        ' Pay for house.
        Dim upgraded As Integer = Convert.ToInt32(owned(39).ToString().Last())
        money(Who) += price(39) + 200 * upgraded ' Remove money.
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        ' Increase rent.
        owned(39) = 0
        ownedPlayer.Remove(39)
        prop22.BackColor = System.Drawing.Color.FromArgb(0, 64, 64)
        sell22.Visible = False
        price22.Text = "$" + price(39).ToString
    End Sub


    Private Sub sellRR1_Click(sender As Object, e As EventArgs) Handles sellRR1.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(5) = 0
        ownedPlayer.Remove(5)
        sellRR1.Visible = False
        priceRR1.Text = "$150"
    End Sub
    Private Sub sellRR2_Click(sender As Object, e As EventArgs) Handles sellRR2.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(15) = 0
        ownedPlayer.Remove(15)
        sellRR2.Visible = False
        priceRR2.Text = "$150"
    End Sub
    Private Sub sellRR3_Click(sender As Object, e As EventArgs) Handles sellRR3.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(25) = 0
        ownedPlayer.Remove(25)
        sellRR3.Visible = False
        priceRR3.Text = "$150"
    End Sub
    Private Sub sellRR4_Click(sender As Object, e As EventArgs) Handles sellRR4.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(35) = 0
        ownedPlayer.Remove(35)
        sellRR4.Visible = False
        priceRR4.Text = "$150"
    End Sub
    Private Sub sellUT1_Click(sender As Object, e As EventArgs) Handles sellUT1.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(12) = 0
        ownedPlayer.Remove(12)
        sellUT1.Visible = False
        priceUT1.Text = "$150"
    End Sub
    Private Sub sellUT2_Click(sender As Object, e As EventArgs) Handles sellUT2.Click
        money(Who) += 150
        lblMoneyPlayer.Text = "$" + money(Who).ToString  ' Upgrade money label.
        owned(12) = 0
        ownedPlayer.Remove(12)
        sellUT2.Visible = False
        priceUT2.Text = "$150"
    End Sub
End Class


