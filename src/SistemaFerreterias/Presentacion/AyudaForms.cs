using Entidad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Presentacion
{
    // Metodos de ayuda compartidos por todos los formularios (mensajes, grillas, combos, imprimir y exportar)
    // CLASE AyudaForms: Reúne mensajes y utilidades que comparten los formularios.
    public static class AyudaForms
    {
        // =====================================================
        // MENSAJES
        // =====================================================

        public static void MtdInfo(string mensaje)
        {
            MessageBox.Show(mensaje, "Ferreterias Multisucursal", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void MtdAviso(string mensaje)
        {
            MessageBox.Show(mensaje, "Ferreterias Multisucursal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool MtdConfirmar(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void MtdError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ferreterias Multisucursal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // =====================================================
        // GRILLAS
        // =====================================================

        public static void MtdTitulo(DataGridView dgv, string columna, string texto)
        {
            if (dgv.Columns.Contains(columna))
            {
                dgv.Columns[columna].HeaderText = texto;
            }
        }

        public static void MtdOcultar(DataGridView dgv, params string[] columnas)
        {
            foreach (string columna in columnas)
            {
                if (dgv.Columns.Contains(columna))
                {
                    dgv.Columns[columna].Visible = false;
                }
            }
        }

        public static void MtdFormato(DataGridView dgv, string columna, string formato)
        {
            if (dgv.Columns.Contains(columna))
            {
                dgv.Columns[columna].DefaultCellStyle.Format = formato;
            }
        }

        // =====================================================
        // COMBOBOX
        // =====================================================

        public static void MtdLlenarCombo(ComboBox cbo, List<ComboItemEntidad> lista)
        {
            cbo.DataSource = null;
            cbo.DisplayMember = "Texto";
            cbo.ValueMember = "Codigo";
            cbo.DataSource = lista;
        }

        public static int MtdValorCombo(ComboBox cbo)
        {
            if (cbo.SelectedValue is int)
            {
                return (int)cbo.SelectedValue;
            }

            return 0;
        }

        // =====================================================
        // EXPORTAR E IMPRIMIR
        // Si hay varias filas seleccionadas (checkbox Seleccionar o Ctrl+clic) usa solo esas; si no, todas.
        // =====================================================

        private static List<DataGridViewColumn> MtdColumnasVisibles(DataGridView dgv)
        {
            return dgv.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        }

        private static List<DataGridViewRow> MtdFilasUsar(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 1)
            {
                return dgv.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).ToList();
            }

            return dgv.Rows.Cast<DataGridViewRow>().ToList();
        }

        private static string MtdTexto(DataGridViewRow fila, DataGridViewColumn columna)
        {
            object valor = fila.Cells[columna.Index].FormattedValue;
            return valor == null ? string.Empty : valor.ToString();
        }

        private static string MtdCsv(string texto)
        {
            if (texto.Contains(",") || texto.Contains("\"") || texto.Contains("\n"))
            {
                return "\"" + texto.Replace("\"", "\"\"") + "\"";
            }

            return texto;
        }

        public static void MtdExportarCsv(DataGridView dgv, string nombreBase)
        {
            if (dgv.Rows.Count == 0)
            {
                MtdAviso("No hay datos para exportar.");
                return;
            }

            List<DataGridViewColumn> columnas = MtdColumnasVisibles(dgv);
            List<DataGridViewRow> filas = MtdFilasUsar(dgv);

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Archivo CSV (*.csv)|*.csv";
                dialogo.FileName = nombreBase + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";

                if (dialogo.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join(",", columnas.Select(c => MtdCsv(c.HeaderText))));

                foreach (DataGridViewRow fila in filas)
                {
                    DataGridViewRow filaActual = fila;
                    sb.AppendLine(string.Join(",", columnas.Select(c => MtdCsv(MtdTexto(filaActual, c)))));
                }

                System.IO.File.WriteAllText(dialogo.FileName, sb.ToString(), new UTF8Encoding(true));
                MtdInfo("Archivo exportado correctamente.");
            }
        }

        public static void MtdImprimir(DataGridView dgv, string titulo)
        {
            if (dgv.Rows.Count == 0)
            {
                MtdAviso("No hay datos para imprimir.");
                return;
            }

            List<DataGridViewColumn> columnas = MtdColumnasVisibles(dgv);
            List<DataGridViewRow> filas = MtdFilasUsar(dgv);

            // Cada columna recibe un ancho proporcional al largo de su contenido
            float[] pesos = new float[columnas.Count];
            for (int i = 0; i < columnas.Count; i++)
            {
                int largo = columnas[i].HeaderText.Length;

                foreach (DataGridViewRow fila in filas.Take(50))
                {
                    largo = Math.Max(largo, Math.Min(MtdTexto(fila, columnas[i]).Length, 40));
                }

                pesos[i] = largo + 2;
            }
            float totalPesos = pesos.Sum();

            int indice = 0;
            PrintDocument documento = new PrintDocument();
            documento.DefaultPageSettings.Landscape = true;

            documento.BeginPrint += (s, ev) =>
            {
                indice = 0;
            };

            documento.PrintPage += (s, ev) =>
            {
                Graphics g = ev.Graphics;

                using (Font fTitulo = new Font("Segoe UI", 16, FontStyle.Bold))
                using (Font fEncabezado = new Font("Segoe UI", 9, FontStyle.Bold))
                using (Font fTexto = new Font("Segoe UI", 9))
                using (StringFormat formato = new StringFormat())
                {
                    formato.Trimming = StringTrimming.EllipsisCharacter;
                    formato.FormatFlags = StringFormatFlags.NoWrap;

                    float x0 = ev.MarginBounds.Left;
                    float y = ev.MarginBounds.Top;
                    float ancho = ev.MarginBounds.Width;

                    g.DrawString(titulo, fTitulo, Brushes.DarkBlue, x0, y);
                    y += 40;

                    // Encabezados
                    float x = x0;
                    for (int i = 0; i < columnas.Count; i++)
                    {
                        float anchoCol = ancho * pesos[i] / totalPesos;
                        g.FillRectangle(Brushes.Gainsboro, x, y, anchoCol, 22);
                        g.DrawString(columnas[i].HeaderText, fEncabezado, Brushes.Black, new RectangleF(x + 2, y + 3, anchoCol - 4, 20), formato);
                        x += anchoCol;
                    }
                    y += 24;

                    // Filas
                    while (indice < filas.Count && y + 20 < ev.MarginBounds.Bottom)
                    {
                        x = x0;
                        for (int i = 0; i < columnas.Count; i++)
                        {
                            float anchoCol = ancho * pesos[i] / totalPesos;
                            g.DrawString(MtdTexto(filas[indice], columnas[i]), fTexto, Brushes.Black, new RectangleF(x + 2, y + 2, anchoCol - 4, 18), formato);
                            x += anchoCol;
                        }
                        g.DrawLine(Pens.LightGray, x0, y + 19, x0 + ancho, y + 19);
                        y += 20;
                        indice++;
                    }

                    ev.HasMorePages = indice < filas.Count;
                }
            };

            using (PrintPreviewDialog vista = new PrintPreviewDialog())
            {
                vista.Document = documento;
                vista.WindowState = FormWindowState.Maximized;
                vista.ShowDialog();
            }
        }
    }
}
