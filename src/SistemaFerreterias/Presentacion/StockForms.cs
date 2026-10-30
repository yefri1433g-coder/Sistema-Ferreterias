using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE StockForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class StockForms : Form
    {
        // Instanciar: Que el forms actual (capa presentacion) pueda acceder a otra capa (Negocio)
        private readonly StockNegocio _stockNegocio = new StockNegocio();
        private readonly CatalogosNegocio _catalogosNegocio = new CatalogosNegocio();

        // Variables de control: registro cargado en Gestionar y modo actual (Agregar / Editar)
        private int _codigoSeleccionado = 0;
        private int _codigoSucursalSeleccionado = 0;
        private int _codigoProductoSeleccionado = 0;
        private string _modo = string.Empty;
        private readonly Label[] _resumenStock = new Label[3];
        private readonly Label[] _detalleResumenStock = new Label[3];
        private DataGridView _gridStockResumen;
        private TextBox _buscarStockResumen;
        private CheckBox _seleccionarTodoStock;
        private Label _totalStockResumen;
        private List<StockEntidad> _listaStockResumen = new List<StockEntidad>();
        private Form _dialogoStock;
        private ComboBox _cboSucursalEditorStock, _cboProductoEditorStock;
        private NumericUpDown _nudCantidadEditorStock, _nudMinimoEditorStock;
        private Button _guardarEditorStock, _limpiarEditorStock;
        private Button _editarStockResumen;
        private Button _anteriorStock, _siguienteStock;
        private Label _paginaStock;
        private int _paginaActualStock = 1;
        private const int FilasPorPaginaStock = 10;

        public StockForms()
        {
            InitializeComponent();
            // La consulta ya no abre el editor al hacer doble clic.
            dgvStock.CellDoubleClick -= dgv_CellDoubleClick;
            EstiloAplicacion.Aplicar(this);
            PrepararDisenoStock();
            BarraListado.Configurar(_gridStockResumen, _buscarStockResumen, "Stock");
            PrepararDialogoStock();
        }

        // CONFIGURACIÓN: PrepararDisenoStock
        private void PrepararDisenoStock()
        {
            Color fondo = EstiloAplicacion.Fondo;
            Color tinta = EstiloAplicacion.Tinta;
            Color secundaria = Color.FromArgb(117, 134, 143);
            Color verde = EstiloAplicacion.Acento;

            BackColor = fondo;
            tabPage1.Padding = Padding.Empty;
            tabPage1.BackColor = fondo;
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            tabPage1.Controls.Clear();
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
            // Se retiran de la interfaz las pestañas antiguas de consulta y gestionar.
            tabPage2.Dispose();
            PanelPrincipal.Dispose();

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = fondo };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = fondo };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Stock", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            encabezado.Controls.Add(new Label { Text = "Stock", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = tinta });
            encabezado.Controls.Add(new Label { Text = "Existencias por sucursal; cada movimiento queda registrado en Kardex.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = EstiloAplicacion.Secundario });

            Panel resumen = new Panel { Dock = DockStyle.Fill, BackColor = fondo, Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = fondo };
            string[] titulos = { "DISPONIBLES", "STOCK BAJO", "AGOTADOS" };
            for (int i = 0; i < titulos.Length; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "↗" : i == 1 ? "◷" : "≡", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : verde, BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : EstiloAplicacion.FondoSuave };
                Label tituloTarjeta = new Label { Text = titulos[i], Left = 14, Top = 42, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario };
                _resumenStock[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = tinta };
                _detalleResumenStock[i] = new Label { Visible = false };
                tarjeta.Controls.Add(icono);
                tarjeta.Controls.Add(tituloTarjeta);
                tarjeta.Controls.Add(_resumenStock[i]);
                tarjeta.Controls.Add(_detalleResumenStock[i]);
                RedondearPanel(tarjeta, 10);
                tarjetas.Controls.Add(tarjeta, i, 0);
            }
            resumen.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            TableLayoutPanel tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Color.White, Margin = Padding.Empty };
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            Panel barra = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Existencias por producto y sucursal", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = tinta });
            barra.Controls.Add(new Label { Text = "Consulta, filtra y administra el stock desde esta pantalla", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 38, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 2, 0, 0) };
            _buscarStockResumen = new TextBox { Width = 180, Height = 28, Margin = new Padding(0, 1, 5, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar stock", ForeColor = EstiloAplicacion.Secundario, BackColor = Color.White };
            _buscarStockResumen.Enter += (s, e) => { if (_buscarStockResumen.Text == "Buscar stock") { _buscarStockResumen.Clear(); _buscarStockResumen.ForeColor = EstiloAplicacion.Tinta; } };
            _buscarStockResumen.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarStockResumen.Text)) { _buscarStockResumen.Text = "Buscar stock"; _buscarStockResumen.ForeColor = EstiloAplicacion.Secundario; } };
            _buscarStockResumen.TextChanged += (s, e) => { _paginaActualStock = 1; FiltrarStockResumen(); };
            Button limpiar = CrearBotonStock("Limpiar", false); limpiar.Width = 80; limpiar.Height = 30; limpiar.Click += (s, e) => { _buscarStockResumen.Text = ""; _buscarStockResumen.Focus(); };
            _seleccionarTodoStock = new CheckBox { Text = "Seleccionar", AutoSize = true, Margin = new Padding(8, 7, 2, 0), Font = new Font("Segoe UI", 8F), ForeColor = tinta };
            _seleccionarTodoStock.CheckedChanged += (s, e) =>
            {
                if (_seleccionarTodoStock.Checked) _gridStockResumen.SelectAll();
                else _gridStockResumen.ClearSelection();
            };
            Button imprimir = CrearBotonStock("Imprimir", false); imprimir.Width = 88; imprimir.Height = 30; imprimir.Click += btnImprimir_Click;
            Button exportar = CrearBotonStock("Exportar", false); exportar.Width = 88; exportar.Height = 30; exportar.Click += btnExportar_Click;
            _editarStockResumen = CrearBotonStock("Editar", false); _editarStockResumen.Width = 82; _editarStockResumen.Height = 30; _editarStockResumen.Click += (s, e) => EditarStockResumen();
            Button eliminar = CrearBotonStock("Eliminar", false); eliminar.Width = 82; eliminar.Height = 30; eliminar.ForeColor = Color.FromArgb(185, 28, 28); eliminar.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230); eliminar.Click += (s, e) => EliminarStockResumen();
            Button nuevo = CrearBotonStock("+ Nuevo", true); nuevo.Width = 96; nuevo.Height = 30; nuevo.Click += (s, e) => AbrirEditorStock();
            acciones.Controls.Add(_buscarStockResumen);
            acciones.Controls.Add(limpiar);
            acciones.Controls.Add(_seleccionarTodoStock);
            acciones.Controls.Add(imprimir);
            acciones.Controls.Add(exportar);
            acciones.Controls.Add(_editarStockResumen);
            acciones.Controls.Add(eliminar);
            acciones.Controls.Add(nuevo);
            barra.Controls.Add(acciones);

            _gridStockResumen = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = EstiloAplicacion.Fondo, EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 40, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = true, Font = new Font("Segoe UI", 9F) };
            _gridStockResumen.SelectionChanged += (s, e) => ActualizarEstadoBotonEditarStock();
            _gridStockResumen.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            _gridStockResumen.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _gridStockResumen.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _gridStockResumen.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            _gridStockResumen.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            _gridStockResumen.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            _gridStockResumen.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            _gridStockResumen.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            _gridStockResumen.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5);
            _gridStockResumen.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            _gridStockResumen.RowTemplate.Height = 42;
            _gridStockResumen.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || _gridStockResumen.Columns[e.ColumnIndex].Name != "Estado" || e.Value == null) return;
                string estado = Convert.ToString(e.Value);
                e.CellStyle.ForeColor = estado == "Correcto" ? EstiloAplicacion.Acento : estado == "Bajo" ? Color.FromArgb(190, 127, 42) : Color.FromArgb(193, 78, 78);
            };
            Panel pie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            _totalStockResumen = new Label { Dock = DockStyle.Left, Width = 320, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = secundaria };
            FlowLayoutPanel pagina = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(0, 4, 0, 0), BackColor = Color.White };
            _siguienteStock = CrearBotonPaginacionStock("Siguiente ›"); _siguienteStock.Click += (s, e) => CambiarPaginaStock(1);
            _anteriorStock = CrearBotonPaginacionStock("‹ Anterior"); _anteriorStock.Click += (s, e) => CambiarPaginaStock(-1);
            _paginaStock = new Label { Width = 70, Height = 24, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            pagina.Controls.Add(_siguienteStock); pagina.Controls.Add(_paginaStock); pagina.Controls.Add(_anteriorStock);
            pie.Controls.Add(_totalStockResumen); pie.Controls.Add(pagina);
            tabla.Controls.Add(barra, 0, 0);
            tabla.Controls.Add(_gridStockResumen, 0, 1);
            tabla.Controls.Add(pie, 0, 2);
            lista.Controls.Add(tabla);
            RedondearPanel(lista, 14);
            _gridStockResumen.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            _gridStockResumen.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(resumen, 0, 1); pantalla.Controls.Add(lista, 0, 2);
            tabPage1.Controls.Add(pantalla);
            tabControl1.SelectedTab = tabPage1;
        }

        // CONFIGURACIÓN: PrepararDialogoStock
        private void PrepararDialogoStock()
        {
            _dialogoStock = new Form
            {
                Text = "Stock",
                ClientSize = new Size(980, 350),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FormBorderStyle = FormBorderStyle.None,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterParent
            };

            Panel contenido = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(22, 18, 22, 8) };
            _cboSucursalEditorStock = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            _cboProductoEditorStock = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            _nudCantidadEditorStock = CrearCantidadEditorStock();
            _nudMinimoEditorStock = CrearCantidadEditorStock();

            TableLayoutPanel campos = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.White,
                Margin = Padding.Empty
            };
            campos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            campos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            campos.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            campos.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            campos.Controls.Add(CrearCampoEditorStock("Sucursal *", _cboSucursalEditorStock), 0, 0);
            campos.Controls.Add(CrearCampoEditorStock("Producto *", _cboProductoEditorStock), 1, 0);
            campos.Controls.Add(CrearCampoEditorStock("Cantidad", _nudCantidadEditorStock), 0, 1);
            campos.Controls.Add(CrearCampoEditorStock("Stock mínimo", _nudMinimoEditorStock), 1, 1);

            _guardarEditorStock = CrearBotonEditorStock("Guardar", EstiloAplicacion.Acento, Color.White);
            _guardarEditorStock.Click += (s, e) => btnGuardar_Click(s, e);
            _limpiarEditorStock = CrearBotonEditorStock("Limpiar", Color.White, EstiloAplicacion.Tinta);
            _limpiarEditorStock.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 228);
            _limpiarEditorStock.Click += (s, e) => btnLimpiar_Click(s, e);
            FlowLayoutPanel acciones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(0, 6, 0, 0)
            };
            acciones.Controls.Add(_limpiarEditorStock);
            acciones.Controls.Add(_guardarEditorStock);
            contenido.Controls.Add(campos);
            contenido.Controls.Add(acciones);

            _dialogoStock.Controls.Add(contenido);
            // El encabezado y Cancelar se comparten con los otros diálogos.
        }

        // CREAR ELEMENTO VISUAL: CrearCantidadEditorStock
        private static NumericUpDown CrearCantidadEditorStock()
        {
            return new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000000,
                ThousandsSeparator = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
        }

        // CREAR ELEMENTO VISUAL: CrearCampoEditorStock
        private static Panel CrearCampoEditorStock(string titulo, Control campo)
        {
            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 14, 6) };
            TableLayoutPanel campoLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = Padding.Empty
            };
            campoLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            campoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            campoLayout.Controls.Add(new Label
            {
                Text = titulo,
                Dock = DockStyle.Fill,
                ForeColor = EstiloAplicacion.Secundario,
                Font = new Font("Segoe UI", 8.5F)
            }, 0, 0);
            campo.Dock = DockStyle.Fill;
            campo.Font = new Font("Segoe UI", 9F);
            campoLayout.Controls.Add(campo, 0, 1);
            panel.Controls.Add(campoLayout);
            return panel;
        }

        // CREAR ELEMENTO VISUAL: CrearBotonEditorStock
        private static Button CrearBotonEditorStock(string texto, Color fondo, Color tinta)
        {
            return new Button
            {
                Text = texto,
                Width = 126,
                Height = 34,
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = fondo,
                ForeColor = tinta,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9F)
            };
        }

        private void AbrirEditorStock()
        {
            btnNuevo_Click(this, EventArgs.Empty);
            _dialogoStock.Text = "Nuevo registro de stock";
            _limpiarEditorStock.Visible = true;
            _dialogoStock.DialogResult = DialogResult.None;
            EstiloAplicacion.AplicarDialogo(_dialogoStock);
            _dialogoStock.ShowDialog(this);
            MtdVolverAModoConsulta();
        }

        private void AbrirEditorStock(StockEntidad item)
        {
            _codigoSeleccionado = item.CodigoStock;
            _codigoSucursalSeleccionado = item.CodigoSucursal;
            _codigoProductoSeleccionado = item.CodigoProducto;
            _modo = "Editar";
            _cboSucursalEditorStock.SelectedValue = item.CodigoSucursal;
            _cboProductoEditorStock.SelectedValue = item.CodigoProducto;
            _nudCantidadEditorStock.Value = item.Cantidad;
            _nudMinimoEditorStock.Value = item.StockMinimo;
            MtdHabilitarCampos(true, false);
            _dialogoStock.Text = "Editar registro de stock";
            _limpiarEditorStock.Visible = false;
            _dialogoStock.DialogResult = DialogResult.None;
            EstiloAplicacion.AplicarDialogo(_dialogoStock);
            _dialogoStock.ShowDialog(this);
            MtdVolverAModoConsulta();
        }

        // EDITAR: ActualizarStockResumen
        private void ActualizarStockResumen(List<StockEntidad> lista)
        {
            _listaStockResumen = lista ?? new List<StockEntidad>();
            if (_resumenStock[0] != null)
            {
                _resumenStock[0].Text = _listaStockResumen.Count(x => x.Cantidad > x.StockMinimo).ToString("N0");
                _resumenStock[1].Text = _listaStockResumen.Count(x => x.Cantidad > 0 && x.Cantidad <= x.StockMinimo).ToString("N0");
                _resumenStock[2].Text = _listaStockResumen.Count(x => x.Cantidad <= 0).ToString("N0");
                _detalleResumenStock[0].Text = "Existencia correcta";
                _detalleResumenStock[1].Text = "Reabastecimiento sugerido";
                _detalleResumenStock[2].Text = "Sin existencias";
            }
            _paginaActualStock = 1;
            FiltrarStockResumen();
        }

        private void FiltrarStockResumen()
        {
            if (_gridStockResumen == null) return;
            string criterio = _buscarStockResumen.Text == "Buscar stock" ? "" : _buscarStockResumen.Text.Trim();
            List<StockEntidad> filtrados = String.IsNullOrEmpty(criterio)
                ? _listaStockResumen
                : _listaStockResumen.Where(x =>
                    x.CodigoStock.ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.CodigoProducto.ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.NombreProducto.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.NombreSucursal.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.Cantidad.ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.StockMinimo.ToString().IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            int paginas = Math.Max(1, (int)Math.Ceiling(filtrados.Count / (double)FilasPorPaginaStock));
            _paginaActualStock = Math.Max(1, Math.Min(_paginaActualStock, paginas));
            List<StockEntidad> paginaActual = filtrados.Skip((_paginaActualStock - 1) * FilasPorPaginaStock).Take(FilasPorPaginaStock).ToList();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CodigoStock", typeof(int));
            tabla.Columns.Add("Producto");
            tabla.Columns.Add("CodigoProducto", typeof(int));
            tabla.Columns.Add("Sucursal");
            tabla.Columns.Add("Cantidad", typeof(int));
            tabla.Columns.Add("StockMinimo", typeof(int));
            tabla.Columns.Add("Estado");
            foreach (StockEntidad item in paginaActual)
            {
                tabla.Rows.Add(item.CodigoStock, item.NombreProducto, item.CodigoProducto, item.NombreSucursal, item.Cantidad, item.StockMinimo,
                    item.Cantidad <= 0 ? "Agotado" : item.Cantidad <= item.StockMinimo ? "Bajo" : "Correcto");
            }
            _gridStockResumen.DataSource = tabla;
            _gridStockResumen.Columns["CodigoStock"].HeaderText = "CÓDIGO STOCK"; _gridStockResumen.Columns["CodigoStock"].FillWeight = 14;
            _gridStockResumen.Columns["Producto"].HeaderText = "PRODUCTO"; _gridStockResumen.Columns["Producto"].FillWeight = 27;
            _gridStockResumen.Columns["CodigoProducto"].HeaderText = "CÓDIGO PRODUCTO"; _gridStockResumen.Columns["CodigoProducto"].FillWeight = 16;
            _gridStockResumen.Columns["Sucursal"].HeaderText = "SUCURSAL"; _gridStockResumen.Columns["Sucursal"].FillWeight = 24;
            _gridStockResumen.Columns["Cantidad"].HeaderText = "CANTIDAD"; _gridStockResumen.Columns["Cantidad"].FillWeight = 12;
            _gridStockResumen.Columns["StockMinimo"].HeaderText = "STOCK MÍNIMO"; _gridStockResumen.Columns["StockMinimo"].FillWeight = 14;
            _gridStockResumen.Columns["Estado"].HeaderText = "NIVEL"; _gridStockResumen.Columns["Estado"].FillWeight = 13;
            _gridStockResumen.ClearSelection();
            foreach (DataGridViewRow fila in _gridStockResumen.Rows)
            {
                int id = Convert.ToInt32(fila.Cells["CodigoStock"].Value);
                fila.Tag = paginaActual.First(x => x.CodigoStock == id);
            }
            ActualizarEstadoBotonEditarStock();
            int desde = filtrados.Count == 0 ? 0 : ((_paginaActualStock - 1) * FilasPorPaginaStock) + 1;
            int hasta = Math.Min(_paginaActualStock * FilasPorPaginaStock, filtrados.Count);
            _totalStockResumen.Text = "Mostrando " + desde + "-" + hasta + " de " + filtrados.Count + " registros";
            _paginaStock.Text = _paginaActualStock + " de " + paginas;
            _anteriorStock.Enabled = _paginaActualStock > 1;
            _siguienteStock.Enabled = _paginaActualStock < paginas;
        }

        // CAMBIAR PÁGINA: CambiarPaginaStock
        private void CambiarPaginaStock(int cambio)
        {
            _paginaActualStock += cambio;
            FiltrarStockResumen();
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPaginacionStock
        private static Button CrearBotonPaginacionStock(string texto)
        {
            Button boton = new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento, Font = new Font("Segoe UI", 8F), Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = 1;
            boton.FlatAppearance.BorderColor = Color.FromArgb(219, 228, 232);
            boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 248, 244);
            return boton;
        }

        // ELIMINAR: EliminarStockResumen
        private void EliminarStockResumen()
        {
            if (_gridStockResumen == null || _gridStockResumen.CurrentRow == null || !_gridStockResumen.CurrentRow.Selected) { AyudaForms.MtdAviso("Selecciona un registro de stock primero."); return; }
            StockEntidad item = _gridStockResumen.CurrentRow.Tag as StockEntidad;
            if (item == null) return;
            _codigoSeleccionado = item.CodigoStock;
            btnEliminar_Click(this, EventArgs.Empty);
        }

        // EDITAR: ActualizarEstadoBotonEditarStock
        private void ActualizarEstadoBotonEditarStock()
        {
            // El botón queda disponible incluso cuando el usuario aún no eligió una fila;
            // así puede mostrar un mensaje claro en lugar de parecer deshabilitado.
            if (_editarStockResumen != null) _editarStockResumen.Enabled = true;
        }

        // EDITAR: EditarStockResumen
        private void EditarStockResumen()
        {
            if (_gridStockResumen == null)
            {
                AyudaForms.MtdAviso("Selecciona un registro de stock primero.");
                return;
            }

            DataGridViewRow filaSeleccionada = _gridStockResumen.SelectedRows.Count > 0
                ? _gridStockResumen.SelectedRows[0]
                : _gridStockResumen.CurrentRow;
            if (filaSeleccionada == null || filaSeleccionada.Cells["CodigoStock"].Value == null)
            {
                AyudaForms.MtdAviso("Haz clic en la fila del registro que deseas editar.");
                return;
            }

            int codigoStock = Convert.ToInt32(filaSeleccionada.Cells["CodigoStock"].Value);
            StockEntidad item = filaSeleccionada.Tag as StockEntidad;
            if (item == null) item = _listaStockResumen.FirstOrDefault(x => x.CodigoStock == codigoStock);
            if (item == null)
            {
                AyudaForms.MtdAviso("No se encontró el registro de stock seleccionado.");
                return;
            }

            AbrirEditorStock(item);
        }

        // CREAR ELEMENTO VISUAL: CrearBotonStock
        private static Button CrearBotonStock(string texto, bool principal)
        {
            Button boton = new Button
            {
                Text = texto,
                Width = principal ? 128 : 96,
                Height = 34,
                Margin = new Padding(8, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = principal ? EstiloAplicacion.Acento : Color.White,
                ForeColor = principal ? Color.White : Color.FromArgb(62, 81, 89),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = principal ? 0 : 1;
            boton.FlatAppearance.BorderColor = Color.FromArgb(219, 228, 232);
            boton.FlatAppearance.MouseOverBackColor = principal ? EstiloAplicacion.Acento : Color.FromArgb(245, 248, 249);
            return boton;
        }

        private static void RedondearPanel(Control control, int radio)
        {
            Action actualizarRegion = () =>
            {
                if (control.Width < 2 || control.Height < 2) return;
                int diametro = radio * 2;
                using (GraphicsPath ruta = new GraphicsPath())
                {
                    ruta.AddArc(0, 0, diametro, diametro, 180, 90);
                    ruta.AddArc(control.Width - diametro, 0, diametro, diametro, 270, 90);
                    ruta.AddArc(control.Width - diametro, control.Height - diametro, diametro, diametro, 0, 90);
                    ruta.AddArc(0, control.Height - diametro, diametro, diametro, 90, 90);
                    ruta.CloseFigure();
                    Region anterior = control.Region;
                    control.Region = new Region(ruta);
                    if (anterior != null) anterior.Dispose();
                }
            };
            control.Resize += (s, e) => actualizarRegion();
            control.HandleCreated += (s, e) => control.BeginInvoke(new MethodInvoker(() => actualizarRegion()));
            control.ParentChanged += (s, e) => { if (control.IsHandleCreated) control.BeginInvoke(new MethodInvoker(() => actualizarRegion())); };
            if (control.IsHandleCreated) control.BeginInvoke(new MethodInvoker(() => actualizarRegion()));
        }

        private void StockForms_Load(object sender, EventArgs e)
        {
            try
            {
                MtdCargarCombos();
                MtdConsultar();
                MtdLimpiarCampos();
                MtdHabilitarCampos(false, false);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // Llena los ComboBox desde la capa Negocio
        // CARGA DE DATOS: MtdCargarCombos
        private void MtdCargarCombos()
        {
            AyudaForms.MtdLlenarCombo(_cboSucursalEditorStock, _catalogosNegocio.MtdSucursales());
            AyudaForms.MtdLlenarCombo(_cboProductoEditorStock, _catalogosNegocio.MtdProductos());
        }

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        // CONSULTAR: MtdConsultar
        private void MtdConsultar()
        {
            MtdMostrarLista(_stockNegocio.MtdConsultar());
        }

        // PRESENTAR DATOS: MtdMostrarLista
        private void MtdMostrarLista(List<StockEntidad> lista)
        {
            ActualizarStockResumen(lista);
        }

        // BUSCAR: btnBuscar_Click
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                MtdMostrarLista(_stockNegocio.MtdBuscar(txtBuscar.Text));
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // LIMPIAR: btnLimpiarFiltro_Click
        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Clear();
                MtdConsultar();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void ChkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (_seleccionarTodoStock != null && _seleccionarTodoStock.Checked)
            {
                _gridStockResumen.SelectAll();
            }
            else
            {
                _gridStockResumen.ClearSelection();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdImprimir(_gridStockResumen, "Stock");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdExportarCsv(_gridStockResumen, "Stock");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // La edición por doble clic está deshabilitada; el registro se crea con Nuevo.
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intencionalmente sin acción.
        }

        // =====================================================
        // GESTIONAR: METODOS DE APOYO
        // =====================================================

        // LEER CAMPOS DEL FORMULARIO: Convierte los valores ingresados en una entidad de stock.
        private StockEntidad MtdObtenerDeCampos()
        {
            if (_modo == "Editar")
            {
                return new StockEntidad
                (
                    _codigoSeleccionado,
                    _codigoSucursalSeleccionado,
                    String.Empty,
                    _codigoProductoSeleccionado,
                    String.Empty,
                    (int)_nudCantidadEditorStock.Value,
                    (int)_nudMinimoEditorStock.Value
                );
            }
            return new StockEntidad
            (
                AyudaForms.MtdValorCombo(_cboSucursalEditorStock),
                AyudaForms.MtdValorCombo(_cboProductoEditorStock),
                (int)_nudCantidadEditorStock.Value,
                (int)_nudMinimoEditorStock.Value
            );
        }

        // LIMPIAR: MtdLimpiarCampos
        private void MtdLimpiarCampos()
        {
            if (_cboSucursalEditorStock.Items.Count > 0)
            {
                _cboSucursalEditorStock.SelectedIndex = 0;
            }
            if (_cboProductoEditorStock.Items.Count > 0)
            {
                _cboProductoEditorStock.SelectedIndex = 0;
            }
            _nudCantidadEditorStock.Value = 0;
            _nudMinimoEditorStock.Value = 0;
        }

        // habilitar: activa o desactiva los campos | esNuevo: los campos clave solo se activan al agregar
        private void MtdHabilitarCampos(bool habilitar, bool esNuevo)
        {
            _cboSucursalEditorStock.Enabled = habilitar && esNuevo;
            _cboProductoEditorStock.Enabled = habilitar && esNuevo;
            _nudCantidadEditorStock.Enabled = habilitar;
            _nudMinimoEditorStock.Enabled = habilitar;
        }

        private void MtdVolverAModoConsulta()
        {
            _codigoSeleccionado = 0;
            _codigoSucursalSeleccionado = 0;
            _codigoProductoSeleccionado = 0;
            _modo = string.Empty;
            MtdLimpiarCampos();
            MtdHabilitarCampos(false, false);
            if (_dialogoStock != null && _dialogoStock.Visible) _dialogoStock.DialogResult = DialogResult.OK;
        }

        // =====================================================
        // GESTIONAR: BOTONES
        // =====================================================

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
            _codigoSeleccionado = 0;
            _modo = "Agregar";
            MtdHabilitarCampos(true, true);
            _cboSucursalEditorStock.Focus();
        }

        // EDITAR: btnEditar_Click
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_codigoSeleccionado == 0)
            {
                AyudaForms.MtdAviso("Primero carga un registro: ve a Consultar y haz doble clic en una fila.");
                return;
            }

            _modo = "Editar";
            MtdHabilitarCampos(true, false);
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modo != "Agregar" && _modo != "Editar")
                {
                    AyudaForms.MtdAviso("Usa Nuevo o Editar para guardar un registro de stock.");
                    return;
                }

                StockEntidad entidad = MtdObtenerDeCampos();
                if (_modo == "Agregar") _stockNegocio.MtdAgregar(entidad);
                else _stockNegocio.MtdEditar(entidad);

                AyudaForms.MtdInfo(_modo == "Agregar" ? "Registro guardado correctamente." : "Registro actualizado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // ELIMINAR: btnEliminar_Click
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_codigoSeleccionado == 0)
                {
                    AyudaForms.MtdAviso("Primero carga un registro: ve a Consultar y haz doble clic en una fila.");
                    return;
                }

                if (!AyudaForms.MtdConfirmar("¿Seguro que deseas eliminar este registro?"))
                {
                    return;
                }

                _stockNegocio.MtdEliminar(_codigoSeleccionado);

                AyudaForms.MtdInfo("Registro eliminado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdVolverAModoConsulta();
            if (_dialogoStock != null && _dialogoStock.Visible) _dialogoStock.DialogResult = DialogResult.Cancel;
        }

        // LIMPIAR: btnLimpiar_Click
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnCerrarGestionar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    } //Clase
} // namespace
