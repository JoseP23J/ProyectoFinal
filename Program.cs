using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.DynamicProxy;
using ProyectoFinal.Domain;
using ProyectoFinal.Events;
using ProyectoFinal.Aspects;
using ProyectoFinal.Functional;
using ProyectoFinal.Interfaces;

// ══════════════════════════════════════════════
// CASTLE WINDSOR — Contenedor de dependencias
// ══════════════════════════════════════════════
var container = new WindsorContainer();

container.Register(
    Component.For<LoggingInterceptor>().LifestyleTransient(),
    Component.For<ValidationInterceptor>().LifestyleTransient(),
    Component.For<INotificable, Cliente>()
        .ImplementedBy<Cliente>()
        .DependsOn(Dependency.OnValue("id", 1),
                   Dependency.OnValue("nombre", "Juan Pérez"),
                   Dependency.OnValue("email", "juan@email.com"),
                   Dependency.OnValue("direccion", "Calle 123"))
        .Interceptors<LoggingInterceptor, ValidationInterceptor>()
        .LifestyleTransient()
);

// ══════════════════════════════════════════════
// DOMINIO — Datos de prueba
// ══════════════════════════════════════════════
var tienda = new Tienda("TechStore");

// Productos
var laptop = new Producto(1, "Laptop Dell", 2500000m, 10, "Electrónica");
var mouse = new Producto(2, "Mouse Logitech", 85000m, 50, "Accesorios");
var teclado = new Producto(3, "Teclado Mecánico", 150000m, 8, "Accesorios");
var monitor = new Producto(4, "Monitor Samsung", 800000m, 3, "Electrónica");

tienda.AgregarProducto(laptop);
tienda.AgregarProducto(mouse);
tienda.AgregarProducto(teclado);
tienda.AgregarProducto(monitor);

// Clientes y empleado
var cliente1 = new Cliente(1, "Juan Pérez", "juan@email.com", "Calle 123");
var cliente2 = new Cliente(2, "María García", "maria@email.com", "Carrera 45");
var empleado = new Empleado(1, "Carlos López", "carlos@tienda.com", "Vendedor", 3000000m);

tienda.AgregarCliente(cliente1);
tienda.AgregarCliente(cliente2);

// ══════════════════════════════════════════════
// EVENTOS — Suscripción
// ══════════════════════════════════════════════
laptop.StockActualizado += (sender, e) =>
{
    Console.WriteLine($"📦 [EVENTO] Stock de '{e.NombreProducto}': {e.StockAnterior} → {e.StockNuevo}");
    if (e.EsCritico)
        Console.WriteLine($"⚠️  [ALERTA] Stock crítico en '{e.NombreProducto}'!");
};

monitor.StockActualizado += (sender, e) =>
{
    Console.WriteLine($"📦 [EVENTO] Stock de '{e.NombreProducto}': {e.StockAnterior} → {e.StockNuevo}");
    if (e.EsCritico)
        Console.WriteLine($"⚠️  [ALERTA] Stock crítico en '{e.NombreProducto}'!");
};

// ══════════════════════════════════════════════
// PEDIDOS
// ══════════════════════════════════════════════
Console.WriteLine("══════════════ CREANDO PEDIDOS ══════════════");

var pedido1 = tienda.CrearPedido(cliente1, empleado);
pedido1.PedidoCreado += (sender, e) =>
{
    Console.WriteLine($"🛒 [EVENTO] Pedido #{e.PedidoId} creado para {e.NombreCliente} | Total: {e.Total:C}");
    cliente1.EnviarNotificacion($"Tu pedido #{e.PedidoId} fue confirmado por {e.Total:C}");
};
pedido1.AgregarLinea(laptop, 1);
pedido1.AgregarLinea(mouse, 2);
pedido1.Confirmar();

Console.WriteLine();

var pedido2 = tienda.CrearPedido(cliente2, empleado);
pedido2.PedidoCreado += (sender, e) =>
{
    Console.WriteLine($"🛒 [EVENTO] Pedido #{e.PedidoId} creado para {e.NombreCliente} | Total: {e.Total:C}");
    cliente2.EnviarNotificacion($"Tu pedido #{e.PedidoId} fue confirmado por {e.Total:C}");
};
pedido2.AgregarLinea(monitor, 2);
pedido2.AgregarLinea(teclado, 1);
pedido2.Confirmar();

// ══════════════════════════════════════════════
// POLIMORFISMO — demostrable en tiempo de ejecución
// ══════════════════════════════════════════════
Console.WriteLine("\n══════════════ POLIMORFISMO ══════════════");
List<Persona> personas = new() { cliente1, cliente2, empleado };
foreach (var p in personas)
    Console.WriteLine(p.ToString()); // cada uno imprime su propio ObtenerRol()

// ══════════════════════════════════════════════
// PROGRAMACIÓN FUNCIONAL
// ══════════════════════════════════════════════
Console.WriteLine("\n══════════════ FUNCIONAL ══════════════");

// Where
var accesorios = TiendaQueries.ProductosPorCategoria(tienda.Catalogo, "Accesorios");
Console.WriteLine("Accesorios disponibles:");
foreach (var p in accesorios) Console.WriteLine($"  • {p.Nombre}");

// Select
Console.WriteLine("\nCatálogo formateado (Select):");
var nombresFormateados = TiendaQueries.ObtenerNombresProductos(tienda.Catalogo);
foreach (var n in nombresFormateados) Console.WriteLine($"  {n}");

// Aggregate
var totalVentas = TiendaQueries.CalcularTotalVentas(tienda.Pedidos);
Console.WriteLine($"\nTotal ventas (Aggregate): {totalVentas:C}");

// Func<> de alto orden
var productosCaros = TiendaQueries.FiltrarProductos(
    tienda.Catalogo, p => p.Precio > 200000m);
Console.WriteLine("\nProductos con precio > $200.000 (Func<>):");
foreach (var p in productosCaros) Console.WriteLine($"  • {p.Nombre}: {p.Precio:C}");

// Action<> de alto orden
Console.WriteLine("\nReposición de stock (Action<>):");
TiendaQueries.ProcesarProductos(
    tienda.Catalogo.Where(p => p.Stock <= 5),
    p => p.ReponerStock(10));

// Record — ResumenPedido
TiendaQueries.MostrarReporte(tienda.Pedidos, tienda.Catalogo);

// ══════════════════════════════════════════════
// CASTLE WINDSOR — AOP en acción
// ══════════════════════════════════════════════
Console.WriteLine("══════════════ AOP — INTERCEPTORES ══════════════");
var clienteProxy = container.Resolve<INotificable>();
clienteProxy.EnviarNotificacion("Bienvenido a TechStore!");

container.Dispose();
Console.WriteLine("\n✅ Proyecto finalizado correctamente.");