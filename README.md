# 🛒 TechStore — Sistema de Tienda en C# (.NET 9)

Proyecto final de programación que implementa un sistema de tienda (TechStore) demostrando múltiples paradigmas de programación en C# con .NET 9.

---

## 📋 Descripción del Sistema

TechStore es un sistema que gestiona productos, clientes, empleados y pedidos. Permite crear pedidos, reducir stock automáticamente, notificar a clientes y generar reportes, integrando varios paradigmas en un mismo dominio.

**Entidades principales:**
- `Tienda` — orquestadora del sistema (agregación de productos, clientes y pedidos)
- `Producto` — con control de stock y eventos de actualización
- `Cliente` / `Empleado` — heredan de `Persona`, con polimorfismo
- `Pedido` / `LineaPedido` — composición, ciclo de vida completo con estados

---

## 🧩 Paradigmas Implementados

### 1. Orientado a Objetos (OOP)

| Concepto | Implementación |
|---|---|
| **Herencia** | `Cliente` y `Empleado` heredan de `Persona` |
| **Polimorfismo** | `ObtenerRol()` sobreescrito en cada subclase |
| **Encapsulamiento** | Stock privado con métodos `ReducirStock` / `ReponerStock` |
| **Interfaces** | `INotificable` implementada por `Cliente` |
| **Composición** | `Pedido` contiene `List<LineaPedido>` (nacen y mueren juntos) |
| **Asociación** | `Pedido` referencia a `Cliente` y `Empleado` externos |
| **Eventos** | `PedidoCreado` y `StockActualizado` con `EventHandler<T>` |

**Decisión de diseño:** Se usó composición para `LineaPedido` (las líneas no tienen sentido fuera de un pedido) y asociación para `Cliente`/`Empleado` (existen independientemente del pedido).

---

### 2. Programación Funcional

Implementada en `Functional/TiendaQueries.cs` usando LINQ y funciones de alto orden:

| Técnica | Método |
|---|---|
| **Where** | `ProductosPorCategoria`, `ProductosConStockBajo`, `PedidosPorCliente` |
| **Select** | `ObtenerNombresProductos`, `GenerarResumenes` |
| **Aggregate** | `CalcularTotalVentas`, `GenerarCatalogo` |
| **Func<>** | `FiltrarProductos(productos, criterio)` |
| **Action<>** | `ProcesarProductos(productos, accion)` |
| **Records** | `ResumenPedido` — snapshot inmutable de un pedido |

**Decisión de diseño:** Se separaron todas las consultas en una clase estática `TiendaQueries` para mantener las funciones puras (sin efectos secundarios) separadas del dominio mutable.

---

### 3. Programación Orientada a Aspectos (AOP)

Implementada con **Castle Windsor** y **Castle DynamicProxy**:

| Aspecto | Interceptor |
|---|---|
| **Logging** | `LoggingInterceptor` — registra entrada, argumentos y valor de retorno |
| **Validación** | `ValidationInterceptor` — valida argumentos antes de ejecutar |

**Decisión de diseño:** Se usa Castle Windsor como contenedor IoC para registrar e inyectar los interceptores sobre `INotificable`, de modo que el logging y la validación ocurren automáticamente sin modificar la clase `Cliente`.

---

## 🗂️ Estructura del Proyecto

```
ProyectoFinal/
├── Domain/
│   ├── Persona.cs          # Clase base abstracta
│   ├── Cliente.cs          # Hereda de Persona, implementa INotificable
│   ├── Empleado.cs         # Hereda de Persona
│   ├── Producto.cs         # Entidad con eventos de stock
│   ├── Pedido.cs           # Composición de LineaPedido + eventos
│   ├── LineaPedido.cs      # Línea de detalle de pedido
│   └── Tienda.cs           # Orquestadora principal
├── Events/
│   ├── PedidoCreadoEventArgs.cs
│   └── StockActualizadoEventArgs.cs
├── Functional/
│   └── TiendaQueries.cs    # Funciones puras, LINQ, records
├── Interfaces/
│   └── INotificable.cs
├── Aspects/
│   ├── LoggingInterceptor.cs
│   └── ValidationInterceptor.cs
└── Program.cs              # Punto de entrada y demostración
```

---

## ▶️ Cómo Ejecutar

**Requisitos:** .NET 9 SDK

```bash
# Clonar el repositorio
git clone https://github.com/TU_USUARIO/ProyectoFinal.git
cd ProyectoFinal

# Restaurar dependencias y ejecutar
dotnet run
```

---

## 📦 Dependencias

- `Castle.Windsor` — contenedor de inyección de dependencias
- `Castle.Core` — proxy dinámico para AOP

---

## 👥 Integrantes

| Nombre | Rol |
|---|---|
| [Nombre 1] | [Paradigma / módulo] |
| [Nombre 2] | [Paradigma / módulo] |
