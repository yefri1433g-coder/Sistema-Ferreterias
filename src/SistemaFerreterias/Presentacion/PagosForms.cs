using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Entidad;
using Negocio;

namespace Presentacion
{
    // CLASE PagosForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class PagosForms : Form
    {
        private readonly PagoNegocio _negocio = new PagoNegocio();
        private decimal _saldo;
        private bool _cargando;
        private readonly Label[] _resumenPagos = new Label[3];
        private DataGridView _gridHistorial;
        private TextBox _buscarPagos;
        private Label _textoRegistros, _textoPagina;
        private Button _anterior, _siguiente;
        private Form _dialogoPago;
        private DataTable _pagos = new DataTable();
        private DataTable _ventas = new DataTable();
        private DataTable _metodosPago;
        private int? _idPagoEdicion;
        private int _idVentaOriginalEdicion;
        private decimal _montoOriginalEdicion;
        private DateTime _fechaPagoEdicion;
        private int _pagina = 1;
        private const int PagosPorPagina = 10;

        public PagosForms()
        {
            InitializeComponent();
            PrepararDisenoPagos();
            BarraListado.Configurar(_gridHistorial, _buscarPagos, "Pagos", () => _gridHistorial_CellDoubleClick(_gridHistorial, new DataGridViewCellEventArgs(0, _gridHistorial.CurrentRow.Index)), () => EliminarPagoSeleccionado(_gridHistorial.CurrentRow));
            EstiloAplicacion.Aplicar(this);
        }

        private void PagosForms_Load(object sender, EventArgs e)
        {
            try
            {
                CargarMetodosPago();
                CargarVentas(null);
                CargarHistorial();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdMensaje("No se pudieron cargar los catálogos de Pagos. " + ex.Message,
                    "Pagos");
            }
        }

        /// <summary>
        /// Carga los métodos permitidos en el esquema actual, que guarda el método como texto.
        /// Se enlazan código y descripción para mantener un uso uniforme de los ComboBox.
        /// </summary>
        // CARGA DE DATOS: CargarMetodosPago
        private void CargarMetodosPago()
        {
            DataTable metodos = new DataTable();
            metodos.Columns.Add("Valor", typeof(string));
            metodos.Columns.Add("Descripcion", typeof(string));
            metodos.Rows.Add("Efectivo", "Efectivo");
            metodos.Rows.Add("Tarjeta de Crédito", "Tarjeta de Crédito");
            metodos.Rows.Add("Tarjeta de Débito", "Tarjeta de Débito");
            metodos.Rows.Add("Tarjeta", "Tarjeta");
            metodos.Rows.Add("Transferencia", "Transferencia");
            _metodosPago = metodos;

            cmbMetodo.DataSource = null;
            cmbMetodo.DisplayMember = "Descripcion";
            cmbMetodo.ValueMember = "Valor";
            cmbMetodo.DataSource = metodos;
            cmbMetodo.SelectedIndex = metodos.Rows.Count > 0 ? 0 : -1;
        }

        // CONFIGURACIÓN: PrepararDisenoPagos
        private void PrepararDisenoPagos()
        {
            Control[] controlesEditor = Controls.Cast<Control>().ToArray();
            Controls.Clear();
            _dialogoPago = new Form { Text = "Registrar pago", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(920, 620), BackColor = EstiloAplicacion.Fondo, Font = new Font("Segoe UI", 9F) };
            _dialogoPago.Controls.AddRange(controlesEditor);
            ConfigurarEditorPagoCompacto();
            EstiloAplicacion.AplicarDialogo(_dialogoPago);
            _dialogoPago.FormClosed += (s, e) =>
            {
                _idPagoEdicion = null;
                _idVentaOriginalEdicion = 0;
                _montoOriginalEdicion = 0m;
                btnRegistrar.Text = "Pagar";
                CargarHistorial();
            };

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(22, 18, 22, 18), BackColor = EstiloAplicacion.Fondo, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18, 10, 18, 8) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Pagos", Location = new Point(18, 6), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            encabezado.Controls.Add(new Label { Text = "Pagos", Location = new Point(18, 25), AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label { Text = "Pagos recibidos y saldos pendientes.", Location = new Point(18, 66), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = EstiloAplicacion.Secundario });

            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(0, 8, 0, 4), BackColor = EstiloAplicacion.Fondo };
            for (int i = 0; i < 3; i++) { tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F)); Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 1 ? 8 : 0, 0, i == 2 ? 0 : 8, 0), Padding = new Padding(18, 12, 12, 8) }; Label nombre = new Label { Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario }; _resumenPagos[i] = new Label { Dock = DockStyle.Top, Height = 43, Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta }; nombre.Text = i == 0 ? "PAGOS DE HOY" : i == 1 ? "COBRADO HOY" : "PENDIENTE"; tarjeta.Controls.Add(_resumenPagos[i]); tarjeta.Controls.Add(nombre); tarjetas.Controls.Add(tarjeta, i, 0); }

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Pagos recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            barra.Controls.Add(new Label { Text = "Historial · doble clic edita · Supr elimina", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 350, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonPago("+ Nuevo registro", true); nuevo.Click += (s, e) => { btnNuevo_Click(s, e); _dialogoPago.Text = "Registrar pago"; _dialogoPago.ShowDialog(this); };
            _buscarPagos = new TextBox { Width = 190, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle };
            _buscarPagos.TextChanged += (s, e) => { _pagina = 1; MostrarHistorialPagos(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(_buscarPagos); barra.Controls.Add(acciones);
            _gridHistorial = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridHistorial.CellClick += _gridHistorial_CellClick;
            _gridHistorial.CellDoubleClick += _gridHistorial_CellDoubleClick;
            _gridHistorial.KeyDown += _gridHistorial_KeyDown;
            _gridHistorial.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino; _gridHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; _gridHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); _gridHistorial.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino; _gridHistorial.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta; _gridHistorial.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion; _gridHistorial.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta; _gridHistorial.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridHistorial.RowTemplate.Height = 42;
            _gridHistorial.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _textoRegistros = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            _textoPagina = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel paginacion = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguiente = CrearBotonPaginacionPago("Siguiente >"); _anterior = CrearBotonPaginacionPago("< Anterior"); _siguiente.Click += (s, e) => CambiarPagina(1); _anterior.Click += (s, e) => CambiarPagina(-1);
            paginacion.Controls.Add(_siguiente); paginacion.Controls.Add(_textoPagina); paginacion.Controls.Add(_anterior); pie.Controls.Add(_textoRegistros); pie.Controls.Add(paginacion);
            lista.Controls.Add(_gridHistorial); lista.Controls.Add(pie); lista.Controls.Add(barra);
            layout.Controls.Add(encabezado, 0, 0); layout.Controls.Add(tarjetas, 0, 1); layout.Controls.Add(lista, 0, 2); Controls.Add(layout);
            Text = "Pagos"; BackColor = EstiloAplicacion.Fondo; MinimumSize = new Size(980, 620);
        }

        // CONFIGURACIÓN: ConfigurarEditorPagoCompacto
        private void ConfigurarEditorPagoCompacto()
        {
            foreach (Control control in _dialogoPago.Controls) control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pictureBox1.Visible = false;
            lblTitulo.Text = "Registrar pago"; lblTitulo.Location = new Point(24, 18); lblTitulo.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblSubtitulo.Text = "Selecciona una venta y registra el abono."; lblSubtitulo.Location = new Point(26, 57); lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblTitulo.Visible = false; lblSubtitulo.Visible = false;
            foreach (Label etiqueta in new[] { lblVenta, lblTotalTitulo, lblTotalValor, lblPagadoTitulo, lblPagadoValor, lblSaldoTitulo, lblSaldoValor, lblMonto, lblMetodo, lblHistorial })
                etiqueta.AutoSize = false;
            lblVenta.SetBounds(24, 24, 840, 22); cmbVenta.SetBounds(24, 52, 840, 30);
            lblTotalTitulo.SetBounds(24, 100, 240, 22); lblTotalValor.SetBounds(24, 128, 240, 30);
            lblPagadoTitulo.SetBounds(310, 100, 240, 22); lblPagadoValor.SetBounds(310, 128, 240, 30);
            lblSaldoTitulo.SetBounds(596, 100, 268, 22); lblSaldoValor.SetBounds(596, 128, 268, 30);
            lblMonto.SetBounds(24, 184, 250, 22); nudMonto.SetBounds(24, 212, 250, 30);
            lblMetodo.SetBounds(300, 184, 350, 22); cmbMetodo.SetBounds(300, 212, 350, 30);
            btnRegistrar.SetBounds(680, 207, 184, 38);
            lblHistorial.SetBounds(24, 274, 840, 22); dgvPagos.SetBounds(24, 304, 840, 220);
            btnNuevo.Visible = false; btnCerrar.Text = "Cancelar"; btnCerrar.SetBounds(744, 535, 120, 38);
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPago
        private static Button CrearBotonPago(string texto, bool primario)
        {
            return new Button { Text = texto, Width = primario ? 132 : 100, Height = 30, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = primario ? EstiloAplicacion.Acento : Color.White, ForeColor = primario ? Color.White : EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPaginacionPago
        private static Button CrearBotonPaginacionPago(string texto)
        {
            return new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 3, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F) };
        }

        // CARGA DE DATOS: CargarHistorial
        private void CargarHistorial()
        {
            if (_gridHistorial == null) return;
            try { _pagos = _negocio.MtdListarPagosRecientes(); _ventas = _negocio.MtdListarVentas(); MostrarHistorialPagos(); }
            catch (Exception ex) { AyudaForms.MtdMensaje("No se pudo cargar el historial de pagos. " + ex.Message, "Pagos"); }
        }

        // PRESENTAR DATOS: MostrarHistorialPagos
        private void MostrarHistorialPagos()
        {
            if (_gridHistorial == null) return;
            string criterio = _buscarPagos.Text.Trim();
            var filtrados = _pagos.AsEnumerable().Where(r =>
                String.IsNullOrWhiteSpace(criterio) ||
                Convert.ToString(r["id_pago"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                Convert.ToString(r["id_venta"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                Convert.ToString(r["monto"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                Convert.ToString(r["metodo_pago"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                Convert.ToString(r["cliente"]).IndexOf(criterio, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            int paginas = Math.Max(1, (int)Math.Ceiling(filtrados.Count / (double)PagosPorPagina)); _pagina = Math.Max(1, Math.Min(_pagina, paginas));
            DataTable pagina = _pagos.Clone(); foreach (DataRow fila in filtrados.Skip((_pagina - 1) * PagosPorPagina).Take(PagosPorPagina)) pagina.ImportRow(fila);
            _gridHistorial.DataSource = pagina;
            _gridHistorial.Columns["id_pago"].HeaderText = "PAGO"; _gridHistorial.Columns["id_pago"].FillWeight = 14;
            _gridHistorial.Columns["cliente"].HeaderText = "CLIENTE"; _gridHistorial.Columns["cliente"].FillWeight = 27;
            _gridHistorial.Columns["metodo_pago"].HeaderText = "MÉTODO"; _gridHistorial.Columns["metodo_pago"].FillWeight = 20;
            _gridHistorial.Columns["fecha"].HeaderText = "FECHA"; _gridHistorial.Columns["fecha"].DefaultCellStyle.Format = "dd MMM yyyy"; _gridHistorial.Columns["fecha"].FillWeight = 20;
            _gridHistorial.Columns["monto"].HeaderText = "IMPORTE"; _gridHistorial.Columns["monto"].DefaultCellStyle.Format = "'Q '#,##0.00"; _gridHistorial.Columns["monto"].FillWeight = 19;
            _gridHistorial.Columns["id_venta"].Visible = false; _gridHistorial.ClearSelection();
            int desde = filtrados.Count == 0 ? 0 : ((_pagina - 1) * PagosPorPagina) + 1, hasta = Math.Min(_pagina * PagosPorPagina, filtrados.Count);
            _textoRegistros.Text = "Mostrando " + desde + "-" + hasta + " de " + filtrados.Count + " registros"; _textoPagina.Text = _pagina + " de " + paginas;
            _anterior.Enabled = _pagina > 1; _siguiente.Enabled = _pagina < paginas;
            var hoy = _pagos.AsEnumerable().Where(r => r.Field<DateTime>("fecha").Date == DateTime.Today).ToList();
            _resumenPagos[0].Text = hoy.Count.ToString("N0"); _resumenPagos[1].Text = "Q " + hoy.Sum(r => r.Field<decimal>("monto")).ToString("N2");
            decimal pendiente = _ventas.AsEnumerable().Sum(r => Math.Max(0m, r.Field<decimal>("total") - r.Field<decimal>("pagado")));
            _resumenPagos[2].Text = "Q " + pendiente.ToString("N2");
        }

        // CAMBIAR PÁGINA: CambiarPagina
        private void CambiarPagina(int cambio) { _pagina += cambio; MostrarHistorialPagos(); }

        // Al seleccionar una fila, prepara sus datos en los controles existentes del editor.
        private void _gridHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _gridHistorial.Rows.Count) return;
            CargarPagoEnEditor(_gridHistorial.Rows[e.RowIndex]);
        }

        // El doble clic abre el editor ya cargado para el pago seleccionado.
        private void _gridHistorial_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _gridHistorial.Rows.Count) return;

            CargarPagoEnEditor(_gridHistorial.Rows[e.RowIndex]);
            if (!_idPagoEdicion.HasValue) return;

            _dialogoPago.Text = "Editar pago";
            _dialogoPago.ShowDialog(this);
        }

        // La tecla Supr elimina el pago de la fila seleccionada después de pedir confirmación.
        private void _gridHistorial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || _gridHistorial.CurrentRow == null) return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            EliminarPagoSeleccionado(_gridHistorial.CurrentRow);
        }

        // CARGA DE DATOS: CargarPagoEnEditor
        private void CargarPagoEnEditor(DataGridViewRow fila)
        {
            try
            {
                _idPagoEdicion = Convert.ToInt32(fila.Cells["id_pago"].Value);
                _idVentaOriginalEdicion = Convert.ToInt32(fila.Cells["id_venta"].Value);
                _montoOriginalEdicion = Convert.ToDecimal(fila.Cells["monto"].Value);
                _fechaPagoEdicion = Convert.ToDateTime(fila.Cells["fecha"].Value);

                // Reutiliza el catálogo de ventas y la FK id_venta como valor seleccionado.
                CargarVentas(_idVentaOriginalEdicion);
                cmbVenta.SelectedValue = _idVentaOriginalEdicion;

                string metodo = Convert.ToString(fila.Cells["metodo_pago"].Value);
                AgregarMetodoSiHaceFalta(metodo);
                cmbMetodo.SelectedValue = metodo;
                nudMonto.Value = Math.Min(_montoOriginalEdicion, nudMonto.Maximum);
                btnRegistrar.Text = "Guardar cambios";
            }
            catch (Exception ex)
            {
                _idPagoEdicion = null;
                AyudaForms.MtdMensaje("No se pudieron cargar los datos del pago seleccionado. " + ex.Message,
                    "Pagos");
            }
        }

        // AGREGAR: AgregarMetodoSiHaceFalta
        private void AgregarMetodoSiHaceFalta(string metodo)
        {
            if (_metodosPago == null || String.IsNullOrWhiteSpace(metodo)) return;

            bool existe = _metodosPago.AsEnumerable().Any(r =>
                string.Equals(Convert.ToString(r["Valor"]), metodo, StringComparison.OrdinalIgnoreCase));
            if (!existe)
                _metodosPago.Rows.Add(metodo, metodo);
        }

        // ELIMINAR: EliminarPagoSeleccionado
        private void EliminarPagoSeleccionado(DataGridViewRow fila)
        {
            if (fila == null || fila.Cells["id_pago"].Value == null) return;

            int idPago = Convert.ToInt32(fila.Cells["id_pago"].Value);
            DialogResult confirmacion = AyudaForms.MtdConfirmacion(
                "¿Deseas eliminar el pago No. " + idPago + "? Esta acción no se puede deshacer.");
            if (confirmacion != DialogResult.Yes) return;

            string mensaje;
            bool eliminado = _negocio.MtdEliminar(idPago, out mensaje);
            if (eliminado) AyudaForms.MtdInfo("Pago eliminado correctamente.");
            else AyudaForms.MtdAviso(mensaje);

            if (eliminado)
            {
                _idPagoEdicion = null;
                CargarHistorial();
            }
        }

        // CARGA DE DATOS: CargarVentas
        private void CargarVentas(int? idSeleccionar)
        {
            _cargando = true;
            try
            {
                DataTable dt = _negocio.MtdListarVentas();
                dt.Columns.Add("texto", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    r["texto"] = string.Format("Venta #{0} - {1} - {2:dd/MM/yyyy} - Total Q {3:N2}",
                        r["id_venta"], r["cliente"], r["fecha"], r["total"]);
                }

                cmbVenta.DataSource = null;
                cmbVenta.DisplayMember = "texto";
                cmbVenta.ValueMember = "id_venta";
                cmbVenta.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    if (idSeleccionar.HasValue)
                        cmbVenta.SelectedValue = idSeleccionar.Value;
                    else
                    {
                        // Al abrir Pagos, prioriza una venta pendiente para habilitar el registro.
                        int indicePendiente = -1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            decimal total = Convert.ToDecimal(dt.Rows[i]["total"]);
                            decimal pagado = Convert.ToDecimal(dt.Rows[i]["pagado"]);
                            if (total > pagado)
                            {
                                indicePendiente = i;
                                break;
                            }
                        }

                        cmbVenta.SelectedIndex = indicePendiente >= 0 ? indicePendiente : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                AyudaForms.MtdMensaje("No se pudieron cargar las ventas: " + ex.Message, "Pagos");
            }
            finally
            {
                _cargando = false;
            }

            MostrarResumen();
        }

        // PRESENTAR DATOS: MostrarResumen
        private void MostrarResumen()
        {
            DataRowView fila = cmbVenta.SelectedItem as DataRowView;
            if (fila == null)
            {
                LimpiarResumen();
                return;
            }

            int idVenta = Convert.ToInt32(fila["id_venta"]);
            decimal total = Convert.ToDecimal(fila["total"]);
            decimal pagado = Convert.ToDecimal(fila["pagado"]);
            _saldo = total - pagado;

            lblTotalValor.Text = "Q " + total.ToString("N2");
            lblPagadoValor.Text = "Q " + pagado.ToString("N2");
            lblSaldoValor.Text = _saldo > 0 ? "Q " + _saldo.ToString("N2") : "Q 0.00 (pagada)";

            try
            {
                // Consulta los pagos de la venta seleccionada y muestra un mensaje claro si falla SQL.
                dgvPagos.DataSource = _negocio.MtdListarPagos(idVenta);
                if (dgvPagos.Columns.Contains("Monto"))
                    dgvPagos.Columns["Monto"].DefaultCellStyle.Format = "N2";
            }
            catch (Exception ex)
            {
                dgvPagos.DataSource = null;
                AyudaForms.MtdMensaje("No se pudieron consultar los pagos de esta venta. " + ex.Message,
                    "Pagos");
            }

            bool pendiente = _saldo > 0;
            bool enEdicion = _idPagoEdicion.HasValue;
            decimal montoMaximo = pendiente ? _saldo : 0m;

            // Al editar, permite conservar el pago actual y valida el máximo en Negocio.
            if (enEdicion)
            {
                montoMaximo = _saldo;
                if (idVenta == _idVentaOriginalEdicion)
                    montoMaximo += _montoOriginalEdicion;
            }

            bool controlesPagoActivos = pendiente || (enEdicion && montoMaximo > 0);
            nudMonto.Enabled = controlesPagoActivos;
            cmbMetodo.Enabled = controlesPagoActivos;
            btnRegistrar.Enabled = controlesPagoActivos;

            nudMonto.Maximum = Math.Max(0m, montoMaximo);
            decimal montoInicial = enEdicion ? _montoOriginalEdicion : (pendiente ? _saldo : 0m);
            nudMonto.Value = Math.Min(montoInicial, nudMonto.Maximum);
        }

        // LIMPIAR: LimpiarResumen
        private void LimpiarResumen()
        {
            _saldo = 0;
            lblTotalValor.Text = "Q 0.00";
            lblPagadoValor.Text = "Q 0.00";
            lblSaldoValor.Text = "Q 0.00";
            dgvPagos.DataSource = null;

            nudMonto.Maximum = 0;
            nudMonto.Value = 0;
            nudMonto.Enabled = false;
            cmbMetodo.Enabled = false;
            btnRegistrar.Enabled = false;
        }

        private void cmbVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargando) return;
            MostrarResumen();
        }

        // GUARDAR: btnRegistrar_Click
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbVenta.SelectedValue == null || cmbMetodo.SelectedValue == null)
            {
                AyudaForms.MtdMensaje("Seleccione una venta y un método de pago.", "Pagos");
                return;
            }

            int idVenta = Convert.ToInt32(cmbVenta.SelectedValue);

            Pago pago = new Pago
            {
                IdPago = _idPagoEdicion.HasValue ? _idPagoEdicion.Value : 0,
                IdVenta = idVenta,
                Fecha = _idPagoEdicion.HasValue ? _fechaPagoEdicion : DateTime.Now,
                Monto = nudMonto.Value,
                MetodoPago = Convert.ToString(cmbMetodo.SelectedValue)
            };

            string mensaje;
            bool ok = _idPagoEdicion.HasValue
                ? _negocio.MtdEditar(pago, out mensaje)
                : _negocio.MtdRegistrar(pago, out mensaje);

            if (ok) AyudaForms.MtdInfo(_idPagoEdicion.HasValue ? "Pago actualizado correctamente." : "Pago registrado correctamente.");
            else AyudaForms.MtdAviso(mensaje);

            if (ok && _idPagoEdicion.HasValue)
            {
                _idPagoEdicion = null;
                _dialogoPago.Close();
            }
            else if (ok)
                CargarVentas(idVenta);
        }

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _idPagoEdicion = null;
            _idVentaOriginalEdicion = 0;
            _montoOriginalEdicion = 0m;
            _fechaPagoEdicion = DateTime.Now;
            btnRegistrar.Text = "Pagar";
            _dialogoPago.Text = "Registrar pago";
            cmbMetodo.SelectedIndex = 0;
            CargarVentas(null);

            // Buscar la primera venta con saldo pendiente
            for (int i = 0; i < cmbVenta.Items.Count; i++)
            {
                DataRowView fila = (DataRowView)cmbVenta.Items[i];
                decimal saldo = Convert.ToDecimal(fila["total"]) - Convert.ToDecimal(fila["pagado"]);

                if (saldo > 0)
                {
                    cmbVenta.SelectedIndex = i;
                    return;
                }
            }

            AyudaForms.MtdMensaje("No hay ventas con saldo pendiente.", "Pagos");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Form contenedor = ((Control)sender).FindForm();
            if (contenedor != null && contenedor != this)
            {
                _idPagoEdicion = null;
                _idVentaOriginalEdicion = 0;
                _montoOriginalEdicion = 0m;
                btnRegistrar.Text = "Pagar";
                contenedor.Close();
            }
        }
    }
}
