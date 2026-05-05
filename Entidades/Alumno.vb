Imports System.Security.Policy

Public Class Alumno
    Implements IEquatable(Of Alumno)

    Public Jornadas As New List(Of Jornada)
    Public Property Dni As String
    Public Property Email As String
    Public Property Telefono As String
    Public Property Nombre As String
    Public Property Apellido1 As String
    Public Property Apellido2 As String
    Public Property CodigoCiclo As String

    Public Sub New(dni As String)
        Me.Dni = dni
    End Sub
    Public Sub New(dni As String, email As String, telefono As String, nombre As String, apellido1 As String, apellido2 As String, codigoCiclo As String)
        Me.Dni = dni
        Me.Email = email
        Me.Telefono = telefono
        Me.Nombre = nombre
        Me.Apellido1 = apellido1
        Me.Apellido2 = apellido2
        Me.CodigoCiclo = codigoCiclo
    End Sub
    Public Sub New(dni As String, email As String, telefono As String, nombre As String, apellido1 As String, codigoCiclo As String)
        Me.Dni = dni
        Me.Email = email
        Me.Telefono = telefono
        Me.Nombre = nombre
        Me.Apellido1 = apellido1
        Me.CodigoCiclo = codigoCiclo
    End Sub
    Public Sub New()

    End Sub



    Public Overrides Function Equals(obj As Object) As Boolean
        Return Equals(TryCast(obj, Alumno))
    End Function

    Public Overloads Function Equals(other As Alumno) As Boolean Implements IEquatable(Of Alumno).Equals
        Return other IsNot Nothing AndAlso
               Dni.ToUpper() = other.Dni.ToUpper()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Dim hashCode As Long = 963633130
        hashCode = (hashCode * -1521134295 + EqualityComparer(Of String).Default.GetHashCode(Dni)).GetHashCode()
        Return hashCode
    End Function

    Public Shared Operator =(left As Alumno, right As Alumno) As Boolean
        Return EqualityComparer(Of Alumno).Default.Equals(left, right)
    End Operator

    Public Shared Operator <>(left As Alumno, right As Alumno) As Boolean
        Return Not left = right
    End Operator
End Class
