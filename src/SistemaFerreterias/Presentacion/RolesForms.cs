using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE RolesForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public class RolesForms : Form
    {
        private readonly RolNegocio _negocio = new RolNegocio();
        private readonly Usuario _usuarioSesion;
        private readonly ListBox _listaRoles = new ListBox();
        private readonly TextBox _nombre = new TextBox();
        private readonly CheckBox _activo = new CheckBox();
        private readonly CheckedListBox _modulos = new CheckedListBox();
        private readonly Label _estado = new Label();
        private Rol _seleccionado;

        private static readonly string[] Modulos =
        {
            "Dashboard", "Sucursales", "Proveedores", "Productos", "Categorías",
            "Compras", "Inventarios", "Stock", "Kardex", "Traslados", "Clientes",
            "Empleados", "Ventas", "Pagos", "Usuarios", "Roles y accesos"
        };

        public RolesForms()
            : this(null)
        {
        }

        public RolesForms(Usuario usuarioSesion)
        {
            _usuarioSesion = usuarioSesion;
            Text = "Roles y accesos";
            BackColor = EstiloAplicacion.Fondo;
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(780, 570);
            Dock = DockStyle.Fill;

            TableLayoutPanel principal = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 2, RowCount = 2, BackColor = BackColor };
            principal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            principal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12, 4, 14, 4) };
            encabezado.Controls.Add(new Label { Text = "Roles y accesos", Dock = DockStyle.Left, Width = 250, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label
            {
                Text = TextoUsuarioConectado(),
                Dock = DockStyle.Right,
                Width = 360,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = EstiloAplicacion.Tinta,
                AutoEllipsis = true
            });
            Panel panelLista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            panelLista.Controls.Add(new Label { Text = "Roles", Dock = DockStyle.Top, Height = 36, Font = new Font("Segoe UI", 15F, FontStyle.Bold) });
            _listaRoles.Dock = DockStyle.Fill;
            _listaRoles.SelectedIndexChanged += ListaRoles_Changed;
            panelLista.Controls.Add(_listaRoles);

            Panel editor = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18) };
            Label titulo = new Label { Text = "Configurar rol y módulos", Dock = DockStyle.Top, Height = 38, Font = new Font("Segoe UI", 15F, FontStyle.Bold) };
            editor.Controls.Add(titulo);
            Label etiquetaNombre = new Label { Text = "Nombre del rol", Left = 18, Top = 54, AutoSize = true };
            _nombre.SetBounds(18, 76, 300, 30);
            _activo.Text = "Rol activo"; _activo.SetBounds(330, 78, 130, 24);
            Label etiquetaModulos = new Label { Text = "Módulos permitidos", Left = 18, Top = 120, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            _modulos.SetBounds(18, 146, 440, 270); _modulos.CheckOnClick = true;
            foreach (string modulo in Modulos) _modulos.Items.Add(modulo);
            Button nuevo = CrearBoton("Nuevo", 18, 438);
            Button guardar = CrearBoton("Guardar cambios", 124, 438);
            Button eliminar = CrearBoton("Eliminar", 280, 438);
            _estado.SetBounds(18, 485, 440, 46); _estado.ForeColor = Color.FromArgb(93, 111, 121);
            nuevo.Click += (s, e) => NuevoRol();
            guardar.Click += (s, e) => GuardarRol();
            eliminar.Click += (s, e) => EliminarRol();
            editor.Controls.AddRange(new Control[] { etiquetaNombre, _nombre, _activo, etiquetaModulos, _modulos, nuevo, guardar, eliminar, _estado });
            principal.Controls.Add(encabezado, 0, 0); principal.SetColumnSpan(encabezado, 2);
            principal.Controls.Add(panelLista, 0, 1); principal.Controls.Add(editor, 1, 1); Controls.Add(principal);
            CargarRoles();
        }

        private string TextoUsuarioConectado()
        {
            if (_usuarioSesion == null) return "Sesión no disponible";
            return "Conectado: " + _usuarioSesion.NombreUsuario + " | Rol: " + _usuarioSesion.Rol;
        }

        // CREAR ELEMENTO VISUAL: CrearBoton
        private static Button CrearBoton(string texto, int x, int y)
        {
            return new Button { Text = texto, Left = x, Top = y, Width = texto == "Guardar cambios" ? 146 : 108, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = EstiloAplicacion.Acento, ForeColor = Color.White, Cursor = Cursors.Hand };
        }

        // CARGA DE DATOS: CargarRoles
        private void CargarRoles()
        {
            try
            {
                int codigo = _seleccionado == null ? 0 : _seleccionado.CodigoRol;
                _listaRoles.SelectedIndexChanged -= ListaRoles_Changed;
                List<Rol> roles = _negocio.Listar(false);
                _listaRoles.DataSource = null;
                _listaRoles.DataSource = roles;
                _listaRoles.DisplayMember = "NombreRol";
                _listaRoles.SelectedItem = roles.FirstOrDefault(r => r.CodigoRol == codigo);
                _listaRoles.SelectedIndexChanged += ListaRoles_Changed;
                if (_listaRoles.SelectedItem == null) NuevoRol(); else CargarSeleccion();
            }
            catch (Exception ex) { _estado.Text = "No se pudieron cargar los roles: " + ex.Message; }
        }

        // CONSULTAR: ListaRoles_Changed
        private void ListaRoles_Changed(object sender, EventArgs e) { CargarSeleccion(); }

        // CARGA DE DATOS: CargarSeleccion
        private void CargarSeleccion()
        {
            Rol rol = _listaRoles.SelectedItem as Rol;
            if (rol == null) return;
            try
            {
                _seleccionado = _negocio.Obtener(rol.NombreRol);
                _nombre.Text = _seleccionado.NombreRol;
                _activo.Checked = _seleccionado.Estado;
                for (int i = 0; i < _modulos.Items.Count; i++)
                    _modulos.SetItemChecked(i, _seleccionado.Modulos.Contains(Convert.ToString(_modulos.Items[i])));
                _estado.Text = "";
            }
            catch (Exception ex) { _estado.Text = "No se pudo cargar el rol: " + ex.Message; }
        }

        // AGREGAR: NuevoRol
        private void NuevoRol()
        {
            _listaRoles.ClearSelected();
            _seleccionado = null;
            _nombre.Clear(); _activo.Checked = true;
            for (int i = 0; i < _modulos.Items.Count; i++) _modulos.SetItemChecked(i, false);
            _estado.Text = "Define el nombre y los módulos y guarda el rol.";
        }

        // GUARDAR: GuardarRol
        private void GuardarRol()
        {
            try
            {
                Rol rol = new Rol
                {
                    CodigoRol = _seleccionado == null ? 0 : _seleccionado.CodigoRol,
                    NombreRol = _nombre.Text,
                    Estado = _activo.Checked,
                    Modulos = _modulos.CheckedItems.Cast<object>().Select(item => Convert.ToString(item)).ToList()
                };
                _negocio.Guardar(rol);
                _seleccionado = rol;
                _estado.Text = "Rol y accesos guardados.";
                AyudaForms.MtdInfo("Rol y accesos guardados.");
                CargarRoles();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles y accesos", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // ELIMINAR: EliminarRol
        private void EliminarRol()
        {
            if (_seleccionado == null) return;
            if (AyudaForms.MtdConfirmacion("¿Eliminar el rol " + _seleccionado.NombreRol + "?") != DialogResult.Yes) return;
            try { _negocio.Eliminar(_seleccionado.CodigoRol); _seleccionado = null; CargarRoles(); AyudaForms.MtdInfo("Rol eliminado correctamente."); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles y accesos", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
