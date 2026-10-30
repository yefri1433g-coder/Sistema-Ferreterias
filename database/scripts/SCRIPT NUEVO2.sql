--1.CREAR BASE DE DATOS

CREATE DATABASE db_Ferreteria;
GO
USE db_Ferreteria;
GO

--2.CREAR TABLAS

CREATE TABLE Tbl_Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL,
    contrasena VARCHAR(100) NOT NULL,
    rol VARCHAR(30) NOT NULL,
    estado BIT NOT NULL
);

CREATE TABLE Tbl_Sucursales (
    id_sucursal INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL,
    direccion VARCHAR(150) NOT NULL,
    telefono VARCHAR(15) NULL,
    estado BIT NOT NULL
);

CREATE TABLE Tbl_Categorias (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    descripcion VARCHAR(150) NULL
);

CREATE TABLE Tbl_Proveedores (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    contacto VARCHAR(80) NULL,
    telefono VARCHAR(15) NULL,
    correo VARCHAR(100) NULL,
    direccion VARCHAR(150) NULL
);

CREATE TABLE Tbl_Clientes (
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    nit VARCHAR(15) NULL,
    telefono VARCHAR(15) NULL,
    direccion VARCHAR(150) NULL
);

CREATE TABLE Tbl_Empleados (
    id_empleado INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_sucursal INT NOT NULL,
    nombre VARCHAR(60) NOT NULL,
    apellido VARCHAR(60) NOT NULL,
    cargo VARCHAR(50) NOT NULL,
    telefono VARCHAR(15) NULL,
    estado BIT NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES Tbl_Usuarios(id_usuario),
    FOREIGN KEY (id_sucursal) REFERENCES Tbl_Sucursales(id_sucursal)
);

CREATE TABLE Tbl_Productos (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    id_categoria INT NOT NULL,
    codigo VARCHAR(30) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200) NULL,
    precio_costo DECIMAL(10,2) NOT NULL,
    precio_venta DECIMAL(10,2) NOT NULL,
    estado BIT NOT NULL,
    FOREIGN KEY (id_categoria) REFERENCES Tbl_Categorias(id_categoria)
);

CREATE TABLE Tbl_Inventarios (
    id_inventario INT IDENTITY(1,1) PRIMARY KEY,
    id_sucursal INT NOT NULL,
    fecha DATETIME NOT NULL,
    descripcion VARCHAR(150) NULL,
    FOREIGN KEY (id_sucursal) REFERENCES Tbl_Sucursales(id_sucursal)
);

CREATE TABLE Tbl_Stock (
    id_stock INT IDENTITY(1,1) PRIMARY KEY,
    id_sucursal INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    stock_minimo INT NOT NULL,
    FOREIGN KEY (id_sucursal) REFERENCES Tbl_Sucursales(id_sucursal),
    FOREIGN KEY (id_producto) REFERENCES Tbl_Productos(id_producto)
);

CREATE TABLE Tbl_Kardex (
    id_kardex INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    fecha DATETIME NOT NULL,
    tipo_movimiento VARCHAR(20) NOT NULL,
    cantidad INT NOT NULL,
    saldo INT NOT NULL,
    descripcion VARCHAR(150) NULL,
    FOREIGN KEY (id_producto) REFERENCES Tbl_Productos(id_producto)
);

CREATE TABLE Tbl_Compras (
    id_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    id_sucursal INT NULL,
    fecha DATE NOT NULL,
    total DECIMAL(12,2) NOT NULL,
    FOREIGN KEY (id_proveedor) REFERENCES Tbl_Proveedores(id_proveedor),
    CONSTRAINT FK_Tbl_Compras_Tbl_Sucursales
        FOREIGN KEY (id_sucursal) REFERENCES Tbl_Sucursales(id_sucursal)
);

CREATE TABLE Tbl_DetalleDeCompras (
    id_detalle_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_compra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_compra) REFERENCES Tbl_Compras(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Tbl_Productos(id_producto)
);

CREATE TABLE Tbl_Ventas (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_sucursal INT NULL,
    fecha DATETIME NOT NULL,
    total DECIMAL(12,2) NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES Tbl_Clientes(id_cliente),
    FOREIGN KEY (id_sucursal) REFERENCES Tbl_Sucursales(id_sucursal)
);

CREATE TABLE Tbl_DetallesVentas (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_venta) REFERENCES Tbl_Ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Tbl_Productos(id_producto)
);

CREATE TABLE Tbl_Pagos (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    fecha DATETIME NOT NULL,
    monto DECIMAL(12,2) NOT NULL,
    metodo_pago VARCHAR(30) NOT NULL,
    FOREIGN KEY (id_venta) REFERENCES Tbl_Ventas(id_venta)
);

CREATE TABLE Tbl_Traslados (
    id_traslado INT IDENTITY(1,1) PRIMARY KEY,
    id_sucursal_origen INT NOT NULL,
    id_sucursal_destino INT NOT NULL,
    fecha DATETIME NOT NULL,
    estado TINYINT NOT NULL, -- 0 = Completado, 1 = Pendiente, 2 = En tránsito
    id_empleado INT NOT NULL,
    FOREIGN KEY (id_sucursal_origen) REFERENCES Tbl_Sucursales(id_sucursal),
    FOREIGN KEY (id_sucursal_destino) REFERENCES Tbl_Sucursales(id_sucursal),
    FOREIGN KEY (id_empleado) REFERENCES Tbl_Empleados(id_empleado)
);

CREATE TABLE Tbl_DetalleTraslado (
    id_detalle_traslado INT IDENTITY(1,1) PRIMARY KEY,
    id_traslado INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    FOREIGN KEY (id_traslado) REFERENCES Tbl_Traslados(id_traslado),
    FOREIGN KEY (id_producto) REFERENCES Tbl_Productos(id_producto)
);
GO

--3.INSERTAR 5 DATOS POR TABLA

DELETE FROM Tbl_Usuarios
WHERE id_usuario > 6
  AND id_usuario NOT IN (SELECT id_usuario FROM Tbl_Empleados);

-- Contraseñas provisionales: cámbialas después de crear la base antes de usarla.
INSERT INTO Tbl_Usuarios (nombre_usuario, contrasena, rol, estado) VALUES
('carlos', '1234', 'Administrador', 1),
('ana', '1234', 'Cajero', 1),
('mario', '1234', 'Bodeguero', 1),
('sofia', '1234', 'Vendedor', 1),
('luis', '1234', 'Supervisor', 1);

INSERT INTO Tbl_Sucursales (nombre, direccion, telefono, estado) VALUES
('Central Zona 1', '4ta Av 10-20 Zona 1', '22345678', 1),
('Sucursal Roosevelt', 'Calzada Roosevelt 14-50 Zona 11', '24718899', 1),
('Sucursal San Cristóbal', '3ra Calle 8-12 Mixco', '23651122', 1),
('Bodega Central Norte', 'Km 8.5 Ruta al Atlántico', '22589900', 1),
('Sucursal Xela', 'Av Las Américas 5-30 Quetzaltenango', '77654321', 1);

INSERT INTO Tbl_Categorias (nombre, descripcion) VALUES
('Herramientas Manuales', 'Martillos, destornilladores y llaves'),
('Materiales Básicos', 'Cemento, cal, hierro y agregados'),
('Fontanería', 'Tubos, grifería y accesorios PVC'),
('Electricidad', 'Cables, bombillos e interruptores'),
('Pinturas', 'Pinturas de agua, aceite y solventes');

INSERT INTO Tbl_Proveedores (nombre, contacto, telefono, correo, direccion) VALUES
('Distribuidora El Tornillo S.A.', 'Ing. Mario López', '24760011', 'contacto@tornillo.com', 'Zona 12 Industrial'),
('Cementos Progreso S.A.', 'Lic. Ana Reyes', '66349988', 'ventas@progreso.com', 'Carretera al Pacífico'),
('Tubos y PVC Maya', 'Pedro Gómez', '23334455', 'pedidos@tubosmaya.com', 'Zona 7 Mixco'),
('Electricidad Industrial Global', 'Karla Juárez', '24317766', 'kjuarez@electroglobal.com', 'Calzada San Juan'),
('Pinturas Continental', 'Roberto Castillo', '24881234', 'info@continental.com', 'Parque Norte');

INSERT INTO Tbl_Clientes (nombre, nit, telefono, direccion) VALUES
('Juan Alberto Morales', '4589214-3', '55123401', 'Zona 11 Las Charcas'),
('Constructora El Roble', '8921345-9', '23315500', 'Avenida Reforma Zona 9'),
('María Eugenia López', 'CF', '41239876', 'Mixco Sector 3'),
('Servicios Técnicos Rivas', '6547891-2', '59871122', 'Villa Nueva Zona 4'),
('Carlos Manuel Ortiz', '7845123-0', '52018844', 'Km 14 Carretera a El Salvador');

INSERT INTO Tbl_Empleados (id_usuario, id_sucursal, nombre, apellido, cargo, telefono, estado) VALUES
(1, 1, 'Carlos', 'Mendoza', 'Gerente General', '55210001', 1),
(2, 1, 'Ana', 'Guzmán', 'Cajera', '55210002', 1),
(3, 4, 'Mario', 'Chajón', 'Jefe de Bodega', '55210003', 1),
(4, 2, 'Sofía', 'Estrada', 'Ejecutiva de Ventas', '55210004', 1),
(5, 1, 'Luis', 'Juárez', 'Supervisor', '55210005', 1);

INSERT INTO Tbl_Productos (id_categoria, codigo, nombre, descripcion, precio_costo, precio_venta, estado) VALUES
(1, 'PROD-001', 'Martillo 16oz Mango Fibra', 'Martillo con mango anti vibración', 35.00, 55.00, 1),
(2, 'PROD-002', 'Bolsa de Cemento Gris 42.5kg', 'Cemento estándar de uso general', 72.00, 88.00, 1),
(3, 'PROD-003', 'Tubo PVC 1/2 pulgada SCH40', 'Tubo para agua a presión 6m', 18.00, 28.00, 1),
(4, 'PROD-004', 'Rollo Cable THHN 12 Negro 100m', 'Cable conductor de cobre aislado', 240.00, 320.00, 1),
(5, 'PROD-005', 'Cubeta Pintura Látex Blanco', 'Pintura de interior y exterior 5 gal', 310.00, 415.00, 1);

INSERT INTO Tbl_Inventarios (id_sucursal, fecha, descripcion) VALUES
(1, '2026-02-01 08:00:00', 'Auditoría mensual Sucursal Central'),
(2, '2026-02-01 08:30:00', 'Conteo inicial febrero Roosevelt'),
(3, '2026-02-02 09:00:00', 'Revisión periódica San Cristóbal'),
(4, '2026-02-02 07:00:00', 'Inventario general de Bodega Norte'),
(5, '2026-02-03 10:00:00', 'Cierre de existencias Xela');

INSERT INTO Tbl_Stock (id_sucursal, id_producto, cantidad, stock_minimo) VALUES
(1, 1, 45, 10),
(1, 2, 200, 30),
(2, 3, 120, 25),
(2, 4, 35, 8),
(4, 5, 80, 15);

INSERT INTO Tbl_Kardex (id_producto, fecha, tipo_movimiento, cantidad, saldo, descripcion) VALUES
(1, '2026-02-01 09:00:00', 'Entrada', 50, 50, 'Recepción de compra inicial'),
(2, '2026-02-01 10:00:00', 'Entrada', 250, 250, 'Lote recibido de cemento'),
(2, '2026-02-02 11:30:00', 'Salida', 50, 200, 'Despacho por venta a constructora'),
(3, '2026-02-02 14:00:00', 'Entrada', 150, 150, 'Ingreso tubería PVC'),
(4, '2026-02-03 08:45:00', 'Entrada', 40, 40, 'Recepción rollos cable eléctrico');

INSERT INTO Tbl_Compras (id_proveedor, fecha, total) VALUES
(1, '2026-01-15', 3500.00),
(2, '2026-01-18', 18000.00),
(3, '2026-01-20', 2700.00),
(4, '2026-01-22', 9600.00),
(5, '2026-01-25', 12400.00);

INSERT INTO Tbl_DetalleDeCompras (id_compra, id_producto, cantidad, precio_unitario) VALUES
(1, 1, 100, 35.00),
(2, 2, 250, 72.00),
(3, 3, 150, 18.00),
(4, 4, 40, 240.00),
(5, 5, 40, 310.00);

INSERT INTO Tbl_Ventas (id_cliente, fecha, total) VALUES
(1, '2026-02-04 09:15:00', 110.00),
(2, '2026-02-04 10:30:00', 4400.00),
(3, '2026-02-05 11:20:00', 84.00),
(4, '2026-02-05 14:40:00', 640.00),
(5, '2026-02-06 16:10:00', 415.00);

INSERT INTO Tbl_DetallesVentas (id_venta, id_producto, cantidad, precio_unitario) VALUES
(1, 1, 2, 55.00),
(2, 2, 50, 88.00),
(3, 3, 3, 28.00),
(4, 4, 2, 320.00),
(5, 5, 1, 415.00);

INSERT INTO Tbl_Pagos (id_venta, fecha, monto, metodo_pago) VALUES
(1, '2026-02-04 09:16:00', 110.00, 'Efectivo'),
(2, '2026-02-04 10:35:00', 4400.00, 'Transferencia'),
(3, '2026-02-05 11:22:00', 84.00, 'Efectivo'),
(4, '2026-02-05 14:42:00', 640.00, 'Tarjeta de Crédito'),
(5, '2026-02-06 16:12:00', 415.00, 'Tarjeta de Débito');

INSERT INTO Tbl_Traslados (id_sucursal_origen, id_sucursal_destino, fecha, estado, id_empleado) VALUES
(4, 1, '2026-02-05 08:00:00', 1, 3),
(4, 2, '2026-02-05 09:30:00', 1, 3),
(1, 3, '2026-02-06 10:15:00', 1, 5),
(4, 5, '2026-02-06 13:00:00', 1, 3),
(2, 3, '2026-02-07 15:30:00', 1, 4);

INSERT INTO Tbl_DetalleTraslado (id_traslado, id_producto, cantidad) VALUES
(1, 1, 15),
(2, 2, 40),
(3, 3, 25),
(4, 4, 10),
(5, 5, 5);
GO

IF NOT EXISTS (SELECT 1 FROM Tbl_Usuarios WHERE nombre_usuario = 'admin')
    INSERT INTO Tbl_Usuarios (nombre_usuario, contrasena, rol, estado)
    VALUES ('admin', 'CAMBIAR_CLAVE', 'Administrador', 1);
GO

