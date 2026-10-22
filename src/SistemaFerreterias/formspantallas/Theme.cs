namespace Formspantallas;

internal static class Theme
{
    public static readonly Color Canvas = Color.FromArgb(244, 247, 248);
    public static readonly Color Navy = Color.FromArgb(16, 31, 43);
    public static readonly Color NavyLight = Color.FromArgb(24, 59, 67);
    public static readonly Color Green = Color.FromArgb(22, 134, 106);
    public static readonly Color Mint = Color.FromArgb(43, 211, 154);
    public static readonly Color Ink = Color.FromArgb(23, 43, 53);
    public static readonly Color Muted = Color.FromArgb(117, 134, 143);
    public static readonly Color Line = Color.FromArgb(229, 235, 237);

    public static Label Text(string value, float size = 10, bool bold = false, Color? color = null) => new()
    {
        Text = value,
        AutoSize = true,
        ForeColor = color ?? Ink,
        Font = new Font(bold ? "Segoe UI Semibold" : "Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular)
    };

    public static Panel Card() => new()
    {
        BackColor = Color.White,
        BorderStyle = BorderStyle.FixedSingle,
        Margin = new Padding(0),
        Padding = new Padding(16)
    };

    public static Button ActionButton(string text, bool primary = true)
    {
        var button = new Button
        {
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = primary ? Green : Color.FromArgb(239, 248, 244),
            ForeColor = primary ? Color.White : Green,
            Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Height = 38,
            Padding = new Padding(10, 0, 10, 0)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(18, 113, 88) : Color.FromArgb(225, 244, 235);
        return button;
    }
}
