Public Class Ciclo

    Public Alumnos As New List(Of Alumno)
    Public Modulos As New List(Of Modulo)

    Public Property CodigoCiclo As String
    Public Property NombreCiclo As String
    Public Sub New(codigoCiclo As String, nombreCiclo As String)
        Me.CodigoCiclo = codigoCiclo
        Me.NombreCiclo = nombreCiclo
    End Sub


    Public Sub New()

    End Sub

End Class
