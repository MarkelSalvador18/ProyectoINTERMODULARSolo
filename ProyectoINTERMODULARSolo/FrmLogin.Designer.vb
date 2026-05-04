<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
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
        Me.btnAceptarDNI = New System.Windows.Forms.Button()
        Me.txtDNI = New System.Windows.Forms.TextBox()
        Me.lblDNI = New System.Windows.Forms.Label()
        Me.btnNoTengoCuenta = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnAceptarDNI
        '
        Me.btnAceptarDNI.Location = New System.Drawing.Point(140, 147)
        Me.btnAceptarDNI.Name = "btnAceptarDNI"
        Me.btnAceptarDNI.Size = New System.Drawing.Size(137, 57)
        Me.btnAceptarDNI.TabIndex = 0
        Me.btnAceptarDNI.Text = "Entrar"
        Me.btnAceptarDNI.UseVisualStyleBackColor = True
        '
        'txtDNI
        '
        Me.txtDNI.Location = New System.Drawing.Point(253, 94)
        Me.txtDNI.Name = "txtDNI"
        Me.txtDNI.Size = New System.Drawing.Size(158, 22)
        Me.txtDNI.TabIndex = 1
        '
        'lblDNI
        '
        Me.lblDNI.Location = New System.Drawing.Point(137, 93)
        Me.lblDNI.Name = "lblDNI"
        Me.lblDNI.Size = New System.Drawing.Size(100, 23)
        Me.lblDNI.TabIndex = 2
        Me.lblDNI.Text = "Introduce DNI :"
        '
        'btnNoTengoCuenta
        '
        Me.btnNoTengoCuenta.Location = New System.Drawing.Point(322, 148)
        Me.btnNoTengoCuenta.Name = "btnNoTengoCuenta"
        Me.btnNoTengoCuenta.Size = New System.Drawing.Size(141, 56)
        Me.btnNoTengoCuenta.TabIndex = 3
        Me.btnNoTengoCuenta.Text = "No tengo cuenta"
        Me.btnNoTengoCuenta.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(482, 245)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FrmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnNoTengoCuenta)
        Me.Controls.Add(Me.lblDNI)
        Me.Controls.Add(Me.txtDNI)
        Me.Controls.Add(Me.btnAceptarDNI)
        Me.Name = "FrmLogin"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnAceptarDNI As Button
    Friend WithEvents txtDNI As TextBox
    Friend WithEvents lblDNI As Label
    Friend WithEvents btnNoTengoCuenta As Button
    Friend WithEvents Button1 As Button
End Class
