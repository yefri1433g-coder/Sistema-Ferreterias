using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Presentacion
{
    // Contenedor con barras delgadas: no depende del color de las barras de Windows.
    public class PanelDesplazable : Panel
    {
        private readonly Panel superficie = new Panel();
        private readonly BarraDelgada vertical = new BarraDelgada();
        private readonly BarraDelgada horizontal = new BarraDelgada { Horizontal = true };
        private Size tamanoContenido;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Superficie { get { return superficie; } }

        [Browsable(false)]
        public Size AreaVisible { get; private set; }

        [Browsable(false)]
        public Size TamanoContenido
        {
            get { return tamanoContenido; }
            set { tamanoContenido = value; ActualizarBarras(); }
        }

        [Browsable(false)]
        public Point Desplazamiento
        {
            get { return new Point(horizontal.Valor, vertical.Valor); }
            set { horizontal.Valor = value.X; vertical.Valor = value.Y; }
        }

        public PanelDesplazable()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            superficie.BackColor = BackColor;
            Controls.Add(superficie);
            Controls.Add(vertical);
            Controls.Add(horizontal);
            vertical.BringToFront(); horizontal.BringToFront();
            vertical.Cambio += (s, e) => MoverSuperficie();
            horizontal.Cambio += (s, e) => MoverSuperficie();
            ActualizarColores();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (superficie != null && vertical != null && horizontal != null) ActualizarColores();
        }

        private void ActualizarColores()
        {
            superficie.BackColor = BackColor;
            vertical.BackColor = BackColor;
            horizontal.BackColor = BackColor;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (vertical != null) ActualizarBarras();
        }

        private void ActualizarBarras()
        {
            const int ancho = 12;
            bool mostrarVertical = tamanoContenido.Height > ClientSize.Height;
            bool mostrarHorizontal = tamanoContenido.Width > ClientSize.Width - (mostrarVertical ? ancho : 0);
            mostrarVertical = tamanoContenido.Height > ClientSize.Height - (mostrarHorizontal ? ancho : 0);
            mostrarHorizontal = tamanoContenido.Width > ClientSize.Width - (mostrarVertical ? ancho : 0);
            AreaVisible = new Size(Math.Max(0, ClientSize.Width - (mostrarVertical ? ancho : 0)),
                Math.Max(0, ClientSize.Height - (mostrarHorizontal ? ancho : 0)));
            vertical.Visible = mostrarVertical;
            horizontal.Visible = mostrarHorizontal;
            vertical.Bounds = new Rectangle(AreaVisible.Width, 0, ancho, AreaVisible.Height);
            horizontal.Bounds = new Rectangle(0, AreaVisible.Height, AreaVisible.Width, ancho);
            vertical.Configurar(tamanoContenido.Height, AreaVisible.Height);
            horizontal.Configurar(tamanoContenido.Width, AreaVisible.Width);
            superficie.Size = new Size(Math.Max(tamanoContenido.Width, AreaVisible.Width),
                Math.Max(tamanoContenido.Height, AreaVisible.Height));
            MoverSuperficie();
        }

        private void MoverSuperficie()
        {
            superficie.Location = new Point(-horizontal.Valor, -vertical.Valor);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int paso = (int)(e.Delta / 120.0 * 48);
            if (vertical.Visible && (ModifierKeys & Keys.Shift) == 0) vertical.Valor -= paso;
            else horizontal.Valor -= paso;
            HandledMouseEventArgs manejado = e as HandledMouseEventArgs;
            if (manejado != null) manejado.Handled = true;
        }

        public void DesplazarHasta(Control control)
        {
            Point posicion = superficie.PointToClient(control.PointToScreen(Point.Empty));
            int x = horizontal.Valor, y = vertical.Valor;
            if (posicion.X < x) x = posicion.X;
            else if (posicion.X + control.Width > x + AreaVisible.Width) x = posicion.X + control.Width - AreaVisible.Width;
            if (posicion.Y < y) y = posicion.Y;
            else if (posicion.Y + control.Height > y + AreaVisible.Height) y = posicion.Y + control.Height - AreaVisible.Height;
            Desplazamiento = new Point(x, y);
        }
    }

    internal class BarraDelgada : Control
    {
        internal bool Horizontal { get; set; }
        private int total, visible, valor, inicioRaton, inicioValor;
        private bool arrastrando, encima;
        internal event EventHandler Cambio;
        internal int Valor
        {
            get { return valor; }
            set
            {
                int nuevo = Math.Max(0, Math.Min(Math.Max(0, total - visible), value));
                if (nuevo == valor) return;
                valor = nuevo; Invalidate();
                if (Cambio != null) Cambio(this, EventArgs.Empty);
            }
        }

        internal BarraDelgada()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            BackColor = Color.FromArgb(15, 35, 65);
            Cursor = Cursors.Hand;
            TabStop = true;
            AccessibleName = "Desplazamiento";
            AccessibleRole = AccessibleRole.ScrollBar;
        }

        internal void Configurar(int longitudTotal, int longitudVisible)
        {
            total = longitudTotal; visible = longitudVisible;
            Valor = valor; Invalidate();
        }

        private Rectangle Indicador()
        {
            int largo = Horizontal ? Width : Height;
            int tamano = Math.Min(largo, Math.Max(28, total == 0 ? largo : largo * visible / total));
            int inicio = total <= visible ? 0 : (int)((long)valor * (largo - tamano) / (total - visible));
            return Horizontal ? new Rectangle(inicio, 3, tamano, 6) : new Rectangle(3, inicio, 6, tamano);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle r = Indicador();
            if (r.Width < 6 || r.Height < 6) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath camino = new GraphicsPath())
            using (Brush pincel = new SolidBrush(encima || arrastrando ? Color.FromArgb(143, 175, 218) : Color.FromArgb(77, 109, 154)))
            {
                camino.AddArc(r.Left, r.Top, 6, 6, 180, 90);
                camino.AddArc(r.Right - 6, r.Top, 6, 6, 270, 90);
                camino.AddArc(r.Right - 6, r.Bottom - 6, 6, 6, 0, 90);
                camino.AddArc(r.Left, r.Bottom - 6, 6, 6, 90, 90);
                camino.CloseFigure(); e.Graphics.FillPath(pincel, camino);
            }
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); encima = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); encima = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            Focus();
            if (!Indicador().Contains(e.Location)) Valor += (Horizontal ? e.X < Indicador().X : e.Y < Indicador().Y) ? -visible : visible;
            inicioRaton = Horizontal ? e.X : e.Y; inicioValor = valor;
            arrastrando = true; Capture = true; Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!arrastrando) return;
            Rectangle indicador = Indicador();
            int recorrido = Horizontal ? Width - indicador.Width : Height - indicador.Height;
            if (recorrido > 0) Valor = inicioValor + (int)((long)((Horizontal ? e.X : e.Y) - inicioRaton) * Math.Max(0, total - visible) / recorrido);
        }
        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); arrastrando = false; Capture = false; Invalidate(); }
        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
            if (!Capture) { arrastrando = false; Invalidate(); }
        }
        protected override bool IsInputKey(Keys keyData)
        {
            Keys tecla = keyData & Keys.KeyCode;
            return tecla == Keys.Up || tecla == Keys.Down || tecla == Keys.Left || tecla == Keys.Right ||
                tecla == Keys.Home || tecla == Keys.End || tecla == Keys.PageUp || tecla == Keys.PageDown || base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up: case Keys.Left: Valor -= 48; break;
                case Keys.Down: case Keys.Right: Valor += 48; break;
                case Keys.PageUp: Valor -= visible; break;
                case Keys.PageDown: Valor += visible; break;
                case Keys.Home: Valor = 0; break;
                case Keys.End: Valor = total; break;
                default: return;
            }
            e.Handled = true;
        }
    }
}
