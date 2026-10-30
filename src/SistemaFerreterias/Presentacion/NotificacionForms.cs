using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // Aviso temporal de éxito: no bloquea el formulario ni toma el foco del teclado.
    internal sealed class NotificacionForms : Form
    {
        private static NotificacionForms actual;
        private readonly Timer temporizador = new Timer { Interval = 5000 };

        internal static void Mostrar(string mensaje)
        {
            Form formularioConFoco = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.ContainsFocus);
            Control campoConFoco = formularioConFoco;
            while (campoConFoco is ContainerControl && ((ContainerControl)campoConFoco).ActiveControl != null)
                campoConFoco = ((ContainerControl)campoConFoco).ActiveControl;
            if (actual != null && !actual.IsDisposed) actual.Close();
            Form ventana = Application.OpenForms.Cast<Form>().FirstOrDefault(f =>
                f.Visible && f.TopLevel && !f.Modal && !(f is NotificacionForms));
            actual = new NotificacionForms(mensaje, ventana);
            if (ventana == null) actual.Show();
            else actual.Show(ventana);
            if (campoConFoco != null && !campoConFoco.IsDisposed) campoConFoco.Focus();
        }

        private NotificacionForms(string mensaje, Form ventana)
        {
            Name = "notificacionExito";
            Text = "Operación completada";
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);
            Rectangle area = ventana == null ? Screen.PrimaryScreen.WorkingArea : Screen.FromControl(ventana).WorkingArea;
            int ancho = Math.Min(390, area.Width - 32);
            Size texto = TextRenderer.MeasureText(mensaje, Font, new Size(ancho - 92, 0), TextFormatFlags.WordBreak);
            ClientSize = new Size(ancho, Math.Max(90, texto.Height + 58));
            Location = new Point(area.Right - Width - 20, area.Bottom - Height - 20);

            Controls.Add(new Panel { Dock = DockStyle.Left, Width = 5, BackColor = Color.FromArgb(34, 139, 89) });
            Controls.Add(new Label { Text = "✓", Bounds = new Rectangle(16, 18, 30, 32),
                Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = Color.FromArgb(34, 139, 89) });
            Controls.Add(new Label { Text = "Listo", Bounds = new Rectangle(54, 14, ancho - 92, 24),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta });
            Controls.Add(new Label { Name = "mensajeNotificacion", Text = mensaje,
                Bounds = new Rectangle(54, 42, ancho - 92, texto.Height + 4), ForeColor = EstiloAplicacion.Tinta });
            Button cerrar = new Button { Name = "cerrarNotificacion", Text = "×", Bounds = new Rectangle(ancho - 34, 8, 28, 28),
                FlatStyle = FlatStyle.Flat, TabStop = false, Cursor = Cursors.Hand, ForeColor = EstiloAplicacion.Secundario };
            cerrar.FlatAppearance.BorderSize = 0;
            cerrar.Click += (s, e) => Close();
            Controls.Add(cerrar);
            temporizador.Tick += (s, e) => Close();
            Shown += (s, e) => temporizador.Start();
        }

        protected override bool ShowWithoutActivation { get { return true; } }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
                return parametros;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) temporizador.Dispose();
            base.Dispose(disposing);
        }
    }
}
