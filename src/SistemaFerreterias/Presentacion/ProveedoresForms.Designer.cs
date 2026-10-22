namespace Presentacion
{
    partial class ProveedoresForms
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PanelPrincipal = new System.Windows.Forms.Panel();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.dgvProveedores = new System.Windows.Forms.DataGridView();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelCerrar = new System.Windows.Forms.Panel();
            this.btnCerrar = new FontAwesome.Sharp.IconButton();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.btnPaginaAnterior = new FontAwesome.Sharp.IconButton();
            this.lblPagina = new System.Windows.Forms.Label();
            this.btnPaginaSiguiente = new FontAwesome.Sharp.IconButton();
            this.PanelCheckBox = new System.Windows.Forms.Panel();
            this.PanelAccionesTabla = new System.Windows.Forms.Panel();
            this.btnImprimir = new FontAwesome.Sharp.IconButton();
            this.btnExportar = new FontAwesome.Sharp.IconButton();
            this.ChkSeleccionar = new System.Windows.Forms.CheckBox();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.PanelDivisor2 = new System.Windows.Forms.Panel();
            this.PanelFiltros = new System.Windows.Forms.Panel();
            this.btnLimpiarFiltro = new FontAwesome.Sharp.IconButton();
            this.btnBuscar = new FontAwesome.Sharp.IconButton();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.PanelDivisor = new System.Windows.Forms.Panel();
            this.panelEncabezado = new System.Windows.Forms.Panel();
            this.btnIcono = new FontAwesome.Sharp.IconButton();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PanelPrincipalGestionar = new System.Windows.Forms.Panel();
            this.panelCentralGestionar = new System.Windows.Forms.Panel();
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.tlpDatos = new System.Windows.Forms.TableLayoutPanel();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblContacto = new System.Windows.Forms.Label();
            this.txtContacto = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.PanelInferiorAcciones = new System.Windows.Forms.Panel();
            this.gboxAcciones = new System.Windows.Forms.GroupBox();
            this.tlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.btnNuevo = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.btnCancelar = new FontAwesome.Sharp.IconButton();
            this.btnEditar = new FontAwesome.Sharp.IconButton();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.btnLimpiar = new FontAwesome.Sharp.IconButton();
            this.PanelCerrarGestionar = new System.Windows.Forms.Panel();
            this.panelCerrarGestionarDer = new System.Windows.Forms.Panel();
            this.btnCerrarGestionar = new FontAwesome.Sharp.IconButton();
            this.PanelDivisorGestionar = new System.Windows.Forms.Panel();
            this.panelEncabezadoGestionar = new System.Windows.Forms.Panel();
            this.btnIconoGestionar = new FontAwesome.Sharp.IconButton();
            this.lblSubtituloGestionar = new System.Windows.Forms.Label();
            this.lblTituloGestionar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            // 
            // tabPage1
            // 
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Text = "Consultar";
            this.tabPage1.Controls.Add(this.PanelPrincipal);
            // 
            // PanelPrincipal
            // 
            this.PanelPrincipal.Name = "PanelPrincipal";
            this.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelPrincipal.Controls.Add(this.panelCentral);
            this.PanelPrincipal.Controls.Add(this.panelInferior);
            this.PanelPrincipal.Controls.Add(this.PanelCheckBox);
            this.PanelPrincipal.Controls.Add(this.PanelDivisor2);
            this.PanelPrincipal.Controls.Add(this.PanelFiltros);
            this.PanelPrincipal.Controls.Add(this.PanelDivisor);
            this.PanelPrincipal.Controls.Add(this.panelEncabezado);
            // 
            // panelCentral
            // 
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Controls.Add(this.dgvProveedores);
            // 
            // dgvProveedores
            // 
            this.dgvProveedores.Name = "dgvProveedores";
            this.dgvProveedores.AllowUserToAddRows = false;
            this.dgvProveedores.AllowUserToDeleteRows = false;
            this.dgvProveedores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProveedores.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProveedores.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvProveedores.MultiSelect = true;
            this.dgvProveedores.ReadOnly = true;
            this.dgvProveedores.RowHeadersWidth = 51;
            this.dgvProveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProveedores.RowTemplate.Height = 24;
            this.dgvProveedores.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            // 
            // panelInferior
            // 
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Size = new System.Drawing.Size(1000, 50);
            this.panelInferior.Controls.Add(this.panelCerrar);
            this.panelInferior.Controls.Add(this.btnPaginaSiguiente);
            this.panelInferior.Controls.Add(this.lblPagina);
            this.panelInferior.Controls.Add(this.btnPaginaAnterior);
            this.panelInferior.Controls.Add(this.lblTotalRegistros);
            // 
            // panelCerrar
            // 
            this.panelCerrar.Name = "panelCerrar";
            this.panelCerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelCerrar.Size = new System.Drawing.Size(200, 50);
            this.panelCerrar.Controls.Add(this.btnCerrar);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnCerrar.IconColor = System.Drawing.Color.Black;
            this.btnCerrar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrar.IconSize = 24;
            this.btnCerrar.Location = new System.Drawing.Point(10, 4);
            this.btnCerrar.Size = new System.Drawing.Size(175, 43);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblTotalRegistros
            // 
            this.lblTotalRegistros.Name = "lblTotalRegistros";
            this.lblTotalRegistros.AutoSize = true;
            this.lblTotalRegistros.Location = new System.Drawing.Point(29, 15);
            this.lblTotalRegistros.Text = "Total Registros: 0";
            // Navegación de páginas del listado de proveedores
            this.btnPaginaAnterior.Name = "btnPaginaAnterior";
            this.btnPaginaAnterior.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaginaAnterior.Location = new System.Drawing.Point(420, 6);
            this.btnPaginaAnterior.Size = new System.Drawing.Size(130, 38);
            this.btnPaginaAnterior.Text = "Anterior";
            this.btnPaginaAnterior.UseVisualStyleBackColor = true;
            this.btnPaginaAnterior.Click += new System.EventHandler(this.btnPaginaAnterior_Click);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.AutoSize = true;
            this.lblPagina.Location = new System.Drawing.Point(570, 16);
            this.lblPagina.Text = "Página 0 de 0";
            this.btnPaginaSiguiente.Name = "btnPaginaSiguiente";
            this.btnPaginaSiguiente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaginaSiguiente.Location = new System.Drawing.Point(680, 6);
            this.btnPaginaSiguiente.Size = new System.Drawing.Size(130, 38);
            this.btnPaginaSiguiente.Text = "Siguiente";
            this.btnPaginaSiguiente.UseVisualStyleBackColor = true;
            this.btnPaginaSiguiente.Click += new System.EventHandler(this.btnPaginaSiguiente_Click);
            // 
            // PanelCheckBox
            // 
            this.PanelCheckBox.Name = "PanelCheckBox";
            this.PanelCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelCheckBox.Size = new System.Drawing.Size(1000, 47);
            this.PanelCheckBox.Controls.Add(this.PanelAccionesTabla);
            this.PanelCheckBox.Controls.Add(this.ChkSeleccionar);
            this.PanelCheckBox.Controls.Add(this.lblAyuda);
            // 
            // PanelAccionesTabla
            // 
            this.PanelAccionesTabla.Name = "PanelAccionesTabla";
            this.PanelAccionesTabla.Dock = System.Windows.Forms.DockStyle.Right;
            this.PanelAccionesTabla.Size = new System.Drawing.Size(390, 47);
            this.PanelAccionesTabla.Controls.Add(this.btnImprimir);
            this.PanelAccionesTabla.Controls.Add(this.btnExportar);
            // 
            // btnImprimir
            // 
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImprimir.IconChar = FontAwesome.Sharp.IconChar.Print;
            this.btnImprimir.IconColor = System.Drawing.Color.Black;
            this.btnImprimir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnImprimir.IconSize = 30;
            this.btnImprimir.Location = new System.Drawing.Point(10, 2);
            this.btnImprimir.Size = new System.Drawing.Size(175, 43);
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportar.IconChar = FontAwesome.Sharp.IconChar.Download;
            this.btnExportar.IconColor = System.Drawing.Color.Black;
            this.btnExportar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnExportar.IconSize = 30;
            this.btnExportar.Location = new System.Drawing.Point(200, 2);
            this.btnExportar.Size = new System.Drawing.Size(175, 43);
            this.btnExportar.Text = "Exportar";
            this.btnExportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // ChkSeleccionar
            // 
            this.ChkSeleccionar.Name = "ChkSeleccionar";
            this.ChkSeleccionar.AutoSize = true;
            this.ChkSeleccionar.Location = new System.Drawing.Point(3, 8);
            this.ChkSeleccionar.Text = "Seleccionar";
            this.ChkSeleccionar.UseVisualStyleBackColor = true;
            this.ChkSeleccionar.CheckedChanged += new System.EventHandler(this.ChkSeleccionar_CheckedChanged);
            // 
            // lblAyuda
            // 
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.AutoSize = true;
            this.lblAyuda.Font = new System.Drawing.Font("Segoe UI", 9.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAyuda.ForeColor = System.Drawing.Color.Gray;
            this.lblAyuda.Location = new System.Drawing.Point(150, 10);
            this.lblAyuda.Text = "Tip: haz doble clic en una fila para verla o editarla en la pestaña Gestionar";
            // 
            // PanelDivisor2
            // 
            this.PanelDivisor2.Name = "PanelDivisor2";
            this.PanelDivisor2.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelDivisor2.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDivisor2.Size = new System.Drawing.Size(1000, 1);
            // 
            // PanelFiltros
            // 
            this.PanelFiltros.Name = "PanelFiltros";
            this.PanelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelFiltros.Size = new System.Drawing.Size(1000, 74);
            this.PanelFiltros.Controls.Add(this.btnLimpiarFiltro);
            this.PanelFiltros.Controls.Add(this.btnBuscar);
            this.PanelFiltros.Controls.Add(this.txtBuscar);
            this.PanelFiltros.Controls.Add(this.lblFiltro);
            // 
            // btnLimpiarFiltro
            // 
            this.btnLimpiarFiltro.Name = "btnLimpiarFiltro";
            this.btnLimpiarFiltro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarFiltro.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnLimpiarFiltro.IconColor = System.Drawing.Color.Black;
            this.btnLimpiarFiltro.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLimpiarFiltro.IconSize = 30;
            this.btnLimpiarFiltro.Location = new System.Drawing.Point(918, 16);
            this.btnLimpiarFiltro.Size = new System.Drawing.Size(175, 43);
            this.btnLimpiarFiltro.Text = "Limpiar";
            this.btnLimpiarFiltro.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpiarFiltro.UseVisualStyleBackColor = true;
            this.btnLimpiarFiltro.Click += new System.EventHandler(this.btnLimpiarFiltro_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnBuscar.IconColor = System.Drawing.Color.Black;
            this.btnBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscar.IconSize = 30;
            this.btnBuscar.Location = new System.Drawing.Point(721, 16);
            this.btnBuscar.Size = new System.Drawing.Size(175, 43);
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Location = new System.Drawing.Point(114, 22);
            this.txtBuscar.Size = new System.Drawing.Size(562, 31);
            // 
            // lblFiltro
            // 
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(5, 26);
            this.lblFiltro.Text = "Nombre:";
            // 
            // PanelDivisor
            // 
            this.PanelDivisor.Name = "PanelDivisor";
            this.PanelDivisor.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelDivisor.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDivisor.Size = new System.Drawing.Size(1000, 1);
            // 
            // panelEncabezado
            // 
            this.panelEncabezado.Name = "panelEncabezado";
            this.panelEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEncabezado.Size = new System.Drawing.Size(1000, 90);
            this.panelEncabezado.Controls.Add(this.btnIcono);
            this.panelEncabezado.Controls.Add(this.lblSubtitulo);
            this.panelEncabezado.Controls.Add(this.lblTitulo);
            // 
            // btnIcono
            // 
            this.btnIcono.Name = "btnIcono";
            this.btnIcono.BackColor = System.Drawing.Color.White;
            this.btnIcono.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIcono.IconChar = FontAwesome.Sharp.IconChar.Handshake;
            this.btnIcono.IconColor = System.Drawing.Color.DarkBlue;
            this.btnIcono.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnIcono.IconSize = 56;
            this.btnIcono.Location = new System.Drawing.Point(10, 6);
            this.btnIcono.Size = new System.Drawing.Size(100, 78);
            this.btnIcono.TabStop = false;
            this.btnIcono.UseVisualStyleBackColor = false;
            this.btnIcono.FlatAppearance.BorderSize = 0;
            this.btnIcono.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnIcono.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitulo.Location = new System.Drawing.Point(123, 50);
            this.lblSubtitulo.Text = "Administración de proveedores registrados en el sistema";
            // 
            // lblTitulo
            // 
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitulo.Location = new System.Drawing.Point(118, 0);
            this.lblTitulo.Text = "PROVEEDORES";
            // 
            // tabPage2
            // 
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Text = "Gestionar";
            this.tabPage2.Controls.Add(this.PanelPrincipalGestionar);
            // 
            // PanelPrincipalGestionar
            // 
            this.PanelPrincipalGestionar.Name = "PanelPrincipalGestionar";
            this.PanelPrincipalGestionar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelPrincipalGestionar.Controls.Add(this.panelCentralGestionar);
            this.PanelPrincipalGestionar.Controls.Add(this.PanelInferiorAcciones);
            this.PanelPrincipalGestionar.Controls.Add(this.PanelCerrarGestionar);
            this.PanelPrincipalGestionar.Controls.Add(this.PanelDivisorGestionar);
            this.PanelPrincipalGestionar.Controls.Add(this.panelEncabezadoGestionar);
            // 
            // panelCentralGestionar
            // 
            this.panelCentralGestionar.Name = "panelCentralGestionar";
            this.panelCentralGestionar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentralGestionar.Controls.Add(this.grpDatos);
            // 
            // grpDatos
            // 
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDatos.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDatos.ForeColor = System.Drawing.Color.DarkBlue;
            this.grpDatos.Text = "Datos Proveedor";
            this.grpDatos.Controls.Add(this.tlpDatos);
            // 
            // tlpDatos
            // 
            this.tlpDatos.Name = "tlpDatos";
            this.tlpDatos.ColumnCount = 4;
            this.tlpDatos.RowCount = 4;
            this.tlpDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDatos.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDatos.Controls.Add(this.lblCodigo, 0, 0);
            this.tlpDatos.Controls.Add(this.txtCodigo, 1, 0);
            this.tlpDatos.Controls.Add(this.lblNombre, 2, 0);
            this.tlpDatos.Controls.Add(this.txtNombre, 3, 0);
            this.tlpDatos.Controls.Add(this.lblContacto, 0, 1);
            this.tlpDatos.Controls.Add(this.txtContacto, 1, 1);
            this.tlpDatos.Controls.Add(this.lblTelefono, 2, 1);
            this.tlpDatos.Controls.Add(this.txtTelefono, 3, 1);
            this.tlpDatos.Controls.Add(this.lblCorreo, 0, 2);
            this.tlpDatos.Controls.Add(this.txtCorreo, 1, 2);
            this.tlpDatos.Controls.Add(this.lblDireccion, 2, 2);
            this.tlpDatos.Controls.Add(this.txtDireccion, 3, 2);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.ReadOnly = true;
            this.txtCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodigo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblNombre
            // 
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblContacto
            // 
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblContacto.AutoSize = true;
            this.lblContacto.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContacto.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblContacto.Text = "Contacto:";
            // 
            // txtContacto
            // 
            this.txtContacto.Name = "txtContacto";
            this.txtContacto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContacto.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblTelefono
            // 
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefono.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTelefono.Text = "Teléfono:";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTelefono.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblCorreo
            // 
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCorreo.Text = "Correo:";
            // 
            // txtCorreo
            // 
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCorreo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblDireccion
            // 
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccion.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblDireccion.Text = "Dirección:";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDireccion.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // PanelInferiorAcciones
            // 
            this.PanelInferiorAcciones.Name = "PanelInferiorAcciones";
            this.PanelInferiorAcciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelInferiorAcciones.Size = new System.Drawing.Size(1000, 90);
            this.PanelInferiorAcciones.Controls.Add(this.gboxAcciones);
            // 
            // gboxAcciones
            // 
            this.gboxAcciones.Name = "gboxAcciones";
            this.gboxAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gboxAcciones.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxAcciones.ForeColor = System.Drawing.Color.DarkBlue;
            this.gboxAcciones.Text = "Acciones";
            this.gboxAcciones.Controls.Add(this.tlpAcciones);
            // 
            // tlpAcciones
            // 
            this.tlpAcciones.Name = "tlpAcciones";
            this.tlpAcciones.ColumnCount = 6;
            this.tlpAcciones.RowCount = 1;
            this.tlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666666666668F));
            this.tlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAcciones.Controls.Add(this.btnNuevo, 0, 0);
            this.tlpAcciones.Controls.Add(this.btnGuardar, 1, 0);
            this.tlpAcciones.Controls.Add(this.btnCancelar, 2, 0);
            this.tlpAcciones.Controls.Add(this.btnEditar, 3, 0);
            this.tlpAcciones.Controls.Add(this.btnEliminar, 4, 0);
            this.tlpAcciones.Controls.Add(this.btnLimpiar, 5, 0);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNuevo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevo.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.btnNuevo.IconColor = System.Drawing.Color.Black;
            this.btnNuevo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnNuevo.IconSize = 28;
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnGuardar.IconColor = System.Drawing.Color.Black;
            this.btnGuardar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnGuardar.IconSize = 28;
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.IconChar = FontAwesome.Sharp.IconChar.Ban;
            this.btnCancelar.IconColor = System.Drawing.Color.Black;
            this.btnCancelar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelar.IconSize = 28;
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.IconChar = FontAwesome.Sharp.IconChar.Pencil;
            this.btnEditar.IconColor = System.Drawing.Color.Black;
            this.btnEditar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEditar.IconSize = 28;
            this.btnEditar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnEditar.Text = "Editar";
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnEliminar.IconColor = System.Drawing.Color.Black;
            this.btnEliminar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEliminar.IconSize = 28;
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnLimpiar.IconColor = System.Drawing.Color.Black;
            this.btnLimpiar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLimpiar.IconSize = 28;
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // PanelCerrarGestionar
            // 
            this.PanelCerrarGestionar.Name = "PanelCerrarGestionar";
            this.PanelCerrarGestionar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelCerrarGestionar.Size = new System.Drawing.Size(1000, 60);
            this.PanelCerrarGestionar.Controls.Add(this.panelCerrarGestionarDer);
            // 
            // panelCerrarGestionarDer
            // 
            this.panelCerrarGestionarDer.Name = "panelCerrarGestionarDer";
            this.panelCerrarGestionarDer.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelCerrarGestionarDer.Size = new System.Drawing.Size(200, 60);
            this.panelCerrarGestionarDer.Controls.Add(this.btnCerrarGestionar);
            // 
            // btnCerrarGestionar
            // 
            this.btnCerrarGestionar.Name = "btnCerrarGestionar";
            this.btnCerrarGestionar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarGestionar.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnCerrarGestionar.IconColor = System.Drawing.Color.Black;
            this.btnCerrarGestionar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrarGestionar.IconSize = 24;
            this.btnCerrarGestionar.Location = new System.Drawing.Point(10, 8);
            this.btnCerrarGestionar.Size = new System.Drawing.Size(175, 43);
            this.btnCerrarGestionar.Text = "Cerrar";
            this.btnCerrarGestionar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarGestionar.UseVisualStyleBackColor = true;
            this.btnCerrarGestionar.Click += new System.EventHandler(this.btnCerrarGestionar_Click);
            // 
            // PanelDivisorGestionar
            // 
            this.PanelDivisorGestionar.Name = "PanelDivisorGestionar";
            this.PanelDivisorGestionar.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelDivisorGestionar.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDivisorGestionar.Size = new System.Drawing.Size(1000, 1);
            // 
            // panelEncabezadoGestionar
            // 
            this.panelEncabezadoGestionar.Name = "panelEncabezadoGestionar";
            this.panelEncabezadoGestionar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEncabezadoGestionar.Size = new System.Drawing.Size(1000, 90);
            this.panelEncabezadoGestionar.Controls.Add(this.btnIconoGestionar);
            this.panelEncabezadoGestionar.Controls.Add(this.lblSubtituloGestionar);
            this.panelEncabezadoGestionar.Controls.Add(this.lblTituloGestionar);
            // 
            // btnIconoGestionar
            // 
            this.btnIconoGestionar.Name = "btnIconoGestionar";
            this.btnIconoGestionar.BackColor = System.Drawing.Color.White;
            this.btnIconoGestionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIconoGestionar.IconChar = FontAwesome.Sharp.IconChar.Handshake;
            this.btnIconoGestionar.IconColor = System.Drawing.Color.DarkBlue;
            this.btnIconoGestionar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnIconoGestionar.IconSize = 56;
            this.btnIconoGestionar.Location = new System.Drawing.Point(10, 6);
            this.btnIconoGestionar.Size = new System.Drawing.Size(100, 78);
            this.btnIconoGestionar.TabStop = false;
            this.btnIconoGestionar.UseVisualStyleBackColor = false;
            this.btnIconoGestionar.FlatAppearance.BorderSize = 0;
            this.btnIconoGestionar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnIconoGestionar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            // 
            // lblSubtituloGestionar
            // 
            this.lblSubtituloGestionar.Name = "lblSubtituloGestionar";
            this.lblSubtituloGestionar.AutoSize = true;
            this.lblSubtituloGestionar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloGestionar.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtituloGestionar.Location = new System.Drawing.Point(123, 50);
            this.lblSubtituloGestionar.Text = "Agregar, editar o eliminar proveedores";
            // 
            // lblTituloGestionar
            // 
            this.lblTituloGestionar.Name = "lblTituloGestionar";
            this.lblTituloGestionar.AutoSize = true;
            this.lblTituloGestionar.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloGestionar.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTituloGestionar.Location = new System.Drawing.Point(118, 0);
            this.lblTituloGestionar.Text = "PROVEEDORES";
            // 
            // ProveedoresForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Name = "ProveedoresForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proveedores";
            this.Controls.Add(this.tabControl1);
            this.Load += new System.EventHandler(this.ProveedoresForms_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel PanelPrincipal;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.DataGridView dgvProveedores;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelCerrar;
        private FontAwesome.Sharp.IconButton btnCerrar;
        private System.Windows.Forms.Label lblTotalRegistros;
        private FontAwesome.Sharp.IconButton btnPaginaAnterior;
        private System.Windows.Forms.Label lblPagina;
        private FontAwesome.Sharp.IconButton btnPaginaSiguiente;
        private System.Windows.Forms.Panel PanelCheckBox;
        private System.Windows.Forms.Panel PanelAccionesTabla;
        private FontAwesome.Sharp.IconButton btnImprimir;
        private FontAwesome.Sharp.IconButton btnExportar;
        private System.Windows.Forms.CheckBox ChkSeleccionar;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Panel PanelDivisor2;
        private System.Windows.Forms.Panel PanelFiltros;
        private FontAwesome.Sharp.IconButton btnLimpiarFiltro;
        private FontAwesome.Sharp.IconButton btnBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblFiltro;
        private System.Windows.Forms.Panel PanelDivisor;
        private System.Windows.Forms.Panel panelEncabezado;
        private FontAwesome.Sharp.IconButton btnIcono;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel PanelPrincipalGestionar;
        private System.Windows.Forms.Panel panelCentralGestionar;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.TableLayoutPanel tlpDatos;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblContacto;
        private System.Windows.Forms.TextBox txtContacto;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Panel PanelInferiorAcciones;
        private System.Windows.Forms.GroupBox gboxAcciones;
        private System.Windows.Forms.TableLayoutPanel tlpAcciones;
        private FontAwesome.Sharp.IconButton btnNuevo;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private FontAwesome.Sharp.IconButton btnEditar;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconButton btnLimpiar;
        private System.Windows.Forms.Panel PanelCerrarGestionar;
        private System.Windows.Forms.Panel panelCerrarGestionarDer;
        private FontAwesome.Sharp.IconButton btnCerrarGestionar;
        private System.Windows.Forms.Panel PanelDivisorGestionar;
        private System.Windows.Forms.Panel panelEncabezadoGestionar;
        private FontAwesome.Sharp.IconButton btnIconoGestionar;
        private System.Windows.Forms.Label lblSubtituloGestionar;
        private System.Windows.Forms.Label lblTituloGestionar;
    }
}
