using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE ClientesForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class ClientesForms : Form
    {
        private readonly ClienteNegocio _negocio = new ClienteNegocio();
        private const int RegistrosPorPagina = 10;
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Cliente> _clientesTotales = new List<Cliente>();
        private string _filtroNombre = "";
        private int _paginaActual = 1;
        private int _idSeleccionado = 0;
        private bool _vistaLimpiada = false;
        private readonly Label[] _resumenClientes = new Label[3];
        private DataGridView _gridClientesVista;
        private TextBox _buscarClientesVista;
        private Label _totalClientesVista, _paginaClientesVista;
        private Button _anteriorClientesVista, _siguienteClientesVista;
        private TabControl _pestanasClientes;
        private int _paginaVistaClientes = 1;
        private const int ClientesPorPaginaVista = 4;

        public ClientesForms()
        {
            InitializeComponent();
            EstiloAplicacion.Aplicar(this);
            txtNombre.KeyDown += txtNombre_KeyDown;
            PrepararDisenoClientes();
            BarraListado.Configurar(_gridClientesVista, _buscarClientesVista, "Clientes", () => AbrirEditorCliente(_gridClientesVista.CurrentRow.Tag as Cliente));
        }

        // CONFIGURACIÓN: PrepararDisenoClientes
        private void PrepararDisenoClientes()
        {
            Control[] controlesAnteriores = new Control[Controls.Count];
            Controls.CopyTo(controlesAnteriores, 0);
            Controls.Clear();

            _pestanasClientes = new TabControl { Dock = DockStyle.Fill, Appearance = TabAppearance.FlatButtons, ItemSize = new Size(0, 1), SizeMode = TabSizeMode.Fixed };
            TabPage resumen = new TabPage("Clientes") { BackColor = EstiloAplicacion.Fondo, Padding = new Padding(0) };
            TabPage administracion = new TabPage("Administrar clientes") { BackColor = Color.White, AutoScroll = true };
            foreach (Control control in controlesAnteriores) administracion.Controls.Add(control);
            Button volver = new Button { Text = "< Volver al resumen", Dock = DockStyle.Top, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento, TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
            volver.FlatAppearance.BorderSize = 0; volver.Click += (s, e) => _pestanasClientes.SelectedTab = resumen;
            administracion.Controls.Add(volver);
            _pestanasClientes.TabPages.Add(resumen); _pestanasClientes.TabPages.Add(administracion);
            Controls.Add(_pestanasClientes);

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = EstiloAplicacion.Fondo };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F)); pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = EstiloAplicacion.Fondo };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Clientes", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            encabezado.Controls.Add(new Label { Text = "Clientes", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label { Text = "Consulta sus datos y revisa la actividad reciente.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = EstiloAplicacion.Secundario });
            LinkLabel administrar = new LinkLabel { Text = "Administrar clientes", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = EstiloAplicacion.Acento, Font = new Font("Segoe UI", 8.5F), Location = new Point(Width - 190, 26) };
            administrar.Click += (s, e) => _pestanasClientes.SelectedTab = administracion; encabezado.Controls.Add(administrar);

            Panel resumenPanel = new Panel { Dock = DockStyle.Fill, BackColor = EstiloAplicacion.Fondo, Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = EstiloAplicacion.Fondo };
            string[] nombres = { "CLIENTES", "CON TELÉFONO", "CON NIT" };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "+" : i == 1 ? "o" : "!", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : EstiloAplicacion.Acento, BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : EstiloAplicacion.FondoSuave };
                Label nombre = new Label { Text = nombres[i], Left = 14, Top = 42, Height = 18, AutoEllipsis = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario };
                _resumenClientes[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(nombre); tarjeta.Controls.Add(_resumenClientes[i]); tarjetas.Controls.Add(tarjeta, i, 0);
            }
            resumenPanel.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Clientes recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            barra.Controls.Add(new Label { Text = "Directorio actualizado - " + DateTime.Today.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("es-MX")), Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 405, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonCliente("+ Nuevo registro", true); nuevo.Click += (s, e) => AbrirEditorCliente(null);
            Button eliminarClienteVista = CrearBotonCliente("Eliminar", false); eliminarClienteVista.Width = 82; eliminarClienteVista.Click += (s, e) => EliminarClienteVista();
            _buscarClientesVista = new TextBox { Width = 165, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar clientes", ForeColor = EstiloAplicacion.Secundario };
            _buscarClientesVista.Enter += (s, e) => { if (_buscarClientesVista.Text == "Buscar clientes") { _buscarClientesVista.Clear(); _buscarClientesVista.ForeColor = EstiloAplicacion.Tinta; } };
            _buscarClientesVista.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarClientesVista.Text)) { _buscarClientesVista.Text = "Buscar clientes"; _buscarClientesVista.ForeColor = EstiloAplicacion.Secundario; } };
            _buscarClientesVista.TextChanged += (s, e) => { if (_buscarClientesVista.Text != "Buscar clientes") { _filtroNombre = _buscarClientesVista.Text.Trim(); _paginaActual = 1; _paginaVistaClientes = 1; ConsultarClientes(); } };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(eliminarClienteVista); acciones.Controls.Add(_buscarClientesVista); barra.Controls.Add(acciones);

            _gridClientesVista = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridClientesVista.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino; _gridClientesVista.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; _gridClientesVista.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); _gridClientesVista.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino; _gridClientesVista.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta; _gridClientesVista.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion; _gridClientesVista.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta; _gridClientesVista.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridClientesVista.RowTemplate.Height = 42;
            _gridClientesVista.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridClientesVista.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridClientesVista.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AbrirEditorCliente(_gridClientesVista.Rows[e.RowIndex].Tag as Cliente); };
            eliminarClienteVista.Enabled = false;
            _gridClientesVista.SelectionChanged += (s, e) => eliminarClienteVista.Enabled = _gridClientesVista.CurrentRow != null;
            ContextMenuStrip menuFila = new ContextMenuStrip();
            menuFila.Items.Add("Editar cliente", null, (s, e) => { if (_gridClientesVista.CurrentRow != null) AbrirEditorCliente(_gridClientesVista.CurrentRow.Tag as Cliente); });
            menuFila.Items.Add("Eliminar cliente", null, (s, e) => EliminarClienteVista());
            _gridClientesVista.ContextMenuStrip = menuFila;
            _gridClientesVista.CellMouseDown += (s, e) => { if (e.Button == MouseButtons.Right && e.RowIndex >= 0) _gridClientesVista.CurrentCell = _gridClientesVista.Rows[e.RowIndex].Cells[0]; };

            _totalClientesVista = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            _paginaClientesVista = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel controlesPagina = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteClientesVista = CrearBotonPaginacionCliente("Siguiente >"); _anteriorClientesVista = CrearBotonPaginacionCliente("< Anterior");
            _siguienteClientesVista.Click += (s, e) => CambiarPaginaCliente(1); _anteriorClientesVista.Click += (s, e) => CambiarPaginaCliente(-1);
            controlesPagina.Controls.Add(_siguienteClientesVista); controlesPagina.Controls.Add(_paginaClientesVista); controlesPagina.Controls.Add(_anteriorClientesVista);
            pie.Controls.Add(_totalClientesVista); pie.Controls.Add(controlesPagina);
            lista.Controls.Add(_gridClientesVista); lista.Controls.Add(pie); lista.Controls.Add(barra);
            RedondearPanel(lista, 14); for (int i = 0; i < tarjetas.Controls.Count; i++) RedondearPanel(tarjetas.Controls[i], 10);
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(resumenPanel, 0, 1); pantalla.Controls.Add(lista, 0, 2);
            resumen.Controls.Add(pantalla);
            _pestanasClientes.SelectedIndexChanged += (s, e) => { if (_pestanasClientes.SelectedTab == resumen) ConsultarClientes(); };
        }

        private void ClientesForms_Load(object sender, EventArgs e)
        {
            ConsultarClientes();
            Limpiar();
        }

        // Consulta por medio de Negocio y presenta los errores de conexión o lectura.
        // CONSULTAR: ConsultarClientes
        private void ConsultarClientes()
        {
            try
            {
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron consultar los clientes. Comprueba la conexión con la base de datos.\n\n" + ex.Message,
                    "Error al consultar clientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CARGA DE DATOS: CargarClientes
        private void CargarClientes()
        {
            _vistaLimpiada = false;
            _clientesTotales = _negocio.Listar();
            _clientes = string.IsNullOrWhiteSpace(_filtroNombre) ? _clientesTotales : _negocio.Buscar(_filtroNombre);
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(_clientes.Count / (double)RegistrosPorPagina));
            if (_paginaActual > totalPaginas)
                _paginaActual = totalPaginas;

            dgvClientes.DataSource = null;
            dgvClientes.DataSource = _clientes
                .Skip((_paginaActual - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            lblPagina.Text = $"Página {_paginaActual} de {totalPaginas} · {_clientes.Count} clientes en total";
            btnPaginaAnterior.Enabled = _paginaActual > 1;
            btnPaginaSiguiente.Enabled = _paginaActual < totalPaginas;

            if (dgvClientes.Columns["IdCliente"] != null)
                dgvClientes.Columns["IdCliente"].HeaderText = "Id";
            if (dgvClientes.Columns["Nit"] != null)
                dgvClientes.Columns["Nit"].HeaderText = "NIT";
            if (dgvClientes.Columns["Telefono"] != null)
                dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";
            if (dgvClientes.Columns["Direccion"] != null)
                dgvClientes.Columns["Direccion"].HeaderText = "Dirección";
            if (_gridClientesVista != null) MostrarClientesVista(_clientes);
        }

        // CREAR ELEMENTO VISUAL: CrearBotonCliente
        private static Button CrearBotonCliente(string texto, bool principal)
        {
            Button boton = new Button { Text = texto, Width = principal ? 128 : 82, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = principal ? EstiloAplicacion.Acento : Color.White, ForeColor = principal ? Color.White : EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = principal ? 0 : 1; boton.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230);
            return boton;
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPaginacionCliente
        private static Button CrearBotonPaginacionCliente(string texto)
        {
            Button boton = new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento, Font = new Font("Segoe UI", 8F) };
            boton.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230);
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
                    ruta.AddArc(0, 0, diametro, diametro, 180, 90); ruta.AddArc(control.Width - diametro, 0, diametro, diametro, 270, 90);
                    ruta.AddArc(control.Width - diametro, control.Height - diametro, diametro, diametro, 0, 90); ruta.AddArc(0, control.Height - diametro, diametro, diametro, 90, 90); ruta.CloseFigure();
                    Region anterior = control.Region; control.Region = new Region(ruta); if (anterior != null) anterior.Dispose();
                }
            };
            control.Resize += (s, e) => actualizarRegion(); control.HandleCreated += (s, e) => actualizarRegion();
        }

        // PRESENTAR DATOS: MostrarClientesVista
        private void MostrarClientesVista(List<Cliente> lista)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(lista.Count / (double)ClientesPorPaginaVista));
            _paginaVistaClientes = Math.Max(1, Math.Min(_paginaVistaClientes, paginas));
            List<Cliente> pagina = lista.Skip((_paginaVistaClientes - 1) * ClientesPorPaginaVista).Take(ClientesPorPaginaVista).ToList();
            DataTable tabla = new DataTable(); tabla.Columns.Add("Nombre"); tabla.Columns.Add("NIT"); tabla.Columns.Add("Telefono"); tabla.Columns.Add("Direccion");
            foreach (Cliente cliente in pagina) tabla.Rows.Add(cliente.Nombre, String.IsNullOrWhiteSpace(cliente.Nit) ? "-" : cliente.Nit, String.IsNullOrWhiteSpace(cliente.Telefono) ? "-" : cliente.Telefono, String.IsNullOrWhiteSpace(cliente.Direccion) ? "-" : cliente.Direccion);
            _gridClientesVista.DataSource = tabla;
            _gridClientesVista.Columns["Nombre"].HeaderText = "CLIENTE"; _gridClientesVista.Columns["Nombre"].FillWeight = 24;
            _gridClientesVista.Columns["NIT"].HeaderText = "NIT"; _gridClientesVista.Columns["NIT"].FillWeight = 16;
            _gridClientesVista.Columns["Telefono"].HeaderText = "TELÉFONO"; _gridClientesVista.Columns["Telefono"].FillWeight = 20;
            _gridClientesVista.Columns["Direccion"].HeaderText = "DIRECCIÓN"; _gridClientesVista.Columns["Direccion"].FillWeight = 40;
            _gridClientesVista.ClearSelection();
            for (int i = 0; i < _gridClientesVista.Rows.Count; i++) _gridClientesVista.Rows[i].Tag = pagina[i];
            int desde = lista.Count == 0 ? 0 : ((_paginaVistaClientes - 1) * ClientesPorPaginaVista) + 1;
            int hasta = Math.Min(_paginaVistaClientes * ClientesPorPaginaVista, lista.Count);
            _totalClientesVista.Text = "Mostrando " + desde + "-" + hasta + " de " + lista.Count + " registros";
            _paginaClientesVista.Text = _paginaVistaClientes + " de " + paginas;
            _anteriorClientesVista.Enabled = _paginaVistaClientes > 1; _siguienteClientesVista.Enabled = _paginaVistaClientes < paginas;
            _resumenClientes[0].Text = _clientesTotales.Count.ToString("N0");
            _resumenClientes[1].Text = _clientesTotales.Count(x => !String.IsNullOrWhiteSpace(x.Telefono)).ToString("N0");
            _resumenClientes[2].Text = _clientesTotales.Count(x => !String.IsNullOrWhiteSpace(x.Nit)).ToString("N0");
        }

        // CAMBIAR PÁGINA: CambiarPaginaCliente
        private void CambiarPaginaCliente(int cambio)
        {
            _paginaVistaClientes += cambio; MostrarClientesVista(_clientes);
        }

        private void AbrirEditorCliente(Cliente existente)
        {
            Form dialogo = new Form { Text = existente == null ? "Nuevo cliente" : "Editar cliente", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(470, 390), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            Label titulo = new Label { Text = existente == null ? "Nuevo cliente" : "Editar cliente", Left = 22, Top = 18, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            TextBox nombre = new TextBox { Left = 24, Top = 72, Width = 410, MaxLength = 100, BorderStyle = BorderStyle.FixedSingle };
            TextBox nit = new TextBox { Left = 24, Top = 140, Width = 190, MaxLength = 15, BorderStyle = BorderStyle.FixedSingle };
            TextBox telefono = new TextBox { Left = 244, Top = 140, Width = 190, MaxLength = 15, BorderStyle = BorderStyle.FixedSingle };
            TextBox direccion = new TextBox { Left = 24, Top = 208, Width = 410, MaxLength = 150, BorderStyle = BorderStyle.FixedSingle };
            Label lNombre = new Label { Text = "Nombre", Left = 24, Top = 52, AutoSize = true, ForeColor = EstiloAplicacion.Secundario };
            Label lNit = new Label { Text = "NIT", Left = 24, Top = 120, AutoSize = true, ForeColor = EstiloAplicacion.Secundario };
            Label lTelefono = new Label { Text = "Teléfono", Left = 244, Top = 120, AutoSize = true, ForeColor = EstiloAplicacion.Secundario };
            Label lDireccion = new Label { Text = "Dirección", Left = 24, Top = 188, AutoSize = true, ForeColor = EstiloAplicacion.Secundario };
            if (existente != null) { nombre.Text = existente.Nombre; nit.Text = existente.Nit; telefono.Text = existente.Telefono; direccion.Text = existente.Direccion; }
            Button guardar = CrearBotonCliente("Guardar", true); guardar.Left = 244; guardar.Top = 335; guardar.Width = 90;
            Button cancelar = CrearBotonCliente("Cancelar", false); cancelar.Left = 344; cancelar.Top = 335; cancelar.Width = 90; cancelar.DialogResult = DialogResult.Cancel; dialogo.CancelButton = cancelar;
            guardar.Click += (s, e) =>
            {
                Cliente cliente = new Cliente { IdCliente = existente == null ? 0 : existente.IdCliente, Nombre = nombre.Text, Nit = nit.Text, Telefono = telefono.Text, Direccion = direccion.Text };
                try
                {
                    string error = existente == null ? _negocio.Agregar(cliente) : _negocio.Editar(cliente);
                    if (!String.IsNullOrEmpty(error)) { MessageBox.Show(error, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    _filtroNombre = String.Empty; _paginaActual = 1; _paginaVistaClientes = 1; _buscarClientesVista.Text = "Buscar clientes"; CargarClientes();
                    dialogo.DialogResult = DialogResult.OK; dialogo.Close();
                    AyudaForms.MtdInfo("Cliente guardado correctamente.");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
            titulo.Visible = false;
            dialogo.Controls.AddRange(new Control[] { titulo, lNombre, nombre, lNit, nit, lTelefono, telefono, lDireccion, direccion, guardar, cancelar });
            EstiloAplicacion.AplicarDialogo(dialogo);
            dialogo.ShowDialog(this);
        }

        // ELIMINAR: EliminarClienteVista
        private void EliminarClienteVista()
        {
            if (_gridClientesVista.CurrentRow == null || !(_gridClientesVista.CurrentRow.Tag is Cliente)) { MessageBox.Show("Selecciona un cliente.", "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            Cliente cliente = (Cliente)_gridClientesVista.CurrentRow.Tag;
            DialogResult confirmar = AyudaForms.MtdConfirmacion("¿Deseas eliminar a " + cliente.Nombre + "?");
            if (confirmar != DialogResult.Yes) return;
            try { _negocio.Eliminar(cliente.IdCliente); ConsultarClientes(); AyudaForms.MtdInfo("Cliente eliminado correctamente."); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // LIMPIAR: Limpiar
        private void Limpiar()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtNit.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtNombre.Focus();
        }

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _filtroNombre = "";
            _paginaActual = 1;
            _idSeleccionado = 0;
            Limpiar();
            // No volver a consultar aquí: tras Limpiar, Nuevo debe mantener la tabla vacía.
            dgvClientes.ClearSelection();
        }

        // Enter en Nombre busca sin agregar controles ni cambiar el diseño.
        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            BuscarClientes();
        }

        // BUSCAR: btnBuscar_Click
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarClientes();
        }

        // BUSCAR: BuscarClientes
        private void BuscarClientes()
        {
            _filtroNombre = txtNombre.Text.Trim();
            _paginaActual = 1;
            _idSeleccionado = 0;
            dgvClientes.ClearSelection();
            ConsultarClientes();
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (!_vistaLimpiada && _paginaActual > 1)
            {
                _paginaActual--;
                ConsultarClientes();
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(_clientes.Count / (double)RegistrosPorPagina));
            if (!_vistaLimpiada && _paginaActual < totalPaginas)
            {
                _paginaActual++;
                ConsultarClientes();
            }
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Cliente c = new Cliente
            {
                IdCliente = _idSeleccionado,
                Nombre = txtNombre.Text,
                Nit = txtNit.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDireccion.Text
            };

            try
            {
                // Un Id igual a cero indica que se trata de un registro nuevo.
                string error = _idSeleccionado == 0
                    ? _negocio.Agregar(c)
                    : _negocio.Guardar(c);

                if (error != "")
                {
                    MessageBox.Show(error, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AyudaForms.MtdInfo("Cliente guardado.");
                CargarClientes();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al agregar el cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Cliente c = (Cliente)dgvClientes.Rows[e.RowIndex].DataBoundItem;
            _idSeleccionado = c.IdCliente;
            txtNombre.Text = c.Nombre;
            txtNit.Text = c.Nit;
            txtTelefono.Text = c.Telefono;
            txtDireccion.Text = c.Direccion;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Limpia solo los campos del formulario y conserva los registros visibles.
        // LIMPIAR: btnLimpiarTabla_Click
        private void btnLimpiarTabla_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        // ELIMINAR: btnEliminar_Click
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona primero un cliente de la tabla.", "Clientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = AyudaForms.MtdConfirmacion(
                "¿Deseas eliminar el cliente seleccionado? Si tiene ventas relacionadas, SQL Server no permitirá borrarlo.");
            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                _negocio.Eliminar(_idSeleccionado);
                CargarClientes();
                Limpiar();
                AyudaForms.MtdInfo("Cliente eliminado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

