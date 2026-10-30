using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion
{
    // Comparte las herramientas del listado; cada formulario conserva sus reglas de negocio.
    internal static class BarraListado
    {
        internal static void Configurar(DataGridView tabla, TextBox buscar, string titulo,
            Action editar = null, Action eliminar = null)
        {
            FlowLayoutPanel barra = buscar.Parent as FlowLayoutPanel;
            if (barra == null) return;
            Control encabezado = barra.Parent;
            Button[] botones = barra.Controls.OfType<Button>().ToArray();
            Button nuevo = botones.FirstOrDefault(b => b.Text.Contains("Nuevo"));
            Button borrar = botones.FirstOrDefault(b => b.Text == "Eliminar");
            Button modificar = botones.FirstOrDefault(b => b.Text == "Editar");
            if (modificar == null) modificar = Boton("Editar", () => EjecutarSeleccion(tabla, editar));
            if (borrar == null) borrar = Boton("Eliminar", () => EjecutarSeleccion(tabla, eliminar));
            if (editar == null && !botones.Contains(modificar)) modificar.Enabled = false;
            if (eliminar == null && !botones.Contains(borrar)) borrar.Enabled = false;

            barra.SuspendLayout();
            barra.Controls.Clear();
            barra.Dock = DockStyle.Bottom;
            barra.Height = 48;
            barra.AutoScroll = true;
            barra.FlowDirection = FlowDirection.LeftToRight;
            barra.WrapContents = false;
            barra.Padding = new Padding(0, 2, 0, 0);
            buscar.Width = 180;
            buscar.Margin = new Padding(0, 1, 8, 0);
            barra.Controls.Add(buscar);
            barra.Controls.Add(Boton("Limpiar", () => { buscar.Clear(); buscar.Focus(); }));
            CheckBox seleccionar = new CheckBox { Text = "Seleccionar", AutoSize = true,
                Font = new Font("Segoe UI", 8F), Margin = new Padding(4, 7, 8, 0) };
            tabla.MultiSelect = true;
            seleccionar.CheckedChanged += (s, e) =>
            {
                if (seleccionar.Checked) tabla.SelectAll();
                else tabla.ClearSelection();
            };
            // Una nueva página o filtro requiere una selección explícita de sus filas.
            tabla.DataBindingComplete += (s, e) => { seleccionar.Checked = false; };
            barra.Controls.Add(seleccionar);
            barra.Controls.Add(Boton("Imprimir", () => AyudaForms.MtdImprimir(tabla, titulo)));
            barra.Controls.Add(Boton("Exportar", () => AyudaForms.MtdExportarCsv(tabla, titulo)));
            barra.Controls.Add(modificar);
            barra.Controls.Add(borrar);
            if (nuevo != null) { nuevo.Text = "+ Nuevo"; barra.Controls.Add(nuevo); }
            foreach (Button boton in barra.Controls.OfType<Button>())
            {
                boton.Size = new Size(boton == nuevo ? 96 : 80, 30);
                boton.Margin = new Padding(0, 0, 8, 0);
                boton.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                boton.FlatStyle = FlatStyle.Flat;
                boton.Padding = Padding.Empty;
                boton.TextAlign = ContentAlignment.MiddleCenter;
                boton.TextImageRelation = TextImageRelation.Overlay;
                FontAwesome.Sharp.IconButton botonIcono = boton as FontAwesome.Sharp.IconButton;
                if (botonIcono != null) botonIcono.IconChar = FontAwesome.Sharp.IconChar.None;
                boton.BackColor = boton == nuevo ? EstiloAplicacion.Acento : Color.White;
                boton.ForeColor = boton == nuevo ? Color.White : boton == borrar
                    ? Color.FromArgb(185, 28, 28) : EstiloAplicacion.Tinta;
                boton.FlatAppearance.BorderColor = Color.FromArgb(218, 228, 230);
            }
            encabezado.Height = 100;
            TableLayoutPanel distribucion = encabezado.Parent as TableLayoutPanel;
            if (distribucion != null)
            {
                int fila = distribucion.GetRow(encabezado);
                if (fila >= 0 && fila < distribucion.RowStyles.Count)
                {
                    distribucion.RowStyles[fila].SizeType = SizeType.Absolute;
                    distribucion.RowStyles[fila].Height = 100;
                }
            }
            barra.ResumeLayout();
        }

        private static Button Boton(string texto, Action accion)
        {
            Button boton = new Button { Text = texto, Cursor = Cursors.Hand };
            boton.Click += (s, e) => accion();
            return boton;
        }

        private static void EjecutarSeleccion(DataGridView tabla, Action accion)
        {
            if (accion == null) return;
            if (tabla.SelectedRows.Count != 1)
            {
                AyudaForms.MtdAviso("Selecciona un solo registro para esta operación.");
                return;
            }
            // La fila seleccionada puede diferir de CurrentRow al seleccionar con el teclado.
            DataGridViewRow fila = tabla.SelectedRows[0];
            DataGridViewCell celda = fila.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
            if (celda == null) return;
            tabla.CurrentCell = celda;
            try { accion(); }
            catch (Exception ex) { AyudaForms.MtdError(ex); }
        }
    }
}
