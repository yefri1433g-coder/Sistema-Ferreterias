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
        private int _pagina = 1;
        private const int PagosPorPagina = 4;

        public PagosForms()
        {
            InitializeComponent();
            PrepararDisenoPagos();
            EstiloAplicacion.Aplicar(this);
        }

        private void PagosForms_Load(object sender, EventArgs e)
        {
            cmbMetodo.Items.Clear();
            cmbMetodo.Items.AddRange(new object[] { "Efectivo", "Tarjeta", "Transferencia" });
            cmbMetodo.SelectedIndex = 0;

            CargarVentas(null);
            CargarHistorial();
        }

        private void PrepararDisenoPagos()
        {
            Control[] controlesEditor = Controls.Cast<Control>().ToArray();
            Controls.Clear();
            _dialogoPago = new Form { Text = "Registrar pago", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(920, 620), BackColor = Color.FromArgb(241, 246, 253), Font = new Font("Segoe UI", 9F) };
            _dialogoPago.Controls.AddRange(controlesEditor);
            ConfigurarEditorPagoCompacto();
            EstiloAplicacion.AplicarDialogo(_dialogoPago);
            _dialogoPago.FormClosed += (s, e) => CargarHistorial();

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(22, 18, 22, 18), BackColor = Color.FromArgb(241, 246, 246), ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18, 10, 18, 8) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Pagos", Location = new Point(18, 6), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(123, 139, 147) });
            encabezado.Controls.Add(new Label { Text = "Pagos", Location = new Point(18, 25), AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            encabezado.Controls.Add(new Label { Text = "Pagos recibidos y saldos pendientes.", Location = new Point(18, 66), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(95, 116, 125) });

            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(0, 8, 0, 4), BackColor = Color.FromArgb(241, 246, 246) };
            for (int i = 0; i < 3; i++) { tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F)); Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 1 ? 8 : 0, 0, i == 2 ? 0 : 8, 0), Padding = new Padding(18, 12, 12, 8) }; Label nombre = new Label { Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(102, 123, 131) }; _resumenPagos[i] = new Label { Dock = DockStyle.Top, Height = 43, Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) }; nombre.Text = i == 0 ? "PAGOS DE HOY" : i == 1 ? "COBRADO HOY" : "PENDIENTE"; tarjeta.Controls.Add(_resumenPagos[i]); tarjeta.Controls.Add(nombre); tarjetas.Controls.Add(tarjeta, i, 0); }

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Pagos recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            barra.Controls.Add(new Label { Text = "Historial de pagos recibidos", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(123, 139, 147) });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 350, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonPago("+ Nuevo registro", true); nuevo.Click += (s, e) => { btnNuevo_Click(s, e); _dialogoPago.Text = "Registrar pago"; _dialogoPago.ShowDialog(this); };
            _buscarPagos = new TextBox { Width = 190, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle };
            _buscarPagos.TextChanged += (s, e) => { _pagina = 1; MostrarHistorialPagos(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(_buscarPagos); barra.Controls.Add(acciones);
            _gridHistorial = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.White; _gridHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(102, 123, 131); _gridHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold); _gridHistorial.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White; _gridHistorial.DefaultCellStyle.ForeColor = Color.FromArgb(50, 69, 77); _gridHistorial.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 246, 241); _gridHistorial.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 59, 56); _gridHistorial.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridHistorial.RowTemplate.Height = 42;
            _gridHistorial.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _textoRegistros = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(119, 136, 145) };
            _textoPagina = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(92, 111, 119) };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel paginacion = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguiente = CrearBotonPaginacionPago("Siguiente >"); _anterior = CrearBotonPaginacionPago("< Anterior"); _siguiente.Click += (s, e) => CambiarPagina(1); _anterior.Click += (s, e) => CambiarPagina(-1);
            paginacion.Controls.Add(_siguiente); paginacion.Controls.Add(_textoPagina); paginacion.Controls.Add(_anterior); pie.Controls.Add(_textoRegistros); pie.Controls.Add(paginacion);
            lista.Controls.Add(_gridHistorial); lista.Controls.Add(pie); lista.Controls.Add(barra);
            layout.Controls.Add(encabezado, 0, 0); layout.Controls.Add(tarjetas, 0, 1); layout.Controls.Add(lista, 0, 2); Controls.Add(layout);
            Text = "Pagos"; BackColor = Color.FromArgb(241, 246, 246); MinimumSize = new Size(980, 620);
        }

        private void ConfigurarEditorPagoCompacto()
        {
            foreach (Control control in _dialogoPago.Controls) control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pictureBox1.Visible = false;
            lblTitulo.Text = "Registrar pago"; lblTitulo.Location = new Point(24, 18); lblTitulo.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblSubtitulo.Text = "Selecciona una venta y registra el abono."; lblSubtitulo.Location = new Point(26, 57); lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblTitulo.Visible = false; lblSubtitulo.Visible = false;
            lblVenta.SetBounds(24, 100, 100, 22); cmbVenta.SetBounds(150, 96, 714, 30);
            lblTotalTitulo.SetBounds(24, 154, 70, 30); lblTotalValor.SetBounds(94, 152, 140, 34);
            lblPagadoTitulo.SetBounds(300, 154, 80, 30); lblPagadoValor.SetBounds(382, 152, 140, 34);
            lblSaldoTitulo.SetBounds(584, 154, 70, 30); lblSaldoValor.SetBounds(654, 152, 210, 34);
            lblMonto.SetBounds(24, 210, 100, 28); nudMonto.SetBounds(150, 208, 150, 30);
            lblMetodo.SetBounds(330, 210, 140, 28); cmbMetodo.SetBounds(470, 208, 200, 30);
            btnRegistrar.SetBounds(690, 205, 174, 38);
            lblHistorial.SetBounds(24, 260, 240, 28); dgvPagos.SetBounds(24, 292, 840, 220);
            btnNuevo.Visible = false; btnCerrar.Text = "Cancelar"; btnCerrar.SetBounds(744, 535, 120, 38);
        }

        private static Button CrearBotonPago(string texto, bool primario)
        {
            return new Button { Text = texto, Width = primario ? 132 : 100, Height = 30, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = primario ? Color.FromArgb(22, 139, 112) : Color.White, ForeColor = primario ? Color.White : Color.FromArgb(50, 69, 77), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
        }

        private static Button CrearBotonPaginacionPago(string texto)
        {
            return new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 3, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(68, 91, 101), Font = new Font("Segoe UI", 8F) };
        }

        private void CargarHistorial()
        {
            if (_gridHistorial == null) return;
            try { _pagos = _negocio.MtdListarPagosRecientes(); _ventas = _negocio.MtdListarVentas(); MostrarHistorialPagos(); }
            catch (Exception ex) { MessageBox.Show("No se pudo cargar el historial de pagos. " + ex.Message, "Pagos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MostrarHistorialPagos()
        {
            if (_gridHistorial == null) return;
            var filtrados = _pagos.AsEnumerable().Where(r => String.IsNullOrWhiteSpace(_buscarPagos.Text) || Convert.ToString(r["id_pago"]).IndexOf(_buscarPagos.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 || Convert.ToString(r["cliente"]).IndexOf(_buscarPagos.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 || Convert.ToString(r["metodo_pago"]).IndexOf(_buscarPagos.Text, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            int paginas = Math.Max(1, (int)Math.Ceiling(filtrados.Count / (double)PagosPorPagina)); _pagina = Math.Max(1, Math.Min(_pagina, paginas));
            DataTable pagina = _pagos.Clone(); foreach (DataRow fila in filtrados.Skip((_pagina - 1) * PagosPorPagina).Take(PagosPorPagina)) pagina.ImportRow(fila);
            _gridHistorial.DataSource = pagina;
            _gridHistorial.Columns["id_pago"].HeaderText = "PAGO"; _gridHistorial.Columns["id_pago"].FillWeight = 14;
            _gridHistorial.Columns["cliente"].HeaderText = "CLIENTE"; _gridHistorial.Columns["cliente"].FillWeight = 27;
            _gridHistorial.Columns["metodo_pago"].HeaderText = "MÉTODO"; _gridHistorial.Columns["metodo_pago"].FillWeight = 20;
            _gridHistorial.Columns["fecha"].HeaderText = "FECHA"; _gridHistorial.Columns["fecha"].DefaultCellStyle.Format = "dd MMM yyyy"; _gridHistorial.Columns["fecha"].FillWeight = 20;
            _gridHistorial.Columns["monto"].HeaderText = "IMPORTE"; _gridHistorial.Columns["monto"].DefaultCellStyle.Format = "'Q 'N2"; _gridHistorial.Columns["monto"].FillWeight = 19;
            _gridHistorial.Columns["id_venta"].Visible = false; _gridHistorial.ClearSelection();
            int desde = filtrados.Count == 0 ? 0 : ((_pagina - 1) * PagosPorPagina) + 1, hasta = Math.Min(_pagina * PagosPorPagina, filtrados.Count);
            _textoRegistros.Text = "Mostrando " + desde + "-" + hasta + " de " + filtrados.Count + " registros"; _textoPagina.Text = _pagina + " de " + paginas;
            _anterior.Enabled = _pagina > 1; _siguiente.Enabled = _pagina < paginas;
            var hoy = _pagos.AsEnumerable().Where(r => r.Field<DateTime>("fecha").Date == DateTime.Today).ToList();
            _resumenPagos[0].Text = hoy.Count.ToString("N0"); _resumenPagos[1].Text = "Q " + hoy.Sum(r => r.Field<decimal>("monto")).ToString("N2");
            decimal pendiente = _ventas.AsEnumerable().Sum(r => Math.Max(0m, r.Field<decimal>("total") - r.Field<decimal>("pagado")));
            _resumenPagos[2].Text = "Q " + pendiente.ToString("N2");
        }

        private void CambiarPagina(int cambio) { _pagina += cambio; MostrarHistorialPagos(); }

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
                MessageBox.Show("No se pudieron cargar las ventas: " + ex.Message, "Pagos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _cargando = false;
            }

            MostrarResumen();
        }

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

            dgvPagos.DataSource = _negocio.MtdListarPagos(idVenta);
            if (dgvPagos.Columns.Contains("Monto"))
                dgvPagos.Columns["Monto"].DefaultCellStyle.Format = "N2";

            bool pendiente = _saldo > 0;
            nudMonto.Enabled = pendiente;
            cmbMetodo.Enabled = pendiente;
            btnRegistrar.Enabled = pendiente;

            nudMonto.Maximum = pendiente ? _saldo : 0;
            nudMonto.Value = pendiente ? _saldo : 0;
        }

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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            DataRowView fila = cmbVenta.SelectedItem as DataRowView;
            if (fila == null)
            {
                MessageBox.Show("Seleccione una venta.", "Pagos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idVenta = Convert.ToInt32(fila["id_venta"]);

            Pago pago = new Pago
            {
                IdVenta = idVenta,
                Fecha = DateTime.Now,
                Monto = nudMonto.Value,
                MetodoPago = cmbMetodo.Text
            };

            string mensaje;
            bool ok = _negocio.MtdRegistrar(pago, out mensaje);

            MessageBox.Show(mensaje, "Pagos", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (ok)
                CargarVentas(idVenta);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
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

            MessageBox.Show("No hay ventas con saldo pendiente.", "Pagos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Form contenedor = ((Control)sender).FindForm();
            if (contenedor != null && contenedor != this) contenedor.Close();
        }
    }
}
