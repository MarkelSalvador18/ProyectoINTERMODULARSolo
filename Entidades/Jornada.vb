Imports System.Security.AccessControl

Public Class Jornada

    Public Tareas As New List(Of Tarea)

    Public Property Fecha As Date
    Public Property DniAlumno As String
    Public Property HorasJornada As Integer
    Public Property Estado As String
    Public Sub New(fecha As Date, dniAlumno As String, horasJornada As Integer, estado As String)
        Me.Fecha = fecha
        Me.DniAlumno = dniAlumno
        Me.HorasJornada = horasJornada
        Me.Estado = estado
    End Sub
    Public Sub New()

    End Sub

End Class
