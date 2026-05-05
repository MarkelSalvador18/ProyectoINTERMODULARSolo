Public Class RA

    Public Tareas As New List(Of Tarea)
    Public Property Numero As Integer
    Public Property CodigoModulo As String
    Public Property CodigoCiclo As String
    Public Property NombreRa As String
    Public Property DescripcionRa As String

    Public Sub New(numero As Integer, codigoModulo As String, codigoCiclo As String, nombreRa As String, descripcionRa As String)
        Me.Numero = numero
        Me.CodigoModulo = codigoModulo
        Me.CodigoCiclo = codigoCiclo
        Me.NombreRa = nombreRa
        Me.DescripcionRa = descripcionRa
    End Sub
    Public Sub New()

    End Sub

End Class
