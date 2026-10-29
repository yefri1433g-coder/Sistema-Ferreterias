using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Entidad;
using Negocio;

namespace Presentacion
{
    public partial class MenuPrincipalForms : Form
    {
        private readonly Dictionary<string, Button> botonesNavegacion = new Dictionary<string, Button>();
        private readonly HashSet<string> _modulosPermitidos;
        private readonly Usuario _usuario;
        private Form moduloActual;
        private string moduloActualNombre;
        public bool CierreSesionSolicitado { get; private set; }

        public MenuPrincipalForms() : this(null) { }

        public MenuPrincipalForms(Usuario usuario)
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            _usuario = usuario;
            // Se conservan los permisos aprobados en el Documento 06.
            _modulosPermitidos = usuario == null
                ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(new RolNegocio().ModulosPermitidos(usuario.Rol), StringComparer.OrdinalIgnoreCase);
            lblSubtitulo.Text = usuario == null ? "" : usuario.NombreUsuario + " | " + usuario.Rol;
            string nombre = usuario == null ? "Invitado" : usuario.NombreUsuario;
            btnPerfil.Text = ObtenerIniciales(nombre);
            btnPerfil_Resize(btnPerfil, EventArgs.Empty);
            btnPerfil.AccessibleName = "Perfil de " + nombre;
            perfilUsuario.Text = "Usuario: " + nombre;
            perfilRol.Text = "Rol: " + (usuario == null ? "Sin sesión" : usuario.Rol);
            foreach (Control control in navegacion.Controls)
            {
                Button boton = control as Button;
                if (boton == null) continue;
                string modulo = Convert.ToString(boton.Tag);
                botonesNavegacion[modulo] = boton;
                boton.Visible = _modulosPermitidos.Contains(modulo);
            }
            seccionInicio.Visible = _modulosPermitidos.Contains("Dashboard");
            seccionOperaciones.Visible = _modulosPermitidos.Overlaps(new[] { "Ventas", "Compras", "Pagos", "Traslados" });
            seccionCatalogos.Visible = _modulosPermitidos.Overlaps(new[] { "Productos", "Categorías", "Clientes", "Proveedores" });
            seccionExistencias.Visible = _modulosPermitidos.Overlaps(new[] { "Inventarios", "Stock", "Kardex" });
            seccionAdministracion.Visible = _modulosPermitidos.Overlaps(new[] { "Sucursales", "Empleados", "Usuarios", "Roles y accesos" });
            AjustarNavegacion();
            Shown += (s, e) => AjustarNavegacion();
            EstiloAplicacion.AplicarPaleta(this);
            if (_modulosPermitidos.Contains("Dashboard")) Navegar("Dashboard");
            else
                foreach (string modulo in botonesNavegacion.Keys)
                    if (_modulosPermitidos.Contains(modulo)) { Navegar(modulo); break; }
        }

        private void btnNavegacion_Click(object sender, EventArgs e)
        {
            Navegar(Convert.ToString(((Control)sender).Tag));
        }

        private void Navegar(string nombre)
        {
            if (_modulosPermitidos == null || !_modulosPermitidos.Contains(nombre))
            {
                MessageBox.Show("Tu rol no tiene acceso a este módulo.", "Acceso restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Un segundo clic conserva el formulario y los datos consultados.
            if (moduloActual != null && !moduloActual.IsDisposed && moduloActualNombre == nombre)
            {
                moduloActual.BringToFront();
                return;
            }
            switch (nombre)
            {
                case "Dashboard": AbrirModulo(new DashboardForms(), nombre); break;
                case "Sucursales": AbrirModulo(new SucursalesForms(), nombre); break;
                case "Proveedores": AbrirModulo(new ProveedoresForms(), nombre); break;
                case "Productos": AbrirModulo(new ProductosForms(), nombre); break;
                case "Categorías": AbrirModulo(new CategoriasForms(), nombre); break;
                case "Compras": AbrirModulo(new ComprasForms(), nombre); break;
                case "Inventarios": AbrirModulo(new InventariosForms(), nombre); break;
                case "Stock": AbrirModulo(new StockForms(), nombre); break;
                case "Kardex": AbrirModulo(new KardexForms(), nombre); break;
                case "Traslados": AbrirModulo(new TrasladosForms(), nombre); break;
                case "Clientes": AbrirModulo(new ClientesForms(), nombre); break;
                case "Empleados": AbrirModulo(new EmpleadosForms(), nombre); break;
                case "Ventas": AbrirModulo(new VentasForms(), nombre); break;
                case "Pagos": AbrirModulo(new PagosForms(), nombre); break;
                case "Usuarios": AbrirModulo(new UsuariosForms(_usuario), nombre); break;
                case "Roles y accesos": AbrirModulo(new RolesForms(_usuario), nombre); break;
            }
        }

        private void AbrirModulo(Form modulo, string nombre)
        {
            CerrarModulo();
            moduloActual = modulo;
            moduloActualNombre = nombre;
            titulo.Text = "Inicio  /  " + nombre;
            cabecera.Visible = modulo is PantallaGestionBase || modulo is RolesForms;
            foreach (KeyValuePair<string, Button> opcion in botonesNavegacion)
            {
                bool activo = opcion.Key == nombre;
                opcion.Value.BackColor = activo ? EstiloAplicacion.Acento : EstiloAplicacion.Marino;
                opcion.Value.ForeColor = activo ? Color.White : Color.FromArgb(180, 193, 201);
            }
            DashboardForms dashboard = modulo as DashboardForms;
            if (dashboard != null)
            {
                dashboard.NavegacionSolicitada += Navegar;
                dashboard.ConfigurarAccesosRapidos(_modulosPermitidos);
            }
            modulo.TopLevel = false;
            modulo.FormBorderStyle = FormBorderStyle.None;
            EstiloAplicacion.Aplicar(modulo);
            // El contenedor permite desplazarse si el formulario requiere más espacio.
            modulo.Dock = DockStyle.None;
            contenido.Desplazamiento = Point.Empty;
            contenido.Superficie.Controls.Add(modulo);
            AjustarModulo();
            modulo.Show();
            // Show aplica la escala del formulario; ajustar después evita barras innecesarias.
            AjustarModulo();
            modulo.BringToFront();
        }

        private void contenido_Resize(object sender, EventArgs e) { AjustarModulo(); }

        private void vistaNavegacion_Resize(object sender, EventArgs e) { AjustarNavegacion(); }

        private void AjustarNavegacion()
        {
            if (navegacion == null || vistaNavegacion == null) return;
            navegacion.Width = Math.Max(1, vistaNavegacion.ClientSize.Width - 12);
            navegacion.Height = navegacion.GetPreferredSize(new Size(navegacion.Width, 0)).Height;
            vistaNavegacion.TamanoContenido = navegacion.Size;
        }

        private void AjustarModulo()
        {
            if (moduloActual == null || moduloActual.IsDisposed) return;
            contenido.TamanoContenido = moduloActual.MinimumSize;
            moduloActual.Size = new Size(
                Math.Max(moduloActual.MinimumSize.Width, contenido.AreaVisible.Width),
                Math.Max(moduloActual.MinimumSize.Height, contenido.AreaVisible.Height));
            contenido.TamanoContenido = moduloActual.Size;
        }

        private void CerrarModulo()
        {
            if (moduloActual == null) return;
            contenido.Superficie.Controls.Remove(moduloActual);
            moduloActual.Close();
            moduloActual.Dispose();
            moduloActual = null;
            moduloActualNombre = null;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CerrarModulo();
            base.OnFormClosed(e);
        }

        private void btnMenuInventarios_Click(object sender, EventArgs e) { Navegar("Inventarios"); }
        private void btnMenuStock_Click(object sender, EventArgs e) { Navegar("Stock"); }
        private void btnMenuKardex_Click(object sender, EventArgs e) { Navegar("Kardex"); }
        private void btnMenuProveedores_Click(object sender, EventArgs e) { Navegar("Proveedores"); }
        private void btnMenuTraslados_Click(object sender, EventArgs e) { Navegar("Traslados"); }
        private void btnMenuDashboard_Click(object sender, EventArgs e) { Navegar("Dashboard"); }
        private void btnSalir_Click(object sender, EventArgs e) { Application.Exit(); }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            CierreSesionSolicitado = true;
            Close();
        }

        private static string ObtenerIniciales(string nombre)
        {
            string[] partes = (nombre ?? "").Split(new[] { ' ', '.', '_', '-', '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return "?";
            if (partes.Length > 1) return (partes[0].Substring(0, 1) + partes[1].Substring(0, 1)).ToUpperInvariant();
            return partes[0].Substring(0, Math.Min(2, partes[0].Length)).ToUpperInvariant();
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            menuPerfil.Show(btnPerfil, new Point(btnPerfil.Width, btnPerfil.Height), ToolStripDropDownDirection.BelowLeft);
        }

        private void btnPerfil_Resize(object sender, EventArgs e)
        {
            using (GraphicsPath circulo = new GraphicsPath())
            {
                circulo.AddEllipse(0, 0, btnPerfil.Width, btnPerfil.Height);
                Region anterior = btnPerfil.Region;
                btnPerfil.Region = new Region(circulo);
                if (anterior != null) anterior.Dispose();
            }
        }

        private void btnPerfil_Paint(object sender, PaintEventArgs e)
        {
            using (Brush fondo = new SolidBrush(btnPerfil.Parent.BackColor))
                e.Graphics.FillRectangle(fondo, btnPerfil.ClientRectangle);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Brush naranja = new SolidBrush(btnPerfil.BackColor))
                e.Graphics.FillEllipse(naranja, 1, 1, btnPerfil.Width - 2, btnPerfil.Height - 2);
            TextRenderer.DrawText(e.Graphics, btnPerfil.Text, btnPerfil.Font, btnPerfil.ClientRectangle,
                Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }
}
