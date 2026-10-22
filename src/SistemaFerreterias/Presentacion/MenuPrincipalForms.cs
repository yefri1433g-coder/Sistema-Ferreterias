using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class MenuPrincipalForms : Form
    {
        private readonly Panel contenido = new Panel();
        private readonly Label titulo = new Label();
        private readonly System.Collections.Generic.Dictionary<string, Button> botonesNavegacion = new System.Collections.Generic.Dictionary<string, Button>();
        private Form moduloActual;

        public MenuPrincipalForms()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Controls.Clear();
            Text = "Ferretería | Gestión multisucursal";
            MinimumSize = new Size(1050, 680);
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(233, 239, 241);
            Font = new Font("Segoe UI", 9F);

            Panel barraLateral = new Panel { Dock = DockStyle.Left, Width = 236, BackColor = Color.FromArgb(16, 31, 43), Padding = new Padding(14, 18, 14, 12) };
            Panel separador = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.FromArgb(41, 57, 69) };
            FlowLayoutPanel navegacion = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(0, 14, 0, 0), BackColor = Color.FromArgb(16, 31, 43) };
            // Se agrega primero para que el Dock Fill respete los espacios del encabezado y del botón de salida.
            barraLateral.Controls.Add(navegacion);
            barraLateral.Controls.Add(separador);
            Panel marca = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.FromArgb(16, 31, 43), Padding = new Padding(8) };
            PictureBox logo = new PictureBox
            {
                Location = new Point(10, 16),
                Size = new Size(38, 38),
                BackColor = Color.FromArgb(43, 211, 154)
            };
            logo.Resize += (s, e) =>
            {
                using (GraphicsPath rutaLogo = new GraphicsPath())
                {
                    rutaLogo.AddEllipse(0, 0, logo.Width, logo.Height);
                    Region anterior = logo.Region;
                    logo.Region = new Region(rutaLogo);
                    if (anterior != null) anterior.Dispose();
                }
            };
            Label logoMarca = new Label { Text = "\u25B2", Location = new Point(10, 16), Size = new Size(38, 38), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(12, 82, 76) };
            Label nombreSistema = new Label
            {
                Text = "FERRETERÍA\nCENTRAL",
                Location = new Point(56, 13),
                Size = new Size(130, 44),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            marca.Controls.Add(logo);
            marca.Controls.Add(logoMarca);
            marca.Controls.Add(nombreSistema);
            barraLateral.Controls.Add(marca);
            Label seccion = new Label { Text = "MENÚ PRINCIPAL", Width = 180, Height = 24, ForeColor = Color.FromArgb(113, 133, 145), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Padding = new Padding(8, 5, 0, 0), Margin = new Padding(0, 2, 0, 4) };
            navegacion.Controls.Add(seccion);
            AddNav(navegacion, "Dashboard", () => AbrirModulo(new DashboardForms(), "Dashboard"));
            AddNav(navegacion, "Sucursales", () => AbrirModulo(new SucursalesForms(), "Sucursales"));
            AddNav(navegacion, "Proveedores", () => AbrirModulo(new ProveedoresForms(), "Proveedores"));
            AddNav(navegacion, "Productos", () => AbrirModulo(new ProductosForms(), "Productos"));
            AddNav(navegacion, "Categorías", () => AbrirModulo(new CategoriasForms(), "Categorías"));
            AddNav(navegacion, "Compras", () => AbrirModulo(new ComprasForms(), "Compras"));
            AddNav(navegacion, "Inventarios", () => AbrirModulo(new InventariosForms(), "Inventarios"));
            AddNav(navegacion, "Stock", () => AbrirModulo(new StockForms(), "Stock"));
            AddNav(navegacion, "Kardex", () => AbrirModulo(new KardexForms(), "Kardex"));
            AddNav(navegacion, "Traslados", () => AbrirModulo(new TrasladosForms(), "Traslados"));
            AddNav(navegacion, "Clientes", () => AbrirModulo(new ClientesForms(), "Clientes"));
            AddNav(navegacion, "Empleados", () => AbrirModulo(new EmpleadosForms(), "Empleados"));
            AddNav(navegacion, "Ventas", () => AbrirModulo(new VentasForms(), "Ventas"));
            AddNav(navegacion, "Pagos", () => AbrirModulo(new PagosForms(), "Pagos"));
            AddNav(navegacion, "Usuarios", () => AbrirModulo(new UsuariosForms(), "Usuarios"));
            Button salir = new Button { Text = "Cerrar sesión", Dock = DockStyle.Bottom, Height = 40, FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(180, 193, 201), BackColor = Color.FromArgb(16, 31, 43), Cursor = Cursors.Hand };
            salir.FlatAppearance.BorderSize = 0; salir.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 247); salir.Click += (s, e) => Application.Exit(); barraLateral.Controls.Add(salir);

            Panel area = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(233, 239, 241), Padding = new Padding(12) };
            Panel cabecera = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.FromArgb(233, 239, 241) };
            titulo.Text = "Dashboard"; titulo.Dock = DockStyle.Fill; titulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold); titulo.ForeColor = Color.FromArgb(28, 28, 30); titulo.TextAlign = ContentAlignment.MiddleLeft;
            cabecera.Controls.Add(titulo);
            contenido.Dock = DockStyle.Fill; contenido.BackColor = Color.FromArgb(241, 246, 253);
            area.Controls.Add(contenido); area.Controls.Add(cabecera);
            Controls.Add(area); Controls.Add(barraLateral);
            AbrirModulo(new DashboardForms(), "Dashboard");
        }

        private void AddNav(Control parent, string nombre, Action accion)
        {
            Button boton = new Button { Text = nombre, Width = 180, Height = 36, TextAlign = ContentAlignment.MiddleLeft, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(16, 31, 43), ForeColor = Color.FromArgb(180, 193, 201), Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand, Margin = new Padding(0, 2, 0, 2), Padding = new Padding(10, 0, 0, 0) };
            boton.FlatAppearance.BorderSize = 0; boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(24, 59, 67);
            boton.Click += (s, e) => accion(); parent.Controls.Add(boton); botonesNavegacion[nombre] = boton;
        }

        private void Navegar(string nombre)
        {
            switch (nombre)
            {
                case "Sucursales": AbrirModulo(new SucursalesForms(), nombre); break;
                case "Productos": AbrirModulo(new ProductosForms(), nombre); break;
                case "Categorías": AbrirModulo(new CategoriasForms(), nombre); break;
                case "Compras": AbrirModulo(new ComprasForms(), nombre); break;
                case "Dashboard": AbrirModulo(new DashboardForms(), nombre); break;
                case "Proveedores": AbrirModulo(new ProveedoresForms(), nombre); break;
                case "Inventarios": AbrirModulo(new InventariosForms(), nombre); break;
                case "Stock": AbrirModulo(new StockForms(), nombre); break;
                case "Kardex": AbrirModulo(new KardexForms(), nombre); break;
                case "Traslados": AbrirModulo(new TrasladosForms(), nombre); break;
                case "Clientes": AbrirModulo(new ClientesForms(), nombre); break;
                case "Empleados": AbrirModulo(new EmpleadosForms(), nombre); break;
                case "Ventas": AbrirModulo(new VentasForms(), nombre); break;
                case "Pagos": AbrirModulo(new PagosForms(), nombre); break;
                case "Usuarios": AbrirModulo(new UsuariosForms(), nombre); break;
            }
        }

        private void AbrirModulo(Form modulo, string nombre)
        {
            CerrarModulo(); contenido.Controls.Clear();
            bool dashboard = modulo is DashboardForms;
            bool pantallaGestion = modulo is PantallaGestionBase;
            bool proveedores = modulo is ProveedoresForms;
            bool inventarios = modulo is InventariosForms;
            bool stock = modulo is StockForms;
            bool kardex = modulo is KardexForms;
            bool traslados = modulo is TrasladosForms;
            bool clientes = modulo is ClientesForms;
            bool empleados = modulo is EmpleadosForms;
            bool ventas = modulo is VentasForms;
            bool pagos = modulo is PagosForms;
            bool usuarios = modulo is UsuariosForms;
            if (titulo.Parent != null) { titulo.Parent.Visible = !dashboard && !proveedores && !inventarios && !stock && !kardex && !traslados && !clientes && !empleados && !ventas && !pagos && !usuarios; titulo.Parent.Height = (dashboard || proveedores || inventarios || stock || kardex || traslados || clientes || empleados || ventas || pagos || usuarios) ? 0 : 48; }
            contenido.Padding = dashboard ? new Padding(12) : Padding.Empty;
            titulo.Text = pantallaGestion ? "Inicio  /  " + nombre : nombre;
            titulo.Font = new Font("Segoe UI", pantallaGestion ? 9F : 20F, pantallaGestion ? FontStyle.Regular : FontStyle.Bold);
            titulo.ForeColor = pantallaGestion ? Color.FromArgb(129, 145, 154) : Color.FromArgb(28, 28, 30);
            MarcarSeleccionado(nombre);
            DashboardForms panelDashboard = modulo as DashboardForms;
            if (panelDashboard != null) panelDashboard.NavegacionSolicitada += Navegar;
            moduloActual = modulo; modulo.TopLevel = false; modulo.FormBorderStyle = FormBorderStyle.None; modulo.Dock = DockStyle.Fill;
            EstiloAplicacion.Aplicar(modulo);
            contenido.Controls.Add(modulo); modulo.Show(); modulo.BringToFront();
        }

        private void CerrarModulo()
        {
            if (moduloActual != null) { contenido.Controls.Remove(moduloActual); moduloActual.Close(); moduloActual.Dispose(); moduloActual = null; }
        }

        private void MarcarSeleccionado(string nombre)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, Button> par in botonesNavegacion)
            {
                bool activo = par.Key == nombre;
                par.Value.BackColor = activo ? Color.FromArgb(24, 59, 67) : Color.FromArgb(16, 31, 43);
                par.Value.ForeColor = activo ? Color.FromArgb(239, 255, 249) : Color.FromArgb(180, 193, 201);
                par.Value.Font = new Font("Segoe UI", 9.5F, activo ? FontStyle.Bold : FontStyle.Regular);
                par.Value.FlatAppearance.MouseOverBackColor = activo ? Color.FromArgb(24, 59, 67) : Color.FromArgb(41, 57, 69);
            }
        }

        private void ConstruirMenuPrincipal() { }
        private void AgregarTarjeta(Control tarjeta, int columna, int fila) { }
        private FontAwesome.Sharp.IconButton CrearTarjeta(string texto, FontAwesome.Sharp.IconChar icono, string modulo) { return null; }
        private FontAwesome.Sharp.IconButton CrearTarjeta(string texto, FontAwesome.Sharp.IconChar icono, Action accion) { return null; }
        private void VolverAlMenu() { AbrirModulo(new DashboardForms(), "Dashboard"); }
        private void btnMenuInventarios_Click(object sender, EventArgs e) { Navegar("Inventarios"); }
        private void btnMenuStock_Click(object sender, EventArgs e) { Navegar("Stock"); }
        private void btnMenuKardex_Click(object sender, EventArgs e) { Navegar("Kardex"); }
        private void btnMenuProveedores_Click(object sender, EventArgs e) { Navegar("Proveedores"); }
        private void btnMenuTraslados_Click(object sender, EventArgs e) { Navegar("Traslados"); }
        private void btnMenuDashboard_Click(object sender, EventArgs e) { Navegar("Dashboard"); }
        private void btnSalir_Click(object sender, EventArgs e) { Application.Exit(); }
        private void btnIcono_Click(object sender, EventArgs e) { }
        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e) { Navegar("Usuarios"); }
        private void clientesToolStripMenuItem_Click(object sender, EventArgs e) { Navegar("Clientes"); }
        private void ventasToolStripMenuItem_Click(object sender, EventArgs e) { Navegar("Ventas"); }
        private void pagosToolStripMenuItem_Click(object sender, EventArgs e) { Navegar("Pagos"); }
        private void lblSubtitulo_Click(object sender, EventArgs e) { }
    }
}


