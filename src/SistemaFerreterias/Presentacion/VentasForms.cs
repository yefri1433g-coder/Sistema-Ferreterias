using Entidad;
using Negocio;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE VentasForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class VentasForms : Form
    {
        private readonly VentaNegocio _negocio = new VentaNegocio();
        private readonly ClienteNegocio _clientes = new ClienteNegocio();
        private readonly BindingList<DetalleVenta> _detalle = new BindingList<DetalleVenta>();
        private readonly Label[] _resumenVentas = new Label[3];
        private DataGridView _gridVentas;
        private TextBox _buscarVentas;
        private Label _textoRegistros, _textoPagina;
        private Button _anterior, _siguiente, _editarVenta, _eliminarVenta;
        private Form _dialogoVenta;
        private DataTable _ventas = new DataTable();
        private Venta _ventaEnEdicion;
        private int _pagina = 1;
        private const int VentasPorPagina = 10;
        private bool _cargando = true;

        public VentasForms()
        {
            InitializeComponent();
            PrepararDisenoVentas();
            BarraListado.Configurar(_gridVentas, _buscarVentas, "Ventas", mostrarEdicion: false);
            EstiloAplicacion.Aplicar(this);
        }

        private void VentasForms_Load(object sender, EventArgs e)
        {
            cboPago.Items.Add("Efectivo");
            cboPago.Items.Add("Transferencia");
            cboPago.Items.Add("Tarjeta de Crédito");
            cboPago.Items.Add("Tarjeta de Débito");
            cboPago.Items.Add("Pendiente");

            cboSucursal.DisplayMember = "Texto";
            cboSucursal.ValueMember = "Id";
            cboSucursal.DataSource = _negocio.ListarSucursales();

            cboCliente.DisplayMember = "Nombre";
            cboCliente.ValueMember = "IdCliente";
            cboCliente.DataSource = _clientes.Listar();

            dgvDetalle.DataSource = _detalle;
            ConfigurarGrid();

            _cargando = false;
            CargarProductos();
            Nuevo();
            CargarVentas();
        }

        // CONFIGURACIÓN: PrepararDisenoVentas
        private void PrepararDisenoVentas()
        {
            Control[] controlesEditor = Controls.Cast<Control>().ToArray();
            Controls.Clear();
            _dialogoVenta = new Form { Text = "Nueva venta", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(920, 620), BackColor = EstiloAplicacion.Fondo, Font = new Font("Segoe UI", 9F) };
            _dialogoVenta.Controls.AddRange(controlesEditor);
            ConfigurarEditorVentaCompacto();
            EstiloAplicacion.AplicarDialogo(_dialogoVenta);
            _dialogoVenta.FormClosed += (s, e) => CargarVentas();

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(22, 18, 22, 18), BackColor = EstiloAplicacion.Fondo, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18, 10, 18, 8) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Ventas", Location = new Point(18, 6), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            encabezado.Controls.Add(new Label { Text = "Ventas", Location = new Point(18, 25), AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label { Text = "Seguimiento de ventas, clientes y montos.", Location = new Point(18, 66), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = EstiloAplicacion.Secundario });

            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(0, 8, 0, 4), BackColor = EstiloAplicacion.Fondo };
            for (int i = 0; i < 3; i++) { tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F)); Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 1 ? 8 : 0, 0, i == 2 ? 0 : 8, 0), Padding = new Padding(18, 12, 12, 8) }; Label nombre = new Label { Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario }; _resumenVentas[i] = new Label { Dock = DockStyle.Top, Height = 43, Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta }; nombre.Text = i == 0 ? "VENTAS DE HOY" : i == 1 ? "TOTAL DEL DÍA" : "TICKET PROMEDIO"; tarjeta.Controls.Add(_resumenVentas[i]); tarjeta.Controls.Add(nombre); tarjetas.Controls.Add(tarjeta, i, 0); }

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Ventas recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            barra.Controls.Add(new Label { Text = "Historial de ventas y clientes", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 530, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonVenta("+ Nuevo registro", true); nuevo.Click += (s, e) => { Nuevo(); _dialogoVenta.Text = "Nueva venta"; _dialogoVenta.ShowDialog(this); };
            _editarVenta = CrearBotonVenta("Editar", false); _editarVenta.Width = 78; _editarVenta.Click += (s, e) => EditarVentaSeleccionada(); _editarVenta.Enabled = false;
            _eliminarVenta = CrearBotonVenta("Eliminar", false); _eliminarVenta.Width = 82; _eliminarVenta.Click += (s, e) => EliminarVentaSeleccionada(); _eliminarVenta.Enabled = false;
            _buscarVentas = new TextBox { Width = 190, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle };
            _buscarVentas.TextChanged += (s, e) => { _pagina = 1; MostrarVentas(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(_editarVenta); acciones.Controls.Add(_eliminarVenta); acciones.Controls.Add(_buscarVentas); barra.Controls.Add(acciones);
            _gridVentas = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridVentas.SelectionChanged += (s, e) => ActualizarAccionesVentaSeleccionada();
            _gridVentas.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ConsultarVentaSeleccionada(); };
            _gridVentas.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino; _gridVentas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; _gridVentas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); _gridVentas.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino; _gridVentas.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta; _gridVentas.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion; _gridVentas.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta; _gridVentas.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridVentas.RowTemplate.Height = 42;
            _gridVentas.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridVentas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _textoRegistros = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            _textoPagina = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel paginacion = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguiente = CrearBotonPaginacion("Siguiente >"); _anterior = CrearBotonPaginacion("< Anterior"); _siguiente.Click += (s, e) => CambiarPagina(1); _anterior.Click += (s, e) => CambiarPagina(-1);
            paginacion.Controls.Add(_siguiente); paginacion.Controls.Add(_textoPagina); paginacion.Controls.Add(_anterior); pie.Controls.Add(_textoRegistros); pie.Controls.Add(paginacion);
            lista.Controls.Add(_gridVentas); lista.Controls.Add(pie); lista.Controls.Add(barra);
            layout.Controls.Add(encabezado, 0, 0); layout.Controls.Add(tarjetas, 0, 1); layout.Controls.Add(lista, 0, 2); Controls.Add(layout);
            Text = "Ventas"; BackColor = EstiloAplicacion.Fondo; MinimumSize = new Size(980, 620);
        }

        // CONFIGURACIÓN: ConfigurarEditorVentaCompacto
        private void ConfigurarEditorVentaCompacto()
        {
            foreach (Control control in _dialogoVenta.Controls) control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pictureBox1.Visible = false;
            lblTitulo.Text = "Nueva venta"; lblTitulo.Location = new Point(24, 18); lblTitulo.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblSubtitulo.Text = "Selecciona cliente, sucursal y productos."; lblSubtitulo.Location = new Point(26, 57); lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblTitulo.Visible = false; lblSubtitulo.Visible = false;
            lblSucursal.SetBounds(24, 98, 90, 22); cboSucursal.SetBounds(24, 122, 410, 30);
            lblCliente.SetBounds(454, 98, 90, 22); cboCliente.SetBounds(454, 122, 410, 30);
            lblProducto.SetBounds(24, 166, 90, 22); cboProducto.SetBounds(24, 190, 460, 30);
            lblCantidad.SetBounds(504, 166, 90, 22); nudCantidad.SetBounds(504, 190, 90, 30);
            lblPago.SetBounds(612, 166, 100, 22); cboPago.SetBounds(612, 190, 160, 30);
            btnAgregar.SetBounds(786, 188, 78, 34);
            dgvDetalle.SetBounds(24, 244, 840, 220);
            btnQuitar.SetBounds(24, 476, 145, 36);
            lblTotal.SetBounds(584, 478, 82, 34); lblTotalValor.SetBounds(668, 478, 190, 34);
            btnNuevo.SetBounds(24, 548, 130, 40); btnGuardar.SetBounds(166, 548, 175, 40); btnCerrar.SetBounds(354, 548, 120, 40);
        }

        // CREAR ELEMENTO VISUAL: CrearBotonVenta
        private static Button CrearBotonVenta(string texto, bool primario)
        {
            return new Button { Text = texto, Width = primario ? 132 : 100, Height = 30, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = primario ? EstiloAplicacion.Acento : Color.White, ForeColor = primario ? Color.White : EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPaginacion
        private static Button CrearBotonPaginacion(string texto)
        {
            return new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 3, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F) };
        }

        // CARGA DE DATOS: CargarVentas
        private void CargarVentas()
        {
            try { _ventas = _negocio.ListarVentas(); MostrarVentas(); }
            catch (Exception ex) { MessageBox.Show("No se pudo cargar el historial de ventas. " + ex.Message, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // PRESENTAR DATOS: MostrarVentas
        private void MostrarVentas()
        {
            if (_gridVentas == null) return;
            string criterio = (_buscarVentas.Text ?? string.Empty).Trim();
            var filtradas = _ventas.AsEnumerable().Where(r =>
                string.IsNullOrEmpty(criterio) ||
                Convert.ToString(r["id_venta"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                Convert.ToString(r["cliente"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            int totalRegistros = filtradas.Count;
            int paginas = Math.Max(1, (int)Math.Ceiling(totalRegistros / (double)VentasPorPagina));
            _pagina = Math.Max(1, Math.Min(_pagina, paginas));
            DataTable pagina = _ventas.Clone();
            foreach (DataRow fila in filtradas.Skip((_pagina - 1) * VentasPorPagina).Take(VentasPorPagina))
                pagina.ImportRow(fila);
            _pagina = Math.Max(1, Math.Min(_pagina, paginas));
            _gridVentas.DataSource = pagina;
            _gridVentas.Columns["id_venta"].HeaderText = "VENTA"; _gridVentas.Columns["id_venta"].FillWeight = 18;
            _gridVentas.Columns["cliente"].HeaderText = "CLIENTE"; _gridVentas.Columns["cliente"].FillWeight = 28;
            _gridVentas.Columns["sucursal"].HeaderText = "SUCURSAL"; _gridVentas.Columns["sucursal"].FillWeight = 24;
            _gridVentas.Columns["id_sucursal"].Visible = false;
            _gridVentas.Columns["fecha"].HeaderText = "FECHA"; _gridVentas.Columns["fecha"].DefaultCellStyle.Format = "dd MMM yyyy"; _gridVentas.Columns["fecha"].FillWeight = 20;
            _gridVentas.Columns["total"].HeaderText = "TOTAL";
            _gridVentas.Columns["total"].DefaultCellStyle.Format = "Q #,##0.00";
            _gridVentas.Columns["total"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
            _gridVentas.Columns["total"].FillWeight = 18;
            _gridVentas.Columns["pagado"].Visible = false;
            _gridVentas.ClearSelection();
            ActualizarAccionesVentaSeleccionada();
            int desde = totalRegistros == 0 ? 0 : ((_pagina - 1) * VentasPorPagina) + 1, hasta = Math.Min(_pagina * VentasPorPagina, totalRegistros);
            _textoRegistros.Text = "Mostrando " + desde + "-" + hasta + " de " + totalRegistros + " registros"; _textoPagina.Text = _pagina + " de " + paginas;
            _anterior.Enabled = _pagina > 1; _siguiente.Enabled = _pagina < paginas;
            var hoy = _ventas.AsEnumerable().Where(r => r.Field<DateTime>("fecha").Date == DateTime.Today).ToList(); decimal totalHoy = hoy.Sum(r => r.Field<decimal>("total"));
            _resumenVentas[0].Text = hoy.Count.ToString("N0"); _resumenVentas[1].Text = "Q " + totalHoy.ToString("N2"); _resumenVentas[2].Text = (hoy.Count == 0 ? 0 : totalHoy / hoy.Count).ToString("Q #,##0.00", CultureInfo.GetCultureInfo("es-GT"));
        }

        // CAMBIAR PÁGINA: CambiarPagina
        private void CambiarPagina(int cambio) { _pagina += cambio; MostrarVentas(); }

        // CONSULTAR: ConsultarVentaSeleccionada
        private void ConsultarVentaSeleccionada()
        {
            bool pendiente;
            int idVenta = VentaSeleccionadaId(out pendiente);
            if (idVenta <= 0) return;

            try
            {
                Venta venta = _negocio.Consultar(idVenta);
                if (venta == null)
                {
                    MessageBox.Show("No se encontró la venta seleccionada.", "Consulta de ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ConfigurarModoConsulta(venta);
                _dialogoVenta.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo consultar la venta. " + ex.Message, "Consulta de ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConfigurarModoCaptura();
            }
        }

        // CONFIGURACIÓN: ConfigurarModoConsulta
        private void ConfigurarModoConsulta(Venta venta)
        {
            _dialogoVenta.Text = "Consulta de venta No. " + venta.IdVenta;
            lblTitulo.Text = "Venta No. " + venta.IdVenta;
            lblSubtitulo.Text = "Fecha: " + venta.Fecha.ToString("dd/MM/yyyy HH:mm");
            lblTitulo.Visible = true;
            lblSubtitulo.Visible = true;
            lblTitulo.SetBounds(24, 18, 500, 32);
            lblSubtitulo.SetBounds(26, 57, 500, 22);

            bool tieneSucursal = venta.IdSucursal > 0;
            lblSucursal.Visible = tieneSucursal; cboSucursal.Visible = tieneSucursal;
            if (tieneSucursal)
            {
                lblSucursal.Text = "Sucursal";
                lblSucursal.SetBounds(454, 98, 100, 22);
                cboSucursal.SetBounds(454, 122, 410, 30);
                cboSucursal.Enabled = false;
                cboSucursal.SelectedValue = venta.IdSucursal;
            }
            lblCliente.Visible = true; cboCliente.Visible = true; cboCliente.Enabled = false;
            lblCliente.SetBounds(24, 98, 90, 22); cboCliente.SetBounds(24, 122, 410, 30);
            cboCliente.SelectedValue = venta.IdCliente;
            lblProducto.Visible = false; cboProducto.Visible = false;
            lblCantidad.Visible = false; nudCantidad.Visible = false;
            btnAgregar.Visible = false;
            lblPago.Visible = false; cboPago.Visible = false;

            dgvDetalle.DataSource = new BindingList<DetalleVenta>(venta.Detalles);
            dgvDetalle.ReadOnly = true;
            dgvDetalle.SetBounds(24, 174, 840, 290);
            ConfigurarGrid();
            btnQuitar.Visible = false;
            lblTotal.Visible = true; lblTotalValor.Visible = true;
            lblTotal.SetBounds(650, 478, 82, 34); lblTotalValor.SetBounds(735, 478, 125, 34);
            lblTotalValor.Text = "Q " + venta.Total.ToString("N2");
            btnNuevo.Visible = false; btnGuardar.Visible = false;
            btnCerrar.Visible = true; btnCerrar.Text = "Cerrar"; btnCerrar.SetBounds(24, 548, 120, 40);
        }

        // CONFIGURACIÓN: ConfigurarModoCaptura
        private void ConfigurarModoCaptura()
        {
            ConfigurarEditorVentaCompacto();
            _dialogoVenta.Text = _ventaEnEdicion == null ? "Nueva venta" : "Editar venta No. " + _ventaEnEdicion.IdVenta;
            lblTitulo.Visible = false; lblSubtitulo.Visible = false;
            lblSucursal.Visible = true; cboSucursal.Visible = true;
            lblCliente.Visible = true; cboCliente.Visible = true; cboCliente.Enabled = true;
            lblProducto.Visible = true; cboProducto.Visible = true;
            lblCantidad.Visible = true; nudCantidad.Visible = true;
            nudCantidad.Enabled = true;
            btnAgregar.Visible = true;
            lblPago.Visible = true; cboPago.Visible = true;
            cboPago.Enabled = _ventaEnEdicion == null;
            dgvDetalle.DataSource = _detalle;
            dgvDetalle.ReadOnly = false;
            dgvDetalle.SetBounds(24, 244, 840, 220);
            btnQuitar.Visible = true;
            lblTotal.Visible = true; lblTotalValor.Visible = true;
            lblTotal.SetBounds(584, 478, 82, 34); lblTotalValor.SetBounds(668, 478, 190, 34);
            btnNuevo.Visible = true; btnGuardar.Visible = true;
            btnGuardar.Text = _ventaEnEdicion == null ? "Guardar venta" : "Actualizar venta";
            btnAgregar.Text = _ventaEnEdicion == null ? "Agregar" : "Aplicar";
            lblSucursal.Text = _ventaEnEdicion == null ? "Sucursal" : "Sucursal original";
            if (_ventaEnEdicion != null) lblSucursal.Width = 150;
            btnCerrar.Visible = true; btnCerrar.Text = "Cerrar"; btnCerrar.SetBounds(354, 548, 120, 40);
            ConfigurarGrid();
            ActualizarTotal();
        }

        // EDITAR: ActualizarAccionesVentaSeleccionada
        private void ActualizarAccionesVentaSeleccionada()
        {
            DataRowView fila = _gridVentas == null || _gridVentas.SelectedRows.Count == 0
                ? null
                : _gridVentas.SelectedRows[0].DataBoundItem as DataRowView;
            bool pendiente = fila != null && fila.Row.Table.Columns.Contains("pagado") && Convert.ToDecimal(fila["pagado"]) == 0m;
            if (_editarVenta != null) _editarVenta.Enabled = pendiente;
            if (_eliminarVenta != null) _eliminarVenta.Enabled = pendiente;
        }

        private int VentaSeleccionadaId(out bool pendiente)
        {
            pendiente = false;
            DataRowView fila = _gridVentas.SelectedRows.Count == 0 ? null : _gridVentas.SelectedRows[0].DataBoundItem as DataRowView;
            if (fila == null) return 0;
            pendiente = fila.Row.Table.Columns.Contains("pagado") && Convert.ToDecimal(fila["pagado"]) == 0m;
            return Convert.ToInt32(fila["id_venta"]);
        }

        // EDITAR: EditarVentaSeleccionada
        private void EditarVentaSeleccionada()
        {
            bool pendiente;
            int idVenta = VentaSeleccionadaId(out pendiente);
            if (idVenta <= 0) return;
            if (!pendiente)
            {
                MessageBox.Show("Solo se pueden editar ventas pendientes, sin pagos registrados.", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Venta venta = _negocio.Obtener(idVenta);
                if (venta == null) throw new InvalidOperationException("No se encontró la venta seleccionada.");
                Nuevo();
                _ventaEnEdicion = venta;
                _dialogoVenta.Text = "Editar venta No. " + idVenta;
                lblSucursal.Text = "Sucursal original";
                lblSucursal.Width = 150;
                btnGuardar.Text = "Actualizar venta";
                btnAgregar.Text = "Aplicar";
                cboSucursal.SelectedIndex = -1;
                if (venta.IdSucursal > 0)
                    cboSucursal.SelectedValue = venta.IdSucursal;
                cboCliente.SelectedValue = venta.IdCliente;
                cboPago.SelectedItem = "Pendiente";
                cboPago.Enabled = false;
                foreach (DetalleVenta detalle in venta.Detalles)
                    _detalle.Add(new DetalleVenta { IdProducto = detalle.IdProducto, Codigo = detalle.Codigo, Producto = detalle.Producto, Cantidad = detalle.Cantidad, PrecioUnitario = detalle.PrecioUnitario });
                ActualizarTotal();
                _dialogoVenta.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la venta. " + ex.Message, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ELIMINAR: EliminarVentaSeleccionada
        private void EliminarVentaSeleccionada()
        {
            bool pendiente;
            int idVenta = VentaSeleccionadaId(out pendiente);
            if (idVenta <= 0) return;
            if (!pendiente)
            {
                MessageBox.Show("Solo se pueden eliminar ventas pendientes, sin pagos registrados.", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idSucursal = SucursalSeleccionadaId();
                if (idSucursal <= 0) idSucursal = ElegirSucursalOriginal();
                if (idSucursal <= 0) return;
                DialogResult confirmacion = MessageBox.Show(
                    "Se restaurará el inventario de la sucursal original registrada y se eliminarán los detalles de la venta No. " + idVenta + ". ¿Deseas continuar?",
                    "Eliminar venta pendiente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmacion != DialogResult.Yes) return;

                string error = _negocio.EliminarPendiente(idVenta, idSucursal);
                if (error != "")
                {
                    MessageBox.Show(error, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar la venta. " + ex.Message, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int SucursalSeleccionadaId()
        {
            DataRowView fila = _gridVentas == null || _gridVentas.SelectedRows.Count == 0
                ? null
                : _gridVentas.SelectedRows[0].DataBoundItem as DataRowView;
            if (fila == null || !fila.Row.Table.Columns.Contains("id_sucursal") || fila["id_sucursal"] == DBNull.Value)
                return 0;
            return Convert.ToInt32(fila["id_sucursal"]);
        }

        private int ElegirSucursalOriginal()
        {
            using (Form dialogo = new Form { Text = "Sucursal original", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MinimizeBox = false, MaximizeBox = false, ShowInTaskbar = false, ClientSize = new Size(390, 150) })
            {
                Label instruccion = new Label { Left = 18, Top = 14, Width = 350, Height = 38, Text = "Este registro no tiene sucursal guardada. Selecciona dónde se registró para restaurar el inventario." };
                ComboBox sucursal = new ComboBox { Left = 18, Top = 58, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Texto", ValueMember = "Id", DataSource = _negocio.ListarSucursales() };
                Button aceptar = new Button { Text = "Continuar", Left = 202, Top = 103, Width = 80, DialogResult = DialogResult.OK };
                Button cancelar = new Button { Text = "Cancelar", Left = 288, Top = 103, Width = 80, DialogResult = DialogResult.Cancel };
                dialogo.Controls.AddRange(new Control[] { instruccion, sucursal, aceptar, cancelar });
                dialogo.AcceptButton = aceptar; dialogo.CancelButton = cancelar;
                return dialogo.ShowDialog(this) == DialogResult.OK && sucursal.SelectedValue != null ? Convert.ToInt32(sucursal.SelectedValue) : 0;
            }
        }

        // CONFIGURACIÓN: ConfigurarGrid
        private void ConfigurarGrid()
        {
            if (dgvDetalle.Columns["IdProducto"] != null)
                dgvDetalle.Columns["IdProducto"].Visible = false;
            if (dgvDetalle.Columns["Codigo"] != null)
                dgvDetalle.Columns["Codigo"].HeaderText = "Código";
            if (dgvDetalle.Columns["Producto"] != null)
                dgvDetalle.Columns["Producto"].HeaderText = "Producto";
            if (dgvDetalle.Columns["Cantidad"] != null)
                dgvDetalle.Columns["Cantidad"].HeaderText = "Cantidad";
            if (dgvDetalle.Columns["PrecioUnitario"] != null)
            {
                dgvDetalle.Columns["PrecioUnitario"].HeaderText = "Precio";
                dgvDetalle.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            }
            if (dgvDetalle.Columns["Subtotal"] != null)
            {
                dgvDetalle.Columns["Subtotal"].HeaderText = "Subtotal";
                dgvDetalle.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
            }
        }

        // CARGA DE DATOS: CargarProductos
        private void CargarProductos()
        {
            ItemCombo sucursal = cboSucursal.SelectedItem as ItemCombo;

            cboProducto.DataSource = null;
            if (sucursal == null) return;

            cboProducto.DisplayMember = "Texto";
            cboProducto.ValueMember = "IdProducto";
            var productos = _negocio.ListarProductos(sucursal.Id);
            if (_ventaEnEdicion != null)
                foreach (ProductoVenta producto in productos)
                {
                    DetalleVenta anterior = _ventaEnEdicion.Detalles.FirstOrDefault(d => d.IdProducto == producto.IdProducto);
                    if (anterior != null) producto.Existencia += anterior.Cantidad;
                }
            cboProducto.DataSource = productos;
            ActualizarTotal();
        }

        // EDITAR: ActualizarTotal
        private void ActualizarTotal()
        {
            decimal total = _detalle.Sum(d => d.Subtotal);
            lblTotalValor.Text = "Q " + total.ToString("N2");

            // No se puede cambiar de sucursal mientras haya productos en la venta
            cboSucursal.Enabled = _detalle.Count == 0 || (_ventaEnEdicion != null && cboSucursal.SelectedIndex < 0);
        }

        // AGREGAR: Nuevo
        private void Nuevo()
        {
            _ventaEnEdicion = null;
            if (_dialogoVenta != null) _dialogoVenta.Text = "Nueva venta";
            lblSucursal.Text = "Sucursal";
            lblSucursal.Width = 90;
            btnGuardar.Text = "Guardar venta";
            btnAgregar.Text = "Agregar";
            cboPago.Enabled = true;
            _detalle.Clear();
            cboPago.SelectedIndex = -1;
            nudCantidad.Value = 1;
            ActualizarTotal();
        }

        private void cboSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando) return;
            CargarProductos();
        }

        // AGREGAR: btnAgregar_Click
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ProductoVenta p = cboProducto.SelectedItem as ProductoVenta;

            if (p == null)
            {
                MessageBox.Show("Selecciona un producto.", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cantidad = (int)nudCantidad.Value;
            DetalleVenta existente = _detalle.FirstOrDefault(d => d.IdProducto == p.IdProducto);
            int yaAgregado = existente == null ? 0 : existente.Cantidad;
            int cantidadFinal = existente != null && _ventaEnEdicion != null ? cantidad : yaAgregado + cantidad;

            if (cantidadFinal > p.Existencia)
            {
                MessageBox.Show(
                    "Stock insuficiente. Disponible para esta venta: " + p.Existencia + ".",
                    "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (existente != null)
            {
                int indice = _detalle.IndexOf(existente);
                existente.Cantidad = _ventaEnEdicion != null ? cantidad : cantidadFinal;
                _detalle.ResetItem(indice);
            }
            else
            {
                _detalle.Add(new DetalleVenta
                {
                    IdProducto = p.IdProducto,
                    Codigo = p.Codigo,
                    Producto = p.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = p.PrecioVenta
                });
            }

            nudCantidad.Value = 1;
            ActualizarTotal();
        }

        // ELIMINAR: btnQuitar_Click
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.CurrentRow == null) return;

            DetalleVenta d = dgvDetalle.CurrentRow.DataBoundItem as DetalleVenta;
            if (d == null) return;

            _detalle.Remove(d);
            ActualizarTotal();
        }

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Cliente cliente = cboCliente.SelectedItem as Cliente;
            ItemCombo sucursal = cboSucursal.SelectedItem as ItemCombo;

            Venta v = new Venta
            {
                IdVenta = _ventaEnEdicion == null ? 0 : _ventaEnEdicion.IdVenta,
                IdCliente = cliente == null ? 0 : cliente.IdCliente,
                IdSucursal = sucursal == null ? 0 : sucursal.Id,
                MetodoPago = _ventaEnEdicion == null ? cboPago.Text : "Pendiente",
                Detalles = _detalle.ToList()
            };

            string error = _ventaEnEdicion == null ? _negocio.Guardar(v) : _negocio.ActualizarPendiente(v);

            if (error != "")
            {
                MessageBox.Show(error, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string estadoPago = string.Equals(v.MetodoPago, "Pendiente", StringComparison.OrdinalIgnoreCase)
                ? "\nSaldo pendiente disponible en Pagos."
                : "";

            AyudaForms.MtdInfo(
                (_ventaEnEdicion == null ? "Venta No. " + v.IdVenta + " guardada." : "Venta No. " + v.IdVenta + " actualizada.") + "\nTotal: Q " + v.Total.ToString("N2") + estadoPago);

            bool estabaEditando = _ventaEnEdicion != null;
            Nuevo();
            CargarProductos();   // refresca las existencias
            if (estabaEditando) _dialogoVenta.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Form contenedor = ((Control)sender).FindForm();
            if (contenedor != null && contenedor != this) contenedor.Close();
        }
    }
}
