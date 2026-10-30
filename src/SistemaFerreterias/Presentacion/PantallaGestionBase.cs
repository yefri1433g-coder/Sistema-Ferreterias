using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE PantallaGestionBase: Presenta información y conecta acciones de usuario con la capa Negocio.
    public abstract class PantallaGestionBase : Form
    {
        private readonly CatalogosGestionNegocio negocio = new CatalogosGestionNegocio();
        private readonly TabControl moduloHost = new TabControl();
        private DataGridView gridCategorias, gridProductos, gridSucursales, gridCompra, gridHistorialCompras, gridDetalleCompra;
        private const int RegistrosPorPaginaSucursales = 10;
        private DataTable _sucursales = new DataTable();
        private int _paginaSucursal = 1;
        private Button btnSucursalAnterior, btnSucursalSiguiente;
        private Label lblPaginaSucursales;
        private const int RegistrosPorPaginaProductos = 10;
        private DataTable productosFiltrados = new DataTable();
        private int paginaProducto = 1;
        private Button btnProductoAnterior, btnProductoSiguiente;
        private Label lblPaginaProductos;
        private bool _vistaSucursalesLimpiada;
        private TextBox catNombre, catDescripcion;
        private const int RegistrosPorPaginaCategorias = 10;
        private DataTable categoriasFiltradas = new DataTable();
        private DataTable categoriasTodas = new DataTable();
        private int paginaCategoria = 1;
        private Button btnCategoriaAnterior, btnCategoriaSiguiente;
        private Label lblPaginaCategorias;
        private const int RegistrosPorPaginaCompras = 10;
        private DataTable comprasTodas = new DataTable();
        private DataTable comprasFiltradas = new DataTable();
        private int paginaCompra = 1;
        private TextBox filtroCompras;
        private Button btnCompraAnterior, btnCompraSiguiente;
        private Label lblPaginaCompras, lblTotalCompras;
        private Form dialogoCompra;
        private TableLayoutPanel layoutCompras;
        private Control panelCapturaCompra, panelDetalleCompra, panelAccionesCompra;
        private Button btnNuevoCompra;
        private Button btnEditarCompra, btnEliminarCompra, btnGuardarCompra;
        private int? compraIdEdicion;

        private int? catId;
        private TextBox prodCodigo, prodNombre, prodDescripcion;
        private NumericUpDown prodPrecio, prodCosto;
        private ComboBox prodCategoria;
        private CheckBox prodActivo;
        private int? prodId;
        private TextBox sucNombre, sucDireccion, sucTelefono;
        private CheckBox sucActivo;
        private int? sucId;
        private ComboBox compraProveedor, compraSucursal, compraProducto;
        private NumericUpDown compraCantidad, compraCosto;
        private DateTimePicker compraFecha;
        private Label compraTotal;
        private readonly Label[] valoresEstadisticas = new Label[3];
        private readonly Label[] titulosEstadisticas = new Label[3];
        private Form editorDialog;
        private TextBox filtroPantalla;
        private Label lblTotalRegistros;
        private Panel pieLista;
        private DataTable tablaCategorias, tablaProductos, sucursalesOriginales;
        private readonly List<CompraLineaCatalogo> lineas = new List<CompraLineaCatalogo>();

        protected PantallaGestionBase(string modulo)
        {
            if (String.IsNullOrWhiteSpace(modulo)) throw new ArgumentException("Indica la pantalla que deseas abrir.", "modulo");
            Text = modulo; Width = 1180; Height = 760; StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9F);
            moduloHost.Dock = DockStyle.Fill;
            moduloHost.Appearance = TabAppearance.FlatButtons;
            moduloHost.SizeMode = TabSizeMode.Fixed;
            moduloHost.ItemSize = new Size(0, 1);
            BackColor = Color.FromArgb(245, 245, 247);
            Controls.Add(moduloHost);
            AgregarEncabezado(modulo);
            if (modulo == "Categor\u00EDas") ConstruirCategorias();
            else if (modulo == "Productos") ConstruirProductos();
            else if (modulo == "Sucursales") ConstruirSucursales();
            else if (modulo == "Compras") ConstruirCompras();
            else throw new ArgumentException("La pantalla indicada no existe.", "modulo");
            AplicarEstiloGestion();
            if (editorDialog != null) AplicarEstiloGestion(editorDialog.Controls);
            EstiloAplicacion.Aplicar(this);
            Shown += (s, e) => { try { RefrescarTodo(); } catch (Exception ex) { MostrarError(ex); } };
        }

        // AGREGAR: AgregarEncabezado
        private void AgregarEncabezado(string modulo)
        {
            Panel encabezado = new Panel { Dock = DockStyle.Top, Height = 108, BackColor = EstiloAplicacion.Fondo };
            Label titulo = new Label { AutoSize = true, Text = modulo, Font = new Font("Segoe UI", 22F, FontStyle.Bold), ForeColor = Color.FromArgb(20, 38, 49), Location = new Point(20, 20) };
            Label subtitulo = new Label { AutoSize = true, Text = DescripcionModulo(modulo), Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(117, 134, 143), Location = new Point(22, 68) };
            encabezado.Controls.Add(titulo);
            encabezado.Controls.Add(subtitulo);
            Controls.Add(encabezado);
        }

        private static string DescripcionModulo(string modulo)
        {
            switch (modulo)
            {
                case "Categor\u00EDas": return "Mantén tu catálogo organizado y fácil de consultar.";
                case "Productos": return "Catálogo de artículos, precios y disponibilidad.";
                case "Sucursales": return "Administra ubicaciones y actividad por sucursal.";
                default: return "\u00D3rdenes de compra y recepci\u00F3n de mercanc\u00EDa.";
            }
        }        public void AbrirPestana(string nombre)
        {
            foreach (TabPage page in moduloHost.TabPages)
            {
                if (string.Equals(page.Text, nombre, StringComparison.Ordinal))
                {
                    moduloHost.SelectedTab = page;
                    return;
                }
            }
        }

        // AGREGAR: AgregarPagina
        private void AgregarPagina(TabPage page)
        {
            moduloHost.TabPages.Add(page);
        }
        // CREAR ELEMENTO VISUAL: CrearGrid
        private static DataGridView CrearGrid()
        {
            return new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White,
                RowHeadersVisible = false, AutoGenerateColumns = true,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True },
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells };
        }
        private static TextBox Campo(Panel panel, string texto, int y, out Label label, bool multiline = false)
        {
            return Campo(panel, texto, 15, y, 280, out label, multiline);
        }
        private static TextBox Campo(Panel panel, string texto, int x, int y, int width, out Label label, bool multiline = false)
        {
            label = new Label { Text = texto, Left = x, Top = y, Width = width, Height = 20 };
            TextBox box = new TextBox { Left = x, Top = y + 21, Width = width, Height = multiline ? 54 : 25, Multiline = multiline };
            panel.Controls.Add(label); panel.Controls.Add(box); return box;
        }
        private static void Estilizar(DataGridView grid, params string[] ocultar)
        {
            foreach (string col in ocultar) if (grid.Columns.Contains(col)) grid.Columns[col].Visible = false;
            foreach (DataGridViewColumn columna in grid.Columns) columna.HeaderText = columna.HeaderText.ToUpperInvariant();
        }
        private Panel Lateral(TabPage page, out DataGridView grid, int alturaFormulario)
        {
            bool listaSucursales = page.Text == "Sucursales";
            bool listaProductos = page.Text == "Productos";
            bool listaCategorias = page.Text == "Categor\u00EDas";
            bool estiloModerno = listaSucursales || listaProductos || listaCategorias;
            TableLayoutPanel distribucion = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = estiloModerno ? new Padding(12, 2, 12, 12) : new Padding(18, 4, 18, 18), BackColor = EstiloAplicacion.Fondo };
            distribucion.RowStyles.Add(new RowStyle(SizeType.Absolute, estiloModerno ? 98F : 82F));
            distribucion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            page.Controls.Add(distribucion);
            distribucion.Controls.Add(CrearTarjetasEstadisticas(estiloModerno), 0, 0);

            TableLayoutPanel lista = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = estiloModerno ? 3 : 4, Padding = estiloModerno ? new Padding(14, 10, 14, 10) : new Padding(22), BackColor = Color.White, BorderStyle = estiloModerno ? BorderStyle.None : BorderStyle.FixedSingle };
            lista.RowStyles.Add(new RowStyle(SizeType.Absolute, estiloModerno ? 55F : 64F));
            if (!estiloModerno) lista.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            lista.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            lista.RowStyles.Add(new RowStyle(SizeType.Absolute, estiloModerno ? 40F : 28F));
            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Label tituloLista = new Label { Text = page.Text + " recientes", Location = new Point(6, 2), AutoSize = true, Font = new Font("Segoe UI", estiloModerno ? 10F : 12F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            Label descripcionLista = new Label { Text = estiloModerno ? "Informaci\u00F3n actualizada \u00B7 " + DateTime.Today.ToString("dd MMM yyyy") : "Consulta y administra los registros guardados.", Location = new Point(7, 25), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            encabezado.Controls.Add(tituloLista);
            encabezado.Controls.Add(descripcionLista);

            FlowLayoutPanel barra = new FlowLayoutPanel { Dock = estiloModerno ? DockStyle.Right : DockStyle.Fill, Width = estiloModerno ? (listaProductos ? 475 : 390) : 335, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            Button nuevo = new Button { Text = "+ Nuevo registro", Width = estiloModerno ? 112 : 150, Height = 30, Margin = new Padding(6, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = EstiloAplicacion.Acento, ForeColor = Color.White, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            nuevo.FlatAppearance.BorderSize = 0;
            if (estiloModerno) Redondear(nuevo, 7);
            nuevo.Click += (s, e) => AbrirNuevoRegistro(page.Text);
            Button eliminar = new Button { Text = "Eliminar", Width = 82, Height = 30, Margin = new Padding(6, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(254, 242, 242), ForeColor = Color.FromArgb(185, 28, 28), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand, Visible = estiloModerno && (listaSucursales || listaProductos || listaCategorias) };
            Button editar = new Button { Text = "Editar", Width = 72, Height = 30, Margin = new Padding(6, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Tinta, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand, Visible = listaProductos, Enabled = false };
            editar.Visible = true;
            editar.Click += (s, e) => EditarRegistroDePantalla(page.Text,
                listaProductos ? gridProductos : listaSucursales ? gridSucursales : gridCategorias);
            filtroPantalla = new TextBox { Width = estiloModerno ? 160 : 250, Height = 30, Margin = new Padding(8, 0, 0, 0), Font = new Font("Segoe UI", 8F), BorderStyle = estiloModerno ? BorderStyle.FixedSingle : BorderStyle.Fixed3D };
            if (estiloModerno)
            {
                string textoAyuda = listaSucursales ? "Buscar sucursales" : listaProductos ? "Buscar productos" : "Buscar categor\u00EDas";
                filtroPantalla.Text = textoAyuda;
                filtroPantalla.ForeColor = EstiloAplicacion.Secundario;
                filtroPantalla.Enter += (s, e) => { if (filtroPantalla.Text == textoAyuda) { filtroPantalla.Clear(); filtroPantalla.ForeColor = EstiloAplicacion.Tinta; } };
                filtroPantalla.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(filtroPantalla.Text)) { filtroPantalla.Text = textoAyuda; filtroPantalla.ForeColor = EstiloAplicacion.Secundario; } };
                nuevo.Height = 30;
            }
            DataGridView gridLista = CrearGrid();
            grid = gridLista;
            eliminar.Enabled = false;
            eliminar.Click += (s, e) => EliminarRegistroDePantalla(page.Text, gridLista);
            gridLista.SelectionChanged += (s, e) => eliminar.Enabled = gridLista.CurrentRow != null;
            gridLista.SelectionChanged += (s, e) => editar.Enabled = gridLista.SelectedRows.Count == 1;
            filtroPantalla.TextChanged += (s, e) => AplicarFiltroPantalla(page.Text, gridLista);
            barra.Controls.Add(nuevo);
            barra.Controls.Add(eliminar);
            barra.Controls.Add(editar);
            barra.Controls.Add(filtroPantalla);
            pieLista = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            lblTotalRegistros = new Label { Dock = DockStyle.Left, Width = 220, TextAlign = ContentAlignment.MiddleLeft, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8.5F) };
            pieLista.Controls.Add(lblTotalRegistros);
            gridLista.DataBindingComplete += (s, e) => lblTotalRegistros.Text = "Total de registros: " + gridLista.Rows.Count;
            if (estiloModerno)
            {
                encabezado.Controls.Add(barra);
                Redondear(lista, 14);
            }
            lista.Controls.Add(encabezado, 0, 0);
            int filaGrid = estiloModerno ? 1 : 2;
            if (!estiloModerno) lista.Controls.Add(barra, 0, 1);
            lista.Controls.Add(gridLista, 0, filaGrid);
            lista.Controls.Add(pieLista, 0, filaGrid + 1);
            distribucion.Controls.Add(lista, 0, 1);

            int ancho = page.Text == "Productos" ? 980 : 650;
            int alto = page.Text == "Productos" ? 470 : Math.Max(380, alturaFormulario + 130);
            editorDialog = new Form { Text = page.Text + " · Nuevo registro", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(ancho, alto), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            Panel editor = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(18), BackColor = Color.White };
            editorDialog.Controls.Add(editor);
            BarraListado.Configurar(gridLista, filtroPantalla, page.Text,
                () => EditarRegistroDePantalla(page.Text, gridLista));
            Panel acciones = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.FromArgb(247, 250, 250), Padding = new Padding(18, 12, 18, 10) };
            Button cerrar = new Button { Text = "Cancelar", Width = 110, Height = 34, Dock = DockStyle.Right, DialogResult = DialogResult.Cancel };
            acciones.Controls.Add(cerrar); editorDialog.Controls.Add(acciones); editorDialog.CancelButton = cerrar;
            return editor;
        }

        private void EditarRegistroDePantalla(string modulo, DataGridView grid)
        {
            DataRowView fila = grid.CurrentRow.DataBoundItem as DataRowView;
            if (fila == null) return;
            if (modulo == "Productos") { EditarProductoSeleccionado(); return; }
            if (modulo == "Sucursales")
            {
                sucId = Convert.ToInt32(fila["Codigo"]);
                sucNombre.Text = Convert.ToString(fila["Nombre"]);
                sucDireccion.Text = Convert.ToString(fila["Direccion"]);
                sucTelefono.Text = Convert.ToString(fila["Telefono"]);
                sucActivo.Checked = Convert.ToBoolean(fila["Activo"]);
            }
            else
            {
                catId = Convert.ToInt32(fila["Codigo"]);
                catNombre.Text = Convert.ToString(fila["Nombre"]);
                catDescripcion.Text = Convert.ToString(fila["Descripcion"]);
            }
            AbrirEditor(modulo + " · Editar registro");
        }

        // ELIMINAR: EliminarRegistroDePantalla
        private void EliminarRegistroDePantalla(string modulo, DataGridView grid)
        {
            if (grid == null || grid.CurrentRow == null)
            {
                MessageBox.Show("Selecciona primero el registro que deseas eliminar.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (modulo == "Sucursales")
            {
                EliminarSucursal();
                return;
            }

            DialogResult respuesta = AyudaForms.MtdConfirmacion("¿Deseas eliminar el registro seleccionado?");
            if (respuesta != DialogResult.Yes) return;
            if (modulo == "Categorías") EliminarCategoria();
            else if (modulo == "Productos") EliminarProducto();
        }

        // AGREGAR: AbrirNuevoRegistro
        private void AbrirNuevoRegistro(string modulo)
        {
            switch (modulo)
            {
                case "Categor\u00EDas": LimpiarCategoria(); break;
                case "Productos": LimpiarProducto(); break;
                case "Sucursales": LimpiarSucursal(); break;
            }
            foreach (Control contenedor in editorDialog.Controls)
                foreach (Control campo in contenedor.Controls)
                    if (campo.Name.StartsWith("btnEliminar", StringComparison.Ordinal)) campo.Visible = false;
            AbrirEditor(modulo + " · Nuevo registro");
        }

        private void AbrirEditor(string titulo)
        {
            if (editorDialog == null) return;
            if (titulo.IndexOf("Editar", StringComparison.OrdinalIgnoreCase) >= 0)
                foreach (Control contenedor in editorDialog.Controls)
                    foreach (Control campo in contenedor.Controls)
                        if (campo.Name.StartsWith("btnEliminar", StringComparison.Ordinal)) campo.Visible = true;
            editorDialog.Text = titulo;
            EstiloAplicacion.AplicarDialogo(editorDialog);
            Form propietario = FindForm();
            if (propietario == this) propietario = null;
            if (propietario == null) editorDialog.ShowDialog();
            else editorDialog.ShowDialog(propietario);
        }

        // CONFIGURACIÓN: AplicarFiltroPantalla
        private void AplicarFiltroPantalla(string modulo, DataGridView grid)
        {
            if (modulo == "Categor\u00EDas") FiltrarCategoriasDePantalla();
            else if (modulo == "Productos") FiltrarProductosDePantalla();
            else if (modulo == "Sucursales") FiltrarSucursalesDePantalla();
            if (lblTotalRegistros != null && modulo != "Productos") lblTotalRegistros.Text = "Total de registros: " + grid.Rows.Count;
        }

        private static void FiltrarTabla(DataGridView grid, DataTable tabla, string criterio, params string[] columnas)
        {
            if (grid == null || tabla == null) return;
            string texto = (criterio ?? "").Trim().Replace("'", "''").Replace("%", "[%]").Replace("*", "[*]");
            string[] condiciones = new string[columnas.Length];
            for (int i = 0; i < columnas.Length; i++)
                condiciones[i] = "Convert([" + columnas[i] + "], 'System.String') LIKE '%" + texto + "%'";
            tabla.DefaultView.RowFilter = String.IsNullOrWhiteSpace(texto) ? "" : String.Join(" OR ", condiciones);
            grid.DataSource = tabla.DefaultView;
        }

        private void FiltrarSucursalesDePantalla()
        {
            if (sucursalesOriginales == null || gridSucursales == null) return;
            string criterio = filtroPantalla.Text == "Buscar sucursales" ? "" : filtroPantalla.Text;
            string texto = (criterio ?? "").Trim().Replace("'", "''").Replace("%", "[%]").Replace("*", "[*]");
            DataView vista = sucursalesOriginales.DefaultView;
            vista.RowFilter = String.IsNullOrWhiteSpace(texto) ? "" : "Convert([Nombre], 'System.String') LIKE '%" + texto + "%' OR Convert([Direccion], 'System.String') LIKE '%" + texto + "%'";
            _sucursales = vista.ToTable();
            _paginaSucursal = 1;
            _vistaSucursalesLimpiada = false;
            MostrarPaginaSucursal();
        }
        // CREAR ELEMENTO VISUAL: CrearTarjetasEstadisticas
        private Panel CrearTarjetasEstadisticas(bool estiloSucursales = false)
        {
            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.FromArgb(246, 249, 250), Padding = new Padding(0, 0, 0, 10) };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0), Padding = new Padding(16, 10, 8, 7) };
                if (estiloSucursales)
                {
                    Label icono = new Label { Text = i == 0 ? "\u2302" : i == 1 ? "\u25F7" : "\u25A4", Location = new Point(13, 12), Size = new Size(29, 29), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold), ForeColor = i == 1 ? Color.FromArgb(91, 130, 238) : EstiloAplicacion.Acento, BackColor = EstiloAplicacion.FondoSuave };
                    titulosEstadisticas[i] = new Label { Left = 52, Top = 13, AutoSize = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario, Tag = "stat-title" };
                    valoresEstadisticas[i] = new Label { Left = 52, Top = 34, AutoSize = true, Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta, Tag = "stat-value" };
                    tarjeta.Controls.Add(icono);
                    tarjeta.Controls.Add(titulosEstadisticas[i]);
                    tarjeta.Controls.Add(valoresEstadisticas[i]);
                    Redondear(tarjeta, 12);
                    Redondear(icono, 8);
                }
                else
                {
                    titulosEstadisticas[i] = new Label { Dock = DockStyle.Top, Height = 20, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario, Tag = "stat-title" };
                    valoresEstadisticas[i] = new Label { Dock = DockStyle.Top, Height = 35, Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta, Tag = "stat-value" };
                    tarjeta.Controls.Add(valoresEstadisticas[i]);
                    tarjeta.Controls.Add(titulosEstadisticas[i]);
                }
                tarjetas.Controls.Add(tarjeta, i, 0);
            }
            return tarjetas;
        }

        // EDITAR: ActualizarEstadisticas
        private void ActualizarEstadisticas(string[] titulos, string[] valores)
        {
            for (int i = 0; i < 3; i++)
            {
                if (titulosEstadisticas[i] == null) continue;
                titulosEstadisticas[i].Text = titulos[i];
                valoresEstadisticas[i].Text = valores[i];
            }
        }
        private static void Redondear(Control control, int radio)
        {
            EventHandler ajustar = (s, e) =>
            {
                if (control.Width < 2 || control.Height < 2) return;
                int diametro = Math.Min(radio * 2, Math.Min(control.Width, control.Height));
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
            control.Resize += ajustar;
            ajustar(control, EventArgs.Empty);
        }
        // CONFIGURACIÓN: AplicarEstiloGestion
        private void AplicarEstiloGestion()
        {
            bool esSucursales = Text == "Sucursales";
            BackColor = esSucursales ? EstiloAplicacion.Fondo : Color.FromArgb(246, 249, 250);
            AplicarEstiloGestion(Controls, esSucursales);
        }
        // CONFIGURACIÓN: AplicarEstiloGestion
        private static void AplicarEstiloGestion(Control.ControlCollection controles, bool esSucursales = false)
        {
            foreach (Control control in controles)
            {
                Panel panel = control as Panel;
                if (panel != null) panel.BackColor = Color.White;
                TabControl pestanas = control as TabControl;
                if (pestanas != null) pestanas.BackColor = esSucursales ? EstiloAplicacion.Fondo : EstiloAplicacion.Fondo;
                TabPage pagina = control as TabPage;
                if (pagina != null) pagina.BackColor = esSucursales ? EstiloAplicacion.Fondo : EstiloAplicacion.Fondo;
                TableLayoutPanel tabla = control as TableLayoutPanel;
                if (tabla != null) tabla.BackColor = esSucursales ? EstiloAplicacion.Fondo : EstiloAplicacion.Fondo;
                Label etiqueta = control as Label;
                if (etiqueta != null && etiqueta.Font.Size < 14F && etiqueta.Tag as string != "stat-title" && etiqueta.Tag as string != "list-title") etiqueta.ForeColor = Color.FromArgb(89, 107, 117);
                TextBox texto = control as TextBox;
                if (texto != null) { texto.BorderStyle = BorderStyle.FixedSingle; texto.BackColor = Color.White; texto.ForeColor = EstiloAplicacion.Tinta; }
                ComboBox combo = control as ComboBox;
                if (combo != null) { combo.FlatStyle = FlatStyle.Flat; combo.BackColor = Color.White; combo.ForeColor = EstiloAplicacion.Tinta; }
                NumericUpDown numero = control as NumericUpDown;
                if (numero != null) { numero.BorderStyle = BorderStyle.FixedSingle; numero.BackColor = Color.White; }
                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.Padding = new Padding(8, 2, 4, 2);
                    grid.DefaultCellStyle.ForeColor = Color.FromArgb(67, 87, 94);
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 248, 241);
                    grid.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 252);
                    grid.GridColor = Color.FromArgb(237, 241, 242);
                    grid.ColumnHeadersHeight = 36;
                    grid.RowTemplate.Height = 34;
                }
                Button boton = control as Button;
                if (boton != null)
                {
                    boton.FlatStyle = FlatStyle.Flat;
                    boton.FlatAppearance.BorderSize = 1;
                    boton.FlatAppearance.BorderColor = Color.FromArgb(228, 234, 237);
                    boton.UseVisualStyleBackColor = false;
                    boton.Cursor = Cursors.Hand;
                    string nombreBoton = boton.Text.ToLowerInvariant();
                    if (nombreBoton.Contains("eliminar") || nombreBoton.Contains("quitar"))
                    {
                        boton.BackColor = Color.FromArgb(255, 244, 244);
                        boton.ForeColor = Color.FromArgb(185, 45, 45);
                    }
                    else if (nombreBoton.Contains("guardar") || nombreBoton.Contains("buscar") || nombreBoton.Contains("agregar") || nombreBoton.Contains("nuevo"))
                    {
                        boton.BackColor = EstiloAplicacion.Acento;
                        boton.ForeColor = Color.White;
                        boton.FlatAppearance.BorderColor = EstiloAplicacion.Acento;
                    }
                    else
                    {
                        boton.BackColor = Color.White;
                        boton.ForeColor = EstiloAplicacion.Tinta;
                    }
                }
                if (control.HasChildren) AplicarEstiloGestion(control.Controls, esSucursales);
            }
        }
        private static Button Boton(Panel panel, string caption, int x, int y, int width, EventHandler click)
        {
            Button b = new Button { Text = caption, Left = x, Top = y, Width = width, Height = 34 };
            b.Click += click; panel.Controls.Add(b); return b;
        }

        // CONFIGURACIÓN: ConstruirCategorias
        private void ConstruirCategorias()
        {
            TabPage page = new TabPage("Categor\u00EDas"); AgregarPagina(page); Panel p = Lateral(page, out gridCategorias, 240);
            Panel barraPaginas = new Panel { Dock = DockStyle.Right, Width = 325, BackColor = Color.White };
            btnCategoriaAnterior = new Button { Text = "\u2039 Anterior", Left = 0, Top = 4, Width = 90, Height = 32, Enabled = false };
            lblPaginaCategorias = new Label { Text = "P\u00E1gina 1 de 1", Left = 94, Top = 8, Width = 115, Height = 20, TextAlign = ContentAlignment.MiddleCenter, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F) };
            btnCategoriaSiguiente = new Button { Text = "Siguiente \u203A", Left = 211, Top = 4, Width = 100, Height = 32, Enabled = false };
            btnCategoriaAnterior.Click += (s, e) => CambiarPaginaCategoria(-1);
            btnCategoriaSiguiente.Click += (s, e) => CambiarPaginaCategoria(1);
            barraPaginas.Controls.Add(btnCategoriaAnterior); barraPaginas.Controls.Add(lblPaginaCategorias); barraPaginas.Controls.Add(btnCategoriaSiguiente);
            pieLista.Controls.Add(barraPaginas);
            catNombre = Campo(p, "Nombre *", 15, 12, 460, out _); catNombre.MaxLength = 60;
            catDescripcion = Campo(p, "Descripción", 70, out _, true); catDescripcion.MaxLength = 150;
            Boton(p, "Guardar", 15, 155, 130, (s,e) => { if (GuardarCategoria()) editorDialog.Close(); });
            Boton(p, "Limpiar", 160, 155, 130, (s,e) => LimpiarCategoria());
            Button eliminarCategoria = Boton(p, "Eliminar", 305, 155, 130, (s,e) => { if (EliminarCategoria()) editorDialog.Close(); });
            eliminarCategoria.Name = "btnEliminarCategoria"; eliminarCategoria.Visible = false;
            gridCategorias.CellDoubleClick += (s,e) =>
            {
                if (e.RowIndex < 0) return;
                DataRowView r = gridCategorias.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (r == null) return;
                catId = Convert.ToInt32(r["Codigo"]);
                catNombre.Text = Convert.ToString(r["Nombre"]);
                catDescripcion.Text = Convert.ToString(r["Descripcion"]);
                AbrirEditor("Categorías · Editar registro");
            };
        }

        // CREAR ELEMENTO VISUAL: CrearVistaCategorias
        private static DataTable CrearVistaCategorias(DataTable categorias, DataTable productos)
        {
            DataTable vista = categorias.Copy();
            vista.Columns.Add("Productos", typeof(int));
            foreach (DataRow categoria in vista.Rows)
            {
                int idCategoria = Convert.ToInt32(categoria["Codigo"]);
                int cantidad = 0;
                foreach (DataRow producto in productos.Rows)
                    if (productos.Columns.Contains("IdCategoria") && !producto.IsNull("IdCategoria") && Convert.ToInt32(producto["IdCategoria"]) == idCategoria) cantidad++;
                categoria["Productos"] = cantidad;
            }
            return vista;
        }

        private void FiltrarCategoriasDePantalla()
        {
            if (categoriasFiltradas == null || gridCategorias == null) return;
            string texto = filtroPantalla == null || filtroPantalla.Text == "Buscar categor\u00EDas" ? "" : filtroPantalla.Text.Trim();
            string criterio = texto.Replace("'", "''").Replace("%", "[%]").Replace("*", "[*]");
            DataView vista = new DataView(categoriasTodas);
            vista.RowFilter = String.IsNullOrWhiteSpace(criterio) ? "" : "Convert([Nombre], 'System.String') LIKE '%" + criterio + "%' OR Convert([Descripcion], 'System.String') LIKE '%" + criterio + "%'";
            categoriasFiltradas = vista.ToTable();
            paginaCategoria = 1;
            MostrarPaginaCategorias();
        }

        // CAMBIAR PÁGINA: CambiarPaginaCategoria
        private void CambiarPaginaCategoria(int cambio)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(categoriasFiltradas.Rows.Count / (double)RegistrosPorPaginaCategorias));
            int nueva = paginaCategoria + cambio;
            if (nueva < 1 || nueva > paginas) return;
            paginaCategoria = nueva;
            MostrarPaginaCategorias();
        }

        // PRESENTAR DATOS: MostrarPaginaCategorias
        private void MostrarPaginaCategorias()
        {
            if (gridCategorias == null || categoriasFiltradas == null) return;
            int paginas = Math.Max(1, (int)Math.Ceiling(categoriasFiltradas.Rows.Count / (double)RegistrosPorPaginaCategorias));
            paginaCategoria = Math.Min(paginaCategoria, paginas);
            int inicio = (paginaCategoria - 1) * RegistrosPorPaginaCategorias;
            int fin = Math.Min(inicio + RegistrosPorPaginaCategorias, categoriasFiltradas.Rows.Count);
            DataTable pagina = categoriasFiltradas.Clone();
            for (int i = inicio; i < fin; i++) pagina.ImportRow(categoriasFiltradas.Rows[i]);
            gridCategorias.DataSource = pagina;
            Estilizar(gridCategorias, "Codigo");
            ConfigurarGridCategorias();
            lblTotalRegistros.Text = "Mostrando " + Math.Max(0, fin - inicio) + " de " + categoriasFiltradas.Rows.Count + " categorías";
            lblPaginaCategorias.Text = "Página " + paginaCategoria + " de " + paginas;
            btnCategoriaAnterior.Enabled = paginaCategoria > 1;
            btnCategoriaSiguiente.Enabled = paginaCategoria < paginas;
        }

        // CONFIGURACIÓN: ConfigurarGridCategorias
        private void ConfigurarGridCategorias()
        {
            gridCategorias.BackgroundColor = Color.White;
            gridCategorias.BorderStyle = BorderStyle.None;
            gridCategorias.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridCategorias.GridColor = Color.FromArgb(234, 240, 240);
            gridCategorias.EnableHeadersVisualStyles = false;
            gridCategorias.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridCategorias.ColumnHeadersHeight = 34;
            gridCategorias.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            gridCategorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridCategorias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            gridCategorias.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            gridCategorias.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            gridCategorias.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
            gridCategorias.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            gridCategorias.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            gridCategorias.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            gridCategorias.DefaultCellStyle.Padding = new Padding(5, 5, 5, 5);
            gridCategorias.RowTemplate.Height = 42;
            gridCategorias.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridCategorias.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            if (gridCategorias.Columns.Contains("Nombre")) { gridCategorias.Columns["Nombre"].HeaderText = "CATEGORÍA"; gridCategorias.Columns["Nombre"].FillWeight = 27; }
            if (gridCategorias.Columns.Contains("Descripcion")) { gridCategorias.Columns["Descripcion"].HeaderText = "DESCRIPCIÓN"; gridCategorias.Columns["Descripcion"].FillWeight = 43; }
            if (gridCategorias.Columns.Contains("Productos")) { gridCategorias.Columns["Productos"].HeaderText = "PRODUCTOS"; gridCategorias.Columns["Productos"].FillWeight = 18; gridCategorias.Columns["Productos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }
            gridCategorias.ClearSelection();
        }
        // CONFIGURACIÓN: ConstruirProductos
        private void ConstruirProductos()
        {
            TabPage page = new TabPage("Productos"); AgregarPagina(page); Panel p = Lateral(page, out gridProductos, 270);
            Panel barraPaginas = new Panel { Dock = DockStyle.Right, Width = 325, BackColor = Color.White };
            btnProductoAnterior = new Button { Text = "\u2039 Anterior", Left = 0, Top = 4, Width = 90, Height = 32, Enabled = false };
            lblPaginaProductos = new Label { Text = "P\u00E1gina 1 de 1", Left = 94, Top = 8, Width = 115, Height = 20, TextAlign = ContentAlignment.MiddleCenter, ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F) };
            btnProductoSiguiente = new Button { Text = "Siguiente \u203A", Left = 211, Top = 4, Width = 100, Height = 32, Enabled = false };
            btnProductoAnterior.Click += (s, e) => CambiarPaginaProducto(-1);
            btnProductoSiguiente.Click += (s, e) => CambiarPaginaProducto(1);
            barraPaginas.Controls.Add(btnProductoAnterior); barraPaginas.Controls.Add(lblPaginaProductos); barraPaginas.Controls.Add(btnProductoSiguiente);
            pieLista.Controls.Add(barraPaginas);
            int anchoCampo = 440;
            prodCodigo = Campo(p, "Código / SKU *", 15, 12, anchoCampo, out _); prodCodigo.MaxLength = 30;
            prodNombre = Campo(p, "Nombre *", 15, 70, anchoCampo, out _); prodNombre.MaxLength = 100;
            prodDescripcion = Campo(p, "Descripción", 15, 128, anchoCampo, out _, true); prodDescripcion.MaxLength = 200;
            Label lc = new Label { Text="Categoría *",Left=490,Top=12,Width=440,Height=20}; p.Controls.Add(lc);
            prodCategoria = new ComboBox { Left=490,Top=33,Width=440,DropDownStyle=ComboBoxStyle.DropDownList,DisplayMember="Nombre",ValueMember="Codigo"}; p.Controls.Add(prodCategoria);
            Label lcosto = new Label { Text="Precio de costo",Left=490,Top=70,Width=205,Height=20}; p.Controls.Add(lcosto);
            prodCosto = new NumericUpDown { Left=490,Top=91,Width=205,DecimalPlaces=2,Maximum=100000000,ThousandsSeparator=true}; p.Controls.Add(prodCosto);
            Label lp = new Label { Text="Precio de venta",Left=725,Top=70,Width=205,Height=20}; p.Controls.Add(lp);
            prodPrecio = new NumericUpDown { Left=725,Top=91,Width=205,DecimalPlaces=2,Maximum=100000000,ThousandsSeparator=true}; p.Controls.Add(prodPrecio);
            prodActivo = new CheckBox { Text="Activo",Left=490,Top=137,Checked=true,AutoSize=true}; p.Controls.Add(prodActivo);
            Boton(p,"Guardar",490,216,130,(s,e)=>{ if (GuardarProducto()) editorDialog.Close(); });
            Boton(p,"Limpiar",635,216,130,(s,e)=>LimpiarProducto());
            Button eliminarProducto = Boton(p,"Eliminar",780,216,130,(s,e)=>{ if (EliminarProducto()) editorDialog.Close(); });
            eliminarProducto.Name = "btnEliminarProducto"; eliminarProducto.Visible = false;
            gridProductos.CellDoubleClick += (s,e) =>
            {
                if (e.RowIndex < 0) return;
                CargarProductoParaEditar(gridProductos.Rows[e.RowIndex]);
            };
        }

        // EDITAR: EditarProductoSeleccionado
        private void EditarProductoSeleccionado()
        {
            DataGridViewRow fila = gridProductos != null && gridProductos.SelectedRows.Count > 0
                ? gridProductos.SelectedRows[0]
                : null;
            if (fila == null)
            {
                MessageBox.Show("Selecciona primero el producto que deseas editar.", "Editar producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CargarProductoParaEditar(fila);
        }

        // EDITAR: CargarProductoParaEditar
        private void CargarProductoParaEditar(DataGridViewRow fila)
        {
            DataRowView registro = fila == null ? null : fila.DataBoundItem as DataRowView;
            if (registro == null) return;
            prodId = Convert.ToInt32(registro["Codigo"]);
            prodCodigo.Text = Convert.ToString(registro["SKU"]);
            prodNombre.Text = Convert.ToString(registro["Nombre"]);
            prodDescripcion.Text = Convert.ToString(registro["Descripcion"]);
            prodCosto.Value = Math.Max(prodCosto.Minimum, Math.Min(prodCosto.Maximum, Convert.ToDecimal(registro["Costo"])));
            prodPrecio.Value = Math.Max(prodPrecio.Minimum, Math.Min(prodPrecio.Maximum, Convert.ToDecimal(registro["Precio"])));
            prodCategoria.SelectedValue = Convert.ToInt32(registro["IdCategoria"]);
            prodActivo.Checked = Convert.ToBoolean(registro["Activo"]);
            AbrirEditor("Productos · Editar registro");
        }

        private void FiltrarProductosDePantalla()
        {
            if (tablaProductos == null || gridProductos == null) return;
            string criterio = filtroPantalla.Text == "Buscar productos" ? "" : filtroPantalla.Text;
            string texto = (criterio ?? "").Trim().Replace("'", "''").Replace("%", "[%]").Replace("*", "[*]");
            DataView vista = new DataView(tablaProductos);
            vista.RowFilter = String.IsNullOrWhiteSpace(texto)
                ? ""
                : "Convert([Codigo], 'System.String') LIKE '%" + texto + "%' OR "
                    + "Convert([SKU], 'System.String') LIKE '%" + texto + "%' OR "
                    + "Convert([Nombre], 'System.String') LIKE '%" + texto + "%' OR "
                    + "Convert([Categoria], 'System.String') LIKE '%" + texto + "%'";
            productosFiltrados = vista.ToTable();
            paginaProducto = 1;
            MostrarPaginaProductos();
        }

        // CAMBIAR PÁGINA: CambiarPaginaProducto
        private void CambiarPaginaProducto(int cambio)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(productosFiltrados.Rows.Count / (double)RegistrosPorPaginaProductos));
            int nueva = paginaProducto + cambio;
            if (nueva < 1 || nueva > paginas) return;
            paginaProducto = nueva;
            MostrarPaginaProductos();
        }

        // PRESENTAR DATOS: MostrarPaginaProductos
        private void MostrarPaginaProductos()
        {
            if (gridProductos == null || productosFiltrados == null) return;
            int paginas = Math.Max(1, (int)Math.Ceiling(productosFiltrados.Rows.Count / (double)RegistrosPorPaginaProductos));
            paginaProducto = Math.Min(paginaProducto, paginas);
            int inicio = (paginaProducto - 1) * RegistrosPorPaginaProductos;
            int fin = Math.Min(inicio + RegistrosPorPaginaProductos, productosFiltrados.Rows.Count);
            DataTable pagina = productosFiltrados.Clone();
            for (int i = inicio; i < fin; i++) pagina.ImportRow(productosFiltrados.Rows[i]);
            gridProductos.DataSource = pagina;
            Estilizar(gridProductos, "Codigo", "IdCategoria", "Descripcion", "Costo", "Activo", "StockBajo");
            ConfigurarGridProductos();
            lblTotalRegistros.Text = "Mostrando " + Math.Max(0, fin - inicio) + " de " + productosFiltrados.Rows.Count + " productos";
            lblPaginaProductos.Text = "P\u00E1gina " + paginaProducto + " de " + paginas;
            btnProductoAnterior.Enabled = paginaProducto > 1;
            btnProductoSiguiente.Enabled = paginaProducto < paginas;
        }

        // CONFIGURACIÓN: ConfigurarGridProductos
        private void ConfigurarGridProductos()
        {
            gridProductos.BackgroundColor = Color.White;
            gridProductos.BorderStyle = BorderStyle.None;
            gridProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridProductos.GridColor = Color.FromArgb(234, 240, 240);
            gridProductos.EnableHeadersVisualStyles = false;
            gridProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridProductos.ColumnHeadersHeight = 34;
            gridProductos.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            gridProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            gridProductos.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            gridProductos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            gridProductos.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
            gridProductos.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            gridProductos.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            gridProductos.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            gridProductos.DefaultCellStyle.Padding = new Padding(5, 5, 5, 5);
            gridProductos.RowTemplate.Height = 42;
            gridProductos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridProductos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            if (gridProductos.Columns.Contains("Nombre")) { gridProductos.Columns["Nombre"].HeaderText = "PRODUCTO"; gridProductos.Columns["Nombre"].FillWeight = 27; }
            if (gridProductos.Columns.Contains("SKU")) { gridProductos.Columns["SKU"].HeaderText = "CÓDIGO"; gridProductos.Columns["SKU"].FillWeight = 18; }
            if (gridProductos.Columns.Contains("Categoria")) { gridProductos.Columns["Categoria"].HeaderText = "CATEGORÍA"; gridProductos.Columns["Categoria"].FillWeight = 22; }
            if (gridProductos.Columns.Contains("Precio")) { gridProductos.Columns["Precio"].HeaderText = "PRECIO"; gridProductos.Columns["Precio"].FillWeight = 16; gridProductos.Columns["Precio"].DefaultCellStyle.Format = "'Q' #,##0.00"; }
            if (gridProductos.Columns.Contains("Existencia")) { gridProductos.Columns["Existencia"].HeaderText = "EXISTENCIA"; gridProductos.Columns["Existencia"].FillWeight = 17; gridProductos.Columns["Existencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }
            gridProductos.ClearSelection();
        }

        // CONFIGURACIÓN: ConstruirSucursales
        private void ConstruirSucursales()
        {
            TabPage page = new TabPage("Sucursales"); AgregarPagina(page); Panel p=Lateral(page,out gridSucursales,300);
            sucNombre=Campo(p,"Nombre *",12,out _); sucNombre.MaxLength=80;
            sucNombre.KeyDown += (s,e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                BuscarSucursales();
            };
            sucDireccion=Campo(p,"Direcci\u00F3n *",66,out _); sucDireccion.MaxLength=150;
            sucTelefono=Campo(p,"Tel\u00E9fono",120,out _); sucTelefono.MaxLength=15;
            sucActivo=new CheckBox { Text="Activa",Left=15,Top=174,Checked=true,AutoSize=true};p.Controls.Add(sucActivo);
            Boton(p,"Guardar",15,207,130,(s,e)=>{ if (GuardarSucursal()) editorDialog.Close(); });Boton(p,"Limpiar",160,207,130,(s,e)=>LimpiarTablaSucursales());Boton(p,"Buscar",305,207,130,(s,e)=>BuscarSucursales());Button eliminarSucursal=Boton(p,"Eliminar",15,249,130,(s,e)=>{ if (EliminarSucursal()) editorDialog.Close(); });eliminarSucursal.Name="btnEliminarSucursal";eliminarSucursal.Visible=false;
            Panel barraPaginas = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            btnSucursalAnterior = new Button { Text = "\u2039 Anterior", Left = 12, Top = 4, Width = 92, Height = 32, Enabled = false };
            btnSucursalSiguiente = new Button { Text = "Siguiente \u203A", Left = 110, Top = 4, Width = 92, Height = 32, Enabled = false };
            lblPaginaSucursales = new Label { Left = 208, Top = 7, Width = 230, Height = 20, AutoEllipsis = false, Text = "P\u00E1gina 1 de 1" };
            btnSucursalAnterior.Click += (s, e) => CambiarPaginaSucursal(-1);
            btnSucursalSiguiente.Click += (s, e) => CambiarPaginaSucursal(1);
            barraPaginas.Controls.Add(btnSucursalAnterior);
            barraPaginas.Controls.Add(btnSucursalSiguiente);
            barraPaginas.Controls.Add(lblPaginaSucursales);
            barraPaginas.Dock = DockStyle.Right;
            barraPaginas.Width = 450;
            pieLista.Controls.Add(barraPaginas);
            gridSucursales.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || gridSucursales.Columns[e.ColumnIndex].Name != "Activo" || e.Value == null) return;
                bool activa = Convert.ToBoolean(e.Value);
                e.Value = activa ? "Activa" : "Inactiva";
                e.FormattingApplied = true;
                e.CellStyle.ForeColor = activa ? EstiloAplicacion.Acento : Color.FromArgb(160, 107, 102);
            };
            gridSucursales.CellDoubleClick += (s,e)=>{if(e.RowIndex<0)return;DataRowView r=gridSucursales.Rows[e.RowIndex].DataBoundItem as DataRowView;if(r==null)return;sucId=Convert.ToInt32(r["Codigo"]);sucNombre.Text=Convert.ToString(r["Nombre"]);sucDireccion.Text=Convert.ToString(r["Direccion"]);sucTelefono.Text=Convert.ToString(r["Telefono"]);sucActivo.Checked=Convert.ToBoolean(r["Activo"]);AbrirEditor("Sucursales · Editar registro");};
        }
        // CONFIGURACIÓN: ConstruirCompras
        private void ConstruirCompras()
        {
            TabPage page=new TabPage("Compras");AgregarPagina(page);
            TableLayoutPanel contenedor = new TableLayoutPanel { Dock=DockStyle.Fill, ColumnCount=1, RowCount=2, Padding=new Padding(12, 2, 12, 12), BackColor=EstiloAplicacion.Fondo };
            contenedor.RowStyles.Add(new RowStyle(SizeType.Absolute, 98F));
            contenedor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contenedor.Controls.Add(CrearTarjetasEstadisticas(true), 0, 0);
            layoutCompras = new TableLayoutPanel { Dock=DockStyle.Fill, ColumnCount=1, RowCount=1, Padding=new Padding(14, 10, 14, 10), BackColor=Color.White };
            TableLayoutPanel layout = layoutCompras;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Panel top=new Panel { Dock=DockStyle.Fill,BackColor=Color.FromArgb(246,249,248),Padding=new Padding(12)};
            panelCapturaCompra = top;
            compraProveedor=Combo(top,"Proveedor",15,12,220); compraSucursal=Combo(top,"Sucursal",255,12,220); compraFecha=new DateTimePicker{Left=495,Top=37,Width=150,Format=DateTimePickerFormat.Short}; top.Controls.Add(new Label{Text="Fecha",Left=495,Top=12,Width=140});top.Controls.Add(compraFecha);
            compraProducto=Combo(top,"Producto",15,68,220); compraCantidad=new NumericUpDown{Left=255,Top=93,Width=90,Minimum=1,Maximum=100000,Value=1}; top.Controls.Add(new Label{Text="Cantidad",Left=255,Top=68,Width=90});top.Controls.Add(compraCantidad);
            compraCosto=new NumericUpDown{Left=365,Top=93,Width=110,DecimalPlaces=2,Maximum=100000000,ThousandsSeparator=true};top.Controls.Add(new Label{Text="Costo unitario",Left=365,Top=68,Width=110});top.Controls.Add(compraCosto);
            Boton(top,"Agregar",495,86,110,(s,e)=>AgregarLinea()); compraTotal=new Label{Text="Total: Q 0.00",Left=690,Top=38,Width=220,Font=new Font("Segoe UI",12,FontStyle.Bold),ForeColor=Color.DarkGreen};top.Controls.Add(compraTotal);

            Panel panelLineas = CrearSeccionCompra("Líneas de la compra", out gridCompra);
            panelDetalleCompra = panelLineas;
            gridCompra.DataSource = TablaCompra();
            Estilizar(gridCompra, "IdProducto");
            Panel panelHistorial = CrearPanelHistorialCompras();

            Panel foot=new Panel{Dock=DockStyle.Fill};
            panelAccionesCompra = foot;
            btnGuardarCompra = Boton(foot,"Guardar compra y actualizar inventario",15,8,300,(s,e)=>GuardarCompra());
            Boton(foot,"Quitar línea",330,8,140,(s,e)=>QuitarLinea());

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Controls.Add(panelHistorial, 0, 0);
            dialogoCompra = new Form { Text = "Nueva compra", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false, ClientSize = new Size(980, 560), BackColor = EstiloAplicacion.Fondo, Font = new Font("Segoe UI", 9F) };
            TableLayoutPanel formularioCompra = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(14), BackColor = EstiloAplicacion.Fondo };
            formularioCompra.RowStyles.Add(new RowStyle(SizeType.Absolute, 145F));
            formularioCompra.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            formularioCompra.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            formularioCompra.Controls.Add(top, 0, 0);
            formularioCompra.Controls.Add(panelLineas, 0, 1);
            formularioCompra.Controls.Add(foot, 0, 2);
            dialogoCompra.Controls.Add(formularioCompra);
            contenedor.Controls.Add(layout, 0, 1);
            page.Controls.Add(contenedor);
        }

        // CREAR ELEMENTO VISUAL: CrearPanelHistorialCompras
        private Panel CrearPanelHistorialCompras()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
            Panel encabezado = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
            Label titulo = new Label { Text = "Compras recientes", Left = 4, Top = 1, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            Label descripcion = new Label { Text = "Historial de compras y recepción", Left = 5, Top = 23, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 515, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, 3, 0, 0) };
            filtroCompras = new TextBox { Width = 190, Height = 28, Margin = new Padding(8, 0, 0, 0), Text = "Buscar compras", ForeColor = EstiloAplicacion.Secundario, Font = new Font("Segoe UI", 8F), BorderStyle = BorderStyle.FixedSingle };
            filtroCompras.Enter += (s, e) => { if (filtroCompras.Text == "Buscar compras") { filtroCompras.Clear(); filtroCompras.ForeColor = EstiloAplicacion.Tinta; } };
            filtroCompras.Leave += (s, e) => { if (String.IsNullOrWhiteSpace(filtroCompras.Text)) { filtroCompras.Text = "Buscar compras"; filtroCompras.ForeColor = EstiloAplicacion.Secundario; } };
            filtroCompras.TextChanged += (s, e) => FiltrarComprasDePantalla();
            btnNuevoCompra = new Button { Text = "+ Nuevo registro", Width = 128, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = EstiloAplicacion.Acento, ForeColor = Color.White, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnNuevoCompra.FlatAppearance.BorderSize = 0;
            btnNuevoCompra.Click += (s, e) =>
            {
                compraIdEdicion = null;
                dialogoCompra.Text = "Nueva compra";
                btnGuardarCompra.Text = "Guardar compra y actualizar inventario";
                compraFecha.Value = DateTime.Today;
                lineas.Clear(); MostrarLineas();
                EstiloAplicacion.AplicarDialogo(dialogoCompra);
                dialogoCompra.ShowDialog(this);
            };
            btnEditarCompra = new Button { Text = "Editar", Width = 76, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, Enabled = false, Cursor = Cursors.Hand };
            btnEditarCompra.Click += (s, e) => AbrirEdicionCompraSeleccionada();
            btnEliminarCompra = new Button { Text = "Eliminar", Width = 76, Height = 29, Margin = new Padding(8, 0, 0, 0), FlatStyle = FlatStyle.Flat, ForeColor = Color.Firebrick, Enabled = false, Cursor = Cursors.Hand };
            btnEliminarCompra.Click += (s, e) => EliminarCompraSeleccionada();
            acciones.Controls.Add(btnNuevoCompra); acciones.Controls.Add(btnEditarCompra); acciones.Controls.Add(btnEliminarCompra); acciones.Controls.Add(filtroCompras);
            encabezado.Controls.Add(titulo); encabezado.Controls.Add(descripcion); encabezado.Controls.Add(acciones);
            gridHistorialCompras = CrearGrid();
            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.White };
            lblTotalCompras = new Label { Dock = DockStyle.Left, Width = 260, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            Panel paginacion = new Panel { Dock = DockStyle.Right, Width = 300, BackColor = Color.White };
            btnCompraAnterior = new Button { Text = "‹ Anterior", Left = 0, Top = 4, Width = 94, Height = 32, Enabled = false };
            lblPaginaCompras = new Label { Text = "Página 1 de 1", Left = 96, Top = 6, Width = 102, Height = 18, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            btnCompraSiguiente = new Button { Text = "Siguiente ›", Left = 200, Top = 4, Width = 96, Height = 32, Enabled = false };
            btnCompraAnterior.Click += (s, e) => CambiarPaginaCompra(-1);
            btnCompraSiguiente.Click += (s, e) => CambiarPaginaCompra(1);
            paginacion.Controls.Add(btnCompraAnterior); paginacion.Controls.Add(lblPaginaCompras); paginacion.Controls.Add(btnCompraSiguiente);
            pie.Controls.Add(lblTotalCompras); pie.Controls.Add(paginacion);
            TableLayoutPanel consulta = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Color.White };
            consulta.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
            consulta.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
            Panel panelDetalle = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0, 5, 0, 0) };
            Label tituloDetalle = new Label { Text = "Detalle de la compra seleccionada", Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            gridDetalleCompra = CrearGrid();
            gridDetalleCompra.ReadOnly = true;
            gridDetalleCompra.AllowUserToAddRows = false;
            gridDetalleCompra.AllowUserToDeleteRows = false;
            panelDetalle.Controls.Add(gridDetalleCompra);
            panelDetalle.Controls.Add(tituloDetalle);
            consulta.Controls.Add(gridHistorialCompras, 0, 0);
            consulta.Controls.Add(panelDetalle, 0, 1);
            gridHistorialCompras.SelectionChanged += (s, e) => { CargarDetalleCompraSeleccionada(); ActualizarBotonEditarCompra(); };
            panel.Controls.Add(consulta); panel.Controls.Add(pie); panel.Controls.Add(encabezado);
            BarraListado.Configurar(gridHistorialCompras, filtroCompras, "Compras");
            Redondear(panel, 12);
            return panel;
        }

        private void FiltrarComprasDePantalla()
        {
            if (comprasTodas == null || comprasTodas.Columns.Count == 0 || gridHistorialCompras == null) return;
            string texto = filtroCompras.Text == "Buscar compras" ? "" : (filtroCompras.Text ?? "").Trim();
            texto = texto.Replace("'", "''").Replace("%", "[%]").Replace("*", "[*]");
            DataView vista = new DataView(comprasTodas);
            vista.RowFilter = String.IsNullOrWhiteSpace(texto) ? "" : "Convert([Codigo], 'System.String') LIKE '%" + texto + "%' OR Convert([Proveedor], 'System.String') LIKE '%" + texto + "%' OR Convert([Sucursal], 'System.String') LIKE '%" + texto + "%'";
            comprasFiltradas = vista.ToTable();
            paginaCompra = 1;
            MostrarPaginaCompras();
        }

        // CAMBIAR PÁGINA: CambiarPaginaCompra
        private void CambiarPaginaCompra(int cambio)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(comprasFiltradas.Rows.Count / (double)RegistrosPorPaginaCompras));
            int nueva = paginaCompra + cambio;
            if (nueva < 1 || nueva > paginas) return;
            paginaCompra = nueva;
            MostrarPaginaCompras();
        }

        // PRESENTAR DATOS: MostrarPaginaCompras
        private void MostrarPaginaCompras()
        {
            if (gridHistorialCompras == null || comprasFiltradas == null) return;
            int paginas = Math.Max(1, (int)Math.Ceiling(comprasFiltradas.Rows.Count / (double)RegistrosPorPaginaCompras));
            paginaCompra = Math.Min(paginaCompra, paginas);
            int inicio = (paginaCompra - 1) * RegistrosPorPaginaCompras;
            int fin = Math.Min(inicio + RegistrosPorPaginaCompras, comprasFiltradas.Rows.Count);
            DataTable pagina = comprasFiltradas.Clone();
            for (int i = inicio; i < fin; i++) pagina.ImportRow(comprasFiltradas.Rows[i]);
            gridHistorialCompras.DataSource = pagina;
            Estilizar(gridHistorialCompras, "Lineas");
            ConfigurarGridCompras();
            lblTotalCompras.Text = "Mostrando " + Math.Max(0, fin - inicio) + " de " + comprasFiltradas.Rows.Count + " compras";
            lblPaginaCompras.Text = "Página " + paginaCompra + " de " + paginas;
            btnCompraAnterior.Enabled = paginaCompra > 1;
            btnCompraSiguiente.Enabled = paginaCompra < paginas;
        }

        // CONFIGURACIÓN: ConfigurarGridCompras
        private void ConfigurarGridCompras()
        {
            gridHistorialCompras.BackgroundColor = Color.White;
            gridHistorialCompras.BorderStyle = BorderStyle.None;
            gridHistorialCompras.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridHistorialCompras.GridColor = Color.FromArgb(234, 240, 240);
            gridHistorialCompras.EnableHeadersVisualStyles = false;
            gridHistorialCompras.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridHistorialCompras.ColumnHeadersHeight = 32;
            gridHistorialCompras.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            gridHistorialCompras.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridHistorialCompras.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            gridHistorialCompras.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            gridHistorialCompras.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
            gridHistorialCompras.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            gridHistorialCompras.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            gridHistorialCompras.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            gridHistorialCompras.DefaultCellStyle.Padding = new Padding(5, 4, 5, 4);
            gridHistorialCompras.RowTemplate.Height = 36;
            if (gridHistorialCompras.Columns.Contains("Codigo")) gridHistorialCompras.Columns["Codigo"].HeaderText = "COMPRA";
            if (gridHistorialCompras.Columns.Contains("Proveedor")) gridHistorialCompras.Columns["Proveedor"].HeaderText = "PROVEEDOR";
            if (gridHistorialCompras.Columns.Contains("Sucursal")) gridHistorialCompras.Columns["Sucursal"].HeaderText = "SUCURSAL";
            if (gridHistorialCompras.Columns.Contains("IdProveedor")) gridHistorialCompras.Columns["IdProveedor"].Visible = false;
            if (gridHistorialCompras.Columns.Contains("IdSucursal")) gridHistorialCompras.Columns["IdSucursal"].Visible = false;
            if (gridHistorialCompras.Columns.Contains("Fecha")) { gridHistorialCompras.Columns["Fecha"].HeaderText = "FECHA"; gridHistorialCompras.Columns["Fecha"].DefaultCellStyle.Format = "dd MMM yyyy"; }
            if (gridHistorialCompras.Columns.Contains("Total")) { gridHistorialCompras.Columns["Total"].HeaderText = "TOTAL"; gridHistorialCompras.Columns["Total"].DefaultCellStyle.Format = "'Q' #,##0.00"; }
            if (gridHistorialCompras.Columns.Contains("Codigo")) gridHistorialCompras.Columns["Codigo"].HeaderText = "COMPRA";
            gridHistorialCompras.ClearSelection();
            ActualizarBotonEditarCompra();
        }

        // CARGA DE DATOS: CargarDetalleCompraSeleccionada
        private void CargarDetalleCompraSeleccionada()
        {
            if (gridDetalleCompra == null || gridHistorialCompras == null || gridHistorialCompras.SelectedRows.Count == 0)
            {
                if (gridDetalleCompra != null) gridDetalleCompra.DataSource = null;
                return;
            }

            try
            {
                int idCompra = Convert.ToInt32(gridHistorialCompras.SelectedRows[0].Cells["Codigo"].Value);
                gridDetalleCompra.DataSource = negocio.DetalleCompra(idCompra);
                Estilizar(gridDetalleCompra);
                if (gridDetalleCompra.Columns.Contains("IdProducto")) gridDetalleCompra.Columns["IdProducto"].Visible = false;
                if (gridDetalleCompra.Columns.Contains("CodigoProducto")) gridDetalleCompra.Columns["CodigoProducto"].HeaderText = "CÓDIGO PRODUCTO";
                if (gridDetalleCompra.Columns.Contains("Producto")) gridDetalleCompra.Columns["Producto"].HeaderText = "PRODUCTO";
                if (gridDetalleCompra.Columns.Contains("Cantidad")) gridDetalleCompra.Columns["Cantidad"].HeaderText = "CANTIDAD";
                if (gridDetalleCompra.Columns.Contains("PrecioUnitario"))
                {
                    gridDetalleCompra.Columns["PrecioUnitario"].HeaderText = "PRECIO UNITARIO";
                    gridDetalleCompra.Columns["PrecioUnitario"].DefaultCellStyle.Format = "'Q' #,##0.00";
                }
                if (gridDetalleCompra.Columns.Contains("Subtotal"))
                {
                    gridDetalleCompra.Columns["Subtotal"].HeaderText = "SUBTOTAL";
                    gridDetalleCompra.Columns["Subtotal"].DefaultCellStyle.Format = "'Q' #,##0.00";
                }
            }
            catch (Exception ex)
            {
                gridDetalleCompra.DataSource = null;
                MostrarError(ex);
            }
        }

        // EDITAR: ActualizarBotonEditarCompra
        private void ActualizarBotonEditarCompra()
        {
            if (btnEditarCompra == null) return;
            bool puedeGestionar = gridHistorialCompras != null && gridHistorialCompras.SelectedRows.Count > 0;
            btnEditarCompra.Enabled = puedeGestionar;
            if (btnEliminarCompra != null) btnEliminarCompra.Enabled = puedeGestionar;
        }

        // ELIMINAR: EliminarCompraSeleccionada
        private void EliminarCompraSeleccionada()
        {
            if (gridHistorialCompras == null || gridHistorialCompras.SelectedRows.Count == 0) return;
            DataGridViewRow fila = gridHistorialCompras.SelectedRows[0];
            int id = Convert.ToInt32(fila.Cells["Codigo"].Value);
            string proveedor = Convert.ToString(fila.Cells["Proveedor"].Value);
            DialogResult confirmar = AyudaForms.MtdConfirmacion(
                "¿Eliminar la compra No. " + id + " de " + proveedor + "? También se revertirá su existencia en la sucursal y se registrará en Kardex. Si el stock ya se consumió, la operación se cancelará.");
            if (confirmar != DialogResult.Yes) return;

            if (Ejecutar(() =>
            {
                if (fila.Cells["IdSucursal"].Value == DBNull.Value)
                {
                    int? sucursal = SolicitarSucursalCompraHistorica();
                    if (!sucursal.HasValue) return;
                    negocio.AsignarSucursalCompra(id, sucursal.Value);
                }
                negocio.EliminarCompra(id);
                RefrescarTodo();
                AyudaForms.MtdInfo("Compra No. " + id + " eliminada. Inventario actualizado.");
            }))
                CargarDetalleCompraSeleccionada();
        }

        private void AbrirEdicionCompraSeleccionada()
        {
            if (gridHistorialCompras == null || gridHistorialCompras.SelectedRows.Count == 0) return;
            DataGridViewRow fila = gridHistorialCompras.SelectedRows[0];
            int id = Convert.ToInt32(fila.Cells["Codigo"].Value);
            Ejecutar(() =>
            {
                if (fila.Cells["IdSucursal"].Value == DBNull.Value)
                {
                    int? sucursal = SolicitarSucursalCompraHistorica();
                    if (!sucursal.HasValue) return;
                    negocio.AsignarSucursalCompra(id, sucursal.Value);
                    RefrescarTodo();
                }

                DataRow[] comprasEncontradas = comprasTodas.Select("Codigo = " + id);
                if (comprasEncontradas.Length == 0) throw new InvalidOperationException("No se encontró la compra seleccionada. Actualiza la lista e intenta de nuevo.");
                DataRow compra = comprasEncontradas[0];
                compraIdEdicion = id;
                DataTable detalle = negocio.DetalleCompra(compraIdEdicion.Value);
                if (detalle.Rows.Count == 0) throw new InvalidOperationException("La compra no contiene productos para editar.");
                compraProveedor.SelectedValue = Convert.ToInt32(compra["IdProveedor"]);
                compraSucursal.SelectedValue = Convert.ToInt32(compra["IdSucursal"]);
                compraFecha.Value = Convert.ToDateTime(compra["Fecha"]);
                lineas.Clear();
                foreach (DataRow r in detalle.Rows)
                {
                    lineas.Add(new CompraLineaCatalogo
                    {
                        IdProducto = Convert.ToInt32(r["IdProducto"]),
                        Producto = Convert.ToString(r["Producto"]),
                        Cantidad = Convert.ToDecimal(r["Cantidad"]),
                        CostoUnitario = Convert.ToDecimal(r["PrecioUnitario"])
                    });
                }
                MostrarLineas();
                btnGuardarCompra.Text = "Actualizar compra e inventario";
                dialogoCompra.Text = "Editar compra No. " + compraIdEdicion.Value;
                EstiloAplicacion.AplicarDialogo(dialogoCompra);
                dialogoCompra.ShowDialog(this);
            });
        }

        private int? SolicitarSucursalCompraHistorica()
        {
            DataTable sucursales = negocio.SucursalesActivas();
            if (sucursales.Rows.Count == 0) throw new InvalidOperationException("No hay sucursales activas para asignar a la compra.");

            using (Form dialogo = new Form
            {
                Text = "Asignar sucursal a compra histórica",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(430, 165),
                Font = new Font("Segoe UI", 9F)
            })
            {
                Label indicacion = new Label
                {
                    Text = "Selecciona la sucursal donde realmente se recibieron estos productos. La asignación no cambia el stock.",
                    Left = 16, Top = 14, Width = 398, Height = 42
                };
                ComboBox sucursal = new ComboBox
                {
                    Left = 16, Top = 62, Width = 398, DropDownStyle = ComboBoxStyle.DropDownList,
                    DataSource = sucursales, DisplayMember = "Nombre", ValueMember = "Codigo"
                };
                sucursal.SelectedIndex = -1;
                Button aceptar = new Button { Text = "Asignar", Left = 236, Top = 112, Width = 86, Height = 32 };
                Button cancelar = new Button { Text = "Cancelar", Left = 328, Top = 112, Width = 86, Height = 32, DialogResult = DialogResult.Cancel };
                aceptar.Click += (s, e) =>
                {
                    if (sucursal.SelectedItem == null)
                    {
                        MessageBox.Show("Selecciona la sucursal correcta para continuar.", "Sucursal requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };
                dialogo.Controls.Add(indicacion);
                dialogo.Controls.Add(sucursal);
                dialogo.Controls.Add(aceptar);
                dialogo.Controls.Add(cancelar);
                dialogo.AcceptButton = aceptar;
                dialogo.CancelButton = cancelar;

                if (dialogo.ShowDialog(this) != DialogResult.OK) return null;
                return Convert.ToInt32(sucursal.SelectedValue);
            }
        }
        // CREAR ELEMENTO VISUAL: CrearSeccionCompra
        private static Panel CrearSeccionCompra(string titulo, out DataGridView grid)
        {
            Panel panel = new Panel { Dock=DockStyle.Fill, Padding=new Padding(0, 2, 0, 2) };
            Label etiqueta = new Label { Text=titulo, Dock=DockStyle.Top, Height=25, Font=new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor=EstiloAplicacion.Tinta };
            grid = CrearGrid();
            panel.Controls.Add(grid);
            panel.Controls.Add(etiqueta);
            return panel;
        }
        private static ComboBox Combo(Panel p,string label,int x,int y,int w)
        { p.Controls.Add(new Label{Text=label,Left=x,Top=y,Width=w});ComboBox c=new ComboBox{Left=x,Top=y+23,Width=w,DropDownStyle=ComboBoxStyle.DropDownList,DisplayMember="Nombre",ValueMember="Codigo"};p.Controls.Add(c);return c; }

        private void RefrescarTodo()
        {
            if (gridCategorias != null)
            {
                DataTable categorias=negocio.Categorias();
                DataTable productos=negocio.Productos();
                tablaCategorias=categorias;
                categoriasTodas = CrearVistaCategorias(categorias, productos);
                categoriasFiltradas = categoriasTodas.Copy();
                paginaCategoria = 1;
                FiltrarCategoriasDePantalla();
                int sinProductos = 0;
                foreach (DataRow fila in categoriasFiltradas.Rows) if (Convert.ToInt32(fila["Productos"]) == 0) sinProductos++;
                ActualizarEstadisticas(new[] { "CATEGORÍAS", "PRODUCTOS", "SIN PRODUCTOS" }, new[] { categorias.Rows.Count.ToString(), productos.Rows.Count.ToString(), sinProductos.ToString() });
            }
            if (prodCategoria != null)
            {
                DataTable cats=negocio.Categorias();prodCategoria.DataSource=cats;prodCategoria.SelectedIndex=cats.Rows.Count>0?0:-1;
                DataTable productos=negocio.Productos();
                tablaProductos=productos;
                productosFiltrados = productos.Copy();
                paginaProducto = 1;
                MostrarPaginaProductos();
                int stockBajo=0;
                foreach (DataRow fila in productos.Rows) if (productos.Columns.Contains("StockBajo") && Convert.ToBoolean(fila["StockBajo"])) stockBajo++;
                ActualizarEstadisticas(new[] { "PRODUCTOS", "STOCK BAJO", "CATEGORÍAS" }, new[] { productos.Rows.Count.ToString(), stockBajo.ToString(), cats.Rows.Count.ToString() });
            }
            if (gridSucursales != null)
            {
                DataTable sucursales=negocio.Sucursales();
                MostrarSucursales(sucursales);
                int activas=0;
                int personal=0;
                foreach (DataRow fila in sucursales.Rows)
                {
                    if (Convert.ToBoolean(fila["Activo"])) activas++;
                    if (sucursales.Columns.Contains("Personal")) personal += Convert.ToInt32(fila["Personal"]);
                }
                ActualizarEstadisticas(new[] { "SUCURSALES", "ACTIVAS", "PERSONAL ACTIVO" }, new[] { sucursales.Rows.Count.ToString(), activas.ToString(), personal.ToString() });
            }
            if (compraProveedor != null)
            {
                compraProveedor.DataSource=negocio.Proveedores();compraSucursal.DataSource=negocio.SucursalesActivas();compraProducto.DataSource=negocio.ProductosActivos();
                DataTable compras=negocio.Compras();
                comprasTodas=compras;
                FiltrarComprasDePantalla();
                int mesActual=0;
                decimal importeMes=0;
                foreach (DataRow fila in compras.Rows)
                    if (Convert.ToDateTime(fila["Fecha"]).Month == DateTime.Today.Month && Convert.ToDateTime(fila["Fecha"]).Year == DateTime.Today.Year)
                    {
                        mesActual++;
                        importeMes+=Convert.ToDecimal(fila["Total"]);
                    }
                ActualizarEstadisticas(new[] { "COMPRAS DEL MES", "REGISTROS", "IMPORTE DEL MES" }, new[] { mesActual.ToString(), compras.Rows.Count.ToString(), importeMes.ToString("'Q' #,##0.00") });
            }
        }
        private static DataTable TablaCompra(){DataTable t=new DataTable();t.Columns.Add("IdProducto",typeof(int));t.Columns.Add("Producto");t.Columns.Add("Cantidad",typeof(decimal));t.Columns.Add("Costo unitario",typeof(decimal));t.Columns.Add("Subtotal",typeof(decimal));return t;}
        // GUARDAR: GuardarCategoria
        private bool GuardarCategoria(){return Ejecutar(()=>{negocio.GuardarCategoria(catId,catNombre.Text,catDescripcion.Text);LimpiarCategoria();RefrescarTodo();AyudaForms.MtdInfo("Categoría guardada correctamente.");});}
        // LIMPIAR: LimpiarCategoria
        private void LimpiarCategoria(){catId=null;catNombre.Clear();catDescripcion.Clear();}
        private int IdSeleccionado(DataGridView g,string col){if(g.CurrentRow==null)throw new InvalidOperationException("Selecciona un registro de la lista.");return Convert.ToInt32(g.CurrentRow.Cells[col].Value);}
        // ELIMINAR: EliminarCategoria
        private bool EliminarCategoria(){return Ejecutar(()=>{negocio.EliminarCategoria(IdSeleccionado(gridCategorias,"Codigo"));RefrescarTodo();AyudaForms.MtdInfo("Categoría eliminada correctamente.");});}
        // GUARDAR: GuardarProducto
        private bool GuardarProducto(){return Ejecutar(()=>{if(prodCategoria.SelectedValue==null)throw new InvalidOperationException("Primero registra una categoría.");negocio.GuardarProducto(prodId,prodCodigo.Text,prodNombre.Text,prodDescripcion.Text,prodCosto.Value,prodPrecio.Value,Convert.ToInt32(prodCategoria.SelectedValue),prodActivo.Checked);LimpiarProducto();RefrescarTodo();AyudaForms.MtdInfo("Producto guardado correctamente.");});}
        // LIMPIAR: LimpiarProducto
        private void LimpiarProducto(){prodId=null;prodCodigo.Clear();prodNombre.Clear();prodDescripcion.Clear();prodCosto.Value=0;prodPrecio.Value=0;prodActivo.Checked=true;if(prodCategoria.Items.Count>0)prodCategoria.SelectedIndex=0;}
        // ELIMINAR: EliminarProducto
        private bool EliminarProducto(){return Ejecutar(()=>{negocio.EliminarProducto(IdSeleccionado(gridProductos,"Codigo"));RefrescarTodo();AyudaForms.MtdInfo("Producto eliminado correctamente.");});}
        // GUARDAR: GuardarSucursal
        private bool GuardarSucursal(){return Ejecutar(()=>{Sucursal sucursal=new Sucursal{IdSucursal=sucId.GetValueOrDefault(),Nombre=sucNombre.Text,Direccion=sucDireccion.Text,Telefono=sucTelefono.Text,Estado=sucActivo.Checked};if(sucId.HasValue){negocio.EditarSucursal(sucursal);}else{negocio.AgregarSucursal(sucursal);}LimpiarSucursal();RefrescarTodo();AyudaForms.MtdInfo("Sucursal guardada correctamente.");});}
        // CONSULTAR: ConsultarSucursales
        private void ConsultarSucursales(){Ejecutar(()=>MostrarSucursales(negocio.Sucursales()));}
        // BUSCAR: BuscarSucursales
        private void BuscarSucursales(){Ejecutar(()=>MostrarSucursales(negocio.BuscarSucursales(sucNombre.Text)));}
        // PRESENTAR DATOS: MostrarSucursales
        private void MostrarSucursales(DataTable sucursales)
        {
            sucursalesOriginales = sucursales == null ? new DataTable() : sucursales.Copy();
            _sucursales = sucursalesOriginales.Copy();
            _paginaSucursal = 1;
            _vistaSucursalesLimpiada = false;
            if (filtroPantalla != null && !String.IsNullOrWhiteSpace(filtroPantalla.Text) && filtroPantalla.Text != "Buscar sucursales") FiltrarSucursalesDePantalla();
            else MostrarPaginaSucursal();
        }
        // CAMBIAR PÁGINA: CambiarPaginaSucursal
        private void CambiarPaginaSucursal(int cambio)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(_sucursales.Rows.Count / (double)RegistrosPorPaginaSucursales));
            int paginaNueva = _paginaSucursal + cambio;
            if (_vistaSucursalesLimpiada || paginaNueva < 1 || paginaNueva > paginas) return;
            _paginaSucursal = paginaNueva;
            MostrarPaginaSucursal();
        }
        // PRESENTAR DATOS: MostrarPaginaSucursal
        private void MostrarPaginaSucursal()
        {
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(_sucursales.Rows.Count / (double)RegistrosPorPaginaSucursales));
            if (_paginaSucursal > totalPaginas) _paginaSucursal = totalPaginas;

            DataTable pagina = _sucursales.Clone();
            int inicio = (_paginaSucursal - 1) * RegistrosPorPaginaSucursales;
            int fin = Math.Min(inicio + RegistrosPorPaginaSucursales, _sucursales.Rows.Count);
            for (int indice = inicio; indice < fin; indice++)
                pagina.ImportRow(_sucursales.Rows[indice]);

            gridSucursales.DataSource = pagina;
            Estilizar(gridSucursales, "Codigo");
            ConfigurarGridSucursales();
            lblTotalRegistros.Text = "Mostrando " + Math.Max(0, fin - inicio) + " de " + _sucursales.Rows.Count + " registros";
            lblPaginaSucursales.Text = "P\u00E1gina " + _paginaSucursal + " de " + totalPaginas +
                " \u00B7 " + _sucursales.Rows.Count + " sucursales";
            btnSucursalAnterior.Enabled = _paginaSucursal > 1;
            btnSucursalSiguiente.Enabled = _paginaSucursal < totalPaginas;
        }

        // CONFIGURACIÓN: ConfigurarGridSucursales
        private void ConfigurarGridSucursales()
        {
            // Activo llega como Boolean desde SQL. La columna CheckBox no admite
            // que CellFormatting lo reemplace por las etiquetas "Activa/Inactiva".
            // Se usa una columna de texto enlazada al mismo campo para conservar
            // el valor Boolean del DataTable y mostrar etiquetas legibles.
            if (gridSucursales.Columns.Contains("Activo") && !(gridSucursales.Columns["Activo"] is DataGridViewTextBoxColumn))
            {
                int indiceActivo = gridSucursales.Columns["Activo"].Index;
                gridSucursales.Columns.Remove("Activo");
                gridSucursales.Columns.Insert(indiceActivo, new DataGridViewTextBoxColumn
                {
                    Name = "Activo",
                    DataPropertyName = "Activo",
                    ValueType = typeof(bool),
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.Automatic
                });
            }

            gridSucursales.BackgroundColor = Color.White;
            gridSucursales.BorderStyle = BorderStyle.None;
            gridSucursales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridSucursales.GridColor = Color.FromArgb(234, 240, 240);
            gridSucursales.EnableHeadersVisualStyles = false;
            gridSucursales.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridSucursales.ColumnHeadersHeight = 30;
            gridSucursales.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            gridSucursales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridSucursales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            gridSucursales.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            gridSucursales.DefaultCellStyle.Font = new Font("Segoe UI", 8F);
            gridSucursales.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            gridSucursales.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            gridSucursales.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            gridSucursales.DefaultCellStyle.Padding = new Padding(4, 7, 4, 7);
            gridSucursales.RowTemplate.Height = 44;
            gridSucursales.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            if (gridSucursales.Columns.Contains("Nombre")) { gridSucursales.Columns["Nombre"].HeaderText = "SUCURSAL"; gridSucursales.Columns["Nombre"].FillWeight = 22; }
            if (gridSucursales.Columns.Contains("Direccion")) { gridSucursales.Columns["Direccion"].HeaderText = "DIRECCI\u00D3N"; gridSucursales.Columns["Direccion"].FillWeight = 27; }
            if (gridSucursales.Columns.Contains("Telefono")) { gridSucursales.Columns["Telefono"].HeaderText = "TEL\u00C9FONO"; gridSucursales.Columns["Telefono"].FillWeight = 18; }
            if (gridSucursales.Columns.Contains("Personal")) { gridSucursales.Columns["Personal"].HeaderText = "EMPLEADOS"; gridSucursales.Columns["Personal"].FillWeight = 15; gridSucursales.Columns["Personal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; gridSucursales.Columns["Personal"].DisplayIndex = 3; }
            if (gridSucursales.Columns.Contains("Activo")) { gridSucursales.Columns["Activo"].HeaderText = "ESTADO"; gridSucursales.Columns["Activo"].FillWeight = 18; gridSucursales.Columns["Activo"].DisplayIndex = 4; }
        }

        // LIMPIAR: LimpiarTablaSucursales
        private void LimpiarTablaSucursales()
        {
            // No consultar ni vaciar la tabla: solo restablecer los campos de sucursal.
            LimpiarSucursal();
        }
        // LIMPIAR: LimpiarSucursal
        private void LimpiarSucursal(){sucId=null;sucNombre.Clear();sucDireccion.Clear();sucTelefono.Clear();sucActivo.Checked=true;}
        // ELIMINAR: EliminarSucursal
        private bool EliminarSucursal()
        {
            return Ejecutar(() =>
            {
                int id = IdSeleccionado(gridSucursales, "Codigo");
                DialogResult confirmar = AyudaForms.MtdConfirmacion(
                    "¿Deseas eliminar la sucursal seleccionada? Si tiene empleados, inventario, stock o traslados relacionados, SQL Server no permitirá borrarla.");
                if (confirmar != DialogResult.Yes) return;

                negocio.EliminarSucursal(id);
                LimpiarSucursal();
                RefrescarTodo();
                AyudaForms.MtdInfo("Sucursal eliminada correctamente.");
            });
        }
        // AGREGAR: AgregarLinea
        private void AgregarLinea(){Ejecutar(()=>{if(compraProducto.SelectedItem==null)throw new InvalidOperationException("No hay productos activos.");DataRowView r=compraProducto.SelectedItem as DataRowView;int id=Convert.ToInt32(r["Codigo"]);string nombre=Convert.ToString(r["Nombre"]);CompraLineaCatalogo l=new CompraLineaCatalogo{IdProducto=id,Producto=nombre,Cantidad=compraCantidad.Value,CostoUnitario=compraCosto.Value};lineas.Add(l);MostrarLineas();});}
        // PRESENTAR DATOS: MostrarLineas
        private void MostrarLineas(){DataTable t=TablaCompra();foreach(CompraLineaCatalogo l in lineas)t.Rows.Add(l.IdProducto,l.Producto,l.Cantidad,l.CostoUnitario,l.Subtotal);gridCompra.DataSource=t;Estilizar(gridCompra,"IdProducto");decimal total=0;foreach(CompraLineaCatalogo l in lineas)total+=l.Subtotal;compraTotal.Text="Total: Q "+total.ToString("N2");}
        // ELIMINAR: QuitarLinea
        private void QuitarLinea(){if(gridCompra.CurrentRow==null)return;int ix=gridCompra.CurrentRow.Index;if(ix>=0&&ix<lineas.Count){lineas.RemoveAt(ix);MostrarLineas();}}
        // GUARDAR: GuardarCompra
        private void GuardarCompra()
        {
            Ejecutar(() =>
            {
                if (compraProveedor.SelectedItem == null || compraSucursal.SelectedItem == null)
                    throw new InvalidOperationException("Registra proveedores y sucursales activas primero.");
                int proveedor = Convert.ToInt32(((DataRowView)compraProveedor.SelectedItem)["Codigo"]);
                int sucursal = Convert.ToInt32(((DataRowView)compraSucursal.SelectedItem)["Codigo"]);
                int? idEditado = compraIdEdicion;
                int id;
                if (idEditado.HasValue)
                {
                    negocio.EditarCompra(idEditado.Value, proveedor, sucursal, compraFecha.Value, lineas);
                    id = idEditado.Value;
                }
                else id = negocio.GuardarCompra(proveedor, sucursal, compraFecha.Value, lineas);
                lineas.Clear();
                compraIdEdicion = null;
                btnGuardarCompra.Text = "Guardar compra y actualizar inventario";
                MostrarLineas();
                RefrescarTodo();
                AyudaForms.MtdInfo("Compra No. " + id + (idEditado.HasValue ? " actualizada." : " guardada.") + " Inventario y Kardex actualizados.");
                dialogoCompra.Close();
            });
        }
        private bool Ejecutar(Action accion){try{accion();return true;}catch(Exception ex){MostrarError(ex);return false;}}
        // PRESENTAR DATOS: MostrarError
        private void MostrarError(Exception ex){MessageBox.Show(ex.Message,"No se pudo completar la operación",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
    }

}

