using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

// AGREGACIÓN: LineaPedido referencia Producto, pero Producto existe independientemente
public class LineaPedido
{
    public Producto Producto { get; set; }   // referencia externa
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }  // precio al momento de la compra

    public decimal Subtotal => Cantidad * PrecioUnitario;

    public LineaPedido(Producto producto, int cantidad)
    {
        Producto = producto;
        Cantidad = cantidad;
        PrecioUnitario = producto.Precio;  // captura precio actual
    }

    public override string ToString() =>
        $"  - {Producto.Nombre} x{Cantidad} @ {PrecioUnitario:C} = {Subtotal:C}";
}