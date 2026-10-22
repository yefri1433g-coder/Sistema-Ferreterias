using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class TrasladosDatos
    {
        private readonly ConexionDatos _conexionDatos = new ConexionDatos();

        // =====================================================
        // CRUD: CONSULTAR / BUSCAR
        // =====================================================

        private List<TrasladosEntidad> MtdLeer(string filtro)
        {
            List<TrasladosEntidad> lista = new List<TrasladosEntidad>();

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    string QueryConsultar = @"
                        SELECT t.id_traslado AS CodigoTraslado, t.id_empleado AS CodigoEmpleado,
                               e.nombre + ' ' + e.apellido AS NombreEmpleado,
                               t.id_sucursal_origen AS SucursalOrigen, so.nombre AS NombreOrigen,
                               t.id_sucursal_destino AS SucursalDestino, sd.nombre AS NombreDestino,
                               t.fecha AS FechaTraslado,
                               CASE WHEN t.estado = 1 THEN 'Pendiente' ELSE 'Completado' END AS Estado
                        FROM Tbl_Traslados t
                        INNER JOIN Tbl_Empleados e ON e.id_empleado = t.id_empleado
                        INNER JOIN Tbl_Sucursales so ON so.id_sucursal = t.id_sucursal_origen
                        INNER JOIN Tbl_Sucursales sd ON sd.id_sucursal = t.id_sucursal_destino
                        WHERE (@Filtro = ''
                               OR so.nombre LIKE '%' + @Filtro + '%'
                               OR sd.nombre LIKE '%' + @Filtro + '%'
                               OR e.nombre LIKE '%' + @Filtro + '%'
                               OR e.apellido LIKE '%' + @Filtro + '%'
                               OR CASE WHEN t.estado = 1 THEN 'Pendiente' ELSE 'Completado' END LIKE '%' + @Filtro + '%')
                        ORDER BY t.id_traslado DESC;
                    ";

                    using (SqlCommand cmd = new SqlCommand(QueryConsultar, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filtro", filtro ?? string.Empty);
                        conn.Open();

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                TrasladosEntidad item = new TrasladosEntidad
                                (
                                    Convert.ToInt32(rd["CodigoTraslado"]),
                                    Convert.ToInt32(rd["CodigoEmpleado"]),
                                    rd["NombreEmpleado"].ToString(),
                                    Convert.ToInt32(rd["SucursalOrigen"]),
                                    rd["NombreOrigen"].ToString(),
                                    Convert.ToInt32(rd["SucursalDestino"]),
                                    rd["NombreDestino"].ToString(),
                                    Convert.ToDateTime(rd["FechaTraslado"]),
                                    rd["Estado"].ToString()
                                );

                                lista.Add(item);
                            }
                        }
                    }
                }

                return lista;
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "consultar los traslados");
            }
        }

        public List<TrasladosEntidad> MtdConsultar()
        {
            return MtdLeer(string.Empty);
        }

        public List<TrasladosEntidad> MtdBuscar(string filtro)
        {
            return MtdLeer(filtro);
        }

        // =====================================================
        // DETALLE DEL TRASLADO
        // =====================================================

        public List<DetalleTrasladoEntidad> MtdConsultarDetalle(int codigoTraslado)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();
                    return MtdLeerDetalle(conn, null, codigoTraslado);
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "consultar el detalle del traslado");
            }
        }

        private List<DetalleTrasladoEntidad> MtdLeerDetalle(SqlConnection conn, SqlTransaction tx, int codigoTraslado)
        {
            List<DetalleTrasladoEntidad> lista = new List<DetalleTrasladoEntidad>();

            string QueryDetalle = @"
                SELECT d.id_producto AS CodigoProducto, p.nombre AS NombreProducto, d.cantidad AS Cantidad
                FROM Tbl_DetalleTraslado d
                INNER JOIN Tbl_Productos p ON p.id_producto = d.id_producto
                WHERE d.id_traslado = @CodigoTraslado
                ORDER BY d.id_detalle_traslado;
            ";

            using (SqlCommand cmd = new SqlCommand(QueryDetalle, conn, tx))
            {
                cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        lista.Add(new DetalleTrasladoEntidad
                        (
                            Convert.ToInt32(rd["CodigoProducto"]),
                            rd["NombreProducto"].ToString(),
                            Convert.ToInt32(rd["Cantidad"])
                        ));
                    }
                }
            }

            return lista;
        }

        private void MtdInsertarDetalle(SqlConnection conn, SqlTransaction tx, int codigoTraslado, List<DetalleTrasladoEntidad> detalles)
        {
            foreach (DetalleTrasladoEntidad det in detalles)
            {
                string QueryDetalle = @"
                    INSERT INTO Tbl_DetalleTraslado (id_traslado, id_producto, cantidad)
                    VALUES (@CodigoTraslado, @CodigoProducto, @Cantidad);
                ";

                using (SqlCommand cmd = new SqlCommand(QueryDetalle, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    cmd.Parameters.AddWithValue("@CodigoProducto", det.CodigoProducto);
                    cmd.Parameters.AddWithValue("@Cantidad", det.Cantidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Lee el estado actual del traslado bloqueando la fila (dentro de la transaccion)
        private string MtdLeerEstado(SqlConnection conn, SqlTransaction tx, int codigoTraslado)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CASE WHEN estado = 1 THEN 'Pendiente' ELSE 'Completado' END FROM Tbl_Traslados WITH (UPDLOCK, ROWLOCK) WHERE id_traslado = @CodigoTraslado;", conn, tx))
            {
                cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                object resultado = cmd.ExecuteScalar();

                if (resultado == null)
                {
                    throw new Exception("El traslado no existe.");
                }

                return Convert.ToString(resultado);
            }
        }

        // =====================================================
        // CRUD: AGREGAR (queda en estado Pendiente, todavia no mueve stock)
        // =====================================================

        public int MtdAgregar(TrasladosEntidad traslado)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        string QueryAgregar = @"
                            INSERT INTO Tbl_Traslados (id_empleado, id_sucursal_origen, id_sucursal_destino, fecha, estado)
                            VALUES (@CodigoEmpleado, @SucursalOrigen, @SucursalDestino, @FechaTraslado, 1);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                        int codigoNuevo;

                        using (SqlCommand cmd = new SqlCommand(QueryAgregar, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@CodigoEmpleado", traslado.CodigoEmpleado);
                            cmd.Parameters.AddWithValue("@SucursalOrigen", traslado.SucursalOrigen);
                            cmd.Parameters.AddWithValue("@SucursalDestino", traslado.SucursalDestino);
                            cmd.Parameters.AddWithValue("@FechaTraslado", traslado.FechaTraslado);
                            codigoNuevo = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        MtdInsertarDetalle(conn, tx, codigoNuevo, traslado.Detalles);

                        tx.Commit();
                        return codigoNuevo;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "agregar el traslado");
            }
        }

        // =====================================================
        // CRUD: EDITAR (solo si no esta completado)
        // =====================================================

        public bool MtdEditar(TrasladosEntidad traslado)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        if (MtdLeerEstado(conn, tx, traslado.CodigoTraslado) == EstadosTraslado.Completado)
                        {
                            throw new Exception("El traslado ya fue completado y no se puede editar.");
                        }

                        string QueryEditar = @"
                            UPDATE Tbl_Traslados
                            SET id_empleado = @CodigoEmpleado,
                                id_sucursal_origen = @SucursalOrigen,
                                id_sucursal_destino = @SucursalDestino,
                                fecha = @FechaTraslado
                            WHERE id_traslado = @CodigoTraslado;
                        ";

                        using (SqlCommand cmd = new SqlCommand(QueryEditar, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@CodigoEmpleado", traslado.CodigoEmpleado);
                            cmd.Parameters.AddWithValue("@SucursalOrigen", traslado.SucursalOrigen);
                            cmd.Parameters.AddWithValue("@SucursalDestino", traslado.SucursalDestino);
                            cmd.Parameters.AddWithValue("@FechaTraslado", traslado.FechaTraslado);
                            cmd.Parameters.AddWithValue("@CodigoTraslado", traslado.CodigoTraslado);
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand del = new SqlCommand("DELETE FROM Tbl_DetalleTraslado WHERE id_traslado = @CodigoTraslado;", conn, tx))
                        {
                            del.Parameters.AddWithValue("@CodigoTraslado", traslado.CodigoTraslado);
                            del.ExecuteNonQuery();
                        }

                        MtdInsertarDetalle(conn, tx, traslado.CodigoTraslado, traslado.Detalles);

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar el traslado");
            }
        }

        // =====================================================
        // CAMBIAR ESTADO
        // Al pasar a Completado: resta stock en origen, suma en destino y registra en el Kardex
        // =====================================================

        public bool MtdCambiarEstado(int codigoTraslado, string nuevoEstado)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        int origen;
                        int destino;
                        string actual;

                        using (SqlCommand sel = new SqlCommand("SELECT id_sucursal_origen AS SucursalOrigen, id_sucursal_destino AS SucursalDestino, CASE WHEN estado = 1 THEN 'Pendiente' ELSE 'Completado' END AS Estado FROM Tbl_Traslados WITH (UPDLOCK, ROWLOCK) WHERE id_traslado = @CodigoTraslado;", conn, tx))
                        {
                            sel.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);

                            using (SqlDataReader rd = sel.ExecuteReader())
                            {
                                if (!rd.Read())
                                {
                                    throw new Exception("El traslado no existe.");
                                }

                                origen = Convert.ToInt32(rd["SucursalOrigen"]);
                                destino = Convert.ToInt32(rd["SucursalDestino"]);
                                actual = rd["Estado"].ToString();
                            }
                        }

                        if (actual == EstadosTraslado.Completado)
                        {
                            throw new Exception("El traslado ya fue completado y no se puede modificar.");
                        }

                        if (nuevoEstado == EstadosTraslado.Completado)
                        {
                            List<DetalleTrasladoEntidad> detalles = MtdLeerDetalle(conn, tx, codigoTraslado);

                            if (detalles.Count == 0)
                            {
                                throw new Exception("El traslado no tiene productos.");
                            }

                            string nombreOrigen = KardexDatos.MtdNombreSucursal(conn, tx, origen);
                            string nombreDestino = KardexDatos.MtdNombreSucursal(conn, tx, destino);

                            foreach (DetalleTrasladoEntidad det in detalles)
                            {
                                // 1) Resta en la sucursal de origen (solo si alcanza el stock)
                                string QueryBaja = @"
                                    UPDATE Tbl_Stock
                                    SET cantidad = cantidad - @Cantidad
                                    WHERE id_sucursal = @Origen AND id_producto = @CodigoProducto AND cantidad >= @Cantidad;
                                ";

                                using (SqlCommand baja = new SqlCommand(QueryBaja, conn, tx))
                                {
                                    baja.Parameters.AddWithValue("@Cantidad", det.Cantidad);
                                    baja.Parameters.AddWithValue("@Origen", origen);
                                    baja.Parameters.AddWithValue("@CodigoProducto", det.CodigoProducto);

                                    if (baja.ExecuteNonQuery() == 0)
                                    {
                                        throw new Exception("Stock insuficiente en " + nombreOrigen + " para: " + det.NombreProducto);
                                    }
                                }

                                // 2) Suma en la sucursal de destino (si no existe el registro, lo crea)
                                string QueryAlta = @"
                                    UPDATE Tbl_Stock
                                    SET cantidad = cantidad + @Cantidad
                                    WHERE id_sucursal = @Destino AND id_producto = @CodigoProducto;

                                    IF @@ROWCOUNT = 0
                                        INSERT INTO Tbl_Stock (id_sucursal, id_producto, cantidad, stock_minimo)
                                        VALUES (@Destino, @CodigoProducto, @Cantidad, 0);
                                ";

                                using (SqlCommand alta = new SqlCommand(QueryAlta, conn, tx))
                                {
                                    alta.Parameters.AddWithValue("@Cantidad", det.Cantidad);
                                    alta.Parameters.AddWithValue("@Destino", destino);
                                    alta.Parameters.AddWithValue("@CodigoProducto", det.CodigoProducto);
                                    alta.ExecuteNonQuery();
                                }

                                // 3) Registra el movimiento en el Kardex
                                KardexDatos.MtdRegistrar(conn, tx, det.CodigoProducto, "Traslado", det.Cantidad,
                                    "Traslado #" + codigoTraslado + ": " + nombreOrigen + " -> " + nombreDestino);
                            }
                        }

                        using (SqlCommand upd = new SqlCommand("UPDATE Tbl_Traslados SET estado = @Estado WHERE id_traslado = @CodigoTraslado;", conn, tx))
                        {
                            upd.Parameters.AddWithValue("@Estado", nuevoEstado == EstadosTraslado.Completado ? 0 : 1);
                            upd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                            upd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "cambiar el estado del traslado");
            }
        }

        // =====================================================
        // CRUD: ELIMINAR (solo si no esta completado)
        // =====================================================

        public bool MtdEliminar(int codigoTraslado)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdObtenerConexionBDD())
                {
                    conn.Open();

                    using (SqlTransaction tx = conn.BeginTransaction())
                    {
                        if (MtdLeerEstado(conn, tx, codigoTraslado) == EstadosTraslado.Completado)
                        {
                            throw new Exception("No se puede eliminar un traslado completado.");
                        }

                        using (SqlCommand delDet = new SqlCommand("DELETE FROM Tbl_DetalleTraslado WHERE id_traslado = @CodigoTraslado;", conn, tx))
                        {
                            delDet.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                            delDet.ExecuteNonQuery();
                        }

                        using (SqlCommand del = new SqlCommand("DELETE FROM Tbl_Traslados WHERE id_traslado = @CodigoTraslado;", conn, tx))
                        {
                            del.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                            del.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar el traslado");
            }
        }
    }
}
