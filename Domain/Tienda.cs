using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

// Clase orquestadora — AGREGACIÓN de Productos, Clientes y Pedidos
public class Tienda
{
    public string Nombre { get; set; }
    private List<Producto> _catalogo = new();
    private List<Cliente> _clientes = new();
    private List<Pedido> _pedidos = new();
    private int _contadorPedidos = 1;

    public IReadOnlyList<Producto> Catalogo => _catalogo.AsReadOnly();
    public IReadOnlyList<Cliente> Clientes => _clientes.AsReadOnly();
    public IReadOnlyList<Pedido> Pedidos => _pedidos.AsReadOnly();

    public Tienda(string nombre) => Nombre = nombre;

    public void AgregarProducto(Producto p) => _catalogo.Add(p);
    public void AgregarCliente(Cliente c) => _clientes.Add(c);

    public Pedido CrearPedido(Cliente cliente, Empleado empleado)
    {
        var pedido = new Pedido(_contadorPedidos++, cliente, empleado);
        _pedidos.Add(pedido);
        return pedido;
    }
}