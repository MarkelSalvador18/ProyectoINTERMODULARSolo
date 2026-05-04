Public Class Ciclo

    Private Ciclos As New List(Of Ciclo)

    Public Property CodigoCiclo As String
    Public Property NombreCiclo As String
    Public Sub New(codigoCiclo As String, nombreCiclo As String)
        Me.CodigoCiclo = codigoCiclo
        Me.NombreCiclo = nombreCiclo
    End Sub


    Public Sub New()

    End Sub

End Class
