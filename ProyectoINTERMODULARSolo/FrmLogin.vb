Imports System.Data.SqlClient
Imports Entidades

Public Class FrmLogin

    Private Sub btnNoTengoCuenta_Click(sender As Object, e As EventArgs) Handles btnNoTengoCuenta.Click
        FrmAñadirAlumno.ShowDialog()
    End Sub

    Private Sub btnAceptarDNI_Click(sender As Object, e As EventArgs) Handles btnAceptarDNI.Click
        Dim res As Alumno = miGestor.AlumnoPorDni(txtDNI.Text)
        If res Is Nothing Then
            MessageBox.Show("No existe " & txtDNI.Text & " en la base de datos")
        Else

            FrmMenuPrincipal.ShowDialog()
            Close()
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' Cadena de conexión (ajústala a tu BD)
            Dim serv = ".\SQLEXPRESS"
            Dim cadConexion As String = $"Data Source={serv};Initial Catalog=g1_GESTFCT;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=2"
            ' Crear y abrir conexión
            Using conn As New SqlConnection(cadConexion)
                conn.Open()
                MessageBox.Show("Conexión exitosa a la base de datos", "Éxito",
                          MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error de conexión: " & ex.Message, "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Seed data temporal desactivado - los datos se gestionan desde la UI
        ' Dim alumno As Alumno = New Alumno("", "aa@gmail.com", "666777888", "markel", "salva", "2")
        ' Dim jornada As Jornada = New Jornada("2025/03/3", "12345678A", "4", "REALIZADA")
        ' Dim tarea As Tarea = New Tarea("1", Today, "dkfjadsk", 3, "12345678Z")
        'miGestor.DarAltaAlumnado(Alumno)
        'MessageBox.Show(miGestor.BorrarYComprobarInformacionAlumno(alumno))
        'miGestor.insertarJornadaAlumno(jornada) MEJORAR
        '  MessageBox.Show(miGestor.totalHorasAlumno("12345678A"))
        ' MessageBox.Show(miGestor.insertarTareaAlumno(tarea))
        'miGestor.ObtenerTareasSemanales("12345678A", #2024/10/1#, #2024/10/1#)
        'miGestor.ObtenerTareasDeJornada(#2024/10/1#, "12345678A")
        'Dim tarea As Tarea = New Tarea(1, "12345678A", #2024/10/1#, "Tarea de programación", 3)
        'miGestor.ModificarTarea(tarea)
        'miGestor.BorrarTarea(1, #2024/10/1#, "12345678A")
        'miGestor.GenerarCodigoTarea(#2024/10/1#, "12345678A")
        'miGestor.JornadaExiste(#2024/10/1#, "12345678A")
        miGestor.ObtenerJornadaSemana("12345678A", #2024/10/1#, #2024/10/1#)
    End Sub
End Class

