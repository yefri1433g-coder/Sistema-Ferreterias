using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidad;
using Negocio;

namespace Presentacion
{
    public partial class LoginForms : Form
    {
        private readonly UsuarioNegocio _negocio = new UsuarioNegocio();

        public LoginForms()
        {
            InitializeComponent();
            ConstruirLogin();
        }

        private void ConstruirLogin()
        {
            Controls.Clear();
            label1 = new Label();
            label2 = new Label();
            txtUsuario = new TextBox();
            txtContrasena = new TextBox();
            btnIngresar = new FontAwesome.Sharp.IconButton();
            btnSalir = new FontAwesome.Sharp.IconButton();
            btnIngresar.Click += btnIngresar_Click;
            btnSalir.Click += btnSalir_Click;

            AutoScaleMode = AutoScaleMode.None;
            Text = "Acceso | Ferretería Multisucursal";
            ClientSize = new Size(1280, 800);
            MinimumSize = new Size(1000, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(238, 245, 242);
            Font = new Font("Segoe UI", 9F);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.FromArgb(238, 245, 242)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            Panel bienvenida = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(12, 82, 76), Padding = new Padding(72, 48, 64, 38) };
            TableLayoutPanel contenidoBienvenida = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
            contenidoBienvenida.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            contenidoBienvenida.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contenidoBienvenida.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            contenidoBienvenida.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

            Label marca = new Label
            {
                Text = "◇  FERRETERÍA MULTISUCURSAL",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 19F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            FlowLayoutPanel textoPrincipal = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 68, 0, 0)
            };
            Label mensajePrincipal = new Label
            {
                Text = "Todo para construir\ny crear.",
                AutoSize = false,
                Width = 590,
                Height = 154,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 29F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            Label descripcionPrincipal = new Label
            {
                Text = "Administra tu negocio desde un solo lugar.",
                AutoSize = true,
                ForeColor = Color.FromArgb(210, 232, 228),
                Font = new Font("Segoe UI", 12F),
                BackColor = Color.Transparent,
                Margin = new Padding(2, 4, 0, 0)
            };
            textoPrincipal.Controls.Add(mensajePrincipal);
            textoPrincipal.Controls.Add(descripcionPrincipal);

            Panel tarjetaInformativa = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 255, 255, 255),
                Padding = new Padding(22, 15, 16, 12),
                Margin = new Padding(0, 0, 90, 0)
            };
            RedondearPanel(tarjetaInformativa, 20);
            Label iconoCorrecto = new Label
            {
                Text = "✓",
                Location = new Point(22, 15),
                Size = new Size(48, 48),
                BackColor = Color.FromArgb(31, 181, 132),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            RedondearPanel(iconoCorrecto, 24);
            Label tituloTarjeta = new Label
            {
                Text = "Control sencillo para tu ferretería",
                Location = new Point(86, 15),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            Label textoTarjeta = new Label
            {
                Text = "Ventas, inventario y sucursales en un mismo sistema.",
                Location = new Point(86, 45),
                AutoSize = true,
                ForeColor = Color.FromArgb(210, 232, 228),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.Transparent
            };
            tarjetaInformativa.Controls.Add(iconoCorrecto);
            tarjetaInformativa.Controls.Add(tituloTarjeta);
            tarjetaInformativa.Controls.Add(textoTarjeta);

            Label pieMarca = new Label
            {
                Text = "© 2026 Ferretería Multisucursal",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(190, 220, 215),
                Font = new Font("Segoe UI", 8.5F),
                TextAlign = ContentAlignment.BottomLeft,
                BackColor = Color.Transparent
            };
            contenidoBienvenida.Controls.Add(marca, 0, 0);
            contenidoBienvenida.Controls.Add(textoPrincipal, 0, 1);
            contenidoBienvenida.Controls.Add(tarjetaInformativa, 0, 2);
            contenidoBienvenida.Controls.Add(pieMarca, 0, 3);
            bienvenida.Controls.Add(contenidoBienvenida);

            Panel zonaAcceso = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(238, 245, 242), Padding = new Padding(34) };
            Panel sombraTarjeta = new Panel { Size = new Size(490, 610), BackColor = Color.FromArgb(222, 235, 230) };
            Panel tarjetaAcceso = new Panel { Size = new Size(490, 610), BackColor = Color.White, Padding = Padding.Empty };
            RedondearPanel(tarjetaAcceso, 26);
            zonaAcceso.Controls.Add(sombraTarjeta);
            zonaAcceso.Controls.Add(tarjetaAcceso);
            tarjetaAcceso.BringToFront();
            EventHandler centrarTarjeta = (s, e) =>
            {
                int x = Math.Max(20, (zonaAcceso.ClientSize.Width - tarjetaAcceso.Width) / 2);
                int y = Math.Max(20, (zonaAcceso.ClientSize.Height - tarjetaAcceso.Height) / 2);
                tarjetaAcceso.Location = new Point(x, y);
                sombraTarjeta.Location = new Point(x + 5, y + 7);
            };
            zonaAcceso.Resize += centrarTarjeta;
            centrarTarjeta(this, EventArgs.Empty);

            label1.Text = "Correo o nombre de usuario";
            label1.SetBounds(48, 132, 394, 22);
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(54, 76, 79);
            label1.BackColor = Color.Transparent;

            txtUsuario.SetBounds(48, 160, 394, 50);
            txtUsuario.Font = new Font("Segoe UI", 11F);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.BackColor = Color.FromArgb(250, 252, 251);
            txtUsuario.ForeColor = Color.FromArgb(35, 55, 58);
            txtUsuario.Margin = Padding.Empty;
            txtUsuario.TabIndex = 0;

            label2.Text = "Contraseña";
            label2.SetBounds(48, 235, 300, 22);
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(54, 76, 79);
            label2.BackColor = Color.Transparent;

            txtContrasena.SetBounds(48, 263, 394, 50);
            txtContrasena.Font = new Font("Segoe UI", 12F);
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.BackColor = Color.FromArgb(250, 252, 251);
            txtContrasena.ForeColor = Color.FromArgb(35, 55, 58);
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.Margin = Padding.Empty;
            txtContrasena.TabIndex = 1;

            Label tituloAcceso = new Label
            {
                Text = "Bienvenido",
                Location = new Point(48, 43),
                Size = new Size(394, 44),
                Font = new Font("Segoe UI", 23F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 53, 57),
                BackColor = Color.Transparent
            };
            Label ayuda = new Label
            {
                Text = "Ingresa tus datos para continuar.",
                Location = new Point(49, 90),
                Size = new Size(394, 24),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(119, 139, 141),
                BackColor = Color.Transparent
            };

            btnIngresar.Text = "Iniciar sesión  →";
            btnIngresar.IconChar = FontAwesome.Sharp.IconChar.None;
            btnIngresar.TextImageRelation = TextImageRelation.Overlay;
            btnIngresar.SetBounds(48, 348, 394, 54);
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatAppearance.MouseOverBackColor = Color.FromArgb(18, 119, 94);
            btnIngresar.BackColor = Color.FromArgb(22, 139, 111);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.TabIndex = 2;

            Panel divisor = new Panel { Location = new Point(48, 438), Size = new Size(394, 1), BackColor = Color.FromArgb(225, 235, 232) };
            Label accesoAutorizado = new Label
            {
                Text = "Acceso exclusivo para personal autorizado",
                Location = new Point(48, 463),
                Size = new Size(394, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(132, 151, 151),
                Font = new Font("Segoe UI", 8.5F),
                BackColor = Color.Transparent
            };
            Label sistemaSeguro = new Label
            {
                Text = "●  Sistema seguro",
                Location = new Point(150, 510),
                Size = new Size(190, 32),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(18, 126, 96),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 247, 240)
            };
            RedondearPanel(sistemaSeguro, 16);

            btnSalir.Text = "Salir de la aplicación";
            btnSalir.IconChar = FontAwesome.Sharp.IconChar.None;
            btnSalir.TextImageRelation = TextImageRelation.Overlay;
            btnSalir.SetBounds(48, 558, 394, 30);
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.BackColor = Color.White;
            btnSalir.ForeColor = Color.FromArgb(103, 124, 124);
            btnSalir.Font = new Font("Segoe UI", 8.5F);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.TabIndex = 3;

            tarjetaAcceso.Controls.Add(tituloAcceso);
            tarjetaAcceso.Controls.Add(ayuda);
            tarjetaAcceso.Controls.Add(label1);
            tarjetaAcceso.Controls.Add(txtUsuario);
            tarjetaAcceso.Controls.Add(label2);
            tarjetaAcceso.Controls.Add(txtContrasena);
            tarjetaAcceso.Controls.Add(btnIngresar);
            tarjetaAcceso.Controls.Add(divisor);
            tarjetaAcceso.Controls.Add(accesoAutorizado);
            tarjetaAcceso.Controls.Add(sistemaSeguro);
            tarjetaAcceso.Controls.Add(btnSalir);

            layout.Controls.Add(bienvenida, 0, 0);
            layout.Controls.Add(zonaAcceso, 1, 0);
            Controls.Add(layout);
            AcceptButton = btnIngresar;
        }

        private static void RedondearPanel(Control control, int radio)
        {
            EventHandler ajustar = (s, e) =>
            {
                if (control.Width <= 0 || control.Height <= 0) return;
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
            control.Resize += ajustar;
            ajustar(control, EventArgs.Empty);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string mensaje;
            Usuario usuario = _negocio.Login(txtUsuario.Text, txtContrasena.Text, out mensaje);

            if (usuario == null)
            {
                MessageBox.Show(mensaje, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MenuPrincipalForms menu = new MenuPrincipalForms(usuario);
            menu.FormClosed += (s, args) => Application.Exit();
            menu.Show();
            this.Hide();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForms_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
