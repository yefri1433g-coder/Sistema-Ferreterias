using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System;

namespace Presentacion
{
    /// <summary>Estilo compartido para mantener una apariencia coherente y permitir seguir editando cada Form desde el diseñador.</summary>
    internal static class EstiloAplicacion
    {
        internal static readonly Color Fondo = Color.FromArgb(241, 246, 253);
        internal static readonly Color Superficie = Color.White;
        internal static readonly Color Tinta = Color.FromArgb(30, 41, 59);
        internal static readonly Color Secundario = Color.FromArgb(100, 116, 139);
        internal static readonly Color Acento = Color.FromArgb(37, 99, 235);

        internal static void Aplicar(Form formulario)
        {
            formulario.BackColor = Fondo;
            formulario.ForeColor = Tinta;
            formulario.Font = new Font("Segoe UI", 9F);
            formulario.MinimumSize = new Size(980, 620);
            if (formulario is PantallaGestionBase || formulario is DashboardForms || formulario is ProveedoresForms || formulario is InventariosForms || formulario is KardexForms || formulario is StockForms || formulario is TrasladosForms || formulario is ClientesForms || formulario is EmpleadosForms || formulario is VentasForms || formulario is PagosForms || formulario is UsuariosForms) return;
            AplicarControles(formulario.Controls);
        }

        internal static Image ObtenerImagenClientes()
        {
            ComponentResourceManager recursos = new ComponentResourceManager(typeof(ClientesForms));
            return recursos.GetObject("pictureBox1.Image") as Image;
        }

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
                const int altoCabecera = 62;
                dialogo.Controls.Clear();
                dialogo.ClientSize = new Size(anchoOriginal, altoOriginal + altoCabecera);
                Panel contenido = new Panel { Name = "contenidoComunDialogo", Bounds = new Rectangle(0, altoCabecera, anchoOriginal, altoOriginal), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White };
                foreach (Control control in contenidoOriginal) contenido.Controls.Add(control);
                dialogo.Controls.Add(contenido);

                Panel cabecera = new Panel { Name = "cabeceraComunDialogo", Bounds = new Rectangle(0, 0, anchoOriginal, altoCabecera), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.FromArgb(16, 31, 43), Padding = new Padding(20, 6, 12, 4) };
                Label titulo = new Label { Name = "tituloComunDialogo", Text = dialogo.Text, Dock = DockStyle.Top, Height = 27, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft };
                Label subtitulo = new Label { Text = "Completa los campos para continuar.", Dock = DockStyle.Top, Height = 19, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(192, 207, 211), TextAlign = ContentAlignment.MiddleLeft };
                Button cerrar = new Button { Name = "btnCerrarComunDialogo", Text = "×", Dock = DockStyle.Right, Width = 38, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(16, 31, 43), ForeColor = Color.White, Font = new Font("Segoe UI", 16F), Cursor = Cursors.Hand, TabStop = false };
                Button cancelar = new Button { Name = "btnCancelarComunDialogo", Text = "Cancelar", Width = 86, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Right, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(16, 31, 43), ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F), Cursor = Cursors.Hand, TabStop = false };
                cancelar.Location = new Point(anchoOriginal - 146, 15);
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
            }
            Panel encabezadoDialogo = dialogo.Controls["cabeceraComunDialogo"] as Panel;
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["tituloComunDialogo"] != null)
                encabezadoDialogo.Controls["tituloComunDialogo"].Text = dialogo.Text;
            AplicarEstiloDialogo(dialogo.Controls);
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["btnCerrarComunDialogo"] is Button botonCerrar)
            {
                botonCerrar.BackColor = Color.FromArgb(16, 31, 43);
                botonCerrar.ForeColor = Color.White;
            }
            if (encabezadoDialogo != null && encabezadoDialogo.Controls["btnCancelarComunDialogo"] is Button botonCancelar)
            {
                botonCancelar.BackColor = Color.FromArgb(16, 31, 43);
                botonCancelar.ForeColor = Color.White;
            }
        }

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

        private static void AplicarEstiloDialogo(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                control.Font = new Font("Segoe UI", control.Font.SizeInPoints > 0 ? control.Font.SizeInPoints : 9F, control.Font.Style);
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

                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 246, 245);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Secundario;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.ForeColor = Tinta;
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 240, 235);
                    grid.DefaultCellStyle.SelectionForeColor = Tinta;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 250);
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
                    if (textoBoton.Contains("eliminar") || textoBoton.Contains("quitar"))
                    {
                        boton.BackColor = Color.FromArgb(254, 242, 242); boton.ForeColor = Color.FromArgb(185, 28, 28); boton.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
                    }
                    else if (textoBoton.Contains("guardar") || textoBoton.Contains("registrar") || textoBoton.Contains("agregar") || textoBoton.Contains("pagar"))
                    {
                        boton.BackColor = Color.FromArgb(22, 139, 112); boton.ForeColor = Color.White; boton.FlatAppearance.BorderColor = Color.FromArgb(22, 139, 112);
                    }
                    else
                    {
                        boton.BackColor = Color.White; boton.ForeColor = Tinta;
                    }
                }

                if (control.HasChildren) AplicarEstiloDialogo(control.Controls);
            }
        }

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
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(219, 234, 254);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 64, 175);
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

