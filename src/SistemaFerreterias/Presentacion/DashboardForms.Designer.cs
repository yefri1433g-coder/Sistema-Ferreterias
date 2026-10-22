namespace Presentacion
{
    partial class DashboardForms
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
            this.panelCentral = new System.Windows.Forms.Panel();
            this.dgvStockBajo = new System.Windows.Forms.DataGridView();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelBotonesDerecha = new System.Windows.Forms.Panel();
            this.btnActualizar = new FontAwesome.Sharp.IconButton();
            this.btnCerrar = new FontAwesome.Sharp.IconButton();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.panelTituloBajo = new System.Windows.Forms.Panel();
            this.lblStockBajoTitulo = new System.Windows.Forms.Label();
            this.panelGraficos = new System.Windows.Forms.Panel();
            this.tlpGraficos = new System.Windows.Forms.TableLayoutPanel();
            this.graficoStock = new Presentacion.PanelGrafico();
            this.graficoTraslados = new Presentacion.PanelGrafico();
            this.panelTarjetas = new System.Windows.Forms.Panel();
            this.tlpTarjetas = new System.Windows.Forms.TableLayoutPanel();
            this.panelTarjetaProductos = new System.Windows.Forms.Panel();
            this.lblValorProductos = new System.Windows.Forms.Label();
            this.lblTituloProductos = new System.Windows.Forms.Label();
            this.panelTarjetaSucursales = new System.Windows.Forms.Panel();
            this.lblValorSucursales = new System.Windows.Forms.Label();
            this.lblTituloSucursales = new System.Windows.Forms.Label();
            this.panelTarjetaProveedores = new System.Windows.Forms.Panel();
            this.lblValorProveedores = new System.Windows.Forms.Label();
            this.lblTituloProveedores = new System.Windows.Forms.Label();
            this.panelTarjetaStockBajo = new System.Windows.Forms.Panel();
            this.lblValorStockBajo = new System.Windows.Forms.Label();
            this.lblTituloStockBajo = new System.Windows.Forms.Label();
            this.panelTarjetaPendientes = new System.Windows.Forms.Panel();
            this.lblValorPendientes = new System.Windows.Forms.Label();
            this.lblTituloPendientes = new System.Windows.Forms.Label();
            this.PanelDivisor = new System.Windows.Forms.Panel();
            this.panelEncabezado = new System.Windows.Forms.Panel();
            this.btnIcono = new FontAwesome.Sharp.IconButton();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelCentral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBajo)).BeginInit();
            this.panelInferior.SuspendLayout();
            this.panelBotonesDerecha.SuspendLayout();
            this.panelTituloBajo.SuspendLayout();
            this.panelGraficos.SuspendLayout();
            this.tlpGraficos.SuspendLayout();
            this.panelTarjetas.SuspendLayout();
            this.tlpTarjetas.SuspendLayout();
            this.panelTarjetaProductos.SuspendLayout();
            this.panelTarjetaSucursales.SuspendLayout();
            this.panelTarjetaProveedores.SuspendLayout();
            this.panelTarjetaStockBajo.SuspendLayout();
            this.panelTarjetaPendientes.SuspendLayout();
            this.panelEncabezado.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCentral
            // 
            this.panelCentral.Controls.Add(this.dgvStockBajo);
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Location = new System.Drawing.Point(0, 447);
            this.panelCentral.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(1111, 169);
            this.panelCentral.TabIndex = 0;
            // 
            // dgvStockBajo
            // 
            this.dgvStockBajo.AllowUserToAddRows = false;
            this.dgvStockBajo.AllowUserToDeleteRows = false;
            this.dgvStockBajo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvStockBajo.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvStockBajo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockBajo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockBajo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStockBajo.Location = new System.Drawing.Point(0, 0);
            this.dgvStockBajo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvStockBajo.Name = "dgvStockBajo";
            this.dgvStockBajo.ReadOnly = true;
            this.dgvStockBajo.RowHeadersWidth = 51;
            this.dgvStockBajo.RowTemplate.Height = 24;
            this.dgvStockBajo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockBajo.Size = new System.Drawing.Size(1111, 169);
            this.dgvStockBajo.TabIndex = 0;
            // 
            // panelInferior
            // 
            this.panelInferior.Controls.Add(this.panelBotonesDerecha);
            this.panelInferior.Controls.Add(this.lblTotalRegistros);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(0, 616);
            this.panelInferior.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(1111, 40);
            this.panelInferior.TabIndex = 1;
            // 
            // panelBotonesDerecha
            // 
            this.panelBotonesDerecha.Controls.Add(this.btnActualizar);
            this.panelBotonesDerecha.Controls.Add(this.btnCerrar);
            this.panelBotonesDerecha.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelBotonesDerecha.Location = new System.Drawing.Point(755, 0);
            this.panelBotonesDerecha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBotonesDerecha.Name = "panelBotonesDerecha";
            this.panelBotonesDerecha.Size = new System.Drawing.Size(356, 40);
            this.panelBotonesDerecha.TabIndex = 0;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.IconChar = FontAwesome.Sharp.IconChar.Sync;
            this.btnActualizar.IconColor = System.Drawing.Color.Black;
            this.btnActualizar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnActualizar.IconSize = 24;
            this.btnActualizar.Location = new System.Drawing.Point(9, 3);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(156, 34);
            this.btnActualizar.TabIndex = 0;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnCerrar.IconColor = System.Drawing.Color.Black;
            this.btnCerrar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrar.IconSize = 24;
            this.btnCerrar.Location = new System.Drawing.Point(178, 3);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(156, 34);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblTotalRegistros
            // 
            this.lblTotalRegistros.AutoSize = true;
            this.lblTotalRegistros.Location = new System.Drawing.Point(26, 12);
            this.lblTotalRegistros.Name = "lblTotalRegistros";
            this.lblTotalRegistros.Size = new System.Drawing.Size(112, 16);
            this.lblTotalRegistros.TabIndex = 1;
            this.lblTotalRegistros.Text = "Total Registros: 0";
            // 
            // panelTituloBajo
            // 
            this.panelTituloBajo.Controls.Add(this.lblStockBajoTitulo);
            this.panelTituloBajo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTituloBajo.Location = new System.Drawing.Point(0, 417);
            this.panelTituloBajo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTituloBajo.Name = "panelTituloBajo";
            this.panelTituloBajo.Size = new System.Drawing.Size(1111, 30);
            this.panelTituloBajo.TabIndex = 2;
            // 
            // lblStockBajoTitulo
            // 
            this.lblStockBajoTitulo.AutoSize = true;
            this.lblStockBajoTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockBajoTitulo.ForeColor = System.Drawing.Color.Firebrick;
            this.lblStockBajoTitulo.Location = new System.Drawing.Point(9, 6);
            this.lblStockBajoTitulo.Name = "lblStockBajoTitulo";
            this.lblStockBajoTitulo.Size = new System.Drawing.Size(531, 25);
            this.lblStockBajoTitulo.TabIndex = 0;
            this.lblStockBajoTitulo.Text = "Productos con stock bajo (cantidad menor o igual al mínimo)";
            // 
            // panelGraficos
            // 
            this.panelGraficos.Controls.Add(this.tlpGraficos);
            this.panelGraficos.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGraficos.Location = new System.Drawing.Point(0, 177);
            this.panelGraficos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelGraficos.Name = "panelGraficos";
            this.panelGraficos.Size = new System.Drawing.Size(1111, 240);
            this.panelGraficos.TabIndex = 3;
            // 
            // tlpGraficos
            // 
            this.tlpGraficos.ColumnCount = 2;
            this.tlpGraficos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tlpGraficos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tlpGraficos.Controls.Add(this.graficoStock, 0, 0);
            this.tlpGraficos.Controls.Add(this.graficoTraslados, 1, 0);
            this.tlpGraficos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGraficos.Location = new System.Drawing.Point(0, 0);
            this.tlpGraficos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpGraficos.Name = "tlpGraficos";
            this.tlpGraficos.RowCount = 1;
            this.tlpGraficos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGraficos.Size = new System.Drawing.Size(1111, 240);
            this.tlpGraficos.TabIndex = 0;
            // 
            // graficoStock
            // 
            this.graficoStock.BackColor = System.Drawing.Color.White;
            this.graficoStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graficoStock.ColorBarra = System.Drawing.Color.SteelBlue;
            this.graficoStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graficoStock.Location = new System.Drawing.Point(7, 6);
            this.graficoStock.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.graficoStock.Name = "graficoStock";
            this.graficoStock.Size = new System.Drawing.Size(674, 228);
            this.graficoStock.TabIndex = 0;
            this.graficoStock.Titulo = "Stock total por sucursal";
            // 
            // graficoTraslados
            // 
            this.graficoTraslados.BackColor = System.Drawing.Color.White;
            this.graficoTraslados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graficoTraslados.ColorBarra = System.Drawing.Color.SeaGreen;
            this.graficoTraslados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graficoTraslados.Location = new System.Drawing.Point(695, 6);
            this.graficoTraslados.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.graficoTraslados.Name = "graficoTraslados";
            this.graficoTraslados.Size = new System.Drawing.Size(409, 228);
            this.graficoTraslados.TabIndex = 1;
            this.graficoTraslados.Titulo = "Traslados por estado";
            // 
            // panelTarjetas
            // 
            this.panelTarjetas.Controls.Add(this.tlpTarjetas);
            this.panelTarjetas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTarjetas.Location = new System.Drawing.Point(0, 73);
            this.panelTarjetas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTarjetas.Name = "panelTarjetas";
            this.panelTarjetas.Size = new System.Drawing.Size(1111, 104);
            this.panelTarjetas.TabIndex = 4;
            // 
            // tlpTarjetas
            // 
            this.tlpTarjetas.ColumnCount = 5;
            this.tlpTarjetas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTarjetas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTarjetas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTarjetas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTarjetas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTarjetas.Controls.Add(this.panelTarjetaProductos, 0, 0);
            this.tlpTarjetas.Controls.Add(this.panelTarjetaSucursales, 1, 0);
            this.tlpTarjetas.Controls.Add(this.panelTarjetaProveedores, 2, 0);
            this.tlpTarjetas.Controls.Add(this.panelTarjetaStockBajo, 3, 0);
            this.tlpTarjetas.Controls.Add(this.panelTarjetaPendientes, 4, 0);
            this.tlpTarjetas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTarjetas.Location = new System.Drawing.Point(0, 0);
            this.tlpTarjetas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpTarjetas.Name = "tlpTarjetas";
            this.tlpTarjetas.RowCount = 1;
            this.tlpTarjetas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTarjetas.Size = new System.Drawing.Size(1111, 104);
            this.tlpTarjetas.TabIndex = 0;
            // 
            // panelTarjetaProductos
            // 
            this.panelTarjetaProductos.BackColor = System.Drawing.Color.SteelBlue;
            this.panelTarjetaProductos.Controls.Add(this.lblValorProductos);
            this.panelTarjetaProductos.Controls.Add(this.lblTituloProductos);
            this.panelTarjetaProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarjetaProductos.Location = new System.Drawing.Point(7, 6);
            this.panelTarjetaProductos.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelTarjetaProductos.Name = "panelTarjetaProductos";
            this.panelTarjetaProductos.Size = new System.Drawing.Size(208, 92);
            this.panelTarjetaProductos.TabIndex = 0;
            // 
            // lblValorProductos
            // 
            this.lblValorProductos.AutoSize = true;
            this.lblValorProductos.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorProductos.ForeColor = System.Drawing.Color.White;
            this.lblValorProductos.Location = new System.Drawing.Point(11, 29);
            this.lblValorProductos.Name = "lblValorProductos";
            this.lblValorProductos.Size = new System.Drawing.Size(54, 62);
            this.lblValorProductos.TabIndex = 0;
            this.lblValorProductos.Text = "0";
            // 
            // lblTituloProductos
            // 
            this.lblTituloProductos.AutoSize = true;
            this.lblTituloProductos.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloProductos.ForeColor = System.Drawing.Color.White;
            this.lblTituloProductos.Location = new System.Drawing.Point(11, 8);
            this.lblTituloProductos.Name = "lblTituloProductos";
            this.lblTituloProductos.Size = new System.Drawing.Size(153, 25);
            this.lblTituloProductos.TabIndex = 1;
            this.lblTituloProductos.Text = "Productos activos";
            // 
            // panelTarjetaSucursales
            // 
            this.panelTarjetaSucursales.BackColor = System.Drawing.Color.SeaGreen;
            this.panelTarjetaSucursales.Controls.Add(this.lblValorSucursales);
            this.panelTarjetaSucursales.Controls.Add(this.lblTituloSucursales);
            this.panelTarjetaSucursales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarjetaSucursales.Location = new System.Drawing.Point(229, 6);
            this.panelTarjetaSucursales.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelTarjetaSucursales.Name = "panelTarjetaSucursales";
            this.panelTarjetaSucursales.Size = new System.Drawing.Size(208, 92);
            this.panelTarjetaSucursales.TabIndex = 1;
            // 
            // lblValorSucursales
            // 
            this.lblValorSucursales.AutoSize = true;
            this.lblValorSucursales.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorSucursales.ForeColor = System.Drawing.Color.White;
            this.lblValorSucursales.Location = new System.Drawing.Point(11, 29);
            this.lblValorSucursales.Name = "lblValorSucursales";
            this.lblValorSucursales.Size = new System.Drawing.Size(54, 62);
            this.lblValorSucursales.TabIndex = 0;
            this.lblValorSucursales.Text = "0";
            // 
            // lblTituloSucursales
            // 
            this.lblTituloSucursales.AutoSize = true;
            this.lblTituloSucursales.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloSucursales.ForeColor = System.Drawing.Color.White;
            this.lblTituloSucursales.Location = new System.Drawing.Point(11, 8);
            this.lblTituloSucursales.Name = "lblTituloSucursales";
            this.lblTituloSucursales.Size = new System.Drawing.Size(94, 25);
            this.lblTituloSucursales.TabIndex = 1;
            this.lblTituloSucursales.Text = "Sucursales";
            // 
            // panelTarjetaProveedores
            // 
            this.panelTarjetaProveedores.BackColor = System.Drawing.Color.DarkOrange;
            this.panelTarjetaProveedores.Controls.Add(this.lblValorProveedores);
            this.panelTarjetaProveedores.Controls.Add(this.lblTituloProveedores);
            this.panelTarjetaProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarjetaProveedores.Location = new System.Drawing.Point(451, 6);
            this.panelTarjetaProveedores.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelTarjetaProveedores.Name = "panelTarjetaProveedores";
            this.panelTarjetaProveedores.Size = new System.Drawing.Size(208, 92);
            this.panelTarjetaProveedores.TabIndex = 2;
            // 
            // lblValorProveedores
            // 
            this.lblValorProveedores.AutoSize = true;
            this.lblValorProveedores.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorProveedores.ForeColor = System.Drawing.Color.White;
            this.lblValorProveedores.Location = new System.Drawing.Point(5, 30);
            this.lblValorProveedores.Name = "lblValorProveedores";
            this.lblValorProveedores.Size = new System.Drawing.Size(54, 62);
            this.lblValorProveedores.TabIndex = 0;
            this.lblValorProveedores.Text = "0";
            this.lblValorProveedores.Click += new System.EventHandler(this.lblValorProveedores_Click);
            // 
            // lblTituloProveedores
            // 
            this.lblTituloProveedores.AutoSize = true;
            this.lblTituloProveedores.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloProveedores.ForeColor = System.Drawing.Color.White;
            this.lblTituloProveedores.Location = new System.Drawing.Point(11, 8);
            this.lblTituloProveedores.Name = "lblTituloProveedores";
            this.lblTituloProveedores.Size = new System.Drawing.Size(111, 25);
            this.lblTituloProveedores.TabIndex = 1;
            this.lblTituloProveedores.Text = "Proveedores";
            // 
            // panelTarjetaStockBajo
            // 
            this.panelTarjetaStockBajo.BackColor = System.Drawing.Color.Firebrick;
            this.panelTarjetaStockBajo.Controls.Add(this.lblValorStockBajo);
            this.panelTarjetaStockBajo.Controls.Add(this.lblTituloStockBajo);
            this.panelTarjetaStockBajo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarjetaStockBajo.Location = new System.Drawing.Point(673, 6);
            this.panelTarjetaStockBajo.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelTarjetaStockBajo.Name = "panelTarjetaStockBajo";
            this.panelTarjetaStockBajo.Size = new System.Drawing.Size(208, 92);
            this.panelTarjetaStockBajo.TabIndex = 3;
            // 
            // lblValorStockBajo
            // 
            this.lblValorStockBajo.AutoSize = true;
            this.lblValorStockBajo.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorStockBajo.ForeColor = System.Drawing.Color.White;
            this.lblValorStockBajo.Location = new System.Drawing.Point(11, 29);
            this.lblValorStockBajo.Name = "lblValorStockBajo";
            this.lblValorStockBajo.Size = new System.Drawing.Size(54, 62);
            this.lblValorStockBajo.TabIndex = 0;
            this.lblValorStockBajo.Text = "0";
            // 
            // lblTituloStockBajo
            // 
            this.lblTituloStockBajo.AutoSize = true;
            this.lblTituloStockBajo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloStockBajo.ForeColor = System.Drawing.Color.White;
            this.lblTituloStockBajo.Location = new System.Drawing.Point(11, 8);
            this.lblTituloStockBajo.Name = "lblTituloStockBajo";
            this.lblTituloStockBajo.Size = new System.Drawing.Size(95, 25);
            this.lblTituloStockBajo.TabIndex = 1;
            this.lblTituloStockBajo.Text = "Stock bajo";
            // 
            // panelTarjetaPendientes
            // 
            this.panelTarjetaPendientes.BackColor = System.Drawing.Color.SlateBlue;
            this.panelTarjetaPendientes.Controls.Add(this.lblValorPendientes);
            this.panelTarjetaPendientes.Controls.Add(this.lblTituloPendientes);
            this.panelTarjetaPendientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarjetaPendientes.Location = new System.Drawing.Point(895, 6);
            this.panelTarjetaPendientes.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.panelTarjetaPendientes.Name = "panelTarjetaPendientes";
            this.panelTarjetaPendientes.Size = new System.Drawing.Size(209, 92);
            this.panelTarjetaPendientes.TabIndex = 4;
            // 
            // lblValorPendientes
            // 
            this.lblValorPendientes.AutoSize = true;
            this.lblValorPendientes.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPendientes.ForeColor = System.Drawing.Color.White;
            this.lblValorPendientes.Location = new System.Drawing.Point(11, 29);
            this.lblValorPendientes.Name = "lblValorPendientes";
            this.lblValorPendientes.Size = new System.Drawing.Size(54, 62);
            this.lblValorPendientes.TabIndex = 0;
            this.lblValorPendientes.Text = "0";
            // 
            // lblTituloPendientes
            // 
            this.lblTituloPendientes.AutoSize = true;
            this.lblTituloPendientes.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloPendientes.ForeColor = System.Drawing.Color.White;
            this.lblTituloPendientes.Location = new System.Drawing.Point(11, 8);
            this.lblTituloPendientes.Name = "lblTituloPendientes";
            this.lblTituloPendientes.Size = new System.Drawing.Size(179, 25);
            this.lblTituloPendientes.TabIndex = 1;
            this.lblTituloPendientes.Text = "Traslados registrados";
            // 
            // PanelDivisor
            // 
            this.PanelDivisor.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelDivisor.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDivisor.Location = new System.Drawing.Point(0, 72);
            this.PanelDivisor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelDivisor.Name = "PanelDivisor";
            this.PanelDivisor.Size = new System.Drawing.Size(1111, 1);
            this.PanelDivisor.TabIndex = 5;
            // 
            // panelEncabezado
            // 
            this.panelEncabezado.Controls.Add(this.btnIcono);
            this.panelEncabezado.Controls.Add(this.lblSubtitulo);
            this.panelEncabezado.Controls.Add(this.lblTitulo);
            this.panelEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEncabezado.Location = new System.Drawing.Point(0, 0);
            this.panelEncabezado.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEncabezado.Name = "panelEncabezado";
            this.panelEncabezado.Size = new System.Drawing.Size(1111, 82);
            this.panelEncabezado.TabIndex = 6;
            // 
            // btnIcono
            // 
            this.btnIcono.BackColor = System.Drawing.Color.White;
            this.btnIcono.FlatAppearance.BorderSize = 0;
            this.btnIcono.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnIcono.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnIcono.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIcono.IconChar = FontAwesome.Sharp.IconChar.ChartLine;
            this.btnIcono.IconColor = System.Drawing.Color.DarkBlue;
            this.btnIcono.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnIcono.IconSize = 56;
            this.btnIcono.Location = new System.Drawing.Point(9, 5);
            this.btnIcono.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnIcono.Name = "btnIcono";
            this.btnIcono.Size = new System.Drawing.Size(89, 62);
            this.btnIcono.TabIndex = 0;
            this.btnIcono.TabStop = false;
            this.btnIcono.UseVisualStyleBackColor = false;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitulo.Location = new System.Drawing.Point(109, 53);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(452, 23);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Resumen del inventario y de los traslados entre sucursales";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitulo.Location = new System.Drawing.Point(105, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(251, 50);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "DASHBOARD";
            // 
            // DashboardForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1111, 656);
            this.Controls.Add(this.panelCentral);
            this.Controls.Add(this.panelInferior);
            this.Controls.Add(this.panelTituloBajo);
            this.Controls.Add(this.panelGraficos);
            this.Controls.Add(this.panelTarjetas);
            this.Controls.Add(this.PanelDivisor);
            this.Controls.Add(this.panelEncabezado);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DashboardForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.DashboardForms_Load);
            this.panelCentral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBajo)).EndInit();
            this.panelInferior.ResumeLayout(false);
            this.panelInferior.PerformLayout();
            this.panelBotonesDerecha.ResumeLayout(false);
            this.panelTituloBajo.ResumeLayout(false);
            this.panelTituloBajo.PerformLayout();
            this.panelGraficos.ResumeLayout(false);
            this.tlpGraficos.ResumeLayout(false);
            this.panelTarjetas.ResumeLayout(false);
            this.tlpTarjetas.ResumeLayout(false);
            this.panelTarjetaProductos.ResumeLayout(false);
            this.panelTarjetaProductos.PerformLayout();
            this.panelTarjetaSucursales.ResumeLayout(false);
            this.panelTarjetaSucursales.PerformLayout();
            this.panelTarjetaProveedores.ResumeLayout(false);
            this.panelTarjetaProveedores.PerformLayout();
            this.panelTarjetaStockBajo.ResumeLayout(false);
            this.panelTarjetaStockBajo.PerformLayout();
            this.panelTarjetaPendientes.ResumeLayout(false);
            this.panelTarjetaPendientes.PerformLayout();
            this.panelEncabezado.ResumeLayout(false);
            this.panelEncabezado.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.DataGridView dgvStockBajo;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelBotonesDerecha;
        private FontAwesome.Sharp.IconButton btnActualizar;
        private FontAwesome.Sharp.IconButton btnCerrar;
        private System.Windows.Forms.Label lblTotalRegistros;
        private System.Windows.Forms.Panel panelTituloBajo;
        private System.Windows.Forms.Label lblStockBajoTitulo;
        private System.Windows.Forms.Panel panelGraficos;
        private System.Windows.Forms.TableLayoutPanel tlpGraficos;
        private Presentacion.PanelGrafico graficoStock;
        private Presentacion.PanelGrafico graficoTraslados;
        private System.Windows.Forms.Panel panelTarjetas;
        private System.Windows.Forms.TableLayoutPanel tlpTarjetas;
        private System.Windows.Forms.Panel panelTarjetaProductos;
        private System.Windows.Forms.Label lblValorProductos;
        private System.Windows.Forms.Label lblTituloProductos;
        private System.Windows.Forms.Panel panelTarjetaSucursales;
        private System.Windows.Forms.Label lblValorSucursales;
        private System.Windows.Forms.Label lblTituloSucursales;
        private System.Windows.Forms.Panel panelTarjetaProveedores;
        private System.Windows.Forms.Label lblValorProveedores;
        private System.Windows.Forms.Label lblTituloProveedores;
        private System.Windows.Forms.Panel panelTarjetaStockBajo;
        private System.Windows.Forms.Label lblValorStockBajo;
        private System.Windows.Forms.Label lblTituloStockBajo;
        private System.Windows.Forms.Panel panelTarjetaPendientes;
        private System.Windows.Forms.Label lblValorPendientes;
        private System.Windows.Forms.Label lblTituloPendientes;
        private System.Windows.Forms.Panel PanelDivisor;
        private System.Windows.Forms.Panel panelEncabezado;
        private FontAwesome.Sharp.IconButton btnIcono;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
    }
}
