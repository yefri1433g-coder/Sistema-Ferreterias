using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class KardexForms : Form
    {
        // Instanciar: Que el forms actual (capa presentacion) pueda acceder a otra capa (Negocio)
        private readonly KardexNegocio _kardexNegocio = new KardexNegocio();
        private readonly CatalogosNegocio _catalogosNegocio = new CatalogosNegocio();
        private readonly Label[] _resumenKardex = new Label[3];
        private List<KardexEntidad> _listaKardexVista = new List<KardexEntidad>();
        private List<KardexEntidad> _listaKardexCompleta = new List<KardexEntidad>();
        private TabControl _pestanasKardex;
        private DataGridView _gridKardexVista;
        private TextBox _buscarKardexVista;
        private Label _totalKardexVista;
        private Label _paginaKardexVista;
        private Button _anteriorKardexVista, _siguienteKardexVista;
        private int _numeroPaginaKardex = 1;
        private const int MovimientosPorPagina = 4;

        public KardexForms()
        {
            InitializeComponent();
            EstiloAplicacion.Aplicar(this);
            PrepararDisenoKardex();
        }

        private void PrepararDisenoKardex()
        {
            BackColor = Color.FromArgb(241, 246, 245);
            PanelPrincipal.Dock = DockStyle.Fill;
            _pestanasKardex = new TabControl { Dock = DockStyle.Fill, Appearance = TabAppearance.FlatButtons, ItemSize = new Size(0, 1), SizeMode = TabSizeMode.Fixed };
            TabPage resumen = new TabPage("Kardex") { BackColor = Color.FromArgb(241, 246, 245), Padding = new Padding(0) };
            TabPage registros = new TabPage("Consulta y exportación") { BackColor = Color.White };
            registros.Controls.Add(PanelPrincipal);
            Button volver = new Button { Text = "< Volver al resumen", Dock = DockStyle.Top, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(23, 143, 112), TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
            volver.FlatAppearance.BorderSize = 0;
            volver.Click += (s, e) => _pestanasKardex.SelectedTab = resumen;
            registros.Controls.Add(volver);
            _pestanasKardex.TabPages.Add(resumen); _pestanasKardex.TabPages.Add(registros);
            Controls.Clear(); Controls.Add(_pestanasKardex);

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = Color.FromArgb(241, 246, 245) };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245) };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Kardex", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(102, 123, 131) });
            encabezado.Controls.Add(new Label { Text = "Kardex", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            encabezado.Controls.Add(new Label { Text = "Historial detallado de movimientos de inventario.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(102, 123, 131) });
            LinkLabel consulta = new LinkLabel { Text = "Consulta completa", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = Color.FromArgb(23, 143, 112), Font = new Font("Segoe UI", 8.5F), Location = new Point(Width - 175, 26) };
            consulta.Click += (s, e) => { _pestanasKardex.SelectedTab = registros; MtdConsultar(); };
            encabezado.Controls.Add(consulta);

            Panel resumenTarjetas = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(241, 246, 245), Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.FromArgb(241, 246, 245) };
            string[] nombres = { "MOVIMIENTOS HOY", "ENTRADAS", "SALIDAS" };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "↗" : i == 1 ? "◷" : "≡", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : Color.FromArgb(35, 184, 139), BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : Color.FromArgb(237, 249, 246) };
                Label nombre = new Label { Text = nombres[i], Left = 14, Top = 42, Height = 18, AutoEllipsis = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = Color.FromArgb(113, 134, 141) };
                _resumenKardex[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(nombre); tarjeta.Controls.Add(_resumenKardex[i]); tarjetas.Controls.Add(tarjeta, i, 0);
            }
            resumenTarjetas.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Kardex reciente", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(23, 43, 53) });
            barra.Controls.Add(new Label { Text = "Historial de entradas, salidas y ajustes", Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(123, 139, 147) });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 338, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonKardex("+ Nuevo registro", true);
            nuevo.Click += (s, e) => AyudaForms.MtdInfo("Los movimientos del Kardex se registran automáticamente al actualizar stock, ventas, compras o traslados.");
            _buscarKardexVista = new TextBox { Width = 190, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar kardex", ForeColor = Color.FromArgb(135, 151, 156) };
            _buscarKardexVista.Enter += (s, e) => { if (_buscarKardexVista.Text == "Buscar kardex") { _buscarKardexVista.Clear(); _buscarKardexVista.ForeColor = Color.FromArgb(45, 64, 70); } };
            _buscarKardexVista.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarKardexVista.Text)) { _buscarKardexVista.Text = "Buscar kardex"; _buscarKardexVista.ForeColor = Color.FromArgb(135, 151, 156); } };
            _buscarKardexVista.TextChanged += (s, e) => { _numeroPaginaKardex = 1; if (_buscarKardexVista.Text != "Buscar kardex") FiltrarKardexVista(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(_buscarKardexVista); barra.Controls.Add(acciones);

            _gridKardexVista = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridKardexVista.ColumnHeadersDefaultCellStyle.BackColor = Color.White; _gridKardexVista.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(102, 123, 131); _gridKardexVista.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold); _gridKardexVista.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White; _gridKardexVista.DefaultCellStyle.ForeColor = Color.FromArgb(50, 69, 77); _gridKardexVista.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 246, 241); _gridKardexVista.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 59, 56); _gridKardexVista.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridKardexVista.RowTemplate.Height = 42;
            _gridKardexVista.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridKardexVista.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridKardexVista.CellFormatting += (s, e) => { if (e.RowIndex >= 0 && _gridKardexVista.Columns[e.ColumnIndex].Name == "Movimiento" && e.Value != null) e.CellStyle.ForeColor = Convert.ToString(e.Value) == "Entrada" ? Color.FromArgb(22, 151, 113) : Color.FromArgb(210, 126, 48); };
            _totalKardexVista = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(119, 136, 145) };
            _paginaKardexVista = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(92, 111, 119) };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel controles = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteKardexVista = new Button { Text = "Siguiente >", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(37, 120, 105) };
            _anteriorKardexVista = new Button { Text = "< Anterior", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(37, 120, 105) };
            _siguienteKardexVista.Click += (s, e) => CambiarPaginaKardex(1); _anteriorKardexVista.Click += (s, e) => CambiarPaginaKardex(-1);
            controles.Controls.Add(_siguienteKardexVista); controles.Controls.Add(_paginaKardexVista); controles.Controls.Add(_anteriorKardexVista);
            pie.Controls.Add(_totalKardexVista); pie.Controls.Add(controles);
            lista.Controls.Add(_gridKardexVista); lista.Controls.Add(pie); lista.Controls.Add(barra);
            RedondearPanel(lista, 14); for (int i = 0; i < tarjetas.Controls.Count; i++) RedondearPanel(tarjetas.Controls[i], 10);
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(resumenTarjetas, 0, 1); pantalla.Controls.Add(lista, 0, 2);
            resumen.Controls.Add(pantalla);
            _pestanasKardex.SelectedIndexChanged += (s, e) => { if (_pestanasKardex.SelectedTab == resumen) MtdConsultar(); };
        }

        private void KardexForms_Load(object sender, EventArgs e)
        {
            try
            {
                AyudaForms.MtdLlenarCombo(cboFiltro, _catalogosNegocio.MtdProductosConTodos());
                MtdConsultar();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // =====================================================
        // CONSULTAR (por producto)
        // =====================================================

        private void MtdConsultar()
        {
            List<KardexEntidad> lista = _kardexNegocio.MtdConsultar(AyudaForms.MtdValorCombo(cboFiltro));
            _listaKardexCompleta = lista ?? new List<KardexEntidad>();
            _numeroPaginaKardex = 1;

            dgvKardex.DataSource = null;
            dgvKardex.DataSource = _listaKardexCompleta;

            AyudaForms.MtdTitulo(dgvKardex, "CodigoKardex", "Código");
            AyudaForms.MtdTitulo(dgvKardex, "NombreProducto", "Producto");
            AyudaForms.MtdTitulo(dgvKardex, "FechaMovimiento", "Fecha");
            AyudaForms.MtdTitulo(dgvKardex, "TipoMovimiento", "Movimiento");
            AyudaForms.MtdTitulo(dgvKardex, "Descripcion", "Descripción");
            AyudaForms.MtdOcultar(dgvKardex, "CodigoProducto");
            AyudaForms.MtdFormato(dgvKardex, "FechaMovimiento", "dd/MM/yyyy HH:mm");

            lblTotalRegistros.Text = "Total Registros: " + _listaKardexCompleta.Count;
            if (_gridKardexVista != null) MostrarKardexVista(_listaKardexCompleta);
        }

        private static Button CrearBotonKardex(string texto, bool principal)
        {
            Button boton = new Button { Text = texto, Width = principal ? 128 : 78, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = principal ? Color.FromArgb(23, 143, 112) : Color.White, ForeColor = principal ? Color.White : Color.FromArgb(50, 69, 77), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = principal ? 0 : 1;
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
                    ruta.AddArc(0, 0, diametro, diametro, 180, 90);
                    ruta.AddArc(control.Width - diametro, 0, diametro, diametro, 270, 90);
                    ruta.AddArc(control.Width - diametro, control.Height - diametro, diametro, diametro, 0, 90);
                    ruta.AddArc(0, control.Height - diametro, diametro, diametro, 90, 90);
                    ruta.CloseFigure();
                    Region anterior = control.Region;
                    control.Region = new Region(ruta);
                    if (anterior != null) anterior.Dispose();
                }
            };
            control.Resize += (s, e) => actualizarRegion();
            control.HandleCreated += (s, e) => actualizarRegion();
        }

        private void FiltrarKardexVista()
        {
            string filtro = (_buscarKardexVista.Text ?? String.Empty).Trim();
            if (filtro.Length == 0) { MostrarKardexVista(_listaKardexCompleta); return; }
            MostrarKardexVista(_listaKardexCompleta.Where(x =>
                (x.NombreProducto ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.TipoMovimiento ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.Descripcion ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                x.FechaMovimiento.ToString("dd MMM yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX")).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList());
        }

        private void MostrarKardexVista(List<KardexEntidad> lista)
        {
            _listaKardexVista = lista ?? new List<KardexEntidad>();
            DateTime hoy = DateTime.Today;
            List<KardexEntidad> movimientosHoy = _listaKardexCompleta.Where(x => x.FechaMovimiento.Date == hoy).ToList();
            _resumenKardex[0].Text = movimientosHoy.Count.ToString("N0");
            _resumenKardex[1].Text = movimientosHoy.Count(x => String.Equals(x.TipoMovimiento, "Entrada", StringComparison.OrdinalIgnoreCase)).ToString("N0");
            _resumenKardex[2].Text = movimientosHoy.Count(x => String.Equals(x.TipoMovimiento, "Salida", StringComparison.OrdinalIgnoreCase)).ToString("N0");

            int paginas = Math.Max(1, (int)Math.Ceiling(_listaKardexVista.Count / (double)MovimientosPorPagina));
            _numeroPaginaKardex = Math.Max(1, Math.Min(_numeroPaginaKardex, paginas));
            List<KardexEntidad> pagina = _listaKardexVista.Skip((_numeroPaginaKardex - 1) * MovimientosPorPagina).Take(MovimientosPorPagina).ToList();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Fecha"); tabla.Columns.Add("Producto"); tabla.Columns.Add("Movimiento"); tabla.Columns.Add("Cantidad"); tabla.Columns.Add("Referencia");
            foreach (KardexEntidad item in pagina)
            {
                string cantidad = String.Equals(item.TipoMovimiento, "Salida", StringComparison.OrdinalIgnoreCase) ? "-" + Math.Abs(item.Cantidad).ToString("N0") : "+" + Math.Abs(item.Cantidad).ToString("N0");
                tabla.Rows.Add(item.FechaMovimiento.ToString("dd MMM - HH:mm", CultureInfo.GetCultureInfo("es-MX")), item.NombreProducto, item.TipoMovimiento, cantidad, item.Descripcion);
            }
            _gridKardexVista.DataSource = tabla;
            _gridKardexVista.Columns["Fecha"].HeaderText = "FECHA";
            _gridKardexVista.Columns["Producto"].HeaderText = "PRODUCTO";
            _gridKardexVista.Columns["Movimiento"].HeaderText = "MOVIMIENTO";
            _gridKardexVista.Columns["Cantidad"].HeaderText = "CANTIDAD";
            _gridKardexVista.Columns["Referencia"].HeaderText = "REFERENCIA";
            _gridKardexVista.Columns["Fecha"].FillWeight = 22;
            _gridKardexVista.Columns["Producto"].FillWeight = 27;
            _gridKardexVista.Columns["Movimiento"].FillWeight = 21;
            _gridKardexVista.Columns["Cantidad"].FillWeight = 14;
            _gridKardexVista.Columns["Referencia"].FillWeight = 26;
            _gridKardexVista.ClearSelection();
            int desde = _listaKardexVista.Count == 0 ? 0 : ((_numeroPaginaKardex - 1) * MovimientosPorPagina) + 1;
            int hasta = Math.Min(_numeroPaginaKardex * MovimientosPorPagina, _listaKardexVista.Count);
            _totalKardexVista.Text = "Mostrando " + desde + "-" + hasta + " de " + _listaKardexVista.Count + " registros";
            _paginaKardexVista.Text = _numeroPaginaKardex + " de " + paginas;
            _anteriorKardexVista.Enabled = _numeroPaginaKardex > 1;
            _siguienteKardexVista.Enabled = _numeroPaginaKardex < paginas;
        }

        private void CambiarPaginaKardex(int cambio)
        {
            _numeroPaginaKardex += cambio;
            if (String.IsNullOrWhiteSpace(_buscarKardexVista.Text) || _buscarKardexVista.Text == "Buscar kardex")
                MostrarKardexVista(_listaKardexCompleta);
            else
                FiltrarKardexVista();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                MtdConsultar();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboFiltro.Items.Count > 0)
                {
                    cboFiltro.SelectedIndex = 0;
                }

                MtdConsultar();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void ChkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionar.Checked)
            {
                dgvKardex.SelectAll();
            }
            else
            {
                dgvKardex.ClearSelection();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdImprimir(dgvKardex, "Kardex de Inventario");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdExportarCsv(dgvKardex, "Kardex");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

