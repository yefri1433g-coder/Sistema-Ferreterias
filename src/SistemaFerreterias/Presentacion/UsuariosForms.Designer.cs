namespace Presentacion
{
    partial class UsuariosForms
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.ComboBox cboRol;
        private System.Windows.Forms.RadioButton rdbActivo;
        private System.Windows.Forms.RadioButton rdbInactivo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private FontAwesome.Sharp.IconButton btnNuevo;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconButton btnLimpiarPruebas;
        private FontAwesome.Sharp.IconButton btnCerrar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelPaginacion;
        private System.Windows.Forms.Button btnPaginaAnterior;
        private System.Windows.Forms.Button btnPaginaSiguiente;
        private System.Windows.Forms.Label lblPagina;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panelBusqueda;
        private System.Windows.Forms.Label lblBuscarUsuario;
        private System.Windows.Forms.TextBox txtBuscarUsuario;
        private System.Windows.Forms.Button btnBuscarUsuario;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.cboRol = new System.Windows.Forms.ComboBox();
            this.rdbActivo = new System.Windows.Forms.RadioButton();
            this.rdbInactivo = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.btnNuevo = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.btnLimpiarPruebas = new FontAwesome.Sharp.IconButton();
            this.btnCerrar = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelPaginacion = new System.Windows.Forms.Panel();
            this.btnPaginaAnterior = new System.Windows.Forms.Button();
            this.btnPaginaSiguiente = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelBusqueda = new System.Windows.Forms.Panel();
            this.lblBuscarUsuario = new System.Windows.Forms.Label();
            this.txtBuscarUsuario = new System.Windows.Forms.TextBox();
            this.btnBuscarUsuario = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();

            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.DarkBlue;
            this.label5.Location = new System.Drawing.Point(24, 16);
            this.label5.Text = "Administración de usuarios";

            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(28, 61);
            this.label6.Text = "Crea, edita y consulta las cuentas del sistema";

            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Height = 96;

            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(16, 12, 16, 8);

            this.label1.Text = "Usuario";
            this.label2.Text = "Contraseña";
            this.label3.Text = "Rol";
            this.label4.Text = "Estado";
            foreach (System.Windows.Forms.Label label in new[] { this.label1, this.label2, this.label3, this.label4 })
            {
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            }

            this.txtNombreUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNombreUsuario.Margin = new System.Windows.Forms.Padding(3, 8, 12, 8);
            this.txtNombreUsuario.MaxLength = 50;
            this.txtContrasena.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContrasena.Margin = new System.Windows.Forms.Padding(3, 8, 12, 8);
            this.txtContrasena.MaxLength = 100;
            this.cboRol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRol.Margin = new System.Windows.Forms.Padding(3, 8, 12, 8);
            this.rdbActivo.Text = "Activo";
            this.rdbActivo.AutoSize = true;
            this.rdbActivo.Checked = true;
            this.rdbInactivo.Text = "Inactivo";
            this.rdbInactivo.AutoSize = true;
            System.Windows.Forms.FlowLayoutPanel estadoPanel = new System.Windows.Forms.FlowLayoutPanel();
            estadoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            estadoPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            estadoPanel.Controls.Add(this.rdbActivo);
            estadoPanel.Controls.Add(this.rdbInactivo);

            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtNombreUsuario, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtContrasena, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboRol, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(estadoPanel, 3, 1);

            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Height = 135;

            this.btnNuevo.Text = "Nuevo";
            this.btnGuardar.Text = "Guardar";
            this.btnCerrar.Text = "Cerrar";
            foreach (FontAwesome.Sharp.IconButton button in new[] { this.btnNuevo, this.btnGuardar, this.btnEliminar, this.btnLimpiarPruebas, this.btnCerrar })
            {
                button.Width = 132;
                button.Height = 40;
                button.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
                button.IconSize = 18;
                button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                button.UseVisualStyleBackColor = true;
            }
            this.btnNuevo.IconChar = FontAwesome.Sharp.IconChar.FileCirclePlus;
            this.btnNuevo.IconColor = System.Drawing.Color.Black;
            this.btnNuevo.Location = new System.Drawing.Point(8, 6);
            this.btnGuardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnGuardar.IconColor = System.Drawing.Color.Black;
            this.btnGuardar.Location = new System.Drawing.Point(150, 6);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnEliminar.IconColor = System.Drawing.Color.Black;
            this.btnEliminar.Location = new System.Drawing.Point(292, 6);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.btnLimpiarPruebas.Text = "Limpiar";
            this.btnLimpiarPruebas.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnLimpiarPruebas.IconColor = System.Drawing.Color.Black;
            this.btnLimpiarPruebas.Location = new System.Drawing.Point(434, 6);
            this.btnLimpiarPruebas.Click += new System.EventHandler(this.btnLimpiarPruebas_Click);
            this.btnCerrar.IconChar = FontAwesome.Sharp.IconChar.Xmark;
            this.btnCerrar.IconColor = System.Drawing.Color.Black;
            this.btnCerrar.Location = new System.Drawing.Point(576, 6);
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click_1);
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click_1);

            this.panel2.Controls.Add(this.btnNuevo);
            this.panel2.Controls.Add(this.btnGuardar);
            this.panel2.Controls.Add(this.btnEliminar);
            this.panel2.Controls.Add(this.btnLimpiarPruebas);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Height = 56;

            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.RowHeadersVisible = false;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsuarios_CellClick);
            this.btnPaginaAnterior.Text = "Anterior";
            this.btnPaginaAnterior.Location = new System.Drawing.Point(0, 4);
            this.btnPaginaAnterior.Size = new System.Drawing.Size(90, 32);
            this.btnPaginaAnterior.Click += new System.EventHandler(this.btnPaginaAnterior_Click);
            this.btnPaginaSiguiente.Text = "Siguiente";
            this.btnPaginaSiguiente.Location = new System.Drawing.Point(260, 4);
            this.btnPaginaSiguiente.Size = new System.Drawing.Size(90, 32);
            this.btnPaginaSiguiente.Click += new System.EventHandler(this.btnPaginaSiguiente_Click);
            this.lblPagina.AutoSize = false;
            this.lblPagina.Location = new System.Drawing.Point(100, 3);
            this.lblPagina.Size = new System.Drawing.Size(150, 30);
            this.lblPagina.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblTotal.Location = new System.Drawing.Point(670, 3);
            this.lblTotal.Size = new System.Drawing.Size(170, 30);
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.panelPaginacion.Controls.Add(this.btnPaginaAnterior);
            this.panelPaginacion.Controls.Add(this.lblPagina);
            this.panelPaginacion.Controls.Add(this.btnPaginaSiguiente);
            this.panelPaginacion.Controls.Add(this.lblTotal);
            this.panelPaginacion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPaginacion.Height = 42;

            this.lblBuscarUsuario.AutoSize = false;
            this.lblBuscarUsuario.Location = new System.Drawing.Point(0, 7);
            this.lblBuscarUsuario.Size = new System.Drawing.Size(60, 26);
            this.lblBuscarUsuario.Text = "Buscar:";
            this.lblBuscarUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBuscarUsuario.Location = new System.Drawing.Point(64, 6);
            this.txtBuscarUsuario.Size = new System.Drawing.Size(250, 26);
            this.txtBuscarUsuario.MaxLength = 100;
            this.txtBuscarUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtBuscarUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscarUsuario_KeyDown);
            this.btnBuscarUsuario.Text = "Buscar";
            this.btnBuscarUsuario.Location = new System.Drawing.Point(322, 4);
            this.btnBuscarUsuario.Size = new System.Drawing.Size(90, 30);
            this.btnBuscarUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnBuscarUsuario.Click += new System.EventHandler(this.btnBuscarUsuario_Click);
            this.panelBusqueda.Controls.Add(this.lblBuscarUsuario);
            this.panelBusqueda.Controls.Add(this.txtBuscarUsuario);
            this.panelBusqueda.Controls.Add(this.btnBuscarUsuario);
            this.panelBusqueda.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBusqueda.Height = 40;

            this.panel1.Controls.Add(this.dgvUsuarios);
            this.panel1.Controls.Add(this.panelPaginacion);
            this.panel1.Controls.Add(this.panelBusqueda);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Padding = new System.Windows.Forms.Padding(24, 8, 24, 20);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 600);
            this.MinimumSize = new System.Drawing.Size(780, 520);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de usuarios";
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Load += new System.EventHandler(this.UsuariosForms_Load);

            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
