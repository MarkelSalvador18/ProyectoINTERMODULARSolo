<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAñadirAlumno
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblDni = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNuevoDNI = New System.Windows.Forms.TextBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.txtApellido2 = New System.Windows.Forms.TextBox()
        Me.txtApellido1 = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtCodigoCiclo = New System.Windows.Forms.TextBox()
        Me.txtTelefono = New System.Windows.Forms.TextBox()
        Me.btnCrearCuenta = New System.Windows.Forms.Button()
        Me.btnVolver = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblDni
        '
        Me.lblDni.Location = New System.Drawing.Point(177, 32)
        Me.lblDni.Name = "lblDni"
        Me.lblDni.Size = New System.Drawing.Size(100, 23)
        Me.lblDni.TabIndex = 0
        Me.lblDni.Text = "DNI :"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(89, 204)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(188, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Apellido2 (no obligatorio) :"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(177, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 23)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Nombre :"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(177, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 23)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Apellido1 :"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(177, 266)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 23)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Email"
        '
        'txtNuevoDNI
        '
        Me.txtNuevoDNI.Location = New System.Drawing.Point(283, 32)
        Me.txtNuevoDNI.Name = "txtNuevoDNI"
        Me.txtNuevoDNI.Size = New System.Drawing.Size(137, 22)
        Me.txtNuevoDNI.TabIndex = 5
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(283, 263)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(137, 22)
        Me.txtEmail.TabIndex = 6
        '
        'txtApellido2
        '
        Me.txtApellido2.Location = New System.Drawing.Point(283, 205)
        Me.txtApellido2.Name = "txtApellido2"
        Me.txtApellido2.Size = New System.Drawing.Size(137, 22)
        Me.txtApellido2.TabIndex = 7
        '
        'txtApellido1
        '
        Me.txtApellido1.Location = New System.Drawing.Point(283, 145)
        Me.txtApellido1.Name = "txtApellido1"
        Me.txtApellido1.Size = New System.Drawing.Size(137, 22)
        Me.txtApellido1.TabIndex = 8
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(283, 84)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(137, 22)
        Me.txtNombre.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(177, 316)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 23)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Teléfono"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(177, 367)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 23)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Código Ciclo"
        '
        'txtCodigoCiclo
        '
        Me.txtCodigoCiclo.Location = New System.Drawing.Point(283, 368)
        Me.txtCodigoCiclo.Name = "txtCodigoCiclo"
        Me.txtCodigoCiclo.Size = New System.Drawing.Size(137, 22)
        Me.txtCodigoCiclo.TabIndex = 12
        '
        'txtTelefono
        '
        Me.txtTelefono.Location = New System.Drawing.Point(283, 316)
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(137, 22)
        Me.txtTelefono.TabIndex = 13
        '
        'btnCrearCuenta
        '
        Me.btnCrearCuenta.Location = New System.Drawing.Point(511, 332)
        Me.btnCrearCuenta.Name = "btnCrearCuenta"
        Me.btnCrearCuenta.Size = New System.Drawing.Size(140, 57)
        Me.btnCrearCuenta.TabIndex = 14
        Me.btnCrearCuenta.Text = "CrearCuenta"
        Me.btnCrearCuenta.UseVisualStyleBackColor = True
        '
        'btnVolver
        '
        Me.btnVolver.Location = New System.Drawing.Point(511, 246)
        Me.btnVolver.Name = "btnVolver"
        Me.btnVolver.Size = New System.Drawing.Size(140, 57)
        Me.btnVolver.TabIndex = 15
        Me.btnVolver.Text = "Volver"
        Me.btnVolver.UseVisualStyleBackColor = True
        '
        'FrmAñadirAlumno
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnVolver)
        Me.Controls.Add(Me.btnCrearCuenta)
        Me.Controls.Add(Me.txtTelefono)
        Me.Controls.Add(Me.txtCodigoCiclo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNombre)
        Me.Controls.Add(Me.txtApellido1)
        Me.Controls.Add(Me.txtApellido2)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtNuevoDNI)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblDni)
        Me.Name = "FrmAñadirAlumno"
        Me.Text = "AñadirAlumni"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblDni As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNuevoDNI As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtApellido2 As TextBox
    Friend WithEvents txtApellido1 As TextBox
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtCodigoCiclo As TextBox
    Friend WithEvents txtTelefono As TextBox
    Friend WithEvents btnCrearCuenta As Button
    Friend WithEvents btnVolver As Button
End Class
