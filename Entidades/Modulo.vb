Public Class Modulo

    Public Property CodigoModulo As String
    Public Property CodigoCiclo As String
    Public Property NombreModulo As String
    Public Sub New(codigoModulo As String, codigoCiclo As String, nombreModulo As String)
        Me.CodigoCiclo = codigoCiclo
        Me.CodigoModulo = codigoModulo
        Me.NombreModulo = nombreModulo
    End Sub
    Public Sub New()

    End Sub

End Class
