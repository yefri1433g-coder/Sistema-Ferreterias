namespace Presentacion
{
    partial class KardexForms
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
            this.PanelPrincipal = new System.Windows.Forms.Panel();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.dgvKardex = new System.Windows.Forms.DataGridView();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelCerrar = new System.Windows.Forms.Panel();
            this.btnCerrar = new FontAwesome.Sharp.IconButton();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.PanelCheckBox = new System.Windows.Forms.Panel();
            this.PanelAccionesTabla = new System.Windows.Forms.Panel();
            this.btnImprimir = new FontAwesome.Sharp.IconButton();
            this.btnExportar = new FontAwesome.Sharp.IconButton();
            this.ChkSeleccionar = new System.Windows.Forms.CheckBox();
            this.PanelDivisor2 = new System.Windows.Forms.Panel();
            this.PanelFiltros = new System.Windows.Forms.Panel();
            this.btnLimpiarFiltro = new FontAwesome.Sharp.IconButton();
            this.btnBuscar = new FontAwesome.Sharp.IconButton();
            this.cboFiltro = new System.Windows.Forms.ComboBox();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.PanelDivisor = new System.Windows.Forms.Panel();
            this.panelEncabezado = new System.Windows.Forms.Panel();
            this.btnIcono = new FontAwesome.Sharp.IconButton();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.panelCentral.Controls.Add(this.dgvKardex);
            // 
            // dgvKardex
            // 
            this.dgvKardex.Name = "dgvKardex";
            this.dgvKardex.AllowUserToAddRows = false;
            this.dgvKardex.AllowUserToDeleteRows = false;
            this.dgvKardex.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvKardex.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvKardex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKardex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKardex.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKardex.MultiSelect = true;
            this.dgvKardex.ReadOnly = true;
            this.dgvKardex.RowHeadersWidth = 51;
            this.dgvKardex.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKardex.RowTemplate.Height = 24;
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
            this.PanelFiltros.Controls.Add(this.cboFiltro);
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
            // cboFiltro
            // 
            this.cboFiltro.Name = "cboFiltro";
            this.cboFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltro.Location = new System.Drawing.Point(114, 22);
            this.cboFiltro.Size = new System.Drawing.Size(562, 33);
            // 
            // lblFiltro
            // 
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(5, 26);
            this.lblFiltro.Text = "Producto:";
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
            this.btnIcono.IconChar = FontAwesome.Sharp.IconChar.ClipboardList;
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
            this.lblSubtitulo.Text = "Historial de movimientos de inventario por producto";
            // 
            // lblTitulo
            // 
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitulo.Location = new System.Drawing.Point(118, 0);
            this.lblTitulo.Text = "KARDEX";
            // 
            // KardexForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Name = "KardexForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kardex";
            this.Controls.Add(this.PanelPrincipal);
            this.Load += new System.EventHandler(this.KardexForms_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel PanelPrincipal;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.DataGridView dgvKardex;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelCerrar;
        private FontAwesome.Sharp.IconButton btnCerrar;
        private System.Windows.Forms.Label lblTotalRegistros;
        private System.Windows.Forms.Panel PanelCheckBox;
        private System.Windows.Forms.Panel PanelAccionesTabla;
        private FontAwesome.Sharp.IconButton btnImprimir;
        private FontAwesome.Sharp.IconButton btnExportar;
        private System.Windows.Forms.CheckBox ChkSeleccionar;
        private System.Windows.Forms.Panel PanelDivisor2;
        private System.Windows.Forms.Panel PanelFiltros;
        private FontAwesome.Sharp.IconButton btnLimpiarFiltro;
        private FontAwesome.Sharp.IconButton btnBuscar;
        private System.Windows.Forms.ComboBox cboFiltro;
        private System.Windows.Forms.Label lblFiltro;
        private System.Windows.Forms.Panel PanelDivisor;
        private System.Windows.Forms.Panel panelEncabezado;
        private FontAwesome.Sharp.IconButton btnIcono;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
    }
}