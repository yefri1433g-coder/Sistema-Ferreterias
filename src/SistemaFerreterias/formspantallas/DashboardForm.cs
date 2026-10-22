using System.Drawing.Drawing2D;

namespace Formspantallas;

public sealed class DashboardForm : Form
{
    private readonly Panel pageHost = new();
    private readonly List<Button> navButtons = new();
    private readonly Label breadcrumb = new();

    private static readonly string[] MenuItems =
    {
        "Dashboard", "Sucursales", "Proveedores", "Productos", "Categorías", "Compras",
        "Inventarios", "Stock", "Kardex", "Traslados", "Clientes", "Empleados", "Ventas", "Pagos", "Usuarios"
    };

    public DashboardForm()
    {
        Text = "Ferretería Central";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1120, 720);
        Size = new Size(1440, 900);
        BackColor = Theme.Canvas;

        var shell = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Margin = Padding.Empty };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        shell.Controls.Add(BuildSidebar(), 0, 0);
        shell.Controls.Add(BuildMainArea(), 1, 0);
        Controls.Add(shell);
        ShowPage("Dashboard");
    }

    private Control BuildSidebar()
    {
        var side = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, BackColor = Theme.Navy, Padding = new Padding(14, 16, 12, 10) };
        side.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
        side.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        side.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        side.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

        var brand = new Panel { Dock = DockStyle.Fill };
        brand.Controls.Add(new Label { Text = "◆", TextAlign = ContentAlignment.MiddleCenter, ForeColor = Theme.Navy, BackColor = Theme.Mint, Font = new Font("Segoe UI", 17, FontStyle.Bold), Size = new Size(38, 38), Location = new Point(4, 5) });
        brand.Controls.Add(new Label { Text = "FERRETERÍA\nCENTRAL", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold), AutoSize = true, Location = new Point(53, 5) });
        side.Controls.Add(brand, 0, 0);
        side.Controls.Add(new Label { Text = "MENÚ PRINCIPAL", ForeColor = Color.FromArgb(113, 133, 145), Font = new Font("Segoe UI", 8, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);

        var menu = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Theme.Navy, Padding = new Padding(0, 2, 0, 0) };
        foreach (var item in MenuItems)
        {
            var button = new Button
            {
                Text = "  " + IconFor(item) + "   " + item, Tag = item,
                TextAlign = ContentAlignment.MiddleLeft, Size = new Size(202, 35),
                FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(180, 194, 201),
                BackColor = Theme.Navy, Font = new Font("Segoe UI", 9), Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 0, 1)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Theme.NavyLight;
            button.Click += (_, _) => ShowPage((string)button.Tag!);
            menu.Controls.Add(button);
            navButtons.Add(button);
        }
        side.Controls.Add(menu, 0, 2);

        var profile = new Panel { Dock = DockStyle.Fill };
        profile.Controls.Add(new Label { Text = "AG", TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.FromArgb(232, 177, 141), ForeColor = Color.FromArgb(85, 61, 50), Font = new Font("Segoe UI", 9, FontStyle.Bold), Size = new Size(36, 36), Location = new Point(2, 7) });
        profile.Controls.Add(new Label { Text = "Alex García\nAdministrador", ForeColor = Color.White, Font = new Font("Segoe UI", 8), AutoSize = true, Location = new Point(48, 7) });
        side.Controls.Add(profile, 0, 3);
        return side;
    }

    private Control BuildMainArea()
    {
        var area = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, BackColor = Theme.Canvas, Padding = new Padding(28, 14, 28, 20) };
        area.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        area.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var header = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 310));
        breadcrumb.Text = "Inicio  /  Dashboard";
        breadcrumb.ForeColor = Theme.Muted;
        breadcrumb.Font = new Font("Segoe UI", 9);
        breadcrumb.TextAlign = ContentAlignment.MiddleLeft;
        breadcrumb.Dock = DockStyle.Fill;
        header.Controls.Add(breadcrumb, 0, 0);
        var profile = new Label { Text = "18 oct. 2026     Alex García  ▾     AG", ForeColor = Theme.Ink, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9) };
        header.Controls.Add(profile, 1, 0);
        area.Controls.Add(header, 0, 0);
        pageHost.Dock = DockStyle.Fill;
        pageHost.BackColor = Theme.Canvas;
        area.Controls.Add(pageHost, 0, 1);
        return area;
    }

    private void ShowPage(string pageName)
    {
        foreach (var button in navButtons)
        {
            var selected = Equals(button.Tag, pageName);
            button.BackColor = selected ? Theme.NavyLight : Theme.Navy;
            button.ForeColor = selected ? Color.White : Color.FromArgb(180, 194, 201);
            button.Font = new Font(selected ? "Segoe UI Semibold" : "Segoe UI", 9, selected ? FontStyle.Bold : FontStyle.Regular);
        }

        breadcrumb.Text = "Inicio  /  " + pageName;
        pageHost.Controls.Clear();
        UserControl view = pageName == "Dashboard" ? new DashboardView() : new ModuleView(ModuleCatalog.Get(pageName));
        view.Dock = DockStyle.Fill;
        pageHost.Controls.Add(view);
    }

    private static string IconFor(string title) => title switch
    {
        "Dashboard" => "▦", "Sucursales" => "⌂", "Proveedores" => "▣", "Productos" => "▤",
        "Categorías" => "▧", "Compras" => "▥", "Inventarios" => "▦", "Stock" => "◉",
        "Kardex" => "◷", "Traslados" => "⇄", "Clientes" => "♙", "Empleados" => "♟",
        "Ventas" => "↗", "Pagos" => "$", _ => "⚙"
    };
}

internal sealed class DashboardView : UserControl
{
    public DashboardView()
    {
        BackColor = Theme.Canvas;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Margin = Padding.Empty };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        Controls.Add(layout);

        var heading = new Panel { Dock = DockStyle.Fill };
        heading.Controls.Add(new Label { Text = "Buenos días, Alex 👋", Font = new Font("Segoe UI Semibold", 21, FontStyle.Bold), ForeColor = Theme.Ink, AutoSize = true, Location = new Point(0, 2) });
        heading.Controls.Add(new Label { Text = "Aquí tienes el resumen de tu ferretería.", Font = new Font("Segoe UI", 9), ForeColor = Theme.Muted, AutoSize = true, Location = new Point(2, 40) });
        layout.Controls.Add(heading, 0, 0);

        var metrics = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, Margin = new Padding(0, 0, 0, 10) };
        for (var i = 0; i < 4; i++) metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        metrics.Controls.Add(new MetricCard("VENTAS DE HOY", "$24,850", "+12.8% vs. ayer", "↗", Color.FromArgb(17, 147, 108)), 0, 0);
        metrics.Controls.Add(new MetricCard("PRODUCTOS", "1,248", "18 con stock bajo", "▤", Color.FromArgb(206, 139, 44)), 1, 0);
        metrics.Controls.Add(new MetricCard("PEDIDOS PENDIENTES", "16", "5 listos para entregar", "◷", Color.FromArgb(84, 119, 209)), 2, 0);
        metrics.Controls.Add(new MetricCard("CLIENTES ACTIVOS", "386", "+24 este mes", "♙", Color.FromArgb(134, 101, 204)), 3, 0);
        layout.Controls.Add(metrics, 0, 1);

        var middle = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Margin = new Padding(0, 0, 0, 10) };
        middle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67));
        middle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
        middle.Controls.Add(new SalesChartCard(), 0, 0);
        middle.Controls.Add(new InventoryStatusCard(), 1, 0);
        layout.Controls.Add(middle, 0, 2);

        var bottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
        bottom.Controls.Add(new RecentActivityCard(), 0, 0);
        bottom.Controls.Add(new QuickActionsCard(), 1, 0);
        layout.Controls.Add(bottom, 0, 3);
    }
}

internal sealed class MetricCard : Panel
{
    public MetricCard(string label, string value, string note, string icon, Color accent)
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Margin = new Padding(0, 0, 9, 0);
        Controls.Add(new Label { Text = icon, TextAlign = ContentAlignment.MiddleCenter, ForeColor = accent, BackColor = Color.FromArgb(242, 249, 247), Font = new Font("Segoe UI", 14, FontStyle.Bold), Size = new Size(32, 32), Location = new Point(12, 10) });
        Controls.Add(new Label { Text = label, ForeColor = Theme.Muted, Font = new Font("Segoe UI", 7.5f, FontStyle.Bold), AutoSize = true, Location = new Point(12, 50) });
        Controls.Add(new Label { Text = value, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 18, FontStyle.Bold), AutoSize = true, Location = new Point(12, 68) });
        Controls.Add(new Label { Text = note, ForeColor = accent, Font = new Font("Segoe UI", 7.5f), AutoSize = true, Location = new Point(100, 77) });
    }
}

internal sealed class SalesChartCard : Panel
{
    public SalesChartCard()
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(16);
        Margin = new Padding(0, 0, 10, 0);
        Controls.Add(new Label { Text = "Rendimiento de ventas\nIngresos de los últimos 7 días", Dock = DockStyle.Top, Height = 43, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold) });
        Controls.Add(new Label { Text = "$138,420    ↗  8.4% vs. semana pasada", Dock = DockStyle.Top, Height = 30, ForeColor = Theme.Green, Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold) });
        var chart = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        chart.Paint += DrawChart;
        Controls.Add(chart);
    }

    private static void DrawChart(object? sender, PaintEventArgs e)
    {
        if (sender is not Panel panel) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var left = 12;
        var top = 8;
        var width = Math.Max(30, panel.ClientSize.Width - 24);
        var height = Math.Max(40, panel.ClientSize.Height - 30);
        using var grid = new Pen(Color.FromArgb(237, 241, 242));
        for (var i = 0; i < 4; i++) g.DrawLine(grid, left, top + i * height / 3, left + width, top + i * height / 3);
        var values = new[] { .45f, .62f, .56f, .73f, .67f, .9f, .78f };
        var points = values.Select((v, i) => new PointF(left + i * width / 6f, top + height * (1 - v))).ToArray();
        using var line = new Pen(Theme.Green, 3);
        using var fill = new SolidBrush(Color.FromArgb(35, Theme.Mint));
        var area = points.Concat(new[] { new PointF(points[^1].X, top + height), new PointF(points[0].X, top + height) }).ToArray();
        g.FillPolygon(fill, area);
        g.DrawLines(line, points);
        using var dotBrush = new SolidBrush(Color.White);
        using var dotPen = new Pen(Theme.Green, 2);
        foreach (var point in points)
        {
            g.FillEllipse(dotBrush, point.X - 4, point.Y - 4, 8, 8);
            g.DrawEllipse(dotPen, point.X - 4, point.Y - 4, 8, 8);
        }
        var days = new[] { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };
        for (var i = 0; i < days.Length; i++)
            TextRenderer.DrawText(g, days[i], panel.Font, new Rectangle((int)(left + i * width / 6f) - 18, panel.ClientSize.Height - 20, 36, 16), Theme.Muted, TextFormatFlags.HorizontalCenter);
    }
}

internal sealed class InventoryStatusCard : Panel
{
    public InventoryStatusCard()
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(14);
        Controls.Add(new Label { Text = "Inventario\nEstado de existencias", Dock = DockStyle.Top, Height = 44, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold) });
        var chart = new Panel { Dock = DockStyle.Fill };
        chart.Paint += (_, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var diameter = Math.Min(136, Math.Min(chart.ClientSize.Width - 10, chart.ClientSize.Height - 10));
            if (diameter < 30) return;
            var rect = new Rectangle((chart.ClientSize.Width - diameter) / 2, (chart.ClientSize.Height - diameter) / 2, diameter, diameter);
            using var basePen = new Pen(Color.FromArgb(237, 242, 240), 16);
            using var availablePen = new Pen(Theme.Mint, 16);
            using var lowPen = new Pen(Color.FromArgb(242, 180, 78), 16);
            g.DrawEllipse(basePen, rect);
            g.DrawArc(availablePen, rect, -90, 258);
            g.DrawArc(lowPen, rect, 174, 42);
            TextRenderer.DrawText(g, "87%\ndisponible", Font, rect, Theme.Ink, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        Controls.Add(chart);
        Controls.Add(new Label { Text = "● Disponible 1,084     ● Stock bajo 126", Dock = DockStyle.Bottom, Height = 23, ForeColor = Theme.Muted, Font = new Font("Segoe UI", 7.5f), TextAlign = ContentAlignment.MiddleCenter });
    }
}

internal sealed class RecentActivityCard : Panel
{
    public RecentActivityCard()
    {
        BackColor = Color.White;
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(14, 10, 14, 6);
        Margin = new Padding(0, 0, 10, 0);
        Controls.Add(new Label { Text = "Actividad reciente\nMovimientos de hoy", Dock = DockStyle.Top, Height = 39, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold) });
        var rows = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(0, 5, 0, 0) };
        rows.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rows.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58));
        rows.Controls.Add(Activity("Venta registrada", "#V-1048 · Roberto Martínez", Theme.Green), 0, 0);
        rows.Controls.Add(Activity("Compra recibida", "Aceros México · Orden OC-048", Color.FromArgb(196, 134, 45)), 0, 1);
        rows.Controls.Add(new Label { Text = "10:42", ForeColor = Theme.Muted, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 1, 0);
        rows.Controls.Add(new Label { Text = "10:18", ForeColor = Theme.Muted, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 1, 1);
        Controls.Add(rows);
    }

    private static Control Activity(string title, string detail, Color accent)
    {
        var row = new Panel { Dock = DockStyle.Fill };
        row.Controls.Add(new Label { Text = "●", ForeColor = accent, Font = new Font("Segoe UI", 13), Location = new Point(0, 4), AutoSize = true });
        row.Controls.Add(new Label { Text = title, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold), Location = new Point(22, 1), AutoSize = true });
        row.Controls.Add(new Label { Text = detail, ForeColor = Theme.Muted, Font = new Font("Segoe UI", 7.5f), Location = new Point(22, 19), AutoSize = true });
        return row;
    }
}

internal sealed class QuickActionsCard : Panel
{
    public QuickActionsCard()
    {
        BackColor = Theme.NavyLight;
        Padding = new Padding(15);
        Controls.Add(new Label { Text = "ACCESO RÁPIDO", ForeColor = Color.FromArgb(141, 229, 194), Font = new Font("Segoe UI", 8, FontStyle.Bold), AutoSize = true, Location = new Point(15, 13) });
        Controls.Add(new Label { Text = "¿Qué necesitas hacer?", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold), AutoSize = true, Location = new Point(15, 37) });
        var sale = Theme.ActionButton("＋  Nueva venta"); sale.Location = new Point(15, 76); sale.Width = 132; Controls.Add(sale);
        var product = Theme.ActionButton("＋  Producto", false); product.Location = new Point(155, 76); product.Width = 120; Controls.Add(product);
    }
}
