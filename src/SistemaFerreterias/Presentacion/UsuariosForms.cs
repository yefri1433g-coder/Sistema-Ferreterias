using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE UsuariosForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class UsuariosForms : Form
    {
        private const int RegistrosPorPagina = 10;
        private readonly UsuarioNegocio _negocio = new UsuarioNegocio();
        private readonly Usuario _usuarioSesion;
        private List<Usuario> _usuarios = new List<Usuario>();
        private List<Usuario> _usuariosTotales = new List<Usuario>();
        private int _idSeleccionado = 0;
        private int _paginaActual = 0;
        private readonly Label[] _resumenUsuarios = new Label[3];
        private DataGridView _gridUsuariosVista;
        private TextBox _buscarUsuariosVista;
        private Label _textoRegistrosVista, _textoPaginaVista;
        private Button _anteriorVista, _siguienteVista;
        private Form _dialogoUsuarios;
        private int _paginaVista = 1;
        private const int UsuariosPorPaginaVista = 4;

        public UsuariosForms()
            : this(null)
        {
        }

        public UsuariosForms(Usuario usuarioSesion)
        {
            InitializeComponent();
            _usuarioSesion = usuarioSesion;
            PrepararDisenoUsuarios();
            BarraListado.Configurar(_gridUsuariosVista, _buscarUsuariosVista, "Usuarios", () => AbrirAdministracion(_gridUsuariosVista.CurrentRow.Tag as Usuario));
            EstiloAplicacion.Aplicar(this);
            MinimumSize = new System.Drawing.Size(780, 520);
            cboRol.BackColor = System.Drawing.Color.White;
            cboRol.ForeColor = EstiloAplicacion.Tinta;
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Height = 30;
            pictureBox1.Image = EstiloAplicacion.ObtenerImagenClientes();
            pictureBox1.Location = new System.Drawing.Point(24, 10);
            pictureBox1.Size = new System.Drawing.Size(128, 72);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            panel3.Controls.Add(pictureBox1);
            label5.Location = new System.Drawing.Point(170, 16);
            label6.Location = new System.Drawing.Point(174, 61);
        }

        // CONFIGURACIÓN: PrepararDisenoUsuarios
        private void PrepararDisenoUsuarios()
        {
            Control[] controlesEditor = Controls.Cast<Control>().ToArray();
            Controls.Clear();
            _dialogoUsuarios = new Form { Text = "Administrar usuarios", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(940, 620), MinimumSize = new Size(800, 540), BackColor = EstiloAplicacion.Fondo, Font = new Font("Segoe UI", 9F) };
            _dialogoUsuarios.Controls.AddRange(controlesEditor);
            _dialogoUsuarios.FormClosed += (s, e) => CargarUsuarios();

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(22, 18, 22, 18), BackColor = EstiloAplicacion.Fondo, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18, 10, 18, 8) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Usuarios", Location = new Point(18, 6), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            Label usuarioConectado = new Label
            {
                Text = TextoUsuarioConectado(),
                Dock = DockStyle.Right,
                Width = 300,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = EstiloAplicacion.Tinta,
                AutoEllipsis = true
            };
            encabezado.Controls.Add(usuarioConectado);
            encabezado.Controls.Add(new Label { Text = "Usuarios", Location = new Point(18, 25), AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label { Text = "Gestiona cuentas, roles y acceso al sistema.", Location = new Point(18, 66), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = EstiloAplicacion.Secundario });

            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(0, 8, 0, 4), BackColor = EstiloAplicacion.Fondo };
            for (int i = 0; i < 3; i++) { tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F)); Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 1 ? 8 : 0, 0, i == 2 ? 0 : 8, 0), Padding = new Padding(18, 12, 12, 8) }; Label nombre = new Label { Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario }; _resumenUsuarios[i] = new Label { Dock = DockStyle.Top, Height = 42, Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta }; nombre.Text = i == 0 ? "USUARIOS" : i == 1 ? "ACTIVOS" : "ROLES"; tarjeta.Controls.Add(_resumenUsuarios[i]); tarjeta.Controls.Add(nombre); tarjetas.Controls.Add(tarjeta, i, 0); }

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Usuarios recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            barra.Controls.Add(new Label { Text = "Cuentas y roles registrados", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 410, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonUsuarioVista("+ Nuevo registro", true); nuevo.Click += (s, e) => AbrirAdministracion(null);
            Button eliminarUsuarioVista = CrearBotonUsuarioVista("Eliminar", false); eliminarUsuarioVista.Width = 82; eliminarUsuarioVista.Click += (s, e) => EliminarUsuarioVista();
            _buscarUsuariosVista = new TextBox { Width = 160, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle };
            _buscarUsuariosVista.TextChanged += (s, e) => { BuscarUsuariosVista(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(eliminarUsuarioVista); acciones.Controls.Add(_buscarUsuariosVista); barra.Controls.Add(acciones);
            _gridUsuariosVista = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridUsuariosVista.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino; _gridUsuariosVista.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; _gridUsuariosVista.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); _gridUsuariosVista.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino; _gridUsuariosVista.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta; _gridUsuariosVista.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion; _gridUsuariosVista.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta; _gridUsuariosVista.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridUsuariosVista.RowTemplate.Height = 42;
            _gridUsuariosVista.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridUsuariosVista.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridUsuariosVista.CellFormatting += (s, e) => { if (e.RowIndex >= 0 && _gridUsuariosVista.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null) e.CellStyle.ForeColor = Convert.ToString(e.Value) == "Activo" ? EstiloAplicacion.Acento : Color.FromArgb(210, 126, 48); };
            eliminarUsuarioVista.Enabled = false;
            _gridUsuariosVista.SelectionChanged += (s, e) => eliminarUsuarioVista.Enabled = _gridUsuariosVista.CurrentRow != null;
            _gridUsuariosVista.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AbrirAdministracion(_gridUsuariosVista.Rows[e.RowIndex].Tag as Usuario); };
            ContextMenuStrip menuFila = new ContextMenuStrip();
            menuFila.Items.Add("Editar usuario", null, (s, e) => { if (_gridUsuariosVista.CurrentRow != null) AbrirAdministracion(_gridUsuariosVista.CurrentRow.Tag as Usuario); });
            menuFila.Items.Add("Eliminar usuario", null, (s, e) => EliminarUsuarioVista());
            _gridUsuariosVista.ContextMenuStrip = menuFila;
            _gridUsuariosVista.CellMouseDown += (s, e) => { if (e.Button == MouseButtons.Right && e.RowIndex >= 0) _gridUsuariosVista.CurrentCell = _gridUsuariosVista.Rows[e.RowIndex].Cells[0]; };
            _textoRegistrosVista = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            _textoPaginaVista = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel paginacion = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteVista = CrearBotonPaginacionUsuario("Siguiente >"); _anteriorVista = CrearBotonPaginacionUsuario("< Anterior"); _siguienteVista.Click += (s, e) => CambiarPaginaVista(1); _anteriorVista.Click += (s, e) => CambiarPaginaVista(-1);
            paginacion.Controls.Add(_siguienteVista); paginacion.Controls.Add(_textoPaginaVista); paginacion.Controls.Add(_anteriorVista); pie.Controls.Add(_textoRegistrosVista); pie.Controls.Add(paginacion);
            lista.Controls.Add(_gridUsuariosVista); lista.Controls.Add(pie); lista.Controls.Add(barra);
            layout.Controls.Add(encabezado, 0, 0); layout.Controls.Add(tarjetas, 0, 1); layout.Controls.Add(lista, 0, 2); Controls.Add(layout);
            Text = "Usuarios"; BackColor = EstiloAplicacion.Fondo; MinimumSize = new Size(980, 620);
        }

        private string TextoUsuarioConectado()
        {
            if (_usuarioSesion == null) return "Sesión no disponible";
            return "Conectado: " + _usuarioSesion.NombreUsuario + " | Rol: " + _usuarioSesion.Rol;
        }

        // CREAR ELEMENTO VISUAL: CrearBotonUsuarioVista
        private static Button CrearBotonUsuarioVista(string texto, bool primario)
        {
            return new Button { Text = texto, Width = primario ? 132 : 100, Height = 30, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = primario ? EstiloAplicacion.Acento : Color.White, ForeColor = primario ? Color.White : EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
        }

        // CREAR ELEMENTO VISUAL: CrearBotonPaginacionUsuario
        private static Button CrearBotonPaginacionUsuario(string texto)
        {
            return new Button { Text = texto, Width = 96, Height = 32, Margin = new Padding(3, 0, 3, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F) };
        }

        // PRESENTAR DATOS: MostrarUsuariosVista
        private void MostrarUsuariosVista()
        {
            if (_gridUsuariosVista == null) return;
            int paginas = Math.Max(1, (int)Math.Ceiling(_usuarios.Count / (double)UsuariosPorPaginaVista)); _paginaVista = Math.Max(1, Math.Min(_paginaVista, paginas));
            List<Usuario> pagina = _usuarios.Skip((_paginaVista - 1) * UsuariosPorPaginaVista).Take(UsuariosPorPaginaVista).ToList();
            DataTable tabla = new DataTable(); tabla.Columns.Add("NombreUsuario"); tabla.Columns.Add("Rol"); tabla.Columns.Add("Estado");
            foreach (Usuario u in pagina) tabla.Rows.Add(u.NombreUsuario, u.Rol, u.Estado ? "Activo" : "Inactivo");
            _gridUsuariosVista.DataSource = tabla;
            _gridUsuariosVista.Columns["NombreUsuario"].HeaderText = "USUARIO"; _gridUsuariosVista.Columns["NombreUsuario"].FillWeight = 35;
            _gridUsuariosVista.Columns["Rol"].HeaderText = "ROL"; _gridUsuariosVista.Columns["Rol"].FillWeight = 35;
            _gridUsuariosVista.Columns["Estado"].HeaderText = "ESTADO"; _gridUsuariosVista.Columns["Estado"].FillWeight = 30;
            _gridUsuariosVista.ClearSelection(); for (int i = 0; i < pagina.Count; i++) _gridUsuariosVista.Rows[i].Tag = pagina[i];
            int desde = _usuarios.Count == 0 ? 0 : ((_paginaVista - 1) * UsuariosPorPaginaVista) + 1, hasta = Math.Min(_paginaVista * UsuariosPorPaginaVista, _usuarios.Count);
            _textoRegistrosVista.Text = "Mostrando " + desde + "-" + hasta + " de " + _usuarios.Count + " registros"; _textoPaginaVista.Text = _paginaVista + " de " + paginas;
            _anteriorVista.Enabled = _paginaVista > 1; _siguienteVista.Enabled = _paginaVista < paginas;
            _resumenUsuarios[0].Text = _usuariosTotales.Count.ToString("N0"); _resumenUsuarios[1].Text = _usuariosTotales.Count(u => u.Estado).ToString("N0"); _resumenUsuarios[2].Text = _usuariosTotales.Select(u => u.Rol).Where(r => !String.IsNullOrWhiteSpace(r)).Distinct(StringComparer.CurrentCultureIgnoreCase).Count().ToString("N0");
        }

        // BUSCAR: BuscarUsuariosVista
        private void BuscarUsuariosVista()
        {
            try { _usuarios = String.IsNullOrWhiteSpace(_buscarUsuariosVista.Text) ? _usuariosTotales : _negocio.Buscar(_buscarUsuariosVista.Text).OrderByDescending(u => u.IdUsuario).ToList(); _paginaVista = 1; MostrarUsuariosVista(); }
            catch (Exception ex) { MessageBox.Show("No se pudo buscar usuarios. " + ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // CAMBIAR PÁGINA: CambiarPaginaVista
        private void CambiarPaginaVista(int cambio) { _paginaVista += cambio; MostrarUsuariosVista(); }

        private void AbrirAdministracion(Usuario usuario)
        {
            Form dialogo = new Form { Text = usuario == null ? "Nuevo usuario" : "Editar usuario", ClientSize = new Size(500, 340), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            Label lUsuario = new Label { Text = "Nombre de usuario", Left = 24, Top = 76, AutoSize = true };
            TextBox nombre = new TextBox { Left = 24, Top = 98, Width = 452, MaxLength = 50, Text = usuario == null ? "" : usuario.NombreUsuario };
            Label lContrasena = new Label { Text = "Contraseña", Left = 24, Top = 140, AutoSize = true };
            TextBox contrasena = new TextBox { Left = 24, Top = 162, Width = 452, MaxLength = 100, UseSystemPasswordChar = true, Text = usuario == null ? "" : usuario.Contrasena };
            Label lRol = new Label { Text = "Rol", Left = 24, Top = 204, AutoSize = true };
            ComboBox rol = new ComboBox { Left = 24, Top = 226, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            rol.Items.AddRange(_negocio.ListarRoles().Cast<object>().ToArray());
            if (usuario != null) rol.SelectedItem = usuario.Rol;
            else rol.SelectedIndex = 0;
            CheckBox activo = new CheckBox { Text = "Usuario activo", Left = 300, Top = 228, AutoSize = true, Checked = usuario == null || usuario.Estado };
            Button guardar = new Button { Text = "Guardar", Left = 250, Top = 276, Width = 105, Height = 36 };
            Button cancelar = new Button { Text = "Cancelar", Left = 370, Top = 276, Width = 105, Height = 36, DialogResult = DialogResult.Cancel };
            guardar.Click += (s, e) =>
            {
                Usuario datos = new Usuario { IdUsuario = usuario == null ? 0 : usuario.IdUsuario, NombreUsuario = nombre.Text, Contrasena = contrasena.Text, Rol = Convert.ToString(rol.SelectedItem), Estado = activo.Checked };
                string error;
                try { error = usuario == null ? _negocio.Agregar(datos) : _negocio.Editar(datos); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!String.IsNullOrEmpty(error)) { MessageBox.Show(error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                _paginaVista = 1; CargarUsuarios(); dialogo.DialogResult = DialogResult.OK; dialogo.Close();
                AyudaForms.MtdInfo("Usuario guardado correctamente.");
            };
            dialogo.Controls.AddRange(new Control[] { lUsuario, nombre, lContrasena, contrasena, lRol, rol, activo, guardar, cancelar });
            dialogo.AcceptButton = guardar; dialogo.CancelButton = cancelar;
            EstiloAplicacion.AplicarDialogo(dialogo);
            dialogo.ShowDialog(this);
        }

        // ELIMINAR: EliminarUsuarioVista
        private void EliminarUsuarioVista()
        {
            if (_gridUsuariosVista.CurrentRow == null || !(_gridUsuariosVista.CurrentRow.Tag is Usuario)) return;
            Usuario usuario = (Usuario)_gridUsuariosVista.CurrentRow.Tag;
            if (AyudaForms.MtdConfirmacion("¿Deseas eliminar el usuario " + usuario.NombreUsuario + "?") != DialogResult.Yes) return;
            try { _negocio.Eliminar(usuario.IdUsuario); CargarUsuarios(); AyudaForms.MtdInfo("Usuario eliminado correctamente."); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void UsuariosForms_Load(object sender, EventArgs e)
        {
            cboRol.Items.Clear();
            cboRol.Items.AddRange(_negocio.ListarRoles().Cast<object>().ToArray());
            CargarUsuarios();
            Limpiar();
        }

        // CARGA DE DATOS: CargarUsuarios
        private void CargarUsuarios()
        {
            _usuariosTotales = _negocio.Listar().OrderByDescending(u => u.IdUsuario).ToList();
            _usuarios = _buscarUsuariosVista != null && !String.IsNullOrWhiteSpace(_buscarUsuariosVista.Text)
                ? _negocio.Buscar(_buscarUsuariosVista.Text).OrderByDescending(u => u.IdUsuario).ToList()
                : _usuariosTotales;
            int paginas = Math.Max(1, (int)Math.Ceiling(_usuarios.Count / (double)RegistrosPorPagina));
            if (_paginaActual >= paginas)
                _paginaActual = paginas - 1;

            MostrarPaginaActual();
            if (_gridUsuariosVista != null) MostrarUsuariosVista();
        }

        // PRESENTAR DATOS: MostrarPaginaActual
        private void MostrarPaginaActual()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = _usuarios
                .Skip(_paginaActual * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();

            if (dgvUsuarios.Columns["IdUsuario"] != null)
                dgvUsuarios.Columns["IdUsuario"].HeaderText = "Id";
            if (dgvUsuarios.Columns["NombreUsuario"] != null)
                dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Usuario";
            if (dgvUsuarios.Columns["Rol"] != null)
                dgvUsuarios.Columns["Rol"].HeaderText = "Rol";
            if (dgvUsuarios.Columns["Estado"] != null)
                dgvUsuarios.Columns["Estado"].HeaderText = "Activo";
            if (dgvUsuarios.Columns["Contrasena"] != null)
                dgvUsuarios.Columns["Contrasena"].Visible = false;

            int paginas = Math.Max(1, (int)Math.Ceiling(_usuarios.Count / (double)RegistrosPorPagina));
            lblPagina.Text = "Página " + (_paginaActual + 1) + " de " + paginas;
            lblTotal.Text = "Total: " + _usuarios.Count + " usuarios";
            btnPaginaAnterior.Enabled = _paginaActual > 0;
            btnPaginaSiguiente.Enabled = _paginaActual < paginas - 1;
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (_paginaActual <= 0) return;
            _paginaActual--;
            MostrarPaginaActual();
            Limpiar();
            dgvUsuarios.ClearSelection();
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(_usuarios.Count / (double)RegistrosPorPagina));
            if (_paginaActual >= paginas - 1) return;
            _paginaActual++;
            MostrarPaginaActual();
            Limpiar();
            dgvUsuarios.ClearSelection();
        }

        // BUSCAR: btnBuscarUsuario_Click
        private void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                _usuarios = _negocio.Buscar(txtBuscarUsuario.Text)
                    .OrderBy(u => u.IdUsuario)
                    .ToList();
                _paginaActual = 0;
                MostrarPaginaActual();
                Limpiar();
                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo buscar usuarios. " + ex.Message,
                    "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BUSCAR: txtBuscarUsuario_KeyDown
        private void txtBuscarUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            btnBuscarUsuario_Click(sender, e);
        }

        // LIMPIAR: Limpiar
        private void Limpiar()
        {
            _idSeleccionado = 0;
            txtNombreUsuario.Clear();
            txtContrasena.Clear();
            cboRol.SelectedIndex = -1;
            rdbActivo.Checked = true;
            txtNombreUsuario.Focus();
        }

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
            dgvUsuarios.ClearSelection();
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario u = new Usuario
            {
                IdUsuario = _idSeleccionado,
                NombreUsuario = txtNombreUsuario.Text,
                Contrasena = txtContrasena.Text,
                Rol = cboRol.Text,
                Estado = rdbActivo.Checked
            };

            string error;
            if (_idSeleccionado == 0)
            {
                try
                {
                    error = _negocio.Agregar(u);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo agregar el usuario. " + ex.Message,
                        "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                try
                {
                    error = _negocio.Editar(u);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo actualizar el usuario. " + ex.Message,
                        "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool agregado = _idSeleccionado == 0;
            if (agregado)
                _paginaActual = int.MaxValue;
            CargarUsuarios();
            Limpiar();
            AyudaForms.MtdInfo(agregado ? "Usuario agregado." : "Usuario actualizado.");
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Usuario u = (Usuario)dgvUsuarios.Rows[e.RowIndex].DataBoundItem;
            _idSeleccionado = u.IdUsuario;
            txtNombreUsuario.Text = u.NombreUsuario;
            txtContrasena.Text = u.Contrasena;
            cboRol.SelectedItem = u.Rol;
            rdbActivo.Checked = u.Estado;
            rdbInactivo.Checked = !u.Estado;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Form contenedor = ((Control)sender).FindForm();
            if (contenedor != null && contenedor != this) contenedor.Close();
        }

        // ELIMINAR: btnEliminar_Click
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona primero un usuario de la tabla.", "Usuarios",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = AyudaForms.MtdConfirmacion(
                "¿Deseas eliminar el usuario seleccionado? Si está asignado a un empleado, SQL Server no permitirá borrarlo.");
            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                _negocio.Eliminar(_idSeleccionado);
                CargarUsuarios();
                Limpiar();
                AyudaForms.MtdInfo("Usuario eliminado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // LIMPIAR: btnLimpiarPruebas_Click
        private void btnLimpiarPruebas_Click(object sender, EventArgs e)
        {
            // El botón limpia los campos; la tabla y sus totales permanecen visibles.
            Limpiar();
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        // GUARDAR: btnGuardar_Click_1
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            btnGuardar_Click(sender, e);
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            btnCerrar_Click(sender, e);
        }
    }
}
