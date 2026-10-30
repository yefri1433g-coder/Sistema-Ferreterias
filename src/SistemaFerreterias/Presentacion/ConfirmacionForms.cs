using System;
using System.Drawing;
using System.Windows.Forms;

namespace Presentacion
{
    internal sealed class ConfirmacionForms : Form
    {
        internal ConfirmacionForms(string mensaje)
        {
            bool eliminar = mensaje.IndexOf("eliminar", StringComparison.OrdinalIgnoreCase) >= 0;
            Text = eliminar ? "Eliminar registro" : "Confirmar operación";
            Name = "confirmacionOperacion";
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = EstiloAplicacion.FondoSuave;
            Padding = new Padding(1);
            int altoTexto = TextRenderer.MeasureText(mensaje, Font, new Size(416, 0), TextFormatFlags.WordBreak).Height;
            ClientSize = new Size(480, Math.Max(250, altoTexto + 186));

            Panel cuerpo = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Controls.Add(cuerpo);
            Panel cabecera = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = EstiloAplicacion.Marino };
            cabecera.Controls.Add(new Label { Text = Text, Bounds = new Rectangle(24, 18, 380, 30),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White });
            Button cerrar = new Button { Name = "cerrarConfirmacion", Text = "×", Bounds = new Rectangle(430, 16, 30, 32),
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = EstiloAplicacion.Marino, Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel, TabStop = false };
            cerrar.FlatAppearance.BorderSize = 0;
            cabecera.Controls.Add(cerrar);
            cuerpo.Controls.Add(cabecera);

            cuerpo.Controls.Add(new Label { Name = "mensajeConfirmacion", Text = mensaje,
                Bounds = new Rectangle(26, 90, 416, altoTexto + 4), ForeColor = EstiloAplicacion.Tinta });
            FlowLayoutPanel acciones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 66,
                FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(16, 12, 16, 12), WrapContents = false,
                BackColor = EstiloAplicacion.FondoSuave };
            Button aceptar = new Button { Name = "aceptarConfirmacion", Text = eliminar ? "Eliminar" : "Confirmar",
                Size = new Size(128, 38), FlatStyle = FlatStyle.Flat, BackColor = eliminar ? Color.FromArgb(185, 38, 48) : EstiloAplicacion.Acento,
                ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand,
                DialogResult = DialogResult.Yes, Margin = new Padding(8, 0, 0, 0) };
            aceptar.FlatAppearance.BorderSize = 0;
            Button cancelar = new Button { Name = "cancelarConfirmacion", Text = "Cancelar", Size = new Size(116, 38),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = EstiloAplicacion.Tinta,
                DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand, Margin = Padding.Empty };
            cancelar.FlatAppearance.BorderColor = Color.FromArgb(211, 220, 232);
            acciones.Controls.Add(aceptar);
            acciones.Controls.Add(cancelar);
            cuerpo.Controls.Add(acciones);
            AcceptButton = cancelar;
            CancelButton = cancelar;
        }
    }
}
