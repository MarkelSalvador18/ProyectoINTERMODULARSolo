Imports System.Data.SqlClient

Public NotInheritable Class MiServidor

    ''' <summary>
    ''' Devuelve el nombre del servidor del ordenador en que se está ejecutando el proyecto
    ''' </summary>
    ''' <param name="errorServidor">
    ''' Saldrá en blanco o con mensaje de error si no se puede conectar con SqlServer
    ''' </param>
    ''' <returns>Devuelve el nombre del servidor</returns>
    Public Shared Function Servidor(ByRef errorServidor As String) As String ' La variable errorServidor se pasa por referencia para que pueda ser modificada dentro de la función y reflejar el resultado al exterior
        errorServidor = ""
        Dim servidores() As String = {".", ".\SQLEXPRESS"} ' , "(local)", "(local)\SQLEXPRESS", "localhost", "localhost\SQLEXPRESS"

        For Each serv As String In servidores
            Try
                Dim cadConexion As String = $"Data Source={serv};Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=2"

                Using con As New SqlConnection(cadConexion)
                    con.Open()
                    Return serv ' Ha podido conectar con SqlServer
                End Using

            Catch exc As Exception
                errorServidor = exc.Message ' NO ha podido conectar, continúa buscando
            End Try
        Next

        errorServidor = "No era ninguno???"
        Return ""
    End Function

End Class