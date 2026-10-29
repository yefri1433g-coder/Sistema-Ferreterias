using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE PanelGrafico: Dibuja los gráficos que se muestran en el Dashboard.
    public class PanelGrafico : Panel
    {
        // ENUM TipoVisualizacion: Presenta información y conecta acciones de usuario con la capa Negocio.
        public enum TipoVisualizacion { Barras, Linea, Anillo }

        private string _titulo = string.Empty;
        private Color _colorBarra = Color.SteelBlue;
        private List<KeyValuePair<string, int>> _valores = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, decimal>> _importes = new List<KeyValuePair<string, decimal>>();
        public string TextoResumen { get; set; }
        private TipoVisualizacion _tipo;

        public TipoVisualizacion Tipo
        {
            get { return _tipo; }
            set { _tipo = value; Invalidate(); }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; Invalidate(); }
        }

        public Color ColorBarra
        {
            get { return _colorBarra; }
            set { _colorBarra = value; Invalidate(); }
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<KeyValuePair<string, int>> Valores
        {
            get { return _valores; }
            set { _valores = value ?? new List<KeyValuePair<string, int>>(); Invalidate(); }
        }

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<KeyValuePair<string, decimal>> Importes
        {
            get { return _importes; }
            set { _importes = value ?? new List<KeyValuePair<string, decimal>>(); Invalidate(); }
        }

        public PanelGrafico()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (_valores.Count == 0 && _importes.Count == 0)
            {
                using (Font vacio = new Font("Segoe UI", 9F))
                    g.DrawString("Sin datos disponibles", vacio, Brushes.Gray, 12, 16);
                return;
            }

            if (_tipo == TipoVisualizacion.Anillo) DibujarAnillo(g);
            else if (_tipo == TipoVisualizacion.Linea && _importes.Count > 0) DibujarLineaVentas(g);
            else if (_tipo == TipoVisualizacion.Linea) DibujarLinea(g);
            else DibujarBarras(g);
        }

        private void DibujarLineaVentas(Graphics g)
        {
            int izquierda = 28, derecha = Width - 8, arriba = 40, abajo = Height - 30;
            int ancho = Math.Max(1, derecha - izquierda), alto = Math.Max(1, abajo - arriba);
            decimal maximo = Math.Max(1m, _importes.Max(v => v.Value));
            using (Font texto = new Font("Segoe UI", 7F))
            using (Font resumen = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (Pen rejilla = new Pen(Color.FromArgb(234, 241, 240)))
            using (Pen linea = new Pen(Color.FromArgb(0, 174, 126), 2.2F))
            using (SolidBrush relleno = new SolidBrush(Color.FromArgb(34, 35, 194, 145)))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center })
            {
                if (!string.IsNullOrEmpty(TextoResumen))
                    using (SolidBrush textoResumen = new SolidBrush(Color.FromArgb(18, 42, 53)))
                        g.DrawString(TextoResumen, resumen, textoResumen, 3, 1);
                for (int r = 0; r < 4; r++)
                {
                    float y = arriba + alto * r / 3f;
                    g.DrawLine(rejilla, izquierda, y, derecha, y);
                    decimal valorEje = maximo * (3 - r) / 3m;
                    g.DrawString(valorEje >= 1000 ? (valorEje / 1000m).ToString("0.#") + "k" : valorEje.ToString("0"), texto,
                        Brushes.SlateGray, new RectangleF(0, y - 6, izquierda - 4, 12), new StringFormat { Alignment = StringAlignment.Far });
                }
                PointF[] puntos = new PointF[_importes.Count];
                for (int i = 0; i < puntos.Length; i++)
                    puntos[i] = new PointF(izquierda + (puntos.Length == 1 ? ancho / 2f : ancho * i / (puntos.Length - 1f)),
                        abajo - (float)(_importes[i].Value / maximo) * (alto - 5));
                if (puntos.Length > 1)
                {
                    using (GraphicsPath area = new GraphicsPath())
                    {
                        area.AddCurve(puntos, 0.35F);
                        area.AddLine(puntos[puntos.Length - 1].X, puntos[puntos.Length - 1].Y, puntos[puntos.Length - 1].X, abajo);
                        area.AddLine(puntos[puntos.Length - 1].X, abajo, puntos[0].X, abajo);
                        area.CloseFigure();
                        g.FillPath(relleno, area);
                    }
                    g.DrawCurve(linea, puntos, 0.35F);
                }
                for (int i = 0; i < puntos.Length; i++)
                {
                    g.FillEllipse(Brushes.White, puntos[i].X - 3, puntos[i].Y - 3, 6, 6);
                    g.DrawEllipse(linea, puntos[i].X - 3, puntos[i].Y - 3, 6, 6);
                    g.DrawString(_importes[i].Key, texto, Brushes.SlateGray,
                        new RectangleF(izquierda + ancho * i / puntos.Length, abajo + 5, ancho / (float)puntos.Length, 18), centro);
                }
            }
        }

        private void DibujarLinea(Graphics g)
        {
            int margenIzq = 10;
            int margenDer = 10;
            int arriba = 16;
            int abajo = Math.Max(arriba + 30, Height - 37);
            int ancho = Math.Max(1, Width - margenIzq - margenDer);
            int alto = Math.Max(1, abajo - arriba);
            int maximo = Math.Max(1, _valores.Max(v => v.Value));
            using (Font etiqueta = new Font("Segoe UI", 7.5F))
            using (Pen rejilla = new Pen(Color.FromArgb(232, 240, 239)))
            using (Pen linea = new Pen(_colorBarra, 2.5F))
            using (SolidBrush relleno = new SolidBrush(Color.FromArgb(35, _colorBarra)))
            using (SolidBrush punto = new SolidBrush(_colorBarra))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center })
            {
                for (int r = 0; r < 4; r++)
                {
                    float y = arriba + (alto * r / 3f);
                    g.DrawLine(rejilla, margenIzq, y, margenIzq + ancho, y);
                }
                PointF[] puntos = new PointF[_valores.Count];
                for (int i = 0; i < puntos.Length; i++)
                {
                    float x = margenIzq + (puntos.Length == 1 ? ancho / 2f : ancho * i / (puntos.Length - 1f));
                    float y = abajo - (float)Math.Max(0, _valores[i].Value) / maximo * (alto - 8);
                    puntos[i] = new PointF(x, y);
                }
                if (puntos.Length > 1)
                {
                    PointF[] area = new PointF[puntos.Length + 2];
                    Array.Copy(puntos, area, puntos.Length);
                    area[area.Length - 2] = new PointF(puntos[puntos.Length - 1].X, abajo);
                    area[area.Length - 1] = new PointF(puntos[0].X, abajo);
                    g.FillPolygon(relleno, area);
                    g.DrawLines(linea, puntos);
                }
                for (int i = 0; i < puntos.Length; i++)
                {
                    g.FillEllipse(Brushes.White, puntos[i].X - 3.5F, puntos[i].Y - 3.5F, 7, 7);
                    g.DrawEllipse(linea, puntos[i].X - 3.5F, puntos[i].Y - 3.5F, 7, 7);
                    RectangleF areaEtiqueta = new RectangleF(margenIzq + ancho * i / puntos.Length, abajo + 4, ancho / (float)puntos.Length, 28);
                    g.DrawString(_valores[i].Key, etiqueta, Brushes.SlateGray, areaEtiqueta, centro);
                }
            }
        }

        private void DibujarAnillo(Graphics g)
        {
            int total = _valores.Sum(v => Math.Max(0, v.Value));
            if (total <= 0)
            {
                using (Font f = new Font("Segoe UI", 9F)) g.DrawString("Sin movimientos", f, Brushes.Gray, 12, 16);
                return;
            }
            int diametro = Math.Min(220, Math.Min(Width - 32, Height - 105));
            diametro = Math.Max(35, diametro);
            Rectangle anillo = new Rectangle((Width - diametro) / 2, 8, diametro, diametro);
            Color[] colores = { Color.FromArgb(36, 190, 140), Color.FromArgb(248, 177, 70), Color.FromArgb(98, 132, 235), Color.FromArgb(163, 124, 222) };
            float inicio = -90;
            for (int i = 0; i < _valores.Count; i++)
            {
                float barrido = 360f * Math.Max(0, _valores[i].Value) / total;
                using (Pen segmento = new Pen(colores[i % colores.Length], 14F))
                {
                    segmento.StartCap = LineCap.Round;
                    segmento.EndCap = LineCap.Round;
                    g.DrawArc(segmento, anillo, inicio, Math.Max(0.5F, barrido - 1));
                }
                inicio += barrido;
            }
            using (Font numero = new Font("Segoe UI", 15F, FontStyle.Bold))
            using (Font texto = new Font("Segoe UI", 7F))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                int disponibles = _valores.Where(v => v.Key.Equals("Disponible", StringComparison.OrdinalIgnoreCase)).Sum(v => Math.Max(0, v.Value));
                int porcentaje = (int)Math.Round(100m * disponibles / total);
                Rectangle centroAnillo = new Rectangle(anillo.X, anillo.Y + 3, anillo.Width, anillo.Height / 2);
                g.DrawString(porcentaje + "%", numero, new SolidBrush(Color.FromArgb(24, 45, 54)), centroAnillo, centro);
                Rectangle subtitulo = new Rectangle(anillo.X, anillo.Y + anillo.Height / 2, anillo.Width, 22);
                g.DrawString("disponible", texto, Brushes.SlateGray, subtitulo, centro);
            }
            using (Font leyenda = new Font("Segoe UI", 7F))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center })
            {
                int y = anillo.Bottom + 5;
                int ranura = Math.Max(1, Width / _valores.Count);
                for (int i = 0; i < _valores.Count; i++)
                {
                    int x = i * ranura;
                    using (SolidBrush color = new SolidBrush(colores[i % colores.Length])) g.FillEllipse(color, x + 3, y + 4, 7, 7);
                    g.DrawString(_valores[i].Key + "  " + _valores[i].Value, leyenda, Brushes.SlateGray,
                        new RectangleF(x + 12, y, ranura - 12, 20), centro);
                }
            }
        }

        private void DibujarBarras(Graphics g)
        {
            int max = Math.Max(1, _valores.Max(v => v.Value));
            float lineaBase = Height - 32;
            float altoMax = Math.Max(1, lineaBase - 12);
            float ranura = (float)Width / _valores.Count;
            using (Font texto = new Font("Segoe UI", 8F))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center })
            using (SolidBrush pincel = new SolidBrush(_colorBarra))
            {
                for (int i = 0; i < _valores.Count; i++)
                {
                    float alto = (float)_valores[i].Value / max * altoMax;
                    float x = i * ranura + ranura * .2f;
                    float ancho = ranura * .6f;
                    g.FillRectangle(pincel, x, lineaBase - alto, ancho, alto);
                    g.DrawString(_valores[i].Value.ToString(), texto, Brushes.Black, new RectangleF(i * ranura, lineaBase - alto - 18, ranura, 18), centro);
                    g.DrawString(_valores[i].Key, texto, Brushes.DimGray, new RectangleF(i * ranura, lineaBase + 3, ranura, 28), centro);
                }
                g.DrawLine(Pens.Gainsboro, 5, lineaBase, Width - 5, lineaBase);
            }
        }
    }
}
