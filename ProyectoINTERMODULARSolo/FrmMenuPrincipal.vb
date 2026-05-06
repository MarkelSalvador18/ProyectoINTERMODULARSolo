Public Class FrmMenuPrincipal
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        FrmLogin.Show()
        Close()
    End Sub
End Class