Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
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
                        Return "No se ha podido añadir al alumno"
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

    Public Function TotalDiasCotizadosAlumno(dni As String) As String
        Dim alumno As Alumno = AlumnoPorDni(dni)
        If alumno Is Nothing Then
            Return "No existe ningún alumno con ese DNI"
        End If
        Dim fechas As New List(Of String)
        Dim sql As String = "SELECT FECHA FROM JORNADA WHERE DNIALUMNO = @dni ORDER BY FECHA;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using command As New SqlCommand(sql, conexion)
                    command.Parameters.AddWithValue("@dni", alumno.Dni)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim fecha As Date = Convert.ToDateTime(reader("FECHA"))
                            fechas.Add(fecha.ToString("yyyy-MM-dd"))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
        Return "Total " & fechas.Count & " días: " & String.Join(", ", fechas)
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

    Public Function ValidarHorasTareasJornada(dni As String, fecha As Date) As String
        Dim sqlHorasJornada As String = "SELECT HORAS FROM JORNADA WHERE DNIALUMNO = @dni AND FECHA = @fecha;"
        Dim sqlSumaTareas As String = "SELECT ISNULL(SUM(HORAS), 0) FROM TAREA WHERE DNIALUMNO = @dni AND FECHAJORNADA = @fecha;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Dim horasJornada As Integer
                Using cmd As New SqlCommand(sqlHorasJornada, conexion)
                    cmd.Parameters.AddWithValue("@dni", dni)
                    cmd.Parameters.AddWithValue("@fecha", fecha)
                    Dim resultado As Object = cmd.ExecuteScalar()
                    If resultado Is Nothing OrElse IsDBNull(resultado) Then
                        Return "Error: No existe la jornada para ese alumno en esa fecha"
                    End If
                    horasJornada = Convert.ToInt32(resultado)
                End Using
                Dim sumaHorasTareas As Integer
                Using cmd As New SqlCommand(sqlSumaTareas, conexion)
                    cmd.Parameters.AddWithValue("@dni", dni)
                    cmd.Parameters.AddWithValue("@fecha", fecha)
                    sumaHorasTareas = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
                If sumaHorasTareas >= horasJornada Then
                    Return "Error: Las horas de las tareas superan o igualan las horas registradas en la jornada"
                End If
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
        Return "Horas válidas"
    End Function

    Public Function insertarTareaAlumno(tarea As Tarea) As String
        Dim mensaje As String = comprobarSiTareaExisteEnAlumno(tarea.CodigoTarea, tarea.FechaJornada, tarea.DniAlumno)
        If mensaje.Contains("Error") Then
            Return mensaje
        End If
        Dim validacionHoras As String = ValidarHorasTareasJornada(tarea.DniAlumno, tarea.FechaJornada)
        If validacionHoras.Contains("Error") Then
            Return validacionHoras
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

    Public Function BorrarJornada(fecha As Date, dniAlumno As String) As String
        If fecha = Nothing Then
            Return "Error: La fecha esta vacío"
        End If
        If dniAlumno Is Nothing Then
            Return "Error: El dni esta vacio"
        End If

        Dim sqlBuscarJornada As String = "Select fecha, dniAlumno from jornada where fecha = @fecha and dnialumno = @dnialumno;"
        Dim sqlBuscarTareas As String = "Select 1 from tarea where fechajornada = @fecha and dnialumno = @dnialumno;"
        Dim sqlBorrar As String = "Delete from jornada where fecha = @fecha and dnialumno = @dnialumno;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using cmdBuscarJornada As New SqlCommand(sqlBuscarJornada, conexion)
                    cmdBuscarJornada.Parameters.AddWithValue("@fecha", fecha)
                    cmdBuscarJornada.Parameters.AddWithValue("@dnialumno", dniAlumno)
                    Using drJornada As SqlDataReader = cmdBuscarJornada.ExecuteReader()
                        If Not drJornada.HasRows Then
                            Return "Error: La jornada no existe"
                        End If
                    End Using
                End Using
                Using cmdBuscarTarea As New SqlCommand(sqlBuscarTareas, conexion)
                    cmdBuscarTarea.Parameters.AddWithValue("@fecha", fecha)
                    cmdBuscarTarea.Parameters.AddWithValue("@dnialumno", dniAlumno)
                    Using drTarea As SqlDataReader = cmdBuscarTarea.ExecuteReader()
                        If drTarea.HasRows Then
                            Return "Error: La jornada tiene tareas"
                        End If
                    End Using
                End Using
                Using cmdBorrarJornada As New SqlCommand(sqlBorrar, conexion)
                    cmdBorrarJornada.Parameters.AddWithValue("@fecha", fecha)
                    cmdBorrarJornada.Parameters.AddWithValue("@dnialumno", dniAlumno)
                    Dim filasBorradas As Integer = cmdBorrarJornada.ExecuteNonQuery()
                    If filasBorradas = 0 Then
                        Return "Error: No se ha podido borrar"
                    Else
                        Return "Se ha borrado la Jornada"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function


    Public Function ObtenerJornadasAlumno(DniAlumno As String) As List(Of Jornada)
        If DniAlumno Is Nothing Then
            Return Nothing
        End If

        Dim listaJornadas As New List(Of Jornada)
        Dim sqlAlumnoExiste As String = "SELECT COUNT(*) FROM ALUMNO WHERE DNI = @DniAlumno;"
        Dim sqlJornadasPorAlumno As String = "SELECT Fecha, DniAlumno, Horas, Estado FROM JORNADA WHERE DniAlumno = @DniAlumno ORDER BY Fecha DESC;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                Using commandAlumnoCheck As New SqlCommand(sqlAlumnoExiste, conexion)
                    commandAlumnoCheck.Parameters.AddWithValue("@DniAlumno", DniAlumno)
                    Dim alumnoCount As Integer = commandAlumnoCheck.ExecuteScalar()
                    If alumnoCount = 0 Then
                        Return Nothing
                    End If
                End Using

                Using cmd As New SqlCommand(sqlJornadasPorAlumno, conexion)
                    cmd.Parameters.AddWithValue("@DniAlumno", DniAlumno)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim j As New Jornada With {
                                .Fecha = reader("Fecha"),
                                .DniAlumno = reader("DniAlumno").ToString(),
                                .HorasJornada = Convert.ToInt32(reader("Horas")),
                                .Estado = reader("Estado").ToString()
                            }
                            listaJornadas.Add(j)
                        End While
                    End Using
                End Using

            End Using

            Return listaJornadas

        Catch ex As Exception
            Return Nothing
        End Try

    End Function


    Public Function ObtenerTareasSemanales(dniAlumno As String, fechaInicio As Date, fechaFin As Date) As List(Of Tarea)

        Dim listaTareas As New List(Of Tarea)

        If dniAlumno Is Nothing Then
            Return listaTareas
        End If

        Dim sql As String = "SELECT CodigoTarea, FechaJornada, Descripcion, Horas, DniAlumno " &
                            "FROM TAREA " &
                            "WHERE DniAlumno = @dniAlumno AND FechaJornada BETWEEN @fechaInicio AND @fechaFin " &
                            "ORDER BY FechaJornada ASC, CodigoTarea ASC;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                Using cmd As New SqlCommand(sql, conexion)
                    cmd.Parameters.AddWithValue("@dniAlumno", dniAlumno)
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio)
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim t As New Tarea With {
                                .CodigoTarea = reader("CodigoTarea").ToString(),
                                .FechaJornada = Convert.ToDateTime(reader("FechaJornada")),
                                .DescripcionTarea = reader("Descripcion").ToString(),
                                .HorasTarea = Convert.ToInt32(reader("Horas")),
                                .DniAlumno = reader("DniAlumno").ToString()
                            }

                            listaTareas.Add(t)
                        End While
                    End Using
                End Using

            End Using

            Return listaTareas

        Catch ex As Exception
            Return New List(Of Tarea)
        End Try

    End Function



    Public Function ObtenerTareasDeJornada(fecha As Date, dni As String) As List(Of Tarea)

        Dim listaTareas As New List(Of Tarea)

        If dni Is Nothing Then
            Return listaTareas
        End If

        Dim sql As String = "SELECT CodigoTarea, FechaJornada, Descripcion, Horas, DniAlumno " &
                            "FROM TAREA " &
                            "WHERE FechaJornada = @fecha AND DniAlumno = @dni " &
                            "ORDER BY CodigoTarea ASC;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                Using cmd As New SqlCommand(sql, conexion)
                    cmd.Parameters.AddWithValue("@fecha", fecha)
                    cmd.Parameters.AddWithValue("@dni", dni)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim t As New Tarea With {
                                .CodigoTarea = reader("CodigoTarea").ToString(),
                                .FechaJornada = Convert.ToDateTime(reader("FechaJornada")),
                                .DescripcionTarea = reader("Descripcion").ToString(),
                                .HorasTarea = Convert.ToInt32(reader("Horas")),
                                .DniAlumno = reader("DniAlumno").ToString()
                            }

                            listaTareas.Add(t)
                        End While
                    End Using
                End Using

            End Using

            Return listaTareas

        Catch ex As Exception
            Return New List(Of Tarea)
        End Try

    End Function


    Public Function ModificarTarea(tarea As Tarea) As String

        If tarea Is Nothing Then
            Return "Error: La tarea no puede ser nula."
        End If

        If String.IsNullOrWhiteSpace(tarea.DescripcionTarea) Then
            Return "Error: La descripción no puede estar vacía."
        End If

        If tarea.HorasTarea < 0 OrElse tarea.HorasTarea > 8 Then
            Return "Error: Las horas deben estar entre 0 y 8."
        End If

        Dim sqlExiste As String = "SELECT COUNT(*) FROM TAREA WHERE CodigoTarea = @codigo;"
        Dim sqlUpdate As String = "UPDATE TAREA SET Descripcion = @descripcion, Horas = @horas WHERE CodigoTarea = @codigo;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                ' Comprobar si existe la tarea
                Using cmdExiste As New SqlCommand(sqlExiste, conexion)
                    cmdExiste.Parameters.AddWithValue("@codigo", tarea.CodigoTarea)
                    Dim count As Integer = Convert.ToInt32(cmdExiste.ExecuteScalar())

                    If count = 0 Then
                        Return "Error: La tarea no existe."
                    End If
                End Using

                ' Actualizar solo los campos permitidos
                Using cmdUpdate As New SqlCommand(sqlUpdate, conexion)
                    cmdUpdate.Parameters.AddWithValue("@descripcion", tarea.DescripcionTarea)
                    cmdUpdate.Parameters.AddWithValue("@horas", tarea.HorasTarea)
                    cmdUpdate.Parameters.AddWithValue("@codigo", tarea.CodigoTarea)

                    Dim filasAfectadas As Integer = cmdUpdate.ExecuteNonQuery()

                    If filasAfectadas > 0 Then
                        Return "Tarea modificada correctamente."
                    Else
                        Return "Error: No se pudo modificar la tarea."
                    End If
                End Using

            End Using

        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try

    End Function




    Public Function BorrarTarea(codigoTarea As String, fecha As Date, dni As String) As String

        If String.IsNullOrWhiteSpace(codigoTarea) OrElse String.IsNullOrWhiteSpace(dni) Then
            Return "Error: Datos insuficientes para borrar la tarea."
        End If

        Dim sqlExiste As String = "SELECT COUNT(*) FROM TAREA WHERE CodigoTarea = @codigo AND FechaJornada = @fecha AND DniAlumno = @dni;"
        Dim sqlDeleteRAs As String = "DELETE FROM RA_TAREA WHERE CodigoTarea = @codigo;"
        Dim sqlDeleteTarea As String = "DELETE FROM TAREA WHERE CodigoTarea = @codigo AND FechaJornada = @fecha AND DniAlumno = @dni;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                ' Comprobar si existe la tarea
                Using cmdExiste As New SqlCommand(sqlExiste, conexion)
                    cmdExiste.Parameters.AddWithValue("@codigo", codigoTarea)
                    cmdExiste.Parameters.AddWithValue("@fecha", fecha)
                    cmdExiste.Parameters.AddWithValue("@dni", dni)

                    Dim count As Integer = Convert.ToInt32(cmdExiste.ExecuteScalar())
                    If count = 0 Then
                        Return "Error: La tarea no existe."
                    End If
                End Using

                ' Borrar relaciones en RA_TAREA (si existen)
                Using cmdDeleteRAs As New SqlCommand(sqlDeleteRAs, conexion)
                    cmdDeleteRAs.Parameters.AddWithValue("@codigo", codigoTarea)
                    cmdDeleteRAs.ExecuteNonQuery()
                End Using

                ' Borrar la tarea
                Using cmdDeleteTarea As New SqlCommand(sqlDeleteTarea, conexion)
                    cmdDeleteTarea.Parameters.AddWithValue("@codigo", codigoTarea)
                    cmdDeleteTarea.Parameters.AddWithValue("@fecha", fecha)
                    cmdDeleteTarea.Parameters.AddWithValue("@dni", dni)

                    Dim filas As Integer = cmdDeleteTarea.ExecuteNonQuery()

                    If filas > 0 Then
                        Return "Tarea eliminada correctamente."
                    Else
                        Return "Error: No se pudo eliminar la tarea."
                    End If
                End Using

            End Using

        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try

    End Function




    Public Function GenerarCodigoTarea(fecha As Date, dni As String) As Integer

        Dim siguienteCodigo As Integer = 1

        If String.IsNullOrWhiteSpace(dni) Then
            Return siguienteCodigo
        End If

        Dim sql As String = "SELECT ISNULL(MAX(CodigoTarea), 0) FROM TAREA " &
                            "WHERE FechaJornada = @fecha AND DniAlumno = @dni;"

        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()

                Using cmd As New SqlCommand(sql, conexion)
                    cmd.Parameters.AddWithValue("@fecha", fecha)
                    cmd.Parameters.AddWithValue("@dni", dni)

                    Dim maxCodigo As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    siguienteCodigo = maxCodigo + 1
                End Using

            End Using

            Return siguienteCodigo

        Catch ex As Exception
            Return 1
        End Try

    End Function

    Public Function JornadaExiste(fecha As Date, dni As String) As Boolean
        If dni Is Nothing Then
            Return False
        End If
        If fecha = Nothing Then
            Return Nothing
        End If
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "Select fecha, dnialumno from jornada where fecha = @fecha and dnialumno = @dni;"
        Try
            conexion.Open()
            Dim cmdJornada As New SqlCommand(sql, conexion)
            cmdJornada.Parameters.AddWithValue("@fecha", fecha)
            cmdJornada.Parameters.AddWithValue("@dni", dni)
            Dim drExisteJornada As SqlDataReader = cmdJornada.ExecuteReader
            If Not drExisteJornada.HasRows Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            Return False
        Finally
            conexion.Close()
        End Try
        Return False
    End Function

    Public Function ObtenerJornadaSemana(dniAlumno As String, fechaInicio As Date, fechaFin As Date) As List(Of Jornada)
        Dim listaJornada As New List(Of Jornada)
        Dim conexion As New SqlConnection(cadenaConexion)
        Dim sql As String = "Select * from jornada where dniAlumno = @dniAlumno and fecha BETWEEN @fechaInicio and @fechaFin
                            order by fecha asc;"
        Try
            conexion.Open()
            Dim cmdJornadaSemanas As New SqlCommand(sql, conexion)
            cmdJornadaSemanas.Parameters.AddWithValue("@dnialumno", dniAlumno)
            cmdJornadaSemanas.Parameters.AddWithValue("@fechainicio", fechaInicio)
            cmdJornadaSemanas.Parameters.AddWithValue("@fechafin", fechaFin)
            Dim drJornada As SqlDataReader = cmdJornadaSemanas.ExecuteReader
            If Not drJornada.HasRows Then
                Return listaJornada
            Else
                Return listaJornada
            End If
        Catch ex As Exception
            Return listaJornada
        End Try
    End Function


    Public Function HorasPorGrupo(codigoCiclo As String) As Dictionary(Of String, Integer)
        If codigoCiclo Is Nothing Then
            Return Nothing
        End If
        Dim sqlCicloExiste As String = "SELECT COUNT(*) FROM CICLO WHERE CODIGOCICLO = @codigoCiclo;"
        Dim sqlHorasPorAlumno As String =
            "SELECT A.DNI, ISNULL(SUM(J.HORAS), 0) AS TotalHoras " &
            "FROM ALUMNO A " &
            "LEFT JOIN JORNADA J ON A.DNI = J.DNIALUMNO " &
            "WHERE A.CODIGOCICLO = @codigoCiclo " &
            "GROUP BY A.DNI;"
        Try
            Using conexion As New SqlConnection(cadenaConexion)
                conexion.Open()
                Using cmdCiclo As New SqlCommand(sqlCicloExiste, conexion)
                    cmdCiclo.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
                    Dim cicloCount As Integer = Convert.ToInt32(cmdCiclo.ExecuteScalar())
                    If cicloCount = 0 Then
                        Return Nothing
                    End If
                End Using
                Dim resultado As New Dictionary(Of String, Integer)
                Using cmd As New SqlCommand(sqlHorasPorAlumno, conexion)
                    cmd.Parameters.AddWithValue("@codigoCiclo", codigoCiclo)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim dni As String = reader("DNI").ToString()
                            Dim horas As Integer = Convert.ToInt32(reader("TotalHoras"))
                            resultado.Add(dni, horas)
                        End While
                    End Using
                End Using
                Return resultado
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
