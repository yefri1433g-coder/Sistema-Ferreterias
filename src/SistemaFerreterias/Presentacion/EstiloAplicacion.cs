using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Linq;

namespace Presentacion
{
    /// <summary>Estilo compartido para mantener una apariencia coherente y permitir seguir editando cada Form desde el diseñador.</summary>
    // CLASE EstiloAplicacion: Aplica estilos visuales compartidos a formularios y controles.
    internal static class EstiloAplicacion
    {
        internal static readonly Color Fondo = Color.White;
        internal static readonly Color Marino = Color.FromArgb(15, 35, 65);
        internal static readonly Color Superficie = Color.White;
        internal static readonly Color Tinta = Color.FromArgb(23, 42, 74);
        internal static readonly Color Secundario = Color.FromArgb(80, 98, 124);
        internal static readonly Color Acento = Color.FromArgb(25, 55, 98);
        internal static readonly Color Seleccion = Color.FromArgb(225, 234, 247);
        internal static readonly Color FondoSuave = Color.FromArgb(237, 242, 250);

        // CONFIGURACIÓN: Aplicar
        internal static void Aplicar(Form formulario)
        {
            formulario.Shown -= AplicarTemaAlMostrar;
            formulario.Shown += AplicarTemaAlMostrar;
            formulario.BackColor = Fondo;
            formulario.ForeColor = Tinta;
            formulario.Font = new Font("Segoe UI", 9F);
            formulario.MinimumSize = new Size(980, 620);
            AplicarTablas(formulario.Controls);
            AplicarPaleta(formulario);
            if (formulario is PantallaGestionBase || formulario is DashboardForms || formulario is ProveedoresForms || formulario is InventariosForms || formulario is KardexForms || formulario is StockForms || formulario is TrasladosForms || formulario is ClientesForms || formulario is EmpleadosForms || formulario is VentasForms || formulario is PagosForms || formulario is UsuariosForms) return;
            AplicarControles(formulario.Controls);
            AplicarTablas(formulario.Controls);
        }

        // Estilo común de los listados y detalles, sin cambiar su edición ni selección.
        internal static void AplicarTablas(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                DataGridView tabla = control as DataGridView;
                if (tabla != null)
                {
                    tabla.BackgroundColor = Color.White;
                    tabla.BorderStyle = BorderStyle.None;
                    tabla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    tabla.GridColor = Color.FromArgb(234, 240, 240);
                    tabla.EnableHeadersVisualStyles = false;
                    tabla.RowHeadersVisible = false;
                    tabla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    tabla.ColumnHeadersHeight = 44;
                    tabla.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
                    tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    tabla.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
                    tabla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                    tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    tabla.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 5, 6, 5);
                    tabla.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
                    tabla.DefaultCellStyle.BackColor = Color.White;
                    tabla.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
                    tabla.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
                    tabla.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
                    tabla.DefaultCellStyle.Padding = new Padding(6, 5, 6, 5);
                    tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    tabla.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    tabla.RowTemplate.MinimumHeight = 42;
                    tabla.RowTemplate.Height = 42;
                    foreach (DataGridViewRow fila in tabla.Rows)
                        fila.MinimumHeight = 42;
                }
                if (control.HasChildren) AplicarTablas(control.Controls);
            }
        }

        private static void AplicarTemaAlMostrar(object sender, EventArgs e)
        {
            Form formulario = (Form)sender;
            AplicarPaleta(formulario);
            AplicarTablas(formulario.Controls);
            AjustarTipografia(formulario);
        }

        // Mantiene contraste en los encabezados marinos y en las tarjetas claras.
        internal static void AplicarPaleta(Control control)
        {
            Control fondo = control;
            while (fondo.Parent != null && fondo.BackColor.A < 255) fondo = fondo.Parent;
            Color color = fondo.BackColor;
            bool oscuro = color.A == 255 && color.R * 0.299 + color.G * 0.587 + color.B * 0.114 < 100;
            if ((control is Label || control is Button || control is CheckBox) && oscuro)
                control.ForeColor = Color.White;
            else if (control is Label && !oscuro && control.ForeColor.GetBrightness() > 0.7F)
                control.ForeColor = Tinta;
            foreach (Control hijo in control.Controls) AplicarPaleta(hijo);
        }

        // Ajusta texto y espacio juntos para evitar recortes con la escala de Windows.
        internal static void AjustarTipografia(Control control)
        {
            if ((control is Label || control is Button || control is CheckBox) && control.Font.SizeInPoints < 9F)
                control.Font = new Font("Segoe UI", 9F, control.Font.Style);
            Label etiqueta = control as Label;
            if (etiqueta != null && !etiqueta.AutoSize && etiqueta.Dock == DockStyle.None)
            {
                etiqueta.Height = Math.Max(etiqueta.Height, etiqueta.Font.Height + 4);
                if (etiqueta.AutoEllipsis && etiqueta.Parent != null && etiqueta.Parent.Name != "cabeceraComunDialogo")
                    etiqueta.Width = Math.Max(etiqueta.Width, Math.Min(etiqueta.Parent.ClientSize.Width - etiqueta.Left - 8,
                        TextRenderer.MeasureText(etiqueta.Text, etiqueta.Font).Width + 6));
            }
            Button boton = control as Button;
            if (boton != null)
            {
                boton.Height = Math.Max(boton.Height, boton.Font.Height + 12);
                FontAwesome.Sharp.IconButton icono = boton as FontAwesome.Sharp.IconButton;
                if (icono != null && icono.IconChar == FontAwesome.Sharp.IconChar.None)
                {
                    boton.TextImageRelation = TextImageRelation.Overlay;
                    boton.Padding = Padding.Empty;
                }
            }
            foreach (Control hijo in control.Controls) AjustarTipografia(hijo);

            Button anterior = control.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Anterior"));
            Button siguiente = control.Controls.OfType<Button>().FirstOrDefault(b => b.Text.Contains("Siguiente"));
            Label pagina = control.Controls.OfType<Label>().FirstOrDefault();
            if (anterior != null && siguiente != null && pagina != null)
            {
                anterior.Text = "< Anterior"; siguiente.Text = "Siguiente >";
                anterior.Size = siguiente.Size = new Size(116, 34);
                pagina.Size = new Size(116, 34);
                pagina.AutoSize = false; pagina.TextAlign = ContentAlignment.MiddleCenter;
                FlowLayoutPanel flujo = control as FlowLayoutPanel;
                if (flujo != null)
                {
                    flujo.FlowDirection = FlowDirection.LeftToRight;
                    flujo.Controls.SetChildIndex(anterior, 0);
                    flujo.Controls.SetChildIndex(pagina, 1);
                    flujo.Controls.SetChildIndex(siguiente, 2);
                    anterior.Margin = pagina.Margin = siguiente.Margin = new Padding(2, 0, 2, 0);
                }
                else
                {
                    anterior.Location = new Point(0, 3);
                    pagina.Location = new Point(120, 3);
                    siguiente.Location = new Point(240, 3);
                }
                control.Width = Math.Max(control.Width, 364 + control.Padding.Horizontal);
                control.Height = Math.Max(control.Height, 42);
                if (control.Parent != null)
                {
                    control.Parent.Height = Math.Max(control.Parent.Height, 48);
                    TableLayoutPanel distribucion = control.Parent.Parent as TableLayoutPanel;
                    if (distribucion != null)
                    {
                        int filaPie = distribucion.GetRow(control.Parent);
                        if (filaPie >= 0 && filaPie < distribucion.RowStyles.Count)
                            distribucion.RowStyles[filaPie].Height = Math.Max(distribucion.RowStyles[filaPie].Height, 48);
                    }
                }
            }

            // Los encabezados con ruta, título y descripción se distribuyen por su alto real.
            Label[] titulos = control.Controls.OfType<Label>().OrderBy(l => l.Top).ToArray();
            if (control is Panel && !(control is FlowLayoutPanel) && !(control is TableLayoutPanel) &&
                titulos.Length == 3 && titulos.Any(l => l.Font.SizeInPoints >= 18F) && titulos.All(l => l.AutoSize))
            {
                int y = Math.Max(8, control.Padding.Top);
                foreach (Label titulo in titulos)
                {
                    titulo.Top = y;
                    y += titulo.PreferredHeight + 6;
                }
                TableLayoutPanel tabla = control.Parent as TableLayoutPanel;
                if (tabla != null)
                {
                    int fila = tabla.GetRow(control);
                    if (fila >= 0 && fila < tabla.RowStyles.Count)
                        tabla.RowStyles[fila].Height = Math.Max(tabla.RowStyles[fila].Height, y + 10);
                }
                else control.Height = Math.Max(control.Height, y + 10);
            }
        }

        // OBTENER RECURSO VISUAL: Carga la imagen compartida del módulo de clientes.
        internal static Image ObtenerImagenClientes()
        {
            ComponentResourceManager recursos = new ComponentResourceManager(typeof(ClientesForms));
            return recursos.GetObject("pictureBox1.Image") as Image;
        }

        // CONFIGURACIÓN: AplicarDialogo
        internal static void AplicarDialogo(Form dialogo)
        {
            if (dialogo == null) return;
            dialogo.StartPosition = FormStartPosition.CenterParent;
            dialogo.FormBorderStyle = FormBorderStyle.None;
            dialogo.MaximizeBox = false;
            dialogo.MinimizeBox = false;
            dialogo.ShowInTaskbar = false;
            dialogo.BackColor = Color.White;
            dialogo.ForeColor = Tinta;
            dialogo.Font = new Font("Segoe UI", 9F);
            if (dialogo.Controls["cabeceraComunDialogo"] == null)
            {
                Control[] contenidoOriginal = new Control[dialogo.Controls.Count];
                dialogo.Controls.CopyTo(contenidoOriginal, 0);
                int altoOriginal = dialogo.ClientSize.Height;
                int anchoOriginal = dialogo.ClientSize.Width;
                const int altoCabecera = 80;
                dialogo.Controls.Clear();
                dialogo.ClientSize = new Size(anchoOriginal, altoOriginal + altoCabecera);
                Panel contenido = new Panel { Name = "contenidoComunDialogo", Bounds = new Rectangle(0, altoCabecera, anchoOriginal, altoOriginal), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White };
                foreach (Control control in contenidoOriginal) contenido.Controls.Add(control);
                dialogo.Controls.Add(contenido);

                Panel cabecera = new Panel { Name = "cabeceraComunDialogo", Bounds = new Rectangle(0, 0, anchoOriginal, altoCabecera), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = EstiloAplicacion.Marino, Padding = new Padding(20, 6, 12, 4) };
                Label titulo = new Label { Name = "tituloComunDialogo", Text = dialogo.Text, Bounds = new Rectangle(24, 12, anchoOriginal - 210, 28), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true };
                Label subtitulo = new Label { Text = "Completa los campos para continuar.", Bounds = new Rectangle(24, 43, anchoOriginal - 210, 24), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(192, 207, 211), TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true };
                Button cerrar = new Button { Name = "btnCerrarComunDialogo", Text = "×", Bounds = new Rectangle(anchoOriginal - 52, 22, 36, 36), Anchor = AnchorStyles.Top | AnchorStyles.Right, FlatStyle = FlatStyle.Flat, BackColor = EstiloAplicacion.Marino, ForeColor = Color.White, Font = new Font("Segoe UI", 16F), Cursor = Cursors.Hand, TabStop = false };
                Button cancelar = new Button { Name = "btnCancelarComunDialogo", Text = "Cancelar", Width = 86, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Right, FlatStyle = FlatStyle.Flat, BackColor = EstiloAplicacion.Marino, ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F), Cursor = Cursors.Hand, TabStop = false };
                cancelar.Location = new Point(anchoOriginal - 154, 24);
                cerrar.FlatAppearance.BorderSize = 0;
                cancelar.FlatAppearance.BorderColor = Color.FromArgb(90, 112, 123);
                cerrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
                Action cancelarEdicion = () =>
                {
                    Button botonCancelarExistente = BuscarBotonPorTexto(dialogo.Controls, "cancel") ?? BuscarBotonPorTexto(dialogo.Controls, "cerrar");
                    if (botonCancelarExistente != null)
                    {
                        bool estabaVisible = botonCancelarExistente.Visible;
                        if (!estabaVisible) botonCancelarExistente.Visible = true;
                        botonCancelarExistente.PerformClick();
                        if (dialogo.Visible) botonCancelarExistente.Visible = estabaVisible;
                    }
                    if (dialogo.Visible) dialogo.DialogResult = DialogResult.Cancel;
                };
                cerrar.Click += (s, e) => cancelarEdicion();
                cancelar.Click += (s, e) => cancelarEdicion();
                cabecera.Controls.Add(cerrar);
                cabecera.Controls.Add(cancelar);
                cabecera.Controls.Add(subtitulo);
                cabecera.Controls.Add(titulo);
                dialogo.Controls.Add(cabecera);
                cabecera.BringToFront();
                OcultarBotonesSalir(contenido.Controls);
                dialogo.CancelButton = cancelar;
                dialogo.VisibleChanged += (s, e) =>
                {
                    if (!dialogo.Visible) return;
                    titulo.Text = dialogo.Text;
                    AplicarEstiloDialogo(dialogo.Controls);
                    SepararEtiquetas(dialogo);
                    OcultarBotonesSalir(contenido.Controls);
                };
            }
            Panel encabezadoDialogo = dialogo.Controls["cabeceraComunDialogo"] as Panel;
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["tituloComunDialogo"] != null)
                encabezadoDialogo.Controls["tituloComunDialogo"].Text = dialogo.Text;
            AplicarEstiloDialogo(dialogo.Controls);
            AplicarTablas(dialogo.Controls);
            AplicarPaleta(dialogo);
            AjustarTipografia(dialogo);
            SepararEtiquetas(dialogo);
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["btnCerrarComunDialogo"] is Button botonCerrar)
            {
                botonCerrar.BackColor = EstiloAplicacion.Marino;
                botonCerrar.ForeColor = Color.White;
            }
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["btnCancelarComunDialogo"] is Button botonCancelar)
            {
                botonCancelar.BackColor = EstiloAplicacion.Marino;
                botonCancelar.ForeColor = Color.White;
            }
        }

        // Los formularios pequeños usan etiquetas encima del campo; reservar su alto real.
        private static void SepararEtiquetas(Control contenedor)
        {
            if (!(contenedor is TableLayoutPanel) && !(contenedor is FlowLayoutPanel))
            {
                foreach (Control campo in contenedor.Controls.Cast<Control>().Where(c =>
                    c is TextBox || c is ComboBox || c is NumericUpDown || c is DateTimePicker))
                {
                    Label etiqueta = contenedor.Controls.OfType<Label>()
                        .Where(l => l.Visible && Math.Abs(l.Left - campo.Left) <= 4 && l.Top < campo.Top && campo.Top < l.Bottom + 6)
                        .OrderByDescending(l => l.Top).FirstOrDefault();
                    if (etiqueta != null) campo.Top = etiqueta.Bottom + 6;
                }
            }
            foreach (Control hijo in contenedor.Controls) SepararEtiquetas(hijo);
        }

        // BUSCAR: BuscarBotonPorTexto
        private static Button BuscarBotonPorTexto(Control.ControlCollection controles, string texto)
        {
            foreach (Control control in controles)
            {
                Button boton = control as Button;
                if (boton != null && boton.Name != "btnCancelarComunDialogo" && boton.Name != "btnCerrarComunDialogo" &&
                    (boton.Name.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 || boton.Text.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0))
                    return boton;
                if (control.HasChildren)
                {
                    Button encontrado = BuscarBotonPorTexto(control.Controls, texto);
                    if (encontrado != null) return encontrado;
                }
            }
            return null;
        }

        private static void OcultarBotonesSalir(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                Button boton = control as Button;
                if (boton != null && boton.Name != "btnCancelarComunDialogo" && boton.Name != "btnCerrarComunDialogo" &&
                    (boton.Name.IndexOf("cancel", StringComparison.OrdinalIgnoreCase) >= 0 || boton.Name.IndexOf("cerrar", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     boton.Text.IndexOf("cancel", StringComparison.OrdinalIgnoreCase) >= 0 || String.Equals(boton.Text.Trim(), "Cerrar", StringComparison.OrdinalIgnoreCase)))
                    boton.Visible = false;
                if (control.HasChildren) OcultarBotonesSalir(control.Controls);
            }
        }

        // CONFIGURACIÓN: AplicarEstiloDialogo
        private static void AplicarEstiloDialogo(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                bool enCabecera = control.Parent != null && control.Parent.Name == "cabeceraComunDialogo";
                control.Font = new Font("Segoe UI", enCabecera ? control.Font.SizeInPoints : 9F,
                    control is Button || control is GroupBox ? FontStyle.Bold : enCabecera ? control.Font.Style : FontStyle.Regular);
                Label etiqueta = control as Label;
                bool etiquetaEnCabecera = control.Parent != null && control.Parent.Name == "cabeceraComunDialogo";
                if (etiqueta != null && etiqueta.Font.Size < 14F && !etiquetaEnCabecera)
                {
                    etiqueta.ForeColor = Secundario;
                    if (!etiqueta.AutoSize && etiqueta.Dock == DockStyle.None && etiqueta.Height < etiqueta.Font.Height + 4)
                        etiqueta.Height = etiqueta.Font.Height + 4;
                }

                TextBox texto = control as TextBox;
                if (texto != null) { texto.BorderStyle = BorderStyle.FixedSingle; texto.BackColor = Color.White; texto.ForeColor = Tinta; }
                ComboBox combo = control as ComboBox;
                if (combo != null) { combo.BackColor = Color.White; combo.ForeColor = Tinta; combo.FlatStyle = FlatStyle.Standard; }
                NumericUpDown numero = control as NumericUpDown;
                if (numero != null) { numero.BorderStyle = BorderStyle.FixedSingle; numero.BackColor = Color.White; numero.ForeColor = Tinta; }
                FontAwesome.Sharp.IconButton icono = control as FontAwesome.Sharp.IconButton;
                if (icono != null) { icono.IconChar = FontAwesome.Sharp.IconChar.None; icono.TextImageRelation = TextImageRelation.Overlay; icono.Padding = Padding.Empty; }

                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.ForeColor = Tinta;
                    grid.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
                    grid.DefaultCellStyle.SelectionForeColor = Tinta;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = EstiloAplicacion.FondoSuave;
                    grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    grid.ColumnHeadersHeight = Math.Max(grid.ColumnHeadersHeight, 34);
                    grid.RowTemplate.Height = Math.Max(grid.RowTemplate.Height, 30);
                }

                Button boton = control as Button;
                if (boton != null)
                {
                    boton.FlatStyle = FlatStyle.Flat;
                    boton.UseVisualStyleBackColor = false;
                    boton.Cursor = Cursors.Hand;
                    boton.Height = Math.Max(boton.Height, 34);
                    boton.FlatAppearance.BorderColor = Color.FromArgb(218, 228, 230);
                    string textoBoton = (boton.Name + " " + boton.Text).ToLowerInvariant();
                    if (enCabecera)
                    {
                        boton.BackColor = Marino; boton.ForeColor = Color.White;
                    }
                    else if (textoBoton.Contains("eliminar") || textoBoton.Contains("quitar"))
                    {
                        boton.BackColor = Color.FromArgb(254, 242, 242); boton.ForeColor = Color.FromArgb(185, 28, 28); boton.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
                    }
                    else if (textoBoton.Contains("guardar") || textoBoton.Contains("registrar") || textoBoton.Contains("agregar") || textoBoton.Contains("pagar"))
                    {
                        boton.BackColor = EstiloAplicacion.Acento; boton.ForeColor = Color.White; boton.FlatAppearance.BorderColor = EstiloAplicacion.Acento;
                    }
                    else
                    {
                        boton.BackColor = Color.White; boton.ForeColor = Tinta;
                    }
                }

                if (control.HasChildren) AplicarEstiloDialogo(control.Controls);
            }
        }

        // CONFIGURACIÓN: AplicarControles
        private static void AplicarControles(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                Panel panel = control as Panel;
                if (panel != null)
                {
                    panel.BackColor = panel.Name.IndexOf("Principal", System.StringComparison.OrdinalIgnoreCase) >= 0 ? Fondo : Superficie;
                    if (panel.Name.IndexOf("Divisor", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        panel.BackColor = Color.FromArgb(229, 231, 235);
                }

                Label etiqueta = control as Label;
                if (etiqueta != null)
                {
                    etiqueta.ForeColor = Secundario;
                    if (etiqueta.Font.Size >= 14F)
                    {
                        etiqueta.ForeColor = Tinta;
                        etiqueta.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                    }
                    else if (etiqueta.Font.Size >= 11F)
                    {
                        etiqueta.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                    }
                }

                TextBox texto = control as TextBox;
                if (texto != null) { texto.BorderStyle = BorderStyle.FixedSingle; texto.BackColor = Superficie; texto.Font = new Font("Segoe UI", 9.5F); }

                ComboBox combo = control as ComboBox;
                if (combo != null)
                {
                    combo.FlatStyle = FlatStyle.Standard;
                    combo.BackColor = Superficie;
                    combo.ForeColor = Tinta;
                    combo.Font = new Font("Segoe UI", 9.5F);
                }

                DateTimePicker fecha = control as DateTimePicker;
                if (fecha != null) fecha.Font = new Font("Segoe UI", 9.5F);

                NumericUpDown numero = control as NumericUpDown;
                if (numero != null) { numero.Font = new Font("Segoe UI", 9.5F); numero.BorderStyle = BorderStyle.FixedSingle; }

                TabPage pagina = control as TabPage;
                if (pagina != null) pagina.BackColor = Superficie;

                TabControl pestanas = control as TabControl;
                if (pestanas != null) pestanas.BackColor = Superficie;

                GroupBox grupo = control as GroupBox;
                if (grupo != null) { grupo.BackColor = Superficie; grupo.ForeColor = Tinta; grupo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); }

                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
                    grid.DefaultCellStyle.SelectionForeColor = Tinta;
                    grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
                    grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    grid.ColumnHeadersHeight = 38;
                    grid.RowTemplate.Height = 32;
                }

                Button boton = control as Button;
                if (boton != null)
                {
                    boton.FlatStyle = FlatStyle.Flat;
                    boton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                    boton.Cursor = Cursors.Hand;
                    boton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                    boton.UseVisualStyleBackColor = false;
                    string nombre = (boton.Name + " " + boton.Text).ToLowerInvariant();
                    if (nombre.Contains("eliminar") || nombre.Contains("quitar"))
                    {
                        boton.BackColor = Color.FromArgb(254, 242, 242);
                        boton.ForeColor = Color.FromArgb(185, 28, 28);
                        boton.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
                    }
                    else if (nombre.Contains("guardar") || nombre.Contains("buscar") || nombre.Contains("nuevo") || nombre.Contains("agregar") || nombre.Contains("actualizar"))
                    {
                        boton.BackColor = Acento;
                        boton.ForeColor = Color.White;
                        boton.FlatAppearance.BorderColor = Acento;
                    }
                    else if (nombre.Contains("limpiar"))
                    {
                        boton.BackColor = Color.FromArgb(239, 246, 255);
                        boton.ForeColor = Color.FromArgb(29, 78, 216);
                        boton.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
                    }
                    else
                    {
                        boton.BackColor = Superficie;
                        boton.ForeColor = Tinta;
                    }
                    boton.FlatAppearance.MouseOverBackColor = boton.ForeColor == Color.White ? Color.FromArgb(29, 78, 216) : Color.FromArgb(243, 244, 246);

                    FontAwesome.Sharp.IconButton botonConIcono = control as FontAwesome.Sharp.IconButton;
                    if (botonConIcono != null)
                        botonConIcono.IconColor = boton.ForeColor;
                }
                if (control.HasChildren) AplicarControles(control.Controls);
            }
        }
    }
}

