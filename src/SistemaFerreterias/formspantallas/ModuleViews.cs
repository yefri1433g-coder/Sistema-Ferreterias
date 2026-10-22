namespace Formspantallas;

internal sealed class ModuleData
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string ActionText { get; init; } = "＋  Nuevo registro";
    public (string Label, string Value, string Note)[] Metrics { get; init; } = Array.Empty<(string, string, string)>();
    public string[] Columns { get; init; } = Array.Empty<string>();
    public string[][] Rows { get; init; } = Array.Empty<string[]>();
}

internal sealed class ModuleView : UserControl
{
    private readonly ModuleData data;

    public ModuleView(ModuleData moduleData)
    {
        data = moduleData;
        BackColor = Theme.Canvas;
        AutoScroll = true;

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = Padding.Empty };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 75));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 106));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(layout);
        layout.Controls.Add(BuildHeading(), 0, 0);
        layout.Controls.Add(BuildMetrics(), 0, 1);
        layout.Controls.Add(BuildTableCard(), 0, 2);
    }

    private Control BuildHeading()
    {
        var panel = new Panel { Dock = DockStyle.Fill };
        panel.Controls.Add(new Label { Text = data.Title, AutoSize = true, Font = new Font("Segoe UI Semibold", 20, FontStyle.Bold), ForeColor = Theme.Ink, Location = new Point(0, 0) });
        panel.Controls.Add(new Label { Text = data.Description, AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Theme.Muted, Location = new Point(2, 38) });
        return panel;
    }

    private Control BuildMetrics()
    {
        var row = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Margin = new Padding(0, 0, 0, 10) };
        for (var i = 0; i < 3; i++) row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333f));
        var colors = new[] { Color.FromArgb(17, 147, 108), Color.FromArgb(84, 119, 209), Color.FromArgb(206, 139, 44) };
        for (var i = 0; i < Math.Min(3, data.Metrics.Length); i++)
        {
            var metric = data.Metrics[i];
            row.Controls.Add(new ModuleMetricCard(metric.Label, metric.Value, metric.Note, colors[i]), i, 0);
        }
        return row;
    }

    private Control BuildTableCard()
    {
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = Padding.Empty };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = Padding.Empty };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        card.Controls.Add(layout);

        var toolbar = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 405));
        var labels = new Panel { Dock = DockStyle.Fill };
        labels.Controls.Add(new Label { Text = data.Title + " recientes", AutoSize = true, Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold), ForeColor = Theme.Ink, Location = new Point(0, 1) });
        labels.Controls.Add(new Label { Text = "Información de muestra · 18 oct. 2026", AutoSize = true, Font = new Font("Segoe UI", 8), ForeColor = Theme.Muted, Location = new Point(1, 26) });
        toolbar.Controls.Add(labels, 0, 0);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 1, 0, 0) };
        var search = new TextBox { PlaceholderText = "⌕  Buscar...", BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(248, 250, 251), Font = new Font("Segoe UI", 8.5f), Width = 195, Height = 35, Margin = new Padding(0, 0, 8, 0) };
        var filter = Theme.ActionButton("☷  Filtros", false); filter.Width = 84; filter.Margin = new Padding(0, 0, 8, 0);
        var add = Theme.ActionButton(data.ActionText); add.Width = 112; add.Margin = Padding.Empty;
        actions.Controls.Add(search); actions.Controls.Add(filter); actions.Controls.Add(add);
        toolbar.Controls.Add(actions, 1, 0);
        layout.Controls.Add(toolbar, 0, 0);

        var grid = BuildGrid();
        layout.Controls.Add(grid, 0, 1);
        return card;
    }

    private DataGridView BuildGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            GridColor = Theme.Line,
            RowHeadersVisible = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToResizeRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight = 42,
            RowTemplate = { Height = 54 },
            Font = new Font("Segoe UI", 9),
            DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.FromArgb(67, 87, 94), SelectionBackColor = Color.FromArgb(232, 248, 241), SelectionForeColor = Theme.Ink, Padding = new Padding(8, 0, 4, 0) },
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(250, 252, 252), SelectionBackColor = Color.FromArgb(232, 248, 241), SelectionForeColor = Theme.Ink },
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(245, 248, 248), ForeColor = Theme.Muted, Font = new Font("Segoe UI", 8, FontStyle.Bold), Padding = new Padding(8, 0, 4, 0) }
        };

        foreach (var column in data.Columns)
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = column, Name = column, SortMode = DataGridViewColumnSortMode.NotSortable });
        foreach (var row in data.Rows)
            grid.Rows.Add(row.Cast<object>().ToArray());
        return grid;
    }
}

internal sealed class ModuleMetricCard : Panel
{
    public ModuleMetricCard(string label, string value, string note, Color accent)
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Margin = new Padding(0, 0, 10, 0);
        Controls.Add(new Label { Text = "●", ForeColor = accent, Font = new Font("Segoe UI", 14), AutoSize = true, Location = new Point(13, 11) });
        Controls.Add(new Label { Text = label, ForeColor = Theme.Muted, Font = new Font("Segoe UI", 8, FontStyle.Bold), AutoSize = true, Location = new Point(36, 15) });
        Controls.Add(new Label { Text = value, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 18, FontStyle.Bold), AutoSize = true, Location = new Point(13, 43) });
        Controls.Add(new Label { Text = note, ForeColor = accent, Font = new Font("Segoe UI", 8), AutoSize = true, Location = new Point(145, 55) });
    }
}

internal static class ModuleCatalog
{
    public static ModuleData Get(string name) => name switch
    {
        "Sucursales" => Make(name, "Administra ubicaciones, responsables y actividad por sucursal.", ("SUCURSALES", "8", "+1 este año"), ("ACTIVAS", "7", "En operación"), ("PERSONAL", "64", "En todas las sedes"),
            new[] { "Sucursal", "Responsable", "Teléfono", "Empleados", "Estado" },
            new[] { new[] { "Centro", "María López", "55 2233 4455", "12", "Activa" }, new[] { "Norte", "Carlos Reyes", "81 1122 3344", "9", "Activa" }, new[] { "Sur", "Ana Torres", "55 6677 8899", "8", "Activa" }, new[] { "Industrial", "Luis Pérez", "33 4455 6677", "11", "Activa" } }, "＋  Nueva sucursal"),
        "Proveedores" => Make(name, "Contactos, categorías y actividad de compra.", ("REGISTRADOS", "42", "Directorio"), ("ACTIVOS", "38", "90% del total"), ("COMPRAS DEL MES", "$186,420", "+8.4% este mes"),
            new[] { "Proveedor", "Contacto", "Categoría", "Última compra", "Estado" },
            new[] { new[] { "Aceros México", "55 3344 2200", "Materiales", "15 oct. 2026", "Activo" }, new[] { "Herramientas del Norte", "81 2233 4455", "Herramientas", "13 oct. 2026", "Activo" }, new[] { "Pinturas y Más", "55 8765 4321", "Pinturas", "10 oct. 2026", "Activo" }, new[] { "Eléctrica y Ferretera", "33 5544 6677", "Electricidad", "07 oct. 2026", "Pendiente" } }, "＋  Nuevo proveedor"),
        "Productos" => Make(name, "Catálogo de artículos, precios y disponibilidad.", ("PRODUCTOS", "1,248", "En catálogo"), ("STOCK BAJO", "18", "Necesitan atención"), ("CATEGORÍAS", "16", "Organizadas"),
            new[] { "Producto", "Código", "Categoría", "Precio", "Existencia" },
            new[] { new[] { "Taladro percutor 1/2”", "HER-00421", "Herramientas", "$1,249.00", "24" }, new[] { "Cemento gris 50 kg", "CON-00083", "Construcción", "$238.00", "86" }, new[] { "Pintura blanca 4 L", "PIN-00210", "Pinturas", "$389.00", "12" }, new[] { "Cable THW calibre 12", "ELE-00118", "Electricidad", "$18.50", "340" } }, "＋  Nuevo producto"),
        "Categorías" => Make(name, "Mantén tu catálogo organizado y fácil de consultar.", ("CATEGORÍAS", "16", "En catálogo"), ("PRODUCTOS", "1,248", "Clasificados"), ("SIN PRODUCTOS", "2", "Revisar catálogo"),
            new[] { "Categoría", "Descripción", "Productos", "Actualización", "Estado" },
            new[] { new[] { "Herramientas", "Manuales y eléctricas", "286", "15 oct. 2026", "Activa" }, new[] { "Construcción", "Material y acabados", "314", "14 oct. 2026", "Activa" }, new[] { "Pinturas", "Pinturas y accesorios", "128", "12 oct. 2026", "Activa" }, new[] { "Electricidad", "Material e iluminación", "205", "09 oct. 2026", "Activa" } }, "＋  Nueva categoría"),
        "Compras" => Make(name, "Órdenes de compra y recepción de mercancía.", ("ÓRDENES DEL MES", "38", "Compras registradas"), ("POR RECIBIR", "5", "En proceso"), ("IMPORTE DEL MES", "$186,420", "+8.4% este mes"),
            new[] { "Folio", "Proveedor", "Fecha", "Total", "Estado" },
            new[] { new[] { "OC-2026-048", "Aceros México", "17 oct. 2026", "$18,450.00", "Recibida" }, new[] { "OC-2026-047", "Herramientas del Norte", "16 oct. 2026", "$9,720.00", "En tránsito" }, new[] { "OC-2026-046", "Pinturas y Más", "14 oct. 2026", "$6,280.00", "Recibida" }, new[] { "OC-2026-045", "Cementos Metropolitanos", "12 oct. 2026", "$22,360.00", "Pendiente" } }, "＋  Nueva compra"),
        "Inventarios" => Make(name, "Existencias por producto y sucursal.", ("ARTÍCULOS", "1,248", "En inventario"), ("VALOR TOTAL", "$2.4 M", "Valuación estimada"), ("ALERTAS", "18", "Stock por revisar"),
            new[] { "Producto", "Sucursal", "Existencia", "Mínimo", "Estado" },
            new[] { new[] { "Taladro percutor 1/2”", "Centro", "24", "8", "Disponible" }, new[] { "Cemento gris 50 kg", "Norte", "86", "20", "Disponible" }, new[] { "Pintura blanca 4 L", "Centro", "12", "15", "Stock bajo" }, new[] { "Cable THW calibre 12", "Sur", "340", "60", "Disponible" } }, "⇄  Registrar movimiento"),
        "Stock" => Make(name, "Detecta productos por reabastecer y evita faltantes.", ("DISPONIBLES", "1,084", "Existencia correcta"), ("STOCK BAJO", "126", "Reabastecimiento sugerido"), ("AGOTADOS", "38", "Sin existencias"),
            new[] { "Producto", "Código", "Sucursal", "Existencia", "Nivel" },
            new[] { new[] { "Pintura blanca 4 L", "PIN-00210", "Centro", "12", "Bajo" }, new[] { "Disco de corte 4 1/2”", "HER-00306", "Norte", "8", "Bajo" }, new[] { "Cemento gris 50 kg", "CON-00083", "Sur", "86", "Correcto" }, new[] { "Guantes de carnaza", "SEG-00054", "Centro", "0", "Agotado" } }, "＋  Ajustar stock"),
        "Kardex" => Make(name, "Historial detallado de movimientos de inventario.", ("MOVIMIENTOS HOY", "42", "Entradas y salidas"), ("ENTRADAS", "26", "Recepciones"), ("SALIDAS", "16", "Ventas y ajustes"),
            new[] { "Fecha", "Producto", "Movimiento", "Cantidad", "Referencia" },
            new[] { new[] { "17 oct. · 10:42", "Taladro percutor", "Salida", "−2", "Venta V-1048" }, new[] { "17 oct. · 10:18", "Cemento gris 50 kg", "Entrada", "+40", "Compra OC-048" }, new[] { "17 oct. · 09:56", "Pintura blanca 4 L", "Ajuste", "+3", "Conteo físico" }, new[] { "16 oct. · 17:21", "Cable THW calibre 12", "Salida", "−25", "Venta V-1041" } }, "＋  Registrar movimiento"),
        "Traslados" => Make(name, "Seguimiento de mercancía entre sucursales.", ("TRASLADOS DEL MES", "24", "Registrados"), ("EN TRÁNSITO", "3", "En movimiento"), ("POR APROBAR", "2", "Requieren revisión"),
            new[] { "Folio", "Origen", "Destino", "Fecha", "Estado" },
            new[] { new[] { "TR-2026-024", "Centro", "Norte", "17 oct. 2026", "En tránsito" }, new[] { "TR-2026-023", "Sur", "Centro", "16 oct. 2026", "Enviado" }, new[] { "TR-2026-022", "Norte", "Industrial", "15 oct. 2026", "Recibido" }, new[] { "TR-2026-021", "Centro", "Sur", "14 oct. 2026", "Por aprobar" } }, "＋  Nuevo traslado"),
        "Clientes" => Make(name, "Consulta sus datos y revisa la actividad reciente.", ("CLIENTES", "386", "En el directorio"), ("NUEVOS ESTE MES", "24", "+6.6%"), ("COMPRA RECIENTE", "152", "Últimos 30 días"),
            new[] { "Cliente", "Teléfono", "Correo", "Última compra", "Estado" },
            new[] { new[] { "Roberto Martínez", "55 1234 5678", "roberto.m@email.com", "15 oct. 2026", "Activo" }, new[] { "Laura Gómez", "55 9876 5432", "laura.g@email.com", "14 oct. 2026", "Activo" }, new[] { "Construcciones del Sur", "55 4321 0987", "compras@cdsur.mx", "10 oct. 2026", "Activo" }, new[] { "Ana Patricia Ruiz", "55 2468 1357", "ana.ruiz@email.com", "08 oct. 2026", "Activo" } }, "＋  Nuevo cliente"),
        "Empleados" => Make(name, "Directorio de personal y asignación a sucursales.", ("EMPLEADOS", "64", "Registrados"), ("ACTIVOS", "61", "En funciones"), ("SUCURSALES", "8", "Con personal"),
            new[] { "Empleado", "Puesto", "Sucursal", "Teléfono", "Estado" },
            new[] { new[] { "María López", "Gerente", "Centro", "55 2233 4455", "Activo" }, new[] { "Carlos Reyes", "Vendedor", "Norte", "81 1122 3344", "Activo" }, new[] { "Ana Torres", "Cajera", "Sur", "55 6677 8899", "Activo" }, new[] { "Luis Pérez", "Almacenista", "Industrial", "33 4455 6677", "Activo" } }, "＋  Nuevo empleado"),
        "Ventas" => Make(name, "Seguimiento de ventas, clientes y montos.", ("VENTAS DE HOY", "48", "Operaciones"), ("TOTAL DEL DÍA", "$24,850", "+12.8% vs. ayer"), ("TICKET PROMEDIO", "$517.70", "Hoy"),
            new[] { "Folio", "Cliente", "Vendedor", "Fecha", "Total" },
            new[] { new[] { "V-1048", "Roberto Martínez", "María López", "17 oct. 2026", "$2,450.00" }, new[] { "V-1047", "Laura Gómez", "Carlos Reyes", "17 oct. 2026", "$1,280.00" }, new[] { "V-1046", "Público general", "Ana Torres", "17 oct. 2026", "$780.00" }, new[] { "V-1045", "Construcciones del Sur", "Luis Pérez", "17 oct. 2026", "$4,610.00" } }, "＋  Nueva venta"),
        "Pagos" => Make(name, "Pagos recibidos y saldos pendientes.", ("PAGOS DE HOY", "36", "Registrados"), ("COBRADO HOY", "$21,430", "Todos los métodos"), ("PENDIENTE", "$8,260", "Saldo por cobrar"),
            new[] { "Folio", "Cliente", "Método", "Fecha", "Importe" },
            new[] { new[] { "P-2084", "Roberto Martínez", "Tarjeta", "17 oct. 2026", "$2,450.00" }, new[] { "P-2083", "Laura Gómez", "Efectivo", "17 oct. 2026", "$1,280.00" }, new[] { "P-2082", "Público general", "Transferencia", "17 oct. 2026", "$780.00" }, new[] { "P-2081", "Construcciones del Sur", "Crédito", "16 oct. 2026", "$4,610.00" } }, "＋  Registrar pago"),
        "Usuarios" => Make(name, "Gestiona cuentas, roles y acceso al sistema.", ("USUARIOS", "28", "Registrados"), ("ACTIVOS", "25", "Acceso habilitado"), ("ROLES", "5", "Perfiles de acceso"),
            new[] { "Usuario", "Rol", "Sucursal", "Último acceso", "Estado" },
            new[] { new[] { "admin.central", "Administrador", "Centro", "Hoy · 09:14", "Activo" }, new[] { "mlopez", "Gerente", "Centro", "Hoy · 08:52", "Activo" }, new[] { "creyes", "Vendedor", "Norte", "Hoy · 08:41", "Activo" }, new[] { "atorres", "Cajera", "Sur", "Ayer · 18:06", "Activo" } }, "＋  Nuevo usuario"),
        _ => Make(name, "Administra la información del sistema.", ("REGISTROS", "0", "Datos de muestra"), ("ACTIVOS", "0", "Disponibles"), ("ACTUALIZACIÓN", "Hoy", "Última consulta"), new[] { "Nombre", "Descripción", "Fecha", "Responsable", "Estado" }, Array.Empty<string[]>(), "＋  Nuevo registro")
    };

    private static ModuleData Make(string title, string description,
        (string, string, string) m1, (string, string, string) m2, (string, string, string) m3,
        string[] columns, string[][] rows, string action) => new()
    {
        Title = title, Description = description, Metrics = new[] { m1, m2, m3 },
        Columns = columns, Rows = rows, ActionText = action
    };
}
