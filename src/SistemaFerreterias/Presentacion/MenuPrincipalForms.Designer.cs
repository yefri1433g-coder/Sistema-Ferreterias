namespace Presentacion
{
    partial class MenuPrincipalForms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuPrincipalForms));
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenuInventarios = new FontAwesome.Sharp.IconButton();
            this.btnMenuStock = new FontAwesome.Sharp.IconButton();
            this.btnMenuKardex = new FontAwesome.Sharp.IconButton();
            this.btnMenuProveedores = new FontAwesome.Sharp.IconButton();
            this.btnMenuTraslados = new FontAwesome.Sharp.IconButton();
            this.btnMenuDashboard = new FontAwesome.Sharp.IconButton();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.btnSalir = new FontAwesome.Sharp.IconButton();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ventasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelDivisor = new System.Windows.Forms.Panel();
            this.panelEncabezado = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlpMenu.SuspendLayout();
            this.panelInferior.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.panelEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMenu
            // 
            this.tlpMenu.ColumnCount = 3;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMenu.Controls.Add(this.btnMenuInventarios, 0, 0);
            this.tlpMenu.Controls.Add(this.btnMenuStock, 1, 0);
            this.tlpMenu.Controls.Add(this.btnMenuKardex, 2, 0);
            this.tlpMenu.Controls.Add(this.btnMenuProveedores, 0, 1);
            this.tlpMenu.Controls.Add(this.btnMenuTraslados, 1, 1);
            this.tlpMenu.Controls.Add(this.btnMenuDashboard, 2, 1);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMenu.Location = new System.Drawing.Point(0, 73);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 3;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMenu.Size = new System.Drawing.Size(889, 391);
            this.tlpMenu.TabIndex = 0;
            // 
            // btnMenuInventarios
            // 
            this.btnMenuInventarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuInventarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuInventarios.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuInventarios.IconChar = FontAwesome.Sharp.IconChar.Warehouse;
            this.btnMenuInventarios.IconColor = System.Drawing.Color.Black;
            this.btnMenuInventarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuInventarios.IconSize = 56;
            this.btnMenuInventarios.Location = new System.Drawing.Point(12, 11);
            this.btnMenuInventarios.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuInventarios.Name = "btnMenuInventarios";
            this.btnMenuInventarios.Size = new System.Drawing.Size(272, 163);
            this.btnMenuInventarios.TabIndex = 0;
            this.btnMenuInventarios.Text = "Inventarios";
            this.btnMenuInventarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuInventarios.UseVisualStyleBackColor = true;
            this.btnMenuInventarios.Click += new System.EventHandler(this.btnMenuInventarios_Click);
            // 
            // btnMenuStock
            // 
            this.btnMenuStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuStock.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuStock.IconChar = FontAwesome.Sharp.IconChar.BoxOpen;
            this.btnMenuStock.IconColor = System.Drawing.Color.Black;
            this.btnMenuStock.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuStock.IconSize = 56;
            this.btnMenuStock.Location = new System.Drawing.Point(308, 11);
            this.btnMenuStock.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuStock.Name = "btnMenuStock";
            this.btnMenuStock.Size = new System.Drawing.Size(272, 163);
            this.btnMenuStock.TabIndex = 1;
            this.btnMenuStock.Text = "Stock";
            this.btnMenuStock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuStock.UseVisualStyleBackColor = true;
            this.btnMenuStock.Click += new System.EventHandler(this.btnMenuStock_Click);
            // 
            // btnMenuKardex
            // 
            this.btnMenuKardex.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuKardex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuKardex.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuKardex.IconChar = FontAwesome.Sharp.IconChar.ClipboardList;
            this.btnMenuKardex.IconColor = System.Drawing.Color.Black;
            this.btnMenuKardex.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuKardex.IconSize = 56;
            this.btnMenuKardex.Location = new System.Drawing.Point(604, 11);
            this.btnMenuKardex.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuKardex.Name = "btnMenuKardex";
            this.btnMenuKardex.Size = new System.Drawing.Size(273, 163);
            this.btnMenuKardex.TabIndex = 2;
            this.btnMenuKardex.Text = "Kardex";
            this.btnMenuKardex.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuKardex.UseVisualStyleBackColor = true;
            this.btnMenuKardex.Click += new System.EventHandler(this.btnMenuKardex_Click);
            // 
            // btnMenuProveedores
            // 
            this.btnMenuProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuProveedores.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuProveedores.IconChar = FontAwesome.Sharp.IconChar.Handshake;
            this.btnMenuProveedores.IconColor = System.Drawing.Color.Black;
            this.btnMenuProveedores.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuProveedores.IconSize = 56;
            this.btnMenuProveedores.Location = new System.Drawing.Point(12, 196);
            this.btnMenuProveedores.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuProveedores.Name = "btnMenuProveedores";
            this.btnMenuProveedores.Size = new System.Drawing.Size(272, 163);
            this.btnMenuProveedores.TabIndex = 3;
            this.btnMenuProveedores.Text = "Proveedores";
            this.btnMenuProveedores.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuProveedores.UseVisualStyleBackColor = true;
            this.btnMenuProveedores.Click += new System.EventHandler(this.btnMenuProveedores_Click);
            // 
            // btnMenuTraslados
            // 
            this.btnMenuTraslados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuTraslados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuTraslados.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuTraslados.IconChar = FontAwesome.Sharp.IconChar.Truck;
            this.btnMenuTraslados.IconColor = System.Drawing.Color.Black;
            this.btnMenuTraslados.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuTraslados.IconSize = 56;
            this.btnMenuTraslados.Location = new System.Drawing.Point(308, 196);
            this.btnMenuTraslados.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuTraslados.Name = "btnMenuTraslados";
            this.btnMenuTraslados.Size = new System.Drawing.Size(272, 163);
            this.btnMenuTraslados.TabIndex = 4;
            this.btnMenuTraslados.Text = "Traslados";
            this.btnMenuTraslados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuTraslados.UseVisualStyleBackColor = true;
            this.btnMenuTraslados.Click += new System.EventHandler(this.btnMenuTraslados_Click);
            // 
            // btnMenuDashboard
            // 
            this.btnMenuDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuDashboard.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenuDashboard.IconChar = FontAwesome.Sharp.IconChar.ChartLine;
            this.btnMenuDashboard.IconColor = System.Drawing.Color.Black;
            this.btnMenuDashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenuDashboard.IconSize = 56;
            this.btnMenuDashboard.Location = new System.Drawing.Point(604, 196);
            this.btnMenuDashboard.Margin = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.btnMenuDashboard.Name = "btnMenuDashboard";
            this.btnMenuDashboard.Size = new System.Drawing.Size(273, 163);
            this.btnMenuDashboard.TabIndex = 5;
            this.btnMenuDashboard.Text = "Dashboard";
            this.btnMenuDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenuDashboard.UseVisualStyleBackColor = true;
            this.btnMenuDashboard.Click += new System.EventHandler(this.btnMenuDashboard_Click);
            // 
            // panelInferior
            // 
            this.panelInferior.Controls.Add(this.btnSalir);
            this.panelInferior.Controls.Add(this.menuStrip2);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(0, 464);
            this.panelInferior.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(889, 48);
            this.panelInferior.TabIndex = 1;
            // 
            // btnSalir
            // 
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.IconChar = FontAwesome.Sharp.IconChar.X;
            this.btnSalir.IconColor = System.Drawing.Color.Black;
            this.btnSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSalir.IconSize = 24;
            this.btnSalir.Location = new System.Drawing.Point(686, 3);
            this.btnSalir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(156, 34);
            this.btnSalir.TabIndex = 0;
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuariosToolStripMenuItem,
            this.clientesToolStripMenuItem,
            this.ventasToolStripMenuItem,
            this.pagosToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(889, 31);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(91, 27);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            this.usuariosToolStripMenuItem.Click += new System.EventHandler(this.usuariosToolStripMenuItem_Click);
            // 
            // clientesToolStripMenuItem
            // 
            this.clientesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(87, 27);
            this.clientesToolStripMenuItem.Text = "Clientes";
            this.clientesToolStripMenuItem.Click += new System.EventHandler(this.clientesToolStripMenuItem_Click);
            // 
            // ventasToolStripMenuItem
            // 
            this.ventasToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ventasToolStripMenuItem.Name = "ventasToolStripMenuItem";
            this.ventasToolStripMenuItem.Size = new System.Drawing.Size(76, 27);
            this.ventasToolStripMenuItem.Text = "Ventas";
            this.ventasToolStripMenuItem.Click += new System.EventHandler(this.ventasToolStripMenuItem_Click);
            // 
            // pagosToolStripMenuItem
            // 
            this.pagosToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagosToolStripMenuItem.Name = "pagosToolStripMenuItem";
            this.pagosToolStripMenuItem.Size = new System.Drawing.Size(71, 27);
            this.pagosToolStripMenuItem.Text = "Pagos";
            this.pagosToolStripMenuItem.Click += new System.EventHandler(this.pagosToolStripMenuItem_Click);
            // 
            // PanelDivisor
            // 
            this.PanelDivisor.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelDivisor.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDivisor.Location = new System.Drawing.Point(0, 72);
            this.PanelDivisor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelDivisor.Name = "PanelDivisor";
            this.PanelDivisor.Size = new System.Drawing.Size(889, 1);
            this.PanelDivisor.TabIndex = 2;
            // 
            // panelEncabezado
            // 
            this.panelEncabezado.Controls.Add(this.pictureBox1);
            this.panelEncabezado.Controls.Add(this.lblSubtitulo);
            this.panelEncabezado.Controls.Add(this.lblTitulo);
            this.panelEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEncabezado.Location = new System.Drawing.Point(0, 0);
            this.panelEncabezado.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEncabezado.Name = "panelEncabezado";
            this.panelEncabezado.Size = new System.Drawing.Size(889, 72);
            this.panelEncabezado.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitulo.Location = new System.Drawing.Point(109, 40);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(124, 23);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Menu Principal";
            this.lblSubtitulo.Click += new System.EventHandler(this.lblSubtitulo_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitulo.Location = new System.Drawing.Point(105, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(565, 50);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "FERRETERÍAS MULTISUCURSAL";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // MenuPrincipalForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(889, 512);
            this.Controls.Add(this.tlpMenu);
            this.Controls.Add(this.panelInferior);
            this.Controls.Add(this.PanelDivisor);
            this.Controls.Add(this.panelEncabezado);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MenuPrincipalForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ferreterías Multisucursal - Menú principal";
            this.tlpMenu.ResumeLayout(false);
            this.panelInferior.ResumeLayout(false);
            this.panelInferior.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.panelEncabezado.ResumeLayout(false);
            this.panelEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private FontAwesome.Sharp.IconButton btnMenuInventarios;
        private FontAwesome.Sharp.IconButton btnMenuStock;
        private FontAwesome.Sharp.IconButton btnMenuKardex;
        private FontAwesome.Sharp.IconButton btnMenuProveedores;
        private FontAwesome.Sharp.IconButton btnMenuTraslados;
        private FontAwesome.Sharp.IconButton btnMenuDashboard;
        private System.Windows.Forms.Panel panelInferior;
        private FontAwesome.Sharp.IconButton btnSalir;
        private System.Windows.Forms.Panel PanelDivisor;
        private System.Windows.Forms.Panel panelEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ventasToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pagosToolStripMenuItem;
    }
}