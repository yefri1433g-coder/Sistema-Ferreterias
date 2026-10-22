namespace Presentacion
{
    partial class TrasladosForms
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
            this.dgvTraslados = new System.Windows.Forms.DataGridView();
            this.panelDetalleConsulta = new System.Windows.Forms.Panel();
            this.dgvDetalleConsulta = new System.Windows.Forms.DataGridView();
            this.panelTituloDetalle = new System.Windows.Forms.Panel();
            this.lblDetalleTitulo = new System.Windows.Forms.Label();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelCerrar = new System.Windows.Forms.Panel();
            this.btnCerrar = new FontAwesome.Sharp.IconButton();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
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
            this.grpDetalle = new System.Windows.Forms.GroupBox();
            this.dgvDetalleGestionar = new System.Windows.Forms.DataGridView();
            this.panelAgregarDetalle = new System.Windows.Forms.Panel();
            this.lblProductoDetalle = new System.Windows.Forms.Label();
            this.cboProducto = new System.Windows.Forms.ComboBox();
            this.lblCantidadDetalle = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarDetalle = new FontAwesome.Sharp.IconButton();
            this.btnQuitarDetalle = new FontAwesome.Sharp.IconButton();
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.tlpDatos = new System.Windows.Forms.TableLayoutPanel();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblEmpleado = new System.Windows.Forms.Label();
            this.cboEmpleado = new System.Windows.Forms.ComboBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.cboOrigen = new System.Windows.Forms.ComboBox();
            this.lblDestino = new System.Windows.Forms.Label();
            this.cboDestino = new System.Windows.Forms.ComboBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.PanelInferiorAcciones = new System.Windows.Forms.Panel();
            this.gboxAcciones = new System.Windows.Forms.GroupBox();
            this.tlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.btnNuevo = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.btnCancelar = new FontAwesome.Sharp.IconButton();
            this.btnEditar = new FontAwesome.Sharp.IconButton();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.btnLimpiar = new FontAwesome.Sharp.IconButton();
            this.btnEnTransito = new FontAwesome.Sharp.IconButton();
            this.btnCompletar = new FontAwesome.Sharp.IconButton();
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
            this.PanelPrincipal.Controls.Add(this.panelDetalleConsulta);
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
            this.panelCentral.Controls.Add(this.dgvTraslados);
            // 
            // dgvTraslados
            // 
            this.dgvTraslados.Name = "dgvTraslados";
            this.dgvTraslados.AllowUserToAddRows = false;
            this.dgvTraslados.AllowUserToDeleteRows = false;
            this.dgvTraslados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTraslados.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvTraslados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraslados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTraslados.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTraslados.MultiSelect = true;
            this.dgvTraslados.ReadOnly = true;
            this.dgvTraslados.RowHeadersWidth = 51;
            this.dgvTraslados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTraslados.RowTemplate.Height = 24;
            this.dgvTraslados.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            this.dgvTraslados.SelectionChanged += new System.EventHandler(this.dgvTraslados_SelectionChanged);
            // 
            // panelDetalleConsulta
            // 
            this.panelDetalleConsulta.Name = "panelDetalleConsulta";
            this.panelDetalleConsulta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDetalleConsulta.Size = new System.Drawing.Size(1000, 200);
            this.panelDetalleConsulta.Controls.Add(this.dgvDetalleConsulta);
            this.panelDetalleConsulta.Controls.Add(this.panelTituloDetalle);
            // 
            // dgvDetalleConsulta
            // 
            this.dgvDetalleConsulta.Name = "dgvDetalleConsulta";
            this.dgvDetalleConsulta.AllowUserToAddRows = false;
            this.dgvDetalleConsulta.AllowUserToDeleteRows = false;
            this.dgvDetalleConsulta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetalleConsulta.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvDetalleConsulta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleConsulta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalleConsulta.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDetalleConsulta.MultiSelect = true;
            this.dgvDetalleConsulta.ReadOnly = true;
            this.dgvDetalleConsulta.RowHeadersWidth = 51;
            this.dgvDetalleConsulta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalleConsulta.RowTemplate.Height = 24;
            // 
            // panelTituloDetalle
            // 
            this.panelTituloDetalle.Name = "panelTituloDetalle";
            this.panelTituloDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTituloDetalle.Size = new System.Drawing.Size(1000, 32);
            this.panelTituloDetalle.Controls.Add(this.lblDetalleTitulo);
            // 
            // lblDetalleTitulo
            // 
            this.lblDetalleTitulo.Name = "lblDetalleTitulo";
            this.lblDetalleTitulo.AutoSize = true;
            this.lblDetalleTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetalleTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblDetalleTitulo.Location = new System.Drawing.Point(5, 5);
            this.lblDetalleTitulo.Text = "Detalle del traslado seleccionado";
            // 
            // panelInferior
            // 
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Size = new System.Drawing.Size(1000, 50);
            this.panelInferior.Controls.Add(this.panelCerrar);
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
            this.lblFiltro.Text = "Buscar:";
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
            this.btnIcono.IconChar = FontAwesome.Sharp.IconChar.Truck;
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
            this.lblSubtitulo.Text = "Movimiento de productos entre sucursales";
            // 
            // lblTitulo
            // 
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitulo.Location = new System.Drawing.Point(118, 0);
            this.lblTitulo.Text = "TRASLADOS";
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
            this.panelCentralGestionar.Controls.Add(this.grpDetalle);
            this.panelCentralGestionar.Controls.Add(this.grpDatos);
            // 
            // grpDetalle
            // 
            this.grpDetalle.Name = "grpDetalle";
            this.grpDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDetalle.ForeColor = System.Drawing.Color.DarkBlue;
            this.grpDetalle.Text = "Detalle del traslado (productos y cantidades)";
            this.grpDetalle.Controls.Add(this.dgvDetalleGestionar);
            this.grpDetalle.Controls.Add(this.panelAgregarDetalle);
            // 
            // dgvDetalleGestionar
            // 
            this.dgvDetalleGestionar.Name = "dgvDetalleGestionar";
            this.dgvDetalleGestionar.AllowUserToAddRows = false;
            this.dgvDetalleGestionar.AllowUserToDeleteRows = false;
            this.dgvDetalleGestionar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetalleGestionar.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvDetalleGestionar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleGestionar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalleGestionar.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDetalleGestionar.MultiSelect = true;
            this.dgvDetalleGestionar.ReadOnly = true;
            this.dgvDetalleGestionar.RowHeadersWidth = 51;
            this.dgvDetalleGestionar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalleGestionar.RowTemplate.Height = 24;
            // 
            // panelAgregarDetalle
            // 
            this.panelAgregarDetalle.Name = "panelAgregarDetalle";
            this.panelAgregarDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAgregarDetalle.Size = new System.Drawing.Size(1000, 62);
            this.panelAgregarDetalle.Controls.Add(this.lblProductoDetalle);
            this.panelAgregarDetalle.Controls.Add(this.cboProducto);
            this.panelAgregarDetalle.Controls.Add(this.lblCantidadDetalle);
            this.panelAgregarDetalle.Controls.Add(this.nudCantidad);
            this.panelAgregarDetalle.Controls.Add(this.btnAgregarDetalle);
            this.panelAgregarDetalle.Controls.Add(this.btnQuitarDetalle);
            // 
            // lblProductoDetalle
            // 
            this.lblProductoDetalle.Name = "lblProductoDetalle";
            this.lblProductoDetalle.AutoSize = true;
            this.lblProductoDetalle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductoDetalle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblProductoDetalle.Location = new System.Drawing.Point(10, 22);
            this.lblProductoDetalle.Text = "Producto:";
            // 
            // cboProducto
            // 
            this.cboProducto.Name = "cboProducto";
            this.cboProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProducto.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProducto.Location = new System.Drawing.Point(95, 18);
            this.cboProducto.Size = new System.Drawing.Size(420, 33);
            // 
            // lblCantidadDetalle
            // 
            this.lblCantidadDetalle.Name = "lblCantidadDetalle";
            this.lblCantidadDetalle.AutoSize = true;
            this.lblCantidadDetalle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadDetalle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCantidadDetalle.Location = new System.Drawing.Point(540, 22);
            this.lblCantidadDetalle.Text = "Cantidad:";
            // 
            // nudCantidad
            // 
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCantidad.Location = new System.Drawing.Point(625, 18);
            this.nudCantidad.Size = new System.Drawing.Size(100, 31);
            this.nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0});
            this.nudCantidad.Maximum = new decimal(new int[] { 100000, 0, 0, 0});
            this.nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0});
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.Name = "btnAgregarDetalle";
            this.btnAgregarDetalle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregarDetalle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarDetalle.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.btnAgregarDetalle.IconColor = System.Drawing.Color.Black;
            this.btnAgregarDetalle.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAgregarDetalle.IconSize = 24;
            this.btnAgregarDetalle.Location = new System.Drawing.Point(745, 10);
            this.btnAgregarDetalle.Size = new System.Drawing.Size(150, 43);
            this.btnAgregarDetalle.Text = "Agregar";
            this.btnAgregarDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregarDetalle.UseVisualStyleBackColor = true;
            this.btnAgregarDetalle.Click += new System.EventHandler(this.btnAgregarDetalle_Click);
            // 
            // btnQuitarDetalle
            // 
            this.btnQuitarDetalle.Name = "btnQuitarDetalle";
            this.btnQuitarDetalle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuitarDetalle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuitarDetalle.IconChar = FontAwesome.Sharp.IconChar.Minus;
            this.btnQuitarDetalle.IconColor = System.Drawing.Color.Black;
            this.btnQuitarDetalle.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQuitarDetalle.IconSize = 24;
            this.btnQuitarDetalle.Location = new System.Drawing.Point(905, 10);
            this.btnQuitarDetalle.Size = new System.Drawing.Size(150, 43);
            this.btnQuitarDetalle.Text = "Quitar";
            this.btnQuitarDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnQuitarDetalle.UseVisualStyleBackColor = true;
            this.btnQuitarDetalle.Click += new System.EventHandler(this.btnQuitarDetalle_Click);
            // 
            // grpDatos
            // 
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDatos.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDatos.ForeColor = System.Drawing.Color.DarkBlue;
            this.grpDatos.Text = "Datos Traslado";
            this.grpDatos.Size = new System.Drawing.Size(1000, 235);
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
            this.tlpDatos.Controls.Add(this.lblEmpleado, 2, 0);
            this.tlpDatos.Controls.Add(this.cboEmpleado, 3, 0);
            this.tlpDatos.Controls.Add(this.lblOrigen, 0, 1);
            this.tlpDatos.Controls.Add(this.cboOrigen, 1, 1);
            this.tlpDatos.Controls.Add(this.lblDestino, 2, 1);
            this.tlpDatos.Controls.Add(this.cboDestino, 3, 1);
            this.tlpDatos.Controls.Add(this.lblFecha, 0, 2);
            this.tlpDatos.Controls.Add(this.dtpFecha, 1, 2);
            this.tlpDatos.Controls.Add(this.lblEstado, 2, 2);
            this.tlpDatos.Controls.Add(this.txtEstado, 3, 2);
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
            // lblEmpleado
            // 
            this.lblEmpleado.Name = "lblEmpleado";
            this.lblEmpleado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblEmpleado.AutoSize = true;
            this.lblEmpleado.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpleado.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEmpleado.Text = "Empleado:";
            // 
            // cboEmpleado
            // 
            this.cboEmpleado.Name = "cboEmpleado";
            this.cboEmpleado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmpleado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEmpleado.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblOrigen
            // 
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigen.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblOrigen.Text = "Origen:";
            // 
            // cboOrigen
            // 
            this.cboOrigen.Name = "cboOrigen";
            this.cboOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrigen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOrigen.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblDestino
            // 
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblDestino.AutoSize = true;
            this.lblDestino.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestino.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblDestino.Text = "Destino:";
            // 
            // cboDestino
            // 
            this.cboDestino.Name = "cboDestino";
            this.cboDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDestino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDestino.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblFecha
            // 
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFecha.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFecha.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblEstado
            // 
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblEstado.Text = "Estado:";
            // 
            // txtEstado
            // 
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.ReadOnly = true;
            this.txtEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEstado.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.tlpAcciones.ColumnCount = 8;
            this.tlpAcciones.RowCount = 1;
            this.tlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAcciones.Controls.Add(this.btnNuevo, 0, 0);
            this.tlpAcciones.Controls.Add(this.btnGuardar, 1, 0);
            this.tlpAcciones.Controls.Add(this.btnCancelar, 2, 0);
            this.tlpAcciones.Controls.Add(this.btnEditar, 3, 0);
            this.tlpAcciones.Controls.Add(this.btnEliminar, 4, 0);
            this.tlpAcciones.Controls.Add(this.btnLimpiar, 5, 0);
            this.tlpAcciones.Controls.Add(this.btnEnTransito, 6, 0);
            this.tlpAcciones.Controls.Add(this.btnCompletar, 7, 0);
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
            this.btnNuevo.IconSize = 26;
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
            this.btnGuardar.IconSize = 26;
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
            this.btnCancelar.IconSize = 26;
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
            this.btnEditar.IconSize = 26;
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
            this.btnEliminar.IconSize = 26;
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
            this.btnLimpiar.IconSize = 26;
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnEnTransito
            // 
            this.btnEnTransito.Name = "btnEnTransito";
            this.btnEnTransito.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnTransito.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEnTransito.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnTransito.IconChar = FontAwesome.Sharp.IconChar.Truck;
            this.btnEnTransito.IconColor = System.Drawing.Color.Black;
            this.btnEnTransito.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEnTransito.IconSize = 26;
            this.btnEnTransito.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnEnTransito.Text = "En tránsito";
            this.btnEnTransito.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnTransito.UseVisualStyleBackColor = true;
            this.btnEnTransito.Click += new System.EventHandler(this.btnEnTransito_Click);
            // 
            // btnCompletar
            // 
            this.btnCompletar.Name = "btnCompletar";
            this.btnCompletar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCompletar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCompletar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompletar.IconChar = FontAwesome.Sharp.IconChar.Check;
            this.btnCompletar.IconColor = System.Drawing.Color.Black;
            this.btnCompletar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCompletar.IconSize = 26;
            this.btnCompletar.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btnCompletar.Text = "Completar";
            this.btnCompletar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCompletar.UseVisualStyleBackColor = true;
            this.btnCompletar.Click += new System.EventHandler(this.btnCompletar_Click);
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
            this.btnIconoGestionar.IconChar = FontAwesome.Sharp.IconChar.Truck;
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
            this.lblSubtituloGestionar.Text = "Crear un traslado, editarlo y cambiar su estado";
            // 
            // lblTituloGestionar
            // 
            this.lblTituloGestionar.Name = "lblTituloGestionar";
            this.lblTituloGestionar.AutoSize = true;
            this.lblTituloGestionar.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloGestionar.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTituloGestionar.Location = new System.Drawing.Point(118, 0);
            this.lblTituloGestionar.Text = "TRASLADOS";
            // 
            // TrasladosForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1250, 820);
            this.Name = "TrasladosForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Traslados entre sucursales";
            this.Controls.Add(this.tabControl1);
            this.Load += new System.EventHandler(this.TrasladosForms_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel PanelPrincipal;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.DataGridView dgvTraslados;
        private System.Windows.Forms.Panel panelDetalleConsulta;
        private System.Windows.Forms.DataGridView dgvDetalleConsulta;
        private System.Windows.Forms.Panel panelTituloDetalle;
        private System.Windows.Forms.Label lblDetalleTitulo;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelCerrar;
        private FontAwesome.Sharp.IconButton btnCerrar;
        private System.Windows.Forms.Label lblTotalRegistros;
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
        private System.Windows.Forms.GroupBox grpDetalle;
        private System.Windows.Forms.DataGridView dgvDetalleGestionar;
        private System.Windows.Forms.Panel panelAgregarDetalle;
        private System.Windows.Forms.Label lblProductoDetalle;
        private System.Windows.Forms.ComboBox cboProducto;
        private System.Windows.Forms.Label lblCantidadDetalle;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private FontAwesome.Sharp.IconButton btnAgregarDetalle;
        private FontAwesome.Sharp.IconButton btnQuitarDetalle;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.TableLayoutPanel tlpDatos;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblEmpleado;
        private System.Windows.Forms.ComboBox cboEmpleado;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.ComboBox cboOrigen;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.ComboBox cboDestino;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.Panel PanelInferiorAcciones;
        private System.Windows.Forms.GroupBox gboxAcciones;
        private System.Windows.Forms.TableLayoutPanel tlpAcciones;
        private FontAwesome.Sharp.IconButton btnNuevo;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private FontAwesome.Sharp.IconButton btnEditar;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconButton btnLimpiar;
        private FontAwesome.Sharp.IconButton btnEnTransito;
        private FontAwesome.Sharp.IconButton btnCompletar;
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