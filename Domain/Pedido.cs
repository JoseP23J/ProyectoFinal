using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

public enum EstadoPedido { Pendiente, Procesando, Enviado, Entregado, Cancelado }

public class Pedido
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; private set; }
    public EstadoPedido Estado { get; private set; }

    // ASOCIACIÓN: referencia a objetos que existen independientemente
    public Cliente Cliente { get; set; }
    public Empleado EmpleadoAsignado { get; set; }

    // COMPOSICIÓN: las líneas pertenecen y nacen con el pedido
    private List<LineaPedido> _lineas = new();
    public IReadOnlyList<LineaPedido> Lineas => _lineas.AsReadOnly();

    public decimal Total => _lineas.Sum(l => l.Subtotal);

    // Evento (lo conectaremos en Paso 3)
    public event EventHandler<Events.PedidoCreadoEventArgs>? PedidoCreado;

    public Pedido(int id, Cliente cliente, Empleado empleado)
    {
        Id = id;
        Cliente = cliente;
        EmpleadoAsignado = empleado;
        FechaCreacion = DateTime.Now;
        Estado = EstadoPedido.Pendiente;
    }

    public bool AgregarLinea(Producto producto, int cantidad)
    {
        if (!producto.ReducirStock(cantidad))
        {
            Console.WriteLine($"⚠️ Stock insuficiente para {producto.Nombre}");
            return false;
        }
        _lineas.Add(new LineaPedido(producto, cantidad));
        return true;
    }

    public void Confirmar()
    {
        Estado = EstadoPedido.Procesando;
        Cliente.AgregarPedido(this);

        PedidoCreado?.Invoke(this, new Events.PedidoCreadoEventArgs(
            Id, Cliente.Nombre, Total, FechaCreacion));
    }

    public void CambiarEstado(EstadoPedido nuevoEstado)
    {
        Estado = nuevoEstado;
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Pedido #{Id} | Cliente: {Cliente.Nombre} | Estado: {Estado}");
        sb.AppendLine($"Fecha: {FechaCreacion:dd/MM/yyyy HH:mm}");
        foreach (var linea in _lineas) sb.AppendLine(linea.ToString());
        sb.AppendLine($"  TOTAL: {Total:C}");
        return sb.ToString();
    }
}