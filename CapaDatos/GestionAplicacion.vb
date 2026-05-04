Imports System.CodeDom
Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.Remoting.Messaging
Imports System.Security.Cryptography.X509Certificates
Imports Entidades

Public Class GestionAplicacion

    Private cadenaConexion As String


    Public Sub New()
        Dim servidor = ".\SQLEXPRESS" ' todo Agregar del proyecto BuscarServidor
        cadenaConexion = $"Data Source={servidor};Initial Catalog=g1_GESTFCT;Integrated Security=True"
    End Sub
    Public Function DarAltaAlumnado(alumno As Alumno) As String
        '   INSERTAR EL ALUMNO

        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "INSERT INTO Alumno (Nombre, DNI, Telefono, Email, Apellido1, CodigoCiclo) " & "VALUES (@Nombre, @DNI, @Telefono, @Email, @Apellido1, @CodigoCiclo)"
        Try

            Dim dni As String = ComprobarDni(alumno.Dni)
            If dni.Contains("Error") Then
                Return "DNI invalido"
            End If
            If String.IsNullOrWhiteSpace(alumno.Nombre) Then

                Return "El alumno debe tener nombre"
            End If
            If String.IsNullOrWhiteSpace(alumno.Apellido1) Then
                Return "El alumno debe tener un apellido"
            End If
            Dim email As String = ComprobarEmail(alumno.Email)
            If email.Contains("Error") Then
                Return "Email invalido"
            End If
            Dim telefono As String = ComprobarTelefono(alumno.Telefono)
            If telefono.Contains("Error") Then
                Return "Teléfono invalido"
            End If
            'Dim sql2 As String = "SELECT nombre, dni, email, telefono, apellido1, codigoCiclo FROM ALUMNO WHERE codigoCiclo = @codigociclo"
            'Try
            '    conexion.Open()
            '    '   COMPROBAR Q EL CODIGO CICLO EXISTE
            '    Dim cmdCodigoCiclo As New SqlCommand(sql2, conexion)
            '    cmdCodigoCiclo.Parameters.AddWithValue("@codigoCiclo", alumno.CodigoCiclo)
            '    Dim drCiclo As SqlDataReader = cmdCodigoCiclo.ExecuteReader
            '    If Not drCiclo.HasRows Then
            '        conexion.Close()
            '        Return "No existe el ciclo"

            '    End If


            'Catch ex As Exception
            '    Return "No se ha podido añadir"
            'End Try


            conexion.Open()
            Dim cmdInsertaAlum As New SqlCommand(sql, conexion)
            cmdInsertaAlum.Parameters.AddWithValue("@Nombre", alumno.Nombre)
            cmdInsertaAlum.Parameters.AddWithValue("@DNI", alumno.Dni)
            cmdInsertaAlum.Parameters.AddWithValue("@Telefono", alumno.Telefono)
            cmdInsertaAlum.Parameters.AddWithValue("@Email", alumno.Email)
            cmdInsertaAlum.Parameters.AddWithValue("@Apellido1", alumno.Apellido1)
            cmdInsertaAlum.Parameters.AddWithValue("@CodigoCiclo", alumno.CodigoCiclo)


            Dim nFilas As Integer = cmdInsertaAlum.ExecuteNonQuery
            If nFilas = 0 Then
                Return "No se ha podido añadir al alumno"
            Else
                Return "Se ha añadido al alumno"
            End If
            conexion.Close()
        Catch ex As Exception
            Return "Error " & ex.Message
        End Try
        ' meter para 2do apellido
        ' Corregir el CODIGOCICLO
    End Function
    Public Function AlumnoPorDni(dni As String) As Alumno

        Dim sql As String = "SELECT Nombre, DNI, Telefono, Email, Apellido1, CodigoCiclo FROM ALUMNO Where dni = @dni;"

        Dim conexion As New SqlConnection(cadenaConexion)
        Try
            conexion.Open()
            Dim cmdComprobarDni As New SqlCommand(sql, conexion)
            cmdComprobarDni.Parameters.AddWithValue("@dni", dni)
            Dim dr As SqlDataReader = cmdComprobarDni.ExecuteReader
            If Not dr.HasRows Then
                conexion.Close()
                Return Nothing
            End If
            dr.Read()
            Dim alumn As Alumno = New Alumno(dr("dni"), dr("nombre"), dr("telefono"), dr("email"), dr("apellido1"), dr("codigociclo"))
            conexion.Close()
            Return alumn

        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function DarBajaAlumnado(dni As String) As String
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "No existe ningún alumno con ese DNI"
        End If
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "DELETE FROM ALUMNO WHERE DNI = @dni"
        Dim cmd As New SqlCommand(sql, conexion)
        Try
            cmd.Parameters.AddWithValue("@dni", alumno.Dni)
            conexion.Open()
            Dim filas As Integer = cmd.ExecuteNonQuery()
            If filas = 0 Then
                Return "No se pudo borrar el alumno"
            Else
                Return "Alumno borrado correctamente: " & alumno.Nombre & " " & alumno.Apellido1
            End If
        Catch ex As Exception
            Return "Error: " & ex.Message
        Finally
            conexion.Close()
        End Try
    End Function

    Public Function BorrarYComprobarInformacionAlumno(alum As Alumno) As String
        If alum Is Nothing Then
            Return "Error: El alumno no existe en la base de datos"
        End If
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "Select dni from alumno where dni = @dni;"
        Dim cmdInformacionAlumno As New SqlCommand(sql, conexion)
        Try
            cmdInformacionAlumno.Parameters.AddWithValue("@dni", alum.Dni)
            conexion.Open()
            Dim drInfoAlumno As SqlDataReader = cmdInformacionAlumno.ExecuteReader
            If drInfoAlumno.HasRows Then
                Dim sql3 As String = "Select * from Jornada where dnialumno = @dni;"
                Dim cmdComprobarInfo As New SqlCommand(sql3, conexion)
                cmdComprobarInfo.Parameters.AddWithValue("@dni", alum.Dni)
                drInfoAlumno.Close()
                Dim drJornada As SqlDataReader = cmdComprobarInfo.ExecuteReader
                If drJornada.HasRows Then
                    drJornada.Close()
                    Return "El alumno con el DNI: " & alum.Dni & " tiene jornadas"
                Else
                    Dim sql2 As String = "delete from alumno where dni = @dni;"
                    Dim cmdDelete As New SqlCommand(sql2, conexion)
                    cmdDelete.Parameters.AddWithValue("@dni", alum.Dni)
                    Dim drEjecuciones As Integer = cmdDelete.ExecuteNonQuery
                    If drEjecuciones = 0 Then
                        Return "No se ha podido borrar el alumno"
                    Else
                        Return "El alumno ha sido borrado"
                    End If
                End If
            End If
        Catch ex As Exception
            Return "Error: " & ex.Message
        Finally
            conexion.Close()
        End Try
        Return "Error: No se ha podido el alumno porque no tiene información"
    End Function

    Public Function AlumnoPorCiclo(codigoCiclo As String) As List(Of Alumno)
        Dim listaAlumnos As New List(Of Alumno)
        If codigoCiclo Is Nothing Then
            Return Nothing
        End If

        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "SELECT COUNT(*) FROM CICLO WHERE codigoCiclo = @codigoCiclo;"
        Try
            conexion.Open()
            Dim cmdCodigoCiclo As New SqlCommand(sql, conexion)
            cmdCodigoCiclo.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
            Dim dr As Integer = cmdCodigoCiclo.ExecuteScalar()

            If dr = 0 Then
                conexion.Close()
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
            conexion.Close()
        End Try
        Dim sql2 As String = "SELECT nombre, dni, email, telefono, apellido1, codigoCiclo FROM ALUMNO WHERE codigoCiclo = @codigociclo;"
        Try
            conexion.Open()
            Dim cmdCodigoLista As New SqlCommand(sql2, conexion)
            cmdCodigoLista.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
            Dim drListaAlumn As SqlDataReader = cmdCodigoLista.ExecuteReader()
            While drListaAlumn.Read()
                Dim alum As New Alumno(drListaAlumn("dni"), drListaAlumn("nombre"), drListaAlumn("telefono"), drListaAlumn("email"), drListaAlumn("apellido1"), drListaAlumn("codigoCiclo"))

                listaAlumnos.Add(alum)
            End While
            conexion.Close()
            Return listaAlumnos
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function insertarJornadaAlumno(jor As Jornada) As String

        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "Insert into Jornada (dniAlumno, fecha, Horas, Estado) values(@dniAlumno, @fecha, @Horas, @Estado);"
        ' COMPROBAR SI LA JORNADA A INSERTAR EXISTE con select
        ' Select * From Jornada Where dniAlumno = @dni And fecha = @fecha

        Try
            'Dim dniAlum As String = ComprobarDni(jor.DniAlumno)
            'If dniAlum.Contains("Error") Then
            '    Return "Error: el Dni no es valido"
            'End If
            'Dim dniA As Alumno = AlumnoPorDni(jor.DniAlumno)
            'If dniA Is Nothing Then
            '    Return "Error: Alumno no encontrado"
            'End If
            'If jor.Fecha = "" Then
            '    Return "La fecha no es valida"
            'End If
            conexion.Open()
            Dim cmdInsertarJornada As New SqlCommand(sql, conexion)
            cmdInsertarJornada.Parameters.AddWithValue("@dniAlumno", jor.DniAlumno)
            cmdInsertarJornada.Parameters.AddWithValue("@fecha", jor.Fecha)
            cmdInsertarJornada.Parameters.AddWithValue("@Horas", jor.HorasJornada)
            cmdInsertarJornada.Parameters.AddWithValue("@Estado", jor.Estado)
            Dim nFilas As Integer = cmdInsertarJornada.ExecuteNonQuery
            If nFilas = 0 Then
                Return "No se ha podido añadir jornadas"
            Else
                Return "Se ha añadido la jornada"
            End If
        Catch ex As Exception
            Return "Error" & ex.Message
        Finally
            conexion.Close()
        End Try
    End Function

    Public Function totalHorasAlumno(dni As String) As String
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "No existe ningún alumno con ese DNI"
        End If
        Dim horasTotales As Integer = 0
        Dim conexion As New SqlConnection(cadenaConexion)
        ' Dim dniAlum As String = ComprobarDni(jorHoras.DniAlumno)
        Dim sql As String = "Select Horas From Jornada Where dniAlumno = @dni;"
        Try
            conexion.Open()
            Dim cmdHorasToral As New SqlCommand(sql, conexion)
            cmdHorasToral.Parameters.AddWithValue("@dni", alumno.Dni)
            Dim drHorasAlum As SqlDataReader = cmdHorasToral.ExecuteReader()

            While drHorasAlum.Read
                horasTotales = horasTotales + drHorasAlum("Horas")

            End While
            drHorasAlum.Close()
        Catch ex As Exception
            Return "Error: " & ex.Message
        Finally
            conexion.Close()
        End Try
        Return "Las horas totales del alumno con el DNI: " & alumno.Dni & " son " & horasTotales
    End Function

    Public Function ComprobarDni(dni As String) As String
        Dim letras As String = "TRWAGMYFPDXBNJZSQVHLCKE"

        If dni.Length <> 9 Then
            Return "Error: Longitud incorrecta"
        End If

        Dim numero As Integer
        If Not Integer.TryParse(dni.Substring(0, 8), numero) Then
            Return "Error: Los primeros 8 caracteres deben ser números"
        End If

        Dim letra As Char = Char.ToUpper(dni(8))
        Dim letraCorrecta As Char = letras(numero Mod 23)

        If letra = letraCorrecta Then
            Return "DNI válido"
        Else
            Return "Error: DNI inválido. La letra correcta es " & letraCorrecta
        End If
    End Function

    Public Function ModuloDeAlumno(alum As Alumno) As List(Of Modulo)
        Dim listaModulos As New List(Of Modulo)
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim alumno As Alumno = AlumnoPorDni(alum.Dni)
        Dim sql As String = "SELECT modulo.nombre FROM modulo INNER JOIN ciclo ON modulo.codigoCiclo = ciclo.codigoCiclo INNER JOIN alumno ON alumno.codigoCiclo = ciclo.codigoCiclo WHERE alumno.dni = @dni;"
        Try
            conexion.Open()
            Dim cmdModulos As New SqlCommand(sql, conexion)
            cmdModulos.Parameters.AddWithValue("@dni", alumno.Dni)
            Dim drModulos As SqlDataReader = cmdModulos.ExecuteReader
            If Not drModulos.HasRows Then
                drModulos.Close()
                Return Nothing
            Else
                ' HACEER UN BUCLE PARA METER LOS MODULOS DE EL ALUMNO
            End If

        Catch ex As Exception
        Finally
            conexion.Close()

        End Try


    End Function

    Public Function insertarTareaAlumno(task As Tarea) As String
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim mensaje As String = comprobarSiTareaExisteEnAlumno(task.CodigoTarea, task.FechaJornada, task.DniAlumno)
        If mensaje.Contains("Error") Then
            Return mensaje
        End If
        Dim sql As String = "Insert into tarea(codigotarea, dnialumno, fechajornada, descripcion, horas) values (@codigotarea, @dnialumno, @fechajornada, @descripcion, @horas;"

        Try
            conexion.Open()
            Dim cmdInsertarTarea As New SqlCommand(sql, conexion)
            cmdInsertarTarea.Parameters.AddWithValue("@dnialumno", task.DniAlumno)

            Dim nFilas As Integer = cmdInsertarTarea.ExecuteNonQuery
            If nFilas = 0 Then
                Return "Error: No se ha podido añadir la tarea"
            Else
                Return "Se ha añadido la tarea al alumno con el DNI: " & task.DniAlumno
            End If


        Catch ex As Exception
            Return "Error: " & ex.Message
        Finally
            conexion.Close()
        End Try


    End Function

    'QUE SE PUEDA MODIFICAR UNA TAREA YA CREADA PERO SOLO LA DESCRIPCION Y LAS HORAS
    Public Function comprobarSiTareaExisteEnAlumno(codigoTarea As String, fechaTarea As Date, dni As String) As String
        Dim dniBien As String = ComprobarDni(dni)
        If dniBien.Contains("Error") Then ' todo la letra A no es valida en DNI
            Return "Error: DNI no válido"
        End If
        Dim alum As Alumno = AlumnoPorDni(dni)
        If alum Is Nothing Then
            Return "Error: el DNI no está en la base de datos"
        End If
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "Select dni from Tarea Where codigotarea = @codigotarea, fechatarea = @fechatarea, dniAlumno = @dnialumno;"

        Try
            conexion.Open()
            Dim cmdComprobarTarea As New SqlCommand(sql, conexion)
            cmdComprobarTarea.Parameters.AddWithValue("@dnialumno", dni)
            ' todo otros parámetrs
            Dim drTarea As SqlDataReader = cmdComprobarTarea.ExecuteReader()
            If Not drTarea.HasRows Then
                Return "El alumno con el DNI " & alum.Dni & " no tiene esa tarea"
            Else
                Return "Error: El alumno con el DNI " & alum.Dni & " tiene esa tarea"
            End If
        Catch ex As Exception
            Return "Error:" & ex.Message
        Finally
            conexion.Close()
        End Try


    End Function
    Public Function ComprobarEmail(email As String) As String
        If String.IsNullOrWhiteSpace(email) Then
            Return "Error: Email vacío"
        End If

        email = email.Trim()

        If Not email.Contains("@") Then
            Return "Error: El email debe contener @"
        End If

        If Not email.Contains(".") Then
            Return "Error: El email debe contener un punto"
        End If
        If email.IndexOf("@") > email.LastIndexOf(".") Then
            Return "Error, no hay punto tras @"
        End If
        Return "Email válido"
    End Function
    Public Function ComprobarTelefono(tlf As String) As String
        If String.IsNullOrWhiteSpace(tlf) Then
            Return "Error: Teléfono vacío"
        End If

        tlf = tlf.Trim()

        If tlf.Length <> 9 Then
            Return "Error: El teléfono debe tener 9 dígitos"
        End If

        If Not Char.IsDigit(tlf(0)) Then
            Return "Error: El teléfono debe empezar por un número"
        End If

        If tlf(0) <> "6"c AndAlso tlf(0) <> "7"c Then
            Return "Error: El teléfono debe empezar por 6 o 7"
        End If

        If Not tlf.All(Function(c) Char.IsDigit(c)) Then
            Return "Error: El teléfono solo puede contener números"
        End If

        Return "Teléfono válido"
    End Function


End Class
