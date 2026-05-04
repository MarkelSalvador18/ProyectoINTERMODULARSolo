Public Class RA_Tarea

    Public Property CodigoTarea As String
    Public Property NumeroRa As Integer
    Public Property CodigoModulo As String
    Public Property CodigoCiclo As String
    Public Property FechaJornada As Date
    Public Property DniAlumno As String

    Public Sub New(codigoTarea As String, numeroRa As Integer, codigoModulo As String, codigoCiclo As String, fechaJornada As Date, dniAlumno As String)

        Me.CodigoCiclo = codigoCiclo
        Me.CodigoModulo = codigoModulo
        Me.CodigoTarea = codigoTarea
        Me.NumeroRa = numeroRa
        Me.FechaJornada = fechaJornada
        Me.DniAlumno = dniAlumno

    End Sub
    Public Sub New()

    End Sub


End Class
