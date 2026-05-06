Imports System.Data.SqlClient
Imports Entidades

Public Class GestionAplicacion

    Private cadenaConexion As String

    Private Const ServidorSql As String = ".\SQLEXPRESS"
    Private Const NombreBaseDatos As String = "g1_GESTFCT"
    Private Const LetrasDni As String = "TRWAGMYFPDXBNJZSQVHLCKE"
    Private Const ModuloDni As Integer = 23

    Public Sub New()
        cadenaConexion = $"Data Source={ServidorSql};Initial Catalog={NombreBaseDatos};Integrated Security=True"
    End Sub

    Public Function DarAltaAlumnado(alumno As Alumno) As String
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
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@Nombre", alumno.Nombre)
                    command.Parameters.AddWithValue("@DNI", alumno.Dni)
                    command.Parameters.AddWithValue("@Telefono", alumno.Telefono)
                    command.Parameters.AddWithValue("@Email", alumno.Email)
                    command.Parameters.AddWithValue("@Apellido1", alumno.Apellido1)
                    command.Parameters.AddWithValue("@CodigoCiclo", alumno.CodigoCiclo)
                    Dim affectedRows As Integer = command.ExecuteNonQuery
                    If affectedRows = 0 Then
                        Return "No se ha podido añadir al alumni"
                    Else
                        Return "Se ha añadido al alumno"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function AlumnoPorDni(dni As String) As Alumno
        Dim sql As String = "SELECT Nombre, DNI, Telefono, Email, Apellido1, CodigoCiclo FROM ALUMNO Where dni = @dni;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dni", dni)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If Not reader.HasRows Then
                            Return Nothing
                        End If
                        reader.Read()
                        ' FIXME: nombre y email intercambiados en el constructor — los datos se asignan incorrectamente
                        Dim alumno As Alumno = New Alumno(reader("dni"), reader("email"), reader("telefono"), reader("nombre"), reader("apellido1"), reader("codigociclo"))
                        Return alumno
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function DarBajaAlumnado(dni As String) As String
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "No existe ningún alumno con ese DNI"
        End If
        Dim sql As String = "DELETE FROM ALUMNO WHERE DNI = @dni"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dni", alumno.Dni)
                    Dim affectedRows As Integer = command.ExecuteNonQuery()
                    If affectedRows = 0 Then
                        Return "No se pudo borrar el alumno"
                    Else
                        Return "Alumno borrado correctamente: " & alumno.Nombre & " " & alumno.Apellido1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function BorrarYComprobarInformacionAlumno(alumno As Alumno) As String
        If alumno Is Nothing Then
            Return "Error: El alumno no existe en la base de datos"
        End If
        Dim sql As String = "Select dni from alumno where dni = @dni;"
        Dim sqlJornadaCheck As String = "Select * from Jornada where dnialumno = @dni;"
        Dim sqlDelete As String = "delete from alumno where dni = @dni;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim alumnoExiste As Boolean
                Using commandAlumnoExiste As New SqlCommand(sql, conexion)
                    commandAlumnoExiste.Parameters.AddWithValue("@dni", alumno.Dni)
                    Using readerAlumnoExiste As SqlDataReader = commandAlumnoExiste.ExecuteReader()
                        alumnoExiste = readerAlumnoExiste.HasRows
                    End Using
                End Using
                If Not alumnoExiste Then
                    Return "Error: No se ha podido el alumno porque no tiene información"
                End If
                Dim tieneJornadas As Boolean
                Using commandJornadaExiste As New SqlCommand(sqlJornadaCheck, conexion)
                    commandJornadaExiste.Parameters.AddWithValue("@dni", alumno.Dni)
                    Using readerJornadas As SqlDataReader = commandJornadaExiste.ExecuteReader()
                        tieneJornadas = readerJornadas.HasRows
                    End Using
                End Using
                If tieneJornadas Then
                    Return "El alumno con el DNI: " & alumno.Dni & " tiene jornadas"
                End If
                Using commandEliminar As New SqlCommand(sqlDelete, conexion)
                    commandEliminar.Parameters.AddWithValue("@dni", alumno.Dni)
                    Dim affectedRows As Integer = commandEliminar.ExecuteNonQuery
                    If affectedRows = 0 Then
                        Return "No se ha podido borrar el alumno"
                    Else
                        Return "El alumno ha sido borrado"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function AlumnoPorCiclo(codigoCiclo As String) As List(Of Alumno)
        If codigoCiclo Is Nothing Then
            Return Nothing
        End If
        Dim listaAlumnos As New List(Of Alumno)
        Dim sql As String = "SELECT COUNT(*) FROM CICLO WHERE codigoCiclo = @codigoCiclo;"
        Dim sqlAlumnosPorCiclo As String = "SELECT nombre, dni, email, telefono, apellido1, codigoCiclo FROM ALUMNO WHERE codigoCiclo = @codigociclo;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using commandCycleCheck As New SqlCommand(sql, conexion)
                    commandCycleCheck.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
                    Dim cycleCount As Integer = commandCycleCheck.ExecuteScalar()
                    If cycleCount = 0 Then
                        Return Nothing
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using commandAlumnosPorCiclo As New SqlCommand(sqlAlumnosPorCiclo, conexion)
                    commandAlumnosPorCiclo.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
                    Using reader As SqlDataReader = commandAlumnosPorCiclo.ExecuteReader()
                        While reader.Read()
                            Dim alumno As New Alumno(reader("dni"), reader("nombre"), reader("telefono"), reader("email"), reader("apellido1"), reader("codigoCiclo"))
                            listaAlumnos.Add(alumno)
                        End While
                    End Using
                End Using
            End Using
            Return listaAlumnos
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function insertarJornadaAlumno(jornada As Jornada) As String
        Dim sql As String = "Insert into Jornada (dniAlumno, fecha, Horas, Estado) values(@dniAlumno, @fecha, @Horas, @Estado);"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dniAlumno", jornada.DniAlumno)
                    command.Parameters.AddWithValue("@fecha", jornada.Fecha)
                    command.Parameters.AddWithValue("@Horas", jornada.HorasJornada)
                    command.Parameters.AddWithValue("@Estado", jornada.Estado)
                    Dim affectedRows As Integer = command.ExecuteNonQuery
                    If affectedRows = 0 Then
                        Return "No se ha podido añadir jornadas"
                    Else
                        Return "Se ha añadido la jornada"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function totalHorasAlumno(dni As String) As String
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "No existe ningún alumno con ese DNI"
        End If
        Dim horasTotales As Integer = 0
        Dim sql As String = "Select Horas From Jornada Where dniAlumno = @dni;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dni", alumno.Dni)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read
                            horasTotales = horasTotales + reader("Horas")
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
        Return "Las horas totales del alumno con el DNI: " & alumno.Dni & " son " & horasTotales
    End Function

    Public Function ComprobarDni(dni As String) As String
        If dni.Length <> 9 Then
            Return "Error: Longitud incorrecta"
        End If

        Dim numero As Integer
        If Not Integer.TryParse(dni.Substring(0, 8), numero) Then
            Return "Error: Los primeros 8 caracteres deben ser números"
        End If

        Dim letra As Char = Char.ToUpper(dni(8))
        Dim letraCorrecta As Char = LetrasDni(numero Mod ModuloDni)

        If letra = letraCorrecta Then
            Return "DNI válido"
        Else
            Return "Error: DNI inválido. La letra correcta es " & letraCorrecta
        End If
    End Function

    Public Function ModuloDeAlumno(alumno As Alumno) As List(Of Modulo)
        Dim listaModulos As New List(Of Modulo)
        Dim alumnoVerificado As Alumno = AlumnoPorDni(alumno.Dni)
        Dim sql As String = "SELECT modulo.codigoModulo, modulo.nombre, modulo.codigoCiclo FROM modulo INNER JOIN ciclo ON modulo.codigoCiclo = ciclo.codigoCiclo INNER JOIN alumno ON alumno.codigoCiclo = ciclo.codigoCiclo WHERE alumno.dni = @dni;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dni", alumnoVerificado.Dni)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim modulo As New Modulo()
                            modulo.CodigoModulo = reader("codigoModulo").ToString()
                            modulo.NombreModulo = reader("nombre").ToString()
                            modulo.CodigoCiclo = reader("codigoCiclo").ToString()
                            listaModulos.Add(modulo)
                        End While
                    End Using
                End Using
            End Using
            If listaModulos.Count = 0 Then
                Return Nothing
            End If
        Catch ex As Exception
            ' Silent catch preserved - no-op by design
        End Try
        Return listaModulos
    End Function

    Public Function insertarTareaAlumno(tarea As Tarea) As String
        Dim mensaje As String = comprobarSiTareaExisteEnAlumno(tarea.CodigoTarea, tarea.FechaJornada, tarea.DniAlumno)
        If mensaje.Contains("Error") Then
            Return mensaje
        End If
        Dim sql As String = "Insert into tarea(codigotarea, dnialumno, fechajornada, descripcion, horas) values (@codigotarea, @dnialumno, @fechajornada, @descripcion, @horas);"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dnialumno", tarea.DniAlumno)
                    command.Parameters.AddWithValue("@codigotarea", tarea.CodigoTarea)
                    command.Parameters.AddWithValue("@fechajornada", tarea.FechaJornada)
                    command.Parameters.AddWithValue("@descripcion", tarea.DescripcionTarea)
                    command.Parameters.AddWithValue("@horas", tarea.HorasTarea)
                    Dim affectedRows As Integer = command.ExecuteNonQuery
                    If affectedRows = 0 Then
                        Return "Error: No se ha podido añadir la tarea"
                    Else
                        Return "Se ha añadido la tarea al alumno con el DNI: " & tarea.DniAlumno
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function comprobarSiTareaExisteEnAlumno(codigoTarea As String, fechaTarea As Date, dni As String) As String
        Dim dniValidacion As String = ComprobarDni(dni)
        If dniValidacion.Contains("Error") Then
            Return "Error: DNI no válido"
        End If
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "Error: el DNI no está en la base de datos"
        End If
        Dim sql As String = "Select dni from Tarea Where codigotarea = @codigotarea AND fechajornada = @fechajornada AND dniAlumno = @dnialumno;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dnialumno", dni)
                    command.Parameters.AddWithValue("@codigotarea", codigoTarea)
                    command.Parameters.AddWithValue("@fechajornada", fechaTarea)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If Not reader.HasRows Then
                            Return "El alumno con el DNI " & alumno.Dni & " no tiene esa tarea"
                        Else
                            Return "Error: El alumno con el DNI " & alumno.Dni & " tiene esa tarea"
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
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
            Return "Error: no hay punto tras @"
        End If
        Return "Email válido"
    End Function

    Public Function ComprobarTelefono(telefono As String) As String
        If String.IsNullOrWhiteSpace(telefono) Then
            Return "Error: Teléfono vacío"
        End If

        telefono = telefono.Trim()

        If telefono.Length <> 9 Then
            Return "Error: El teléfono debe tener 9 dígitos"
        End If

        If Not Char.IsDigit(telefono(0)) Then
            Return "Error: El teléfono debe empezar por un número"
        End If

        If telefono(0) <> "6" AndAlso telefono(0) <> "7" Then
            Return "Error: El teléfono debe empezar por 6 o 7"
        End If

        If Not telefono.All(Function(c) Char.IsDigit(c)) Then
            Return "Error: El teléfono solo puede contener números"
        End If

        Return "Teléfono válido"
    End Function

    Public Function ModificarJornada(jor As Jornada) As String
        Dim sqlExists As String = "SELECT COUNT(*) FROM Jornada WHERE fecha = @fecha AND dniAlumno = @dniAlumno;"
        Dim sqlUpdate As String = "UPDATE Jornada SET Horas = @Horas, Estado = @Estado WHERE fecha = @fecha AND dniAlumno = @dniAlumno;"

        If jor.HorasJornada < 0 OrElse jor.HorasJornada > 8 Then
            Return "Error: Las horas deben estar entre 0 y 8"
        End If

        Dim estadosValidos As String() = {"PENDIENTE", "REALIZADA", "CANCELADA"}
        If Not estadosValidos.Contains(jor.Estado.ToUpper()) Then
            Return "Error: El estado debe ser PENDIENTE, REALIZADA o CANCELADA"
        End If

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using commandExists As New SqlCommand(sqlExists, conexion)
                    commandExists.Parameters.AddWithValue("@fecha", jor.Fecha)
                    commandExists.Parameters.AddWithValue("@dniAlumno", jor.DniAlumno)
                    Dim exists As Integer = commandExists.ExecuteScalar()
                    If exists = 0 Then
                        Return "Error: La jornada no existe"
                    End If
                End Using

                Using commandUpdate As New SqlCommand(sqlUpdate, conexion)
                    commandUpdate.Parameters.AddWithValue("@fecha", jor.Fecha)
                    commandUpdate.Parameters.AddWithValue("@dniAlumno", jor.DniAlumno)
                    commandUpdate.Parameters.AddWithValue("@Horas", jor.HorasJornada)
                    commandUpdate.Parameters.AddWithValue("@Estado", jor.Estado.ToUpper())
                    Dim affectedRows As Integer = commandUpdate.ExecuteNonQuery()
                    If affectedRows = 0 Then
                        Return "Error: No se ha podido modificar la jornada"
                    Else
                        Return "Jornada modificada correctamente"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

End Class
