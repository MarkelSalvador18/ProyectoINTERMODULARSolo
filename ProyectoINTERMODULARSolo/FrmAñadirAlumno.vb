Imports Entidades

Public Class FrmAñadirAlumno
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Close()
    End Sub

    Private Sub btnCrearCuenta_Click(sender As Object, e As EventArgs) Handles btnCrearCuenta.Click
        Dim crearAlumn As New Alumno(txtNuevoDNI.Text, txtEmail.Text, txtTelefono.Text, txtNombre.Text, txtApellido1.Text, txtCodigoCiclo.Text)
        Dim mensaje As String = miGestor.DarAltaAlumnado(crearAlumn)
        If mensaje.Contains("Error") Then
            MessageBox.Show("Error, no se ha podido dar de alta al alumno " & txtNombre.Text)
            Exit Sub
        End If
        MessageBox.Show("Se ha añadido el usuario " & txtNombre.Text & " con el DNI: " & txtNuevoDNI.Text)
        txtApellido1.Clear()
        txtApellido2.Clear()
        txtCodigoCiclo.Clear()
        txtEmail.Clear()
        txtNombre.Clear()
        txtNuevoDNI.Clear()
        txtTelefono.Clear()
        Close()


    End Sub

    Private Sub FrmAñadirAlumno_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class