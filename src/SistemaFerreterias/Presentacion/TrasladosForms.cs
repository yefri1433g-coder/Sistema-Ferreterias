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
    // CLASE TrasladosForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class TrasladosForms : Form
    {
        // Instanciar: Que el forms actual (capa presentacion) pueda acceder a otra capa (Negocio)
        private readonly TrasladosNegocio _trasladosNegocio = new TrasladosNegocio();
        private readonly CatalogosNegocio _catalogosNegocio = new CatalogosNegocio();

        // Productos del traslado que se esta creando o editando
        private readonly BindingList<DetalleTrasladoEntidad> _detalles = new BindingList<DetalleTrasladoEntidad>();

        // Variables de control: registro cargado en Gestionar y modo actual (Agregar / Editar)
        private int _codigoSeleccionado = 0;
        private string _modo = string.Empty;
        private readonly Label[] _resumenTraslados = new Label[3];
        private List<TrasladosEntidad> _trasladosCompletos = new List<TrasladosEntidad>();
        private List<TrasladosEntidad> _trasladosVisibles = new List<TrasladosEntidad>();
        private DataGridView _gridTrasladosVista;
        private TextBox _buscarTrasladosVista;
        private Label _totalTrasladosVista, _paginaTrasladosVista;
        private Button _anteriorTrasladosVista, _siguienteTrasladosVista;
        private int _paginaTraslados = 1;
        // La paginación se realiza solo en Presentación y no cambia los totales del resultado completo.
        private const int TrasladosPorPagina = 10;
        private Form _dialogoTraslado;

        public TrasladosForms()
        {
            InitializeComponent();
            EstiloAplicacion.Aplicar(this);
            PrepararDisenoTraslados();
            BarraListado.Configurar(_gridTrasladosVista, _buscarTrasladosVista, "Traslados", () => AbrirTrasladoExistente((TrasladosEntidad)_gridTrasladosVista.CurrentRow.Tag));
            PrepararDialogoTraslado();
        }

        // CONFIGURACIÓN: PrepararDialogoTraslado
        private void PrepararDialogoTraslado()
        {
            _dialogoTraslado = new Form { Text = "Nuevo traslado", ClientSize = new Size(1120, 740), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            tabPage2.Controls.Remove(PanelPrincipalGestionar);
            PanelPrincipalGestionar.Dock = DockStyle.Fill;
            panelEncabezadoGestionar.Visible = false;
            PanelCerrarGestionar.Visible = false;
            _dialogoTraslado.Controls.Add(PanelPrincipalGestionar);
        }

        // PRESENTAR DATOS: MostrarDialogoTraslado
        private void MostrarDialogoTraslado(bool nuevo)
        {
            _dialogoTraslado.Text = nuevo ? "Nuevo traslado" : _modo == "Editar" ? "Editar traslado" : "Consultar traslado";
            _dialogoTraslado.DialogResult = DialogResult.None;
            EstiloAplicacion.AplicarDialogo(_dialogoTraslado);
            _dialogoTraslado.ShowDialog(this);
        }

        // CONFIGURACIÓN: PrepararDisenoTraslados
        private void PrepararDisenoTraslados()
        {
            BackColor = EstiloAplicacion.Fondo;
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabPage1.Controls.Clear();
            TabPage consultaCompleta = new TabPage("Consulta completa") { BackColor = Color.White };
            consultaCompleta.Controls.Add(PanelPrincipal);
            Button volver = new Button { Text = "< Volver al resumen", Dock = DockStyle.Top, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento, TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
            volver.FlatAppearance.BorderSize = 0;
            volver.Click += (s, e) => tabControl1.SelectedTab = tabPage1;
            consultaCompleta.Controls.Add(volver);
            tabControl1.TabPages.Add(consultaCompleta);

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(16), BackColor = EstiloAplicacion.Fondo };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = EstiloAplicacion.Fondo };
            encabezado.Controls.Add(new Label { Text = "Inicio  /  Traslados", Left = 5, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            encabezado.Controls.Add(new Label { Text = "Traslados", Left = 4, Top = 22, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            encabezado.Controls.Add(new Label { Text = "Seguimiento de mercancía entre sucursales.", Left = 7, Top = 66, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = EstiloAplicacion.Secundario });
            LinkLabel consulta = new LinkLabel { Text = "Consulta completa", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = EstiloAplicacion.Acento, Font = new Font("Segoe UI", 8.5F), Location = new Point(Width - 180, 26) };
            consulta.Click += (s, e) => { tabControl1.SelectedTab = consultaCompleta; MtdConsultar(); };
            encabezado.Controls.Add(consulta);

            Panel panelResumen = new Panel { Dock = DockStyle.Fill, BackColor = EstiloAplicacion.Fondo, Padding = new Padding(0, 0, 0, 10) };
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = EstiloAplicacion.Fondo };
            string[] nombres = { "TRASLADOS DEL MES", "EN TRÁNSITO", "POR APROBAR" };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "↗" : i == 1 ? "◷" : "≡", Left = 14, Top = 10, Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold), ForeColor = i == 2 ? Color.FromArgb(222, 143, 61) : EstiloAplicacion.Acento, BackColor = i == 2 ? Color.FromArgb(255, 246, 233) : EstiloAplicacion.FondoSuave };
                Label nombre = new Label { Text = nombres[i], Left = 14, Top = 42, Width = 230, Height = 18, AutoEllipsis = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario };
                _resumenTraslados[i] = new Label { Text = "0", Left = 14, Top = 62, AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(nombre); tarjeta.Controls.Add(_resumenTraslados[i]); tarjetas.Controls.Add(tarjeta, i, 0);
            }
            panelResumen.Controls.Add(tarjetas);

            Panel lista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 10) };
            Panel barra = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            barra.Controls.Add(new Label { Text = "Traslados recientes", Left = 4, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            barra.Controls.Add(new Label { Text = "Información actualizada · " + DateTime.Today.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("es-MX")), Left = 5, Top = 24, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 410, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            Button nuevo = CrearBotonTraslado("+ Nuevo registro", true);
            nuevo.Click += (s, e) => btnNuevo_Click(this, EventArgs.Empty);
            Button eliminar = CrearBotonTraslado("Eliminar", false); eliminar.Width = 82; eliminar.Click += (s, e) => EliminarTrasladoVista();
            _buscarTrasladosVista = new TextBox { Width = 165, Height = 28, Margin = new Padding(8, 1, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle, Text = "Buscar traslados", ForeColor = EstiloAplicacion.Secundario };
            _buscarTrasladosVista.Enter += (s, e) => { if (_buscarTrasladosVista.Text == "Buscar traslados") { _buscarTrasladosVista.Clear(); _buscarTrasladosVista.ForeColor = EstiloAplicacion.Tinta; } };
            _buscarTrasladosVista.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(_buscarTrasladosVista.Text)) { _buscarTrasladosVista.Text = "Buscar traslados"; _buscarTrasladosVista.ForeColor = EstiloAplicacion.Secundario; } };
            _buscarTrasladosVista.TextChanged += (s, e) => { _paginaTraslados = 1; if (_buscarTrasladosVista.Text != "Buscar traslados") FiltrarTrasladosVista(); };
            acciones.Controls.Add(nuevo); acciones.Controls.Add(eliminar); acciones.Controls.Add(_buscarTrasladosVista); barra.Controls.Add(acciones);

            _gridTrasladosVista = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Color.FromArgb(234, 240, 240), EnableHeadersVisualStyles = false, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None, ColumnHeadersHeight = 36, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, Font = new Font("Segoe UI", 9F) };
            _gridTrasladosVista.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino; _gridTrasladosVista.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; _gridTrasladosVista.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold); _gridTrasladosVista.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino; _gridTrasladosVista.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta; _gridTrasladosVista.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion; _gridTrasladosVista.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta; _gridTrasladosVista.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5); _gridTrasladosVista.RowTemplate.Height = 42;
            _gridTrasladosVista.DefaultCellStyle.WrapMode = DataGridViewTriState.True; _gridTrasladosVista.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridTrasladosVista.CellFormatting += (s, e) => { if (e.RowIndex >= 0 && _gridTrasladosVista.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null) e.CellStyle.ForeColor = String.Equals(Convert.ToString(e.Value), EstadosTraslado.Completado, StringComparison.OrdinalIgnoreCase) ? EstiloAplicacion.Acento : Color.FromArgb(210, 126, 48); };
            _gridTrasladosVista.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AbrirTrasladoExistente((TrasladosEntidad)_gridTrasladosVista.Rows[e.RowIndex].Tag); };
            ContextMenuStrip menuFila = new ContextMenuStrip();
            menuFila.Items.Add("Ver / editar traslado", null, (s, e) => { if (_gridTrasladosVista.CurrentRow != null) AbrirTrasladoExistente((TrasladosEntidad)_gridTrasladosVista.CurrentRow.Tag); });
            _gridTrasladosVista.ContextMenuStrip = menuFila;
            _gridTrasladosVista.CellMouseDown += (s, e) => { if (e.Button == MouseButtons.Right && e.RowIndex >= 0) _gridTrasladosVista.CurrentCell = _gridTrasladosVista.Rows[e.RowIndex].Cells[0]; };

            _totalTrasladosVista = new Label { Dock = DockStyle.Left, Width = 300, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            _paginaTrasladosVista = new Label { Width = 90, Height = 25, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            FlowLayoutPanel controles = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 310, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            _siguienteTrasladosVista = new Button { Text = "Siguiente >", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento };
            _anteriorTrasladosVista = new Button { Text = "< Anterior", Width = 96, Height = 32, Margin = new Padding(3, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Acento };
            _siguienteTrasladosVista.Click += (s, e) => CambiarPaginaTraslados(1); _anteriorTrasladosVista.Click += (s, e) => CambiarPaginaTraslados(-1);
            controles.Controls.Add(_siguienteTrasladosVista); controles.Controls.Add(_paginaTrasladosVista); controles.Controls.Add(_anteriorTrasladosVista);
            pie.Controls.Add(_totalTrasladosVista); pie.Controls.Add(controles);
            lista.Controls.Add(_gridTrasladosVista); lista.Controls.Add(pie); lista.Controls.Add(barra);
            RedondearPanel(lista, 14); for (int i = 0; i < tarjetas.Controls.Count; i++) RedondearPanel(tarjetas.Controls[i], 10);
            pantalla.Controls.Add(encabezado, 0, 0); pantalla.Controls.Add(panelResumen, 0, 1); pantalla.Controls.Add(lista, 0, 2);
            tabPage1.Controls.Add(pantalla);
            tabControl1.SelectedIndexChanged += (s, e) => { if (tabControl1.SelectedTab == tabPage1) MtdConsultar(); };
        }

        private void TrasladosForms_Load(object sender, EventArgs e)
        {
            try
            {
                AyudaForms.MtdLlenarCombo(cboEmpleado, _catalogosNegocio.MtdEmpleados());
                AyudaForms.MtdLlenarCombo(cboOrigen, _catalogosNegocio.MtdSucursales());
                AyudaForms.MtdLlenarCombo(cboDestino, _catalogosNegocio.MtdSucursales());
                AyudaForms.MtdLlenarCombo(cboProducto, _catalogosNegocio.MtdProductos());

                dgvDetalleGestionar.DataSource = _detalles;
                MtdConfigurarDetalle(dgvDetalleGestionar);

                MtdConsultar();
                MtdLimpiarCampos();
                MtdHabilitarCampos(false);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // ELIMINAR: EliminarTrasladoVista
        private void EliminarTrasladoVista()
        {
            if (_gridTrasladosVista == null || _gridTrasladosVista.CurrentRow == null) { AyudaForms.MtdAviso("Selecciona primero un traslado."); return; }
            TrasladosEntidad item = _gridTrasladosVista.CurrentRow.Tag as TrasladosEntidad;
            if (item == null) return;
            MtdCargarEnCampos(item);
            _codigoSeleccionado = item.CodigoTraslado;
            btnEliminar_Click(this, EventArgs.Empty);
        }

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        // CONFIGURACIÓN: MtdConfigurarDetalle
        private void MtdConfigurarDetalle(DataGridView dgv)
        {
            AyudaForms.MtdTitulo(dgv, "NombreProducto", "Producto");
            AyudaForms.MtdOcultar(dgv, "CodigoProducto");
        }

        // CONSULTAR: MtdConsultar
        private void MtdConsultar()
        {
            _trasladosCompletos = _trasladosNegocio.MtdConsultar();
            MtdMostrarLista(_trasladosCompletos);
            if (_gridTrasladosVista != null) MostrarTrasladosVista(_trasladosCompletos);
        }

        // CREAR ELEMENTO VISUAL: CrearBotonTraslado
        private static Button CrearBotonTraslado(string texto, bool principal)
        {
            Button boton = new Button { Text = texto, Width = principal ? 128 : 78, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = principal ? EstiloAplicacion.Acento : Color.White, ForeColor = principal ? Color.White : EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
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

        private void FiltrarTrasladosVista()
        {
            string filtro = (_buscarTrasladosVista.Text ?? String.Empty).Trim();
            if (filtro.Length == 0) { MostrarTrasladosVista(_trasladosCompletos); return; }
            MostrarTrasladosVista(_trasladosCompletos.Where(x =>
                ("TR-" + x.FechaTraslado.ToString("yyyy") + "-" + x.CodigoTraslado.ToString("000")).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.NombreOrigen ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.NombreDestino ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.NombreEmpleado ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                (x.Estado ?? String.Empty).IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList());
        }

        // PRESENTAR DATOS: MostrarTrasladosVista
        private void MostrarTrasladosVista(List<TrasladosEntidad> lista)
        {
            _trasladosVisibles = lista ?? new List<TrasladosEntidad>();
            DateTime hoy = DateTime.Today;
            _resumenTraslados[0].Text = _trasladosCompletos.Count(x => x.FechaTraslado.Year == hoy.Year && x.FechaTraslado.Month == hoy.Month).ToString("N0");
            _resumenTraslados[1].Text = _trasladosCompletos.Count(x => String.Equals(x.Estado, EstadosTraslado.EnTransito, StringComparison.OrdinalIgnoreCase)).ToString("N0");
            _resumenTraslados[2].Text = _trasladosCompletos.Count(x => String.Equals(x.Estado, EstadosTraslado.Pendiente, StringComparison.OrdinalIgnoreCase)).ToString("N0");

            int paginas = Math.Max(1, (int)Math.Ceiling(_trasladosVisibles.Count / (double)TrasladosPorPagina));
            _paginaTraslados = Math.Max(1, Math.Min(_paginaTraslados, paginas));
            List<TrasladosEntidad> pagina = _trasladosVisibles.Skip((_paginaTraslados - 1) * TrasladosPorPagina).Take(TrasladosPorPagina).ToList();
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Numero"); tabla.Columns.Add("Origen"); tabla.Columns.Add("Destino"); tabla.Columns.Add("Fecha"); tabla.Columns.Add("Estado");
            foreach (TrasladosEntidad item in pagina)
                tabla.Rows.Add("TR-" + item.FechaTraslado.ToString("yyyy") + "-" + item.CodigoTraslado.ToString("000"), item.NombreOrigen, item.NombreDestino, item.FechaTraslado.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("es-MX")), item.Estado);
            _gridTrasladosVista.DataSource = tabla;
            _gridTrasladosVista.Columns["Numero"].HeaderText = "TRASLADO"; _gridTrasladosVista.Columns["Numero"].FillWeight = 22;
            _gridTrasladosVista.Columns["Origen"].HeaderText = "ORIGEN"; _gridTrasladosVista.Columns["Origen"].FillWeight = 24;
            _gridTrasladosVista.Columns["Destino"].HeaderText = "DESTINO"; _gridTrasladosVista.Columns["Destino"].FillWeight = 24;
            _gridTrasladosVista.Columns["Fecha"].HeaderText = "FECHA"; _gridTrasladosVista.Columns["Fecha"].FillWeight = 18;
            _gridTrasladosVista.Columns["Estado"].HeaderText = "ESTADO"; _gridTrasladosVista.Columns["Estado"].FillWeight = 18;
            _gridTrasladosVista.ClearSelection();
            int desde = _trasladosVisibles.Count == 0 ? 0 : ((_paginaTraslados - 1) * TrasladosPorPagina) + 1;
            int hasta = Math.Min(_paginaTraslados * TrasladosPorPagina, _trasladosVisibles.Count);
            _totalTrasladosVista.Text = "Mostrando " + desde + "-" + hasta + " de " + _trasladosVisibles.Count + " registros";
            _paginaTrasladosVista.Text = _paginaTraslados + " de " + paginas;
            _anteriorTrasladosVista.Enabled = _paginaTraslados > 1;
            _siguienteTrasladosVista.Enabled = _paginaTraslados < paginas;
            for (int i = 0; i < _gridTrasladosVista.Rows.Count; i++)
                _gridTrasladosVista.Rows[i].Tag = pagina[i];
        }

        // CAMBIAR PÁGINA: CambiarPaginaTraslados
        private void CambiarPaginaTraslados(int cambio)
        {
            _paginaTraslados += cambio;
            if (String.IsNullOrWhiteSpace(_buscarTrasladosVista.Text) || _buscarTrasladosVista.Text == "Buscar traslados")
                MostrarTrasladosVista(_trasladosCompletos);
            else
                FiltrarTrasladosVista();
        }

        private void AbrirTrasladoExistente(TrasladosEntidad item)
        {
            if (item == null) return;
            try
            {
                MtdCargarEnCampos(item);
                bool editable = !String.Equals(item.Estado, EstadosTraslado.Completado, StringComparison.OrdinalIgnoreCase);
                _modo = editable ? "Editar" : String.Empty;
                MtdHabilitarCampos(editable);
                btnEditar.Visible = false;
                btnNuevo.Visible = false;
                MostrarDialogoTraslado(false);
            }
            catch (Exception ex) { AyudaForms.MtdError(ex); }
        }

        // PRESENTAR DATOS: MtdMostrarLista
        private void MtdMostrarLista(List<TrasladosEntidad> lista)
        {
            dgvTraslados.DataSource = null;
            dgvTraslados.DataSource = lista;

            AyudaForms.MtdTitulo(dgvTraslados, "CodigoTraslado", "N°");
            AyudaForms.MtdTitulo(dgvTraslados, "NombreEmpleado", "Empleado");
            AyudaForms.MtdTitulo(dgvTraslados, "NombreOrigen", "Origen");
            AyudaForms.MtdTitulo(dgvTraslados, "NombreDestino", "Destino");
            AyudaForms.MtdTitulo(dgvTraslados, "FechaTraslado", "Fecha");
            AyudaForms.MtdOcultar(dgvTraslados, "CodigoEmpleado", "SucursalOrigen", "SucursalDestino");
            AyudaForms.MtdFormato(dgvTraslados, "FechaTraslado", "dd/MM/yyyy");

            lblTotalRegistros.Text = "Total Registros: " + lista.Count;
            MtdMostrarDetalleSeleccionado();
        }

        private TrasladosEntidad MtdTrasladoActual()
        {
            if (dgvTraslados.CurrentRow == null)
            {
                return null;
            }

            return dgvTraslados.CurrentRow.DataBoundItem as TrasladosEntidad;
        }

        // Muestra abajo el detalle del traslado que esta seleccionado en la lista
        // PRESENTAR DATOS: MtdMostrarDetalleSeleccionado
        private void MtdMostrarDetalleSeleccionado()
        {
            try
            {
                TrasladosEntidad actual = MtdTrasladoActual();

                if (actual == null)
                {
                    dgvDetalleConsulta.DataSource = null;
                    return;
                }

                dgvDetalleConsulta.DataSource = _trasladosNegocio.MtdConsultarDetalle(actual.CodigoTraslado);
                MtdConfigurarDetalle(dgvDetalleConsulta);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void dgvTraslados_SelectionChanged(object sender, EventArgs e)
        {
            MtdMostrarDetalleSeleccionado();
        }

        // BUSCAR: btnBuscar_Click
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                MtdMostrarLista(_trasladosNegocio.MtdBuscar(txtBuscar.Text));
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // LIMPIAR: btnLimpiarFiltro_Click
        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Clear();
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
                dgvTraslados.SelectAll();
            }
            else
            {
                dgvTraslados.ClearSelection();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdImprimir(dgvTraslados, "Traslados entre sucursales");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdExportarCsv(dgvTraslados, "Traslados");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Doble clic en una fila: la carga en la pestaña Gestionar
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            TrasladosEntidad item = dgvTraslados.Rows[e.RowIndex].DataBoundItem as TrasladosEntidad;

            if (item == null)
            {
                return;
            }

            AbrirTrasladoExistente(item);
        }

        // =====================================================
        // GESTIONAR: METODOS DE APOYO
        // =====================================================

        // CARGA DE DATOS: MtdCargarEnCampos
        private void MtdCargarEnCampos(TrasladosEntidad item)
        {
            _codigoSeleccionado = item.CodigoTraslado;
            _modo = string.Empty;

            txtCodigo.Text = item.CodigoTraslado.ToString();
            cboEmpleado.SelectedValue = item.CodigoEmpleado;
            cboOrigen.SelectedValue = item.SucursalOrigen;
            cboDestino.SelectedValue = item.SucursalDestino;
            dtpFecha.Value = item.FechaTraslado;
            txtEstado.Text = item.Estado;

            _detalles.Clear();
            foreach (DetalleTrasladoEntidad det in _trasladosNegocio.MtdConsultarDetalle(item.CodigoTraslado))
            {
                _detalles.Add(det);
            }

            MtdHabilitarCampos(false);
        }

        // LEER CAMPOS DEL FORMULARIO: Convierte los valores ingresados en una entidad de traslado.
        private TrasladosEntidad MtdObtenerDeCampos()
        {
            TrasladosEntidad traslado = new TrasladosEntidad();
            traslado.CodigoTraslado = _codigoSeleccionado;
            traslado.CodigoEmpleado = AyudaForms.MtdValorCombo(cboEmpleado);
            traslado.SucursalOrigen = AyudaForms.MtdValorCombo(cboOrigen);
            traslado.SucursalDestino = AyudaForms.MtdValorCombo(cboDestino);
            traslado.FechaTraslado = dtpFecha.Value.Date;
            traslado.Estado = txtEstado.Text;
            traslado.Detalles = _detalles.ToList();
            return traslado;
        }

        // LIMPIAR: MtdLimpiarCampos
        private void MtdLimpiarCampos()
        {
            txtCodigo.Clear();
            txtEstado.Clear();

            if (cboEmpleado.Items.Count > 0)
            {
                cboEmpleado.SelectedIndex = 0;
            }
            if (cboOrigen.Items.Count > 0)
            {
                cboOrigen.SelectedIndex = 0;
            }
            if (cboDestino.Items.Count > 0)
            {
                cboDestino.SelectedIndex = cboDestino.Items.Count > 1 ? 1 : 0;
            }
            if (cboProducto.Items.Count > 0)
            {
                cboProducto.SelectedIndex = 0;
            }

            dtpFecha.Value = DateTime.Now;
            nudCantidad.Value = 1;
            _detalles.Clear();
        }

        private void MtdHabilitarCampos(bool habilitar)
        {
            cboEmpleado.Enabled = habilitar;
            cboOrigen.Enabled = habilitar;
            cboDestino.Enabled = habilitar;
            dtpFecha.Enabled = habilitar;
            cboProducto.Enabled = habilitar;
            nudCantidad.Enabled = habilitar;
            btnAgregarDetalle.Enabled = habilitar;
            btnQuitarDetalle.Enabled = habilitar;
        }

        private void MtdVolverAModoConsulta()
        {
            _codigoSeleccionado = 0;
            _modo = string.Empty;
            MtdLimpiarCampos();
            MtdHabilitarCampos(false);
            btnNuevo.Visible = true;
            btnEditar.Visible = true;
            if (_dialogoTraslado != null && _dialogoTraslado.Visible) _dialogoTraslado.DialogResult = DialogResult.Cancel;
        }

        // =====================================================
        // GESTIONAR: DETALLE (PRODUCTOS DEL TRASLADO)
        // =====================================================

        // AGREGAR: btnAgregarDetalle_Click
        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            ComboItemEntidad producto = cboProducto.SelectedItem as ComboItemEntidad;

            if (producto == null)
            {
                AyudaForms.MtdAviso("Selecciona un producto.");
                return;
            }

            int cantidad = (int)nudCantidad.Value;

            // Si el producto ya esta en la lista, solo suma la cantidad
            foreach (DetalleTrasladoEntidad det in _detalles)
            {
                if (det.CodigoProducto == producto.Codigo)
                {
                    det.Cantidad += cantidad;
                    _detalles.ResetBindings();
                    nudCantidad.Value = 1;
                    return;
                }
            }

            _detalles.Add(new DetalleTrasladoEntidad(producto.Codigo, producto.Texto, cantidad));
            nudCantidad.Value = 1;
        }

        // ELIMINAR: btnQuitarDetalle_Click
        private void btnQuitarDetalle_Click(object sender, EventArgs e)
        {
            DetalleTrasladoEntidad det = null;

            if (dgvDetalleGestionar.CurrentRow != null)
            {
                det = dgvDetalleGestionar.CurrentRow.DataBoundItem as DetalleTrasladoEntidad;
            }

            if (det == null)
            {
                AyudaForms.MtdAviso("Selecciona el producto que quieres quitar de la lista.");
                return;
            }

            _detalles.Remove(det);
        }

        // =====================================================
        // GESTIONAR: BOTONES
        // =====================================================

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
            _codigoSeleccionado = 0;
            _modo = "Agregar";
            txtEstado.Text = EstadosTraslado.Pendiente;
            MtdHabilitarCampos(true);
            btnNuevo.Visible = false;
            btnEditar.Visible = false;
            MostrarDialogoTraslado(true);
            cboEmpleado.Focus();
        }

        // EDITAR: btnEditar_Click
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_codigoSeleccionado == 0)
            {
                AyudaForms.MtdAviso("Primero carga un traslado: ve a Consultar y haz doble clic en una fila.");
                return;
            }

            if (txtEstado.Text == EstadosTraslado.Completado)
            {
                AyudaForms.MtdAviso("El traslado ya fue completado y no se puede editar.");
                return;
            }

            _modo = "Editar";
            MtdHabilitarCampos(true);
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modo == string.Empty)
                {
                    AyudaForms.MtdAviso("Presiona Nuevo o Editar antes de guardar.");
                    return;
                }

                TrasladosEntidad traslado = MtdObtenerDeCampos();

                if (_modo == "Agregar")
                {
                    int codigoNuevo = _trasladosNegocio.MtdAgregar(traslado);
                    AyudaForms.MtdInfo("Traslado #" + codigoNuevo + " guardado en estado Pendiente.\n\nCuando se complete, cárgalo (doble clic en Consultar) y usa el botón Completar para mover el stock.");
                }
                else
                {
                    _trasladosNegocio.MtdEditar(traslado);
                    AyudaForms.MtdInfo("Traslado actualizado correctamente.");
                }

                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // ELIMINAR: btnEliminar_Click
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_codigoSeleccionado == 0)
                {
                    AyudaForms.MtdAviso("Primero carga un traslado: ve a Consultar y haz doble clic en una fila.");
                    return;
                }

                if (!AyudaForms.MtdConfirmar("¿Seguro que deseas eliminar el traslado #" + _codigoSeleccionado + "?"))
                {
                    return;
                }

                _trasladosNegocio.MtdEliminar(_codigoSeleccionado);

                AyudaForms.MtdInfo("Traslado eliminado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdVolverAModoConsulta();
        }

        // LIMPIAR: btnLimpiar_Click
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnCerrarGestionar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =====================================================
        // GESTIONAR: CAMBIAR ESTADO
        // Completar = resta stock en el origen, suma en el destino y registra el Kardex
        // =====================================================

        private void btnEnTransito_Click(object sender, EventArgs e)
        {
            MtdCambiarEstado(EstadosTraslado.EnTransito);
        }

        private void btnCompletar_Click(object sender, EventArgs e)
        {
            MtdCambiarEstado(EstadosTraslado.Completado);
        }

        private void MtdCambiarEstado(string nuevoEstado)
        {
            try
            {
                if (_codigoSeleccionado == 0)
                {
                    AyudaForms.MtdAviso("Primero carga un traslado: ve a Consultar y haz doble clic en una fila.");
                    return;
                }

                string pregunta = "¿Cambiar el traslado #" + _codigoSeleccionado + " a '" + nuevoEstado + "'?";

                if (nuevoEstado == EstadosTraslado.Completado)
                {
                    pregunta = "Se moverá el stock: se resta en " + cboOrigen.Text + " y se suma en " + cboDestino.Text +
                               ".\n\n¿Completar el traslado #" + _codigoSeleccionado + "?";
                }

                if (!AyudaForms.MtdConfirmar(pregunta))
                {
                    return;
                }

                _trasladosNegocio.MtdCambiarEstado(_codigoSeleccionado, nuevoEstado);

                txtEstado.Text = nuevoEstado;
                MtdConsultar();
                AyudaForms.MtdInfo("Estado actualizado a: " + nuevoEstado);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }
    }
}
