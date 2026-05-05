Public Class Tarea
    Implements IEquatable(Of Tarea)

    Public RAs As New List(Of RA)
    Public Property CodigoTarea As String
    Public Property FechaJornada As Date
    Public Property DescripcionTarea As String
    Public Property HorasTarea As Integer
    Public Property DniAlumno As String
    Public Sub New(codigoTarea As String, fechaJornada As Date, descripcionTarea As String, horasTarea As Integer, dniAlumno As String)
        Me.CodigoTarea = codigoTarea
        Me.FechaJornada = fechaJornada
        Me.DescripcionTarea = descripcionTarea
        Me.HorasTarea = horasTarea
        Me.DniAlumno = dniAlumno
    End Sub
    Public Sub New()

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Equals(TryCast(obj, Tarea))
    End Function

    Public Overloads Function Equals(other As Tarea) As Boolean Implements IEquatable(Of Tarea).Equals
        Return other IsNot Nothing AndAlso
               CodigoTarea.ToUpper = other.CodigoTarea.ToUpper AndAlso
               FechaJornada = other.FechaJornada
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (CodigoTarea, FechaJornada).GetHashCode()
    End Function

    Public Shared Operator =(left As Tarea, right As Tarea) As Boolean
        Return EqualityComparer(Of Tarea).Default.Equals(left, right)
    End Operator

    Public Shared Operator <>(left As Tarea, right As Tarea) As Boolean
        Return Not left = right
    End Operator
End Class
