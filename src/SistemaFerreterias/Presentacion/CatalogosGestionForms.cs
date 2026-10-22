using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Presentacion
{
    public class CatalogosGestionForms : Form
    {
        private readonly CatalogosGestionNegocio negocio = new CatalogosGestionNegocio();
        private readonly TabControl moduloHost = new TabControl();
        private DataGridView gridCategorias, gridProductos, gridSucursales, gridCompra, gridHistorialCompras;
        private const int RegistrosPorPaginaSucursales = 10;
        private DataTable _sucursales = new DataTable();
        private int _paginaSucursal = 1;
        private Button btnSucursalAnterior, btnSucursalSiguiente;
        private Label lblPaginaSucursales;
        private bool _vistaSucursalesLimpiada;
        private TextBox catNombre, catDescripcion;

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
        private readonly List<CompraLineaCatalogo> lineas = new List<CompraLineaCatalogo>();

        public CatalogosGestionForms()
        {
            Text = "Catálogos y compras"; Width = 1180; Height = 760; StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 9F);
            moduloHost.Dock = DockStyle.Fill;
            moduloHost.Appearance = TabAppearance.FlatButtons;
            moduloHost.SizeMode = TabSizeMode.Fixed;
            moduloHost.ItemSize = new Size(0, 1);
            BackColor = Color.FromArgb(245, 245, 247);
            Controls.Add(moduloHost);
            AgregarEncabezado();
            ConstruirCategorias(); ConstruirProductos(); ConstruirSucursales(); ConstruirCompras();
            EstiloAplicacion.Aplicar(this);
            Shown += (s, e) => { try { RefrescarTodo(); } catch (Exception ex) { MostrarError(ex); } };
        }

        private void AgregarEncabezado()
        {
            Panel encabezado = new Panel
            {
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = Color.White
            };
            PictureBox imagen = new PictureBox
            {
                Image = EstiloAplicacion.ObtenerImagenClientes(),
                Location = new Point(24, 10),
                Size = new Size(128, 74),
                SizeMode = PictureBoxSizeMode.Zoom,
                TabStop = false
            };
            Label titulo = new Label
            {
                AutoSize = true,
                Text = "Ferretería multisucursal",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = EstiloAplicacion.Tinta,
                Location = new Point(170, 14)
            };
            Label subtitulo = new Label
            {
                AutoSize = true,
                Text = "Administración de catálogos, sucursales y compras",
                Font = new Font("Segoe UI", 10F),
                ForeColor = EstiloAplicacion.Secundario,
                Location = new Point(174, 60)
            };
            encabezado.Controls.Add(imagen);
            encabezado.Controls.Add(titulo);
            encabezado.Controls.Add(subtitulo);
            Controls.Add(encabezado);
        }
        public void AbrirPestana(string nombre)
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

        private void AgregarPagina(TabPage page)
        {
            moduloHost.TabPages.Add(page);
        }
        private static DataGridView CrearGrid()
        {
            return new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White,
                RowHeadersVisible = false, AutoGenerateColumns = true };
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
        }
        private Panel Lateral(TabPage page, out DataGridView grid, int alturaFormulario)
        {
            grid = CrearGrid(); page.Controls.Add(grid);
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = alturaFormulario,
                AutoScroll = true,
                BackColor = Color.FromArgb(246, 248, 252),
                Padding = new Padding(5)
            };
            page.Controls.Add(panel); return panel;
        }
        private static Button Boton(Panel panel, string caption, int x, int y, int width, EventHandler click)
        {
            Button b = new Button { Text = caption, Left = x, Top = y, Width = width, Height = 34 };
            b.Click += click; panel.Controls.Add(b); return b;
        }

        private void ConstruirCategorias()
        {
            TabPage page = new TabPage("Categorías"); AgregarPagina(page); Panel p = Lateral(page, out gridCategorias, 240);
            catNombre = Campo(p, "Nombre *", 14, out _); catNombre.MaxLength = 60;
            catDescripcion = Campo(p, "Descripción", 70, out _, true); catDescripcion.MaxLength = 150;
            Boton(p, "Guardar", 15, 145, 130, (s,e) => GuardarCategoria());
            Boton(p, "Limpiar", 160, 145, 130, (s,e) => LimpiarCategoria());
            Boton(p, "Eliminar", 15, 187, 130, (s,e) => EliminarCategoria());
            gridCategorias.CellDoubleClick += (s,e) =>
            {
                if (e.RowIndex < 0) return;
                DataRowView r = gridCategorias.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (r == null) return;
                catId = Convert.ToInt32(r["Codigo"]);
                catNombre.Text = Convert.ToString(r["Nombre"]);
                catDescripcion.Text = Convert.ToString(r["Descripcion"]);
            };
        }
        private void ConstruirProductos()
        {
            TabPage page = new TabPage("Productos"); AgregarPagina(page); Panel p = Lateral(page, out gridProductos, 270);
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
            Boton(p,"Guardar",490,178,130,(s,e)=>GuardarProducto());
            Boton(p,"Limpiar",635,178,130,(s,e)=>LimpiarProducto());
            Boton(p,"Eliminar",780,178,130,(s,e)=>EliminarProducto());
            gridProductos.CellDoubleClick += (s,e) =>
            {
                if (e.RowIndex < 0) return;
                DataRowView r = gridProductos.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (r == null) return;
                prodId = Convert.ToInt32(r["Codigo"]); prodCodigo.Text = Convert.ToString(r["SKU"]);
                prodNombre.Text = Convert.ToString(r["Nombre"]); prodDescripcion.Text = Convert.ToString(r["Descripcion"]);
                prodCosto.Value = Math.Max(prodCosto.Minimum, Math.Min(prodCosto.Maximum, Convert.ToDecimal(r["Costo"])));
                prodPrecio.Value = Math.Max(prodPrecio.Minimum, Math.Min(prodPrecio.Maximum, Convert.ToDecimal(r["Precio"])));
                prodCategoria.SelectedValue = Convert.ToInt32(r["IdCategoria"]); prodActivo.Checked = Convert.ToBoolean(r["Activo"]);
            };
        }
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
            sucDireccion=Campo(p,"Dirección *",66,out _); sucDireccion.MaxLength=150;
            sucTelefono=Campo(p,"Teléfono",120,out _); sucTelefono.MaxLength=15;
            sucActivo=new CheckBox { Text="Activa",Left=15,Top=174,Checked=true,AutoSize=true};p.Controls.Add(sucActivo);
            Boton(p,"Guardar",15,207,130,(s,e)=>GuardarSucursal());Boton(p,"Limpiar",160,207,130,(s,e)=>LimpiarTablaSucursales());Boton(p,"Buscar",305,207,130,(s,e)=>BuscarSucursales());Boton(p,"Eliminar",15,249,130,(s,e)=>EliminarSucursal());
            Panel barraPaginas = new Panel { Dock = DockStyle.Bottom, Height = 42, BackColor = Color.White };
            btnSucursalAnterior = new Button { Text = "Anterior", Left = 12, Top = 5, Width = 95, Height = 30, Enabled = false };
            btnSucursalSiguiente = new Button { Text = "Siguiente", Left = 112, Top = 5, Width = 95, Height = 30, Enabled = false };
            lblPaginaSucursales = new Label { Left = 220, Top = 10, Width = 420, Height = 22, Text = "Página 1 de 1 · 0 sucursales en total" };
            btnSucursalAnterior.Click += (s, e) => CambiarPaginaSucursal(-1);
            btnSucursalSiguiente.Click += (s, e) => CambiarPaginaSucursal(1);
            barraPaginas.Controls.Add(btnSucursalAnterior);
            barraPaginas.Controls.Add(btnSucursalSiguiente);
            barraPaginas.Controls.Add(lblPaginaSucursales);
            page.Controls.Add(barraPaginas);
            gridSucursales.CellDoubleClick += (s,e)=>{if(e.RowIndex<0)return;DataRowView r=gridSucursales.Rows[e.RowIndex].DataBoundItem as DataRowView;if(r==null)return;sucId=Convert.ToInt32(r["Codigo"]);sucNombre.Text=Convert.ToString(r["Nombre"]);sucDireccion.Text=Convert.ToString(r["Direccion"]);sucTelefono.Text=Convert.ToString(r["Telefono"]);sucActivo.Checked=Convert.ToBoolean(r["Activo"]);};
        }
        private void ConstruirCompras()
        {
            TabPage page=new TabPage("Compras");AgregarPagina(page);
            TableLayoutPanel layout = new TableLayoutPanel { Dock=DockStyle.Fill, ColumnCount=1, RowCount=4, Padding=new Padding(8) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            Panel top=new Panel { Dock=DockStyle.Fill,BackColor=Color.FromArgb(246,248,252),Padding=new Padding(5)};
            compraProveedor=Combo(top,"Proveedor",15,12,220); compraSucursal=Combo(top,"Sucursal",255,12,220); compraFecha=new DateTimePicker{Left=495,Top=37,Width=150,Format=DateTimePickerFormat.Short}; top.Controls.Add(new Label{Text="Fecha",Left=495,Top=12,Width=140});top.Controls.Add(compraFecha);
            compraProducto=Combo(top,"Producto",15,68,220); compraCantidad=new NumericUpDown{Left=255,Top=93,Width=90,Minimum=1,Maximum=100000,Value=1}; top.Controls.Add(new Label{Text="Cantidad",Left=255,Top=68,Width=90});top.Controls.Add(compraCantidad);
            compraCosto=new NumericUpDown{Left=365,Top=93,Width=110,DecimalPlaces=2,Maximum=100000000,ThousandsSeparator=true};top.Controls.Add(new Label{Text="Costo unitario",Left=365,Top=68,Width=110});top.Controls.Add(compraCosto);
            Boton(top,"Agregar",495,86,110,(s,e)=>AgregarLinea()); compraTotal=new Label{Text="Total: Q 0.00",Left=690,Top=38,Width=220,Font=new Font("Segoe UI",12,FontStyle.Bold),ForeColor=Color.DarkGreen};top.Controls.Add(compraTotal);

            Panel panelLineas = CrearSeccionCompra("Líneas de la compra", out gridCompra);
            gridCompra.DataSource = TablaCompra();
            Estilizar(gridCompra, "IdProducto");
            Panel panelHistorial = CrearSeccionCompra("Compras registradas", out gridHistorialCompras);

            Panel foot=new Panel{Dock=DockStyle.Fill};
            Boton(foot,"Guardar compra y actualizar inventario",15,8,300,(s,e)=>GuardarCompra());
            Boton(foot,"Quitar línea",330,8,140,(s,e)=>QuitarLinea());

            layout.Controls.Add(top, 0, 0);
            layout.Controls.Add(panelLineas, 0, 1);
            layout.Controls.Add(panelHistorial, 0, 2);
            layout.Controls.Add(foot, 0, 3);
            page.Controls.Add(layout);
        }
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
            gridCategorias.DataSource=negocio.Categorias();Estilizar(gridCategorias,"Codigo");
            DataTable cats=negocio.Categorias();prodCategoria.DataSource=cats;prodCategoria.SelectedIndex=cats.Rows.Count>0?0:-1;
            gridProductos.DataSource=negocio.Productos();Estilizar(gridProductos,"Codigo","IdCategoria");
            ConsultarSucursales();
            compraProveedor.DataSource=negocio.Proveedores();compraSucursal.DataSource=negocio.SucursalesActivas();compraProducto.DataSource=negocio.ProductosActivos();
            gridHistorialCompras.DataSource=negocio.Compras();
            if (gridHistorialCompras.Columns.Contains("Codigo")) gridHistorialCompras.Columns["Codigo"].HeaderText="Compra";
            if (gridHistorialCompras.Columns.Contains("Proveedor")) gridHistorialCompras.Columns["Proveedor"].HeaderText="Proveedor";
            if (gridHistorialCompras.Columns.Contains("Fecha")) gridHistorialCompras.Columns["Fecha"].DefaultCellStyle.Format="dd/MM/yyyy";
            if (gridHistorialCompras.Columns.Contains("Total")) gridHistorialCompras.Columns["Total"].DefaultCellStyle.Format="'Q' #,##0.00";
        }
        private static DataTable TablaCompra(){DataTable t=new DataTable();t.Columns.Add("IdProducto",typeof(int));t.Columns.Add("Producto");t.Columns.Add("Cantidad",typeof(decimal));t.Columns.Add("Costo unitario",typeof(decimal));t.Columns.Add("Subtotal",typeof(decimal));return t;}
        private void GuardarCategoria(){Ejecutar(()=>{negocio.GuardarCategoria(catId,catNombre.Text,catDescripcion.Text);LimpiarCategoria();RefrescarTodo();});}
        private void LimpiarCategoria(){catId=null;catNombre.Clear();catDescripcion.Clear();}
        private int IdSeleccionado(DataGridView g,string col){if(g.CurrentRow==null)throw new InvalidOperationException("Selecciona un registro de la lista.");return Convert.ToInt32(g.CurrentRow.Cells[col].Value);}
        private void EliminarCategoria(){Ejecutar(()=>{negocio.EliminarCategoria(IdSeleccionado(gridCategorias,"Codigo"));RefrescarTodo();});}
        private void GuardarProducto(){Ejecutar(()=>{if(prodCategoria.SelectedValue==null)throw new InvalidOperationException("Primero registra una categoría.");negocio.GuardarProducto(prodId,prodCodigo.Text,prodNombre.Text,prodDescripcion.Text,prodCosto.Value,prodPrecio.Value,Convert.ToInt32(prodCategoria.SelectedValue),prodActivo.Checked);LimpiarProducto();RefrescarTodo();});}
        private void LimpiarProducto(){prodId=null;prodCodigo.Clear();prodNombre.Clear();prodDescripcion.Clear();prodCosto.Value=0;prodPrecio.Value=0;prodActivo.Checked=true;if(prodCategoria.Items.Count>0)prodCategoria.SelectedIndex=0;}
        private void EliminarProducto(){Ejecutar(()=>{negocio.EliminarProducto(IdSeleccionado(gridProductos,"Codigo"));RefrescarTodo();});}
        private void GuardarSucursal(){Ejecutar(()=>{Sucursal sucursal=new Sucursal{IdSucursal=sucId.GetValueOrDefault(),Nombre=sucNombre.Text,Direccion=sucDireccion.Text,Telefono=sucTelefono.Text,Estado=sucActivo.Checked};if(sucId.HasValue){negocio.EditarSucursal(sucursal);}else{negocio.AgregarSucursal(sucursal);}LimpiarSucursal();RefrescarTodo();});}
        private void ConsultarSucursales(){Ejecutar(()=>MostrarSucursales(negocio.Sucursales()));}
        private void BuscarSucursales(){Ejecutar(()=>MostrarSucursales(negocio.BuscarSucursales(sucNombre.Text)));}
        private void MostrarSucursales(DataTable sucursales)
        {
            _sucursales = sucursales ?? new DataTable();
            _paginaSucursal = 1;
            _vistaSucursalesLimpiada = false;
            MostrarPaginaSucursal();
        }
        private void CambiarPaginaSucursal(int cambio)
        {
            int paginas = Math.Max(1, (int)Math.Ceiling(_sucursales.Rows.Count / (double)RegistrosPorPaginaSucursales));
            int paginaNueva = _paginaSucursal + cambio;
            if (_vistaSucursalesLimpiada || paginaNueva < 1 || paginaNueva > paginas) return;
            _paginaSucursal = paginaNueva;
            MostrarPaginaSucursal();
        }
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
            lblPaginaSucursales.Text = "Página " + _paginaSucursal + " de " + totalPaginas +
                " · " + _sucursales.Rows.Count + " sucursales en total";
            btnSucursalAnterior.Enabled = _paginaSucursal > 1;
            btnSucursalSiguiente.Enabled = _paginaSucursal < totalPaginas;
        }
        private void LimpiarTablaSucursales()
        {
            // No consultar ni vaciar la tabla: solo restablecer los campos de sucursal.
            LimpiarSucursal();
        }
        private void LimpiarSucursal(){sucId=null;sucNombre.Clear();sucDireccion.Clear();sucTelefono.Clear();sucActivo.Checked=true;}
        private void EliminarSucursal()
        {
            Ejecutar(() =>
            {
                int id = IdSeleccionado(gridSucursales, "Codigo");
                DialogResult confirmar = MessageBox.Show(
                    "¿Deseas eliminar la sucursal seleccionada? Si tiene empleados, inventario, stock o traslados relacionados, SQL Server no permitirá borrarla.",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmar != DialogResult.Yes) return;

                negocio.EliminarSucursal(id);
                LimpiarSucursal();
                RefrescarTodo();
            });
        }
        private void AgregarLinea(){Ejecutar(()=>{if(compraProducto.SelectedItem==null)throw new InvalidOperationException("No hay productos activos.");DataRowView r=compraProducto.SelectedItem as DataRowView;int id=Convert.ToInt32(r["Codigo"]);string nombre=Convert.ToString(r["Nombre"]);CompraLineaCatalogo l=new CompraLineaCatalogo{IdProducto=id,Producto=nombre,Cantidad=compraCantidad.Value,CostoUnitario=compraCosto.Value};lineas.Add(l);MostrarLineas();});}
        private void MostrarLineas(){DataTable t=TablaCompra();foreach(CompraLineaCatalogo l in lineas)t.Rows.Add(l.IdProducto,l.Producto,l.Cantidad,l.CostoUnitario,l.Subtotal);gridCompra.DataSource=t;Estilizar(gridCompra,"IdProducto");decimal total=0;foreach(CompraLineaCatalogo l in lineas)total+=l.Subtotal;compraTotal.Text="Total: Q "+total.ToString("N2");}
        private void QuitarLinea(){if(gridCompra.CurrentRow==null)return;int ix=gridCompra.CurrentRow.Index;if(ix>=0&&ix<lineas.Count){lineas.RemoveAt(ix);MostrarLineas();}}
        private void GuardarCompra(){Ejecutar(()=>{if(compraProveedor.SelectedItem==null||compraSucursal.SelectedItem==null)throw new InvalidOperationException("Registra proveedores y sucursales activas primero.");int prov=Convert.ToInt32(((DataRowView)compraProveedor.SelectedItem)["Codigo"]);int suc=Convert.ToInt32(((DataRowView)compraSucursal.SelectedItem)["Codigo"]);int id=negocio.GuardarCompra(prov,suc,compraFecha.Value,lineas);lineas.Clear();MostrarLineas();MessageBox.Show("Compra No. "+id+" guardada. El inventario y el kardex fueron actualizados.","Compra guardada",MessageBoxButtons.OK,MessageBoxIcon.Information);});}
        private void Ejecutar(Action accion){try{accion();}catch(Exception ex){MostrarError(ex);}}
        private void MostrarError(Exception ex){MessageBox.Show(ex.Message,"No se pudo completar la operación",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
    }
}
