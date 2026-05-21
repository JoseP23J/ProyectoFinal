using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; private set; }
    public string Categoria { get; set; }

    // Evento de stock (lo conectaremos en el Paso 3)
    public event EventHandler<Events.StockActualizadoEventArgs>? StockActualizado;

    public Producto(int id, string nombre, decimal precio, int stock, string categoria)
    {
        Id = id;
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
        Categoria = categoria;
    }

    public bool ReducirStock(int cantidad)
    {
        if (Stock < cantidad) return false;

        int stockAnterior = Stock;
        Stock -= cantidad;

        StockActualizado?.Invoke(this, new Events.StockActualizadoEventArgs(
            Id, Nombre, stockAnterior, Stock));

        return true;
    }

    public void ReponerStock(int cantidad)
    {
        int stockAnterior = Stock;
        Stock += cantidad;

        StockActualizado?.Invoke(this, new Events.StockActualizadoEventArgs(
            Id, Nombre, stockAnterior, Stock));
    }

    public override string ToString() =>
        $"{Nombre} | Precio: {Precio:C} | Stock: {Stock} | Categoría: {Categoria}";
}
