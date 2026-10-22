using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Datos
{

    public class CatalogosGestionDatos
    {
        private readonly ConexionDatos conexion = new ConexionDatos();

        private DataTable Leer(string sql)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                DataTable tabla = new DataTable();
                da.Fill(tabla);
                return tabla;
            }
        }

        public DataTable Categorias()
        {
            return Leer("SELECT id_categoria AS Codigo, nombre AS Nombre, descripcion AS Descripcion FROM Tbl_Categorias ORDER BY nombre;");
        }

        public DataTable Productos()
        {
            return Leer(@"SELECT p.id_producto AS Codigo, p.codigo AS SKU, p.nombre AS Nombre,
                                  p.descripcion AS Descripcion,
                                  p.precio_costo AS Costo, p.precio_venta AS Precio,
                                  p.id_categoria AS IdCategoria, c.nombre AS Categoria,
                                  p.estado AS Activo,
                                  ISNULL((SELECT SUM(st.cantidad) FROM Tbl_Stock st WHERE st.id_producto = p.id_producto), 0) AS Existencia,
                                  CASE WHEN EXISTS (SELECT 1 FROM Tbl_Stock st WHERE st.id_producto = p.id_producto AND st.cantidad <= st.stock_minimo) THEN 1 ELSE 0 END AS StockBajo
                           FROM Tbl_Productos p LEFT JOIN Tbl_Categorias c ON c.id_categoria = p.id_categoria
                           ORDER BY p.nombre;");
        }
        public DataTable Sucursales()
        {
            return Leer(@"SELECT s.id_sucursal AS Codigo, s.nombre AS Nombre, s.direccion AS Direccion,
                                  s.telefono AS Telefono, s.estado AS Activo,
                                  (SELECT COUNT(*) FROM Tbl_Empleados e WHERE e.id_sucursal = s.id_sucursal AND e.estado = 1) AS Personal
                           FROM Tbl_Sucursales s ORDER BY s.nombre;");
        }

        public DataTable BuscarSucursales(string criterio)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT s.id_sucursal AS Codigo, s.nombre AS Nombre, s.direccion AS Direccion,
                         s.telefono AS Telefono, s.estado AS Activo,
                         (SELECT COUNT(*) FROM dbo.Tbl_Empleados e WHERE e.id_sucursal = s.id_sucursal AND e.estado = 1) AS Personal
                  FROM dbo.Tbl_Sucursales s
                  WHERE s.nombre LIKE @criterio
                  ORDER BY s.nombre;", cn))
            {
                cmd.Parameters.Add("@criterio", SqlDbType.VarChar, 82).Value = "%" + (criterio ?? "").Trim() + "%";
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable tabla = new DataTable();
                    da.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable Proveedores()
        {
            return Leer("SELECT id_proveedor AS Codigo, nombre AS Nombre FROM Tbl_Proveedores ORDER BY nombre;");
        }

        public DataTable Compras()
        {
            return Leer(@"SELECT c.id_compra AS Codigo, p.nombre AS Proveedor, c.fecha AS Fecha,
                                  c.total AS Total,
                                  (SELECT COUNT(1) FROM Tbl_DetalleDeCompras d WHERE d.id_compra = c.id_compra) AS Lineas
                           FROM Tbl_Compras c
                           INNER JOIN Tbl_Proveedores p ON p.id_proveedor = c.id_proveedor
                           ORDER BY c.fecha DESC, c.id_compra DESC;");
        }

        public DataTable ProductosActivos()
        {
            return Leer("SELECT id_producto AS Codigo, nombre AS Nombre FROM Tbl_Productos WHERE estado = 1 ORDER BY nombre;");
        }

        public DataTable SucursalesActivas()
        {
            return Leer("SELECT id_sucursal AS Codigo, nombre AS Nombre FROM Tbl_Sucursales WHERE estado = 1 ORDER BY nombre;");
        }

        public void GuardarCategoria(int? id, string nombre, string descripcion)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? "UPDATE Tbl_Categorias SET nombre=@n, descripcion=@d WHERE id_categoria=@id"
                : "INSERT INTO Tbl_Categorias(nombre, descripcion) VALUES(@n,@d)", cn))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 60).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 150).Value = (object)(descripcion ?? "").Trim();
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void EliminarCategoria(int id)
        {
            Ejecutar("DELETE FROM Tbl_Categorias WHERE id_categoria=@id", id);
        }

        public void GuardarProducto(int? id, string codigo, string nombre, string descripcion, decimal costo, decimal precio, int categoria, bool activo)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? @"UPDATE Tbl_Productos SET codigo=@c,nombre=@n,descripcion=@d,precio_costo=@costo,precio_venta=@p,id_categoria=@cat,estado=@e WHERE id_producto=@id"
                : @"INSERT INTO Tbl_Productos(codigo,nombre,descripcion,precio_costo,precio_venta,id_categoria,estado) VALUES(@c,@n,@d,@costo,@p,@cat,@e)", cn))
            {
                cmd.Parameters.Add("@c", SqlDbType.VarChar, 30).Value = codigo.Trim();
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 100).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 200).Value = (object)(descripcion ?? "").Trim();
                cmd.Parameters.Add("@costo", SqlDbType.Decimal).Value = costo;
                cmd.Parameters["@costo"].Precision = 10; cmd.Parameters["@costo"].Scale = 2;
                cmd.Parameters.Add("@p", SqlDbType.Decimal).Value = precio;
                cmd.Parameters["@p"].Precision = 10; cmd.Parameters["@p"].Scale = 2;
                cmd.Parameters.Add("@cat", SqlDbType.Int).Value = categoria;
                cmd.Parameters.Add("@e", SqlDbType.Bit).Value = activo;
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void EliminarProducto(int id) { Ejecutar("DELETE FROM Tbl_Productos WHERE id_producto=@id", id); }

        public void GuardarSucursal(int? id, string nombre, string direccion, string telefono, bool activo)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(id.HasValue
                ? @"UPDATE Tbl_Sucursales SET nombre=@n,direccion=@d,telefono=@t,estado=@e WHERE id_sucursal=@id"
                : @"INSERT INTO Tbl_Sucursales(nombre,direccion,telefono,estado) VALUES(@n,@d,@t,@e)", cn))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 80).Value = nombre.Trim();
                cmd.Parameters.Add("@d", SqlDbType.VarChar, 150).Value = direccion.Trim();
                cmd.Parameters.Add("@t", SqlDbType.VarChar, 15).Value = (object)(telefono ?? "").Trim();
                cmd.Parameters.Add("@e", SqlDbType.Bit).Value = activo;
                if (id.HasValue) cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void AgregarSucursal(Sucursal sucursal)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO dbo.Tbl_Sucursales (nombre, direccion, telefono, estado) " +
                "VALUES (@nombre, @direccion, @telefono, @estado)", cn))
            {
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = sucursal.Nombre.Trim();
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = sucursal.Direccion.Trim();
                cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                    String.IsNullOrWhiteSpace(sucursal.Telefono) ? (object)DBNull.Value : sucursal.Telefono.Trim();
                cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = sucursal.Estado;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditarSucursal(Sucursal sucursal)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.Tbl_Sucursales
                      SET nombre = @nombre, direccion = @direccion,
                          telefono = @telefono, estado = @estado
                      WHERE id_sucursal = @idSucursal;", cn))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = sucursal.Nombre.Trim();
                    cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 150).Value = sucursal.Direccion.Trim();
                    cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value =
                        String.IsNullOrWhiteSpace(sucursal.Telefono) ? (object)DBNull.Value : sucursal.Telefono.Trim();
                    cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = sucursal.Estado;
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = sucursal.IdSucursal;

                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró la sucursal seleccionada para editar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "editar la sucursal");
            }
        }

        public void EliminarSucursal(int id)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Sucursales WHERE id_sucursal = @idSucursal", cn))
                {
                    cmd.Parameters.Add("@idSucursal", SqlDbType.Int).Value = id;
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No se encontró la sucursal seleccionada para eliminar.");
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "eliminar la sucursal");
            }
        }

        public int EliminarSucursalesDePrueba(int ultimoIdOriginal)
        {
            try
            {
                using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Tbl_Sucursales WHERE id_sucursal > @ultimoIdOriginal", cn))
                {
                    cmd.Parameters.Add("@ultimoIdOriginal", SqlDbType.Int).Value = ultimoIdOriginal;
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ErrorDatos.Traducir(ex, "limpiar las sucursales de prueba");
            }
        }

        private void Ejecutar(string sql, int id)
        {
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public int GuardarCompra(int proveedor, int sucursal, DateTime fecha, IList<CompraLineaCatalogo> detalles)
        {
            if (detalles == null || detalles.Count == 0) throw new InvalidOperationException("Agrega al menos un producto a la compra.");
            using (SqlConnection cn = conexion.MtdObtenerConexionBDD())
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        decimal total = 0;
                        foreach (CompraLineaCatalogo d in detalles) total += d.Subtotal;
                        int idCompra;
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_Compras(id_proveedor,fecha,total) VALUES(@prov,@fecha,@total); SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            cmd.Parameters.Add("@prov", SqlDbType.Int).Value = proveedor;
                            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                            cmd.Parameters.Add("@total", SqlDbType.Decimal).Value = total;
                            cmd.Parameters["@total"].Precision = 12; cmd.Parameters["@total"].Scale = 2;
                            idCompra = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        foreach (CompraLineaCatalogo d in detalles)
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_DetalleDeCompras(id_compra,id_producto,cantidad,precio_unitario) VALUES(@compra,@prod,@cant,@costo)", cn, tx))
                            {
                                cmd.Parameters.Add("@compra", SqlDbType.Int).Value = idCompra;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@costo", SqlDbType.Decimal).Value = d.CostoUnitario;

                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand("UPDATE Tbl_Stock WITH (UPDLOCK) SET cantidad=cantidad+@cant WHERE id_sucursal=@suc AND id_producto=@prod; IF @@ROWCOUNT=0 INSERT INTO Tbl_Stock(id_sucursal,id_producto,cantidad,stock_minimo) VALUES(@suc,@prod,@cant,0);", cn, tx))
                            {
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@suc", SqlDbType.Int).Value = sucursal;
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.ExecuteNonQuery();
                            }
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_Kardex(id_producto,fecha,tipo_movimiento,cantidad,saldo,descripcion) VALUES(@prod,@fecha,'Entrada',@cant,(SELECT ISNULL(SUM(cantidad),0) FROM Tbl_Stock WHERE id_producto=@prod),'Compra No. '+CAST(@compra AS VARCHAR(20)))", cn, tx))
                            {
                                cmd.Parameters.Add("@prod", SqlDbType.Int).Value = d.IdProducto;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                                cmd.Parameters.Add("@cant", SqlDbType.Int).Value = Decimal.ToInt32(d.Cantidad);
                                cmd.Parameters.Add("@compra", SqlDbType.Int).Value = idCompra;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit(); return idCompra;
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }
    }
}
