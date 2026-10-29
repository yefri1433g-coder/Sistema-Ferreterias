using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    // CLASE ProveedoresForms: Presenta información y conecta acciones de usuario con la capa Negocio.
    public partial class ProveedoresForms : Form
    {
        // Instanciar: Que el forms actual (capa presentacion) pueda acceder a otra capa (Negocio)
        private readonly ProveedoresNegocio _proveedoresNegocio = new ProveedoresNegocio();

        // Variables de control: registro cargado en Gestionar y modo actual (Agregar / Editar)
        private int _codigoSeleccionado = 0;
        private string _modo = string.Empty;

        // La lista completa y la página actual se mantienen únicamente en Presentación.
        private const int RegistrosPorPagina = 10;
        private List<ProveedoresEntidad> _listaProveedores = new List<ProveedoresEntidad>();
        private int _paginaActual = 0;
        private readonly Label[] _valoresResumen = new Label[3];
        private Form _dialogoProveedor;

        public ProveedoresForms()
        {
            InitializeComponent();
            EstiloAplicacion.Aplicar(this);
            AplicarDisenoProveedores();
            PrepararDialogoProveedor();
            txtBuscar.TextChanged += btnBuscar_Click;
            BarraListado.Configurar(dgvProveedores, txtBuscar, "Proveedores", () => dgv_CellDoubleClick(dgvProveedores, new DataGridViewCellEventArgs(0, dgvProveedores.CurrentRow.Index)));
            btnExportar.Visible = false;
            btnImprimir.Visible = false;
        }

        // CONFIGURACIÓN: PrepararDialogoProveedor
        private void PrepararDialogoProveedor()
        {
            _dialogoProveedor = new Form { Text = "Nuevo proveedor", ClientSize = new Size(900, 470), BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            tabPage2.Controls.Remove(PanelPrincipalGestionar);
            PanelPrincipalGestionar.Dock = DockStyle.Fill;
            panelEncabezadoGestionar.Visible = false;
            PanelCerrarGestionar.Visible = false;
            _dialogoProveedor.Controls.Add(PanelPrincipalGestionar);
        }

        // CONFIGURACIÓN: AplicarDisenoProveedores
        private void AplicarDisenoProveedores()
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(0, 1);
            tabPage1.Padding = Padding.Empty;
            tabPage2.Padding = new Padding(20);
            tabPage1.Controls.Clear();
            tabPage1.BackColor = EstiloAplicacion.Fondo;

            TableLayoutPanel pantalla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(18, 12, 18, 18), BackColor = EstiloAplicacion.Fondo };
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 104));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            pantalla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tabPage1.Controls.Add(pantalla);

            Panel encabezado = new Panel { Dock = DockStyle.Fill, BackColor = EstiloAplicacion.Fondo };
            Label ruta = new Label { Text = "Inicio  /  Proveedores", Left = 2, Top = 0, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(116, 137, 145) };
            Label titulo = new Label { Text = "Proveedores", Left = 2, Top = 16, AutoSize = true, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            Label descripcion = new Label { Text = "Administra contactos e informaci\u00F3n de tus proveedores.", Left = 4, Top = 64, AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(116, 137, 145) };
            encabezado.Controls.Add(ruta); encabezado.Controls.Add(titulo); encabezado.Controls.Add(descripcion);
            pantalla.Controls.Add(encabezado, 0, 0);

            TableLayoutPanel tarjetas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = EstiloAplicacion.Fondo, Padding = new Padding(0, 0, 0, 10) };
            for (int i = 0; i < 3; i++)
            {
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
                Panel tarjeta = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(i == 0 ? 0 : 8, 0, 8, 0) };
                Label icono = new Label { Text = i == 0 ? "\u25B2" : i == 1 ? "\u25F7" : "@", Location = new Point(14, 13), Size = new Size(28, 28), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = EstiloAplicacion.Acento, BackColor = EstiloAplicacion.FondoSuave };
                Label tituloTarjeta = new Label { Text = i == 0 ? "REGISTRADOS" : i == 1 ? "CON TEL\u00C9FONO" : "CON CORREO", Left = 52, Top = 14, AutoSize = true, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = EstiloAplicacion.Secundario };
                _valoresResumen[i] = new Label { Left = 52, Top = 39, AutoSize = true, Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
                tarjeta.Controls.Add(icono); tarjeta.Controls.Add(tituloTarjeta); tarjeta.Controls.Add(_valoresResumen[i]);
                tarjetas.Controls.Add(tarjeta, i, 0);
            }
            pantalla.Controls.Add(tarjetas, 0, 1);

            TableLayoutPanel lista = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(14, 10, 14, 10), BackColor = Color.White };
            lista.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            lista.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            lista.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            Panel barra = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Label tituloLista = new Label { Text = "Proveedores recientes", Left = 4, Top = 2, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = EstiloAplicacion.Tinta };
            Label fechaLista = new Label { Text = "Informaci\u00F3n actualizada \u00B7 " + DateTime.Today.ToString("dd MMM yyyy"), Left = 5, Top = 27, AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = EstiloAplicacion.Secundario };
            FlowLayoutPanel herramientas = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 500, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 6, 0, 0), BackColor = Color.White };
            txtBuscar.Size = new Size(145, 28); txtBuscar.Margin = new Padding(4, 0, 4, 0); txtBuscar.Font = new Font("Segoe UI", 8.5F);
            btnBuscar.Size = new Size(34, 28); btnBuscar.Margin = new Padding(4, 0, 4, 0); btnBuscar.Text = ""; btnBuscar.IconSize = 16; btnBuscar.BackColor = Color.White; btnBuscar.ForeColor = Color.FromArgb(62, 85, 91);
            btnLimpiarFiltro.Size = new Size(34, 28); btnLimpiarFiltro.Margin = new Padding(4, 0, 4, 0); btnLimpiarFiltro.Text = ""; btnLimpiarFiltro.IconSize = 16; btnLimpiarFiltro.BackColor = Color.White; btnLimpiarFiltro.ForeColor = Color.FromArgb(62, 85, 91);
            btnNuevo.Size = new Size(118, 28); btnNuevo.Margin = new Padding(4, 0, 0, 0); btnNuevo.Text = "+ Nuevo registro"; btnNuevo.IconChar = FontAwesome.Sharp.IconChar.None;
            btnNuevo.BackColor = EstiloAplicacion.Acento; btnNuevo.ForeColor = Color.White; btnNuevo.FlatAppearance.BorderSize = 0; btnNuevo.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            Button eliminarLista = new Button { Text = "Eliminar", Size = new Size(82, 28), Margin = new Padding(4, 0, 0, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(254, 242, 242), ForeColor = Color.FromArgb(185, 28, 28), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            eliminarLista.Click += (s, e) => EliminarProveedorDesdeLista();
            herramientas.Controls.Add(txtBuscar); herramientas.Controls.Add(btnBuscar); herramientas.Controls.Add(btnLimpiarFiltro); herramientas.Controls.Add(eliminarLista); herramientas.Controls.Add(btnNuevo);
            barra.Controls.Add(tituloLista); barra.Controls.Add(fechaLista); barra.Controls.Add(herramientas);
            lista.Controls.Add(barra, 0, 0);

            dgvProveedores.Dock = DockStyle.Fill;
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.RowHeadersVisible = false;
            dgvProveedores.MultiSelect = true;
            dgvProveedores.BackgroundColor = Color.White;
            dgvProveedores.BorderStyle = BorderStyle.None;
            dgvProveedores.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProveedores.GridColor = Color.FromArgb(234, 240, 240);
            dgvProveedores.EnableHeadersVisualStyles = false;
            dgvProveedores.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProveedores.ColumnHeadersHeight = 34;
            dgvProveedores.ColumnHeadersDefaultCellStyle.BackColor = EstiloAplicacion.Marino;
            dgvProveedores.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProveedores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProveedores.ColumnHeadersDefaultCellStyle.SelectionBackColor = EstiloAplicacion.Marino;
            dgvProveedores.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvProveedores.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
            dgvProveedores.DefaultCellStyle.ForeColor = EstiloAplicacion.Tinta;
            dgvProveedores.DefaultCellStyle.SelectionBackColor = EstiloAplicacion.Seleccion;
            dgvProveedores.DefaultCellStyle.SelectionForeColor = EstiloAplicacion.Tinta;
            dgvProveedores.DefaultCellStyle.Padding = new Padding(5, 5, 5, 5);
            dgvProveedores.RowTemplate.Height = 42;
            dgvProveedores.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvProveedores.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lista.Controls.Add(dgvProveedores, 0, 1);

            Panel pie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            lblTotalRegistros.Dock = DockStyle.Left; lblTotalRegistros.Width = 230; lblTotalRegistros.TextAlign = ContentAlignment.MiddleLeft;
            btnPaginaAnterior.Size = new Size(104, 32); btnPaginaAnterior.Text = "\u2039 Anterior"; btnPaginaAnterior.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPaginaAnterior.TextImageRelation = TextImageRelation.Overlay; btnPaginaAnterior.TextAlign = ContentAlignment.MiddleCenter; btnPaginaAnterior.Padding = Padding.Empty; btnPaginaAnterior.Font = new Font("Segoe UI", 8.5F);
            btnPaginaSiguiente.Size = new Size(112, 32); btnPaginaSiguiente.Text = "Siguiente \u203A"; btnPaginaSiguiente.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPaginaSiguiente.TextImageRelation = TextImageRelation.Overlay; btnPaginaSiguiente.TextAlign = ContentAlignment.MiddleCenter; btnPaginaSiguiente.Padding = Padding.Empty; btnPaginaSiguiente.Font = new Font("Segoe UI", 8.5F);
            lblPagina.AutoSize = false; lblPagina.Size = new Size(110, 32); lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            FlowLayoutPanel accionesPie = new FlowLayoutPanel { Dock = DockStyle.Right, Width = 455, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 4, 0, 0), BackColor = Color.White };
            btnPaginaAnterior.Margin = new Padding(2, 0, 2, 0);
            btnPaginaSiguiente.Margin = new Padding(2, 0, 2, 0);
            lblPagina.Size = new Size(150, 32); lblPagina.Margin = new Padding(2, 0, 2, 0); lblPagina.Font = new Font("Segoe UI", 8.5F);
            btnExportar.Size = new Size(30, 28); btnExportar.Text = ""; btnExportar.IconSize = 16; btnExportar.Margin = new Padding(2, 0, 2, 0);
            btnImprimir.Size = new Size(30, 28); btnImprimir.Text = ""; btnImprimir.IconSize = 16; btnImprimir.Margin = new Padding(2, 0, 2, 0);
            ToolTip ayudaAcciones = new ToolTip(); ayudaAcciones.SetToolTip(btnExportar, "Exportar proveedores a CSV"); ayudaAcciones.SetToolTip(btnImprimir, "Imprimir lista de proveedores");
            btnCerrar.Visible = false;
            accionesPie.Controls.Add(btnPaginaAnterior); accionesPie.Controls.Add(lblPagina); accionesPie.Controls.Add(btnPaginaSiguiente); accionesPie.Controls.Add(btnExportar); accionesPie.Controls.Add(btnImprimir);
            pie.Controls.Add(lblTotalRegistros); pie.Controls.Add(accionesPie);
            lista.Controls.Add(pie, 0, 2);
            pantalla.Controls.Add(lista, 0, 2);
            tabControl1.SelectedTab = tabPage1;
        }

        private void ProveedoresForms_Load(object sender, EventArgs e)
        {
            try
            {
                MtdConsultar();
                MtdLimpiarCampos();
                MtdHabilitarCampos(false, false);
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }


        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        // CONSULTAR: MtdConsultar
        private void MtdConsultar()
        {
            MtdMostrarLista(_proveedoresNegocio.MtdConsultar());
        }

        // PRESENTAR DATOS: MtdMostrarLista
        private void MtdMostrarLista(List<ProveedoresEntidad> lista)
        {
            _listaProveedores = lista ?? new List<ProveedoresEntidad>();
            _paginaActual = 0;
            MtdActualizarResumen();
            MtdActualizarPaginacion();
        }

        // EDITAR: MtdActualizarResumen
        private void MtdActualizarResumen()
        {
            _valoresResumen[0].Text = _listaProveedores.Count.ToString();
            _valoresResumen[1].Text = _listaProveedores.Count(p => !string.IsNullOrWhiteSpace(p.Telefono)).ToString();
            _valoresResumen[2].Text = _listaProveedores.Count(p => !string.IsNullOrWhiteSpace(p.Correo)).ToString();
        }

        // EDITAR: MtdActualizarPaginacion
        private void MtdActualizarPaginacion()
        {
            int totalPaginas = (int)Math.Ceiling(_listaProveedores.Count / (double)RegistrosPorPagina);
            int indiceInicial = _paginaActual * RegistrosPorPagina;
            List<ProveedoresEntidad> registrosPagina = _listaProveedores
                .Skip(indiceInicial)
                .Take(RegistrosPorPagina)
                .ToList();

            dgvProveedores.DataSource = null;
            dgvProveedores.DataSource = registrosPagina;

            // Encabezados con nombres amigables y columnas ocultas
            AyudaForms.MtdOcultar(dgvProveedores, "CodigoProveedor");
            AyudaForms.MtdTitulo(dgvProveedores, "NombreProveedor", "PROVEEDOR");
            AyudaForms.MtdTitulo(dgvProveedores, "Contacto", "CONTACTO");
            AyudaForms.MtdTitulo(dgvProveedores, "Telefono", "TELÉFONO");
            AyudaForms.MtdTitulo(dgvProveedores, "Correo", "CORREO");
            AyudaForms.MtdTitulo(dgvProveedores, "Direccion", "DIRECCIÓN");
            if (dgvProveedores.Columns.Contains("NombreProveedor")) dgvProveedores.Columns["NombreProveedor"].FillWeight = 22;
            if (dgvProveedores.Columns.Contains("Contacto")) dgvProveedores.Columns["Contacto"].FillWeight = 18;
            if (dgvProveedores.Columns.Contains("Telefono")) dgvProveedores.Columns["Telefono"].FillWeight = 15;
            if (dgvProveedores.Columns.Contains("Correo")) dgvProveedores.Columns["Correo"].FillWeight = 23;
            if (dgvProveedores.Columns.Contains("Direccion")) dgvProveedores.Columns["Direccion"].FillWeight = 22;
            dgvProveedores.ClearSelection();



            // El total siempre corresponde al resultado completo, no a esta página.
            lblTotalRegistros.Text = "Total Registros: " + _listaProveedores.Count;
            lblPagina.Text = totalPaginas == 0
                ? "Página 0 de 0"
                : "Página " + (_paginaActual + 1) + " de " + totalPaginas;
            btnPaginaAnterior.Enabled = _paginaActual > 0;
            btnPaginaSiguiente.Enabled = _paginaActual + 1 < totalPaginas;
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (_paginaActual > 0)
            {
                _paginaActual--;
                MtdActualizarPaginacion();
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            int totalPaginas = (int)Math.Ceiling(_listaProveedores.Count / (double)RegistrosPorPagina);
            if (_paginaActual + 1 < totalPaginas)
            {
                _paginaActual++;
                MtdActualizarPaginacion();
            }
        }

        // BUSCAR: btnBuscar_Click
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                MtdMostrarLista(_proveedoresNegocio.MtdBuscar(txtBuscar.Text));
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // LIMPIAR: btnLimpiarFiltro_Click
        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Clear();
                MtdConsultar();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        private void ChkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionar.Checked)
            {
                dgvProveedores.SelectAll();
            }
            else
            {
                dgvProveedores.ClearSelection();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdImprimir(dgvProveedores, "Proveedores");
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            AyudaForms.MtdExportarCsv(dgvProveedores, "Proveedores");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Doble clic en una fila: la carga en la pestaña Gestionar
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            ProveedoresEntidad item = dgvProveedores.Rows[e.RowIndex].DataBoundItem as ProveedoresEntidad;

            if (item != null)
            {
                MtdCargarEnCampos(item);
                _modo = "Editar";
                MtdHabilitarCampos(true, false);
                btnEditar.Visible = false; btnEliminar.Visible = true;
                _dialogoProveedor.Text = "Editar proveedor";
                EstiloAplicacion.AplicarDialogo(_dialogoProveedor);
                _dialogoProveedor.ShowDialog(this);
            }
        }

        // =====================================================
        // GESTIONAR: METODOS DE APOYO
        // =====================================================

        // CARGA DE DATOS: MtdCargarEnCampos
        private void MtdCargarEnCampos(ProveedoresEntidad item)
        {
            _codigoSeleccionado = item.CodigoProveedor;
            _modo = string.Empty;

            txtCodigo.Text = item.CodigoProveedor.ToString();
            txtNombre.Text = item.NombreProveedor;
            txtContacto.Text = item.Contacto;
            txtTelefono.Text = item.Telefono;
            txtCorreo.Text = item.Correo;
            txtDireccion.Text = item.Direccion;

            MtdHabilitarCampos(false, false);
        }

        // LEER CAMPOS DEL FORMULARIO: Convierte los valores ingresados en una entidad de proveedor.
        private ProveedoresEntidad MtdObtenerDeCampos()
        {
            if (_modo == "Agregar")
            {
                return new ProveedoresEntidad
                (
                    txtNombre.Text.Trim(),
                    txtContacto.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtDireccion.Text.Trim()
                );
            }

            return new ProveedoresEntidad
                (
                    _codigoSeleccionado,
                    txtNombre.Text.Trim(),
                    txtContacto.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    txtDireccion.Text.Trim()
                );
        }

        // LIMPIAR: MtdLimpiarCampos
        private void MtdLimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtContacto.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
        }

        // habilitar: activa o desactiva los campos | esNuevo: los campos clave solo se activan al agregar
        private void MtdHabilitarCampos(bool habilitar, bool esNuevo)
        {
            txtNombre.Enabled = habilitar;
            txtContacto.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtCorreo.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;
        }

        private void MtdVolverAModoConsulta()
        {
            _codigoSeleccionado = 0;
            _modo = string.Empty;
            MtdLimpiarCampos();
            MtdHabilitarCampos(false, false);
            btnEditar.Visible = true; btnEliminar.Visible = true;
            tabControl1.SelectedTab = tabPage1;
            if (_dialogoProveedor != null && _dialogoProveedor.Visible) _dialogoProveedor.DialogResult = DialogResult.Cancel;
        }

        // =====================================================
        // GESTIONAR: BOTONES
        // =====================================================

        // AGREGAR: btnNuevo_Click
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
            _codigoSeleccionado = 0;
            _modo = "Agregar";
            MtdHabilitarCampos(true, true);
            btnEditar.Visible = false; btnEliminar.Visible = false;
            _dialogoProveedor.Text = "Nuevo proveedor";
            EstiloAplicacion.AplicarDialogo(_dialogoProveedor);
            _dialogoProveedor.ShowDialog(this);
            txtNombre.Focus();
        }

        // EDITAR: btnEditar_Click
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_codigoSeleccionado == 0)
            {
                AyudaForms.MtdAviso("Primero carga un registro: ve a Consultar y haz doble clic en una fila.");
                return;
            }

            _modo = "Editar";
            MtdHabilitarCampos(true, false);
            btnEditar.Visible = false; btnEliminar.Visible = true;
            _dialogoProveedor.Text = "Editar proveedor";
            EstiloAplicacion.AplicarDialogo(_dialogoProveedor);
            _dialogoProveedor.ShowDialog(this);
        }

        // GUARDAR: btnGuardar_Click
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modo == string.Empty)
                {
                    AyudaForms.MtdAviso("Presiona Nuevo o Editar antes de guardar.");
                    return;
                }

                ProveedoresEntidad entidad = MtdObtenerDeCampos();

                if (_modo == "Agregar")
                {
                    _proveedoresNegocio.MtdAgregar(entidad);
                }
                else
                {
                    bool actualizado = _proveedoresNegocio.MtdEditar(entidad);
                    if (!actualizado)
                    {
                        AyudaForms.MtdAviso("No se encontró el proveedor que intentas editar.");
                        return;
                    }
                }

                AyudaForms.MtdInfo("Registro guardado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // ELIMINAR: btnEliminar_Click
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_codigoSeleccionado == 0)
                {
                    AyudaForms.MtdAviso("Primero carga un registro: ve a Consultar y haz doble clic en una fila.");
                    return;
                }

                if (!AyudaForms.MtdConfirmar("¿Seguro que deseas eliminar este registro?"))
                {
                    return;
                }

                bool eliminado = _proveedoresNegocio.MtdEliminar(_codigoSeleccionado);
                if (!eliminado)
                {
                    AyudaForms.MtdAviso("No se encontró el proveedor que intentas eliminar.");
                    MtdConsultar();
                    MtdVolverAModoConsulta();
                    return;
                }

                AyudaForms.MtdInfo("Registro eliminado correctamente.");
                MtdConsultar();
                MtdVolverAModoConsulta();
            }
            catch (Exception ex)
            {
                AyudaForms.MtdError(ex);
            }
        }

        // ELIMINAR: EliminarProveedorDesdeLista
        private void EliminarProveedorDesdeLista()
        {
            if (dgvProveedores.CurrentRow == null) { AyudaForms.MtdAviso("Selecciona un proveedor primero."); return; }
            ProveedoresEntidad item = dgvProveedores.CurrentRow.DataBoundItem as ProveedoresEntidad;
            if (item == null) return;
            MtdCargarEnCampos(item);
            btnEliminar_Click(this, EventArgs.Empty);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdVolverAModoConsulta();
        }

        // LIMPIAR: btnLimpiar_Click
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnCerrarGestionar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    } //Clase
} // namespace
