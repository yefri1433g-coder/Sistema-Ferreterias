using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class DashboardForms : Form
    {
        private readonly DashboardNegocio _dashboardNegocio = new DashboardNegocio();
        private PanelGrafico graficoVentas, graficoInventario;
        private FlowLayoutPanel listaActividad;
        private Panel panelAccesosRapidos;
        public event Action<string> NavegacionSolicitada;

        private readonly Color fondo = Color.FromArgb(239, 245, 244);
        private readonly Color tinta = Color.FromArgb(18, 42, 53);
        private readonly Color verde = Color.FromArgb(30, 190, 140);

        public DashboardForms()
        {
            ConstruirDashboard();
            Load += DashboardForms_Load;
        }

        private void ConstruirDashboard()
        {
            Text = "Dashboard";
            AutoScaleMode = AutoScaleMode.None;
            BackColor = fondo;
            Font = new Font("Segoe UI", 9F);
            Dock = DockStyle.Fill;

            lblValorProductos = new Label();
            lblValorSucursales = new Label();
            lblValorProveedores = new Label();
            lblValorStockBajo = new Label();
            lblValorPendientes = new Label();
            btnActualizar = new FontAwesome.Sharp.IconButton();
            btnCerrar = new FontAwesome.Sharp.IconButton();

            lblValorProductos = new Label();
            lblValorSucursales = new Label();
            lblValorProveedores = new Label();
            lblValorStockBajo = new Label();
            lblValorPendientes = new Label();
            graficoVentas = new PanelGrafico { Dock = DockStyle.Fill, Tipo = PanelGrafico.TipoVisualizacion.Linea, BackColor = Color.White };
            graficoInventario = new PanelGrafico { Dock = DockStyle.Fill, Tipo = PanelGrafico.TipoVisualizacion.Anillo, BackColor = Color.White };
            listaActividad = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.White, Padding = new Padding(4, 0, 4, 0) };
            listaActividad.ClientSizeChanged += (s, e) =>
            {
                foreach (Control fila in listaActividad.Controls)
                    if (fila is Panel) fila.Width = Math.Max(300, listaActividad.ClientSize.Width - 12);
            };
            btnActualizar = new FontAwesome.Sharp.IconButton();
            btnCerrar = new FontAwesome.Sharp.IconButton();

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.None, Anchor = AnchorStyles.Top,
                ColumnCount = 1, RowCount = 4,
                Padding = new Padding(16, 8, 16, 14), BackColor = fondo,
                Width = Math.Min(1450, Math.Max(800, ClientSize.Width)), Height = ClientSize.Height
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
            layout.Controls.Add(CrearEncabezado(), 0, 0);
            layout.Controls.Add(CrearIndicadores(), 0, 1);
            layout.Controls.Add(CrearFilaGraficas(), 0, 2);
            layout.Controls.Add(CrearFilaInferior(), 0, 3);
            Controls.Add(layout);
            EventHandler centrar = (s, e) =>
            {
                layout.Width = Math.Min(1450, Math.Max(800, ClientSize.Width));
                layout.Height = ClientSize.Height;
                layout.Left = Math.Max(0, (ClientSize.Width - layout.Width) / 2);
            };
            SizeChanged += centrar;
            centrar(this, EventArgs.Empty);
        }

        private Control CrearEncabezado()
        {
            TableLayoutPanel encabezado = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = fondo };
            encabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76F));
            encabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            FlowLayoutPanel textos = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.Transparent, Padding = new Padding(2, 0, 0, 0) };
            textos.Controls.Add(new Label { Text = "Inicio  /  Panel principal", AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(105, 132, 136), Margin = new Padding(0, 0, 0, 2) });
            textos.Controls.Add(new Label { Text = SaludoActual(), AutoSize = true, Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = tinta, Margin = new Padding(0) });
            textos.Controls.Add(new Label { Text = "Aquí tienes el resumen de tu ferretería.", AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(111, 132, 138), Margin = new Padding(0) });
            Panel fechaPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            Label fecha = new Label { Text = "▦  " + DateTime.Today.ToString("dd MMM yyyy"), Size = new Size(138, 32), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(57, 81, 86), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(0, 18) };
            fechaPanel.Controls.Add(fecha);
            fechaPanel.Resize += (s, e) => fecha.Left = Math.Max(0, fechaPanel.ClientSize.Width - fecha.Width - 4);
            Redondear(fecha, 9);
            encabezado.Controls.Add(textos, 0, 0);
            encabezado.Controls.Add(fechaPanel, 1, 0);
            return encabezado;
        }

        private static string SaludoActual()
        {
            int hora = DateTime.Now.Hour;
            if (hora < 12) return "Buenos días";
            if (hora < 19) return "Buenas tardes";
            return "Buenas noches";
        }

        private Control CrearIndicadores()
        {
            TableLayoutPanel fila = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1, BackColor = fondo };
            for (int i = 0; i < 4; i++) fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            fila.Controls.Add(CrearTarjetaIndicador("VENTAS DE HOY", lblValorSucursales, "↗", verde, ""), 0, 0);
            fila.Controls.Add(CrearTarjetaIndicador("PRODUCTOS", lblValorProductos, "▤", Color.FromArgb(242, 164, 74), ""), 1, 0);
            fila.Controls.Add(CrearTarjetaIndicador("PEDIDOS PENDIENTES", lblValorProveedores, "◷", Color.FromArgb(93, 130, 238), ""), 2, 0);
            fila.Controls.Add(CrearTarjetaIndicador("CLIENTES REGISTRADOS", lblValorStockBajo, "♙", Color.FromArgb(157, 119, 222), ""), 3, 0);
            return fila;
        }

        private static Panel CrearTarjetaIndicador(string titulo, Label valor, string icono, Color color, string nota)
        {
            Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(5, 5, 7, 4) };
            Label simbolo = new Label { Text = icono, Location = new Point(13, 9), Size = new Size(27, 27), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = color, BackColor = Color.FromArgb(237, 249, 245) };
            Label encabezado = new Label { Text = titulo, Location = new Point(13, 39), Size = new Size(220, 18), Font = new Font("Segoe UI", 7.5F), ForeColor = Color.FromArgb(105, 127, 134) };
            valor.Location = new Point(13, 58);
            valor.Size = new Size(190, 30);
            valor.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            valor.ForeColor = Color.FromArgb(17, 40, 52);
            valor.TextAlign = ContentAlignment.MiddleLeft;
            tarjeta.Controls.Add(simbolo);
            tarjeta.Controls.Add(encabezado);
            tarjeta.Controls.Add(valor);
            Redondear(tarjeta, 13);
            Redondear(simbolo, 8);
            return tarjeta;
        }

        private Control CrearFilaGraficas()
        {
            TableLayoutPanel fila = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = fondo };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            graficoVentas.Tipo = PanelGrafico.TipoVisualizacion.Linea;
            graficoInventario.Tipo = PanelGrafico.TipoVisualizacion.Anillo;
            fila.Controls.Add(CrearTarjetaSeccion("Rendimiento de ventas", "Ingresos de los últimos 7 días", graficoVentas, new Padding(5, 5, 7, 7)), 0, 0);
            fila.Controls.Add(CrearTarjetaSeccion("Inventario", "Estado de existencias", graficoInventario, new Padding(5, 5, 5, 7)), 1, 0);
            return fila;
        }

        private Control CrearFilaInferior()
        {
            TableLayoutPanel fila = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = fondo };
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            fila.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            Panel actividad = CrearTarjetaSeccion("Actividad reciente", "Movimientos registrados", listaActividad, new Padding(5, 5, 7, 5));
            Panel accesos = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(12, 96, 86), Margin = new Padding(5), Padding = new Padding(17) };
            panelAccesosRapidos = accesos;
            Label etiqueta = new Label { Text = "ACCESO RÁPIDO", Location = new Point(18, 17), Size = new Size(220, 18), Font = new Font("Segoe UI", 7F, FontStyle.Bold), ForeColor = Color.FromArgb(127, 237, 202), BackColor = Color.Transparent };
            Label pregunta = new Label { Text = "¿Qué necesitas hacer?", Location = new Point(18, 39), Size = new Size(250, 26), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent };
            btnActualizar.Text = "+  Nueva venta";
            btnActualizar.SetBounds(18, 72, 118, 32);
            ConfigurarBoton(btnActualizar, true);
            btnCerrar.Text = "+  Producto";
            btnCerrar.SetBounds(145, 72, 112, 32);
            ConfigurarBoton(btnCerrar, false);
            btnActualizar.Click += (s, e) => SolicitarNavegacion("Ventas");
            btnCerrar.Click += (s, e) => SolicitarNavegacion("Productos");
            accesos.Controls.Add(etiqueta);
            accesos.Controls.Add(pregunta);
            accesos.Controls.Add(btnActualizar);
            accesos.Controls.Add(btnCerrar);
            accesos.Resize += (s, e) => AjustarAccesosRapidos();
            Redondear(accesos, 15);
            fila.Controls.Add(actividad, 0, 0);
            fila.Controls.Add(accesos, 1, 0);
            return fila;
        }

        private static void ConfigurarBoton(FontAwesome.Sharp.IconButton boton, bool principal)
        {
            boton.IconChar = FontAwesome.Sharp.IconChar.None;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = principal ? 0 : 1;
            boton.FlatAppearance.BorderColor = Color.FromArgb(72, 151, 138);
            boton.BackColor = principal ? Color.FromArgb(37, 202, 153) : Color.FromArgb(35, 111, 104);
            boton.ForeColor = Color.White;
            boton.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
            boton.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static Panel CrearTarjetaSeccion(string titulo, string descripcion, Control contenido, Padding margen)
        {
            Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = margen };
            TableLayoutPanel distribucion = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(13, 10, 13, 10), BackColor = Color.White };
            distribucion.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            distribucion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            encabezado.Controls.Add(new Label { Text = titulo, Location = new Point(0, 0), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(21, 42, 51) });
            encabezado.Controls.Add(new Label { Text = descripcion, Location = new Point(0, 20), AutoSize = true, Font = new Font("Segoe UI", 7F), ForeColor = Color.FromArgb(116, 137, 143) });
            contenido.Dock = DockStyle.Fill;
            distribucion.Controls.Add(encabezado, 0, 0);
            distribucion.Controls.Add(contenido, 0, 1);
            tarjeta.Controls.Add(distribucion);
            Redondear(tarjeta, 14);
            return tarjeta;
        }

        private static Panel CrearFilaActividad(ActividadDashboard actividad, bool venta)
        {
            Panel fila = new Panel { Width = 620, Height = 44, BackColor = Color.White, Margin = new Padding(0, 0, 0, 2) };
            Label icono = new Label { Text = venta ? "↗" : "▤", Location = new Point(2, 7), Size = new Size(25, 25), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = venta ? Color.FromArgb(32, 179, 132) : Color.FromArgb(222, 151, 64), BackColor = venta ? Color.FromArgb(235, 249, 243) : Color.FromArgb(255, 245, 231) };
            Label titulo = new Label { Text = actividad.Tipo, Location = new Point(36, 3), Size = new Size(300, 18), Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(33, 52, 58) };
            Label detalle = new Label { Text = actividad.Descripcion + " · " + actividad.Referencia, Location = new Point(36, 22), Size = new Size(400, 17), Font = new Font("Segoe UI", 7F), ForeColor = Color.FromArgb(122, 141, 146) };
            Label hora = new Label { Text = actividad.Fecha.ToString("HH:mm"), Size = new Size(50, 18), TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI", 7F), ForeColor = Color.FromArgb(122, 141, 146), Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(550, 12) };
            Panel linea = new Panel { Height = 1, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(239, 243, 242) };
            fila.Controls.Add(icono); fila.Controls.Add(titulo); fila.Controls.Add(detalle); fila.Controls.Add(hora); fila.Controls.Add(linea);
            fila.Resize += (s, e) => hora.Left = fila.ClientSize.Width - hora.Width - 2;
            return fila;
        }

        private static void Redondear(Control control, int radio)
        {
            EventHandler ajustar = (s, e) =>
            {
                if (control.Width < 2 || control.Height < 2) return;
                int d = Math.Min(radio * 2, Math.Min(control.Width, control.Height));
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, d, d, 180, 90); path.AddArc(control.Width - d, 0, d, d, 270, 90);
                    path.AddArc(control.Width - d, control.Height - d, d, d, 0, 90); path.AddArc(0, control.Height - d, d, d, 90, 90); path.CloseFigure();
                    Region anterior = control.Region; control.Region = new Region(path); if (anterior != null) anterior.Dispose();
                }
            };
            control.Resize += ajustar;
            ajustar(control, EventArgs.Empty);
        }

        private void DashboardForms_Load(object sender, EventArgs e) { MtdActualizar(); }

        private void MtdActualizar()
        {
            try
            {
                DashboardEntidad resumen = _dashboardNegocio.MtdResumen();
                lblValorSucursales.Text = "Q " + resumen.VentasHoy.ToString("N0");
                lblValorProductos.Text = resumen.TotalProductos.ToString("N0");
                lblValorProveedores.Text = resumen.TrasladosPendientes.ToString("N0");
                lblValorStockBajo.Text = resumen.TotalClientes.ToString("N0");
                graficoVentas.TextoResumen = "Q " + resumen.VentasHoy.ToString("N0");
                graficoVentas.Importes = resumen.VentasUltimosSieteDias;
                graficoInventario.Valores = resumen.EstadoInventario;
                listaActividad.Controls.Clear();
                if (resumen.ErroresConsulta.Count > 0)
                    listaActividad.Controls.Add(new Label
                    {
                        Text = "Aviso: " + resumen.ErroresConsulta[0],
                        AutoSize = true, MaximumSize = new Size(Math.Max(300, listaActividad.ClientSize.Width - 20), 0),
                        Font = new Font("Segoe UI", 7F), ForeColor = Color.FromArgb(168, 103, 38),
                        Margin = new Padding(8, 4, 0, 6)
                    });
                foreach (ActividadDashboard item in resumen.ActividadReciente)
                {
                    Panel fila = CrearFilaActividad(item, item.Tipo.StartsWith("Venta", StringComparison.OrdinalIgnoreCase));
                    fila.Width = Math.Max(300, listaActividad.ClientSize.Width - 12);
                    listaActividad.Controls.Add(fila);
                }
                if (resumen.ActividadReciente.Count == 0)
                    listaActividad.Controls.Add(new Label { Text = "Aún no hay movimientos registrados.", AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.Gray, Margin = new Padding(8, 12, 0, 0) });
            }
            catch (Exception ex) { AyudaForms.MtdError(ex); }
        }

        private void SolicitarNavegacion(string modulo)
        {
            Action<string> navegar = NavegacionSolicitada;
            if (navegar != null) navegar(modulo);
        }

        public void ConfigurarAccesosRapidos(ICollection<string> modulosPermitidos)
        {
            bool puedeVender = modulosPermitidos != null && modulosPermitidos.Contains("Ventas");
            bool puedeVerProductos = modulosPermitidos != null && modulosPermitidos.Contains("Productos");
            btnActualizar.Visible = puedeVender;
            btnCerrar.Visible = puedeVerProductos;
            if (panelAccesosRapidos != null)
                panelAccesosRapidos.Visible = puedeVender || puedeVerProductos;
            AjustarAccesosRapidos();
        }

        private void AjustarAccesosRapidos()
        {
            if (panelAccesosRapidos == null) return;
            int ancho = Math.Max(88, panelAccesosRapidos.ClientSize.Width - 48);
            if (btnActualizar.Visible && btnCerrar.Visible)
            {
                ancho = Math.Max(88, (panelAccesosRapidos.ClientSize.Width - 48) / 2);
                btnActualizar.Left = 18;
                btnCerrar.Left = btnActualizar.Right + 10;
            }
            else if (btnActualizar.Visible)
            {
                btnActualizar.Left = 18;
            }
            else if (btnCerrar.Visible)
            {
                btnCerrar.Left = 18;
            }
            btnActualizar.Width = ancho;
            btnCerrar.Width = ancho;
        }

        private void btnActualizar_Click(object sender, EventArgs e) { MtdActualizar(); }
        private void btnCerrar_Click(object sender, EventArgs e) { Close(); }
        private void lblValorProveedores_Click(object sender, EventArgs e) { }
    }
}
