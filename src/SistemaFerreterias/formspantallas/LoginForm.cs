namespace Formspantallas;

public sealed class LoginForm : Form
{
    private readonly TextBox username = new();
    private readonly TextBox password = new();
    private Button? loginButton;

    public LoginForm()
    {
        Text = "Ferretería Central | Iniciar sesión";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 620);
        Size = new Size(1200, 760);
        BackColor = Theme.Canvas;

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Margin = Padding.Empty };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46));
        layout.Controls.Add(BuildWelcomePanel(), 0, 0);
        layout.Controls.Add(BuildLoginPanel(), 1, 0);
        Controls.Add(layout);
        AcceptButton = loginButton;
    }

    private Control BuildWelcomePanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Navy };
        panel.Controls.Add(new Label { Text = "◆", ForeColor = Theme.Navy, BackColor = Theme.Mint, Font = new Font("Segoe UI", 18, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Size = new Size(42, 42), Location = new Point(48, 44) });
        panel.Controls.Add(new Label { Text = "FERRETERÍA CENTRAL", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold), AutoSize = true, Location = new Point(104, 56) });
        panel.Controls.Add(new Label { Text = "Tu negocio,\nbajo control.", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 34, FontStyle.Bold), AutoSize = true, Location = new Point(54, 205) });
        panel.Controls.Add(new Label { Text = "Ventas, inventario y sucursales en un solo lugar.", ForeColor = Color.FromArgb(184, 203, 201), Font = new Font("Segoe UI", 12), AutoSize = true, Location = new Point(58, 315) });

        var info = new Panel { BackColor = Color.FromArgb(35, 255, 255, 255), Size = new Size(490, 98), Location = new Point(56, 410) };
        info.Controls.Add(new Label { Text = "✓", ForeColor = Theme.Navy, BackColor = Theme.Mint, Font = new Font("Segoe UI", 15, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Size = new Size(38, 38), Location = new Point(22, 29) });
        info.Controls.Add(new Label { Text = "Una forma más simple de administrar", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold), AutoSize = true, Location = new Point(76, 25) });
        info.Controls.Add(new Label { Text = "Toda la operación de tu ferretería, siempre a la mano.", ForeColor = Color.FromArgb(184, 203, 201), Font = new Font("Segoe UI", 9), AutoSize = true, Location = new Point(76, 52) });
        panel.Controls.Add(info);
        panel.Controls.Add(new Label { Text = "© 2026 Ferretería Central", ForeColor = Color.FromArgb(154, 175, 177), Font = new Font("Segoe UI", 8), AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Location = new Point(54, 690) });
        return panel;
    }

    private Control BuildLoginPanel()
    {
        var background = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(247, 250, 251), Padding = new Padding(34) };
        var card = new Panel { Size = new Size(430, 510), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
        background.Controls.Add(card);
        background.Resize += (_, _) =>
        {
            card.Left = Math.Max(0, (background.ClientSize.Width - card.Width) / 2);
            card.Top = Math.Max(0, (background.ClientSize.Height - card.Height) / 2);
        };
        card.Left = 30;
        card.Top = 40;

        card.Controls.Add(new Label { Text = "Qué gusto verte", ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold), AutoSize = true, Location = new Point(38, 42) });
        card.Controls.Add(new Label { Text = "Inicia sesión para entrar a tu espacio de trabajo.", ForeColor = Theme.Muted, Font = new Font("Segoe UI", 9), AutoSize = true, Location = new Point(40, 82) });

        AddField(card, "Correo o nombre de usuario", username, 130);
        AddField(card, "Contraseña", password, 232);
        password.UseSystemPasswordChar = true;

        var forgot = new LinkLabel { Text = "¿Olvidaste tu contraseña?", LinkColor = Theme.Green, AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(267, 298) };
        card.Controls.Add(forgot);
        var remember = new CheckBox { Text = "Mantener sesión iniciada", ForeColor = Theme.Muted, Font = new Font("Segoe UI", 8), AutoSize = true, Location = new Point(39, 356) };
        card.Controls.Add(remember);

        var login = CreateLoginButton();
        loginButton = login;
        login.Location = new Point(38, 315);
        login.Size = new Size(352, 42);
        card.Controls.Add(login);
        card.Controls.Add(new Panel { BackColor = Theme.Line, Location = new Point(38, 392), Size = new Size(352, 1) });
        card.Controls.Add(new Label { Text = "●  Conexión segura · Solo personal autorizado", ForeColor = Theme.Muted, Font = new Font("Segoe UI", 8), AutoSize = true, Location = new Point(68, 421) });
        card.Controls.Add(new Label { Text = "Pantalla de demostración: no valida usuarios contra la base de datos.", ForeColor = Theme.Muted, Font = new Font("Segoe UI", 7), AutoSize = true, Location = new Point(39, 473) });
        return background;
    }

    private static void AddField(Control parent, string label, TextBox input, int top)
    {
        parent.Controls.Add(new Label { Text = label, ForeColor = Theme.Ink, Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold), AutoSize = true, Location = new Point(40, top) });
        input.Location = new Point(38, top + 23);
        input.Size = new Size(352, 36);
        input.Font = new Font("Segoe UI", 9);
        input.BorderStyle = BorderStyle.FixedSingle;
        input.BackColor = Color.FromArgb(248, 250, 251);
        parent.Controls.Add(input);
    }

    private Button CreateLoginButton()
    {
        var button = Theme.ActionButton("Iniciar sesión   →");
        button.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Escribe tu usuario y contraseña para continuar.", "Datos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        };
        return button;
    }
}
