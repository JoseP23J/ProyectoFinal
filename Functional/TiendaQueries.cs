using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProyectoFinal.Domain;

namespace ProyectoFinal.Functional;

// Record inmutable para snapshots de reportes
public record ResumenPedido(
    int PedidoId,
    string NombreCliente,
    decimal Total,
    int CantidadProductos,
    DateTime Fecha
);

public static class TiendaQueries
{
    // ── FUNCIONES PURAS (sin efectos secundarios) ──────────────────────

    // Func<> como parámetro de alto orden
    public static IEnumerable<Producto> FiltrarProductos(
        IEnumerable<Producto> productos,
        Func<Producto, bool> criterio) => productos.Where(criterio);

    public static IEnumerable<T> Transformar<T, R>(
        IEnumerable<T> lista,
        Func<T, T> transformacion) => lista.Select(transformacion);

    // ── LINQ: Where ────────────────────────────────────────────────────

    public static IEnumerable<Producto> ProductosConStockBajo(
        IEnumerable<Producto> productos, int umbral = 10) =>
        productos.Where(p => p.Stock <= umbral);

    public static IEnumerable<Producto> ProductosPorCategoria(
        IEnumerable<Producto> productos, string categoria) =>
        productos.Where(p => p.Categoria.Equals(categoria,
            StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<Pedido> PedidosPorCliente(
        IEnumerable<Pedido> pedidos, int clienteId) =>
        pedidos.Where(p => p.Cliente.Id == clienteId);

    // ── LINQ: Select ───────────────────────────────────────────────────

    public static IEnumerable<ResumenPedido> GenerarResumenes(
        IEnumerable<Pedido> pedidos) =>
        pedidos.Select(p => new ResumenPedido(
            p.Id,
            p.Cliente.Nombre,
            p.Total,
            p.Lineas.Count,
            p.FechaCreacion
        ));

    public static IEnumerable<string> ObtenerNombresProductos(
        IEnumerable<Producto> productos) =>
        productos.Select(p => $"{p.Nombre} ({p.Categoria}) - {p.Precio:C}");

    // ── LINQ: Aggregate ────────────────────────────────────────────────

    public static decimal CalcularTotalVentas(IEnumerable<Pedido> pedidos) =>
        pedidos.Aggregate(0m, (acum, p) => acum + p.Total);

    public static string GenerarCatalogo(IEnumerable<Producto> productos) =>
        productos.Aggregate("📦 CATÁLOGO:\n", (acum, p) =>
            acum + $"  • {p.Nombre} - {p.Precio:C}\n");

    // ── Action<> como parámetro de alto orden ─────────────────────────

    public static void ProcesarProductos(
        IEnumerable<Producto> productos,
        Action<Producto> accion)
    {
        foreach (var p in productos) accion(p);
    }

    // ── Reporte completo (función pura) ───────────────────────────────

    public static void MostrarReporte(
        IEnumerable<Pedido> pedidos,
        IEnumerable<Producto> productos)
    {
        var resumenes = GenerarResumenes(pedidos).ToList();
        var totalVentas = CalcularTotalVentas(pedidos);
        var stockBajo = ProductosConStockBajo(productos).ToList();

        Console.WriteLine("\n══════════════ REPORTE FUNCIONAL ══════════════");
        Console.WriteLine($"Total pedidos: {resumenes.Count}");
        Console.WriteLine($"Total ventas:  {totalVentas:C}");

        Console.WriteLine("\n📋 Resumen de pedidos:");
        resumenes.ForEach(r => Console.WriteLine(
            $"  Pedido #{r.PedidoId} | {r.NombreCliente} | {r.Total:C} | {r.CantidadProductos} productos"));

        Console.WriteLine("\n⚠️ Productos con stock bajo:");
        if (stockBajo.Any())
            stockBajo.ForEach(p => Console.WriteLine($"  • {p.Nombre}: {p.Stock} unidades"));
        else
            Console.WriteLine("  Todos los productos tienen stock suficiente.");

        Console.WriteLine("════════════════════════════════════════════════\n");
    }
}
