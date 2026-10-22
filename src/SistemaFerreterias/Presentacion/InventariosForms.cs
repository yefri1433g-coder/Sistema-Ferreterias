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
    public partial class InventariosForms : Form
    {
        // Instanciar: Que el forms actual (capa presentacion) pueda acceder a otra capa (Negocio)
        private readonly InventariosNegocio _inventariosNegocio = new InventariosNegocio();
        private readonly CatalogosNegocio _catalogosNegocio = new CatalogosNegocio();

        // Variables de control: registro cargado en Gestionar y modo actual (Agregar / Editar)
        private int _codigoSeleccionado = 0;
        private string _modo = string.Empty;
        private readonly Label[] _resumenInventarios = new Label[3];
        private readonly StockNegocio _stockNegocio = new StockNegocio();
        private readonly CatalogosNegocio _catalogosNegocioStock = new CatalogosNegocio();
        private List<StockEntidad> _listaStock = new List<StockEntidad>();
        private DataGridView _gridExistencias;
        private TextBox _buscarExistencias;
        private Label _totalExistencias;
        private Label _paginaExistencias;
        private Button _anteriorExistencias, _siguienteExistencias;
        private int _numeroPaginaExistencias = 1;
        private const int ExistenciasPorPagina = 4;

        // La lista completa permanece en memoria. Solo la porción mostrada se pagina en Presentación.
        private List<InventariosEntidad> _listaInventarios = new List<InventariosEntidad>();
        private int _numeroPaginaInventarios = 1;
        private const int InventariosPorPagina = 10;
        private Label _paginaInventarios;
        private Button _anteriorInventarios;
        private Button _siguienteInventarios;

        public InventariosForms()
        {
            InitializeComponent();
            EstiloAplicacion.Aplicar(this);
            PrepararDisenoInventarios();
        }

        private void PrepararDisenoInventarios()
        {
            BackColor = Color.FromArgb(241, 246, 245);
            Text = "Inventarios";
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabPage1.Controls.Clear();
            TabPage registrosConteo = new TabPage("Registros de conteo") { Name = "tabRegistrosConteo", BackColor = Color.White };
            registrosConteo.Controls.Add(PanelPrincipal);
            Button volverExistencias = new Button { Text = "< Volver a existencias", Dock = DockStyle.Top, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(23, 143, 112), TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
            volverExistencias.FlatAppearance.BorderSize = 0;
            volverExistencias.Click += (s, e) => tabControl1.SelectedTab = tabPage1;
            registrosConteo.Controls.Add(volverExistencias);
            tabControl1.TabPages.Add(registrosConteo);

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = Color.FromArgb(241, 246, 245) };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245) };
            Label ruta = new Label { Text = "Inicio  /  Inventarios", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(102, 123, 131) };
            Label tituloPrincipal = new Label { Text = "Inventarios", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
            Label subtituloPrincipal = new Label { Text = "Existencias por producto y sucursal.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(102, 123, 131) };
            LinkLabel conteos = new LinkLabel { Text = "Registros de conteo", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = Color.FromArgb(23, 143, 112), Font = new Font("Segoe UI", 8.5F) };
            conteos.Location = new Point(Width - 190, 26);
            conteos.Click += (s, e) => { tabControl1.SelectedTab = registrosConteo; MtdConsultar(); };
            encabezado.Controls.Add(ruta); encabezado.Controls.Add(tituloPrincipal); encabezado.Controls.Add(subtituloPrincipal); encabezado.Controls.Add(conteos);

            Panel resumen = new Panel { Name = "panelResumenInventarios", Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245), Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.FromArgb(241, 246, 245) };
            string[] titulos = { "ARTÍCULOS", "UNIDADES EN EXISTENCIA", "ALERTAS DE STOCK" };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "↗" : i == 1 ? "◷" : "!", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : Color.FromArgb(35, 184, 139), BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : Color.FromArgb(237, 249, 246) };
                Label titulo = new Label { Text = titulos[i], Left = 14, Top = 42, Height = 18, AutoEllipsis = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(113, 134, 141) };
                _resumenInventarios[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(_resumenInventarios[i]); tarjeta.Controls.Add(titulo); tarjetas.Controls.Add(tarjeta, i, 0);
            }
            resumen.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            Label tituloLista = new Label { Text = "Inventarios recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
            Label descripcionLista = new Label { Text = "Existencias registradas por producto y sucursal", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(123, 139, 147) };
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 405, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonInventario("+ Nuevo registro", true); nuevo.Click += (s, e) => AbrirEditorStock(null);
            Button eliminarInventarioVista = CrearBotonInventario("Eliminar", false); eliminarInventarioVista.Width = 82; eliminarInventarioVista.Click += (s, e) => EliminarStockSeleccionado();
            _buscarExistencias = new TextBox { Width = 165, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar inventarios", ForeColor = Color.FromArgb(135, 151, 156) };
            _buscarExistencias.Enter += (s, e) => { if (_buscarExistencias.Text == "Buscar inventarios") { _buscarExistencias.Clear(); _buscarExistencias.ForeColor = Color.FromArgb(45, 64, 70); } };
            _buscarExistencias.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarExistencias.Text)) { _buscarExistencias.Text = "Buscar inventarios"; _buscarExistencias.ForeColor = Color.FromArgb(135, 151, 156); } };
            _buscarExistencias.TextChanged += (s, e) => { if (_buscarExistencias.Text != "Buscar inventarios") BuscarExistencias(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(eliminarInventarioVista); acciones.Controls.Add(_buscarExistencias);
            barra.Controls.Add(tituloLista); barra.Controls.Add(descripcionLista); barra.Controls.Add(acciones);
            _gridExistencias = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            eliminarInventarioVista.Enabled = false;
            _gridExistencias.SelectionChanged += (s, e) => eliminarInventarioVista.Enabled = _gridExistencias.CurrentRow != null;
            _gridExistencias.ColumnHeadersDefaultCellStyle.BackColor = Color.White; _gridExistencias.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(102, 123, 131); _gridExistencias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold); _gridExistencias.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White; _gridExistencias.DefaultCellStyle.ForeColor = Color.FromArgb(50, 69, 77); _gridExistencias.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 246, 241); _gridExistencias.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 59, 56); _gridExistencias.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridExistencias.RowTemplate.Height = 42;
            _gridExistencias.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridExistencias.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridExistencias.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AbrirEditorStock((StockEntidad)_gridExistencias.Rows[e.RowIndex].Tag); };
            _gridExistencias.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || _gridExistencias.Columns[e.ColumnIndex].Name != "Estado" || e.Value == null) return;
                e.CellStyle.ForeColor = Convert.ToString(e.Value) == "Stock bajo" ? Color.FromArgb(210, 126, 48) : Color.FromArgb(22, 151, 113);
            };
            ContextMenuStrip menuFila = new ContextMenuStrip();
            menuFila.Items.Add("Editar registro", null, (s, e) => { if (_gridExistencias.CurrentRow != null) AbrirEditorStock((StockEntidad)_gridExistencias.CurrentRow.Tag); });
            menuFila.Items.Add("Eliminar registro", null, (s, e) => EliminarStockSeleccionado());
            _gridExistencias.ContextMenuStrip = menuFila;
            _gridExistencias.CellMouseDown += (s, e) => { if (e.Button == MouseButtons.Right && e.RowIndex >= 0) _gridExistencias.CurrentCell = _gridExistencias.Rows[e.RowIndex].Cells[0]; };
            _totalExistencias = new Label { Dock = DockStyle.Left, Width = 250, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(119, 136, 145) };
            _paginaExistencias = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(92, 111, 119) };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel controlesPagina = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteExistencias = new Button { Text = "Siguiente >", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(37, 120, 105) };
            _anteriorExistencias = new Button { Text = "< Anterior", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(37, 120, 105) };
            _siguienteExistencias.Click += (s, e) => CambiarPaginaExistencias(1);
            _anteriorExistencias.Click += (s, e) => CambiarPaginaExistencias(-1);
            controlesPagina.Controls.Add(_siguienteExistencias); controlesPagina.Controls.Add(_paginaExistencias); controlesPagina.Controls.Add(_anteriorExistencias);
            pie.Controls.Add(_totalExistencias); pie.Controls.Add(controlesPagina);
            lista.Controls.Add(_gridExistencias); lista.Controls.Add(pie); lista.Controls.Add(barra);
            RedondearPanel(lista, 14);
            for (int i = 0; i < tarjetas.Controls.Count; i++) RedondearPanel(tarjetas.Controls[i], 10);
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(resumen, 0, 1); pantalla.Controls.Add(lista, 0, 2);
            tabPage1.Controls.Add(pantalla);
            tabControl1.SelectedIndexChanged += (s, e) =>
            {
                if (tabControl1.SelectedTab == tabPage1) MtdConsultarExistencias();
                else if (tabControl1.SelectedTab == tabPage2) { MtdCargarCombos(); MtdLimpiarCampos(); MtdHabilitarCampos(false, false); }
            };
        }

        private void InventariosForms_Load(object sender, EventArgs e)
        {
            try
            {
                MtdPrepararPaginacionInventarios();
                MtdCargarCombos();
                MtdConsultarExistencias();
                MtdLimpiarCampos();
                MtdHabilitarCampos(false, false);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // Llena los ComboBox desde la capa Negocio
        private void MtdCargarCombos()
        {
            AyudaForms.MtdLlenarCombo(cboSucursal, _catalogosNegocio.MtdSucursales());
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
            control.HandleCreated += (s, e) => actualizarRegion();
        }

        private static Button CrearBotonInventario(string texto, bool principal)
        {
            Button boton = new Button { Text = texto, Width = principal ? 128 : 74, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = principal ? Color.FromArgb(23, 143, 112) : Color.White, ForeColor = principal ? Color.White : Color.FromArgb(50, 69, 77), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = principal ? 0 : 1;
            boton.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230);
            return boton;
        }

        private void MtdConsultarExistencias()
        {
            _numeroPaginaExistencias = 1;
            MostrarExistencias(_stockNegocio.MtdConsultar());
        }

        private void BuscarExistencias()
        {
            try
            {
                MostrarExistencias(_stockNegocio.MtdBuscar(_buscarExistencias.Text));
            }
            catch (Exception ex) { AyudaForms.MtdError(ex); }
        }

        private void MostrarExistencias(List<StockEntidad> lista)
        {
            _listaStock = lista ?? new List<StockEntidad>();
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(_listaStock.Count / (double)ExistenciasPorPagina));
            _numeroPaginaExistencias = Math.Max(1, Math.Min(_numeroPaginaExistencias, totalPaginas));
            List<StockEntidad> pagina = _listaStock.Skip((_numeroPaginaExistencias - 1) * ExistenciasPorPagina).Take(ExistenciasPorPagina).ToList();

            DataTable tabla = new DataTable();
            tabla.Columns.Add("CodigoStock", typeof(int)); tabla.Columns.Add("CodigoSucursal", typeof(int)); tabla.Columns.Add("CodigoProducto", typeof(int));
            tabla.Columns.Add("Producto"); tabla.Columns.Add("Sucursal"); tabla.Columns.Add("Existencia", typeof(int)); tabla.Columns.Add("Minimo", typeof(int)); tabla.Columns.Add("Estado");
            foreach (StockEntidad item in pagina)
                tabla.Rows.Add(item.CodigoStock, item.CodigoSucursal, item.CodigoProducto, item.NombreProducto, item.NombreSucursal, item.Cantidad, item.StockMinimo, item.Cantidad <= item.StockMinimo ? "Stock bajo" : "Disponible");
            _gridExistencias.DataSource = tabla;
            _gridExistencias.Columns["CodigoStock"].Visible = false; _gridExistencias.Columns["CodigoSucursal"].Visible = false; _gridExistencias.Columns["CodigoProducto"].Visible = false;
            _gridExistencias.Columns["Producto"].HeaderText = "PRODUCTO"; _gridExistencias.Columns["Producto"].FillWeight = 30;
            _gridExistencias.Columns["Sucursal"].HeaderText = "SUCURSAL"; _gridExistencias.Columns["Sucursal"].FillWeight = 22;
            _gridExistencias.Columns["Existencia"].HeaderText = "EXISTENCIA"; _gridExistencias.Columns["Existencia"].FillWeight = 15;
            _gridExistencias.Columns["Minimo"].HeaderText = "M\u00cdNIMO"; _gridExistencias.Columns["Minimo"].FillWeight = 15;
            _gridExistencias.Columns["Estado"].HeaderText = "ESTADO"; _gridExistencias.Columns["Estado"].FillWeight = 18;
            _gridExistencias.ClearSelection();

            int desde = _listaStock.Count == 0 ? 0 : ((_numeroPaginaExistencias - 1) * ExistenciasPorPagina) + 1;
            int hasta = Math.Min(_numeroPaginaExistencias * ExistenciasPorPagina, _listaStock.Count);
            _totalExistencias.Text = "Mostrando " + desde + "-" + hasta + " de " + _listaStock.Count + " registros";
            _paginaExistencias.Text = _numeroPaginaExistencias + " de " + totalPaginas;
            _anteriorExistencias.Enabled = _numeroPaginaExistencias > 1;
            _siguienteExistencias.Enabled = _numeroPaginaExistencias < totalPaginas;
            _resumenInventarios[0].Text = _listaStock.Select(x => x.CodigoProducto).Distinct().Count().ToString("N0");
            _resumenInventarios[1].Text = _listaStock.Sum(x => x.Cantidad).ToString("N0");
            _resumenInventarios[2].Text = _listaStock.Count(x => x.Cantidad <= x.StockMinimo).ToString("N0");
            for (int i = 0; i < _gridExistencias.Rows.Count; i++)
            {
                int codigo = Convert.ToInt32(_gridExistencias.Rows[i].Cells["CodigoStock"].Value);
                _gridExistencias.Rows[i].Tag = pagina.FirstOrDefault(x => x.CodigoStock == codigo);
            }
        }

        private void CambiarPaginaExistencias(int cambio)
        {
            _numeroPaginaExistencias += cambio;
            if (String.IsNullOrWhiteSpace(_buscarExistencias.Text) || _buscarExistencias.Text == "Buscar inventarios")
                MostrarExistencias(_stockNegocio.MtdConsultar());
            else
                MostrarExistencias(_stockNegocio.MtdBuscar(_buscarExistencias.Text));
        }
        private void AbrirEditorStock(StockEntidad existente)
        {
            Form dialogo = new Form { Text = existente == null ? "Nuevo registro de inventario" : "Editar inventario", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(470, 350), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            Label titulo = new Label { Text = existente == null ? "Nuevo registro" : "Editar registro", Left = 22, Top = 18, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
            Label lSucursal = new Label { Text = "Sucursal", Left = 24, Top = 70, Width = 190 };
            ComboBox sucursal = new ComboBox { Left = 24, Top = 92, Width = 410, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Texto", ValueMember = "Codigo", DataSource = _catalogosNegocioStock.MtdSucursales() };
            Label lProducto = new Label { Text = "Producto", Left = 24, Top = 135, Width = 190 };
            ComboBox producto = new ComboBox { Left = 24, Top = 157, Width = 410, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Texto", ValueMember = "Codigo", DataSource = _catalogosNegocioStock.MtdProductos() };
            Label lCantidad = new Label { Text = "Existencia", Left = 24, Top = 200, Width = 190 };
            NumericUpDown cantidad = new NumericUpDown { Left = 24, Top = 222, Width = 190, Maximum = 100000000, ThousandsSeparator = true };
            Label lMinimo = new Label { Text = "Stock mínimo", Left = 244, Top = 200, Width = 190 };
            NumericUpDown minimo = new NumericUpDown { Left = 244, Top = 222, Width = 190, Maximum = 100000000, ThousandsSeparator = true };
            Button guardar = CrearBotonInventario("Guardar", true); guardar.Left = 244; guardar.Top = 280; guardar.Width = 90; guardar.DialogResult = DialogResult.None;
            Button cancelar = CrearBotonInventario("Cancelar", false); cancelar.Left = 344; cancelar.Top = 280; cancelar.Width = 90; cancelar.DialogResult = DialogResult.Cancel;
            dialogo.CancelButton = cancelar;
            if (existente != null) { sucursal.SelectedValue = existente.CodigoSucursal; producto.SelectedValue = existente.CodigoProducto; cantidad.Value = existente.Cantidad; minimo.Value = existente.StockMinimo; }
            guardar.Click += (s, e) =>
            {
                try
                {
                    StockEntidad entidad = existente == null
                        ? new StockEntidad(Convert.ToInt32(sucursal.SelectedValue), Convert.ToInt32(producto.SelectedValue), (int)cantidad.Value, (int)minimo.Value)
                        : new StockEntidad(existente.CodigoStock, Convert.ToInt32(sucursal.SelectedValue), existente.NombreSucursal, Convert.ToInt32(producto.SelectedValue), existente.NombreProducto, (int)cantidad.Value, (int)minimo.Value);
                    if (existente == null) _stockNegocio.MtdAgregar(entidad); else _stockNegocio.MtdEditar(entidad);
                    MtdConsultarExistencias(); dialogo.DialogResult = DialogResult.OK; dialogo.Close();
                }
                catch (Exception ex) { AyudaForms.MtdError(ex); }
            };
            titulo.Visible = false;
            dialogo.Controls.AddRange(new Control[] { titulo, lSucursal, sucursal, lProducto, producto, lCantidad, cantidad, lMinimo, minimo, guardar, cancelar });
            EstiloAplicacion.AplicarDialogo(dialogo);
            dialogo.ShowDialog(this);
        }

        private void EliminarStockSeleccionado()
        {
            if (_gridExistencias.CurrentRow == null || !(_gridExistencias.CurrentRow.Tag is StockEntidad)) { AyudaForms.MtdAviso("Selecciona un registro de inventario."); return; }
            StockEntidad item = (StockEntidad)_gridExistencias.CurrentRow.Tag;
            try
            {
                if (!AyudaForms.MtdConfirmar("¿Seguro que deseas eliminar este registro de inventario?")) return;
                _stockNegocio.MtdEliminar(item.CodigoStock);
                MtdConsultarExistencias();
            }
            catch (Exception ex) { AyudaForms.MtdError(ex); }
        }

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        private void MtdConsultar()
        {
            _numeroPaginaInventarios = 1;
            MtdMostrarLista(_inventariosNegocio.MtdConsultar());
        }

        private void MtdMostrarLista(List<InventariosEntidad> lista)
        {
            // La paginación se realiza aquí, en Presentación; Datos y Negocio entregan la lista completa.
            _listaInventarios = lista ?? new List<InventariosEntidad>();
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(_listaInventarios.Count / (double)InventariosPorPagina));
            _numeroPaginaInventarios = Math.Max(1, Math.Min(_numeroPaginaInventarios, totalPaginas));
            List<InventariosEntidad> pagina = _listaInventarios
                .Skip((_numeroPaginaInventarios - 1) * InventariosPorPagina)
                .Take(InventariosPorPagina)
                .ToList();

            dgvInventarios.DataSource = null;
            dgvInventarios.DataSource = pagina;

            // Encabezados con nombres amigables y columnas ocultas
            AyudaForms.MtdTitulo(dgvInventarios, "CodigoInventario", "Código");
            AyudaForms.MtdTitulo(dgvInventarios, "NombreSucursal", "Sucursal");
            AyudaForms.MtdTitulo(dgvInventarios, "FechaInventario", "Fecha");
            AyudaForms.MtdTitulo(dgvInventarios, "Descripcion", "Descripción");
            AyudaForms.MtdOcultar(dgvInventarios, "CodigoSucursal");
            AyudaForms.MtdFormato(dgvInventarios, "FechaInventario", "dd/MM/yyyy");

            // El total siempre usa la lista completa o filtrada, nunca solo los diez de la página.
            lblTotalRegistros.Text = "Total Registros: " + _listaInventarios.Count;
            _paginaInventarios.Text = "Página " + _numeroPaginaInventarios + " de " + totalPaginas;
            _anteriorInventarios.Enabled = _numeroPaginaInventarios > 1;
            _siguienteInventarios.Enabled = _numeroPaginaInventarios < totalPaginas;
        }

        private void MtdPrepararPaginacionInventarios()
        {
            // Se agregan al panel inferior existente para no editar el archivo Designer.cs.
            Panel panelPaginacion = new Panel { Dock = DockStyle.Right, Width = 250 };
            _anteriorInventarios = new Button { Text = "<", Left = 4, Top = 10, Width = 36, Height = 30 };
            _paginaInventarios = new Label { Left = 48, Top = 16, Width = 140, TextAlign = ContentAlignment.MiddleCenter, Text = "Página 1 de 1" };
            _siguienteInventarios = new Button { Text = ">", Left = 196, Top = 10, Width = 36, Height = 30 };

            _anteriorInventarios.Click += (s, e) => MtdCambiarPaginaInventarios(-1);
            _siguienteInventarios.Click += (s, e) => MtdCambiarPaginaInventarios(1);
            panelPaginacion.Controls.AddRange(new Control[] { _anteriorInventarios, _paginaInventarios, _siguienteInventarios });
            panelInferior.Controls.Add(panelPaginacion);
        }

        private void MtdCambiarPaginaInventarios(int cambio)
        {
            _numeroPaginaInventarios += cambio;
            MtdMostrarLista(_listaInventarios);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                _numeroPaginaInventarios = 1;
                MtdMostrarLista(_inventariosNegocio.MtdBuscar(txtBuscar.Text));
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

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
            if (ChkSeleccionar.Checked)
            {
                dgvInventarios.SelectAll();
            }
            else
            {
                dgvInventarios.ClearSelection();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdImprimir(dgvInventarios, "Inventarios");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdExportarCsv(dgvInventarios, "Inventarios");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Doble clic en una fila: la carga en la pestaña Gestionar
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            InventariosEntidad item = dgvInventarios.Rows[e.RowIndex].DataBoundItem as InventariosEntidad;

            if (item != null)
            {
                MtdCargarEnCampos(item);
                tabControl1.SelectedTab = tabPage2;
            }
        }

        // =====================================================
        // GESTIONAR: METODOS DE APOYO
        // =====================================================

        private void MtdCargarEnCampos(InventariosEntidad item)
        {
            _codigoSeleccionado = item.CodigoInventario;
            _modo = string.Empty;

            // El ComboBox debe tener su catálogo antes de asignar la FK con SelectedValue.
            if (cboSucursal.DataSource == null)
            {
                MtdCargarCombos();
            }

            txtCodigo.Text = item.CodigoInventario.ToString();
            cboSucursal.SelectedValue = item.CodigoSucursal;
            dtpFecha.Value = item.FechaInventario;
            txtDescripcion.Text = item.Descripcion;

            MtdHabilitarCampos(false, false);
        }

        private InventariosEntidad MtdObtenerDeCampos()
        {
            if (_modo == "Agregar")
            {
                return new InventariosEntidad
                (
                    AyudaForms.MtdValorCombo(cboSucursal),
                    dtpFecha.Value.Date,
                    txtDescripcion.Text.Trim()
                );
            }

            return new InventariosEntidad
                (
                    _codigoSeleccionado,
                    AyudaForms.MtdValorCombo(cboSucursal),
                    string.Empty,
                    dtpFecha.Value.Date,
                    txtDescripcion.Text.Trim()
                );
        }

        private void MtdLimpiarCampos()
        {
            txtCodigo.Clear();
            if (cboSucursal.Items.Count > 0)
            {
                cboSucursal.SelectedIndex = 0;
            }
            dtpFecha.Value = DateTime.Now;
            txtDescripcion.Clear();
        }

        // habilitar: activa o desactiva los campos | esNuevo: los campos clave solo se activan al agregar
        private void MtdHabilitarCampos(bool habilitar, bool esNuevo)
        {
            cboSucursal.Enabled = habilitar;
            dtpFecha.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
        }

        private void MtdVolverAModoConsulta()
        {
            _codigoSeleccionado = 0;
            _modo = string.Empty;
            MtdLimpiarCampos();
            MtdHabilitarCampos(false, false);
        }

        // =====================================================
        // GESTIONAR: BOTONES
        // =====================================================

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
            _codigoSeleccionado = 0;
            _modo = "Agregar";
            MtdHabilitarCampos(true, true);
            cboSucursal.Focus();
        }

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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modo == string.Empty)
                {
                    AyudaForms.MtdAviso("Presiona Nuevo o Editar antes de guardar.");
                    return;
                }

                InventariosEntidad entidad = MtdObtenerDeCampos();

                if (_modo == "Agregar")
                {
                    _inventariosNegocio.MtdAgregar(entidad);
                }
                else
                {
                    _inventariosNegocio.MtdEditar(entidad);
                }

                AyudaForms.MtdInfo("Registro guardado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

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

                _inventariosNegocio.MtdEliminar(_codigoSeleccionado);

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
        }

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

