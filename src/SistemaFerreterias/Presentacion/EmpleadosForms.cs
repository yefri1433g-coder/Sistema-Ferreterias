using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Entidad;
using Negocio;

namespace Presentacion
{
    public class EmpleadosForms : Form
    {
        private const int RegistrosPorPagina = 10;
        private readonly EmpleadoNegocio _negocio = new EmpleadoNegocio();
        private readonly ComboBox cboUsuario = new ComboBox();
        private readonly ComboBox cboSucursal = new ComboBox();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtApellido = new TextBox();
        private readonly TextBox txtCargo = new TextBox();
        private readonly TextBox txtTelefono = new TextBox();
        private readonly TextBox txtBuscar = new TextBox();
        private readonly CheckBox chkEstado = new CheckBox();
        private readonly DataGridView dgvEmpleados = new DataGridView();
        private readonly Button btnAnterior = new Button();
        private readonly Button btnSiguiente = new Button();
        private readonly Button btnNuevo = new Button();
        private readonly Button btnEditar = new Button();
        private readonly Button btnEliminar = new Button();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnBuscar = new Button();
        private readonly Label lblPagina = new Label();
        private List<Empleado> _empleados = new List<Empleado>();
        private List<Empleado> _empleadosTotales = new List<Empleado>();
        private Empleado _empleadoSeleccionado;
        private bool _modoEdicion;
        private int _paginaActual = 1;
        private readonly Label[] _resumenEmpleados = new Label[3];
        private DataGridView _gridEmpleadosVista;
        private TextBox _buscarEmpleadosVista;
        private Label _totalEmpleadosVista, _paginaEmpleadosVista;
        private Button _anteriorEmpleadosVista, _siguienteEmpleadosVista;
        private TabControl _pestanasEmpleados;
        private int _paginaVistaEmpleados = 1;
        private const int EmpleadosPorPaginaVista = 4;

        public EmpleadosForms()
        {
            Text = "Empleados";
            BuildForm();
            Load += EmpleadosForms_Load;
            EstiloAplicacion.Aplicar(this);
            PrepararDisenoEmpleados();
        }

        private void PrepararDisenoEmpleados()
        {
            Control[] controlesAnteriores = new Control[Controls.Count]; Controls.CopyTo(controlesAnteriores, 0); Controls.Clear();
            _pestanasEmpleados = new TabControl { Dock = DockStyle.Fill, Appearance = TabAppearance.FlatButtons, ItemSize = new Size(0, 1), SizeMode = TabSizeMode.Fixed };
            TabPage resumen = new TabPage("Empleados") { BackColor = Color.FromArgb(241, 246, 245), Padding = new Padding(0) };
            TabPage administracion = new TabPage("Administrar empleados") { BackColor = Color.White, AutoScroll = true };
            foreach (Control control in controlesAnteriores) administracion.Controls.Add(control);
            Button volver = new Button { Text = "< Volver al resumen", Dock = DockStyle.Top, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(23, 143, 112), TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
            volver.FlatAppearance.BorderSize = 0; volver.Click += (s, e) => _pestanasEmpleados.SelectedTab = resumen;
            administracion.Controls.Add(volver); _pestanasEmpleados.TabPages.Add(resumen); _pestanasEmpleados.TabPages.Add(administracion); Controls.Add(_pestanasEmpleados);

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = Color.FromArgb(241, 246, 245) };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F)); pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Empleados", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(102, 123, 131) });
            encabezado.Controls.Add(new Label { Text = "Empleados", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            encabezado.Controls.Add(new Label { Text = "Directorio de personal y asignación a sucursales.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(102, 123, 131) });
            LinkLabel administrar = new LinkLabel { Text = "Administrar empleados", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = Color.FromArgb(23, 143, 112), Font = new Font("Segoe UI", 8.5F), Location = new Point(Width - 200, 26) };
            administrar.Click += (s, e) => _pestanasEmpleados.SelectedTab = administracion; encabezado.Controls.Add(administrar);

            Panel panelResumen = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245), Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.FromArgb(241, 246, 245) };
            string[] nombres = { "EMPLEADOS", "ACTIVOS", "SUCURSALES" };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "+" : i == 1 ? "o" : "≡", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : Color.FromArgb(35, 184, 139), BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : Color.FromArgb(237, 249, 246) };
                Label nombre = new Label { Text = nombres[i], Left = 14, Top = 42, Height = 18, AutoEllipsis = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(113, 134, 141) };
                _resumenEmpleados[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(nombre); tarjeta.Controls.Add(_resumenEmpleados[i]); tarjetas.Controls.Add(tarjeta, i, 0);
            }
            panelResumen.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Empleados recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            barra.Controls.Add(new Label { Text = "Directorio actualizado - " + DateTime.Today.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("es-MX")), Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(123, 139, 147) });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 405, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonEmpleado("+ Nuevo registro", true); nuevo.Click += (s, e) => AbrirEditorEmpleado(null);
            Button eliminarEmpleadoVista = CrearBotonEmpleado("Eliminar", false); eliminarEmpleadoVista.Width = 82; eliminarEmpleadoVista.Click += (s, e) => EliminarEmpleadoVista();
            _buscarEmpleadosVista = new TextBox { Width = 165, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar empleados", ForeColor = Color.FromArgb(135, 151, 156) };
            _buscarEmpleadosVista.Enter += (s, e) => { if (_buscarEmpleadosVista.Text == "Buscar empleados") { _buscarEmpleadosVista.Clear(); _buscarEmpleadosVista.ForeColor = Color.FromArgb(45, 64, 70); } };
            _buscarEmpleadosVista.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarEmpleadosVista.Text)) { _buscarEmpleadosVista.Text = "Buscar empleados"; _buscarEmpleadosVista.ForeColor = Color.FromArgb(135, 151, 156); } };
            _buscarEmpleadosVista.TextChanged += (s, e) => { if (_buscarEmpleadosVista.Text != "Buscar empleados") { txtBuscar.Text = _buscarEmpleadosVista.Text.Trim(); _paginaActual = 1; _paginaVistaEmpleados = 1; CargarEmpleados(); } };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(eliminarEmpleadoVista); acciones.Controls.Add(_buscarEmpleadosVista); barra.Controls.Add(acciones);

            _gridEmpleadosVista = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            eliminarEmpleadoVista.Enabled = false;
            _gridEmpleadosVista.SelectionChanged += (s, e) => eliminarEmpleadoVista.Enabled = _gridEmpleadosVista.CurrentRow != null;
            _gridEmpleadosVista.ColumnHeadersDefaultCellStyle.BackColor = Color.White; _gridEmpleadosVista.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(102, 123, 131); _gridEmpleadosVista.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold); _gridEmpleadosVista.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White; _gridEmpleadosVista.DefaultCellStyle.ForeColor = Color.FromArgb(50, 69, 77); _gridEmpleadosVista.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 246, 241); _gridEmpleadosVista.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 59, 56); _gridEmpleadosVista.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridEmpleadosVista.RowTemplate.Height = 42;
            _gridEmpleadosVista.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridEmpleadosVista.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridEmpleadosVista.CellFormatting += (s, e) => { if (e.RowIndex >= 0 && _gridEmpleadosVista.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null) e.CellStyle.ForeColor = Convert.ToString(e.Value) == "Activo" ? Color.FromArgb(22, 151, 113) : Color.FromArgb(210, 126, 48); };
            _gridEmpleadosVista.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AbrirEditorEmpleado(_gridEmpleadosVista.Rows[e.RowIndex].Tag as Empleado); };
            ContextMenuStrip menuFila = new ContextMenuStrip(); menuFila.Items.Add("Editar empleado", null, (s, e) => { if (_gridEmpleadosVista.CurrentRow != null) AbrirEditorEmpleado(_gridEmpleadosVista.CurrentRow.Tag as Empleado); }); menuFila.Items.Add("Eliminar empleado", null, (s, e) => EliminarEmpleadoVista()); _gridEmpleadosVista.ContextMenuStrip = menuFila;
            _gridEmpleadosVista.CellMouseDown += (s, e) => { if (e.Button == MouseButtons.Right && e.RowIndex >= 0) _gridEmpleadosVista.CurrentCell = _gridEmpleadosVista.Rows[e.RowIndex].Cells[0]; };
            _totalEmpleadosVista = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(119, 136, 145) };
            _paginaEmpleadosVista = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(92, 111, 119) };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel controlesPagina = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteEmpleadosVista = CrearBotonPaginacionEmpleado("Siguiente >"); _anteriorEmpleadosVista = CrearBotonPaginacionEmpleado("< Anterior");
            _siguienteEmpleadosVista.Click += (s, e) => CambiarPaginaEmpleado(1); _anteriorEmpleadosVista.Click += (s, e) => CambiarPaginaEmpleado(-1);
            controlesPagina.Controls.Add(_siguienteEmpleadosVista); controlesPagina.Controls.Add(_paginaEmpleadosVista); controlesPagina.Controls.Add(_anteriorEmpleadosVista);
            pie.Controls.Add(_totalEmpleadosVista); pie.Controls.Add(controlesPagina); lista.Controls.Add(_gridEmpleadosVista); lista.Controls.Add(pie); lista.Controls.Add(barra);
            RedondearPanel(lista, 14); for (int i = 0; i < tarjetas.Controls.Count; i++) RedondearPanel(tarjetas.Controls[i], 10);
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(panelResumen, 0, 1); pantalla.Controls.Add(lista, 0, 2); resumen.Controls.Add(pantalla);
            _pestanasEmpleados.SelectedIndexChanged += (s, e) => { if (_pestanasEmpleados.SelectedTab == resumen) CargarEmpleados(); };
        }

        private void BuildForm()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(18),
                BackColor = Color.FromArgb(246, 247, 249)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 270));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));

            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            PictureBox imagen = new PictureBox
            {
                Image = EstiloAplicacion.ObtenerImagenClientes(),
                Location = new Point(12, 7),
                Size = new Size(92, 56),
                SizeMode = PictureBoxSizeMode.Zoom,
                TabStop = false
            };
            Label titulo = new Label
            {
                Text = "Gestión de empleados",
                Location = new Point(120, 0),
                Size = new Size(600, 70),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            encabezado.Controls.Add(imagen);
            encabezado.Controls.Add(titulo);

            Panel campos = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            AgregarEtiquetaYControl(campos, "Usuario *", cboUsuario, 18, 14, 430, 31);
            AgregarEtiquetaYControl(campos, "Sucursal *", cboSucursal, 490, 14, 430, 31);
            AgregarEtiquetaYControl(campos, "Nombre *", txtNombre, 18, 76, 280, 31);
            AgregarEtiquetaYControl(campos, "Apellido *", txtApellido, 330, 76, 280, 31);
            AgregarEtiquetaYControl(campos, "Cargo *", txtCargo, 642, 76, 278, 31);
            AgregarEtiquetaYControl(campos, "Teléfono", txtTelefono, 18, 138, 280, 31);

            chkEstado.Text = "Empleado activo";
            chkEstado.Checked = true;
            chkEstado.AutoSize = true;
            chkEstado.Location = new Point(330, 164);
            campos.Controls.Add(chkEstado);

            btnNuevo.Text = "Nuevo";
            btnNuevo.Size = new Size(90, 36);
            btnNuevo.Location = new Point(642, 198);
            btnNuevo.Click += btnNuevo_Click;
            campos.Controls.Add(btnNuevo);

            btnEditar.Text = "Editar";
            btnEditar.Size = new Size(90, 36);
            btnEditar.Location = new Point(642, 148);
            btnEditar.Click += btnEditar_Click;
            campos.Controls.Add(btnEditar);

            btnEliminar.Text = "Eliminar";
            btnEliminar.Size = new Size(110, 36);
            btnEliminar.Location = new Point(770, 148);
            btnEliminar.Click += btnEliminar_Click;
            campos.Controls.Add(btnEliminar);

            btnGuardar.Text = "Guardar";
            btnGuardar.Size = new Size(110, 36);
            btnGuardar.Location = new Point(770, 198);
            btnGuardar.Click += btnGuardar_Click;
            campos.Controls.Add(btnGuardar);

            Panel barraBusqueda = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Label lblBuscar = new Label
            {
                Text = "Buscar empleado:",
                Location = new Point(12, 15),
                Size = new Size(125, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };
            txtBuscar.Location = new Point(142, 10);
            txtBuscar.Size = new Size(520, 30);
            txtBuscar.MaxLength = 100;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(674, 8);
            btnBuscar.Size = new Size(130, 36);
            btnBuscar.Click += btnBuscar_Click;
            barraBusqueda.Controls.Add(lblBuscar);
            barraBusqueda.Controls.Add(txtBuscar);
            barraBusqueda.Controls.Add(btnBuscar);

            dgvEmpleados.Dock = DockStyle.Fill;
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.AllowUserToDeleteRows = false;
            dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.MultiSelect = false;
            dgvEmpleados.RowHeadersVisible = false;
            dgvEmpleados.CellClick += dgvEmpleados_CellClick;

            FlowLayoutPanel paginacion = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(6)
            };
            btnAnterior.Text = "Anterior";
            btnAnterior.Width = 100;
            btnAnterior.Height = 32;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Text = "Siguiente";
            btnSiguiente.Width = 100;
            btnSiguiente.Height = 32;
            btnSiguiente.Click += btnSiguiente_Click;
            lblPagina.AutoSize = true;
            lblPagina.Margin = new Padding(12, 8, 0, 0);
            paginacion.Controls.Add(btnAnterior);
            paginacion.Controls.Add(btnSiguiente);
            paginacion.Controls.Add(lblPagina);

            layout.Controls.Add(encabezado, 0, 0);
            layout.Controls.Add(campos, 0, 1);
            layout.Controls.Add(barraBusqueda, 0, 2);
            layout.Controls.Add(dgvEmpleados, 0, 3);
            layout.Controls.Add(paginacion, 0, 4);
            Controls.Add(layout);
        }

        private static void AgregarEtiquetaYControl(Panel panel, string texto, Control control,
            int x, int y, int ancho, int alto)
        {
            Label etiqueta = new Label
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(ancho, 20)
            };
            control.Location = new Point(x, y + 23);
            control.Size = new Size(ancho, alto);
            TextBox caja = control as TextBox;
            if (caja != null)
            {
                caja.MaxLength = texto == "Nombre *" || texto == "Apellido *" ? 60 :
                    texto == "Cargo *" ? 50 : 15;
            }
            ComboBox combo = control as ComboBox;
            if (combo != null)
            {
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.DisplayMember = "Texto";
                combo.ValueMember = "Id";
            }
            panel.Controls.Add(etiqueta);
            panel.Controls.Add(control);
        }

        private static Button CrearBotonEmpleado(string texto, bool principal)
        {
            Button boton = new Button { Text = texto, Width = principal ? 128 : 82, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = principal ? Color.FromArgb(23, 143, 112) : Color.White, ForeColor = principal ? Color.White : Color.FromArgb(50, 69, 77), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = principal ? 0 : 1; boton.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230);
            return boton;
        }

        private static Button CrearBotonPaginacionEmpleado(string texto)
        {
            Button boton = new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(37, 120, 105), Font = new Font("Segoe UI", 8F) };
            boton.FlatAppearance.BorderColor = Color.FromArgb(222, 231, 230); return boton;
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

        private void MostrarEmpleadosVista(List<Empleado> lista)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(lista.Count / (double)EmpleadosPorPaginaVista));
            _paginaVistaEmpleados = Math.Max(1, Math.Min(_paginaVistaEmpleados, paginas));
            List<Empleado> pagina = lista.Skip((_paginaVistaEmpleados - 1) * EmpleadosPorPaginaVista).Take(EmpleadosPorPaginaVista).ToList();
            DataTable tabla = new DataTable(); tabla.Columns.Add("Empleado"); tabla.Columns.Add("Puesto"); tabla.Columns.Add("Sucursal"); tabla.Columns.Add("Telefono"); tabla.Columns.Add("Estado");
            foreach (Empleado empleado in pagina) tabla.Rows.Add(((empleado.Nombre ?? "") + " " + (empleado.Apellido ?? "")).Trim(), empleado.Cargo, empleado.Sucursal, String.IsNullOrWhiteSpace(empleado.Telefono) ? "-" : empleado.Telefono, empleado.Estado ? "Activo" : "Inactivo");
            _gridEmpleadosVista.DataSource = tabla;
            _gridEmpleadosVista.Columns["Empleado"].HeaderText = "EMPLEADO"; _gridEmpleadosVista.Columns["Empleado"].FillWeight = 23;
            _gridEmpleadosVista.Columns["Puesto"].HeaderText = "PUESTO"; _gridEmpleadosVista.Columns["Puesto"].FillWeight = 22;
            _gridEmpleadosVista.Columns["Sucursal"].HeaderText = "SUCURSAL"; _gridEmpleadosVista.Columns["Sucursal"].FillWeight = 20;
            _gridEmpleadosVista.Columns["Telefono"].HeaderText = "TELÉFONO"; _gridEmpleadosVista.Columns["Telefono"].FillWeight = 20;
            _gridEmpleadosVista.Columns["Estado"].HeaderText = "ESTADO"; _gridEmpleadosVista.Columns["Estado"].FillWeight = 15;
            _gridEmpleadosVista.ClearSelection();
            for (int i = 0; i < _gridEmpleadosVista.Rows.Count; i++) _gridEmpleadosVista.Rows[i].Tag = pagina[i];
            int desde = lista.Count == 0 ? 0 : ((_paginaVistaEmpleados - 1) * EmpleadosPorPaginaVista) + 1;
            int hasta = Math.Min(_paginaVistaEmpleados * EmpleadosPorPaginaVista, lista.Count);
            _totalEmpleadosVista.Text = "Mostrando " + desde + "-" + hasta + " de " + lista.Count + " registros";
            _paginaEmpleadosVista.Text = _paginaVistaEmpleados + " de " + paginas;
            _anteriorEmpleadosVista.Enabled = _paginaVistaEmpleados > 1; _siguienteEmpleadosVista.Enabled = _paginaVistaEmpleados < paginas;
            _resumenEmpleados[0].Text = _empleadosTotales.Count.ToString("N0");
            _resumenEmpleados[1].Text = _empleadosTotales.Count(x => x.Estado).ToString("N0");
            _resumenEmpleados[2].Text = _empleadosTotales.Select(x => x.IdSucursal).Distinct().Count().ToString("N0");
        }

        private void CambiarPaginaEmpleado(int cambio)
        {
            _paginaVistaEmpleados += cambio; MostrarEmpleadosVista(_empleados);
        }

        private void AbrirEditorEmpleado(Empleado existente)
        {
            List<OpcionEmpleado> usuarios = _negocio.ListarUsuariosActivos().Select(x => new OpcionEmpleado { Id = x.Codigo, Texto = x.Texto }).ToList();
            List<OpcionEmpleado> sucursales = _negocio.ListarSucursalesActivas().Select(x => new OpcionEmpleado { Id = x.Codigo, Texto = x.Texto }).ToList();
            if (existente != null && !usuarios.Any(x => x.Id == existente.IdUsuario)) usuarios.Add(new OpcionEmpleado { Id = existente.IdUsuario, Texto = existente.Usuario });
            if (existente != null && !sucursales.Any(x => x.Id == existente.IdSucursal)) sucursales.Add(new OpcionEmpleado { Id = existente.IdSucursal, Texto = existente.Sucursal });
            Form dialogo = new Form { Text = existente == null ? "Nuevo empleado" : "Editar empleado", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(520, 440), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            Label titulo = new Label { Text = existente == null ? "Nuevo empleado" : "Editar empleado", Left = 22, Top = 18, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
            ComboBox usuario = CrearComboEditorEmpleado(usuarios, 24, 82, 210); ComboBox sucursal = CrearComboEditorEmpleado(sucursales, 254, 82, 220);
            TextBox nombre = CrearTextoEditorEmpleado(24, 142, 210, 60); TextBox apellido = CrearTextoEditorEmpleado(254, 142, 220, 60);
            TextBox cargo = CrearTextoEditorEmpleado(24, 204, 210, 50); TextBox telefono = CrearTextoEditorEmpleado(254, 204, 220, 15);
            CheckBox activo = new CheckBox { Text = "Empleado activo", Left = 24, Top = 266, AutoSize = true, Checked = existente == null || existente.Estado };
            AgregarEtiquetaEditorEmpleado(dialogo, "Usuario", 62, 24); AgregarEtiquetaEditorEmpleado(dialogo, "Sucursal", 62, 254);
            AgregarEtiquetaEditorEmpleado(dialogo, "Nombre", 122, 24); AgregarEtiquetaEditorEmpleado(dialogo, "Apellido", 122, 254);
            AgregarEtiquetaEditorEmpleado(dialogo, "Cargo", 184, 24); AgregarEtiquetaEditorEmpleado(dialogo, "Teléfono", 184, 254);
            if (existente != null) { usuario.SelectedValue = existente.IdUsuario; sucursal.SelectedValue = existente.IdSucursal; nombre.Text = existente.Nombre; apellido.Text = existente.Apellido; cargo.Text = existente.Cargo; telefono.Text = existente.Telefono; }
            Button guardar = CrearBotonEmpleado("Guardar", true); guardar.Left = 274; guardar.Top = 364; guardar.Width = 100;
            Button cancelar = CrearBotonEmpleado("Cancelar", false); cancelar.Left = 384; cancelar.Top = 364; cancelar.Width = 100; cancelar.DialogResult = DialogResult.Cancel; dialogo.CancelButton = cancelar;
            guardar.Click += (s, e) =>
            {
                try
                {
                    Empleado empleado = new Empleado { IdEmpleado = existente == null ? 0 : existente.IdEmpleado, IdUsuario = Convert.ToInt32(usuario.SelectedValue), IdSucursal = Convert.ToInt32(sucursal.SelectedValue), Nombre = nombre.Text, Apellido = apellido.Text, Cargo = cargo.Text, Telefono = telefono.Text, Estado = activo.Checked };
                    if (existente == null) _negocio.Agregar(empleado); else _negocio.Editar(empleado);
                    txtBuscar.Clear(); _paginaActual = 1; _paginaVistaEmpleados = 1; _buscarEmpleadosVista.Text = "Buscar empleados"; CargarEmpleados(); dialogo.DialogResult = DialogResult.OK; dialogo.Close();
                }
                catch (Exception ex) { MostrarError(ex); }
            };
            titulo.Visible = false;
            dialogo.Controls.AddRange(new Control[] { titulo, usuario, sucursal, nombre, apellido, cargo, telefono, activo, guardar, cancelar });
            EstiloAplicacion.AplicarDialogo(dialogo);
            dialogo.ShowDialog(this);
        }

        private static ComboBox CrearComboEditorEmpleado(List<OpcionEmpleado> opciones, int left, int top, int width)
        {
            return new ComboBox { Left = left, Top = top, Width = width, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Texto", ValueMember = "Id", DataSource = opciones };
        }

        private static TextBox CrearTextoEditorEmpleado(int left, int top, int width, int maxLength)
        {
            return new TextBox { Left = left, Top = top, Width = width, MaxLength = maxLength, BorderStyle = BorderStyle.FixedSingle };
        }

        private static void AgregarEtiquetaEditorEmpleado(Form form, string texto, int top, int left)
        {
            form.Controls.Add(new Label { Text = texto, Left = left, Top = top, AutoSize = true, ForeColor = Color.FromArgb(102, 123, 131) });
        }

        private void EliminarEmpleadoVista()
        {
            if (_gridEmpleadosVista.CurrentRow == null || !(_gridEmpleadosVista.CurrentRow.Tag is Empleado)) { MessageBox.Show("Selecciona un empleado.", "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            Empleado empleado = (Empleado)_gridEmpleadosVista.CurrentRow.Tag;
            if (MessageBox.Show("¿Deseas eliminar a " + empleado.Nombre + " " + empleado.Apellido + "?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try { _negocio.Eliminar(empleado.IdEmpleado); _empleadoSeleccionado = null; CargarEmpleados(); }
            catch (Exception ex) { MostrarError(ex); }
        }

        private void EmpleadosForms_Load(object sender, EventArgs e)
        {
            try
            {
                CargarOpciones();
                CargarEmpleados();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarOpciones()
        {
            cboUsuario.DataSource = _negocio.ListarUsuariosActivos()
                .Select(x => new OpcionEmpleado { Id = x.Codigo, Texto = x.Texto }).ToList();
            cboSucursal.DataSource = _negocio.ListarSucursalesActivas()
                .Select(x => new OpcionEmpleado { Id = x.Codigo, Texto = x.Texto }).ToList();
        }

        private void CargarEmpleados()
        {
            _empleadosTotales = _negocio.Listar();
            _empleados = string.IsNullOrWhiteSpace(txtBuscar.Text)
                ? _empleadosTotales
                : _negocio.Buscar(txtBuscar.Text);
            MostrarPaginaActual();
        }

        private void BuscarEmpleados()
        {
            _paginaActual = 1;
            CargarEmpleados();
        }

        private void MostrarPaginaActual()
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(_empleados.Count / (double)RegistrosPorPagina));
            if (_paginaActual > paginas) _paginaActual = paginas;

            dgvEmpleados.DataSource = null;
            dgvEmpleados.DataSource = _empleados
                .Skip((_paginaActual - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            ConfigurarColumnas();
            lblPagina.Text = "Página " + _paginaActual + " de " + paginas +
                " · " + _empleados.Count + " empleados en total";
            btnAnterior.Enabled = _paginaActual > 1;
            btnSiguiente.Enabled = _paginaActual < paginas;
            if (_gridEmpleadosVista != null) MostrarEmpleadosVista(_empleados);
        }

        private void ConfigurarColumnas()
        {
            if (dgvEmpleados.Columns["IdEmpleado"] != null) dgvEmpleados.Columns["IdEmpleado"].HeaderText = "Código";
            if (dgvEmpleados.Columns["IdUsuario"] != null) dgvEmpleados.Columns["IdUsuario"].Visible = false;
            if (dgvEmpleados.Columns["IdSucursal"] != null) dgvEmpleados.Columns["IdSucursal"].Visible = false;
            if (dgvEmpleados.Columns["Usuario"] != null) dgvEmpleados.Columns["Usuario"].HeaderText = "Usuario";
            if (dgvEmpleados.Columns["Sucursal"] != null) dgvEmpleados.Columns["Sucursal"].HeaderText = "Sucursal";
            if (dgvEmpleados.Columns["Nombre"] != null) dgvEmpleados.Columns["Nombre"].HeaderText = "Nombre";
            if (dgvEmpleados.Columns["Apellido"] != null) dgvEmpleados.Columns["Apellido"].HeaderText = "Apellido";
            if (dgvEmpleados.Columns["Cargo"] != null) dgvEmpleados.Columns["Cargo"].HeaderText = "Cargo";
            if (dgvEmpleados.Columns["Telefono"] != null) dgvEmpleados.Columns["Telefono"].HeaderText = "Teléfono";
            if (dgvEmpleados.Columns["Estado"] != null) dgvEmpleados.Columns["Estado"].HeaderText = "Activo";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Empleado empleado = dgvEmpleados.Rows[e.RowIndex].DataBoundItem as Empleado;
            if (empleado == null) return;

            _empleadoSeleccionado = empleado;
            _modoEdicion = false;
            btnGuardar.Text = "Guardar";
            cboUsuario.SelectedValue = empleado.IdUsuario;
            cboSucursal.SelectedValue = empleado.IdSucursal;
            txtNombre.Text = empleado.Nombre;
            txtApellido.Text = empleado.Apellido;
            txtCargo.Text = empleado.Cargo;
            txtTelefono.Text = empleado.Telefono;
            chkEstado.Checked = empleado.Estado;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_empleadoSeleccionado == null)
            {
                MessageBox.Show("Selecciona un empleado de la tabla para editar.", "Empleados",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _modoEdicion = true;
            btnGuardar.Text = "Actualizar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_empleadoSeleccionado == null)
            {
                MessageBox.Show("Selecciona un empleado de la tabla para eliminar.", "Empleados",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmar = MessageBox.Show(
                "¿Deseas eliminar el empleado seleccionado?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            try
            {
                _negocio.Eliminar(_empleadoSeleccionado.IdEmpleado);
                MessageBox.Show("Empleado eliminado correctamente.", "Empleados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarEmpleados();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarEmpleados();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            btnBuscar_Click(sender, e);
        }

        private void LimpiarCampos()
        {
            _empleadoSeleccionado = null;
            _modoEdicion = false;
            btnGuardar.Text = "Guardar";
            if (cboUsuario.Items.Count > 0) cboUsuario.SelectedIndex = 0;
            if (cboSucursal.Items.Count > 0) cboSucursal.SelectedIndex = 0;
            txtNombre.Clear();
            txtApellido.Clear();
            txtCargo.Clear();
            txtTelefono.Clear();
            chkEstado.Checked = true;
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboUsuario.SelectedValue == null || cboSucursal.SelectedValue == null)
                    throw new InvalidOperationException("Debe existir al menos un usuario y una sucursal activa.");

                Empleado empleado = new Empleado
                {
                    IdEmpleado = _modoEdicion && _empleadoSeleccionado != null
                        ? _empleadoSeleccionado.IdEmpleado : 0,
                    IdUsuario = Convert.ToInt32(cboUsuario.SelectedValue),
                    IdSucursal = Convert.ToInt32(cboSucursal.SelectedValue),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Cargo = txtCargo.Text,
                    Telefono = txtTelefono.Text,
                    Estado = chkEstado.Checked
                };

                if (_modoEdicion)
                    _negocio.Editar(empleado);
                else
                    _negocio.Agregar(empleado);

                bool fueEdicion = _modoEdicion;
                MessageBox.Show(fueEdicion ? "Empleado actualizado correctamente." : "Empleado agregado correctamente.", "Empleados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!fueEdicion)
                    _paginaActual = Math.Max(1, (int)Math.Ceiling((_empleados.Count + 1) / (double)RegistrosPorPagina));
                CargarEmpleados();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (_paginaActual <= 1) return;
            _paginaActual--;
            CargarEmpleados();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(_empleados.Count / (double)RegistrosPorPagina));
            if (_paginaActual >= paginas) return;
            _paginaActual++;
            CargarEmpleados();
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private class OpcionEmpleado
        {
            public int Id { get; set; }
            public string Texto { get; set; }
        }
    }
}


